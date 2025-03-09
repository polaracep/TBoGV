using System;
using System.Text.Json;
using TBoGV;

public class DialogueIntro : Dialogue
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("intro").RootElement;

    public DialogueIntro() : base()
    {
        Actions.Add(1, () =>
        {
            if (TBoGVGame.screenCurrent is ScreenGame)
            {
                ScreenGame sg = (ScreenGame)TBoGVGame.screenCurrent;
            }
        });

    }
}