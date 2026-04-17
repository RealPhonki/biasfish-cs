namespace Biasfish.V2
{
    public ref struct MoveList
    {
        private Span<Move> moves;

        public int count;

        public MoveList(Span<Move> allocatedMemory)
        {
            moves = allocatedMemory;
            count = 0;
        }

        public void Add(Move move)
        {
            moves[count++] = move;
        }

        public Move this[int index]
        {
            get => moves[index];
            set => moves[index] = value;
        }
    }
}