using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace TBoGV;

public class MinigameKomisionalky : Minigame
{
	private const int BarWidth = 300;
	private const int BarHeight = 20;
	private const int ArrowWidth = 10;
	private int greenStart, greenEnd;
	private int arrowPosition;
	private int successCount = 0;
	private const int RequiredSuccesses = 3;
	private float arrowSpeed = 500f;
	private bool movingRight = true;
	private KeyboardState prevKeyboardState;
	static Texture2D SpriteForeground = TextureManager.GetTexture("whiteSquare");
	static Texture2D SpriteArrow = TextureManager.GetTexture("whiteSquare");

	public static minigameState State;
	private Texture2D barTexture;
	private Texture2D arrowTexture;

	public Vector2 Position;
	public Vector2 Size;

	public MinigameKomisionalky(Action onSuccess, Action onFailure)
	{
		State = minigameState.ONGOING;
		OnSuccess = onSuccess;
		OnFailure = onFailure;

		greenStart = BarWidth / 3;
		greenEnd = greenStart + (BarWidth / 3);

		arrowPosition = 0;
		int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

		Size = new Vector2((screenWidth - BarWidth) / 2, screenHeight / 2 + ArrowWidth);
	}
	public override void Draw(SpriteBatch spriteBatch)
	{

		int barX = (int)Size.X;
		int barY = (int)Size.Y- ArrowWidth;

		// Draw the bar
		spriteBatch.Draw(SpriteForeground, new Rectangle(barX, barY, BarWidth, BarHeight), Color.Red);
		spriteBatch.Draw(SpriteForeground, new Rectangle(barX + greenStart, barY, greenEnd - greenStart, BarHeight), Color.Green);

		// Draw the arrow
		spriteBatch.Draw(SpriteArrow, new Rectangle(barX + arrowPosition, barY - 10, ArrowWidth, 30), Color.White);
	}

	public override void Update(KeyboardState keyboardState, GameTime gameTime)
	{
		if (successCount >= RequiredSuccesses)
			State = minigameState.SUCCESS;

		base.UpdateState(keyboardState);
		if (State == minigameState.SUCCESS || State == minigameState.FAILURE)
			return;
		float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

		if (movingRight)
		{
			arrowPosition += (int)(arrowSpeed * elapsed);
			if (arrowPosition >= BarWidth - ArrowWidth)
			{
				movingRight = false;
			}
		}
		else
		{
			arrowPosition -= (int)(arrowSpeed * elapsed);
			if (arrowPosition <= 0)
			{
				movingRight = true;
			}
		}

		if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
		{
			if (arrowPosition >= greenStart && arrowPosition <= greenEnd - ArrowWidth)
			{
				successCount++;
			}
			else
				State = minigameState.FAILURE;
		}


		prevKeyboardState = keyboardState;
	}

	public override minigameState GetState()
	{
		return State;
	}

	public override Rectangle GetRect()
	{
		return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
	}
}
