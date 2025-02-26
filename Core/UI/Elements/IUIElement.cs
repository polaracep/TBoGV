using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public interface IUIElement
{

    public Vector2 Position { get; set; }

    public abstract void Update(MouseState mouseState);

    public abstract Rectangle GetRect();
}