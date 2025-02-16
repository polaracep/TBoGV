using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
internal class ItemTeeth : ItemContainerable
{
	static Texture2D Sprite;
	public ItemTeeth(Vector2 position) 
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Masochismus";
		Description = "Co te nezabije to te posili. \nBrainrot ti vlezl do mozku a tobe se to libilo.\nZabijeni nepratel ti bude navracet zdravi.";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 }, { StatTypes.MAX_HP, -6 }, { StatTypes.ATTACK_SPEED, -4 }, { StatTypes.XP_GAIN, -4 }, { StatTypes.PROJECTILE_COUNT, -2 }, { StatTypes.MOVEMENT_SPEED, -8 } };
		Effects = new List<EffectTypes>() { EffectTypes.LIFE_STEAL};
		Sprite = TextureManager.GetTexture("teeth");
		ItemType = ItemTypes.EFFECT;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}

