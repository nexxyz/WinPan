using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;
using static WinPan;

public class AudioWindowDetector {
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    public int windowPosition = 0;
    private AudioSessionManager audioSessionManager;

    public AudioWindowDetector(AudioSessionManager manager) {
        audioSessionManager = manager;
    }

    public int DetectAudioWindows() {        
        // Get all audio sessions
        SessionCollection sessions = audioSessionManager.Sessions;

        int activeAudioWindows = 0;

        for (int i = 0; i < sessions.Count; i++)
        {
            var session = sessions[i];

            // Get the process associated with the session
            Process process = Process.GetProcessById((int)session.GetProcessID);

            try {
                // Get window position of the process
                windowPosition = GetHorizontalWindowPosition(process.MainWindowHandle);
                activeAudioWindows++;
            } catch(NullReferenceException) {
            }
        }
        return activeAudioWindows;
    }
    
    private int GetHorizontalWindowPosition(IntPtr windowHandle)
    {
        if (GetWindowRect(windowHandle, out RECT rect))
        {
            int horizontalPosition = rect.Left + ((rect.Right - rect.Left) / 2);
            return horizontalPosition;
        }
        throw new NullReferenceException();
    }
}