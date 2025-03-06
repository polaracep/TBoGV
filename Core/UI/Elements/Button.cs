using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;


public class Button : UIElement, IDraw 
{
	protected string Text;
	protected Vector2 Size;
	protected SpriteFont Font;
	protected static Texture2D SpriteBackground = TextureManager.GetTexture("whiteSquare");
	protected int BorderOffset = 5;
	protected Color TextColor = Color.White;
	public Action OnClick { get; private set; }
	protected MouseState PrevMouseState;
	protected Color[] color = [new Color(40,40,40)* 0.5f, Color.Black * 0.5f];
	protected int ColorIndex = 0;

	public Button(string text, SpriteFont font, Action onClick)
	{
		Font = font;
		Size = font.MeasureString(text);
		Text = text;
		OnClick = onClick;
	}

	public override void Update(MouseState mouseState)
	{
		if (mouseState.LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released
			&& GetRect().Contains(mouseState.Position))
			OnClick();
		if (GetRect().Contains(mouseState.Position))
			ColorIndex = 1;
		else
			ColorIndex = 0;
		PrevMouseState = mouseState;
	}

	public virtual void SetSize(Vector2 size)
	{
		Size = size;
	}
	public void SetTextColor(Color color)
	{
		TextColor = color;
	}
	public override Rectangle GetRect()
	{
		return new Rectangle(
			(int)Position.X - BorderOffset, (int)Position.Y - BorderOffset,
			(int)Size.X + 2 * BorderOffset, (int)Size.Y + 2 * BorderOffset);
	}

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(SpriteBackground, GetRect(), color[ColorIndex]);
		spriteBatch.DrawString(Font, Text, new Vector2(Position.X, Position.Y), TextColor);
	}
}

