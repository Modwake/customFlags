Custom mod that lets you load your own flag for your own ship.

## Initial Setup

### MANUAL

The steamID(s) are to be placed in `Managed/Mods/Assets/FlagReplacement/` named `steamID.txt` (You may have to create this folder, depending on if you did the automatic start or not). **NOTE: YOU MUST USE STEAMID64**

Should look like this when you are done:

- `Mods/Assets/FlagReplacement/customFlag1.png`
- `Mods/Assets/FlagReplacement/steamID.txt`

From there, simply place the desired flag(s) inside `/FlagReplacement/`. For example:

- portFlag.png
- AUS.png
- Pineapple.png

Inside your `steamID.txt` file you would simply place the following below:

```TEXT
1stSTEAMID64=portFlag.png
MySTEAMID64=Pineapple.png
123456789123=AUS.png
```

Now boot up the game and have fun!

### AUTOMATIC

Run the game with the dll inside your `Mods` folder.

It will auto-generate the file(s) and folder(s) that are required.

From there, after you have closed the game, simply place the desired flag inside `/FlagReplacement/`. For example:

- portFlag.png
- AUS.png
- Pineapple.png

Inside your `steamID.txt` file you would simply place the following below:

```TEXT
1stSTEAMID64=portFlag.png
MySTEAMID64=Pineapple.png
123456789123=AUS.png
```

Now boot up the game and have fun!

## Example of a custom flag applied to a hoy and galleon

![example NL](/Images/NL_Flag.png)