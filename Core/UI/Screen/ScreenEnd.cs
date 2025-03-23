using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class ScreenEnd : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Texture2D Vyzo = TextureManager.GetTexture("vyzo");
    private Viewport viewport;
    private Button backButton;
    private Button continueButton;

    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        // Create a button to go back to the main menu.
        backButton = new Button("Zpátky do menu\n(odejít z GV)", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = ScreenManager.ScreenStart;
            FileHelper.ResetSaves();
        });
        continueButton = new Button("Pokračovat do vypadnutí\n(experimental endless)", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = ScreenManager.ScreenGame;
            Storyline.Endless = true;
        });
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        viewport = graphics.GraphicsDevice.Viewport;

        // Start the sprite batch.
        _spriteBatch.Begin();

        _spriteBatch.Draw(TextureManager.GetTexture("gymvod"), new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

        // Draw the "You Win!" message centered at the top third of the screen.
        string winText = "Gratuluju! Odmaturoval jsi!";
        Vector2 textSize = LargerFont.MeasureString(winText);
        Vector2 textPosition = new Vector2((viewport.Width - textSize.X) / 2, viewport.Height / 9);
        _spriteBatch.DrawString(LargerFont, winText, textPosition, Color.Gold);

        // Adjust the image size relative to the screen size.
        int imageWidth = viewport.Width / 4;
        int imageHeight = (int)(imageWidth * (float)Vyzo.Height / Vyzo.Width);
        int imageX = (viewport.Width - imageWidth) / 2;
        int imageY = (viewport.Height - imageHeight) / 2;
        _spriteBatch.Draw(Vyzo, new Rectangle(imageX, imageY, imageWidth, imageHeight), Color.White);


        // Position and draw the back button centered in the lower third.
        backButton.Position = new Vector2((viewport.Width - backButton.GetRect().Width) / 2, viewport.Height * 4 / 5);
        backButton.Draw(_spriteBatch);

        continueButton.Position = new Vector2((viewport.Width - backButton.GetRect().Width) / 2, backButton.Position.Y + continueButton.GetRect().Height + 20);
        continueButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        // Update button state based on mouse input.
        MouseState mouseState = Mouse.GetState();
        backButton.Update(mouseState);
        continueButton.Update(mouseState);
    }
}