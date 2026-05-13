using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    public enum PieceType : int
    {
        NoPieceType, // 0 - 000
        Pawn,        // 1 - 001
        Knight,      // 2 - 010
        Bishop,      // 3 - 011
        Rook,        // 4 - 100
        Queen,       // 5 - 101
        King,        // 6 - 110
        AllPieces,   // 7 - 111
        PieceTypeNB,
    }
}
