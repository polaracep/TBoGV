using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

public abstract class Effect : IDraw
{	
	public string Name { get; set; }
	public string Description { get; set; }
	public bool Positive { get; set; }
	public int Level { get; set; }
	public Dictionary<StatTypes, int> Stats { get; set; }
	public List<EffectTypes> Effects { get; set; }
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
	static SpriteFont LargerFont = FontManager.GetFont("Arial16");
	static Texture2D SpriteForeground = TextureManager.GetTexture("whiteSquare");
	static protected int Border = 10;
	public abstract void ChangeLevel(int delta);
	public virtual void UpdateSize()
	{
		string basicText = $"{Name} \n(Lv {Level})";
		Size = LargerFont.MeasureString(basicText) + new Vector2(GetSprite().Width+Border*3, 0);
		Size = new Vector2(Size.X,Math.Max(GetSprite().Height + Border,Size.Y));
	}
	public virtual void Draw(SpriteBatch spriteBatch)
	{
		string basicText = $"{Name} \n(Lv {Level})";
		Rectangle effectRect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
		spriteBatch.Draw(SpriteForeground, effectRect, new Color(0,0,0,128));
		spriteBatch.DrawString(LargerFont, basicText, Position + new Vector2(GetSprite().Width + Border*2, GetSprite().Height/2 + Border/2 - LargerFont.MeasureString(basicText).Y / 2), Positive ? Color.CadetBlue : Color.IndianRed);
	}
	public virtual Rectangle GetRect()
	{
		return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
	}
	public abstract Texture2D GetSprite();
}
