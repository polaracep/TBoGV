using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
class ItemAdBlock : ItemContainerable
{
	static Texture2D Sprite;
	public ItemAdBlock(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Ad Block";
		Description = "Teď už tě nic nezastaví. \nTvoje střely jsou průrazné";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.ATTACK_SPEED, -4 } };
		Effects = new List<EffectTypes>() { EffectTypes.PIERCING };
		Sprite = TextureManager.GetTexture("adBlock");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemAdBlock() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemAdBlock();
    }
}


