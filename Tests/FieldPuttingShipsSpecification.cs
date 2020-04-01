using Battleships.Domain;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Tests
{
    // > GetShipToPutOrNull
    // первым предлагается к размещению самый большой корабль
    // предлагается самый большой корабль из оставшихся, если часть кораблей размещено
    // кораблей нет, размещать нечего
    // все корабли размещены

    // > PutShip
    // просто разместить, проверить, что разместился в правильной точке
    // размещение за пределами (горизонталь и вертикаль, меньшая и большая границы)
    // размещение на границе пределов (горизонталь и вертикаль, меньшая и большая границы)
    // размещение размещенного корабля в допустимую позицию
    // размещенного корабля в недопустимую позицию (не перемещается, false)

    // > GetShipsAt
    // корабли не отслеживаются в точке, если не добавлены
    // размещенный корабль должен отслеживаться GetShipAt (горизонталь и вертикаль, в любой из точек)
    // размещенный корабль не должен отслеживаться в остальных точках (горизонталь и вертикаль, горизонтальны и вертикальный корабль)
    // неудачно размещенный корабль не должен отслеживаться
    // несколько кораблей в точке отслеживаются, сортировка по размеру (сначала более короткие)
    // * убранный корабль должен перестать отслеживаться
    // * перемещенный корабль должен отслеживаться в новой точке
    // * дважды добавленный корабль отслеживается только один раз

    // > ChangeShipDirection
    // ничего не делает, если корабль не размещен
    // изменение направления просто меняет его
    // корабль отодвигается для крайне правого вертикального
    // корабль отодвигается для крайне низкого горизонтального
    // если отодвинуться не получается, то корабль удаляется с поля и возвращается false

    // > GetConflictingPoints
    // нет, если нет кораблей
    // нет, если корабль один
    // полное перекрытие в основной точке
    // полное перекрытие в точке окончаний
    // касание рядом с основной точкой
    // касание рядом с точкой окончаний
    // несколько кораблей

    [TestFixture]
    public class FieldPuttingShipsSpecification
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

        [Test]
        public void GetShipToPutOrNull_ShouldReturnBiggestShip_WhenItIsNotPut()
        {
            field.GetShipToPutOrNull().Should().Be(bigShip);
        }

        [Test]
        public void GetShipToPutOrNull_ShouldReturnSecondBiggestShip_WhenBiggestIsPut()
        {
            bigShip.Position = new Point(0, 0);

            field.GetShipToPutOrNull().Should().Be(ship);
        }

        [Test]
        public void GetShipToPutOrNull_ShouldReturnNull_WhenNoShips()
        {
            var field = new Field(10, 10);
            field.GetShipToPutOrNull().Should().BeNull();
        }

        [Test]
        public void GetShipToPutOrNull_ShouldReturnNull_WhenAllShipsPut()
        {
            ship.Position = new Point(0, 0);
            bigShip.Position = new Point(0, 0);
            smallShip.Position = new Point(0, 0);

            field.GetShipToPutOrNull().Should().BeNull();
        }

        [Test]
        public void PutShip_ShouldSetShipPosition()
        {
            var point = new Point(2, 5);
            field.PutShip(ship, point).Should().BeTrue();
            ship.Position.Should().Be(point);
        }

        [Test]
        public void PutShip_ShouldThrow_WhenShipNotAdded()
        {
            var otherShip = new Ship(3);
            var point = new Point(2, 5);
            Action action = () => field.PutShip(otherShip, point);
            action.Should().Throw<InvalidOperationException>();
            otherShip.Position.Should().BeNull();
        }

        [TestCase(0, 5, Direction.Vertical)]
        [TestCase(9, 5, Direction.Vertical)]
        [TestCase(0, 5, Direction.Horizontal)]
        [TestCase(7, 5, Direction.Horizontal)]
        [TestCase(2, 0, Direction.Horizontal)]
        [TestCase(2, 9, Direction.Horizontal)]
        [TestCase(2, 0, Direction.Vertical)]
        [TestCase(2, 7, Direction.Vertical)]
        public void PutShip_ShouldSetShipPosition_WhenJustBeforeOutOfTheBounds(int x, int y, Direction direction)
        {
            var point = new Point(x, y);
            ship.Direction = direction;
            field.PutShip(ship, point).Should().BeTrue();
            ship.Position.Should().Be(point);
        }

        [TestCase(-1, 5, Direction.Vertical)]
        [TestCase(10, 5, Direction.Vertical)]
        [TestCase(-1, 5, Direction.Horizontal)]
        [TestCase(8, 5, Direction.Horizontal)]
        [TestCase(2, -1, Direction.Horizontal)]
        [TestCase(2, 10, Direction.Horizontal)]
        [TestCase(2, -1, Direction.Vertical)]
        [TestCase(2, 8, Direction.Vertical)]
        public void PutShip_ShouldSetShipPosition_WhenOutOfTheBounds(int x, int y, Direction direction)
        {
            var point = new Point(x, y);
            ship.Direction = direction;
            field.PutShip(ship, point).Should().BeFalse();
            ship.Position.Should().BeNull();
        }

        [Test]
        public void PutShip_ShouldResetShipPosition_WhenInside()
        {
            var point = new Point(2, 5);
            field.PutShip(ship, point);

            var secondPoint = new Point(3, 6);
            field.PutShip(ship, secondPoint).Should().BeTrue();
            ship.Position.Should().Be(secondPoint);
        }

        [Test]
        public void PutShip_ShouldRemoveShipPosition_WhenOut()
        {
            var point = new Point(2, 5);
            field.PutShip(ship, point).Should().BeTrue();

            var secondPoint = new Point(10, 5);
            field.PutShip(ship, secondPoint).Should().BeFalse();
            ship.Position.Should().BeNull();
        }

        [Test]
        public void GetShipsAt_ShouldReturnNothing_WhenNoShips()
        {
            var point = new Point(2, 5);
            field.GetShipsAt(point).Should().BeEmpty();
        }

        [TestCase(0, 0, Direction.Horizontal)]
        [TestCase(1, 0, Direction.Horizontal)]
        [TestCase(2, 0, Direction.Horizontal)]
        [TestCase(0, 0, Direction.Vertical)]
        [TestCase(0, 1, Direction.Vertical)]
        [TestCase(0, 2, Direction.Vertical)]
        public void GetShipsAt_ShouldReturnShip_WhenPointInShip(int dx, int dy, Direction direction)
        {
            ship.Direction = direction;
            var point = new Point(2, 5);
            field.PutShip(ship, point);

            var deltaPoint = new Point(point.X + dx, point.Y + dy);
            field.GetShipsAt(deltaPoint).Should().BeEquivalentTo(ship);
        }

        [TestCase(-1, 0, Direction.Horizontal)]
        [TestCase(3, 0, Direction.Horizontal)]
        [TestCase(0, -1, Direction.Horizontal)]
        [TestCase(0, 1, Direction.Horizontal)]
        [TestCase(-1, 0, Direction.Vertical)]
        [TestCase(1, 0, Direction.Vertical)]
        [TestCase(0, -1, Direction.Vertical)]
        [TestCase(0, 3, Direction.Vertical)]
        public void GetShipsAt_ShouldReturnNothing_WhenPointOutOfShip(int dx, int dy, Direction direction)
        {
            ship.Direction = direction;
            var point = new Point(2, 5);
            field.PutShip(ship, point);

            var deltaPoint = new Point(point.X + dx, point.Y + dy);
            field.GetShipsAt(deltaPoint).Should().BeEmpty();
        }

        [Test]
        public void GetShipsAt_ShouldReturnNothing_AfterPutFailed()
        {
            var point = new Point(8, 5);
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, point);

            field.GetShipsAt(point).Should().BeEmpty();
        }

        [Test]
        public void GetShipsAt_ShouldReturnShip_AfterReset()
        {
            var point = new Point(2, 5);
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, point);

            var secondPoint = new Point(3, 6);
            field.PutShip(ship, secondPoint);

            field.GetShipsAt(secondPoint).Should().BeEquivalentTo(ship);
        }

        [Test]
        public void GetShipsAt_ShouldReturnNothing_AfterRemove()
        {
            var point = new Point(2, 5);
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, point);

            var secondPoint = new Point(10, 5);
            field.PutShip(ship, secondPoint);

            field.GetShipsAt(secondPoint).Should().BeEmpty();
        }

        [Test]
        public void GetShipsAt_ShouldReturnShipsFromSmallToBig_WhenSeveralShips()
        {
            var point = new Point(2, 5);
            ship.Direction = Direction.Horizontal;
            bigShip.Direction = Direction.Vertical;

            field.PutShip(ship, point);
            field.PutShip(smallShip, point);
            field.PutShip(bigShip, point);

            field.GetShipsAt(point)
                .Should().BeEquivalentTo(
                    new[] { smallShip, ship, bigShip },
                    config => config.WithStrictOrdering());
        }

        [Test]
        public void GetShipsAt_ShouldReturnOneShip_WhenPutSeveralTimes()
        {
            var point = new Point(2, 5);
            field.PutShip(ship, point);
            field.PutShip(ship, point);

            field.GetShipsAt(point).Should().BeEquivalentTo(ship);
        }

        [Test]
        public void ChangeShipDirection_ShouldThrow_WhenShipNotAdded()
        {
            var otherShip = new Ship(3);
            Action action = () => field.ChangeShipDirection(otherShip);
            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ChangeShipDirection_ShouldDoNoting_WhenNotPut()
        {
            ship.Direction = Direction.Horizontal;
            field.ChangeShipDirection(ship).Should().BeFalse();
            ship.Direction.Should().Be(Direction.Horizontal);
        }

        [TestCase(Direction.Horizontal, Direction.Vertical)]
        [TestCase(Direction.Vertical, Direction.Horizontal)]
        public void ChangeShipDirection_ShouldJustChangeDirection_WhenEnoughSpace(Direction beforeDirection, Direction afterDirection)
        {
            var point = new Point(2, 5);
            ship.Direction = beforeDirection;
            field.PutShip(ship, point);

            field.ChangeShipDirection(ship).Should().BeTrue();
            ship.Direction.Should().Be(afterDirection);
            ship.Position.Should().Be(point);
        }

        [TestCase(9, 5, Direction.Vertical, 7, 5)]
        [TestCase(8, 5, Direction.Vertical, 7, 5)]
        [TestCase(2, 9, Direction.Horizontal, 2, 7)]
        [TestCase(2, 8, Direction.Horizontal, 2, 7)]
        public void ChangeShipDirection_ShouldMoveShip_WhenNotEnoughSpace(int x, int y, Direction direction,
            int newX, int newY)
        {
            var point = new Point(x, y);
            ship.Direction = direction;
            field.PutShip(ship, point);

            field.ChangeShipDirection(ship).Should().BeTrue();
            ship.Position.Should().Be(new Point(newX, newY));
        }

        [Test]
        public void ChangeShipDirection_ShouldRemoveShip_WhenCanNotMoveHorizontalShip()
        {
            var field = new Field(3, 2);
            var ship = new Ship(3);
            field.AddShip(ship);

            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(0, 0));

            field.ChangeShipDirection(ship).Should().BeFalse();
            ship.Position.Should().BeNull();
        }

        [Test]
        public void ChangeShipDirection_ShouldRemoveShip_WhenCanNotMoveVerticalShip()
        {
            var field = new Field(2, 3);
            var ship = new Ship(3);
            field.AddShip(ship);

            ship.Direction = Direction.Vertical;
            field.PutShip(ship, new Point(0, 0));

            field.ChangeShipDirection(ship).Should().BeFalse();
            ship.Position.Should().BeNull();
        }

        [Test]
        public void GetConflictingPoints_ShouldBeEmpty_WhenNoShips()
        {
            field.GetConflictingPoints().Should().BeEmpty();
        }

        [Test]
        public void GetConflictingPoints_ShouldBeEmpty_WhenOneShip()
        {
            var point = new Point(2, 5);
            field.PutShip(ship, point);

            field.GetConflictingPoints().Should().BeEmpty();
        }

        [Test]
        public void GetConflictingPoints_ShouldReturn3Points_WhenMainPointConflict()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(2, 5));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(2, 5),
                    new Point(3, 5),
                    new Point(2, 6));
        }

        [Test]
        public void GetConflictingPoints_ShouldReturn3Points_WhenBackPointConflict()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 8));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(4, 5));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(4, 8),
                    new Point(3, 8),
                    new Point(4, 7));
        }

        [Test]
        public void GetConflictingPoints_ShouldReturn2Points_WhenMainPointTouch()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(3, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(2, 6));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(3, 5),
                    new Point(2, 6));
        }

        [Test]
        public void GetConflictingPoints_ShouldReturn2Points_WhenBackPointTouch()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(1, 8));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(4, 4));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(3, 8),
                    new Point(4, 7));
        }

        [Test]
        public void GetConflictingPoints_ShouldReturnPointsOnce_WhenSeveralShipsInConflict()
        {
            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(2, 5));
            smallShip.Direction = Direction.Horizontal;
            field.PutShip(smallShip, new Point(2, 5));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(2, 5),
                    new Point(3, 5),
                    new Point(2, 6));
        }

        [Test]
        public void GetConflictingPoints_ShouldAllPoints_WhenSeveralConflicts()
        {
            var ship1 = new Ship(1);
            var ship2 = new Ship(2);
            field.AddShip(ship1);
            field.AddShip(ship2);

            ship.Direction = Direction.Horizontal;
            field.PutShip(ship, new Point(2, 5));
            bigShip.Direction = Direction.Vertical;
            field.PutShip(bigShip, new Point(2, 5));
            field.PutShip(ship1, new Point(4, 5));
            field.PutShip(ship2, new Point(2, 8));

            field.GetConflictingPoints()
                .Should().BeEquivalentTo(
                    new Point(2, 5),
                    new Point(2, 6),
                    new Point(2, 7),
                    new Point(2, 8),
                    new Point(3, 8),
                    new Point(3, 5),
                    new Point(4, 5));
        }
    }
}
