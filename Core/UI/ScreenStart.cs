using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

class ScreenStart : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Viewport viewport;
    private Button startButton;


    private string headline = "TBoGV";
    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        startButton = new Button("test", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = new ScreenGame();
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

        // center
        int menuX = viewport.Width / 2;
        int menuY = viewport.Height / 2;

        _spriteBatch.Begin();
        Vector2 headlineSize = LargerFont.MeasureString(headline);
        Vector2 headlinePos = new Vector2(menuX - (headlineSize.X / 2), menuY - 40);
        _spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

        // Center the Start Game button at the bottom of the panel.
        // Assumes startGameButton is a Button instance (see Button.cs :contentReference[oaicite:2]{index=2}).
        Rectangle buttonRect = startButton.GetRect();
        startButton.Position = new Vector2(
            menuX - (buttonRect.Width / 2),
            menuY - buttonRect.Height + 40
        );
        startButton.Draw(_spriteBatch);
        _spriteBatch.End();

    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        MouseState mouseState = Mouse.GetState();
        startButton.Update(mouseState);
    }
}