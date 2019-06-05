# FivemServerManager
This is a tool to easily manage server fivem

## Feature
- Start/Stop Server
- AutoRestart Server and Clear Cache by schedule
- Change SystemInfo
- LOG Console
- Resource Manager (Enable/Disable, Add/Remove Resource)

## ScreenShot
![Screenshot_42](https://user-images.githubusercontent.com/30838114/58843041-0d239780-869b-11e9-8aef-00940ad4bcb8.png)

## Download
to download this tool. you can go to [Release](https://github.com/Oky12/FivemServerManager/releases) and download latest version

**Requirement**
- net. framework 4.0 or above
- start_server.bat (include "run.cmd +exec server.cfg" command)

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
