using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

internal class BossZeman : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("milosSpritesheet");
	static SoundEffect AligatorSfx = SoundManager.GetSound("aligator");

	private float Scale;
	private int frameWidth = 147;
	private int frameHeight = 113;
	private int frameCount = 4;
	private int currentFrame = 0;
	private bool lookingLeft = true;

	private DateTime phaseChange = DateTime.UtcNow;
	private int chillDuration = 3000;
	private int rageDuration = 2500;

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
		Scale = 50f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		XpValue = 70;
		phaseChange = DateTime.UtcNow;
	}
	public BossZeman() : this(Vector2.Zero) { }

	public override void Update(Vector2 playerPosition)
	{
		PlayerPosition = playerPosition;
		lookingLeft = playerPosition.X - Position.X < 0;
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
			if (Rage)
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
			path = FindPath(Position, PlayerPosition - Size / 2, place);
			targetPosition = PlayerPosition;
			lastPathUpdate = DateTime.UtcNow; // Update the last path update time
		}

		if (Rage)
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

	public override bool ReadyToAttack()
	{
		return Rage;
	}

	public override bool IsDead()
	{
		return Hp <= 0;
	}

	public override List<Item> Drop(int looting)
	{
		return new List<Item>() { new ItemTeeth(this.Position) };
	}

	public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile))
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);
			return 0;
		}
		return projectile.Damage;
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		currentFrame = Convert.ToInt32(!lookingLeft) + Convert.ToInt32(!Rage) * 2;
		Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)), sourceRect, Color.White);

	}
}