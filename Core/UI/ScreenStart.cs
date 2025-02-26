using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

class ScreenStart : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Viewport viewport;
    private Button startButton;
    private Button settingsButton;

    private Texture2D logo = TextureManager.GetTexture("tbogv");
    private string headline = "TBoGV";
    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        startButton = new Button("Jedeeem!", LargerFont, () =>
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

        // mezera mezi cudilky

        // center
        int menuX = viewport.Width / 2;
        int menuY = viewport.Height / 2;

        int hp = menuY / 3;

        _spriteBatch.Begin();
        Vector2 headlineSize = LargerFont.MeasureString(headline);
        Vector2 headlinePos = new Vector2(menuX - (headlineSize.X / 2), menuY - 40);
        // _spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

        int logoW = viewport.Width / 2;
        int logoY = (int)((float)logoW / logo.Width * logo.Height);
        Rectangle rect = new Rectangle(menuX - (logoW / 2), menuY - hp - (logoY / 2), logoW, logoY);
        _spriteBatch.Draw(logo, rect, Color.White);

        Rectangle buttonRect = startButton.GetRect();
        startButton.Position = new Vector2(
            menuX - (buttonRect.Width / 2),
            menuY + hp
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