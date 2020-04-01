using Battleships.Domain;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Tests
{
    // > ShootTo
    // промах, попадание, взрыв
    // повторы: промах, попадание, в осколки взрыва

    // > GetShots
    // промах
    // попадание
    // взрыв в открытом море
    // взрыв в каждом из углов
    // без повторов: попадание, в осколки взрыва
    // объединяют несколько выстрелов
    // объединяют несколько взрывов

    [TestFixture]
    public class FieldShootingSpecification
    {
        private Field field;
        private Ship ship;
        private Ship bigShip;
        private Ship smallShip;

        [SetUp]
        public void SetUp()
        {
            ship = new Ship(3);
            bigShip = new Ship(4);
            smallShip = new Ship(1);

            field = new Field(10, 10);

            field.AddShip(ship);
            field.AddShip(bigShip);
            field.AddShip(smallShip);
        }

        [TestCase(1, 5)]
        [TestCase(5, 5)]
        [TestCase(2, 4)]
        [TestCase(2, 6)]
        public void ShootTo_ShouldReturnMiss_WhenMiss(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint)
                .Should().Be(ShotResult.Miss);
        }

        [TestCase(1, 5)]
        [TestCase(5, 5)]
        [TestCase(2, 4)]
        [TestCase(2, 6)]
        public void GetShots_ShouldReturnOneShot_WhenMiss(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.GetShots()
                .Should().BeEquivalentTo(shotPoint);
        }

        [TestCase(1, 5)]
        [TestCase(5, 5)]
        [TestCase(2, 4)]
        [TestCase(2, 6)]
        public void ShootTo_ShouldReturnCancel_WhenMissAgain(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.ShootTo(shotPoint)
                .Should().Be(ShotResult.Cancel);
        }

        [TestCase(1, 5)]
        [TestCase(5, 5)]
        [TestCase(2, 4)]
        [TestCase(2, 6)]
        public void GetShots_ShouldNotDuplicateShot_WhenMissAgain(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.ShootTo(shotPoint);
            field.GetShots()
                .Should().BeEquivalentTo(shotPoint);
        }

        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]
        public void ShootTo_ShouldReturnHit_WhenHit(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint)
                .Should().Be(ShotResult.Hit);
        }

        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]
        public void GetShots_ShouldReturnOneShot_WhenHit(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.GetShots()
                .Should().BeEquivalentTo(shotPoint);
        }

        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]
        public void ShootTo_ShouldReturnCancel_WhenHitAgain(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.ShootTo(shotPoint)
                .Should().Be(ShotResult.Cancel);
        }

        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]
        public void GetShots_ShouldNotDuplicateShot_WhenHitAgain(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.ShootTo(shotPoint);
            field.GetShots()
                .Should().BeEquivalentTo(shotPoint);
        }

        [Test]
        public void GetShots_ShouldNotBlow_WhenLastShotNeeded()
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.GetShots()
                .Should().BeEquivalentTo(new Point(2, 5), new Point(3, 5));
        }

        [Test]
        public void ShootTo_ShouldReturnHit_WhenBlowing()
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));

            field.ShootTo(new Point(4, 5))
                .Should().Be(ShotResult.Hit);
        }

        [Test]
        public void GetShots_ShouldReturnManyShots_WhenBlowingInCenter()
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(5, 4),
                    new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5),
                    new Point(1, 6), new Point(2, 6), new Point(3, 6), new Point(4, 6), new Point(5, 6));
        }

        [TestCase(1, 4), TestCase(2, 4), TestCase(3, 4), TestCase(4, 4), TestCase(5, 4)]
        [TestCase(1, 5), TestCase(2, 5), TestCase(3, 5), TestCase(4, 5), TestCase(5, 5)]
        [TestCase(1, 6), TestCase(2, 6), TestCase(3, 6), TestCase(4, 6), TestCase(5, 6)]
        public void ShootTo_ShouldReturnCancel_WhenHitAfterBlowing(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint).Should().Be(ShotResult.Cancel);
        }

        [TestCase(1, 4), TestCase(2, 4), TestCase(3, 4), TestCase(4, 4), TestCase(5, 4)]
        [TestCase(1, 5), TestCase(2, 5), TestCase(3, 5), TestCase(4, 5), TestCase(5, 5)]
        [TestCase(1, 6), TestCase(2, 6), TestCase(3, 6), TestCase(4, 6), TestCase(5, 6)]
        public void GetShots_ShouldNotDuplicateShots_WhenHitAfterBlowing(int x, int y)
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));

            var shotPoint = new Point(x, y);
            field.ShootTo(shotPoint);
            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(5, 4),
                    new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5),
                    new Point(1, 6), new Point(2, 6), new Point(3, 6), new Point(4, 6), new Point(5, 6));
        }

        [Test]
        public void GetShots_ShouldReturnManyShots_WhenBlowingInTopLeftCorner()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(0, 0));
            field.ShootTo(new Point(0, 0));
            field.ShootTo(new Point(1, 0));
            field.ShootTo(new Point(2, 0));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0),
                    new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1));
        }

        [Test]
        public void GetShots_ShouldReturnManyShots_WhenBlowingInTopRightCorner()
        {
            ship.Direction = Direction.Vertical;
            field.PutShip(ship, new Point(9, 0));
            field.ShootTo(new Point(9, 0));
            field.ShootTo(new Point(9, 1));
            field.ShootTo(new Point(9, 2));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(8, 0), new Point(9, 0),
                    new Point(8, 1), new Point(9, 1),
                    new Point(8, 2), new Point(9, 2),
                    new Point(8, 3), new Point(9, 3));
        }

        [Test]
        public void GetShots_ShouldReturnManyShots_WhenBlowingInBottomLeftCorner()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(7, 9));
            field.ShootTo(new Point(7, 9));
            field.ShootTo(new Point(8, 9));
            field.ShootTo(new Point(9, 9));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(6, 8), new Point(7, 8), new Point(8, 8), new Point(9, 8),
                    new Point(6, 9), new Point(7, 9), new Point(8, 9), new Point(9, 9));
        }

        [Test]
        public void GetShots_ShouldReturnManyShots_WhenBlowingInBottomRightCorner()
        {
            ship.Direction = Direction.Vertical;
            field.PutShip(ship, new Point(0, 7));
            field.ShootTo(new Point(0, 7));
            field.ShootTo(new Point(0, 8));
            field.ShootTo(new Point(0, 9));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(0, 6), new Point(1, 6),
                    new Point(0, 7), new Point(1, 7),
                    new Point(0, 8), new Point(1, 8),
                    new Point(0, 9), new Point(1, 9));
        }

        [Test]
        public void GetShots_ShouldReturnAllShots_WhenTwoHits()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(8, 3));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(8, 5));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(3, 5),
                    new Point(8, 5));
        }

        [Test]
        public void GetShots_ShouldReturnAllShots_WhenMissAndBlowing()
        {
            field.PutShip(smallShip, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(8, 5));

            field.GetShots()
                .Should().BeEquivalentTo(
                    new Point(1, 4), new Point(2, 4), new Point(3, 4),
                    new Point(1, 5), new Point(2, 5), new Point(3, 5),
                    new Point(1, 6), new Point(2, 6), new Point(3, 6),
                    new Point(8, 5));
        }

        [Test]
        public void IsAlive_ShouldThrow_WhenShipNotAdded()
        {
            var otherShip = new Ship(3);
            otherShip.Position = new Point(2, 5);
            Action action = () => field.IsAlive(otherShip);
            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IsAlive_ShouldReturnTrue_WhenShipHasNotHits()
        {
            field.PutShip(ship, new Point(2, 5));
            field.IsAlive(ship).Should().BeTrue();
        }

        [Test]
        public void IsAlive_ShouldReturnTrue_WhenShipHasOneAlivePoint()
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(4, 5));
            field.IsAlive(ship).Should().BeTrue();
        }

        [Test]
        public void IsAlive_ShouldReturnFalse_WhenShipIsShot()
        {
            field.PutShip(ship, new Point(2, 5));
            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));
            field.IsAlive(ship).Should().BeFalse();
        }

        [Test]
        public void HasAlivePoints_ShouldReturnFalse_WhenNoShips()
        {
            field.HasAliveShips().Should().BeFalse();
        }

        [Test]
        public void HasAlivePoints_ShouldReturnTrue_WhenFirstIsAlive()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(8, 3));

            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));

            field.ShootTo(new Point(8, 3));
            field.ShootTo(new Point(8, 4));
            field.ShootTo(new Point(8, 6));

            field.HasAliveShips().Should().BeTrue();
        }

        [Test]
        public void HasAlivePoints_ShouldReturnTrue_WhenLastIsAlive()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(8, 3));

            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));

            field.ShootTo(new Point(8, 3));
            field.ShootTo(new Point(8, 4));
            field.ShootTo(new Point(8, 5));
            field.ShootTo(new Point(8, 6));

            field.HasAliveShips().Should().BeTrue();
        }

        [Test]
        public void HasAlivePoints_ShouldReturnFalse_WhenAllShot()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(8, 3));

            field.ShootTo(new Point(2, 5));
            field.ShootTo(new Point(3, 5));
            field.ShootTo(new Point(4, 5));

            field.ShootTo(new Point(8, 3));
            field.ShootTo(new Point(8, 4));
            field.ShootTo(new Point(8, 5));
            field.ShootTo(new Point(8, 6));

            field.HasAliveShips().Should().BeFalse();
        }
    }
}
