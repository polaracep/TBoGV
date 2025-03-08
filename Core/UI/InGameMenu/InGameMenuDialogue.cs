using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class InGameMenuDialogue : InGameMenu
{
    JsonElement currentDialogue;
    public InGameMenuDialogue(string dialogueName, Texture2D LeftSprite, Texture2D RightSprite)
    {
        currentDialogue = DialogueManager.GetDialogue(dialogueName).RootElement;

        foreach (var item in currentDialogue.EnumerateArray())
        {

        }
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

}