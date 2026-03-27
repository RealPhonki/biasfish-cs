namespace Biasfish.Core
{
    public class Board
    {
        // bitboards[piece]
        public ulong[] bitboards = new ulong[12];
        // occupied[color]
        public ulong[] occupied = new ulong[2];
        public ulong occupiedAll = 0;

        public int sideToMove = Colors.White;
        public int epSquare = 64;
        public int castlingRights = 15;
        public int halfMoveClock = 0;

        public ulong key;
    }
}