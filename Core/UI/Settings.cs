public static class Settings
{
    public static float MusicVolume = 0.1f;
    public static float SfxVolume = 0.5f;
    public static bool FixedCamera = false;

    private static string settingsPath = "tbogv_settings.json";

    public static void Save()
    {
        FileHelper.Save(settingsPath, new SettingsData(MusicVolume, SfxVolume, FixedCamera), SaveType.GENERIC);
    }

    public static void Load()
    {
        var data = FileHelper.Load<SettingsData>(settingsPath, SaveType.GENERIC);
        if (data != null)
        {
            MusicVolume = data.MusicVolume;
            SfxVolume = data.SfxVolume;
            FixedCamera = data.FixedCamera;
        }
    }

    private class SettingsData
    {
        public float MusicVolume = 0.1f;
        public float SfxVolume = 0.5f;
        public bool FixedCamera = false;

        public SettingsData(float musicVolume, float sfxVolume, bool fixedCamera)
        {
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;
            FixedCamera = fixedCamera;
        }
    }
}