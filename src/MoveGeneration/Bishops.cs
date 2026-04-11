using System.Numerics;
using Biasfish.Core;

namespace Biasfish
{
    public static class Bishops
    {
        public static readonly ulong[] NorthEastRay = new ulong[64];
        public static readonly ulong[] NorthWestRay = new ulong[64];
        public static readonly ulong[] SouthEastRay = new ulong[64];
        public static readonly ulong[] SouthWestRay = new ulong[64];

        static Bishops()
        {
            for (int square = 0; square < 64; square++)
            {
                Raycast(ref NorthEastRay, square,  1,  1);
                Raycast(ref NorthWestRay, square, -1,  1);
                Raycast(ref SouthEastRay, square,  1, -1);
                Raycast(ref SouthWestRay, square, -1, -1);
            }
        }

        private static void Raycast(ref ulong[] RayLookup, int fromSquare, int xOffset, int yOffset)
        {
            int x = fromSquare % 8 + xOffset;
            int y = fromSquare / 8 + yOffset;
            while (x >= 0 && x < 8 && y >= 0 && x < 8)
            {
                RayLookup[fromSquare] |= 1UL << (x + y * 8);
                x += xOffset;
                y += yOffset;
            }
        }

        public static void GetPseudoLegal(ref Board board, ref MoveList moveList)
        {
            ulong bishops = board.Get(Piece.Bishops | board.sideToMove);

            while (bishops != 0)
            {
                int fromSquare = BitOperations.TrailingZeroCount(bishops);
                ulong bishopAttacks = GetAttackBitboard(ref board, fromSquare);

                MoveGenUtils.SerializeMoves(ref moveList, ref board, bishopAttacks, fromSquare);
                bishops &= bishops - 1;
            }
        }

        public static ulong GetAttackBitboard(ref Board board, int fromSquare)
        {
            ulong occupied = board.Get(Piece.Any);
            ulong bishopAttacks = 0;
            ulong blockers;
            ulong ray;

            // scan north east
            ray = NorthEastRay[fromSquare];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= NorthEastRay[BitOperations.TrailingZeroCount(blockers)];
            bishopAttacks |= ray;

            // scan north west
            ray = NorthWestRay[fromSquare];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= NorthWestRay[BitOperations.TrailingZeroCount(blockers)];
            bishopAttacks |= ray;

            ray = SouthEastRay[fromSquare];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= SouthEastRay[63 - BitOperations.LeadingZeroCount(blockers)];
            bishopAttacks |= ray;

            ray = SouthWestRay[fromSquare];
            blockers = ray & occupied;
            if (blockers != 0) ray ^= SouthWestRay[BitOperations.TrailingZeroCount(blockers)];
            bishopAttacks |= ray;

            return bishopAttacks;
        }
    }
}