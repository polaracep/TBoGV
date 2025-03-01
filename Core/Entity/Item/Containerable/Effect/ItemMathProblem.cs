using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
class ItemMathProblem : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMathProblem(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Složitý matematický problém";
		Description = "Vyžaduje přílišně soustředění, šance ignorace brainrotu.";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 4 } };
		Effects = new List<EffectTypes>() { EffectTypes.DODGE };
		Sprite = TextureManager.GetTexture("mathProblem");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemMathProblem() : this(Vector2.Zero) { }
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
        return new ItemMathProblem();
    }
}

