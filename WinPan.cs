using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wasapi.CoreAudioApi; // This is a wrapper for the Windows Core Audio API


partial class WinPan
{
    private AudioSessionManager audioSessionManager;
    private AudioEndpointVolume volumeControl;

    private float lastVolumeLevel = 0;
    private float lastVolumeLevelSet = 0;
    private int minScreenX;
    private int maxHorizontalPixels;



    public WinPan()
    {
        // Initialize Core Audio API Manager
        // Create an MMDeviceEnumerator
        MMDeviceEnumerator enumerator = new();

        // Get the default audio endpoint
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        Console.WriteLine($"Default Audio Device: {device.FriendlyName}");

        volumeControl = device.AudioEndpointVolume;

        // Get the AudioSessionManager from the default device
        this.audioSessionManager = device.AudioSessionManager;

        // Get the maximum width to be able to calculate relative horizontal position %age
        SetScreenGeometry();
    }

    private void SetScreenGeometry()
    {
        // Get information about all screens
        Screen[] screens = Screen.AllScreens;
        foreach (Screen screen in screens)
        {
            maxHorizontalPixels += screen.Bounds.Width;
            if (screen.Bounds.X < minScreenX) minScreenX = screen.Bounds.X;
        }
    }

    public void UpdateAudioPanning()
    {
        if (volumeControl.MasterVolumeLevelScalar != lastVolumeLevelSet) {
            lastVolumeLevel = volumeControl.MasterVolumeLevelScalar;
        }

        AudioWindowDetector detector = new(audioSessionManager);

        if (detector.DetectAudioWindows() == 1) {
                ApplyPanning(detector.windowPosition + -minScreenX);
        } else {
        }
        lastVolumeLevelSet = volumeControl.MasterVolumeLevelScalar;
    }


    private void ApplyPanning(int position)
    {
        // Calculate the panning value based on the position and maxHorizontalPixels
        double panningValue = new BalanceCalculator().CalculatePanningValue(position, maxHorizontalPixels, minScreenX);

        // Apply the scaled panning value to the audio
        SetSystemPanning(panningValue);
    }

    private void SetSystemPanning(double panningValue)
    {
        // Calculate left and right channel volumes
        double leftChannelVolume = Math.Max(0, Math.Min(lastVolumeLevel, Math.Abs(panningValue - 1) * lastVolumeLevel));
        double rightChannelVolume = Math.Max(0, Math.Min(lastVolumeLevel, Math.Abs(panningValue + 1) * lastVolumeLevel));

        // Check if the endpoint has separate volume controls for left and right channels
        if (volumeControl.Channels.Count >= 2)
        {
            volumeControl.Channels[0].VolumeLevelScalar = (float)leftChannelVolume;
            volumeControl.Channels[1].VolumeLevelScalar = (float)rightChannelVolume;
        }
    }

    internal void ResetBalance()
    {
        Console.WriteLine("Exiting: Resetting to center.");
        SetSystemPanning(0);
    }
}

class Program
{
    static void Main()
    {
        var audioPanner = new WinPan();

        // Add Ctrl-C (interrupt) event handler
        Console.CancelKeyPress += (sender, e) =>
        {
            audioPanner.ResetBalance();
            e.Cancel = false;
        };

        Console.WriteLine("Press Ctrl-C to exit.");
        
        // Main loop to continuously update panning
        while (true)
        {
            audioPanner.UpdateAudioPanning();
            Thread.Sleep(500);
        }
    }
}