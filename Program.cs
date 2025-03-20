using System.Diagnostics.CodeAnalysis;

public static class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(TBoGV.JsonDocumentReader))]
    public static void Main(string[] args)
    {
        using var game = new TBoGV.TBoGVGame();
        game.Run();
    }
}
