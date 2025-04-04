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
    protected bool exitGenerated = false;
    public bool IsVisited = false;
    /// <summary>
    /// List of spawnable enemies
    /// </summary>
    protected List<Enemy> EnemyPool = new List<Enemy>();
    protected abstract List<Enemy> validEnemies { get; set; }
    protected Directions? direction = null;
    protected static Texture2D SpriteBlacksquare = TextureManager.GetTexture("blackSquare");
    protected static Texture2D SpriteWhitesquare = TextureManager.GetTexture("whiteSquare");
    public Room((int sMin, int sMax, int bMax) dimensions, Player p, List<Entity> entityList)
    {
        player = p;
        Position = Vector2.Zero;

        GenerateDimensions(dimensions);
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        if (entityList == null)
            return;

        // Sort list based on the entity type
        entityList.ForEach(e =>
        {
            if (e is Enemy && !EnemyPool.Contains((Enemy)e))
                EnemyPool.Add((Enemy)e);
            else if (e is Item)
                Drops.Add((Item)e);
            else if (e is EntityPassive)
                Entities.Add((EntityPassive)e);
            else
                throw new Exception("Invalid entity type provided.");
        });
    }
    public Room(Vector2 dimensions, Player p, List<Entity> entityList) : this((1, 2, 3), p, entityList)
    {
        Dimensions = dimensions;
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
    }
    public Room((int sMin, int sMax, int bMax) dimensions, Player p) : this(dimensions, p, null) { }
    public Room(Vector2 dimensions, Player p) : this(dimensions, p, null) { }

    /* === Update methods === */
    public override void Update(double dt)
    {
        UpdateProjectiles();
        UpdateEnemies(dt);
        for (int i = 0; i < Particles.Count; i++)
        {
            Particles[i].Update(dt);
            if (!Particles[i].Visible)
                Particles.Remove(Particles[i]);
        }
        UpdateDrops();
        foreach (var t in Decorations)
        {
            if (t is TileShower shower)
            {
                shower.Update(dt);
            }
        }

        if (IsEndRoom && !exitGenerated && Enemies.Count == 0)
            GenerateExit();
    }
    protected void UpdateDrops()
    {
        foreach (var d in Drops)
            d.Update(this);
    }
    protected void UpdateProjectiles()
    {
        for (int i = Projectiles.Count - 1; i >= 0; i--)
        {
            Projectiles[i].Update();

            if (ObjectCollision.CircleCircleCollision(Projectiles[i], player))
            {
                float excessDmg = player.RecieveDmg(Projectiles[i]);
                Projectiles[i].SetDamage(excessDmg);
                if (Projectiles[i].Damage <= 0)
                    Projectiles.RemoveAt(i);
                continue;
            }
            if (ShouldCollideAt(Projectiles[i].GetCircleCenter(), true))
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
            if (ShouldCollideAt(player.Projectiles[i].GetCircleCenter(), true))
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

                        player.Kill(Enemies[j].XpValue);
                        foreach (Item item in Enemies[j].Drop(1))
                            Drops.Add(item);
                        Enemies.RemoveAt(j);
                    }
                    if (!player.Inventory.GetEffect().Contains(EffectTypes.PIERCING) && !player.Inventory.GetEffect().Contains(EffectTypes.EXPLOSIVE))
                    {
                        player.Projectiles[i].SetDamage(excessDmg);
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
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].Update(player.Position + player.Size / 2, dt);
            Enemies[i].Move(this);

            if (Enemies[i].ReadyToAttack())
            {
                foreach (var projectile in Enemies[i].Attack())
                {
                    Projectiles.Add(projectile);
                }
            }
        }
        bool isTriangle = false, isCat = false, isSoldier = false, isPolhreich = false, isJirka = false, isZdena = false;
        foreach (var e in Enemies)
        {
            if (e is EnemyTriangle)
                isTriangle = true;
            if (e is EnemyCat)
                isCat = true;
            if (e is EnemySoldier)
                isSoldier = true;
            if (e is EnemyPolhreich)
                isPolhreich = true;
            if (e is EnemyJirka)
                isJirka = true;
            if (e is EnemyZdena)
                isZdena = true;
        }
        if (!isTriangle && EnemyTriangle.Sfx.State == Microsoft.Xna.Framework.Audio.SoundState.Playing)
            EnemyTriangle.Sfx.Stop();
        if (!isCat && EnemyCat.vibeSfxInstance.State == Microsoft.Xna.Framework.Audio.SoundState.Playing)
            EnemyCat.vibeSfxInstance.Stop();
        if(isSoldier)
            EnemySoldier.UpdateSfx(dt);
            else
            EnemySoldier.StopSfx();

        if (isPolhreich)
            EnemyPolhreich.UpdateSfx(dt);
        else
            EnemyPolhreich.StopSfx();

        if (isJirka)
            EnemyJirka.UpdateSfx(dt);
        else
            EnemyJirka.StopSfx();

        if (isZdena)
            EnemyZdena.UpdateSfx(dt);
        else
            EnemyZdena.StopSfx();
    }

    /* === Generation methods === */
    protected abstract void GenerateEnemies();
    protected void GenerateEnemies(int roomWeight)
    {

        List<Enemy> chosenEnemies = new List<Enemy>();
        if (EnemyPool.Count == 0)
        {
            if (validEnemies.Count == 0)
                return;
            // get 2 types of enemies
            EnemyPool = [
                (Enemy)validEnemies[Random.Shared.Next(validEnemies.Count)].Clone(),
                (Enemy)validEnemies[Random.Shared.Next(validEnemies.Count)].Clone()
            ];
        }

        // 1 enemy for 'concentration' tiles
        int weight = 0;
        int weightCap = 7;
        if (roomWeight > weightCap)
            roomWeight = weightCap;

        while (weight < roomWeight)
        {
            Enemy e = (Enemy)EnemyPool[Random.Shared.Next(EnemyPool.Count)].Clone();
            chosenEnemies.Add(e);
            weight += (int)e.Weight;
        }

        foreach (var enemy in chosenEnemies)
        {

            // max 150 tries
            for (int i = 0; i < 150; i++)
            {
                Vector2 spawnPos = new Vector2(
                    Random.Shared.Next(50 * ((int)Dimensions.X - 3)) + 50,
                    Random.Shared.Next(50 * ((int)Dimensions.Y - 3)) + 50);

                if (Doors.Any(d => ((d.DoorTpPosition * 50) - spawnPos).Length() < 100))
                    continue;

                if (!ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), enemy.Size.ToPoint())))
                {
                    enemy.Position = spawnPos;
                    AddEnemy(enemy);
                    break;
                }
            }
        }
    }

    protected virtual void GeneratePassive() { }

    protected int GetValidPositionCount()
    {
        // Generate random enemies
        int validPositions = 0;

        for (int x = 0; x < Dimensions.X; x++)
            for (int y = 0; y < Dimensions.Y; y++)
                validPositions += (Floor[x, y]?.DoCollision ?? false) || (Decorations[x, y]?.DoCollision ?? false) ? 0 : 1;
        return validPositions;
    }

    protected virtual void GenerateBase() { GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC); }
    protected virtual void GenerateBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        ClearEnemies();
        ClearProjectiles();
        ClearDrops();

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
        if (Doors == null)
            throw new ArgumentNullException("This room does not have any doors!");

        Random rand = new Random();

        foreach (TileDoor door in Doors)
        {
            if (door.Sprite != TextureManager.GetTexture("doorBoss"))
                door.SetDoorType(doors);

            bool validPosition = false;
            // tp pos
            int _x = 0, _y = 0;
            int tpX = _x, tpY = _y;

            while (!validPosition)
            {
                _x = rand.Next((int)Dimensions.X - 2) + 1;
                _y = rand.Next((int)Dimensions.Y - 2) + 1;

                tpX = _x;
                tpY = _y;

                switch (door.Direction)
                {
                    case Directions.LEFT: tpX = 1; break;
                    case Directions.RIGHT: tpX = (int)Dimensions.X - 2; break;
                    case Directions.DOWN: tpY = (int)Dimensions.Y - 2; break;
                    case Directions.UP: tpY = 1; break;
                }

                if (Decorations[tpX, tpY] == null && !Floor[tpX, tpY].DoCollision)
                    validPosition = true;
            }

            door.DoorTpPosition = new Vector2(tpX, tpY);
            // Place the door and assign a valid teleport position
            switch (door.Direction)
            {
                case Directions.LEFT:
                    Decorations[0, _y] = door;
                    break;
                case Directions.RIGHT:
                    Decorations[(int)Dimensions.X - 1, _y] = door;
                    break;
                case Directions.UP:
                    Decorations[_x, 0] = door;
                    break;
                case Directions.DOWN:
                    Decorations[_x, (int)Dimensions.Y - 1] = door;
                    break;
            }
        }
    }
    public virtual void GenerateExit()
    {
        exitGenerated = true;
        Random rand = new Random();
        int _x = rand.Next((int)Dimensions.X - 2) + 1;
        int _y = rand.Next((int)Dimensions.Y - 2) + 1;

        bool validPosition = false;
        // tp pos
        int tpX = _x, tpY = _y;
        int dir = rand.Next(4);

        while (!validPosition)
        {
            _x = rand.Next((int)Dimensions.X - 2) + 1;
            _y = rand.Next((int)Dimensions.Y - 2) + 1;

            tpX = _x;
            tpY = _y;

            switch (dir)
            {
                case 0: tpX = 1; break;
                case 1: tpX = (int)Dimensions.X - 2; break;
                case 2: tpY = (int)Dimensions.Y - 2; break;
                case 3: tpY = 1; break;
            }

            if (Decorations[tpX, tpY] == null && !Floor[tpX, tpY].DoCollision)
                validPosition = true;
        }

        switch (dir)
        {
            case 0:
                Decorations[0, _y] = new TileExit();
                break;
            case 1:
                Decorations[(int)Dimensions.X - 1, _y] = new TileExit();
                break;
            case 2:
                Decorations[_x, 0] = new TileExit();
                break;
            case 3:
                Decorations[_x, (int)Dimensions.Y - 1] = new TileExit();
                break;
        }
    }

    /// <summary>
    /// Generate the dimensions for a room
    /// </summary>
    /// <param name="sMin">Min value for smaller side</param>
    /// <param name="sMax">Max value for smaller side/param>
    /// <param name="bMax">Max value for bigger side/param>
    protected virtual void GenerateDimensions(int sMin, int sMax, int bMax)
    {
        if (sMin > sMax)
            throw new Exception("sMax must be bigger than sMin!");
        if (sMax > bMax)
            throw new Exception("bMax must be bigger than sMax!");

        direction = (Directions)Random.Shared.Next(4);
        int smaller = Random.Shared.Next(sMin, sMax);
        int bigger = Random.Shared.Next(smaller, bMax);

        if (direction == Directions.LEFT || direction == Directions.RIGHT)
            // Horizontal
            Dimensions = new Vector2(bigger, smaller);
        else
            // vertical
            Dimensions = new Vector2(smaller, bigger);
    }
    protected virtual void GenerateDimensions((int sMin, int sMax, int bMax) dim) { GenerateDimensions(dim.sMin, dim.sMax, dim.bMax); }

    /* === Clear methods === */
    public override void Reset()
    {
        ClearRoom();
        if (!IsGenerated)
            Generate();
        GenerateEnemies();
        GeneratePassive();
    }
    public virtual void ClearDrops()
    {
        Drops.Clear();
    }
    public virtual void ClearEnemies()
    {
        Enemies.Clear();
    }
    public virtual void ClearEntities()
    {
        Entities.Clear();
    }
    public virtual void ClearProjectiles()
    {
        Projectiles.Clear();
        player.Projectiles.Clear();
    }
    public virtual void ClearRoom()
    {
        ClearProjectiles();
        ClearEnemies();
        ClearEntities();
        ClearDrops();
    }

    public virtual void AddEnemy(Enemy enemy)
    {
        enemy.InitStats(Storyline.Difficulty);
        Enemies.Add(enemy);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (Item item in Drops)
            item.Draw(spriteBatch);
        foreach (Enemy enemy in Enemies)
            enemy.Draw(spriteBatch);
        foreach (var e in Entities)
            e.Draw(spriteBatch);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Projectile projectile in Projectiles)
            projectile.Draw(spriteBatch);
        foreach (Particle particle in Particles)
            particle.Draw(spriteBatch);
    }
    public Vector2 IconBaseSize = new Vector2(10);
    public virtual void DrawMinimapIcon(SpriteBatch spriteBatch, Vector2 position, float scale = 20f, bool active = false)
    {
        int width = (int)(IconBaseSize.X * scale);
        int height = (int)(IconBaseSize.Y * scale);

        // Draw the black filled rectangle (inside of the room)
        spriteBatch.Draw(SpriteBlacksquare, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);

        // Draw the white outline
        // Top border
        spriteBatch.Draw(SpriteWhitesquare, new Rectangle((int)position.X, (int)position.Y, width, 1), active ? Color.Aqua : Color.White);
        // Bottom border
        spriteBatch.Draw(SpriteWhitesquare, new Rectangle((int)position.X, (int)(position.Y + height - 1), width, 1), active ? Color.Aqua : Color.White);
        // Left border
        spriteBatch.Draw(SpriteWhitesquare, new Rectangle((int)position.X, (int)position.Y, 1, height), active ? Color.Aqua : Color.White);
        // Right border
        spriteBatch.Draw(SpriteWhitesquare, new Rectangle((int)(position.X + width - 1), (int)position.Y, 1, height), active ? Color.Aqua : Color.White);
    }


}