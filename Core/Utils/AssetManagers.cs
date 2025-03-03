using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

public static class TextureManager
{
    private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    public static void Load(ContentManager content)
    {
        List<string> names = new List<string>
        {
            // Floors
            "tile",
            "floorYellow",
            "floorLobby",
            "stairs",
            "showerClean",
            "showerVomit",
            "showerPiss",
            "showerRust",
            "showerFloor",

            // "floorHallway",

            // Walls
            "wall",
            "wallBrick",
            "wallWhite",
            "wallWhiteCorner",
            "wallLobby",
            "wallLobbyCorner",
            "wallGreen",
            "wallGreenCorner",
            // "wallHallway",
            // "wallHallwayCorner",
            
            // Doors
            "door",
            "doorBoss",

            // Decorations 
            "tableClassroom",
            "chairClassroom",
            "tableComputerClassroom",
            "blackboard",
            "decoInfo1",
            "decoInfo2",
            "decoInfo3",
            "decoInfo4",
            "chairCafeteriaRed",
            "chairCafeteriaGreen",
            "tableCafeteria",
            "decoFridge1",
            "decoFridge2",
            "coffeeMachine",
            "showerSink",

            // Entities
            "korenovy_vezen",
            "sarka",
            "kavesAles",
            "chillAles",
            "kaves",
            "dzojkAles",
            "spritesheetOIIA",
            "milosSpinks",
            "milosSchlafen",
            "milosMood",
            "milosMoodHandless",
            "milosSpritesheet",
            "zdenda",
            "jirka",
            "vibeCatSpritesheet",
            "toiletSpritesheet",
            "triangleSpritesheet",
            "amogusSpritesheet",
            "svartaChill",
            "svartaJump",
            "svartaSpinks",
            "richardSpritesheet",
            "ventSpritesheet",
            "petr",
            "lol",
            "soldier",


            // Vitek
            "vitek-nobg",
            "vitekElegan",
            "vitekEleganBolderCut",

            // Projectiles
            "projectile",
            "koren",
            "jatrovyKnedlicek",
            "jirkovaNokie",
            "note",
            "boolet",

            // UI
            "taunt",
            "admiration",
            "blackSquare",
            "whiteSquare",
            "containerBorder",
            "containerWeapon",
            "containerEffect",
            "containerBoots",
            "containerBasic",
            "gymvodMap",
            "cooked",
            "wheat4",
            "arrow",
            "jirkaEffect",
            "check",
            "cross",

            // Items
            "heal",
            "coin",
            "adBlock",
            "calculator",
            "fancyShoes",
            "crocs",
            "gumaky",
            "lorentzovaTransformace",
            "mathProblem",
            "pencil",
            "trackShoes",
            "dagger",
            "maso",
            "teeth",
            "bookWeapon",
            "bookBio",
            "bookCj",
            "bookPetakova",
            "bookZsv",
            "bookTabulky",
            "bookPE",
            "bryle",
            "kriz",
            "monster",
            "pen",
            "scissors",
            "fialovaFixa",
            "labCoat",

            // Misc
            "gymvod",
            "platina",
            "boom",
            "gold",
            "exit",
            "tbogv",
            "richard",
            "vyzo",
        };

        foreach (string name in names)
        {
            textures.Add(name, content.Load<Texture2D>("Textures/" + name));
        }
    }
    public static Texture2D GetTexture(string name)
    {
        return textures.GetValueOrDefault(name);
    }
}

public static class SongManager
{
    private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

    public static void Load(ContentManager content)
    {
        List<string> names = new List<string>
        {
            "soundtrack",
        };

        foreach (string name in names)
        {
            songs.Add(name, content.Load<Song>("Sounds/" + name));
        }
    }
    public static Song GetSong(string name)
    {
        return songs.GetValueOrDefault(name);
    }
}
public static class SoundManager
{
    private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

    public static void Load(ContentManager content)
    {
        List<string> names = new List<string>
        {
            "bouchaniDoKorenu",
            "dzojkShorter",
            "OIIAOIIA",
            "aligator",
            "kaves",
            "vibe",
            "toilet",
            "AMOGUS",
            "takToJsemNevidel",
            "svartaDelej",
            "rickroll",
            "neverGonnaGiveUUp",
        };

        foreach (string name in names)
        {
            soundEffects.Add(name, content.Load<SoundEffect>("Sounds/" + name));
        }
    }

    public static SoundEffect GetSound(string name)
    {
        return soundEffects.GetValueOrDefault(name);
    }
}

public static class FontManager
{
    private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

    public static void Load(ContentManager content)
    {
        List<string> names = new List<string>
        {
            "font",
            "Arial8",
            "Arial12",
            "Arial16",
            "Arial24",
        };

        foreach (string name in names)
        {
            fonts.Add(name, content.Load<SpriteFont>("Fonts/" + name));
        }
    }
    public static SpriteFont GetFont(string name)
    {
        return fonts.GetValueOrDefault(name);
    }
}
