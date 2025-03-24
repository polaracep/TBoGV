using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemAtlas : ItemContainerable
{
	static Texture2D Sprite;
	private static List<StatTypes> allStats = new List<StatTypes>()
	{
		StatTypes.DAMAGE,
		StatTypes.MOVEMENT_SPEED,
		StatTypes.MAX_HP,
		StatTypes.XP_GAIN,
		StatTypes.PROJECTILE_COUNT,
		StatTypes.ATTACK_SPEED
	};
	public ItemAtlas(Vector2 position)
	{
		Rarity = 4;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Atlas světa";
		Description = "Je v něm úplně všechno! \n(minimapu otevřeš M)\n";
		Stats = new Dictionary<StatTypes, float>() { };
		foreach (StatTypes type in allStats)
			Stats[type] = 2;
		Effects = new List<EffectTypes>() { EffectTypes.MAP_REVEAL };
		Sprite = TextureManager.GetTexture("atlas");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemAtlas() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override ItemContainerable Clone()
	{
		return new ItemAtlas();
	}
}


