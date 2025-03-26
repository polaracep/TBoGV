using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(TBoGV.JsonDocumentReader))]
    public static void Main(string[] args)
    {
#if RELEASE
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Exception ex = (Exception)args.ExceptionObject;
            LogCrash(ex);
        };
#endif

        using var game = new TBoGV.TBoGVGame();
        game.Run();
    }
#if RELEASE
    public static void LogCrash(Exception e)
    {
        Directory.CreateDirectory("logs");
        string path = "logs/" + DateTime.Now.GetDateTimeFormats().GetValue(21).ToString().Replace(':', '-') + ".log";

        File.WriteAllText(path, e.ToString(), Encoding.UTF8);
        Console.WriteLine("AURR NAURRRR!!!! An error occured :(((");
        Console.WriteLine("log created at: " + path);
        throw e;
    }
#endif
}
