using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
class ItemTeeth : ItemContainerable
{
	static Texture2D Sprite;
	public ItemTeeth(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Masochismus";
		Description = "Co tě nezabije to tě posílí. \nBrainrot ti vlezl do mozku a tobě se to líbilo.\nZabíjení nepřátel ti bude navracet zdraví.";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MAX_HP, -8 }, { StatTypes.ATTACK_SPEED, -4 }, { StatTypes.XP_GAIN, -4 }, { StatTypes.PROJECTILE_COUNT, -4 } };
		Effects = new List<EffectTypes>() { EffectTypes.LIFE_STEAL };
		Sprite = TextureManager.GetTexture("maso");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemTeeth() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemTeeth();
    }
}

