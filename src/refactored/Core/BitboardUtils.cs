using System.Diagnostics;
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
            Debug.Assert(bitboard != 0, "Cannot find the LSB on an empty bitboard");
            return BitOperations.TrailingZeroCount(bitboard);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MSB(Bitboard bitboard)
        {
            Debug.Assert(bitboard != 0, "Cannot find the MSB on an empty bitboard");
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
            Debug.Assert(Enum.IsDefined(typeof(Direction), direction), $"Invalid direction '{direction}'");

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

        public static void Visualize(Bitboard bitboard)
        {
            Console.WriteLine("  +---+---+---+---+---+---+---+---+");
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write($"{rank + 1} | ");
                for (int file = 0; file < 8; file++)
                {
                    int square = rank * 8 + file;
                    if ((bitboard & Masks.Square[square]) != 0)
                    {
                        Console.Write($"1 | ");
                    }
                    else
                    {
                        Console.Write($"  | ");
                    }
                }
                Console.WriteLine("\n  +---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("    a   b   c   d   e   f   g   h   ");
        }
    }
}