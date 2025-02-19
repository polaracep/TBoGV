using System.Collections.Generic;
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
            "tile",
            "floorYellow",
            "floorLobby",
            "wall",
            "wallBrick",
            "wallWhite",
            "wallWhiteCorner",
            "wallLobby",
            "wallLobbyCorner",
            "door",
            "vitek-nobg",
            "vitekElegan",
            "projectile",
            "taunt",
            "admiration",
            "heal",
            "blackSquare",
            "koren",
            "korenovy_vezen",
            "gymvod",
            "platina",
            "coin",
            "whiteSquare",
            "containerBorder",
            "containerWeapon",
            "containerEffect",
            "containerBoots",
            "containerBasic",
            "wheat4",
            "teeth",
            "gymvodMap",
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
            "boom",
            "vitekEleganBolderCut",
            "cooked"

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
