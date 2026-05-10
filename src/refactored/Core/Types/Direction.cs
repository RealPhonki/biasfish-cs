namespace Biasfish.Core
{
    public enum Direction : int
    {
        North = 8,
        East  = 1,
        South = -North,
        West  = -East,

        NorthEast = North + East,
        NorthWest = North + West,
        SouthEast = South + East,
        SouthWest = South + West,
    }
}