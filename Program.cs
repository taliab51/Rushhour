namespace NEA_Rushhour_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            //game.DisplayMenu();
            game.GetGrid().DisplayGrid();
            game.Rushhour();
            
        }
    }
}