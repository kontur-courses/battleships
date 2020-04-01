using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Domain
{
    public class Field : IField
    {
        private readonly HashSet<Ship> ships = new HashSet<Ship>();
        private readonly HashSet<Point> shots = new HashSet<Point>();

        public Field(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public event Action Updated;

        public int Width { get; }
        public int Height { get; }

        public void AddShip(Ship ship)
        {
            ships.Add(ship);
            Updated?.Invoke();
        }

        public IReadOnlyList<IShip> GetShips()
        {
            return ships.ToList();
        }

        public IShip GetShipToPutOrNull()
        {
            return ships
                .Where(ship => !ship.Position.HasValue)
                .OrderByDescending(ship => ship.Size)
                .FirstOrDefault();
        }

        public bool PutShip(IShip ship, Point point)
        {
            if (!ships.Contains(ship))
                throw new InvalidOperationException();
            var actualShip = ship as Ship;

            var dx = 1;
            var dy = 1;
            if (ship.Direction == Direction.Horizontal)
                dx = actualShip.Size;
            else
                dy = actualShip.Size;

            if (0 <= point.X && point.X + dx <= Width
                && 0 <= point.Y && point.Y + dy <= Height)
            {
                actualShip.Position = point;
                Updated?.Invoke();
                return true;
            }
            actualShip.Position = null;
            Updated?.Invoke();
            return false;
        }

        public IReadOnlyList<IShip> GetShipsAt(Point point)
        {
            var result = ships
                .Where(ship => ship.GetPositionPoints().Contains(point))
                .OrderBy(ship => ship.Size)
                .ToList();
            return result;
        }

        public bool ChangeShipDirection(IShip ship)
        {
            if (!ships.Contains(ship))
                throw new InvalidOperationException();
            var actualShip = ship as Ship;

            if (!actualShip.Position.HasValue)
                return false;

            var position = actualShip.Position.Value;
            if (actualShip.Direction == Direction.Horizontal)
            {
                var overflow = position.Y + ship.Size - Height;
                if (overflow > 0)
                {
                    var newPosition = new Point(position.X, position.Y - overflow);
                    if (newPosition.Y < 0)
                    {
                        actualShip.Position = null;
                        Updated?.Invoke();
                        return false;
                    }

                    actualShip.Position = newPosition;
                }
                actualShip.Direction = Direction.Vertical;
            }
            else
            {
                var overflow = position.X + ship.Size - Width;
                if (overflow > 0)
                {
                    var newPosition = new Point(position.X - overflow, position.Y);
                    if (newPosition.X < 0)
                    {
                        actualShip.Position = null;
                        Updated?.Invoke();
                        return false;
                    }

                    actualShip.Position = newPosition;
                }
                actualShip.Direction = Direction.Horizontal;
            }
            Updated?.Invoke();
            return true;
        }

        // конфликт — это когда корабль заходит в окружение другого корабля
        // построить все окружения в виде пар (корабль, окружение), например, в виде dictionary
        // для каждого корабля найти все точки, которые пересекаются с чужими окружениями
        public ISet<Point> GetConflictingPoints()
        {
            var shipToRoundMap = ships.ToDictionary(ship => ship, GetShipRoundPoints);

            var result = new HashSet<Point>();
            foreach (var ship in ships)
            {
                var positionPoints = ship.GetPositionPoints();
                foreach (var point in positionPoints)
                {
                    var isPointInOtherShipRound = shipToRoundMap
                        .Any(pair => !pair.Key.Equals(ship) && pair.Value.Contains(point));
                    if (isPointInOtherShipRound)
                        result.Add(point);
                }
            }
            return result;
        }

        public ShotResult ShootTo(Point point)
        {
            if (shots.Contains(point))
                return ShotResult.Cancel;

            shots.Add(point);

            var ship = GetShipsAt(point).FirstOrDefault();
            if (ship == null)
            {
                Updated?.Invoke();
                return ShotResult.Miss;
            }

            var willBlow = ship.GetPositionPoints()
                .All(p => shots.Contains(p));

            if (willBlow)
                shots.UnionWith(GetShipRoundPoints(ship));

            Updated?.Invoke();
            return ShotResult.Hit;
        }

        private HashSet<Point> GetShipRoundPoints(IShip ship)
        {
            var result = ship.GetPositionPoints()
                .SelectMany(p => p.GetRoundPoints())
                .Where(p => 0 <= p.X && p.X < Width && 0 <= p.Y && p.Y < Height)
                .ToHashSet();
            return result;
        }

        public ISet<Point> GetShots()
        {
            return shots.ToHashSet();
        }

        public bool IsAlive(IShip ship)
        {
            if (!ships.Contains(ship))
                throw new InvalidOperationException();

            return ship.GetPositionPoints().Any(p => !shots.Contains(p));
        }

        public bool HasAliveShips()
        {
            return ships.Any(ship => IsAlive(ship));
        }
    }
}
