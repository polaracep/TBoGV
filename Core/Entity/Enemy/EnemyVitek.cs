using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
namespace TBoGV;

internal class EnemyVitek : EnemyRanged
{
	static Texture2D Sprite;
	public EnemyVitek(Vector2 position)
	{	
			Position = position;
		Hp = 3;
		MovementSpeed = 4;
		AttackSpeed = 50;
		AttackDmg = 1;
		Sprite = TextureManager.GetTexture("korenovy_vezen");
		Size = new Vector2(Sprite.Width, Sprite.Height);
		XpValue = 1;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

	public override void Move(Place place)
	{
		return;
	}
}

