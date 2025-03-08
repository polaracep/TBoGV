using System;
using System.Text.Json;
using TBoGV;

public class DialogueIntro : Dialogue
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("intro").RootElement;

    public DialogueIntro() : base()
    {
        Actions.Add(1, () => Console.WriteLine("A"));

    }
}