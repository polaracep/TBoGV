using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;

namespace TBoGV;

public class MinigameRick : Minigame
{
	private Viewport Viewport;
	private int smashCount = 0;
	private const int RequiredSmashCount = 8;
	static SpriteFont MiddleFont;
	static SpriteFont LargerFont;
	static SoundEffect rickroll = SoundManager.GetSound("neverGonnaGiveUUp");

	static SoundEffectInstance rickrollInstance = rickroll.CreateInstance();
	public static minigameState State;
	private Color hintColor = Color.White;
	private double timeSinceLastHint = 0;
	private KeyboardState prevKeyboardState;

	static Texture2D Spritesheet = TextureManager.GetTexture("richardSpritesheet");
	static SoundEffect SfxRickroll = SoundManager.GetSound("rickroll");
	float Scale;
	int frameWidth = 420;
	int frameHeight = 454;
	int frameCount = 28;
	int frameColumns = 5;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	double frameSpeed = SfxRickroll.Duration.TotalMilliseconds / (27 * 4);
	public MinigameRick(Action onSuccess)
	{
		State = minigameState.ONGOING;
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial16");
		OnSuccess = onSuccess;
		rickrollInstance.IsLooped = true;
		rickrollInstance.Volume = Settings.SfxVolume;
		rickrollInstance.Play();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		int row = currentFrame / frameColumns;
		int col = currentFrame % frameColumns;
		int screenWidth = Viewport.Width;
		int screenHeight = Viewport.Height;
		Scale = (float)screenHeight / Math.Max(frameWidth, frameHeight);
		Rectangle sourceRect = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
		float alpha = MathHelper.Clamp(1f - (float)smashCount / RequiredSmashCount, 0f, 1f);
		byte alphaByte = (byte)(alpha * 255);
		byte colorByte = (byte)(255 * alpha); // Scale RGB by alpha to avoid brightness boost


		spriteBatch.Draw(
			Spritesheet,
			new Rectangle(
				(int)((screenWidth - frameWidth * Scale) / 2), 0,
				screenHeight, (int)(frameWidth * Scale)
			),
			sourceRect,
			new Color(colorByte, colorByte, colorByte, alphaByte) // Scale RGB by alpha
		);





		string hintText = "!!! mackej E pro utek!!!";
		Vector2 hintSize = LargerFont.MeasureString(hintText);
		Vector2 hintPosition = new Vector2((screenWidth - hintSize.X) / 2, (screenHeight - hintSize.Y) * 3 / 4);

		Color outlineColor = Color.Black;
		Vector2 offset = new Vector2(1, 1);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(-offset.X, -offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(offset.X, -offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(-offset.X, offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(offset.X, offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition, hintColor);
	}

	public override void Update(Viewport viewport, KeyboardState keyboardState, double dt)
	{
		Viewport = viewport;
		UpdateAnimation(dt);
		if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E))
		{
			smashCount++;
		}

		if (smashCount >= RequiredSmashCount)
		{
			State = minigameState.SUCCESS;
			rickrollInstance.Stop();
			OnSuccess.Invoke();
		}

		prevKeyboardState = keyboardState;
		timeSinceLastHint += dt;
		if (timeSinceLastHint > 500)
		{
			timeSinceLastHint = 0;
			hintColor = (hintColor == Color.White) ? Color.Red : Color.White;
		}
	}
	private void UpdateAnimation(double dt)
	{
		lastFrameChangeElapsed += dt;
		if (lastFrameChangeElapsed > frameSpeed)
		{
			currentFrame = (currentFrame + 1) % frameCount;
			lastFrameChangeElapsed = 0;
		}
	}
	public override minigameState GetState()
	{
		return State;
	}

	public override Rectangle GetRect()
	{
		throw new NotImplementedException();
	}
}
