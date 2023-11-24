# WinPan
A little Windows tool that pans audio to the horizontal location of the window, as long as there is only one window with audio.

# Why?
I like it, because sometimes I do stuff while watching youtube, and it annoyed me that sound is always coming from the "front". Now I can move the Youtube window to a side, and audio will move with it.

It checks for window positions every 500ms and always updates the position. It detects volume changes that it did not do itself, and tries to respect them.

# How to use
Set up git, and your C# SDK and so on.

Then "git clone" this repository, open the resulting folder your C# IDE of choice (I use VSCode), and compile/run it.

Not much to this. It looks for windows with active audio sessions. If only one is found, it sets the left/right balance of the system audio relative to the horizontal position of that window on your screen(s). Supports multiple screens, but does not take into account pixel density (it assumes that screens and pixel density are roughly corresponding to your screen positions).

# Limitations
Stero only (e.g. only channels 1 and 2 of your default audio interface are affected). The reason this is just active when only one window has an audio session, is that the Windows API does not allow you to control the stereo/multiple channel of a particular program, just its overall volume. So it seems right now there is no chance of panning multiple audio streams around separately. And moving them all to a side seemed - very messy. 

Maybe at some I'll make it configurable, or even build a little UI, but for now this is it.
