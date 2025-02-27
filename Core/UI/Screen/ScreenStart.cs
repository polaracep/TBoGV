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

    private Texture2D logo = TextureManager.GetTexture("tbogv");
    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        startButton = new Button("Jedeeem!", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = ScreenManager.ScreenGame;
            TBoGVGame.screenCurrent.BeginRun(graphics);
        });
        settingsButton = new Button("Šteluj", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = ScreenManager.ScreenSettings;
            TBoGVGame.screenCurrent.BeginRun(graphics);
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
        startButton.Position = new Vector2(menuX - (startButton.GetRect().Width / 2), menuY + hp);
        startButton.Draw(_spriteBatch);

        // settings button
        settingsButton.Position = new Vector2(menuX - (settingsButton.GetRect().Width / 2), menuY + (2 * hp));
        settingsButton.Draw(_spriteBatch);

        _spriteBatch.End();

    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        MouseState mouseState = Mouse.GetState();
        startButton.Update(mouseState);
        settingsButton.Update(mouseState);
    }
}