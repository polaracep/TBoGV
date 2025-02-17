using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace TBoGV;

internal class Heart : IDraw
{
    public bool Broken;
    static Texture2D SpriteFull;
    static Texture2D SpriteBroken;
    public Vector2 Position;
    public static Vector2 Size;
    public Heart()
    {
        SpriteFull = TextureManager.GetTexture("vitekElegan");
        SpriteBroken = TextureManager.GetTexture("vitekEleganBolderCut");
        Broken = false;
        Size = new Vector2(30, 30);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(!Broken ? SpriteFull : SpriteBroken, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), !Broken ? Color.White : new Color(200,200,200));
    }
}

