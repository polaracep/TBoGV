using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class DialogueIntro : Dialogue
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("intro").RootElement;
    public override string NpcName { get; set; } = "Schovánek";
    public override Texture2D NpcSprite { get; set; } = TextureManager.GetTexture("wiseman");

    public DialogueIntro() : base()
    {
        Actions.Add(1, () =>
        {
            if (TBoGVGame.screenCurrent is ScreenGame)
            {
                ScreenGame sg = (ScreenGame)TBoGVGame.screenCurrent;
                sg.SendPlayerToTutorial();
            }
        });

    }
}