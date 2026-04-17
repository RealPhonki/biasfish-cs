using System.Numerics;
using Biasfish.Core;

namespace Biasfish.MoveGeneration
{
    public static class Rooks
    {
        public static readonly ulong[] NorthRay = new ulong[64];
        public static readonly ulong[] EastRay = new ulong[64];
        public static readonly ulong[] SouthRay = new ulong[64];
        public static readonly ulong[] WestRay = new ulong[64];

        static Rooks()
        {
            for (int square = 0; square < 64; square++)
            {
                int x = square % 8;
                int y = square / 8;

                NorthRay[square] = Masks.AboveRank[y] & Masks.File[x];
                EastRay[square]  = Masks.RightFile[x] & Masks.Rank[y];
                SouthRay[square] = Masks.BelowRank[y] & Masks.File[x];
                WestRay[square]  = Masks.LeftFile[x]  & Masks.Rank[y];
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong rooks = board.Get(Piece.Rooks | board.sideToMove);

            while (rooks != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(rooks);
                ulong rookAttacks = GetAttackBitboard(fromSquare, board.GetOccupied());

                MoveGeneration.SerializeMoves(ref board, ref moveList, rookAttacks, fromSquare);
                rooks &= rooks - 1;
            }
        }

        public static ulong GetAttackBitboard(int square, ulong occupied)
        {
            ulong rookAttacks = 0;
            ulong blockers;
            ulong ray;

            // scan north (first blocker is at a higher bit)
            ray = NorthRay[square];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= NorthRay[BitOperations.TrailingZeroCount(blockers)];
            rookAttacks |= ray;

            // scan east (first blocker is at a higher bit)
            ray = EastRay[square];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= EastRay[BitOperations.TrailingZeroCount(blockers)];
            rookAttacks |= ray;

            // scan south (first blocker is at a lower bit)
            ray = SouthRay[square];
            blockers = ray & occupied;
            
            if (blockers != 0) ray ^= SouthRay[63 - BitOperations.LeadingZeroCount(blockers)];
            rookAttacks |= ray;

            // scan west (first blocker is at a lower bit)
            ray = WestRay[square];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= WestRay[63 - BitOperations.LeadingZeroCount(blockers)];
            rookAttacks |= ray;

            return rookAttacks;
        }
    }
}