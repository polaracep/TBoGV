using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class BossAmogus : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("amogusSpritesheet");
	static Texture2D VentSpritesheet = TextureManager.GetTexture("ventSpritesheet");
	static SoundEffect AligatorSfx = SoundManager.GetSound("AMOGUS");

	private static int frameWidthVent = 16;
	private static int frameHeightVent = 21;
	private static float ScaleVent = 54f / Math.Max(frameWidthVent, frameHeightVent);
	int ventFrame = -1;
	private float Scale;
	private int frameWidth = 100;
	private int frameHeight = 100;
	private int currentFrame = 0;
	private bool lookingLeft = true;
	private double animationElapsed = 0;
	private double animationSpeed = 100;
	private int totalMoveFrames = 4; // First 5 frames are for movement


	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int rageDuration = 2500;

	private Queue<Vector2> path = new Queue<Vector2>();
	private double lastPathUpdateElapsed = 0;
	private Vector2 PlayerPosition;
	private Vector2 targetPosition;
	private float pathUpdateCooldown = 0.1f;
	private bool Rage = false;
	private enum amogusState : int
	{
		MOVING = 0,
		VENTINGOUT = 1,
		VENTINGIN = 2,
		STANDING = 3,
	}
	private amogusState State = amogusState.STANDING;
	private double ventingTimer = 0;
	private int ventingFrame = 6;
	private float hpBeforeVenting;
	public BossAmogus(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		Position = position;
		Scale = 50f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		phaseChangeElapsed = 0;
	}
	public BossAmogus() : this(Vector2.Zero) { }

	public override void Update(Vector2 playerPosition, double dt)
	{
		PlayerPosition = playerPosition;
		if (!IsVenting())
		{
			lookingLeft = playerPosition.X - Position.X < 0;
			UpdateAnimation(dt);
			UpdatePhase(dt);
		}
		else
		{
			UpdateVenting(dt);
		}
	}

	private void UpdateAnimation(double dt)
	{
		animationElapsed += dt;
		if (animationElapsed >= animationSpeed)
		{
			animationElapsed = 0;
			currentFrame = (currentFrame + 1) % totalMoveFrames;
		}
	}

	private void UpdateVenting(double dt)
	{
		ventingTimer += dt;

		if (IsVenting())
			ventFrame = 1;
		if (ventingFrame == 8 && State == amogusState.VENTINGIN) // Fully inside the vent
			ventFrame = 0;


		if (State == amogusState.VENTINGOUT)
		{
			if (ventingTimer >= 400 / 4)
			{
				ventingFrame++;
				ventingTimer = 0;
				if (ventingFrame > 8)
				{
					ventingFrame = 8;
					State = amogusState.VENTINGIN;
					ventingTimer = -500; // Pause in the vent
					VentToNewPosition();
				}
			}
		}
		else if (State == amogusState.VENTINGIN)
		{
			if (ventingTimer >= 400 / 4)
			{
				ventingFrame--;
				ventingTimer = 0;
				if (ventingFrame < 5)
				{
					ventingFrame = 5;
					State = amogusState.MOVING;
				}
			}
		}
	}
	private void VentToNewPosition()
	{
		while (true)
		{
			Position = new Vector2(random.Next(50, 400), random.Next(50, 400));
			if ((Position - PlayerPosition).Length() > 200)
				return;
		}
	}
	protected void UpdatePhase(double dt)
	{
		phaseChangeElapsed += dt;
		lastPathUpdateElapsed += dt;

		if ((phaseChangeElapsed > rageDuration && Rage) ||
			(phaseChangeElapsed > chillDuration && !Rage))
		{
			Rage = !Rage;
			phaseChangeElapsed = 0;
			chillDuration = 0;
		}
		if (!IsVenting())
			State = Rage ? amogusState.MOVING : amogusState.STANDING;
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

		if (Rage && !IsVenting())
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
        Random random = new Random();
        List<Item> droppedItems = new List<Item>();
        droppedItems = base.Drop(looting);
        var item = new ItemFixa(Position + Size / 2);
        item.InitMovement();
        droppedItems.Add(item);
        return droppedItems;
    }
    public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}
	private bool IsVenting()
	{
		return State == amogusState.VENTINGIN || State == amogusState.VENTINGOUT;
	}
	public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile) && !IsVenting())
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);

			if (Hp <= hpBeforeVenting - 10)
			{
				Vent();
			}
			return 0;
		}
		return projectile.Damage;
	}
	private void Vent()
	{
		State = amogusState.VENTINGOUT;
		ventingFrame = 6;
		ventingTimer = 0;
		hpBeforeVenting = Hp;
		AligatorSfx.Play();
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		// Draw the vent if applicable
		if (IsVenting())
		{
			Rectangle ventSourceRect = new Rectangle(ventFrame * frameWidthVent, 0, frameWidthVent, frameHeightVent);
			spriteBatch.Draw(VentSpritesheet,
				new Rectangle((int)Position.X - 2, (int)(Position.Y + Size.Y - frameHeightVent * ScaleVent + 8), (int)(frameWidthVent * ScaleVent), (int)(frameHeightVent * ScaleVent)),
				ventSourceRect,
				Color.White,
				0,
				Vector2.Zero,
				SpriteEffects.None,
				0);
		}

		// Determine Amogus frame
		int frameIndex = !IsVenting() ? (State == amogusState.STANDING ? 0 : currentFrame) : ventingFrame;
		Rectangle sourceRect = new Rectangle(123, frameIndex * frameHeight, frameWidth, frameHeight);
		SpriteEffects effects = lookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

		// Draw Amogus on top
		spriteBatch.Draw(Spritesheet,
			new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)),
			sourceRect,
			Color.White,
			0,
			Vector2.Zero,
			effects,
			0);
	}

	protected override void InitStats(int difficulty)
	{
		hpBeforeVenting = Hp = 80;
		//	AttackSpeed = 0;
		MovementSpeed = 6;
		AttackDmg = 1;
		XpValue = 70;
	}
}