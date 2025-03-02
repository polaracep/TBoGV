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
    private InGameMenu inGameMenu;
    private InGameMenuEffect effectMenu;
    private InGameMenuLevelUp levelUpMenu;
    private InGameMenuDeath deathMenu;
    private InGameMenuItemJournal itemJournalMenu;
    private InGameMenuShop shopMenu;
    public bool openShop = false;
    private List<Minigame> miniGames = new List<Minigame>();

    private UI UI;
    private MouseState mouseState;
    private KeyboardState keyboardState;
    private KeyboardState previousKeyboardState;
    private Song Song;

    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        player = new Player();
        Storyline.Player = player;

        Storyline.GenerateStoryline();

        // CurrentLevel = new Level(player, rL, start, 6);
        //activePlace = CurrentLevel.ActiveRoom;
        lobby = new Lobby(player);
        activePlace = lobby;

        UI = new UI();
        _camera = new Camera();
        inGameMenu = effectMenu = new InGameMenuEffect(graphics.GraphicsDevice.Viewport);
        levelUpMenu = new InGameMenuLevelUp(graphics.GraphicsDevice.Viewport);
        deathMenu = new InGameMenuDeath(graphics.GraphicsDevice.Viewport);
        shopMenu = new InGameMenuShop(graphics.GraphicsDevice.Viewport);

        // deathMenu.ResetLevel = () => Reset();
        // lol
        deathMenu.ResetLevel = GameManager.Shutdown;
        deathMenu.PassTest = Revive;
        itemJournalMenu = new InGameMenuItemJournal(graphics.GraphicsDevice.Viewport);

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

        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        activePlace.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        _spriteBatch.Begin();
        UI.Draw(_spriteBatch);
        player.Inventory.Draw(_spriteBatch);
        if (inGameMenu.Active)
        {
            inGameMenu.Draw(_spriteBatch);
        }
        foreach (Minigame miniGame in miniGames)
            miniGame.Draw(_spriteBatch);
        _spriteBatch.End();
    }
    GameTime prevGameTime = new GameTime();
    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        double dt = gameTime.ElapsedGameTime.TotalMilliseconds;
        prevGameTime = gameTime;
        if (Storyline.CurrentLevel?.ActiveRoom != activePlace && player.IsPlaying)
            activePlace = Storyline.CurrentLevel.ActiveRoom;

        mouseState = Mouse.GetState();
        int levelStatsCount = 0;
        itemJournalMenu.Update(graphics.GraphicsDevice.Viewport, player, mouseState, keyboardState, dt);
        foreach (var item in player.LevelUpStats)
        {
            levelStatsCount += (int)player.LevelUpStats[item.Key];
        }
        if (player.Level != levelStatsCount && !levelUpMenu.Active)
        {
            inGameMenu = levelUpMenu;
            levelUpMenu.OpenMenu();
        }
        if (player.Hp < 1 && !deathMenu.Active)
        {
            inGameMenu = deathMenu;
            MinigameRooted.State = minigameState.SUCCESS;

            deathMenu.OpenMenu();
        }
        if (openShop && !shopMenu.Active)
        {
            inGameMenu = shopMenu;
            shopMenu.OpenMenu(player);
            openShop = false;
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
            player.LevelChanged = false;
        }


        if (!inGameMenu.Active)
        {
            player.Update(keyboardState, mouseState, _camera.Transform, activePlace, graphics.GraphicsDevice.Viewport, dt);
            activePlace.Update(dt);
            UI.Update(player, graphics);
            if (Settings.FixedCamera)
                _camera.SetCenter(graphics.GraphicsDevice.Viewport, player.Position);
            else
                _camera.SetCenter(graphics.GraphicsDevice.Viewport, activePlace.Dimensions * Tile.GetSize() / 2);
            _camera.Update(graphics.GraphicsDevice.Viewport, player.Position + player.Size / 2);

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
            inGameMenu.Update(graphics.GraphicsDevice.Viewport, player, mouseState, keyboardState, dt);
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
            miniGame.Update(keyboardState, dt);

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
            if (!levelUpMenu.Active && !deathMenu.Active && !shopMenu.Active)
            {
                inGameMenu = effectMenu;
                effectMenu.Active = !effectMenu.Active;
            }
        }
        if (KeyReleased(Keys.J) && MinigameRooted.State != minigameState.ONGOING)
        {
            if (!levelUpMenu.Active && !deathMenu.Active)
            {
                itemJournalMenu.ShowAll();
                inGameMenu = itemJournalMenu;
                itemJournalMenu.Active = !itemJournalMenu.Active;
            }
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
        if (keyboardState.IsKeyDown(Keys.R) && previousKeyboardState.IsKeyUp(Keys.R) && !inGameMenu.Active)
        {
            activePlace.Reset();
            shopMenu.ClearShop();
        }
        if (keyboardState.IsKeyDown(Keys.C) && previousKeyboardState.IsKeyUp(Keys.C) && !inGameMenu.Active)
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
        player.Heal((uint)player.MaxHp);
        deathMenu.Active = false;
        player.LastRecievedDmgElapsed = 0;
    }
    void Revive()
    {
        deathMenu.Active = false;
        player.Heal(3);
        player.LastRecievedDmgElapsed = 0;
    }
}
