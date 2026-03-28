namespace Biasfish.Core
{
    public readonly struct Move
    {
        private readonly ushort value;

        public int FromSquare => value & 0b111111;
        public int ToSquare => (value >> 6) & 0b111111;
        public int Flags => (value >> 12) & 0b1111;
        public ushort Value => value;

        public Move(int fromSquare, int toSquare, int flags = 0)
        {
            value = (ushort)(fromSquare | (toSquare << 6) | (flags << 12));
        }
    }
}