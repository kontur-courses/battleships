using System.Collections.Generic;

namespace Battleships.Domain
{
    public class GameOptions
    {
        private int[] sizes = new[] { 1, 1, 1, 1, 2, 2, 2, 3, 3, 4 };

        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public void SetShipSizes(params int[] sizes)
        {
            this.sizes = sizes;
        }

        public Field CreateField()
        {
            return new Field(Width, Height);
        }

        public IEnumerable<Ship> CreateFleet()
        {
            foreach (var size in sizes)
                yield return new Ship(size);
        }
    }
}
