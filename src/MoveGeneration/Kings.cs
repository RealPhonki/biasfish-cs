using System.Numerics;

namespace Biasfish.Core
{
    public static class Kings
    {
        public static readonly ulong[] KingAttacks = new ulong[64];

        static Kings()
        {
            for (int square = 0; square < 64; square++)
            {
                ulong king = 1UL << square;
                ulong attacks = 0;

                attacks |= king << Offset.Rank;                                    // Up 1
                attacks |= king >> Offset.Rank;                                    // Down 1
                attacks |= (king << Offset.File) & Masks.NotFileA;                 // Right 1
                attacks |= (king >> Offset.File) & Masks.NotFileH;                 // Left 1
                attacks |= (king << (Offset.Rank + Offset.File)) & Masks.NotFileA; // Up 1, Right 1
                attacks |= (king << (Offset.Rank - Offset.File)) & Masks.NotFileH; // Up 1, Left 1
                attacks |= (king >> (Offset.Rank - Offset.File)) & Masks.NotFileA; // Down 1, Right 1
                attacks |= (king >> (Offset.Rank + Offset.File)) & Masks.NotFileH; // Down 1, Left 1

                KingAttacks[square] = attacks;
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong friendly = board.GetBitboard(board.sideToMove);
            ulong kings = board.GetBitboard(Piece.Kings | board.sideToMove);

            int fromSquare = BitOperations.TrailingZeroCount(kings);
            ulong kingAttacks = KingAttacks[fromSquare] & ~friendly;
            
            while (kingAttacks != 0)
            {
                int toSquare = BitOperations.TrailingZeroCount(kingAttacks);
                moveList.Add(new Move(fromSquare, toSquare, 0));

                kingAttacks &= kingAttacks - 1;
            }
        }
    }
}