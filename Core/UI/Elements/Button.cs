using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class Button : IDraw, IUIElement
{
	public Vector2 Position { get; set; }
	private string Text;
	private Vector2 Size;
	private SpriteFont Font;
	private static Texture2D SpriteBackground = TextureManager.GetTexture("whiteSquare");
	private int BorderOffset = 5;

	public Action OnClick { get; private set; }
	protected MouseState PrevMouseState;
	protected Color[] color = [new(60, 60, 60, 128), new(20, 20, 20, 128)];
	protected int ColorIndex = 0;

	public Button(string text, SpriteFont font, Action onClick)
	{
		Font = font;
		Size = font.MeasureString(text);
		Text = text;
		OnClick = onClick;
	}
	public void Update(MouseState mouseState)
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
	public Rectangle GetRect()
	{
		return new Rectangle(
			(int)Position.X - BorderOffset, (int)Position.Y - BorderOffset,
			(int)Size.X + 2 * BorderOffset, (int)Size.Y + 2 * BorderOffset);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(SpriteBackground, GetRect(), color[ColorIndex]);
		spriteBatch.DrawString(Font, Text, new Vector2(Position.X, Position.Y), Color.White);
	}
}

