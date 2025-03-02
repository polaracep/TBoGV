using System;
using System.IO;
using Newtonsoft.Json;


public static class FileHelper
{
    public static void Save<T>(string filePath, T data)
    {
        try
        {
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving file {filePath}: {e.Message}");
        }
    }

    public static T Load<T>(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading file {filePath}: {e.Message}");
        }
        return default;
    }
}
