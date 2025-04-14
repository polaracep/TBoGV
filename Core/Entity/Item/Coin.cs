using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class Coin : Item
{
	static Texture2D Sprite;
	public Coin(Vector2 position)
	{
		Sprite = TextureManager.GetTexture("coin");
		Size = new Vector2(15, 15);
		Position = position - Size / 2;
		if (position != Vector2.Zero)
			InitMovement();
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
	public override void Update(Place place)
	{
		if(!place.player.Inventory.GetEffect().Contains(EffectTypes.MAGNET))
		{
			base.Update(place);
			return;
		}

		if (!IsMoving)
			return;

		Direction = Vector2.Lerp(Direction, place.player.Position-Position, 0.025f);

		float stepSize = 1.0f; // Move 1 pixel at a time to avoid skipping walls
		int steps = (int)Math.Ceiling(MovementSpeed / stepSize); // Total steps needed
		Vector2 stepDirection = Vector2.Normalize(Direction) * stepSize; // Ensure direction is normalized

		for (int i = 0; i < steps; i++)
		{
			Vector2 nextPositionX = new Vector2(Position.X + stepDirection.X, Position.Y);
			Vector2 nextPositionY = new Vector2(Position.X, Position.Y + stepDirection.Y);

			bool collidesX = CollidesWithWall(nextPositionX, place);
			bool collidesY = CollidesWithWall(nextPositionY, place);

			if (collidesX)
			{
				Direction.X *= -1; // Bounce on X-axis
			}
			if (collidesY)
			{
				Direction.Y *= -1; // Bounce on Y-axis
			}
			if (collidesX || collidesY)
			{
				break; // Stop moving if a collision occurs
			}

			Position += stepDirection; // Apply the step movement
		}
		MovementSpeed += -0.2f * Acceleration;
		MovementSpeed = Math.Min(MovementSpeed, 10);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
}
