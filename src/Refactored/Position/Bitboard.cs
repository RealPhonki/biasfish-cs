using System.Numerics;

namespace Biasfish.V2
{
    public static class Bitboard
    {
        public static ulong Square(Square square)
        {
            return 1UL << (int)square;
        }

        // ! This method name may become ambiguous in the future
        public static ulong Mask(Square square)
        {
            return ~(1UL << (int)square);
        }

        public static bool MoreThanOne(ulong bitboard)
        {
            return (bitboard & (bitboard - 1)) != 0;
        }

        public static int LSB(ulong bitboard)
        {
            return BitOperations.TrailingZeroCount(bitboard);
        }

        public static int MSB(ulong bitboard)
        {
            return BitOperations.LeadingZeroCount(bitboard);
        }

        public static int PopLSB(ref ulong bitboard)
        {
            int lsb = LSB(bitboard);
            bitboard &= bitboard - 1;
            return lsb;
        }

        public static int PopCount(ulong bitboard)
        {
            return BitOperations.PopCount(bitboard);
        }

        public static ulong Shift(ulong bitboard, int direction)
        {
            if (direction > 0)
            {
                return bitboard << direction;
            }
            else
            {
                return bitboard >> direction;
            }
        }

        public static ulong PawnAttacks(ulong bitboard, int sideToMove)
        {
            throw new NotImplementedException();
        }

        public static ulong Rank(Square square)
        {
            throw new NotImplementedException();
        }

        public static ulong File(Square square)
        {
            throw new NotImplementedException();
        }

        public static ulong Line(Square square1, Square square2)
        {
            throw new NotImplementedException();
        }

        public static ulong Between(Square square1, Square square2)
        {
            throw new NotImplementedException();
        }

        public static int Distance(Square square1, Square square2)
        {
            throw new NotImplementedException();
        }
    }
}