using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBoGV;

public abstract class Minigame
{
	// Lambda property for success callback
	public Action? OnSuccess { get; set; }

	public abstract void Update(KeyboardState keyboardState, GameTime gameTime);
	public abstract void Draw(SpriteBatch spriteBatch);
	public abstract minigameState GetState();
	public virtual void UpdateState(KeyboardState keyboardState)
	{
		// If state changes to SUCCESS, invoke OnSuccess
		if (GetState() == minigameState.SUCCESS)
		{
			OnSuccess?.Invoke();
		}
	}
}

public enum minigameState : int
{
	SUCCESS = 1,
	FAILURE = 2,
	ONGOING = 3,
}
