using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TBoGV;

public abstract class EnemyMelee : Enemy
{
	protected double lastPathUpdateElapsed = 0;
	protected Vector2 PlayerPosition;
	protected Vector2 targetPosition;
	protected float pathUpdateCooldown = 0.1f;
	protected Queue<Vector2> path = new Queue<Vector2>();

	protected bool Moving;
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int movingDuration = 2500;

	public override void Update(Vector2 playerPosition, double dt)
	{
		PlayerPosition = playerPosition;
		phaseChangeElapsed += dt;
		lastPathUpdateElapsed += dt;
		UpdateMoving(dt);
	}
	protected virtual void UpdateMoving(double dt)
	{
		if ((phaseChangeElapsed > movingDuration && Moving) ||
			(phaseChangeElapsed > chillDuration && !Moving))
		{
			Moving = !Moving;
			phaseChangeElapsed = 0;
		}
	}
	public override bool ReadyToAttack()
	{
		return Moving;
	}
	public override bool IsDead()
	{
		return Hp <= 0;
	}
	public override List<Item> Drop(int looting)
	{
		int rnd = new Random().Next(0, 100);
		if (50 / looting > rnd)
			return new List<Item>();
		else return new List<Item>() { new Coin(Position + Size / 2) };
	}
	private void FollowPath()
	{
		if (path.Count > 0)
		{
			float traveledDist = 0;
			Vector2 prevPos = Position;

			while (traveledDist < MovementSpeed && path.Count > 0)
			{
				Vector2 nextStep = path.Peek();
				Vector2 direction = nextStep - Position;
				float distanceToNextStep = direction.Length();

				if (distanceToNextStep > 0)
				{
					direction.Normalize();

					// Calculate move distance for this frame
					float moveDistance = Math.Min(MovementSpeed - traveledDist, distanceToNextStep); // Move until we reach MovementSpeed

					// Move the position
					Position += direction * moveDistance;

					// Track the distance traveled in this frame
					traveledDist += moveDistance;

					// If we've reached or passed the next step, dequeue it
					if (distanceToNextStep <= moveDistance)
					{
						Position = nextStep;
						path.Dequeue();
					}
				}

				// If we have traveled the movement speed distance, exit the loop
				if (traveledDist >= MovementSpeed)
					break;
			}
		}
	}
	public override void Move(Place place)
	{
		// Check if the cooldown has passed based on DateTime
		if (lastPathUpdateElapsed >= pathUpdateCooldown && (path.Count == 0 || targetPosition != PlayerPosition))
		{
			// Update path and target position
			path = FindPath(Position, PlayerPosition - Size / 2, place);
			targetPosition = PlayerPosition;
			lastPathUpdateElapsed = 0; // Update the last path update time
		}

		if (Moving)
			FollowPath();
	}

	private Queue<Vector2> FindPath(Vector2 start, Vector2 goal, Place place)
	{
		// Convert positions to grid-aligned points (multiples of 50)
		Vector2 startNode = new Vector2((float)Math.Round(start.X / 50) * 50, (float)Math.Round(start.Y / 50) * 50);
		Vector2 goalNode = new Vector2((float)Math.Round(goal.X / 50) * 50, (float)Math.Round(goal.Y / 50) * 50);

		HashSet<Vector2> closedSet = new HashSet<Vector2>();
		PriorityQueue<Vector2, float> openSet = new PriorityQueue<Vector2, float>();
		Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
		Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float> { [startNode] = 0 };
		Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float> { [startNode] = Vector2.Distance(startNode, goalNode) };

		openSet.Enqueue(startNode, fScore[startNode]);

		while (openSet.Count > 0)
		{
			Vector2 current = openSet.Dequeue();

			if (current == goalNode)
			{
				Queue<Vector2> path = new Queue<Vector2>();
				while (cameFrom.ContainsKey(current))
				{
					path.Enqueue(current);
					current = cameFrom[current];
				}
				path = new Queue<Vector2>(path.Reverse());
				return path;
			}

			closedSet.Add(current);

			foreach (Vector2 neighbor in GetNeighbors(current))
			{
				if (closedSet.Contains(neighbor) || place.ShouldCollideAt(neighbor))
					continue;

				float tentativeGScore = gScore[current] + Vector2.Distance(current, neighbor);

				if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
				{
					cameFrom[neighbor] = current;
					gScore[neighbor] = tentativeGScore;
					fScore[neighbor] = tentativeGScore + Vector2.Distance(neighbor, goalNode);
					openSet.Enqueue(neighbor, fScore[neighbor]);
				}
			}
		}
		return new Queue<Vector2>();
	}

	private IEnumerable<Vector2> GetNeighbors(Vector2 node)
	{
		List<Vector2> neighbors = new List<Vector2>
	{
		node + new Vector2(50, 0),  // Right
		node + new Vector2(-50, 0), // Left
		node + new Vector2(0, 50),  // Down
		node + new Vector2(0, -50)  // Up
	};
		return neighbors;
	}
}