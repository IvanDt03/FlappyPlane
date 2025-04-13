
public class Program
{
    private static void Main(string[] args)
    {
        using var game = new FlappyPlane.FlappyPlaneGame();
        game.Run();
    }
}