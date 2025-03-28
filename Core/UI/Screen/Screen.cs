using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public abstract class Screen
{
    public Screen(GraphicsDeviceManager graphics) { }
    public abstract void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics);
    public abstract void Update(GameTime gameTime, GraphicsDeviceManager graphics);
}

