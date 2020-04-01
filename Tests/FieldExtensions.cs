using NUnit.Framework;
using Battleships.Domain;
using FluentAssertions;
using Battleships.AI;

namespace Tests
{
    [TestFixture]
    public class FieldExtensionsSpecification
    {
        [Test]
        public void ArrangeShipsAutomatically_ShouldArrange()
        {
            var options = new GameOptions();
            var field = options.CreateField();
            var fleet = options.CreateFleet();
            foreach (var ship in fleet)
                field.AddShip(ship);

            var result = field.ArrangeShipsAutomatically();
            result.Should().BeTrue();
        }
    }
}
