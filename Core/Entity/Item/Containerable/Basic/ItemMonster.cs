using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
internal class ItemMonster : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMonster(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Monster";
		Description = "Asi bys to nemel prehanet";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, -6 }, { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 1 }, { StatTypes.XP_GAIN, 1 }, { StatTypes.PROJECTILE_COUNT, 1 } };
		Effects = new List<EffectTypes>() { };
		Sprite = TextureManager.GetTexture("tile");
		ItemType = ItemTypes.BASIC;
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

