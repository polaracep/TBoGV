using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBoGV;

public abstract class Minigame
{
	public Action OnSuccess { get; set; }
	public Action OnFailure { get; set; }
	public abstract Rectangle GetRect();
	public abstract void Update(Viewport Viewport, KeyboardState keyboardState, double dt);
	public abstract void Draw(SpriteBatch spriteBatch);
	public abstract MinigameState GetState();
	public virtual void UpdateState(KeyboardState keyboardState)
	{
		if (GetState() == MinigameState.SUCCESS)
		{
			OnSuccess.Invoke();
		}
		if (GetState() == MinigameState.FAILURE)
		{
			OnFailure.Invoke();
		}
	}
}

public enum MinigameState : int
{
	SUCCESS = 1,
	FAILURE = 2,
	ONGOING = 3,
}
