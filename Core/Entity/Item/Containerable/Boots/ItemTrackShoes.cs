using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemTrackShoes : ItemContainerable
{
	static Texture2D Sprite;
	public ItemTrackShoes(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Vojenské trekovky";
		Description = "Tohle asi nebudou přezůvky...";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, 5 }, { StatTypes.MAX_HP, 3 } };
		Effects = new List<EffectTypes>() { };
		Sprite = TextureManager.GetTexture("trackShoes");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemTrackShoes() : this(Vector2.Zero) { }
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
        return new ItemTrackShoes();
    }
}

