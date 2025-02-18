using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public class MinigameRooted : Minigame
{
	private int smashCount = 0;
	private const int RequiredSmashCount = 2;
	private const int RequiredKeys = 3;
	static SpriteFont MiddleFont;
	static SpriteFont LargerFont;
	static Texture2D SpriteBackground;
	public static minigameState State;
	private Color hintColor = Color.White;
	private TimeSpan timeSinceLastHint = TimeSpan.Zero;
	public MinigameRooted(Action onSuccess)
	{
		State = minigameState.ONGOING;
		SpriteBackground = TextureManager.GetTexture("blackSquare");
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial16");
		OnSuccess = onSuccess;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{

		int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;






		string headline = "Korenovy vezen";
		string hintText = "!!!MACKEJ HODNE KLAVES!!!";

		Vector2 headlineSize = LargerFont.MeasureString(headline);
		Vector2 hintSize = MiddleFont.MeasureString(hintText);

		int rectWidth = (int)Math.Max(headlineSize.X, hintSize.X) + 20;
		int rectHeight = (int)(headlineSize.Y + hintSize.Y + 20);

		int rectX = (screenWidth - rectWidth) / 2;
		int rectY = (screenHeight - rectHeight)*3 / 4;

		spriteBatch.Draw(SpriteBackground, new Rectangle(rectX, rectY, rectWidth, rectHeight), new Color(0, 0, 0, 200));
		int Ypos = rectY +5;
		Vector2 headlinePosition = new Vector2((screenWidth - headlineSize.X) / 2, Ypos);
		Ypos += (int)headlineSize.Y + 5;
		Vector2 hintPosition = new Vector2((screenWidth - hintSize.X) / 2, Ypos); // Placing hint below description

		spriteBatch.DrawString(LargerFont, headline, headlinePosition, Color.White);
		spriteBatch.DrawString(MiddleFont, hintText, hintPosition, hintColor);  // Drawing the hint with dynamic color
	}


	public override void Update(KeyboardState keyboardState, GameTime gameTime)
	{
		UpdateState(keyboardState);

		// Accumulate elapsed time
		timeSinceLastHint += gameTime.ElapsedGameTime;

		// If more than 1 second has passed, change the hint color
		if (timeSinceLastHint.TotalSeconds > 0.5)
		{
			timeSinceLastHint = TimeSpan.Zero;
			hintColor = (hintColor == Color.White) ? Color.Red : Color.White;
		}
	}

	public override void UpdateState(KeyboardState keyboardState)
	{
		if (smashCount >= RequiredSmashCount)
			State = minigameState.SUCCESS;

		if (State == minigameState.SUCCESS)
			OnSuccess();

		if (DetectKeySmash(keyboardState))
			smashCount++;
	}

	private bool DetectKeySmash(KeyboardState keyboardState)
	{
		return keyboardState.GetPressedKeyCount() >= RequiredKeys;
	}

	public override minigameState GetState()
	{
		return State;
	}
}
