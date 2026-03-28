using Biasfish.Core;
using System;
using System.Reflection.Metadata;

namespace Biasfish
{
    class Program
    {
        public static void Main(string[] args)
        {
            Board testBoard = new Board();
            testBoard.SetBitboard(Piece.WhitePawns, 809412089);
            Debug.PrintBoard(testBoard);
        }
    }
}