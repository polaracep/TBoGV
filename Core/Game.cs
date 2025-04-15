using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class TBoGVGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static Screen screenCurrent;
    public static bool _isFullScreen = false;

    public TBoGVGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.IsFullScreen = _isFullScreen;
        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureManager.Load(Content);
        SongManager.Load(Content);
        FontManager.Load(Content);
        SoundManager.Load(Content);
        DialogueManager.Load(Content);
        Settings.Load();
    }

    // Run after LoadContent
    protected override void BeginRun()
    {
        base.BeginRun();

        GameManager.Start(this, _graphics);
        ScreenManager.Init(_graphics);
        screenCurrent = ScreenManager.ScreenStart;
    }

    protected override void Update(GameTime gameTime)
    {
        if (screenCurrent is ScreenGame && !GameManager.playtimeStopwatch.IsRunning)
            GameManager.playtimeStopwatch.Start();
        if (screenCurrent is not ScreenGame && GameManager.playtimeStopwatch.IsRunning)
            GameManager.playtimeStopwatch.Stop();

        GameManager.Player.Playtime = TimeOnly.FromTimeSpan(GameManager.playtimeStopwatch.Elapsed);

        if (_isFullScreen != (bool)Settings.Fullscreen.Value)
        {
            ToggleFullScreen();
        }
        // exit coded
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();
        GameManager.Viewport = _graphics.GraphicsDevice.Viewport;
        screenCurrent.Update(gameTime, _graphics);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightBlue);
        screenCurrent.Draw(_spriteBatch, _graphics);
        base.Draw(gameTime);
    }
    private void ToggleFullScreen()
    {
        _isFullScreen = !_isFullScreen;
        Settings.Fullscreen.Value = _isFullScreen;
        Settings.Save();
        _graphics.IsFullScreen = _isFullScreen;
        _graphics.ApplyChanges();
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        base.OnExiting(sender, args);

#if DEBUG
        Console.WriteLine("Exiting!");
#endif
        var data = FileHelper.Load<Dictionary<string, object>>(Player.DataPath, SaveType.AUTO);

        if (data == null)
            return;
        if (data.TryGetValue("te", out object sTime))
        {
            data["te"] = GameManager.GetPlaytime();
        }
        else
        {
#if DEBUG
            Console.WriteLine("Failed saving time data");
#endif
        }
        FileHelper.Save(Player.DataPath, data, SaveType.AUTO);
    }
}

public static class GameManager
{
    public static TBoGVGame Game;
    public static Player Player;
    public static Viewport Viewport;
    public static Stopwatch playtimeStopwatch;
    private static TimeOnly startPlaytime;
    private static GraphicsDeviceManager graphics;
    public static void Start(TBoGVGame game, GraphicsDeviceManager graphics)
    {
        GameManager.graphics = graphics;
        Game = game;
        Player = new Player();
        Viewport = game.GraphicsDevice.Viewport;
        playtimeStopwatch = new();
        startPlaytime = Player.Playtime;
    }

    public static TimeOnly GetPlaytime() { return startPlaytime.Add(playtimeStopwatch.Elapsed); }

    public static void Exit()
    {
        Game.Exit();
    }

    public static void ResetPlaythrough()
    {
        Storyline.ResetStoryline();
        FileHelper.ResetSaves();

        startPlaytime = TimeOnly.MinValue;
        Player = new Player();
        playtimeStopwatch = new();
        Player.Save(SaveType.AUTO);
        ScreenManager.Init(graphics);
    }
}