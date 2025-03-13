using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
public class InGameMenuBet : InGameMenu
{
    private Player player;

    private Slider betSlider;
    private Button confirmButton;

    // Fonts for title, bet value, and button text.
    private SpriteFont titleFont = FontManager.GetFont("Arial24");
    private SpriteFont betFont = FontManager.GetFont("Arial16");
    private SpriteFont buttonFont = FontManager.GetFont("Arial16");

    private EntityGambler gambler;
    // Menu title text.
    private readonly string titleText = "Generační bohatství";

    public InGameMenuBet(Viewport viewport, Player player, EntityGambler gambler)
    {
        this.viewport = viewport;
        this.player = player;
        this.gambler = gambler;

        // Set a background overlay texture (using the already loaded "blackSquare" texture).
        SpriteBackground = TextureManager.GetTexture("blackSquare");

        // Slider dimensions.
        int sliderWidth = 300;
        int sliderHeight = 20;
        // Create the slider: range from 0 to the player's money.
        betSlider = new Slider(0, player.Coins, 0, sliderWidth, sliderHeight, "", betFont, (value) => { });

        betSlider.Position = new Vector2((viewport.Width - sliderWidth) / 2, prcY(40));

        confirmButton = new Button("Pošli to tam", buttonFont, () =>
        {
            player.Coins -= (int)betSlider.Value;
            gambler.SetBet((int)betSlider.Value);
            ScreenManager.ScreenGame.OpenDialogue(new DialogueBasic(DialogueManager.GetDialogue("gamblerPlaced").RootElement, gambler.Name, gambler.GetSprite()));
        });

        confirmButton.Position = new Vector2(
            (viewport.Width - confirmButton.GetRect().Width) / 2,
            betSlider.Position.Y + sliderHeight + 30
        );
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);

        betSlider.Update(mouseState);
        confirmButton.Update(mouseState);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Text nahore
        Vector2 titleSize = titleFont.MeasureString(titleText);
        Vector2 titlePos = new Vector2((viewport.Width - titleSize.X) / 2, prcY(12));
        spriteBatch.DrawString(titleFont, titleText, titlePos, Color.White);

        // slider
        betSlider.Draw(spriteBatch);

        // kolik vsazim
        string betDisplay = "Tvoje sázka: " + ((int)betSlider.Value).ToString();
        Vector2 betDisplaySize = betFont.MeasureString(betDisplay);
        Vector2 betDisplayPos = new Vector2((viewport.Width - betDisplaySize.X) / 2, betSlider.Position.Y - betDisplaySize.Y - 10);
        spriteBatch.DrawString(betFont, betDisplay, betDisplayPos, Color.White);

        // confirm cudlik
        confirmButton.Draw(spriteBatch);
    }
}