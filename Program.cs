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
            testBoard.LoadFEN("kp6/pp6/8/8/4B3/8/PP6/KP6 w - - 0 1");

            Debug.PrintBoard(testBoard);

            // generate legal moves
            Span<Move> memoryBuffer = stackalloc Move[256];
            MoveList moveList = new MoveList(memoryBuffer);

            //Console.WriteLine($"+Knight moves, moveList.Length: {moveList.Length}");
            //Kings.GetPseudoLegal(ref testBoard, ref moveList);
            //Console.WriteLine($"+King moves,   moveList.Length: {moveList.Length}");
            //Pawns.GetPseudoLegal(ref testBoard, ref moveList);
            //Console.WriteLine($"+Pawn moves,   moveList.Length: {moveList.Length}");
            //Rooks.GetPseudoLegal(ref testBoard, ref moveList);
            //Console.WriteLine($"+Rook moves,   moveList.Length: {moveList.Length}");
            Bishops.GetPseudoLegal(ref testBoard, ref moveList);
            Console.WriteLine($"+Bishop Moves, moveList.Length: {moveList.Length}");

            for (int i=0; i<moveList.Length; i++)
            {
                Console.WriteLine($"{i}: {moveList[i]}");
            }
        }
    }
}