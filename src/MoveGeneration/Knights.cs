using System.Numerics;

namespace Biasfish.Core
{
    public static class Knights
    {
        public static readonly ulong[] KnightAttacks = new ulong[64];

        static Knights()
        {
            for (int square = 0; square < 64; square++)
            {
                ulong knight = 1UL << square;
                ulong attacks = 0;

                attacks |= (knight << (2 * Offset.Rank +     Offset.File)) & Masks.NotFileA;                  // Up 2, Right 1
                attacks |= (knight << (2 * Offset.Rank -     Offset.File)) & Masks.NotFileH;                  // Up 2, Left 1
                attacks |= (knight << (    Offset.Rank + 2 * Offset.File)) & Masks.NotFileA & Masks.NotFileB; // Up 1, Right 2
                attacks |= (knight << (    Offset.Rank - 2 * Offset.File)) & Masks.NotFileG & Masks.NotFileH; // Up 1, Left 2
                attacks |= (knight >> (    Offset.Rank - 2 * Offset.File)) & Masks.NotFileA & Masks.NotFileB; // Down 1, Right 2
                attacks |= (knight >> (    Offset.Rank + 2 * Offset.File)) & Masks.NotFileG & Masks.NotFileH; // Down 1, Left 2
                attacks |= (knight >> (2 * Offset.Rank -     Offset.File)) & Masks.NotFileA;                  // Down 2, Right 1
                attacks |= (knight >> (2 * Offset.Rank +     Offset.File)) & Masks.NotFileH;                  // Down 2, Left 1

                KnightAttacks[square] = attacks;
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong friendly = board.Get(board.sideToMove);
            ulong knights = board.Get(Piece.Knights | board.sideToMove);

            while (knights != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(knights);
                ulong knightAttacks = KnightAttacks[fromSquare] & ~friendly; // mask friendly pieces

                while (knightAttacks != 0)
                {
                    int toSquare = BitOperations.TrailingZeroCount(knightAttacks);
                    moveList.Add(new Move(fromSquare, toSquare, 0));

                    knightAttacks &= knightAttacks - 1; // this clears the lsb
                }
                knights &= knights - 1; // this clears the lsb
            }
        }
    }
}