using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public enum PieceType : int
    {
        NoPieceType,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
        AllPieces,
        PieceTypeNB,
    }

    public static class PieceTypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Piece MakePiece(this PieceType pieceType, Color color)
        {
            return (Piece)(((int)color << 3) | (int)pieceType);
        }
    }
}
