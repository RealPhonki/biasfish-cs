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
            testBoard.SetBitboard(Piece.WhitePawns, 65280);
            testBoard.SetBitboard(Piece.WhiteKnights,  66);
            testBoard.SetBitboard(Piece.WhiteBishops,  36);
            testBoard.SetBitboard(Piece.WhiteRooks,   129);
            testBoard.SetBitboard(Piece.WhiteQueens,    8);
            testBoard.SetBitboard(Piece.WhiteKings,    16);
            Debug.PrintBoard(testBoard);
        }
    }
}