using System.Numerics;
using Biasfish.Core;

namespace Biasfish.MoveGeneration
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

                // Knight attack mappings: https://www.chessprogramming.org/Knight_Pattern
                //         NoWe      NoEa
                //           +15  +17
                //            |     |
                // WeWe  +6 __|     |__+10  EaEa
                //             \   /
                //              >0<
                //          __ /   \ __
                // WeWe -10   |     |   -6  EaEa
                //            |     |
                //           -17  -15
                //         SoWe      SoEa
                attacks |= (knight << 17) & Masks.NotFileA;  // NoEa
                attacks |= (knight << 15) & Masks.NotFileH;  // NoWe
                attacks |= (knight << 10) & Masks.NotFileAB; // EaEa
                attacks |= (knight <<  6) & Masks.NotFileGH; // WeWe
                attacks |= (knight >>  6) & Masks.NotFileAB; // EaEa
                attacks |= (knight >> 10) & Masks.NotFileGH; // WeWe
                attacks |= (knight >> 15) & Masks.NotFileA;  // SoEa
                attacks |= (knight >> 17) & Masks.NotFileH;  // SoWe

                KnightAttacks[square] = attacks;
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong knights = board.Get(Piece.Knights | board.sideToMove);

            while (knights != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(knights);

                MoveGeneration.SerializeMoves(ref board, ref moveList, KnightAttacks[fromSquare], fromSquare);
                knights &= knights - 1;
            }
        }

        // consistency
        public static ulong GetAttackBitboard(ref Board _, int fromSquare) => KnightAttacks[fromSquare];
    }
}