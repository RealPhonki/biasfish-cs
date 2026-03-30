namespace Biasfish.Core
{
    /// <summary>
    /// Represents a move and metadata with a 16 bit value.
    /// </summary>
    public readonly struct Move
    {
        private readonly ushort value;

        // Bits from 1-6 represent the starting square (0-63) for the move.
        public int FromSquare => value & 0b111111;

        // Bits from 7-12 represent the ending square (0-63) for the move.
        public int ToSquare => (value >> 6) & 0b111111;

        // Bits from 12-16 represent the type of move, for example:
        // 0 = quiet move
        // 1 = double pawn push
        // ...
        public int Flags => (value >> 12) & 0b1111;

        public ushort Value => value;

        public Move(int fromSquare, int toSquare, int flags = 0)
        {
            value = (ushort)(fromSquare | (toSquare << 6) | (flags << 12));
        }
    }
}