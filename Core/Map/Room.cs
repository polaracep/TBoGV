using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public enum Directions
{
    LEFT = 0,
    RIGHT = 1,
    UP = 2,
    DOWN = 3,
    ENTRY = 4,
}

public abstract class Room : Place
{
    /// <summary>
    /// Map position used in level generation
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// Contains all doors.
    /// Needed for moving between rooms
    /// </summary>
    public List<TileDoor> Doors = new List<TileDoor>();
    protected List<Projectile> Projectiles = new List<Projectile>();

    public bool IsEndRoom = false;
    /// <summary>
    /// List of spawnable enemies
    /// </summary>
    protected List<Enemy> EnemyPool = new List<Enemy>();

    public Room(Vector2 dimensions, Vector2 pos, Player p, List<Entity> entityList)
    {
        this.player = p;
        this.Dimensions = dimensions;
        this.Position = pos;

        this.Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        if (entityList == null)
            return;

        // Sort list based on the entity type
        entityList.ForEach(e =>
        {
            if (e is Enemy)
                EnemyPool.Add((Enemy)e);
            else if (e is Item)
                Drops.Add((Item)e);
            else if (e is EntityPassive)
                Entities.Add((EntityPassive)e);
            else
                throw new Exception("Invalid entity type provided.");
        });

        /*
        this.Drops.Add(new ItemDoping(new Vector2(200, 200)));
        this.Drops.Add(new ItemTeeth(new Vector2(100, 200)));
        this.Drops.Add(new ItemCalculator(new Vector2(150, 200)));
        this.Drops.Add(new ItemPencil(new Vector2(100, 100)));
        this.Drops.Add(new ItemAdBlock(new Vector2(50, 50)));
        this.Drops.Add(new ItemMathProblem(new Vector2(50, 100)));
        this.Drops.Add(new ItemExplosive(new Vector2(50, 150)));
        */
    }
    public Room(Vector2 dimensions, Vector2 pos, Player p) : this(dimensions, pos, p, null) { }
    public Room(Vector2 dimensions, Player p) : this(dimensions, Vector2.Zero, p, null) { }
    public Room(Vector2 dimensions, Player p, List<Entity> entityList) : this(dimensions, Vector2.Zero, p, entityList) { }

    public override void Reset()
    {
        this.ClearEnemies();
        this.ClearProjectiles();
        this.Generate();
        this.GenerateEnemies();
    }

    /* === Update methods === */
    public override void Update(double dt)
    {
        this.UpdateProjectiles();
        this.UpdateEnemies(dt);
        for (int i = 0; i < Particles.Count; i++)
        {
            Particles[i].Update(dt);
            if (!Particles[i].Visible)
                Particles.Remove(Particles[i]);
        }
    }
    protected void UpdateProjectiles()
    {
        for (int i = Projectiles.Count - 1; i >= 0; i--)
        {
            Projectiles[i].Update();

            if (ObjectCollision.CircleCircleCollision(Projectiles[i], player))
            {
                float excessDmg = player.RecieveDmg(Projectiles[i]);
                Projectiles[i].Damage = excessDmg;
                if (Projectiles[i].Damage <= 0)
                    Projectiles.RemoveAt(i);
                continue;
            }
            if (this.ShouldCollideAt(Projectiles[i].GetCircleCenter(), true))
            {
                Projectiles.RemoveAt(i);
                continue;
            }
            if (Projectiles[i] is ProjectileMelee)
                Projectiles.RemoveAt(i);
        }
        for (int i = player.Projectiles.Count - 1; i >= 0; i--)
        {
            player.Projectiles[i].Update();
            if (this.ShouldCollideAt(player.Projectiles[i].GetCircleCenter(), true))
            {
                DestroyPlayerProjectile(i);
                continue;
            }
            for (int j = 0; j < Enemies.Count; j++)
                if (ObjectCollision.CircleCircleCollision(player.Projectiles[i], Enemies[j]))
                {
                    // HOnim HOdne HOdin - SANTA REFERENCE
                    float excessDmg = Enemies[j].RecieveDmg(player.Projectiles[i]);
                    if (Enemies[j].IsDead())
                    {
                        if (Enemies[j] is EnemyBoss || (Enemies.Count == 1 && IsEndRoom))
                            GenerateExit();

                        player.Kill(Enemies[j].XpValue);
                        foreach (Item item in Enemies[j].Drop(1))
                            Drops.Add(item);
                        Enemies.RemoveAt(j);
                    }
                    if (!player.Inventory.GetEffect().Contains(EffectTypes.PIERCING) && !player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
                    {
                        player.Projectiles[i].Damage = excessDmg;
                    }
                    if (player.Projectiles[i].Damage <= 0 || player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
                        DestroyPlayerProjectile(i);
                    if (Enemies.Count > j)

                        break;
                }
        }
    }
    private void DestroyPlayerProjectile(int index)
    {
        if (player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
        {
            ParticleExplosion explosion = new ParticleExplosion(player.Projectiles[index].Position);
            Particles.Add(explosion);
            for (int j = 0; j < Enemies.Count; j++)
            {
                if (ObjectCollision.CircleCircleCollision(explosion, Enemies[j]))
                {
                    Enemies[j].RecieveDmg(player.Projectiles[index]);
                    if (Enemies[j].IsDead())
                    {

                        player.Kill(Enemies[j].XpValue);
                        foreach (Item item in Enemies[j].Drop(1))
                            Drops.Add(item);
                        Enemies.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
        player.Projectiles.RemoveAt(index);
    }
    protected void UpdateEnemies(double dt)
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.Update(player.Position + player.Size / 2, dt);
            enemy.Move(this);

            if (enemy.ReadyToAttack())
            {
                foreach (var projectile in enemy.Attack())
                {
                    Projectiles.Add(projectile);
                }
            }

        }

    }

    /* === Generation methods === */
    protected virtual void GenerateEnemies()
    {
        Random rand = new Random();
        foreach (var enemy in EnemyPool)
        {
            while (true)
            {
                Vector2 spawnPos = new Vector2(rand.Next((int)Dimensions.X - 2) + 1, rand.Next((int)Dimensions.Y - 2) + 1) * 50;
                if (!this.ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), enemy.Size.ToPoint())))
                {
                    enemy.Position = spawnPos;
                    this.AddEnemy(enemy);
                    break;
                }
            }
        }
    }
    protected virtual void GenerateBase() { GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC); }
    protected virtual void GenerateBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        this.ClearEnemies();
        this.ClearProjectiles();

        Floor.GenerateFilledRectangle(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(floors),
            new TileWall(walls)
        );

        GenerateDoors(doors);

        IsGenerated = true;
    }

    protected virtual void GenerateDecor() { }

    protected virtual void GenerateDoors(DoorTypes doors) { GenerateDoors(doors, false); }
    protected virtual void GenerateDoors(DoorTypes doors, bool center)
    {
        if (this.Doors == null)
            throw new ArgumentNullException("This room does not have any doors!");
        // Generace dveri
        int _x;
        int _y;
        if (center)
        {
            _x = (int)Dimensions.X / 2;
            _y = (int)Dimensions.Y / 2;
        }
        else
        {
            Random rand = new Random();
            _x = rand.Next((int)Dimensions.X - 2) + 1;
            _y = rand.Next((int)Dimensions.Y - 2) + 1;
        }
        foreach (TileDoor door in this.Doors)
        {
            if (door.Sprite != TextureManager.GetTexture("doorBoss"))
                door.SetDoorType(doors);
            switch (door.Direction)
            {
                case Directions.LEFT:
                    door.DoorTpPosition = new Vector2(1, _y);
                    Decorations[0, _y] = door;
                    break;
                case Directions.RIGHT:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X - 2, _y);
                    Decorations[(int)Dimensions.X - 1, _y] = door;
                    break;
                case Directions.UP:
                    door.DoorTpPosition = new Vector2(_x, 1);
                    Decorations[_x, 0] = door;
                    break;
                case Directions.DOWN:
                    door.DoorTpPosition = new Vector2(_x, (int)Dimensions.Y - 2);
                    Decorations[_x, (int)Dimensions.Y - 1] = door;
                    break;
            }
        }
    }

    public virtual void ClearEnemies()
    {
        this.Enemies.Clear();
    }
    public virtual void ClearProjectiles()
    {
        this.Projectiles.Clear();
        this.player.Projectiles.Clear();
    }
    public virtual void ClearRoom()
    {
        ClearProjectiles();
        ClearEnemies();
    }
    public virtual void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (Item item in Drops)
            item.Draw(spriteBatch);
        foreach (Enemy enemy in Enemies)
            enemy.Draw(spriteBatch);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Projectile projectile in Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Particle particle in Particles)
            particle.Draw(spriteBatch);
    }

    public virtual void GenerateExit()
    {
        Random rand = new Random();
        int _x, _y;
        do
        {
            _x = rand.Next((int)Dimensions.X - 2) + 1;
            _y = rand.Next((int)Dimensions.Y - 2) + 1;
        }
        while (Decorations[_x, _y] != null);

        switch (rand.Next(4))
        {
            case 0:
                AddDecoTile(new Vector2(0, _y), new TileExit());
                break;
            case 1:
                AddDecoTile(new Vector2(_x, 0), new TileExit());
                break;
            case 2:
                AddDecoTile(new Vector2(Dimensions.X - 1, _y), new TileExit());
                break;
            case 3:
                AddDecoTile(new Vector2(_x, Dimensions.Y - 1), new TileExit());
                break;
        }
    }
}