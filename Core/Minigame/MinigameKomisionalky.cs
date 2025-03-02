using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Net.Mime.MediaTypeNames;

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
	private int RequiredSuccesses = 3;
	private float arrowSpeed = 500f;
	private bool movingRight = true;
	private bool highlightSegment = false;
	private double highlightTime = 0;
	private int highlightedSegment = -1;
	private KeyboardState prevKeyboardState;
	static Texture2D SpriteForeground = TextureManager.GetTexture("whiteSquare");
	static Texture2D SpriteArrow = TextureManager.GetTexture("arrow");
	static SpriteFont Font = FontManager.GetFont("Arial12");
	float elapsed;

	public static minigameState State;
	public Vector2 PositionOffset;
	public Vector2 Size;

	private string displayedText;
	public MinigameKomisionalky(Action onSuccess, Action onFailure, int difficulty)
	{
		State = minigameState.ONGOING;
		OnSuccess = onSuccess;
		OnFailure = onFailure;

		GenerateGreenZone(difficulty);
		RequiredSuccesses = Math.Max(3, 6 - difficulty);

		arrowPosition = 0;
		int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

		if (difficulty < -4)
			displayedText = "Fyzika\nDifficulty: hodne stesti";
		else if (difficulty >= -4 && difficulty < -1)
			displayedText = "Anglictina se Synkovou\nDifficulty: G_G";
		else if (difficulty >= -1 && difficulty < 1)
			displayedText = "Matematika\nDifficulty: hard";
		else if (difficulty == 1)
			displayedText = "Cestina\nDifficulty: normal";
		else if (difficulty == 2)
			displayedText = "Zsv\nDifficulty: 2 ez";
		else
			displayedText = "Telocvik\nDifficulty: free pass";

		displayedText += "\nMackej mezernik";


		Vector2 textSize = Font.MeasureString(displayedText);
		Size = new Vector2((screenWidth - BarWidth) / 2, screenHeight / 2 + ArrowWidth + textSize.Y);
	}

	private void GenerateGreenZone(int difficulty)
	{
		int maxSegments = BarWidth / SegmentWidth;
		int greenSegments = Math.Max(3, difficulty * maxSegments / 10);
		int startSegment = new Random().Next(0, maxSegments - greenSegments);

		greenStart = startSegment * SegmentWidth;
		greenEnd = greenStart + (greenSegments * SegmentWidth);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Vector2 textSize = Font.MeasureString(displayedText);
		int barX = (int)(Size.X);
		int barY = (int)(Size.Y - ArrowWidth);

		// Draw the text above the minigame

		Vector2 textPosition = new Vector2(barX + BarWidth / 2 - textSize.X / 2 + (int)PositionOffset.X, barY - textSize.Y + (int)PositionOffset.Y);
		spriteBatch.DrawString(Font, displayedText, textPosition, Color.White);

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
		spriteBatch.Draw(SpriteArrow, new Rectangle(barX + arrowPosition + (int)PositionOffset.X, barY - 10 + (int)PositionOffset.Y, ArrowWidth, 20), Color.White);

		// Draw the progress bar
		int progressBarWidth = (int)((float)successCount / RequiredSuccesses * BarWidth);
		spriteBatch.Draw(SpriteForeground, new Rectangle(barX + (int)PositionOffset.X, barY + BarHeight + 10 + (int)PositionOffset.Y, BarWidth, 5), Color.Gray);
		spriteBatch.Draw(SpriteForeground, new Rectangle(barX + (int)PositionOffset.X, barY + BarHeight + 10 + (int)PositionOffset.Y, progressBarWidth, 5), Color.Blue);
	}

	public override void Update(KeyboardState keyboardState, double dt)
	{
		if (successCount >= RequiredSuccesses)
			State = minigameState.SUCCESS;

		base.UpdateState(keyboardState);
		if (State == minigameState.FAILURE || State == minigameState.SUCCESS)
			return;

		elapsed = (float)dt / 1000;

		if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space) ||
			keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
		{
			if (arrowPosition + ArrowWidth / 2 >= greenStart && arrowPosition <= greenEnd - ArrowWidth / 2)
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
