using System.Runtime.CompilerServices;

namespace Biasfish.Core
{
    /// <summary>
    /// It is important to know that there is a difference between 'piece' and 'pieceType'.
    /// 'piece' encodes both type and color, while 'pieceType' only contains the former. This
    /// distinction is important because they are used for separate purposes and have slightly
    /// different encodings.
    /// </summary>
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
