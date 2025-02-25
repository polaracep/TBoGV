using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TBoGV;

public class MinigameRooted : Minigame
{
	private int smashCount = 0;
	private const int RequiredSmashCount = 2;
	private const int RequiredKeys = 3;
	static SpriteFont MiddleFont;
	static SpriteFont LargerFont;
	static SoundEffect rootTap = SoundManager.GetSound("bouchaniDoKorenu");
	public static minigameState State;
	private Color hintColor = Color.White;
	private double timeSinceLastHint = 0;
	private KeyboardState prevKeyboardState;

	public MinigameRooted(Action onSuccess)
	{
		State = minigameState.ONGOING;
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial16");
		OnSuccess = onSuccess;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
		int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

		string hintText = "!!!MACKEJ HODNE KLAVES!!!";
		Vector2 hintSize = LargerFont.MeasureString(hintText);
		Vector2 hintPosition = new Vector2((screenWidth - hintSize.X) / 2, (screenHeight - hintSize.Y) * 3 / 4);

		// Draw text outline
		Color outlineColor = Color.Black;
		Vector2 offset = new Vector2(1, 1);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(-offset.X, -offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(offset.X, -offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(-offset.X, offset.Y), outlineColor);
		spriteBatch.DrawString(LargerFont, hintText, hintPosition + new Vector2(offset.X, offset.Y), outlineColor);

		// Draw the main text
		spriteBatch.DrawString(LargerFont, hintText, hintPosition, hintColor);
	}

	public override void Update(KeyboardState keyboardState, double dt)
	{
		UpdateState(keyboardState);
		prevKeyboardState = keyboardState;
		timeSinceLastHint += dt;
		if (timeSinceLastHint > 0.5)
		{
			timeSinceLastHint = 0;
			hintColor = (hintColor == Color.White) ? Color.Red : Color.White;
		}
	}

	public override void UpdateState(KeyboardState keyboardState)
	{
		if (DetectKeySmash(keyboardState))
		{
			smashCount++;
			rootTap.Play();
		}
		if (smashCount >= RequiredSmashCount)
			State = minigameState.SUCCESS;
		base.UpdateState(keyboardState);
	}

	private bool DetectKeySmash(KeyboardState keyboardState)
	{
		if(prevKeyboardState.GetPressedKeyCount() < 3) 
			return keyboardState.GetPressedKeyCount() >= RequiredKeys;
		return false;
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
