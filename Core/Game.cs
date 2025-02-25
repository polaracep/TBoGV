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

        screenCurrent = new ScreenGame();

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
    }

    // Run after LoadContent
    protected override void BeginRun()
    {

        screenCurrent.BeginRun(_graphics);
        base.BeginRun();
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
