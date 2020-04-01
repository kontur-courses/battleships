using NUnit.Framework;
using Battleships.Domain;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class ShipSpecification
    {
        [Test]
        public void Constructor_ShouldCreateNotPlacedHorizontalShip()
        {
            var ship = new Ship(3);
            ship.Size.Should().Be(3);
            ship.Direction.Should().Be(Direction.Horizontal);
            ship.Position.Should().BeNull();
        }

        [Test]
        public void GetPositionPoints_ShouldReturnNothing_WhenNotPlaced()
        {
            var ship = new Ship(3);
            ship.Position = null;
            ship.GetPositionPoints().Should().BeEmpty();
        }

        [Test]
        public void GetPositionPoints_ShouldBeOk_WhenPlacedHorizontally()
        {
            var ship = new Ship(3);
            ship.Position = new Point(2, 5);
            ship.Direction = Direction.Horizontal;

            ship.GetPositionPoints().Should().BeEquivalentTo(new[] {
                new Point(2, 5),
                new Point(3, 5),
                new Point(4, 5),
            });
        }

        [Test]
        public void GetPositionPoints_ShouldBeOk_WhenPlacedVertically()
        {
            var ship = new Ship(3);
            ship.Position = new Point(2, 5);
            ship.Direction = Direction.Vertical;

            ship.GetPositionPoints().Should().BeEquivalentTo(new[] {
                new Point(2, 5),
                new Point(2, 6),
                new Point(2, 7),
            });
        }
    }
}
