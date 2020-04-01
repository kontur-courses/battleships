using System;
using System.Collections.Generic;

namespace Battleships.Domain
{
    public interface IField
    {
        int Height { get; }
        int Width { get; }

        bool ChangeShipDirection(IShip ship);
        ISet<Point> GetConflictingPoints();
        IReadOnlyList<IShip> GetShips();
        IReadOnlyList<IShip> GetShipsAt(Point point);
        IShip GetShipToPutOrNull();
        ISet<Point> GetShots();
        bool HasAliveShips();
        bool IsAlive(IShip ship);
        bool PutShip(IShip ship, Point point);

        event Action Updated;
    }
}