using NUnit.Framework;
using Battleships.Domain;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class ExtensionsSpecification
    {
        [Test]
        public void GetRoundPoints_ShouldReturnRound_WhenAnyPoint()
        {
            var point = new Point(2, 5);
            point.GetRoundPoints().Should().BeEquivalentTo(
                new Point(1, 4), new Point(2, 4), new Point(3, 4),
                new Point(1, 5), new Point(2, 5), new Point(3, 5),
                new Point(1, 6), new Point(2, 6), new Point(3, 6));
        }
    }
}
