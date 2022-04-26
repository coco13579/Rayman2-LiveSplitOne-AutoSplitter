# TUNIC.LiveSplitOne.AutoSplitter

![Preview](/preview.gif)

## Features

- LiveSplit One control with custom splits
- New LiveSplit One layout suggestion for OBS

## OBS Usage

1. Add browser source
    - Set URL https://one.livesplit.org/
    - Set size to full screen (currently only support 1920x1080)
    - Apply provided Custom CSS

2. Interact source ***(OBS browser import/input window might pop behind OBS application)***
    - Right-click/Left-click to toggle menu
    - Import your splits, click save
    - Import provided layout, click save
    - Update WR and PB values in Layout → Edit


3. Edit splits.txt
    - Each line is a split condition, either a zone number or an event
    - Order them according to your splits order
    - Zone numbers and events reference: https://github.com/just-ero/asl/blob/main/TUNIC/TUNIC.asl

5. Start AutoSplitter.exe

6. Interact source
    - Select Compare Against: "Game Time"
    - Connect to server "ws://localhost" (should be in clipboard)

**Repeat 4 and 5 on each usage.**

### How to add the blurry timer background
1. Install OBS plugin StreamFX https://obsproject.com/forum/resources/streamfx-for-obs%C2%AE-studio.578/
2. Make sure your game and browser source have the same size
3. Add "Blur" filter on game source
4. Check "Apply a mask", Mask Type: "Source", Source Mask: "your browser source"
5. Set Mask Alpha Filter to 5, and Size to 30

## Additionnal notes

Exporting splits won't work directly from OBS browser source.
If you want to export splits to .lss you can:
1. Copy content from
`%APPDATA%\obs-studio\plugin_config\obs-browser\IndexedDB\https_one.livesplit.org_0.indexeddb.leveldb`
2. Paste to
`%USERPROFILE%\AppData\Local\Google\Chrome\User Data\Default\IndexedDB\https_one.livesplit.org_0.indexeddb.leveldb`
3. Export splits by browsing https://one.livesplit.org/ on chrome

## Upcoming features

- Custom CSS variables for **Scale** and **Max Visible Splits** 
- More resolution support
