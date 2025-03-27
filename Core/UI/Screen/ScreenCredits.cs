using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class ScreenCredits : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    private Button escapeButton;
    public Screen LastScreen;
    private KeyboardState previousKeyboardState;
    private List<string> credits = [
        "Developeři / Textury",
        "Pavel Škop",
        "Adam Beyer",
        "",
        "Dále děkujeme Anežce, Vítkovi a Kosťovi za pomoc s texturami <3",
        "oprcam ti dite",
    ];

    private int maxCreditsWidth;

    protected ScreenCredits(GraphicsDeviceManager graphics) : base(graphics)
    {
        Viewport v = graphics.GraphicsDevice.Viewport;
        escapeButton = new Button("Zpět", LargerFont, () =>
        {
            Settings.Save();
            TBoGVGame.screenCurrent = LastScreen;
        });
        maxCreditsWidth = (int)MiddleFont.MeasureString(credits.MaxBy(x => x.Count())).X;
    }
    public ScreenCredits(GraphicsDeviceManager graphics, Screen lastScreen) : this(graphics)
    {
        LastScreen = lastScreen;
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        Viewport viewport = graphics.GraphicsDevice.Viewport;
        int creditsStartY = viewport.Height / 100 * 30;

        _spriteBatch.Begin();

        // bg
        _spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
            new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        Rectangle backgroundRect = new Rectangle(
            ((viewport.Width - maxCreditsWidth) / 2) - 10, creditsStartY - 10,
            maxCreditsWidth + 20, 10 + viewport.Height / 100 * 5 * credits.Count());

        _spriteBatch.Draw(TextureManager.GetTexture("blackSquare"), backgroundRect, Color.Black * 0.5f);

        string header = "Lidé zodpovědní za tento skvost!";
        Vector2 textPosition = new Vector2((viewport.Width - LargerFont.MeasureString(header).X) / 2, viewport.Height / 12);
        _spriteBatch.DrawString(LargerFont, header, textPosition, Color.White);


        for (int i = 0; i < credits.Count; i++)
        {
            string str = credits[i];
            _spriteBatch.DrawString(MiddleFont, str, new Vector2((viewport.Width - MiddleFont.MeasureString(str).X) / 2, creditsStartY + viewport.Height / 100 * 5 * i), Color.White);
        }



        // esc
        escapeButton.Position = new Vector2(viewport.Width / 10, viewport.Height / 10);
        escapeButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            escapeButton.OnClick();
        }

        MouseState mouseState = Mouse.GetState();
        escapeButton.Update(mouseState);

        previousKeyboardState = keyboardState;
    }
}