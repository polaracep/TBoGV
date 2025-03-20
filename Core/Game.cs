using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class TBoGVGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static Screen screenCurrent;
	private bool _isFullScreen = false;
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

        GameManager.Start(this);
        ScreenManager.Init(_graphics);
        screenCurrent = ScreenManager.ScreenStart;

    }

    protected override void Update(GameTime gameTime)
    {
		KeyboardState keyboardState = Keyboard.GetState();
		if (keyboardState.IsKeyDown(Keys.F11))
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
		_graphics.IsFullScreen = _isFullScreen;
		_graphics.ApplyChanges();
	}
}

public static class GameManager
{
    public static Game Game;
    public static Player Player;
    public static Viewport Viewport;

    public static void Start(Game game)
    {
        Game = game;
        Player = new Player();
        Viewport = game.GraphicsDevice.Viewport;
    }

    public static void Exit()
    {
        Game.Exit();
    }
}