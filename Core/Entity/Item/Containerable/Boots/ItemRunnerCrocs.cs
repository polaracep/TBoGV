using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemRunnerCrocs : ItemContainerable
{
	static Texture2D Sprite;
	public ItemRunnerCrocs(Vector2 position)
	{
		Rarity = 4;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Běžecká obuv";
		Description = "! Běžecký mód crocsů !\n" +
					  " Můžeš lézt na lavice";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, 7 } };
		Effects = new List<EffectTypes>() { EffectTypes.BOOTS, EffectTypes.JUMP };
		Sprite = TextureManager.GetTexture("crocsMc");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemRunnerCrocs() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override ItemContainerable Clone()
	{
		return new ItemRunnerCrocs();
	}

}

