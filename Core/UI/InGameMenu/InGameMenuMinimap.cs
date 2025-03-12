﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class InGameMenuMinimap : InGameMenu
{
    private static Viewport Viewport;
    private static Level LevelToDraw = Storyline.CurrentLevel;
    private static Texture2D WhiteSquare = TextureManager.GetTexture("whiteSquare");
    private KeyboardState previousKeyboardState;

    public InGameMenuMinimap(Viewport viewport, Player player)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");

        LevelToDraw = Storyline.CurrentLevel;

        if (LevelToDraw == null || LevelToDraw.RoomMap == null) return;
        if (player.Inventory.GetEffect().Contains(EffectTypes.MAP_REVEAL))
            foreach (var r in Storyline.CurrentLevel.RoomMap)
                if (r != null)
                    r.IsVisited = true;
        int mapWidth = LevelToDraw.RoomMap.GetLength(0);
        int mapHeight = LevelToDraw.RoomMap.GetLength(1);
        Vector2 roomBossLoc = new Vector2(-1);

        for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
                if (LevelToDraw.RoomMap[x, y] is RoomBoss)
                    roomBossLoc = new Vector2(x, y);


        if (roomBossLoc.X >= 0 && roomBossLoc.Y >= 0)
        {
            List<Vector2> directions = new List<Vector2>() { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
            foreach (var direction in directions)
            {
                Vector2 index = direction + roomBossLoc;
                int x = (int)index.X;
                int y = (int)index.Y;
                if (x > mapWidth - 1 || x < 0 || y > mapHeight - 1 || y < 0)
                    continue;
                Room room = LevelToDraw.RoomMap[x, y];
                if (room == null) continue;
                if (room.IsVisited)
                {
                    LevelToDraw.RoomMap[(int)roomBossLoc.X, (int)roomBossLoc.Y].IsVisited = true;
                    break;
                }
            }
        }
    }
    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);
        if (previousKeyboardState.IsKeyDown(Keys.M) && keyboardState.IsKeyUp(Keys.M))
        {
            CloseMenu.Invoke();
            return;
        }
        previousKeyboardState = keyboardState;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        LevelToDraw = Storyline.CurrentLevel;

        if (LevelToDraw == null || LevelToDraw.RoomMap == null) return;

        int mapWidth = LevelToDraw.RoomMap.GetLength(0);
        int mapHeight = LevelToDraw.RoomMap.GetLength(1);

        float scaleFactor = 4;

        float totalWidth = mapWidth * 10 * scaleFactor + (mapWidth - 1) * 10;
        float totalHeight = mapHeight * 10 * scaleFactor + (mapHeight - 1) * 10;

        Vector2 minimapPos = new Vector2(
            (Viewport.Width - totalWidth) / 2 - (10 * scaleFactor + 10),
            (Viewport.Height - totalHeight) / 2
        );

        //spriteBatch.Draw(SpriteBackground, new Rectangle(minimapPos.ToPoint(), new Point((int)totalWidth, (int)totalHeight)),Color.White);
        for (int y = 0; y < mapHeight; y++)
        {
            Vector2 rowStartPos = new Vector2(
    (Viewport.Width - totalWidth) / 2 - (10 * scaleFactor + 10),
    minimapPos.Y);
            for (int x = 0; x < mapWidth; x++)
            {
                Room room = LevelToDraw.RoomMap[x, y];
                minimapPos.X += 10 * scaleFactor + 10;
                if (room == null || !room.IsVisited) continue;

                Vector2 roomSize = new Vector2(10) * scaleFactor;
                room.DrawMinimapIcon(spriteBatch, minimapPos, scaleFactor, room == LevelToDraw.ActiveRoom);

                foreach (TileDoor door in room.Doors)
                {
                    Vector2 roomCenter = minimapPos + roomSize / 2;
                    Vector2 lineStartPos = Vector2.Zero, lineSize = Vector2.Zero, lineDirection = Vector2.Zero;
                    switch (door.Direction)
                    {
                        case Directions.LEFT:
                            lineDirection = new Vector2(-1,0);
                            lineDirection.Normalize();
                            lineStartPos = roomCenter + (roomSize / 2 * lineDirection) + 5 * lineDirection;
                            lineSize = new Vector2(5,1);
                            break;

                        case Directions.RIGHT:
                            lineDirection = new Vector2(1, 0);
                            lineDirection.Normalize();
                            lineStartPos = roomCenter + (roomSize / 2 * lineDirection);
                            lineSize = new Vector2(5, 1);
                            break;

                        case Directions.UP:
                            lineDirection = new Vector2(0, -1);
                            lineDirection.Normalize();
                            lineStartPos = roomCenter + (roomSize / 2 * lineDirection) + 5 * lineDirection;
                            lineSize = new Vector2(1, 5);
                            break;

                        case Directions.DOWN:
                            lineDirection = new Vector2(0, 1);
                            lineDirection.Normalize();
                            lineStartPos = roomCenter + (roomSize / 2 * lineDirection);
                            lineSize = new Vector2(1, 5);
                            break;
                    }
                    spriteBatch.Draw(WhiteSquare,new Rectangle(lineStartPos.ToPoint(), lineSize.ToPoint()),Color.White);
                }
            }
            minimapPos = rowStartPos + new Vector2(0, 10*scaleFactor + 10);
        }
    }

}
