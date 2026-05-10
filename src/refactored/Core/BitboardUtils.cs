using System.Numerics;
using System.Runtime.CompilerServices;
using Bitboard = ulong;

namespace Biasfish.Core
{
    public static class BitboardUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCount(Bitboard bitboard)
        {
            return BitOperations.PopCount(bitboard);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopLSB(ref Bitboard bitboard)
        {
            int lsb = LSB(bitboard);
            bitboard &= bitboard - 1;
            return lsb;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LSB(Bitboard bitboard)
        {
            return BitOperations.TrailingZeroCount(bitboard);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(Bitboard bitboard)
        {
            return 63 - BitOperations.LeadingZeroCount(bitboard);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MoreThanOne(Bitboard bitboard)
        {
            return (bitboard & (bitboard - 1)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bitboard Shift(Direction direction, Bitboard bitboard)
        {
            return direction switch
            {
                Direction.North     => bitboard << 8,
                Direction.South     => bitboard >> 8,
                Direction.NorthEast => (bitboard & ~Masks.FileH) << 9,
                Direction.NorthWest => (bitboard & ~Masks.FileA) << 7,
                Direction.SouthEast => (bitboard & ~Masks.FileH) >> 7,
                Direction.SouthWest => (bitboard & ~Masks.FileA) >> 9,
                Direction.East      => (bitboard & ~Masks.FileH) << 1,
                Direction.West      => (bitboard & ~Masks.FileA) >> 1,
                _ => 0,
            };
        }
    }
}