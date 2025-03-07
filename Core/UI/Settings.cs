using System.Collections.Generic;

public interface ISetting
{
    public string Name { get; }
    public object GetValue();
}
public class Setting<T> : ISetting
{
    private T Value;
    private string Name;
    public Setting(string name, T val)
    {
        Value = val;
        Name = name;
    }

    string ISetting.Name { get => Name; }

    public object GetValue()
    {
        return Value;
    }
}
public static class Settings
{
    public static Setting<float> MusicVolume = new Setting<float>("Hlasitost hudby", 0.1f);
    public static Setting<float> SfxVolume = new Setting<float>("Hlasitost efektů", 0.5f);
    public static Setting<bool> FixedCamera = new Setting<bool>("Kamera na hráči", false);

    public static List<ISetting> SettingsList = [
        MusicVolume,
        SfxVolume,
        FixedCamera,
    ];

    private static string settingsPath = "tbogv_settings.json";

    public static void Save()
    {
        FileHelper.Save(settingsPath, SettingsList, SaveType.GENERIC);
    }

    public static void Load()
    {
        var data = FileHelper.Load<List<ISetting>>(settingsPath, SaveType.GENERIC);
        if (data != null)
        {
            SettingsList = data;
        }
    }
}