using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;

public abstract class Effect : IDraw, ICloneable
{
	public string Name { get; set; }
	public string Description { get; set; }
	public bool Positive { get; set; }
	public int Level { get; set; }
	public int LevelCap { get; set; }
	protected double effectTime = -1;
	protected double timeElapsed { get; set; }
	public Dictionary<StatTypes, float> Stats = new Dictionary<StatTypes, float>();
	public List<EffectTypes> Effects = new List<EffectTypes>();
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	public Vector2 SpriteSize = new Vector2(50, 50);
	protected float scale { get; set; }

	static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
	static SpriteFont LargerFont = FontManager.GetFont("Arial16");
	static Texture2D SpriteForeground = TextureManager.GetTexture("whiteSquare");
	static protected int Border = 5;
	public abstract void ChangeLevel(int delta);
	public virtual bool EnsureLevelCap()
	{
		//if(Level < 0)
		//	Level = 0;
		if (LevelCap < Level)
		{ 
			Level = LevelCap; 
			return true;
		}
		return false;
	}
	public virtual void UpdateSize()
	{
		string basicText = $"{Name} \n(Lv {Level})";
		Size = LargerFont.MeasureString(basicText) + new Vector2(SpriteSize.X + Border * 3, 0);
		Size = new Vector2(Size.X, Math.Max(SpriteSize.Y + Border, Size.Y + Border));
	}
	public virtual void Draw(SpriteBatch spriteBatch)
	{
		string basicText = $"{Name} \n(Lv {Level})";
		Rectangle effectRect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
		spriteBatch.Draw(SpriteForeground, effectRect, new Color(0, 0, 0, 128));
		spriteBatch.DrawString(LargerFont, basicText, Position + new Vector2(SpriteSize.X + Border * 2, SpriteSize.Y / 2 + Border / 2 - LargerFont.MeasureString(basicText).Y / 2), Positive ? Color.CadetBlue : Color.IndianRed);
	}
	public virtual void IconDraw(SpriteBatch spriteBatch)
	{
		Color borderColor = !Positive ? Color.OrangeRed : Color.CadetBlue;
		int borderThickness = 2; // Thickness of the border

		// Draw border by rendering the sprite slightly offset in 8 directions
		spriteBatch.Draw(SpriteForeground, new Rectangle((int)Position.X - borderThickness, (int)Position.Y - borderThickness, (int)SpriteSize.X + borderThickness, borderThickness), borderColor);
		spriteBatch.Draw(SpriteForeground, new Rectangle((int)Position.X - borderThickness, (int)Position.Y - borderThickness, borderThickness, (int)SpriteSize.Y + borderThickness), borderColor);
		spriteBatch.Draw(SpriteForeground, new Rectangle((int)(Position.X + SpriteSize.X), (int)Position.Y - borderThickness, borderThickness, (int)SpriteSize.Y + borderThickness), borderColor);
		spriteBatch.Draw(SpriteForeground, new Rectangle((int)Position.X - borderThickness, (int)(Position.Y + SpriteSize.Y), (int)SpriteSize.X + 2 * borderThickness, borderThickness), borderColor);
		spriteBatch.Draw(SpriteForeground, new Rectangle((int)Position.X, (int)Position.Y, (int)SpriteSize.X, (int)SpriteSize.Y), new Color(60, 60, 60, 200));
	}
	public virtual Rectangle GetRect()
	{
		return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
	}
	public virtual void UpdateTime(double dt)
	{
		timeElapsed += dt;
	}
	public virtual bool IsExpired()
	{
		if (effectTime < 0)
			return false;
		return timeElapsed > effectTime;
	}
	public abstract Texture2D GetSprite();

	public object Clone()
	{
		return MemberwiseClone();
	}
}

public static class EffectDatabase
{
	private static readonly Dictionary<string, Effect> EffectsByName = new Dictionary<string, Effect>();

	static EffectDatabase()
	{
		var effectsList = new Effect[]
		{
			new EffectCooked(1),
			new EffectDelej(1),
			new EffectFyjalovaDrahota(1),
			new EffectLol(1),
			new EffectRickroll(1),
			new EffectRooted(1),
			new EffectCloseCall(),
			new EffectPrezuvky(),
			new EffectEndless(),
		};

		foreach (var effect in effectsList)
		{
			EffectsByName[effect.Name] = effect;
		}
	}

	public static Effect GetEffectByName(string name)
	{
		return EffectsByName.TryGetValue(name, out var effect) ? (Effect)effect.Clone() : null;
	}
}
