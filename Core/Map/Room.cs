using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV.Core;

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
    /// Position used in level generation
    /// </summary>
    public Vector2 Position { get; protected set; }
    public List<Directions> DoorDirectons = new List<Directions>();
    /// <summary>
    /// Use for room layout
    /// </summary>
    protected Tile[,] roomFloor;
    /// <summary>
    /// Use for interactable and changing tiles
    /// </summary>
    protected Tile[,] roomDecorations;

    protected List<Room> adjacentRooms = new List<Room>();
    protected List<Projectile> projectiles;
    protected List<Enemy> enemies;
    protected Player player;

    public Room(Vector2 dimensions, Vector2 pos, Directions? entryDir, Player p)
    {
        this.projectiles = new List<Projectile>();
        this.enemies = new List<Enemy>();
        this.player = p;
        this.Dimensions = dimensions;
        this.Position = pos;
        if (entryDir != null)
            this.DoorDirectons.Add((Directions)entryDir);
        else
            this.DoorDirectons.Add(Directions.ENTRY);
    }
    public Room(Vector2 dimensions, Vector2 pos, Player p) : this(dimensions, pos, null, p) { }

    /// <summary>
    /// Returns the left-top world position for any tile position
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Vector2 GetTilePos(Vector2 coords)
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
    public (Tile floor, Tile decor) GetTile(Vector2 coords)
    {
        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            return new(Tile.NoTile, Tile.NoTile);
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            return new(Tile.NoTile, Tile.NoTile);

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
    public virtual void Update()
    {
        this.UpdateProjectiles();
        this.UpdateEnemies();
    }
    protected virtual void UpdateProjectiles()
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update();

            if (ObjectCollision.CircleCircleCollision(projectiles[i], player))
            {
                player.RecieveDmg(projectiles[i].Damage);
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
                player.Projectiles.RemoveAt(i);
                continue;
            }
            for (int j = 0; j < enemies.Count; j++)
                if (ObjectCollision.CircleCircleCollision(player.Projectiles[i], enemies[j]))
                {
                    // HOnim HOdne HOdin - SANTA REFERENCE
                    enemies[j].RecieveDmg(player.Projectiles[i].Damage);
                    player.Projectiles.RemoveAt(i);
                    if (enemies[j].IsDead())
                    {
                        player.Kill(enemies[j].XpValue);
                        enemies.RemoveAt(j);
                    }
                    break;
                }

        }
    }
    protected virtual void UpdateEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Update(player.Position + player.Size / 2);

            if (enemy.ReadyToAttack())
                projectiles.Add(enemy.Attack());
        }

    }

    /* === Generation methods === */
    public abstract void GenerateRoom();
    protected abstract void GenerateEnemies();
    protected virtual void GenerateRoomBase()
    {
        this.ClearRoom();
        this.roomFloor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.roomDecorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        for (int i = 1; i < Dimensions.X - 1; i++)
            for (var j = 1; j < Dimensions.Y - 1; j++)
                roomFloor[i, j] = new TileFloor(FloorTypes.BASIC);

        for (int i = 0; i < Dimensions.X; i++)
        {
            roomFloor[i, 0] = new TileWall(WallTypes.BASIC);
            roomFloor[i, (int)Dimensions.Y - 1] = new TileWall(WallTypes.BASIC);
        }

        for (int i = 0; i < Dimensions.Y; i++)
        {
            roomFloor[0, i] = new TileWall(WallTypes.BASIC);
            roomFloor[(int)Dimensions.X - 1, i] = new TileWall(WallTypes.BASIC);
        }
    }
    protected virtual void ClearRoom()
    {
        this.projectiles.Clear();
        this.enemies.Clear();
    }
    public virtual bool AddTile(Tile tile, Vector2 position)
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
    public virtual void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Dimensions.X; i++)
            for (var j = 0; j < Dimensions.Y; j++)
            {
                Tile t = roomFloor[i, j];
                spriteBatch.Draw(t.Sprite, new Vector2(i * Tile.GetSize().X, j * Tile.GetSize().Y), Color.White);
            }

        foreach (Enemy enemy in enemies)
            enemy.Draw(spriteBatch);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Projectile projectile in projectiles)
            projectile.Draw(spriteBatch);
    }
}