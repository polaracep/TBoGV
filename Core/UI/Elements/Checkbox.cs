using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBoGV;
class Checkbox : UIElement, IDraw
{
    private string Label;
    private SpriteFont Font;
    private static Texture2D BoxTexture = TextureManager.GetTexture("whiteSquare");
    private static Texture2D SpriteCheck = TextureManager.GetTexture("check");
    private static Texture2D SpriteCross = TextureManager.GetTexture("cross");
    private int BorderOffset = 3;
    private bool IsChecked;
    private MouseState PrevMouseState;

    private Color UncheckedColor = new Color(100, 0, 0, 128);
    private Color CheckedColor = new Color(0, 100, 0, 128);

    public Action<bool> OnToggle;

    public Checkbox(string label, Vector2 position, bool initialState, Action<bool> onToggle)
    {
        Label = label;
        Font = FontManager.GetFont("Arial16");
        Position = position;
        IsChecked = initialState;
        OnToggle = onToggle;
    }

    public override Rectangle GetRect()
    {
        return Rectangle.Union(GetBoxRect(), GetLabelRect());
    }

    /// <summary>
    /// Returns the rectangle for the checkbox square.
    /// </summary>
    /// <returns></returns>
    public Rectangle GetBoxRect()
    {
        // Use the font's line spacing as the base size.
        int size = (int)Font.LineSpacing;
        return new Rectangle((int)Position.X - BorderOffset, (int)Position.Y - BorderOffset, size + 2 * BorderOffset, size + 2 * BorderOffset);
    }

    /// <summary>
    /// Returns the rectangle for the label text.
    /// </summary>
    /// <returns></returns>
    public Rectangle GetLabelRect()
    {
        int spacing = 10; // Space between checkbox and label.
        Vector2 labelSize = Font.MeasureString(Label);
        return new Rectangle((int)(Position.X + Font.LineSpacing + spacing), (int)Position.Y, (int)labelSize.X, (int)labelSize.Y);
    }

    public override void Update(MouseState mouseState)
    {
        Rectangle boxRect = GetBoxRect();
        // Also consider clicks on the label.
        if (mouseState.LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
        {
            if (boxRect.Contains(mouseState.Position) || GetLabelRect().Contains(mouseState.Position))
            {
                IsChecked = !IsChecked;
                OnToggle(IsChecked);
            }
        }
        PrevMouseState = mouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle boxRect = GetBoxRect();
        // Draw the checkbox background.
        Color boxColor = IsChecked ? CheckedColor : UncheckedColor;
        spriteBatch.Draw(BoxTexture, boxRect, boxColor);

        // Draw the checkmark texture centered within the box.
        Rectangle checkmarkRect = new Rectangle(
            boxRect.X + BorderOffset,
            boxRect.Y + BorderOffset,
            boxRect.Width - 2 * BorderOffset,
            boxRect.Height - 2 * BorderOffset
        );
        spriteBatch.Draw(IsChecked ? SpriteCheck : SpriteCross, checkmarkRect, Color.White);

        // Draw the label next to the checkbox.
        spriteBatch.DrawString(Font, Label, new Vector2(Position.X + Font.LineSpacing + 10, Position.Y), Color.White);
    }
}