using Biasfish.Core;
using Biasfish.MoveGeneration;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            // initialize board
            int positions;
            Board board = new Board();

            for (int i = 0; i < 10; i++)
            {
                // clear board
                board.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

                positions = Perft(ref board, i);

                Console.WriteLine($"Depth {i + 1}: {positions}");
            }
        }

        public static int Perft(ref Board board, int depth)
        {
            if (depth == 0)
            {
                return 1;
            }

            int positionsFound = 0;

            // get legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList moveList = new MoveList(memoryBuffer);
            MoveGeneration.MoveGeneration.GetLegalMoves(ref board, ref moveList);
            for (int i = 0; i < moveList.count; i++)
            {
                board.Push(moveList[i]);

                positionsFound += Perft(ref board, depth - 1);

                board.Pop(moveList[i]);
            }

            return positionsFound;
        }
    }
}