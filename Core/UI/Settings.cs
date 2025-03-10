using System;
using System.Collections.Generic;
using System.Linq;

public class Setting
{
    public object Value;
    public string Id;
    public string Name;
    public Setting(string id, string name, object val)
    {
        Id = id;
        Value = val;
        Name = name;
    }
}
public static class Settings
{
    public static Setting MusicVolume = new Setting("music", "Hlasitost hudby", 0.1f);
    public static Setting SfxVolume = new Setting("sfx", "Hlasitost efektů", 0.5f);
    public static Setting FixedCamera = new Setting("fixedCamera", "Kamera na hráči", false);

    public static List<Setting> SettingsList = [
        MusicVolume,
        SfxVolume,
        FixedCamera,
    ];

    private static Dictionary<string, object> Serialize()
    {
        Dictionary<string, object> output = [];
        SettingsList.ToList().ForEach(s => output.Add(s.Id, s.Value));
        return output;
    }

    private static void Deserialize(Dictionary<string, object> pairs)
    {
        try
        {
            foreach (var item in pairs)
                SettingsList.Find(x => x.Id == item.Key).Value = item.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine("Didn't find the corresponding setting: " + e.Message);
        }
    }

    private static string settingsPath = "./tbogv_settings.json";

    public static void Save()
    {
        Dictionary<string, object> serialized = Serialize();
        FileHelper.Save(settingsPath, serialized, SaveType.GENERIC);
    }

    public static void Load()
    {
        var data = FileHelper.Load<Dictionary<string, object>>(settingsPath, SaveType.GENERIC);
        if (data != null)
        {
            Deserialize(data);
        }
    }
}