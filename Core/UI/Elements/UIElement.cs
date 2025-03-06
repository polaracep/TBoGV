using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public abstract class UIElement
{
    public Vector2 Position;

    public abstract void Update(MouseState mouseState);

    public abstract Rectangle GetRect();
}