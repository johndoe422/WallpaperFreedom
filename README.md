# WallpaperFreedom
A project aimed at helping non-admin users change and cycle wallpapers in Win10/11 where corporate policies restrict wallpaper change.

## How to use 
Download the release, and extract it into any folder. Edit the config file to indictae the folder where you keep your wallpaper files. Save the config file. Double click on the executable. You may also change the CycleTime value to set the time interval at which the application should switch to the next wallpaper available in that folder. If there's only one wallpaper file in the folder, this setting is not useful.

## Logs
The executable produces a log file in the same folder, you may check this log file to see if it is working properly.

## Terminating the application
The application is head-less. So you will have to kill it from task manager.

## Extra
It is a good idea to create a scheduled task to run this application at user login event.
