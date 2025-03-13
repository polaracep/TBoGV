using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : Place
{
    protected bool IsFyjala = false;
    protected EntityGambler gambler = null;
    public Lobby(Player p)
    {
        player = p;
        Dimensions = new Vector2(15, 6);
        SpawnPos = new Vector2(Dimensions.X - 2, 2);
    }

    public override void Generate()
    {
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );
#if DEBUG
        AddDecoTile(new Vector2(3, 2), new TileTreasure());
#endif

        // Spawnpos + o jedno doprava
        AddDecoTile(SpawnPos + Vector2.UnitX, new TileStart(MathHelper.PiOver2));

        // kavovar
        AddDecoTile(new Vector2(1, 2), new TileCoffeeMachine(-MathHelper.PiOver2));

        // lednice
        AddDecoTile(new Vector2(1, Dimensions.Y - 2), new TileFridge(DecorationTypes.FRIDGE1, MathHelper.Pi));
        AddDecoTile(new Vector2(2, Dimensions.Y - 2), new TileFridge(DecorationTypes.FRIDGE2, MathHelper.Pi));
        AddDecoTile(new Vector2(3, Dimensions.Y - 2), new TileFridge(DecorationTypes.FRIDGE1, MathHelper.Pi));

        // lol
        AddDecoTile(new Vector2(5, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(5, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(6, Dimensions.Y - 3), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(6, Dimensions.Y - 2), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(7, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        AddDecoTile(new Vector2(7, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));

        AddDecoTile(new Vector2(9, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(9, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(10, Dimensions.Y - 3), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(10, Dimensions.Y - 2), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(11, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        AddDecoTile(new Vector2(11, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));

        player.Position = GetTileWorldPos(Vector2.One);
        GenerateSarka();
        IsGenerated = true;
    }

    private void GenerateSarka()
    {
        Entities.Add(new EntitySarka(GetTileWorldPos(new Vector2(1, 0))));
    }
    private void GenerateFyjala()
    {
        // 1/4 chance
        if (new Random().Next(4) != 1)
            return;

        IsFyjala = true;
        Entities.Add(new EntityFyjala(GetTileWorldPos(new Vector2(9, 4))));
        player.Inventory.AddEffect(new EffectFyjalovaDrahota(1));
    }
    private void GenerateGambler()
    {
        // 1/3 chance
        if (new Random().Next(3) != 1)
            return;

        gambler = new EntityGambler(GetTileWorldPos(new Vector2(5, 3)));
        Entities.Add(gambler);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (var e in Entities)
            e.Draw(spriteBatch);
        foreach (var i in Drops)
            i.Draw(spriteBatch);
    }

    public override void Update(double dt)
    {
        if (player.Inventory.GetEffect().Contains(EffectTypes.EXPENSIVE) && !IsFyjala)
            player.Inventory.RemoveEffect(new EffectFyjalovaDrahota(1));
        UpdateDrops();
    }
    protected void UpdateDrops()
    {
        foreach (var d in Drops)
            d.Update(this);
    }
    public override void Reset()
    {
        Drops.Clear();
        Enemies.Clear();

        Entities.Clear();
        IsFyjala = false;

        // negenerovat prvni v prvnim lobby
        if (Storyline.CurrentLevelNumber != 0)
        {
            GenerateFyjala();

            if (gambler != null && gambler.BetPlaced == true)
            {
                gambler.EvalBet();
                Entities.Add(gambler);
            }
            else if (gambler != null && gambler.BetPlaced == false)
                gambler = null;
            else
                GenerateGambler();
        }

        GenerateSarka();
        player.Inventory.RemoveEffect(new EffectFyjalovaDrahota(1));
    }
}