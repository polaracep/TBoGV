using System;
using System.Collections.Generic;
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

public abstract class Room : IDraw
{
    public Vector2 Dimensions { get; protected set; }
    /// <summary>
    /// Map position used in level generation
    /// </summary>
    public Vector2 Position;
    public List<TileDoor> Doors = new List<TileDoor>();
    public bool IsGenerated { get; protected set; } = false;
    /// <summary>
    /// Use for room layout
    /// </summary>
    protected Tile[,] roomFloor;
    /// <summary>
    /// Use for interactable and changing tiles
    /// </summary>
    protected Tile[,] roomDecorations;
    protected Tile[,] ValidSpawns;

    protected List<Projectile> projectiles = new List<Projectile>();
    protected List<Enemy> enemies = new List<Enemy>();
    protected List<Enemy> EnemyPool = new List<Enemy>();
    public List<Item> drops = new List<Item>() {
        new ItemDoping(new Vector2(200, 200)),
        new ItemTeeth(new Vector2(100, 200)),
        new ItemCalculator(new Vector2(150, 200)),
        new ItemPencil(new Vector2(100, 100)),
        new ItemAdBlock(new Vector2(50, 50)),
        new ItemMathProblem(new Vector2(50, 100)),
        new ItemExplosive(new Vector2(50, 150))
    };
    protected List<Particle> particles = new List<Particle>();
    public Player player;

    public Room(Vector2 dimensions, Vector2 pos, Player p, List<Enemy> enemyList)
    {
        this.player = p;
        this.Dimensions = dimensions;
        this.Position = pos;

        if (enemyList != null)
        {
            enemyList.ForEach(x => x.Position = Vector2.Zero);
            this.EnemyPool = enemyList;
        }
    }
    public Room(Vector2 dimensions, Player p, List<Enemy> enemyList) : this(dimensions, Vector2.Zero, p, enemyList) { }

    public Room(Vector2 dimensions, Vector2 pos, Player p) : this(dimensions, pos, p, null) { }

    public Room(Vector2 dimensions, Player p) : this(dimensions, Vector2.Zero, p, null) { }

    /// <summary>
    /// Returns the left-top world position for any tile position
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Vector2 GetTileWorldPos(Vector2 coords)
    {
        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            throw new ArgumentOutOfRangeException();
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            throw new ArgumentOutOfRangeException();
        return new Vector2((int)(coords.X * Tile.GetSize().X), (int)(coords.Y * Tile.GetSize().Y));
    }
    public Tile GetTileFloor(Vector2 coords)
    {
        return GetTile(coords).floor;
    }
    public Tile GetTileDecoration(Vector2 coords)
    {
        return GetTile(coords).decor;
    }
    public Tile GetTileInteractable(Vector2 coords)
    {
        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            throw new ArgumentOutOfRangeException();
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            throw new ArgumentOutOfRangeException();

        (Tile, Tile) t = GetTile(coords);
        if (t.Item2 is IInteractable)
            return t.Item2;
        else if (t.Item1 is IInteractable)
            return t.Item1;

        return null;
    }
    public Item GetItemInteractable(Vector2 coords)
    {
        foreach (var item in drops)
        {
            if (ObjectCollision.RectCircleCollision(item.GetRectangle(), coords, 5))
                return item;
        }
        return null;
    }
    public void RemoveItem(Item item)
    {
        drops.Remove(item);
    }
    public (Tile floor, Tile decor) GetTile(Vector2 coords)
    {
        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            throw new ArgumentOutOfRangeException();
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            throw new ArgumentOutOfRangeException();

        return (roomFloor[(int)(coords.X / Tile.GetSize().X), (int)(coords.Y / Tile.GetSize().Y)],
                roomDecorations[(int)(coords.X / Tile.GetSize().X), (int)(coords.Y / Tile.GetSize().Y)]);
    }
    public bool ShouldCollideAt(Vector2 coords)
    {
        return (this.GetTileFloor(coords)?.DoCollision ?? false) ||
               (this.GetTileDecoration(coords)?.DoCollision ?? false);
    }
    public virtual void ResetRoom()
    {
        this.ClearRoom();
        this.GenerateRoom();
    }

    /* === Update methods === */
    public void Update(GameTime gameTime)
    {
        this.UpdateProjectiles();
        this.UpdateEnemies();
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Update(gameTime);
            if (!particles[i].Visible)
                particles.Remove(particles[i]);
        }
    }
    protected void UpdateProjectiles()
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update();

            if (ObjectCollision.CircleCircleCollision(projectiles[i], player))
            {
                float excessDmg = player.RecieveDmg(projectiles[i]);
                projectiles[i].Damage = excessDmg;
                if (projectiles[i].Damage <= 0)
                    projectiles.RemoveAt(i);
                continue;
            }
            if (this.ShouldCollideAt(projectiles[i].GetCircleCenter()))
            {
                projectiles.RemoveAt(i);
            }
        }
        for (int i = player.Projectiles.Count - 1; i >= 0; i--)
        {
            player.Projectiles[i].Update();
            if (this.ShouldCollideAt(player.Projectiles[i].GetCircleCenter()))
            {
                DestroyPlayerProjectile(i);
                continue;
            }
            for (int j = 0; j < enemies.Count; j++)
                if (ObjectCollision.CircleCircleCollision(player.Projectiles[i], enemies[j]))
                {
                    // HOnim HOdne HOdin - SANTA REFERENCE
                    float excessDmg = enemies[j].RecieveDmg(player.Projectiles[i]);
                    if (enemies[j].IsDead())
                    {
                        player.Kill(enemies[j].XpValue);
                        foreach (Item item in enemies[j].Drop(1))
                            drops.Add(item);
                        enemies.RemoveAt(j);
                    }
                    if (!player.Inventory.GetEffect().Contains(EffectTypes.PIERCING) && !player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
                    {
                        player.Projectiles[i].Damage = excessDmg;
                    }
                    if (player.Projectiles[i].Damage <= 0 || player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
                        DestroyPlayerProjectile(i);
                    if (enemies.Count > j)

                        break;
                }
        }
    }
    private void DestroyPlayerProjectile(int index)
    {
        if (player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
        {
            ParticleExplosion explosion = new ParticleExplosion(player.Projectiles[index].Position);
            particles.Add(explosion);
            for (int j = 0; j < enemies.Count; j++)
            {
                if (ObjectCollision.CircleCircleCollision(explosion, enemies[j]))
                {
                    enemies[j].RecieveDmg(player.Projectiles[index]);
                    if (enemies[j].IsDead())
                    {
                        player.Kill(enemies[j].XpValue);
                        foreach (Item item in enemies[j].Drop(1))
                            drops.Add(item);
                        enemies.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
        player.Projectiles.RemoveAt(index);
    }
    protected void UpdateEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Update(player.Position + player.Size / 2);

            if (enemy.ReadyToAttack())
            {
                foreach (var projectile in enemy.Attack())
                {
                    projectiles.Add(projectile);
                }
            }

        }

    }

    /* === Generation methods === */
    public virtual void GenerateRoom()
    {
        IsGenerated = true;
    }
    protected virtual void GenerateEnemies() { }
    protected virtual void GenerateRoomBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        if (this.Doors == null)
            throw new ArgumentNullException("This room does not have any doors!");

        this.ClearRoom();
        this.roomFloor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.roomDecorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        for (int i = 0; i < Dimensions.X; i++)
            for (var j = 0; j < Dimensions.Y; j++)
                roomFloor[i, j] = new TileFloor(floors);

        for (int i = 0; i < Dimensions.X; i++)
        {
            roomFloor[i, 0] = new TileWall(walls);
            roomFloor[i, (int)Dimensions.Y - 1] = new TileWall(walls);
        }

        for (int i = 0; i < Dimensions.Y; i++)
        {
            roomFloor[0, i] = new TileWall(walls);
            roomFloor[(int)Dimensions.X - 1, i] = new TileWall(walls);
        }

        // Generace dveri
        foreach (TileDoor door in this.Doors)
        {
            door.SetDoorType(doors);
            switch (door.Direction)
            {
                case Directions.LEFT:
                    door.DoorTpPosition = new Vector2(1, (int)Dimensions.Y / 2);
                    roomDecorations[0, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.RIGHT:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X - 2, (int)Dimensions.Y / 2);
                    roomDecorations[(int)Dimensions.X - 1, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.UP:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, 1);
                    roomDecorations[(int)Dimensions.X / 2, 0] = door;
                    break;
                case Directions.DOWN:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, (int)Dimensions.Y - 2);
                    roomDecorations[(int)Dimensions.X / 2, (int)Dimensions.Y - 1] = door;
                    break;
            }
        }

        IsGenerated = true;
    }
    protected virtual void ClearRoom()
    {
        this.projectiles.Clear();
        this.enemies.Clear();
        //this.drops.Clear();
    }
    public bool AddFloorTile(Tile tile, Vector2 position)
    {
        try
        {
            roomFloor[(int)position.X, (int)position.Y] = tile;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public bool AddDecorationTile(Tile tile, Vector2 position)
    {
        try
        {
            roomDecorations[(int)position.X, (int)position.Y] = tile;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public virtual void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!this.IsGenerated)
            this.GenerateRoom();
        for (int i = 0; i < Dimensions.X; i++)
            for (var j = 0; j < Dimensions.Y; j++)
            {
                Tile t = roomFloor[i, j];
                if (t != null)
                {
                    Vector2 origin = new Vector2(25, 25);
                    spriteBatch.Draw(t.Sprite, new Vector2(i * Tile.GetSize().X, j * Tile.GetSize().Y) + origin, null, Color.White, t.Rotation, origin, 1f, SpriteEffects.None, 0f);
                }
                t = roomDecorations[i, j];
                if (t != null)
                {
                    Vector2 origin = new Vector2(25, 25);
                    spriteBatch.Draw(t.Sprite, new Vector2(i * Tile.GetSize().X, j * Tile.GetSize().Y) + origin, null, Color.White, t.Rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        foreach (Item item in drops)
            item.Draw(spriteBatch);
        foreach (Enemy enemy in enemies)
            enemy.Draw(spriteBatch);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Projectile projectile in projectiles)
            projectile.Draw(spriteBatch);
        foreach (Particle particle in particles)
            particle.Draw(spriteBatch);
    }
}