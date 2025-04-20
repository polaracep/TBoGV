using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class ScreenStart : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Viewport viewport;
    private Button startButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button exitButton;

    private Texture2D logo = TextureManager.GetTexture("tbogv");

    public ScreenStart(GraphicsDeviceManager graphics) : base(graphics)
    {
        startButton = new Button("Jedeeem!", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = ScreenManager.ScreenGame;
            GameManager.playtimeStopwatch.Start();
        });
        settingsButton = new Button("Šteluj", LargerFont, () =>
        {
            ScreenManager.ScreenSettings.LastScreen = TBoGVGame.screenCurrent;
            TBoGVGame.screenCurrent = ScreenManager.ScreenSettings;
        });
        creditsButton = new Button("Tvořiči", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = new ScreenCredits(graphics, this);
        });
        exitButton = new Button("Vypadni!!", LargerFont, () =>
        {
            GameManager.Exit();
        });
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        viewport = graphics.GraphicsDevice.Viewport;
        // pozadi
        _spriteBatch.Begin(blendState: BlendState.Opaque);
        _spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
            new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);

        _spriteBatch.End();
        // mezera mezi cudilky

        // center
        int menuX = viewport.Width / 2;
        int menuY = viewport.Height / 2;

        int hp = viewport.Height / 6;
        int logoW = viewport.Width / 2;
        int logoY = (int)((float)logoW / logo.Width * logo.Height);

        _spriteBatch.Begin();
        // logo
        _spriteBatch.Draw(logo, new Rectangle(menuX - (logoW / 2), menuY - hp - (logoY / 2), logoW, logoY), Color.White);

        // start button
        startButton.Position = new Vector2(menuX - (startButton.GetRect().Width / 2), viewport.Height * 60 / 100);
        startButton.Draw(_spriteBatch);

        // settings button
        settingsButton.Position = new Vector2(menuX - (settingsButton.GetRect().Width / 2), viewport.Height * 70 / 100);
        settingsButton.Draw(_spriteBatch);

        // credits button
        creditsButton.Position = new Vector2(menuX - (creditsButton.GetRect().Width / 2), viewport.Height * 80 / 100);
        creditsButton.Draw(_spriteBatch);

        exitButton.Position = new Vector2(menuX - (exitButton.GetRect().Width / 2), viewport.Height * 90 / 100);
        exitButton.Draw(_spriteBatch);

        _spriteBatch.End();

    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        MouseState mouseState = Mouse.GetState();
        startButton.Update(mouseState);
        settingsButton.Update(mouseState);
        creditsButton.Update(mouseState);
        exitButton.Update(mouseState);
    }
}