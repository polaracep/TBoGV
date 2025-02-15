using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
namespace TBoGV;

internal class ScreenGame : Screen
{
    private Player player;
    private Camera _camera;
    private Level CurrentLevel;
    private InGameMenu inGameMenu;
    private InGameMenuEffect effectMenu;
    private InGameMenuLevelUp levelUpMenu;
    private UI UI;
    private MouseState mouseState;
    private KeyboardState keyboardState;
    private int Frame;
    private Song Song;

    public ScreenGame()
    {
        Frame = 0;
    }

    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        player = new Player();

        List<Room> rL = new List<Room> {
            new RoomEmpty(new Vector2(10, 10), player),
            new RoomEmpty(new Vector2(7, 10), player),
            new RoomEmpty(new Vector2(10, 7), player),
            new RoomEmpty(new Vector2(7, 7), player)
        };

        CurrentLevel = new Level(player, rL, 6);

        UI = new UI();
        _camera = new Camera(graphics.GraphicsDevice.Viewport, (int)(CurrentLevel.ActiveRoom.Dimensions.X * Tile.GetSize().X), (int)(CurrentLevel.ActiveRoom.Dimensions.Y * Tile.GetSize().Y));
        inGameMenu = effectMenu = new InGameMenuEffect(graphics.GraphicsDevice.Viewport);
        levelUpMenu = new InGameMenuLevelUp(graphics.GraphicsDevice.Viewport);

        // check the current state of the MediaPlayer.
        Song = SongManager.GetSong("soundtrack");
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop(); // stop current audio playback if playing or paused.
        }

        // Play the selected song reference.
        MediaPlayer.Play(Song);
        MediaPlayer.Volume = 0.01f;
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        //_spriteBatch.Begin(blendState: BlendState.Opaque);
        //// _spriteBatch.Draw(TextureManager.GetTexture("gymvod"), Vector2.Zero, Color.White);
        //_spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
        //    new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        //_spriteBatch.End();

        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        CurrentLevel.ActiveRoom.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        _spriteBatch.Begin();
        UI.Draw(_spriteBatch);
		player.Inventory.Draw(_spriteBatch);
        if (inGameMenu.Active)
        {
            inGameMenu.Draw(_spriteBatch);
        }

        _spriteBatch.End();
    }
    public override void LoadContent()
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        previousKeyboardState = keyboardState;
        mouseState = Mouse.GetState();
        keyboardState = Keyboard.GetState();
        int levelStatsCount = 0;
        foreach (var item in player.LevelUpStats)
        {
            levelStatsCount += (int)player.LevelUpStats[item.Key];
        }
        if (player.Level != levelStatsCount && !levelUpMenu.Active)
        {
            inGameMenu = levelUpMenu;
            levelUpMenu.OpenMenu();
        }

        if (KeyReleased(Keys.Escape))
        {
            effectMenu.Active = !effectMenu.Active;
            if (!levelUpMenu.Active)
                inGameMenu = effectMenu;
        }

        if (!inGameMenu.Active)
        {
            player.Update(keyboardState, mouseState, _camera.Transform, CurrentLevel.ActiveRoom, graphics.GraphicsDevice.Viewport);
            CurrentLevel.ActiveRoom.Update();
            UI.Update(player, graphics);
            _camera.Update(player.Position + player.Size / 2);
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(Song);
        }
        else
        {
            inGameMenu.Update(graphics.GraphicsDevice.Viewport, player, mouseState);
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }
    }
    KeyboardState previousKeyboardState;

    public bool KeyReleased(Keys key)
    {
        return previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }
}
