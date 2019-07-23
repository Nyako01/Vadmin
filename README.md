# Vadmin
This is a tool to easily manage server fivem

## Feature
* Start/Stop Server
* AutoRestart Server by schedule
* Change SystemInfo
* Console Command
* Resource Manager (Enable/Disable/Rename, Install/Uninstall, Start/Stop/Restart Resource)
* Restart Button (Manual Restart)
* AUTO Restart if Server Crash or Force Close
* Disable Close Program if Server Currently Running
* Start Server Without start_server.bat (batch file)
* PlayerList (include Kick Player)
* Check For Update (Progam and Server)
* OneSync Support
* Kick All Player Button
* Option to Clear Cache When Restart in Setting menu
* Clear Cache Button (manual Clear Cache)
* Custom Server Config
* Server Mode (Public Server and Test Server)
* Theme Option (Dark Theme available)
* Execute Command Before Auto Restart
* Kick All Player When Auto Restart
* Tooltip(information if you pointing something)
* Information About Version of Program and Server in Setting
* Auto Check Version Program and Server every 5 Second
## ScreenShot
![Screenshot_67](https://user-images.githubusercontent.com/30838114/61728034-93f11880-ad9e-11e9-9578-388047d757ea.png)

## Download
to download this tool. you can go to [Release](https://github.com/Oky12/FivemServerManager/releases) and download latest version

**Requirement**
- net. framework 4.5 or above


**Installation**
1. extract to Server-Data (Config.cfg and the program)
2. add resource tag(#System,  #InESXFolder, #OutESXFolder, #Addons) on server.cfg

Example
```
#System
start mapmanager
start chat
start spawnmanager
start sessionmanager
start fivem
start hardcap
start rconlog
#start scoreboard
#start playernames
start bob74_ipl
start mysql-async
start essentialmode
start esplugin_mysql
start es_admin2
#endSystem

#InESXFolder
start async
start cron
start es_extended
start instance
start es_camera
start skinchanger
start esx_accessories
start esx_addonaccount
#start esx_addoninventory
#start esx_phone
start esx_borrmaskin
#endInESXFolder

#OutESXFolder
#start CalmAI
#start CarRental
start 3dme
start hospital
start BlipsBuilder
start HangOntoAVehicle
start es_ui
start vMenu
#endOutESXFolder

#Addons
start taxi_toyota
start prison-map-addon
start Amarok
start audi
start bmw1000
start bmw_m5
start bmwf8
start bmwi8
start bmwm3
start bmwm4gts
start caferacerducati
start camry2016
#endAddons
```
3. open/run Fivem Server Manager
4. Set Location Fivem Server
