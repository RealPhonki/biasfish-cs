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
            testBoard.LoadFEN("8/5k2/5p2/5Kp1/r3P3/2N5/8/8 w - - 0 1");

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