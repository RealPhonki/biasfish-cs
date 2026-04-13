using Biasfish.Core;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            // initialize board
            Board testBoard = new Board();
            testBoard.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            testBoard.Push(new Move(Squares.E2, Squares.E4, Flags.Quiet));
            testBoard.Push(new Move(Squares.E7, Squares.E5, Flags.Quiet));
            testBoard.Push(new Move(Squares.G1, Squares.F3, Flags.Quiet));
            testBoard.Push(new Move(Squares.B8, Squares.C6, Flags.Quiet));
            testBoard.Push(new Move(Squares.F1, Squares.B5, Flags.Quiet));
            testBoard.Push(new Move(Squares.G8, Squares.F6, Flags.Quiet));

            Debug.PrintBoard(testBoard);

            // generate legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList moveList = new MoveList(memoryBuffer);

            Pawns.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Pawn moves,   moveList.Length: {moveList.Length}");
            Knights.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Knight moves, moveList.Length: {moveList.Length}");
            Bishops.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Bishop Moves, moveList.Length: {moveList.Length}");
            Rooks.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Rook moves,   moveList.Length: {moveList.Length}");
            Queens.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Queen moves,  moveList.Length: {moveList.Length}");
            Kings.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+King moves,   moveList.Length: {moveList.Length}");

            for (int i=0; i<moveList.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {moveList[i]}");
            }
        }
    }
}