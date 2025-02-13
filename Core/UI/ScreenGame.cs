using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV.Core;
using Microsoft.Xna.Framework.Media;
namespace TBoGV;

internal class ScreenGame : Screen
{
    private Player player;
    private RoomEmpty r;
    private Camera _camera;
    private InGameMenu inGameMenu;
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
        r = new RoomEmpty(new Vector2(10, 10), player);
        r.GenerateRoom();
        UI = new UI();
        _camera = new Camera(graphics.GraphicsDevice.Viewport, (int)(r.Dimensions.X * Tile.GetSize().X), (int)(r.Dimensions.Y * Tile.GetSize().Y));
        inGameMenu = new InGameMenuInventory(graphics.GraphicsDevice.Viewport);

        // check the current state of the MediaPlayer.
        Song = SongManager.GetSong("soundtrack");
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop(); // stop current audio playback if playing or paused.
        }

        // Play the selected song reference.
        MediaPlayer.Play(Song);
        MediaPlayer.Volume = 0.01f;

        Level one = new Level(player, 7, 10);
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        _spriteBatch.Begin(blendState: BlendState.Opaque);
        // _spriteBatch.Draw(TextureManager.GetTexture("gymvod"), Vector2.Zero, Color.White);
        _spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
            new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        _spriteBatch.End();

        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        r.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        _spriteBatch.Begin();
        UI.Draw(_spriteBatch);
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
        if (KeyReleased(Keys.Escape))
            inGameMenu.Active = !inGameMenu.Active;
        if (!inGameMenu.Active)
        {
            player.Update(keyboardState, mouseState, _camera.Transform, r);
            r.Update();
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
        Frame++;

        // Loop

    }
    KeyboardState previousKeyboardState;

    public bool KeyReleased(Keys key)
    {
        return previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }
}
