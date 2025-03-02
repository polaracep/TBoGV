using Microsoft.Xna.Framework;
using TBoGV;

public static class ScreenManager
{
    public static ScreenStart ScreenStart = new ScreenStart();
    public static ScreenSettings ScreenSettings = new ScreenSettings();
    public static ScreenGame ScreenGame = new ScreenGame();
    public static ScreenEnd ScreenEnd = new ScreenEnd();

    public static void Init(GraphicsDeviceManager graphics)
    {
        ScreenStart.BeginRun(graphics);
        ScreenSettings.BeginRun(graphics);
        ScreenGame.BeginRun(graphics);
        ScreenEnd.BeginRun(graphics);
    }
}