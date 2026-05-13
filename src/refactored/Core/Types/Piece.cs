using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public enum Piece : int
    {
        NoPiece,                            //  0 - 0000

        WhitePawn   = PieceType.Pawn,       //  1 - 0001
        WhiteKnight = PieceType.Knight,     //  2 - 0010
        WhiteBishop = PieceType.Bishop,     //  3 - 0011
        WhiteRook   = PieceType.Rook,       //  4 - 0100
        WhiteQueen  = PieceType.Queen,      //  5 - 0101
        WhiteKing   = PieceType.King,       //  6 - 0110

        BlackPawn   = PieceType.Pawn   + 8, //  9 - 1001
        BlackKnight = PieceType.Knight + 8, // 10 - 1010
        BlackBishop = PieceType.Bishop + 8, // 11 - 1011
        BlackRook   = PieceType.Rook   + 8, // 12 - 1100
        BlackQueen  = PieceType.Queen  + 8, // 13 - 1101
        BlackKing   = PieceType.King   + 8, // 14 - 1110

        PieceNB = 16,
    }

    public static class PieceExtensions
    {
        private const string PieceToSymbol = " ♟♞♝♜♛♚  ♙♘♗♖♕♔";
        private const string PieceToChar   = " PNBRQK  pnbrqk";
        
        extension(Piece)
        {
            public static Piece FromChar(char character) => (Piece)PieceToChar.IndexOf(character);
            public static char ToChar(Piece piece)       => PieceToChar[(int)piece];
            public static char ToSymbol(Piece piece)     => PieceToSymbol[(int)piece];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PieceType TypeOf(this Piece piece)
        {
            return (PieceType)((int)piece & 7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color GetColor(this Piece piece)
        {
            return (Color)((int)piece >> 3);
        }
    }
}