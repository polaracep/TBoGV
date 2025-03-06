using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
class InGameMenuDeath : InGameMenu
{
	private static Viewport Viewport;
	private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
	private static SpriteFont LargerFont = FontManager.GetFont("Arial16");
	private static Texture2D SpriteCooked = TextureManager.GetTexture("wheat4");
	private string chosenDeathMessage = "";
	private MinigameKomisionalky minigame;
	private bool minigameCompleted = false;
	private bool minigameSuccess = false;
	private bool minigameRunning = false;
	private Button buttonResetLevel;
	private Button buttonPassTest;
	private Button buttonRunMinigame;

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
		Active = false;

		buttonRunMinigame = new Button("Pokusit se o komisionalky", MiddleFont, () => { minigameRunning = true; });
		buttonResetLevel = new Button("Opakovat rocnik", MiddleFont, () => ResetLevel());
		buttonPassTest = new Button("Slozit komisionalky", MiddleFont, () => PassTest());
	}

	private void GenerateDeathMessage()
	{
		chosenDeathMessage = deathMessages[Random.Shared.Next(deathMessages.Count)];
	}

	public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
	{
		base.Update(viewport, player, mouseState, keyboardState, dt);

		if (!Active)
			return;

		if (!minigameRunning)
			buttonRunMinigame.Update(mouseState);
		if (minigameRunning)
		{
			minigame.Update(viewport, keyboardState, dt);
		}
		if (minigameCompleted)
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
		int margin = prcY(2);

		int menuWidth = Math.Max((int)descSize.X + 40, 400);
		int menuHeight = (int)(headlineSize.Y + descSize.Y + imageHeight + buttonHeight + 5 * margin + 100);
		menuHeight = Math.Min(menuHeight, Viewport.Height - 100);
		int menuX = (Viewport.Width - menuWidth) / 2;
		int menuY = (Viewport.Height - menuHeight) / 2;

		// bg
		Rectangle menuBackground = new Rectangle(menuX, menuY, menuWidth, menuHeight);
		spriteBatch.Draw(SpriteBackground, menuBackground, Color.Black * 0.7f);

		// headline
		Vector2 headlinePos = new Vector2(
			menuBackground.X + (menuBackground.Width - headlineSize.X) / 2,
			menuBackground.Y + margin
		);
		spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

		// hlaska
		Vector2 descPos = new Vector2(
			menuBackground.X + (menuBackground.Width - descSize.X) / 2,
			headlinePos.Y + headlineSize.Y + 10
		);
		spriteBatch.DrawString(MiddleFont, chosenDeathMessage, descPos, Color.White);

		// vita
		Vector2 imagePos = new Vector2(
			menuBackground.X + (menuBackground.Width - imageWidth) / 2,
			descPos.Y + descSize.Y + 10
		);
		Rectangle imageBounds = new Rectangle((int)imagePos.X, (int)imagePos.Y, imageWidth, imageHeight);
		spriteBatch.Draw(SpriteCooked, imageBounds, Color.White);

		// minihra
		if (minigameRunning)
		{
			minigame.PositionOffset = new Vector2(0, (int)imagePos.Y + imageHeight - Viewport.Height / 2 + 20);
			minigame.Draw(spriteBatch);
		}
		else
		{
			Vector2 buttonSize = new Vector2(buttonRunMinigame.GetRect().Width, buttonRunMinigame.GetRect().Height);
			buttonRunMinigame.Position = new Vector2(menuBackground.X + (menuBackground.Width - (int)buttonSize.X) / 2,
				imageBounds.Y + imageBounds.Height + margin + 100);
			buttonRunMinigame.Draw(spriteBatch);
		}

		// hotovo
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
		minigame = new MinigameKomisionalky(() => OnSuccess(), () => OnFailure(), new Random().Next(-10, 5));
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
