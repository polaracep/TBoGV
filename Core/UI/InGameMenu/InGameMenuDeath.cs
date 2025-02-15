using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
// chatgpt generated - 2 lazy
internal class InGameMenuDeath : InGameMenu
{
    private static Viewport Viewport;
    private static SpriteFont MiddleFont;
    private static SpriteFont LargerFont;
    private static Texture2D SpriteCooked;
    private Rectangle buttonBounds;
    private string chosenDeathMessage = "";
    private bool isHovered = false;

    private MouseState previousMouseState;

    private readonly List<string> deathMessages = new List<string>
    {
        "Zapl jsi Youtube shorts",
        "Nemel jsi se divat na posledni skibidi toilet episodu",
        "Zoned out - mozna zkus spat vic nez 2h",
        "Nepozdravil jsi vratnou",
        "Nezavrel jsi dvere pred skolnikem",
        "Nepipl jsi si u vydeje",
        "Mikes te videl s telefonem",
        "Potkal jsi Prochazkovou",
        "Neprisel jsi na hodinu Predescu",

    };

    public InGameMenuDeath(Viewport viewport)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        MiddleFont = FontManager.GetFont("Arial12");
        LargerFont = FontManager.GetFont("Arial16");
        SpriteCooked = TextureManager.GetTexture("wheat4");
        Active = false;
    }

    private void GenerateDeathMessage()
    {
        Random random = new Random();
        chosenDeathMessage = deathMessages[random.Next(deathMessages.Count)];
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState)
    {
        base.Update(viewport, player, mouseState);

        if (!Active)
            return;

        isHovered = buttonBounds.Contains(mouseState.Position);

        if (isHovered &&
            previousMouseState.LeftButton == ButtonState.Pressed &&
            mouseState.LeftButton == ButtonState.Released)
        {
            ResetLevel();
        }

        previousMouseState = mouseState;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Active)
            return;

        base.Draw(spriteBatch);

        string headline = "Komisionalky";
        Vector2 headlineSize = LargerFont.MeasureString(headline);
        Vector2 descSize = MiddleFont.MeasureString(chosenDeathMessage);

        int imageWidth = 100, imageHeight = 150;
        int buttonHeight = (int)MiddleFont.MeasureString("Opakovat rocnik").Y + 10;
        int margin = 20;

        // Calculate menu dimensions
        int menuWidth = Math.Max((int)descSize.X + 40, 400);
        int menuHeight = (int)(headlineSize.Y + descSize.Y + imageHeight + buttonHeight + 5 * margin);

        // Ensure menu doesn't exceed viewport bounds
        menuHeight = Math.Min(menuHeight, Viewport.Height - 100);
        int menuX = (Viewport.Width - menuWidth) / 2;
        int menuY = (Viewport.Height - menuHeight) / 2;

        Rectangle menuBackground = new Rectangle(menuX, menuY, menuWidth, menuHeight);
        spriteBatch.Draw(SpriteBackground, menuBackground, Color.Black * 0.7f);

        // Headline position
        Vector2 headlinePos = new Vector2(
            menuBackground.X + (menuBackground.Width - headlineSize.X) / 2,
            menuBackground.Y + margin
        );
        spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

        // Description text
        Vector2 descPos = new Vector2(
            menuBackground.X + (menuBackground.Width - descSize.X) / 2,
            headlinePos.Y + headlineSize.Y + 10
        );
        spriteBatch.DrawString(MiddleFont, chosenDeathMessage, descPos, Color.White);

        // Image Position (now correctly centered)
        Vector2 imagePos = new Vector2(
            menuBackground.X + (menuBackground.Width - imageWidth) / 2,
            descPos.Y + descSize.Y + 10
        );
        Rectangle imageBounds = new Rectangle((int)imagePos.X, (int)imagePos.Y, imageWidth, imageHeight);
        spriteBatch.Draw(SpriteCooked, imageBounds, Color.White);

        // Button
        string buttonText = "Opakovat rocnik";
        Vector2 buttonSize = MiddleFont.MeasureString(buttonText);
        buttonBounds = new Rectangle(
            menuBackground.X + (menuBackground.Width - (int)buttonSize.X - 20) / 2,
            imageBounds.Y + imageBounds.Height + margin,
            (int)buttonSize.X + 20,
            buttonHeight
        );

        Color buttonColor = isHovered ? Color.Gray * 0.8f : Color.Black * 0.5f;
        spriteBatch.Draw(SpriteBackground, buttonBounds, buttonColor);

        Vector2 buttonTextPos = new Vector2(
            buttonBounds.X + (buttonBounds.Width - buttonSize.X) / 2,
            buttonBounds.Y + (buttonBounds.Height - buttonSize.Y) / 2
        );
        spriteBatch.DrawString(MiddleFont, buttonText, buttonTextPos, Color.White);
    }

    public void OpenMenu()
    {
        GenerateDeathMessage();
        Active = true;
    }

    private void ResetLevel()
    {
        Active = false;
    }
}
