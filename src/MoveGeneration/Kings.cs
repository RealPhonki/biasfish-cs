

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

                attacks |= king << Offset.Rank;                    // UP, bitshift leftwards will not wrap
                attacks |= king >> Offset.Rank;                    // DOWN, bitshift rightwards will not wrap
                attacks |= (king << Offset.File) & Masks.NotFileA; // RIGHT, the king should never reach the left-most file by moving rightwards
                attacks |= (king >> Offset.File) & Masks.NotFileH; // LEFT, the king should never reach the right-most file by moving leftwards

                attacks |= (king << (Offset.Rank + Offset.File)) & Masks.NotFileA; // UP-RIGHT, same mask as RIGHT
                attacks |= (king << (Offset.Rank - Offset.File)) & Masks.NotFileH; // UP-LEFT, same mask as LEFT
                attacks |= (king >> (Offset.Rank - Offset.File)) & Masks.NotFileA; // DOWN-RIGHT, same mask as RIGHT
                attacks |= (king >> (Offset.Rank + Offset.File)) & Masks.NotFileH; // DOWN-LEFT, same mask as LEFT

                KingAttacks[square] = attacks;
            }
        }

        public static MoveList GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            

            return moveList;
        }
    }
}