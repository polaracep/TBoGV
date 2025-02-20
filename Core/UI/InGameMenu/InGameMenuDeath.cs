using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
internal class InGameMenuDeath : InGameMenu
{
	private static Viewport Viewport;
	private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
	private static SpriteFont LargerFont;
	private static Texture2D SpriteCooked;
	private Rectangle buttonBounds;
	private string chosenDeathMessage = "";
	private bool isHovered = false;
	private MinigameKomisionalky minigame;
	private MouseState previousMouseState;
	private bool minigameCompleted = false;
	private bool minigameSuccess = false;

	private Button buttonResetLevel;
	private Button buttonPassTest;

	public Action ResetLevel;
	public Action PassTest;

	private readonly List<string> deathMessages = new List<string>
	{
		"Zapl jsi Youtube shorts",
		"Nemel jsi se divat na posledni skibidi toilet episodu",
		"Zoned out - mozna zkus spat vic nez 2h",
		"Nepozdravil jsi vratnou",
		"Nezavrel jsi dvere pred skolnikem",
		"Nepipl jsi si u vydeje",
		"Mikes te videl s mobilem",
		"Potkal jsi Prochazkovou",
		"Neprisel jsi na hodinu Predescu",
		"O prestavce jsi poslal pivko a nabils",
	};

	public InGameMenuDeath(Viewport viewport)
	{
		Viewport = viewport;
		SpriteBackground = TextureManager.GetTexture("blackSquare");
		LargerFont = FontManager.GetFont("Arial16");
		SpriteCooked = TextureManager.GetTexture("wheat4");
		Active = false;

		buttonResetLevel = new Button("Opakovat rocnik", MiddleFont, () => ResetLevel());
		buttonPassTest = new Button("Slozit komisionalky", MiddleFont, () => PassTest());
	}

	private void GenerateDeathMessage()
	{
		Random random = new Random();
		chosenDeathMessage = deathMessages[random.Next(deathMessages.Count)];
	}

	public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, GameTime gameTime)
	{
		base.Update(viewport, player, mouseState, keyboardState, gameTime);

		if (!Active)
			return;

		if (!minigameCompleted)
		{
			minigame.Update(keyboardState, gameTime);
		}
		else
		{
			if (!minigameSuccess)
				buttonResetLevel.Update(mouseState);
			else
				buttonPassTest.Update(mouseState);
		}
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

		int menuWidth = Math.Max((int)descSize.X + 40, 400);
		int menuHeight = (int)(headlineSize.Y + descSize.Y + imageHeight + buttonHeight + 5 * margin + 100);
		menuHeight = Math.Min(menuHeight, Viewport.Height - 100);
		int menuX = (Viewport.Width - menuWidth) / 2;
		int menuY = (Viewport.Height - menuHeight) / 2;

		Rectangle menuBackground = new Rectangle(menuX, menuY, menuWidth, menuHeight);
		spriteBatch.Draw(SpriteBackground, menuBackground, Color.Black * 0.7f);

		Vector2 headlinePos = new Vector2(
			menuBackground.X + (menuBackground.Width - headlineSize.X) / 2,
			menuBackground.Y + margin
		);
		spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

		Vector2 descPos = new Vector2(
			menuBackground.X + (menuBackground.Width - descSize.X) / 2,
			headlinePos.Y + headlineSize.Y + 10
		);
		spriteBatch.DrawString(MiddleFont, chosenDeathMessage, descPos, Color.White);

		Vector2 imagePos = new Vector2(
			menuBackground.X + (menuBackground.Width - imageWidth) / 2,
			descPos.Y + descSize.Y + 10
		);
		Rectangle imageBounds = new Rectangle((int)imagePos.X, (int)imagePos.Y, imageWidth, imageHeight);
		spriteBatch.Draw(SpriteCooked, imageBounds, Color.White);
		minigame.PositionOffset = new Vector2(0, (int)imagePos.Y+ imageHeight-Viewport.Height/2 + 20);
		minigame.Draw(spriteBatch);

		if (minigameCompleted)
		{
			Button button = minigameSuccess ? buttonPassTest : buttonResetLevel;

			Vector2 buttonSize = new Vector2(button.GetRect().Width, button.GetRect().Height);
			button.Position = new Vector2(menuBackground.X + (menuBackground.Width - (int)buttonSize.X) / 2,
				imageBounds.Y + imageBounds.Height + margin + 100);
			button.Draw(spriteBatch);
		}
	}

	public void OpenMenu()
	{
		GenerateDeathMessage();
		minigame = new MinigameKomisionalky(() => OnSuccess(), () => OnFailure(), 1);
		minigameCompleted = false;
		Active = true;
	}

	private void OnSuccess()
	{
		minigameCompleted = true;
		minigameSuccess = true;
	}

	private void OnFailure()
	{
		minigameCompleted = true;
		minigameSuccess = false;
	}
}
