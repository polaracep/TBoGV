using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
namespace TBoGV;

public class ScreenGame : Screen
{
    private Player player;
    private Camera _camera;
    private Lobby lobby;
    private Place activePlace;
    private InGameMenu activeMenu = null;
    private List<Minigame> miniGames = new List<Minigame>();
    private Viewport _viewport;

    private UI UI;
    private MouseState mouseState;
    private KeyboardState keyboardState;
    private KeyboardState previousKeyboardState;
    private Song Song;

    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        player = new Player();

        player.Load(SaveType.USER);
        Storyline.Player = player;

        Storyline.GenerateStoryline();
        player.Save(SaveType.AUTO);

        lobby = new Lobby(player);
        activePlace = lobby;

        UI = new UI();
        _camera = new Camera();
        _viewport = graphics.GraphicsDevice.Viewport;

        InGameMenu.CloseMenu = () => activeMenu = null;

        InGameMenuDeath.ResetLevel = Reset;
        InGameMenuDeath.PassTest = Revive;

        // In-game Soundtrack
        Song = SongManager.GetSong("soundtrack");
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop();
        }
        // MediaPlayer.Play(Song);
        MediaPlayer.Volume = Settings.MusicVolume;
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        // _camera.SetCenter(graphics.GraphicsDevice.Viewport, activePlace.Dimensions * Tile.GetSize() / 2);
        _viewport = graphics.GraphicsDevice.Viewport;

        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        activePlace.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        _spriteBatch.Begin();
        UI.Draw(_spriteBatch);
        player.Inventory.Draw(_spriteBatch);
        if (activeMenu != null)
            activeMenu.Draw(_spriteBatch);
        foreach (Minigame miniGame in miniGames)
            miniGame.Draw(_spriteBatch);
        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        _viewport = graphics.GraphicsDevice.Viewport;

        double dt = gameTime.ElapsedGameTime.TotalMilliseconds;
        mouseState = Mouse.GetState();
        int levelStatsCount = 0;

        // check right activeplace
        if (Storyline.CurrentLevel?.ActiveRoom != activePlace && player.IsPlaying)
            activePlace = Storyline.CurrentLevel.ActiveRoom;

        // ingamemenu update
        if (activeMenu != null)
            activeMenu.Update(_viewport, player, mouseState, keyboardState, dt);

        // check for level up
        foreach (var item in player.LevelUpStats)
        {
            levelStatsCount += player.LevelUpStats[item.Key];
        }
        if (player.Level != levelStatsCount && activeMenu is not InGameMenuLevelUp)
        {
            activeMenu = new InGameMenuLevelUp(_viewport);
        }
        if (player.Hp < 1 && activeMenu is not InGameMenuDeath)
        {
            activeMenu = new InGameMenuDeath(_viewport);
            MinigameRooted.State = minigameState.SUCCESS;
        }

        UpdateKeyboard();

        // Level checker
        if (player.LevelChanged)
        {
            if (player.IsPlaying)
            {
                player.IsPlaying = false;
                activePlace = lobby;
            }
            else
            {
                player.IsPlaying = true;
                activePlace = Storyline.CurrentLevel.ActiveRoom;
            }
            InGameMenuShop.ResetShop();
            player.LevelChanged = false;
            player.Position = activePlace.SpawnPos * 50;
        }


        if (activeMenu == null)
        {
            player.Update(keyboardState, mouseState, _camera.Transform, activePlace, _viewport, dt);
            activePlace.Update(dt);
            UI.Update(player, graphics);
            if (Settings.FixedCamera)
                _camera.SetCenter(_viewport, player.Position);
            else
                _camera.SetCenter(_viewport, activePlace.Dimensions * Tile.GetSize() / 2);
            _camera.Update(_viewport, player.Position + player.Size / 2);

            // update volume
            MediaPlayer.Volume = Settings.MusicVolume;
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Song);
            }
        }
        else
        {
            activeMenu.Update(_viewport, player, mouseState, keyboardState, dt);
            UI.Update(player, graphics);
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }
        if (player.Inventory.GetEffect().Contains(EffectTypes.ROOTED) && MinigameRooted.State != minigameState.ONGOING)
            miniGames.Add(new MinigameRooted(() => player.Inventory.RemoveEffect(new EffectRooted(1))));
        if (player.Inventory.GetEffect().Contains(EffectTypes.RICKROLL) && MinigameRick.State != minigameState.ONGOING)
            miniGames.Add(new MinigameRick(() => player.Inventory.RemoveEffect(new EffectRickroll(1))));

        foreach (Minigame miniGame in miniGames)
            miniGame.Update(_viewport, keyboardState, dt);

        for (int i = 0; i < miniGames.Count; i++)
            if (miniGames[i].GetState() != minigameState.ONGOING)
                miniGames.RemoveAt(i);

    }

    protected void UpdateKeyboard()
    {
        previousKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            if (activeMenu == null)
                activeMenu = new InGameMenuEffect(player);
            else
                activeMenu = null;
        }
        if (KeyReleased(Keys.J) && MinigameRooted.State != minigameState.ONGOING)
        {
            if (activeMenu == null)
                activeMenu = new InGameMenuItemJournal(_viewport);
        }

#if DEBUG
        if (keyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
        {
            player.LevelChanged = true;
            if (!player.IsPlaying)
            {
                Storyline.NextLevel();
            }
        }
        if (keyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R) && activeMenu == null)
        {
            activePlace.Reset();
            InGameMenuShop.ResetShop();
        }
        if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C) && activeMenu == null)
        {
            activePlace.Enemies.Clear();
        }
#endif

    }

    public bool KeyReleased(Keys key)
    {
        return previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }
    void Reset()
    {
        activeMenu = null;
        player.Heal((uint)player.MaxHp);
        player.LastRecievedDmgElapsed = 0;
        player.Load(SaveType.AUTO);
        Storyline.FailedTimes++;
        Storyline.ResetLevel();

        if (Storyline.FailedTimes >= 3)
        {
            FileHelper.ResetSaves();
            Storyline.ResetStoryline();
            Storyline.CurrentLevelNumber = 0;
            player.Reset();
            lobby.Reset();
            TBoGVGame.screenCurrent = ScreenManager.ScreenDeath;
        }
        player.LevelChanged = true;
        player.IsPlaying = true;
    }
    void Revive()
    {
        activeMenu = null;
        player.Heal(3);
        player.LastRecievedDmgElapsed = 0;
    }
    public void OpenShop(ShopTypes shop)
    {
        activeMenu = shop switch
        {
            ShopTypes.SARKA => new InGameMenuShop(_viewport, player, ShopTypes.SARKA),
            ShopTypes.PERLOUN => new InGameMenuShop(_viewport, player, ShopTypes.PERLOUN),
            _ => throw new Exception("No shop provided"),
        };
    }
}
