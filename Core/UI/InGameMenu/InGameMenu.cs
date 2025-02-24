using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace TBoGV;

public abstract class InGameMenu : IDraw
{
    public static Texture2D SpriteBackground;
    static Viewport Viewport;
    public bool Active;
    public virtual void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, GameTime gameTime)
    {
        Viewport = viewport;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpriteBackground,
            new Rectangle(0, 0, Viewport.Width, Viewport.Height),
            new Color(0, 0, 0, (int)(255 * 0.25)));
    }

}
