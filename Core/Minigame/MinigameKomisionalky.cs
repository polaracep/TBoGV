using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBoGV;

public class MinigameKomisionalky : Minigame
{
	private const int BarWidth = 300;
	private const int BarHeight = 20;
	private const int ArrowWidth = 10;
	private const int SegmentWidth = 8;
	private int greenStart, greenEnd;
	private int arrowPosition;
	private int successCount = 0;
	private const int RequiredSuccesses = 3;
	private float arrowSpeed = 500f;
	private bool movingRight = true;
	private bool highlightSegment = false;
	private double highlightTime = 0;
	private int highlightedSegment = -1;
	private KeyboardState prevKeyboardState;
	static Texture2D SpriteForeground = TextureManager.GetTexture("whiteSquare");
	static Texture2D SpriteArrow = TextureManager.GetTexture("arrow");

	public static minigameState State;
	public Vector2 PositionOffset;
	public Vector2 Size;

	public MinigameKomisionalky(Action onSuccess, Action onFailure, int difficulty)
	{
		State = minigameState.ONGOING;
		OnSuccess = onSuccess;
		OnFailure = onFailure;

		GenerateGreenZone(difficulty);

		arrowPosition = 0;
		int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

		Size = new Vector2((screenWidth - BarWidth) / 2, screenHeight / 2 + ArrowWidth);
	}

	private void GenerateGreenZone(int difficulty)
	{
		int maxSegments = BarWidth / SegmentWidth;
		int greenSegments = Math.Max(1, difficulty * maxSegments / 10);
		int startSegment = new Random().Next(0, maxSegments - greenSegments);

		greenStart = startSegment * SegmentWidth;
		greenEnd = greenStart + (greenSegments * SegmentWidth);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		int barX = (int)Size.X;
		int barY = (int)Size.Y - ArrowWidth;

		// Draw the bar segments with black borders
		for (int i = 0; i < BarWidth; i += SegmentWidth)
		{
			Color segmentColor = (i >= greenStart && i < greenEnd) ? Color.Green : Color.Red;
			if (highlightSegment && i != highlightedSegment)
			{
				segmentColor = Color.LightGray;
			}

			spriteBatch.Draw(SpriteForeground, new Rectangle(barX + i + (int)PositionOffset.X, barY + (int)PositionOffset.Y, SegmentWidth, BarHeight), segmentColor);
			spriteBatch.Draw(SpriteForeground, new Rectangle(barX + i + (int)PositionOffset.X, barY + (int)PositionOffset.Y, SegmentWidth, 1), Color.Black); // Top border
			spriteBatch.Draw(SpriteForeground, new Rectangle(barX + i + (int)PositionOffset.X, barY + (int)PositionOffset.Y + BarHeight - 1, SegmentWidth, 1), Color.Black); // Bottom border
			spriteBatch.Draw(SpriteForeground, new Rectangle(barX + i + (int)PositionOffset.X, barY + (int)PositionOffset.Y, 1, BarHeight), Color.Black); // Left border
			spriteBatch.Draw(SpriteForeground, new Rectangle(barX + i + (int)PositionOffset.X + SegmentWidth - 1, barY + (int)PositionOffset.Y, 1, BarHeight), Color.Black); // Right border
		}

		// Draw the arrow
		spriteBatch.Draw(SpriteArrow, new Rectangle(barX + arrowPosition + (int)PositionOffset.X, barY - 10 + (int)PositionOffset.Y, ArrowWidth, 30), Color.White);
	}

	public override void Update(KeyboardState keyboardState, GameTime gameTime)
	{
		if (successCount >= RequiredSuccesses)
			State = minigameState.SUCCESS;

		base.UpdateState(keyboardState);
		float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

		if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
		{
			if (arrowPosition+ ArrowWidth / 2 >= greenStart && arrowPosition <= greenEnd - ArrowWidth/2)
			{
				successCount++;
				highlightedSegment = (arrowPosition / SegmentWidth) * SegmentWidth;
				highlightSegment = true;
				highlightTime = 0.2;
			}
			else
			{
				highlightedSegment = (arrowPosition / SegmentWidth) * SegmentWidth;
				highlightSegment = true;
				highlightTime = 0.2;
				State = minigameState.FAILURE;
			}
		}
		else 
		{
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
		}

		if (highlightSegment)
		{
			highlightTime -= elapsed;
			if (highlightTime <= 0)
			{
				highlightSegment = false;
				highlightedSegment = -1;
			}
		}

		prevKeyboardState = keyboardState;
	}

	public override minigameState GetState()
	{
		return State;
	}

	public override Rectangle GetRect()
	{
		return new Rectangle((int)PositionOffset.X, (int)PositionOffset.Y, (int)Size.X, (int)Size.Y);
	}
}
