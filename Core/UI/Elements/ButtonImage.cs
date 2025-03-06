﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class ButtonImage : Button
{
	Texture2D Sprite;
	ImageOrientation ImageOrientation;
	float ImageScale;
	public ButtonImage(string text, SpriteFont font, Action onClick, Texture2D sprite, ImageOrientation imageOrientation) : base(text, font, onClick)
	{
		Sprite = sprite;
		ImageOrientation = imageOrientation;
		switch (ImageOrientation)
		{
			case ImageOrientation.TOP:
			case ImageOrientation.BOTTOM:
				ImageScale = 1;
				Size = font.MeasureString(text) + new Vector2(0, Sprite.Height);
				break;
			case ImageOrientation.LEFT:
			case ImageOrientation.RIGHT:
				ImageScale = font.MeasureString(text).Y / Sprite.Height;
				Size = font.MeasureString(text) + new Vector2(Sprite.Width * ImageScale, 0);
				break;
			default:
				break;
		}
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		Rectangle rect = GetRect();
		spriteBatch.Draw(SpriteBackground, rect, color[ColorIndex]);

		Vector2 textSize = Font.MeasureString(Text);
		Vector2 imagePos = Vector2.Zero;
		Vector2 textPos = Vector2.Zero;

		switch (ImageOrientation)
		{
			case ImageOrientation.TOP:
				imagePos = new Vector2(rect.X + (rect.Width - Sprite.Width) / 2, rect.Y + BorderOffset);
				textPos = new Vector2(rect.X + (rect.Width - textSize.X) / 2, imagePos.Y + Sprite.Height + BorderOffset);
				break;

			case ImageOrientation.BOTTOM:
				textPos = new Vector2(rect.X + (rect.Width - textSize.X) / 2, rect.Y + BorderOffset);
				imagePos = new Vector2(rect.X + (rect.Width - Sprite.Width) / 2, textPos.Y + textSize.Y + BorderOffset);
				break;

			case ImageOrientation.LEFT:
				imagePos = new Vector2(rect.X + BorderOffset, rect.Y + (rect.Height - Sprite.Height * ImageScale) / 2);
				textPos = new Vector2(imagePos.X + Sprite.Width * ImageScale + BorderOffset, rect.Y + (rect.Height - textSize.Y) / 2);
				break;

			case ImageOrientation.RIGHT:
				textPos = new Vector2(rect.X + BorderOffset, rect.Y + (rect.Height - textSize.Y) / 2);
				imagePos = new Vector2(textPos.X + textSize.X + BorderOffset, rect.Y + (rect.Height - Sprite.Height * ImageScale) / 2);
				break;
		}

		spriteBatch.Draw(Sprite, imagePos, null, Color.White, 0, Vector2.Zero, ImageScale, SpriteEffects.None, 0);
		spriteBatch.DrawString(Font, Text, textPos, Color.White);
	}
}

public enum ImageOrientation : int
{
	TOP, BOTTOM, LEFT, RIGHT
}