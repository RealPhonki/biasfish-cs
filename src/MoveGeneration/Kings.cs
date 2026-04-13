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

        public static void SerializeCastling(ref Board board, ref MoveList moveList)
        {
            if (board.sideToMove == Piece.White)
            {
                // kingside
                if (board.CanWhiteCastleKingside() && (board.Get(Piece.Any) & (Masks.F1 | Masks.G1)) == 0)
                {
                    moveList.Add(new Move(Squares.E1, Squares.G1, Flags.KingCastle));
                }
                // queenside
                if (board.CanWhiteCastleQueenside() && (board.Get(Piece.Any) & (Masks.B1 | Masks.C1 | Masks.D1)) == 0)
                {
                    moveList.Add(new Move(Squares.E1, Squares.C1, Flags.QueenCastle));
                }
            }
            else
            {
                // kingside
                if (board.CanBlackCastleKingside() && (board.Get(Piece.Any) & (Masks.F8 | Masks.G8)) == 0)
                {
                    moveList.Add(new Move(Squares.E8, Squares.G8, Flags.KingCastle));
                }
                // queenside
                if (board.CanBlackCastleQueenside() && (board.Get(Piece.Any) & (Masks.B8 | Masks.C8 | Masks.D8)) == 0)
                {
                    moveList.Add(new Move(Squares.E8, Squares.C8, Flags.QueenCastle));
                }
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong kings = board.Get(Piece.Kings | board.sideToMove);
            int fromSquare = BitOperations.TrailingZeroCount(kings);

            SerializeCastling(ref board, ref moveList);

            MoveGenUtils.SerializeMoves(ref moveList, ref board, KingAttacks[fromSquare], fromSquare);
        }

        // consistency
        public static ulong GetAttackBitboard(ref Board _, int fromSquare) => KingAttacks[fromSquare];
    }
}