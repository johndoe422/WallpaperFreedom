# WallpaperFreedom
A project aimed at helping non-admin users change and cycle wallpapers in Win10/11 systems where corporate policies restrict wallpaper change. This will work for non-admin users and this application doesn't tamper with the system registry or any policy settings. The wallpaper change may not persist after a reboot, but you can run this applciation again to change the wallpaper.

## How to use 
Download the release, and extract it into any folder. Edit the config file to indictae the folder where you keep your wallpaper files. Save the config file. Double click on the executable. You may also change the CycleTime value to set the time interval at which the application should switch to the next wallpaper available in that folder. If there's only one wallpaper file in the folder, this setting is not useful.

## Supported Wallpaper file types
The app supports jpg, png and bmp file extenstions. A mix of these file types can be placed in the configured folder. On every start of the application, the file list is randomized for cycling.

## Logs
The executable produces a log file in the same folder, you may check this log file to see if it is working properly.

## Terminating the application
The application is head-less. So you will have to kill it from task manager.

## Extra
It is a good idea to create a scheduled task to run this application at user login event.
