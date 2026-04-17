using Biasfish.V2;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            // initialize board
            Board board = new Board();

            board.Push(new Move(Squares.E2, Squares.E4, 0));

            Debug.PrintBoard(board);
        }
    }
}