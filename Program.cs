using Biasfish.V2;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            // initialize board
            Board board = new Board();

            board.Push(new Move(Squares.E2, Squares.E4, Flags.Quiet));
            board.Push(new Move(Squares.E7, Squares.E5, Flags.Quiet));
            board.Push(new Move(Squares.G1, Squares.F3, Flags.Quiet));
            board.Push(new Move(Squares.G8, Squares.F6, Flags.Quiet));

            
            
            foreach (int piece in Piece.PieceTypes)
            {
                Debug.PrintBB(board.Get(piece));
            }

            board.Push(new Move(Squares.F3, Squares.E5, Flags.Capture));

            Debug.PrintBoard(board);
            foreach (int piece in Piece.PieceTypes)
            {
                Debug.PrintBB(board.Get(piece));
            }
        }
    }
}