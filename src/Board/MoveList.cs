namespace Biasfish.Core
{
    /// <summary>
    /// Represents a set of moves and handles all related metadata.
    /// </summary>
    public ref struct MoveList
    {
        // Represents the list of moves.
        private Span<Move> moves;

        // Represents the number of move elements.
        public int Length;

        public MoveList(Span<Move> allocatedMemory)
        {
            moves = allocatedMemory;
            Length = 0;
        }

        /// <summary>
        /// Inserts a move element at the next empty index and increments the length.
        /// </summary>
        /// <param name="move">Represents the move to append.</param>
        public void Add(Move move)
        {
            moves[Length] = move;
            Length++;
        }

        /// <summary>
        /// Returns the move element at the specified index.
        /// </summary>
        /// <param name="index">The index of the move to retrieve.</param>
        /// <returns>Represents the move at the specified index.</returns>
        public Move Get(int index)
        {
            return moves[index];
        }
    }
}