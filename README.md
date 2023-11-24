# WinPan
A little Windows tool that pans audio to the horizontal location of the window, as long as there is only one window with audio.

# Why?
I like it, because sometimes I do stuff while watching youtube, and it annoyed me that sound is always coming from the "front". Now I can move the Youtube window to a side, and audio will move with it.

Mainly, I wanted a solution to this - and I wanted to test coding with C# with ChatGPT 4 as my copilot. I'm not a C# person, but I made do. So here it is.

It checks for window positions every 500ms and always updates the position. It detects volume changes that it did not do itself, and tries to respect them.

# How to use
Download the installer from the releases, install, then run WinPan whenever you like to move around your sound-playing-window. Terminate it with CTRL-C, and upon exiting it will restore center balance. That's it - not much more to it.

Not much to this. It looks for windows with active audio sessions. If only one is found, it sets the left/right balance of the system audio relative to the horizontal position of that window on your screen(s). Supports multiple screens, but does not take into account pixel density (it assumes that screens and pixel density are roughly corresponding to your screen positions).

# Limitations
Stero only (e.g. only channels 1 and 2 of your default audio interface are affected). The reason this is just active when only one window has an audio session, is that the Windows API does not allow you to control the stereo/multiple channel of a particular program, just its overall volume. So it seems right now there is no chance of panning multiple audio streams around separately. And moving them all to a side seemed - very messy. 

Maybe at some I'll make it configurable, or even build a little UI, but for now this is it.
