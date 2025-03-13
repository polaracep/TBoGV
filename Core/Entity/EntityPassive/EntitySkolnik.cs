using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;
using System.Linq;

class EntitySkolnik : EntityPassive
{
    public const string NAME = "Školník";
    protected double lastPathUpdateElapsed = 0;
    protected Vector2 PlayerPosition;
    protected Vector2 targetPosition;
    protected float pathUpdateCooldown = 500;
    protected bool talkedWithPlayer = false;
    protected Queue<Vector2> path = new Queue<Vector2>();
    public EntitySkolnik(Vector2 position) : base(position, NAME)
    {
        MovementSpeed = 3;
    }
    public EntitySkolnik() : base(NAME) { MovementSpeed = 3; }

    public override Texture2D GetSprite()
    {
        return TextureManager.GetTexture("skolnik");
    }
    public void Update(double dt)
    {
        lastPathUpdateElapsed += dt;

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
    public void Move(Place place)
    {
        if (talkedWithPlayer)
            return;
        PlayerPosition = place.player.Position;
        // Check distance to player
        float distanceToPlayer = Vector2.Distance(Position, PlayerPosition);

        if (distanceToPlayer <= 200)
        {
            // Move directly towards the player
            if ((path.Count == 0 || lastPathUpdateElapsed >= pathUpdateCooldown) && distanceToPlayer > 25)
            {
                path = FindPath(Position, PlayerPosition + place.player.Size / 3, place);
                targetPosition = PlayerPosition;
                lastPathUpdateElapsed = 0;
            }
        }
        else
        {
            // Move to random position when far from the player
            if (path.Count == 0 || Vector2.Distance(Position, targetPosition) <= 100)
            {
                targetPosition = new Vector2(
                    new Random().Next(50, (int)(place.Dimensions.X * 50)), // Random X position (adjust limits)
                    new Random().Next(50, (int)(place.Dimensions.Y * 50))  // Random Y position (adjust limits)
                );
                path = FindPath(Position, targetPosition, place);
            }
        }
        FollowPath();
        if (distanceToPlayer <= 100 && !talkedWithPlayer)
        {
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnik());
            talkedWithPlayer = true;
        }
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