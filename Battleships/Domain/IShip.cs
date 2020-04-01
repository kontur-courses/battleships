using System.Collections.Generic;

namespace Battleships.Domain
{
    public interface IShip
    {
        Direction Direction { get; }
        Point? Position { get; }
        int Size { get; }

        IReadOnlyList<Point> GetPositionPoints();
    }
}