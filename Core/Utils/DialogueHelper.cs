using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TBoGV;

public class DialogueHelper
{

}

public class Dialogue
{
    protected JsonElement root = DialogueManager.GetDialogue("testJson").RootElement;
    protected List<DialogueElement> dialogue = [];
    public Dictionary<int, Action> Actions = [];

    public DialogueElement CurrentElement
    {
        get => dialogue[index];
        protected set => CurrentElement = value;
    }
    protected int index = 0;
    public Dialogue()
    {
        Actions.Add(1, () => Console.WriteLine("A"));
        ParseDialogue();
    }

    public void Respond(string choice)
    {
        if (choice == "")
            return;
        JsonElement selected = root.GetProperty("choices").GetProperty(choice);
        if (selected.ValueKind != JsonValueKind.Array)
            throw new Exception("The response has to be an array");
        int insertPos = index + 1;
        foreach (var item in selected.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.Number)
            {
                dialogue.Insert(insertPos, new DialogueElement(Actions.GetValueOrDefault(item.GetInt16())));
            }
            else
                dialogue.Insert(insertPos, new DialogueElement(item.GetString()));

            insertPos++;
        }
        //  Advance();
    }

    public void ParseDialogue()
    {
        JsonElement story = root.GetProperty("story");

        foreach (JsonProperty e in story.EnumerateObject())
        {
            JsonElement val = e.Value;
            // npc talking
            if (val.ValueKind == JsonValueKind.Array)
            {
                val.EnumerateArray().ToList().ForEach(x => dialogue.Add(new DialogueElement(x.GetString())));
            }
            // your talking choices
            if (val.ValueKind == JsonValueKind.Object)
            {
                Dictionary<string, string> pairs = [];
                // get the question
                JsonElement question = val.GetProperty("question");
                // remove it 
                var options = val.EnumerateObject().ToList();
                options.RemoveAll(x => x.Name == "question");
                // rest will be the options
                options.ForEach(x => pairs.Add(x.Name, x.Value.GetString()));

                dialogue.Add(new DialogueElement(question.GetString(), pairs));
            }
        }
    }

    public bool Advance()
    {
        index++;
        return index < dialogue.Count;
    }


}

public class DialogueElement
{
    /// <summary>
    /// Text said by the npc
    /// </summary>
    public string Text { get; protected set; }
    /// <summary>
    /// Your answers:
    //  s1: Text, s2: reference to an answer.
    /// </summary>
    public Dictionary<string, string> Choices { get; protected set; }

    public Action Action { get; protected set; }

    public DialogueElement(string text, Dictionary<string, string> choices)
    {
        Text = text;
        Choices = choices;
    }
    public DialogueElement(string text)
    {
        Text = text;
    }
    public DialogueElement(Action action)
    {
        Action = action;
    }
}