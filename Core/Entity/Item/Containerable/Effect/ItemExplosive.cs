using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
class ItemExplosive : ItemContainerable
{
	static Texture2D Sprite;
	public ItemExplosive(Vector2 position)
	{
		Rarity = 4;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Lorentzova transformace";
		Description = "Tvoje střely vybuchují.\nO tomhle nám Schovánek neřekl..";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 5 }, { StatTypes.ATTACK_SPEED, -3 } };
		Effects = new List<EffectTypes>() { EffectTypes.EXPLOSIVE };
		Sprite = TextureManager.GetTexture("lorentzovaTransformace");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemExplosive() : this(Vector2.Zero) { }
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
        return new ItemExplosive();
    }
}


