using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

internal class BossZeman : EnemyBoss
{
	static Texture2D Sprite = TextureManager.GetTexture("milosSpinks");
	static SoundEffect AligatorSfx = SoundManager.GetSound("aligator");
	protected DateTime phaseChange = DateTime.UtcNow;
	protected int chillDuration = 3000;
	protected int rageDuration = 2500;

	private Queue<Vector2> path = new Queue<Vector2>();
	private DateTime lastPathUpdate = DateTime.UtcNow;
	private Vector2 PlayerPosition;
	private Vector2 targetPosition;
	private float pathUpdateCooldown = 0.1f;
	private bool Rage = false;
	public BossZeman(Vector2 position)
	{
		Position = position;
		Hp = 80;
		MovementSpeed = 7;
		AttackDmg = 2;
		float scale = 45f / Math.Max(Sprite.Width, Sprite.Height);
		Size = new Vector2(Sprite.Width * scale, Sprite.Height * scale);
		XpValue = 70;
	}

	public override void Update(Vector2 playerPosition)
	{
		PlayerPosition = playerPosition;
		UpdatePhase();
	}
	protected void UpdatePhase()
	{
		if (((DateTime.UtcNow - phaseChange).TotalMilliseconds > rageDuration && Rage) ||
			((DateTime.UtcNow - phaseChange).TotalMilliseconds > chillDuration && !Rage))
		{
			Rage = !Rage;
			phaseChange = DateTime.UtcNow;
			chillDuration = new Random().Next(2000, 3500);
			if(Rage)
				AligatorSfx.Play();
		}
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
		if ((DateTime.UtcNow - lastPathUpdate).TotalSeconds >= pathUpdateCooldown && (path.Count == 0 || targetPosition != PlayerPosition))
		{
			// Update path and target position
			path = FindPath(Position, PlayerPosition, place);
			targetPosition = PlayerPosition;
			lastPathUpdate = DateTime.UtcNow; // Update the last path update time
		}

		if(Rage)
			FollowPath();
	}

	private Queue<Vector2> FindPath(Vector2 start, Vector2 goal, Place place)
	{
		HashSet<Vector2> closedSet = new HashSet<Vector2>();
		PriorityQueue<Vector2, float> openSet = new PriorityQueue<Vector2, float>();
		Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
		Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float> { [start] = 0 };
		Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float> { [start] = Vector2.Distance(start, goal) };

		openSet.Enqueue(start, fScore[start]);

		while (openSet.Count > 0)
		{
			Vector2 current = openSet.Dequeue();

			if (current == goal)
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
					fScore[neighbor] = tentativeGScore + Vector2.Distance(neighbor, goal);
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
			node + new Vector2(1, 0),
			node + new Vector2(-1, 0),
			node + new Vector2(0, 1),
			node + new Vector2(0, -1)
		};
		return neighbors;
	}

	public override bool ReadyToAttack()
	{
		return true;
	}

	public override bool IsDead()
	{
		return Hp <= 0;
	}

	public override List<Item> Drop(int looting)
	{
		return new List<Item>() { new ItemTeeth(Vector2.Zero) };
	}

	public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile))
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);
		}
		return projectile.Damage;
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
	}
}