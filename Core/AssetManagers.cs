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
            // "floorHallway",

            // Walls
            "wall",
            "wallBrick",
            "wallWhite",
            "wallWhiteCorner",
            "wallLobby",
            "wallLobbyCorner",
            // "wallHallway",
            // "wallHallwayCorner",
            
            // Doors
            "door",

            // Decorations 
            "lavice",
            "zidle",
            "katedra",

            // Entities
            "korenovy_vezen",
            "sarka",

            // Vitek
            "vitek-nobg",
            "vitekElegan",
            "vitekEleganBolderCut",

            // Projectiles
            "projectile",
            "koren",

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

            // Misc
            "gymvod",
            "platina",
            "boom",
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
