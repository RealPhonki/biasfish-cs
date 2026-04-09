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
            testBoard.LoadFEN("6rk/p5rr/1R6/8/8/8/2p3RR/1N1R2RK b - - 0 1");

            Debug.PrintBoard(testBoard);

            // generate legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList moveList = new MoveList(memoryBuffer);

            Console.WriteLine($"+None,         moveList.Length: {moveList.Length}");
            Knights.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Knight moves, moveList.Length: {moveList.Length}");
            Kings.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+King moves,   moveList.Length: {moveList.Length}");
            Pawns.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Pawn moves,   moveList.Length: {moveList.Length}");

            for (int i=0; i<moveList.Length; i++)
            {
                Console.WriteLine($"{i}: {moveList[i]}");
            }
        }
    }
}