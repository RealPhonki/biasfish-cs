using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public enum Piece : int
    {
        NoPiece,

        WhitePawn   = PieceType.Pawn,
        WhiteKnight = PieceType.Knight,
        WhiteBishop = PieceType.Bishop,
        WhiteRook   = PieceType.Rook,
        WhiteQueen  = PieceType.Queen,
        WhiteKing   = PieceType.King,

        BlackPawn   = PieceType.Pawn   + 8,
        BlackKnight = PieceType.Knight + 8,
        BlackBishop = PieceType.Bishop + 8,
        BlackRook   = PieceType.Rook   + 8,
        BlackQueen  = PieceType.Queen  + 8,
        BlackKing   = PieceType.King   + 8,

        PieceNB = 16,
    }

    public static class PieceExtensions
    {
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