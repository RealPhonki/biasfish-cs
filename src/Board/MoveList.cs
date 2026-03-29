

namespace Biasfish.Core
{
    public ref struct MoveList
    {
        private Span<Move> moves;
        public int Count;

        public int Length => moves.Length;

        public MoveList(Span<Move> allocatedMemory)
        {
            moves = allocatedMemory;
            Count = 0;
        }

        public void Add(Move move)
        {
            moves[Count] = move;
            Count++;
        }

        public Move Get(int index)
        {
            return moves[index];
        }
    }
}