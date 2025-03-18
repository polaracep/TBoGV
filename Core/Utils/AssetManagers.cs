using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TBoGV;
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
            "showerWater",

            // "floorHallway",

            // Walls
            "wall",
            "wallBrick",
            "wallWhite",
            "wallWhiteCorner",
            "wallOrange",
            "wallOrangeCorner",
            "wallShower",
            "wallShowerCorner",
            "wallGreen",
            "wallGreenCorner",
            "wallLightGreen",
            "wallLightGreenCorner",
            "wallBlue",
            "wallBlueCorner",
            
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
            "lockerOpen",
            "lockerClosed",
            "bench",

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
            "perloun",
            "cameraman",
            "gambler",
            "skolnik",

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
            "skull",
            "check",
            "cross",
            "reroll",
            "bossIcon",
            "showerIcon",
            "lockerIcon",
            "iconExit",
            "iconNotCleared",

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
            "pravyuhel",
            "sesitZeleny",
            "sesitCerveny",
            "sesitModry",
            "kruzitko",
            "sharpener",

            // Misc
            "gymvod",
            "platina",
            "boom",
            "gold",
            "exit",
            "tbogv",
            "richard",
            "vyzo",
            "wiseman",
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
            "shower",
            "triangle",
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

public static class DialogueManager
{
    private static Dictionary<string, JsonDocument> dialogues = new Dictionary<string, JsonDocument>();

    public static void Load(ContentManager content)
    {
        List<string> names = new List<string>
        {
            "intro",
            "tutorial1",
            "tutorial2",
            "tutorial3",
            "tutorial4",
            "skolnik",
            "gambler",
            "gamblerPlaced",
            "gamblerWait",
            "gamblerResult",
            "gamblerDone",
            "machineGuide",
        };

        foreach (string name in names)
        {
            dialogues.Add(name, content.Load<JsonDocument>("Dialogues/" + name));
        }
    }
    public static JsonDocument GetDialogue(string name)
    {
        return dialogues.GetValueOrDefault(name);
    }
}
// Runtime reader for JsonDocument.
public class JsonDocumentReader : ContentTypeReader<JsonDocument>
{
    protected override JsonDocument Read(ContentReader input, JsonDocument existingInstance)
    {
        // Read the raw JSON string written by the writer.
        string json = input.ReadString();
        // Parse the string into a JsonDocument.
        return JsonDocument.Parse(json);
    }
}