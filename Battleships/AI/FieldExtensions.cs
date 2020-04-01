using Battleships.Domain;
using System;
using System.Linq;

namespace Battleships.AI
{
    public static class FieldExtensions
    {
        private static Random random = new Random();

        public static Point GenerateRandomShot(this IField field)
        {
            var shots = field.GetShots();

            for (var i = 0; i < 1000; i++)
            {
                var x = random.Next(0, field.Width);
                var y = random.Next(0, field.Height);
                var point = new Point(x, y);
                if (!shots.Contains(point))
                    return point;
            }

            for (var x = 0; x < field.Width; x++)
            {
                for (var y = 0; y < field.Height; y++)
                {
                    var point = new Point(x, y);
                    if (!shots.Contains(point))
                        return point;
                }
            }

            return new Point(0, 0);
        }

        public static bool ArrangeShipsAutomatically(this IField field)
        {
            for (var i = 0; i < 1000; i++)
            {
                field.RemoveAllShips();
                if (field.TryArrangeShips(1000))
                    return true;
            }
            return false;
        }

        private static void RemoveAllShips(this IField field)
        {
            foreach (var ship in field.GetShips().Where(it => it.Position != null))
                field.PutShip(ship, new Point(-1, -1));
        }

        private static bool TryArrangeShips(this IField field, int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                var ship = field.GetShipToPutOrNull();
                if (ship == null)
                    break;
                var x = random.Next(0, field.Width);
                var y = random.Next(0, field.Height);
                field.PutShip(ship, new Point(x, y));
                if (random.Next(0, 2) == 1)
                    field.ChangeShipDirection(ship);

                if (field.GetConflictingPoints().Any())
                    field.PutShip(ship, new Point(-1, -1));
            }

            return field.GetShipToPutOrNull() == null
                && !field.GetConflictingPoints().Any();
        }
    }
}
