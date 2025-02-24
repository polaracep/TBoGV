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

    private List<Minigame> miniGames = new List<Minigame>();

    private UI UI;
    private MouseState mouseState;
    private KeyboardState keyboardState;
    private KeyboardState previousKeyboardState;

    private Song Song;

    public bool openShop = false;
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
        _camera = new Camera(graphics.GraphicsDevice.Viewport, (int)(activePlace.Dimensions.X * Tile.GetSize().X), (int)(activePlace.Dimensions.Y * Tile.GetSize().Y));
        _camera.SetCenter(activePlace.Dimensions * Tile.GetSize() / 2);
        inGameMenu = effectMenu = new InGameMenuEffect(graphics.GraphicsDevice.Viewport);
        levelUpMenu = new InGameMenuLevelUp(graphics.GraphicsDevice.Viewport);
        deathMenu = new InGameMenuDeath(graphics.GraphicsDevice.Viewport);
        shopMenu = new InGameMenuShop(graphics.GraphicsDevice.Viewport);

        deathMenu.ResetLevel = () => Reset();
        deathMenu.PassTest = () => Revive();
        itemJournalMenu = new InGameMenuItemJournal(graphics.GraphicsDevice.Viewport);

        // In-game Soundtrack
        Song = SongManager.GetSong("soundtrack");
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop();
        }
        // MediaPlayer.Play(Song);
        MediaPlayer.Volume = 0.1f;
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        //_spriteBatch.Begin(blendState: BlendState.Opaque);
        //// _spriteBatch.Draw(TextureManager.GetTexture("gymvod"), Vector2.Zero, Color.White);
        //_spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
        //    new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        //_spriteBatch.End();

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

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        if (Storyline.CurrentLevel?.ActiveRoom != activePlace && player.IsPlaying)
            activePlace = Storyline.CurrentLevel.ActiveRoom;

        previousKeyboardState = keyboardState;
        mouseState = Mouse.GetState();
        keyboardState = Keyboard.GetState();
        int levelStatsCount = 0;
        itemJournalMenu.Update(graphics.GraphicsDevice.Viewport, player, mouseState, keyboardState, gameTime);
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
            shopMenu.OpenMenu();
            openShop = false;
        }
        if (KeyReleased(Keys.Tab))
        {
            if (!levelUpMenu.Active && !deathMenu.Active)
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

        if (keyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
        {
            player.LevelChanged = true;
            if (!player.IsPlaying)
            {
                Storyline.NextLevel();
            }
        }

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
            player.Update(keyboardState, mouseState, _camera.Transform, activePlace, graphics.GraphicsDevice.Viewport);
            activePlace.Update(gameTime);
            UI.Update(player, graphics);
            _camera.SetCenter(activePlace.Dimensions * Tile.GetSize() / 2);
            _camera.Update(player.Position + player.Size / 2);
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Song);
            }
        }
        else
        {
            inGameMenu.Update(graphics.GraphicsDevice.Viewport, player, mouseState, keyboardState, gameTime);
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }

        foreach (Minigame miniGame in miniGames)
            miniGame.Update(keyboardState, gameTime);
        if (player.Inventory.GetEffect().Contains(EffectTypes.ROOTED) && MinigameRooted.State != minigameState.ONGOING)
            miniGames.Add(new MinigameRooted(() => player.Inventory.RemoveEffect(new EffectRooted(1))));
        for (int i = 0; i < miniGames.Count; i++)
            if (miniGames[i].GetState() != minigameState.ONGOING)
                miniGames.RemoveAt(i);

    }

    public bool KeyReleased(Keys key)
    {
        return previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }
    void Reset()
    {
        player.Heal((uint)player.MaxHp);
        deathMenu.Active = false;
        player.LastRecievedDmgTime = DateTime.UtcNow;
    }
    void Revive()
    {
        deathMenu.Active = false;
        player.Heal(3);
        player.LastRecievedDmgTime = DateTime.UtcNow;
    }
}
