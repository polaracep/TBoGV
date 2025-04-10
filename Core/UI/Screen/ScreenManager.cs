using Microsoft.Xna.Framework;
using TBoGV;

public static class ScreenManager
{
    public static ScreenStart ScreenStart;
    public static ScreenSettings ScreenSettings;
    public static ScreenGame ScreenGame;
    public static ScreenEnd ScreenEnd;
    public static ScreenDeath ScreenDeath;

    public static void Init(GraphicsDeviceManager graphics)
    {
        ScreenStart = new ScreenStart(graphics);
        ScreenSettings = new ScreenSettings(graphics);
        ScreenGame = new ScreenGame(graphics);
        ScreenEnd = new ScreenEnd(graphics);
        ScreenDeath = new ScreenDeath(graphics);
    }

}