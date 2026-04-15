using System.Runtime.CompilerServices;

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
        public int count;

        public MoveList(Span<Move> allocatedMemory)
        {
            moves = allocatedMemory;
            count = 0;
        }

        /// <summary>
        /// Inserts a move element at the next empty index and increments the length.
        /// </summary>
        /// <param name="move">Represents the move to append.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Move move)
        {
            // Length++ is evaluated after indexing.
            // If written as `++Length` it would be evaluated before (causing errors)
            moves[count++] = move;
        }

        public Move this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => moves[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => moves[index] = value;
        }
    }
}