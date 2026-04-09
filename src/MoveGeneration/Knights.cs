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

            ulong empty = ~board.Get(Piece.Any);
            ulong enemy = board.Get(Piece.FlipColor(board.sideToMove));

            while (knights != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(knights);
                ulong knightAttacks = KnightAttacks[fromSquare];

                ulong quietMoves = knightAttacks & empty;
                ulong captureMoves = knightAttacks & enemy;

                while (quietMoves != 0)
                {
                    int toSquare = BitOperations.TrailingZeroCount(quietMoves);
                    moveList.Add(new Move(fromSquare, toSquare, Flag.Quiet));

                    quietMoves &= quietMoves - 1; // clear lsb
                }

                while (captureMoves != 0)
                {
                    int toSquare = BitOperations.TrailingZeroCount(captureMoves);
                    moveList.Add(new Move(fromSquare, toSquare, Flag.Capture));

                    captureMoves &= captureMoves - 1; // clear lsb
                }

                knights &= knights - 1;
            }
        }
    }
}