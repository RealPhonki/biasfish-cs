using Biasfish.Core;
using System;
using System.Reflection.Metadata;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            // initialize board
            Board testBoard = new Board();
            testBoard.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Debug.PrintBoard(testBoard);

            // load move stack
            Move[] moveStack = {
                new Move(Squares.E2, Squares.E4),
                new Move(Squares.E7, Squares.E5),
                new Move(Squares.G1, Squares.F3),
                new Move(Squares.B8, Squares.C6),
                new Move(Squares.F1, Squares.C4),
                new Move(Squares.G8, Squares.F6),
            };
            foreach (Move move in moveStack)
            {
                testBoard.Push(move);
            }

            Debug.PrintBoard(testBoard);

            // generate legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList moveList = new MoveList(memoryBuffer);

            Console.WriteLine($"+None,         moveList.Length: {moveList.Length}");
            Knights.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Knight moves, moveList.Length: {moveList.Length}");
            Kings.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+King moves,   moveList.Length: {moveList.Length}");

            for (int i=0; i<moveList.Length; i++)
            {
                Console.WriteLine(moveList[i]);
            }
        }
    }
}