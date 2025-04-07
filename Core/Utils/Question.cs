using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class Question
{
    public string QuestionText { get; protected set; }
    protected List<string> WrongAnswers;
    protected List<string> CorrectAnswers;
    public List<string> Answers { get; protected set; } = [];

    public Question(JsonElement json)
    {
        QuestionText = json.GetProperty("q").GetString();
        WrongAnswers = json.GetProperty("0").EnumerateArray().ToList().Select(x => x.ToString()).ToList();
        CorrectAnswers = json.GetProperty("1").EnumerateArray().ToList().Select(x => x.ToString()).ToList();
        Answers.AddRange(CorrectAnswers);
        Answers.AddRange(WrongAnswers);
        Answers.Shuffle();
    }

    public bool CheckAnswer(string ans)
    {
        return CorrectAnswers.Contains(ans);
    }

}
public static class ListExtension
{
    // https://stackoverflow.com/questions/273313/randomize-a-listt 
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}