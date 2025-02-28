using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class ObjectCollision
{
	// 📌 Rectangle vs. Rectangle (Entity)
	public static bool RectRectCollision(Entity entity1, Entity entity2)
	{
		return RectRectCollision(entity1.GetRectangle(), entity2.GetRectangle());
	}

	// 📌 Rectangle vs. Rectangle (Rectangle)
	public static bool RectRectCollision(Rectangle rect1, Rectangle rect2)
	{
		return rect1.Intersects(rect2);
	}

	// 📌 Circle vs. Circle (Entity)
	public static bool CircleCircleCollision(Entity entity1, Entity entity2)
	{
		return CircleCircleCollision(entity1.GetCircleCenter(), entity1.GetRadius(),
										entity2.GetCircleCenter(), entity2.GetRadius());
	}

	// 📌 Circle vs. Circle (Vector2, float)
	public static bool CircleCircleCollision(Vector2 center1, float radius1, Vector2 center2, float radius2)
	{
		float distanceSquared = Vector2.DistanceSquared(center1, center2);
		float radiusSum = radius1 + radius2;
		return distanceSquared <= (radiusSum * radiusSum);
	}

	// 📌 Rectangle vs. Circle (Entity, Entity)
	public static bool RectCircleCollision(Entity rectEntity, Entity circleEntity)
	{
		return RectCircleCollision(rectEntity.GetRectangle(), circleEntity.GetCircleCenter(), circleEntity.GetRadius());
	}

	// 📌 Rectangle vs. Circle (Rectangle, Entity)
	public static bool RectCircleCollision(Rectangle rect, Entity circleEntity)
	{
		return RectCircleCollision(rect, circleEntity.GetCircleCenter(), circleEntity.GetRadius());
	}

	// 📌 Rectangle vs. Circle (Rectangle, Vector2, float)
	public static bool RectCircleCollision(Rectangle rect, Vector2 circleCenter, float radius)
	{
		float closestX = Math.Clamp(circleCenter.X, rect.Left, rect.Right);
		float closestY = Math.Clamp(circleCenter.Y, rect.Top, rect.Bottom);

		float distanceX = circleCenter.X - closestX;
		float distanceY = circleCenter.Y - closestY;
		float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

		return distanceSquared <= (radius * radius);
	}

	// 📌 Point inside Rectangle (Entity)
	public static bool PointInRect(Vector2 point, Entity entity)
	{
		return PointInRect(point, entity.GetRectangle());
	}

	// 📌 Point inside Rectangle (Rectangle)
	public static bool PointInRect(Vector2 point, Rectangle rect)
	{
		return rect.Contains(point);
	}

	// 📌 Point inside Circle (Entity)
	public static bool PointInCircle(Vector2 point, Entity entity)
	{
		return PointInCircle(point, entity.GetCircleCenter(), entity.GetRadius());
	}

	// 📌 Point inside Circle (Vector2, float)
	public static bool PointInCircle(Vector2 point, Vector2 circleCenter, float radius)
	{
		float distanceSquared = Vector2.DistanceSquared(point, circleCenter);
		return distanceSquared <= (radius * radius);
	}
}

