using Battleships.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    // > AddShip
    // > GetShips
    [TestFixture]
    public class FieldAddingShipsSpecification
    {
        private Field field;
        private Ship ship;
        private Ship bigShip;
        private Ship smallShip;

        [SetUp]
        public void SetUp()
        {
            field = new Field(10, 10);
            ship = new Ship(3);
            bigShip = new Ship(4);
            smallShip = new Ship(1);
        }

        [Test]
        public void GetShips_ShouldReturnNothing_WhenNotAdded()
        {
            field.GetShips().Should().BeEmpty();
        }

        [Test]
        public void GetShips_ShouldReturnAddedShips()
        {
            field.AddShip(ship);
            field.AddShip(bigShip);
            field.AddShip(smallShip);

            field.GetShips().Should().BeEquivalentTo(ship, bigShip, smallShip);
        }

        [Test]
        public void GetShips_ShouldNotReturnDuplicates()
        {
            field.AddShip(ship);
            field.AddShip(ship);

            field.GetShips().Should().BeEquivalentTo(ship);
        }
    }
}
