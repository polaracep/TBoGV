using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
namespace TBoGV;

public abstract class InGameMenu : IDraw
{
    public static Action CloseMenu;
    public static Texture2D SpriteBackground;
    static Viewport Viewport;

    public virtual void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        Viewport = viewport;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpriteBackground,
            new Rectangle(0, 0, Viewport.Width, Viewport.Height),
            new Color(0, 0, 0, (int)(255 * 0.25)));
    }
    protected int prcY(float y)
    {
        return (int)(y * Viewport.Height / 100);
    }
    protected int prcX(float x)
    {
        return (int)(x * Viewport.Width / 100);
    }
}
