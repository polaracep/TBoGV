using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class TBoGVGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static Screen screenCurrent;
    public TBoGVGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
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

        GameManager.Start(this);
        ScreenManager.Init(_graphics);
        screenCurrent = ScreenManager.ScreenStart;

    }

    protected override void Update(GameTime gameTime)
    {
        // exit coded
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();
        screenCurrent.Update(gameTime, _graphics);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        screenCurrent.Draw(_spriteBatch, _graphics);
        base.Draw(gameTime);
    }
}

public static class GameManager
{
    public static Game Game;
    public static Player Player;

    public static void Start(Game game)
    {
        Game = game;
        Player = new Player();
    }

    public static void Exit()
    {
        Game.Exit();
    }
}