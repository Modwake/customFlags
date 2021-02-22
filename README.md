Custom mod that lets you load your own flag for your own ship. Required files are listed inside the release

## File setup

See notes on the latest release.

## Initial Setup

### MANUAL

The steamID(s) are to be placed in `Managed/Mods/Configs/` named `customFlags.txt` (You may have to create this folder, depending on if you did the automatic start or not). **NOTE: YOU MUST USE STEAMID64**

Should look like this when you are done:

- `Mods/Assets/FlagReplacement/customFlag1.png`
- `Mods/Configs/customFlags.cfg`

From there, simply place the desired flag(s) inside `Mods/Assets/FlagReplacement/`. For example:

- portFlag.png
- AUS.png
- Pineapple.png

Inside your `customFlags.cfg` file you would simply place any steamID's and flags using the following template below:

```TEXT
STEAMID64=portFlag
STEAMID64=Pineapple
123456789123=AUS
```

Now boot up the game and have fun!

### AUTOMATIC

Run the game with the dll inside your `Mods` folder.

It will auto-generate the file(s) and folder(s) that are required.

From there, after you have closed the game, simply place the desired flag inside `/FlagReplacement/`. For example:

- portFlag.png
- AUS.png
- Pineapple.png

Inside your `customFlags.cfg` file you would simply place any steamID's and flags using the following template below:

```TEXT
STEAMID64=portFlag
STEAMID64=Pineapple
123456789123=AUS
```

Now boot up the game and have fun!

## Example of a custom flag applied to a hoy and galleon

![example NL](/Images/NL_Flag.png)
