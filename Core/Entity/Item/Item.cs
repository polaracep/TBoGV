
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public abstract class Item : Entity, IInteractable
{
	protected bool IsMoving = false;
	protected float Acceleration = -0.1f;
	public virtual void Interact(Entity e, Place p)
    {
        p.player.Coins++;
    }
	public virtual void InitMovement()
	{
		Random random = new Random();
		MovementSpeed = random.Next(1,6);
		IsMoving = true;
		Direction = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));

	}
	public virtual void Update(Place place)
	{
		if (!IsMoving)
			return;

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

		MovementSpeed += Acceleration;
		if (MovementSpeed <= 0)
			IsMoving = false;
	}
	private bool CollidesWithWall(Vector2 testPosition, Place place)
	{
		return place.ShouldCollideAt(testPosition+Size/2);
	}
}

