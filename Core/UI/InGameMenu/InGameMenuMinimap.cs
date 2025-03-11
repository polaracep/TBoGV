using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class InGameMenuMinimap : InGameMenu
{
    private static Viewport Viewport;
    private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    private static Level LevelToDraw = Storyline.CurrentLevel;
    private static Texture2D WhiteSquare = TextureManager.GetTexture("whiteSquare");
    private KeyboardState previousKeyboardState;

    public InGameMenuMinimap(Viewport viewport, Player player)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        if (player.Inventory.GetEffect().Contains(EffectTypes.MAP_REVEAL))
            foreach (var r in Storyline.CurrentLevel.RoomMap)
                if (r != null)
                    r.IsVisited = true;
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



        float totalWidth = 0;
        float totalHeight = 0;
        Vector2[,] roomSizes = new Vector2[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Room room = LevelToDraw.RoomMap[x, y];
                if (room == null) continue;

                float roomWidth = room.IconBaseSize.X;
                float roomHeight =room.IconBaseSize.Y;
                roomSizes[x, y] = new Vector2(roomWidth, roomHeight);
            }
        }
        for (int x = 0; x < mapWidth; x++)
        {
            float columnHeight = 0;
            for (int y = 0; y < mapHeight; y++)
            {
                columnHeight += roomSizes[x, y].Y;
            }
            totalHeight = Math.Max(totalHeight, columnHeight);
        }
        for (int y = 0; y < mapHeight; y++)
        {
            float rowWidth = 0;
            for (int x = 0; y < mapHeight; y++)
            {
                rowWidth += roomSizes[x, y].X;
            }
            totalWidth = Math.Max(totalWidth, rowWidth);
        }
        float scaleFactor = 4;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                roomSizes[x, y] *= scaleFactor;
            }
        }

        Vector2 minimapPos = new Vector2(
            (Viewport.Width - totalWidth * scaleFactor) / 2,
            (Viewport.Height - totalHeight * scaleFactor) / 2
        );

        for (int y = 0; y < mapHeight; y++)
        {
                    Vector2 rowStartPos = new Vector2(
            (Viewport.Width - totalWidth * scaleFactor) / 2,
            minimapPos.Y
        ); 
            for (int x = 0; x < mapWidth; x++)
            {
                Room room = LevelToDraw.RoomMap[x, y];
                minimapPos.X += 10 * scaleFactor + 10;
                if (room == null || !room.IsVisited) continue;

                Vector2 roomSize = roomSizes[x, y];
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
