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

                // King attack mappings
                // +7 +8 +9
                // -1  0 +1
                // -9 -8 -7
                attacks |= (king << 9) & Masks.NotFileA; // NoEa
                attacks |= king << 8;                    // No
                attacks |= (king << 7) & Masks.NotFileH; // NoWe
                attacks |= (king << 1) & Masks.NotFileA; // Ea
                attacks |= (king >> 1) & Masks.NotFileH; // We
                attacks |= (king >> 7) & Masks.NotFileA; // SoEa
                attacks |= king >> 8;                    // So
                attacks |= (king >> 9) & Masks.NotFileH; // SoWe

                KingAttacks[square] = attacks;
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong kings = board.Get(Piece.Kings | board.sideToMove);

            int fromSquare = BitOperations.TrailingZeroCount(kings);
            ulong kingAttacks = KingAttacks[fromSquare];

            ulong quietMoves = kingAttacks & ~board.Get(Piece.Any);
            ulong captureMoves = kingAttacks & board.Get(Piece.FlipColor(board.sideToMove));
            
            while (quietMoves != 0)
            {
                int toSquare = BitOperations.TrailingZeroCount(quietMoves);
                moveList.Add(new Move(fromSquare, toSquare, Flag.Quiet));

                quietMoves &= quietMoves - 1;
            }

            while (captureMoves != 0)
            {
                int toSquare = BitOperations.TrailingZeroCount(captureMoves);
                moveList.Add(new Move(fromSquare, toSquare, Flag.Capture));

                captureMoves &= captureMoves - 1;
            }
        }
    }
}