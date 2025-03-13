using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;

public abstract class Dialogue
{
    protected abstract JsonElement root { get; set; }
    protected List<DialogueElement> dialogue = [];
    public Dictionary<int, Action> Actions = [];
    public abstract string NpcName { get; set; }
    // TODO: Schovanek
    public abstract Texture2D NpcSprite { get; set; }

    public Dialogue() { }
    public DialogueElement CurrentElement
    {
        get => dialogue[index];
        protected set => CurrentElement = value;
    }
    protected int index = 0;

    public void Respond(string choice)
    {
        JsonElement selected;
        if (!root.GetProperty("choices").TryGetProperty(choice, out selected))
            return;

        if (selected.ValueKind == JsonValueKind.Array)
            AddArray(selected, false);
        else if (selected.ValueKind == JsonValueKind.Object)
            AddQuestion(selected, index);
        else
            throw new Exception("Unknown kind");
    }

    public bool Advance()
    {
        index++;
        return index < dialogue.Count;
    }

    protected void ParseDialogue()
    {
        JsonElement story = root.GetProperty("story");

        foreach (JsonProperty e in story.EnumerateObject())
        {
            JsonElement val = e.Value;

            switch (val.ValueKind)
            {
                case JsonValueKind.Array:
                    AddArray(val, true);
                    break;
                case JsonValueKind.Object:
                    AddQuestion(val, null);
                    break;
                case JsonValueKind.Number:
                    dialogue.Add(new DialogueElement(Actions.GetValueOrDefault(val.GetInt32())));
                    break;
                default: break;
            }
        }
    }

    protected void AddArray(JsonElement textArray, bool append)
    {
        int insertPos;
        if (append)
            insertPos = dialogue.Count - 1;
        else
            insertPos = index + 1;

        textArray.EnumerateArray().ToList().ForEach(x =>
        {
            // kdyz tam je cislo, priradime prislusnou funkci

            switch (x.ValueKind)
            {
                case JsonValueKind.Number:
                    if (append)
                        dialogue.Add(new DialogueElement(Actions.GetValueOrDefault(x.GetInt32())));
                    else
                        dialogue.Insert((int)insertPos, new DialogueElement(Actions.GetValueOrDefault(x.GetInt32())));
                    break;
                // jinak je to normalni text
                case JsonValueKind.String:
                    if (append)
                        dialogue.Add(new DialogueElement(x.GetString()));
                    else
                        dialogue.Insert((int)insertPos, new DialogueElement(x.GetString()));
                    break;
                case JsonValueKind.Object:
                    if (append)
                        AddQuestion(x, null);
                    else
                        AddQuestion(x, insertPos);
                    break;

                default: break;
            }
            insertPos++;
        });
    }

    protected void AddQuestion(JsonElement questionObj, int? insert)
    {
        Dictionary<string, string> pairs = [];
        // otazku dame pryc
        JsonElement question = questionObj.GetProperty("question");
        var options = questionObj.EnumerateObject().ToList();
        options.RemoveAll(x => x.Name == "question");

        // zbytek jsou moznosti odpovedi
        options.ForEach(x => pairs.Add(x.Value.GetString(), x.Name));

        if (insert == null)
            dialogue.Add(new DialogueElement(question.GetString(), pairs));
        else
            dialogue.Insert((int)insert + 1, new DialogueElement(question.GetString(), pairs));
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