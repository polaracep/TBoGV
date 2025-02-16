using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public abstract class Entity
{
	public Vector2 Position;
	public Vector2 Direction;
	public Vector2 Size;
	public int MovementSpeed;
	public Rectangle GetRectangle()
	{
		return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
	}
	public Vector2 GetCircleCenter()
	{
		return Position + Size / 2;
	}
	public float GetRadius()
	{
		return Math.Min(Size.X, Size.Y) / 2;
	}
}
