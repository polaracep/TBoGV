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
    public DialogueElement CurrentElement
    {
        get => dialogue[index];
        protected set => CurrentElement = value;
    }
    protected int index = 0;
    public int Length { get; protected set; }
    public Dialogue()
    {
        ParseDialogue();
    }

    public void ParseDialogue()
    {
        JsonElement story = root.GetProperty("story");
        JsonElement choices = root.GetProperty("choices");

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
                val.EnumerateObject().ToList().RemoveAll(x => x.Name == "question");
                // rest will be the options
                val.EnumerateObject().ToList().ForEach(x => pairs.Add(x.Name, x.Value.GetString()));

                dialogue.Add(new DialogueElement(question.GetString(), pairs));
            }
        }
    }

    public bool Advance()
    {
        DialogueElement element;
        try
        {
            element = dialogue[index];
        }
        catch
        {
            return false;
        }
        index++;
        return true;
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

    public DialogueElement(string text, Dictionary<string, string> choices)
    {
        Text = text;
        Choices = choices;
    }
    public DialogueElement(string text)
    {
        Text = text;
    }
}