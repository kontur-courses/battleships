using NUnit.Framework;
using Battleships.Domain;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class GameSpecification
    {
        [Test]
        public void Constructor_ShouldCreateNotStarted()
        {
            var game = new Game();
            game.Stage.Should().Be(GameStage.NotStarted);
            game.FirstPlayer.Should().BeNull();
            game.SecondPlayer.Should().BeNull();
            game.CurrentPlayer.Should().BeNull();
        }

        [Test]
        public void Game_ShouldPlay()
        {
            var game = new Game(options =>
            {
                options.SetShipSizes(3);
                options.Width = 10;
                options.Height = 10;
            });

            game.Start("first", "second");

            game.Stage.Should().Be(GameStage.ArrangingShips);

            var ship1 = game.CurrentPlayer.Field.GetShipToPutOrNull();
            game.CurrentPlayer.Field.PutShip(ship1, new Point(2, 5));
            game.EndArrangingCurrentPlayerShips();

            game.Stage.Should().Be(GameStage.ArrangingShips);

            var ship2 = game.CurrentPlayer.Field.GetShipToPutOrNull();
            game.CurrentPlayer.Field.PutShip(ship2, new Point(2, 5));
            game.EndArrangingCurrentPlayerShips();

            game.Stage.Should().Be(GameStage.Battle);

            game.ShootTo(new Point(2, 5));
            game.ShootTo(new Point(3, 5));
            game.ShootTo(new Point(4, 5));

            game.Stage.Should().Be(GameStage.Finished);
        }

        // # Другие возможные тесты для Game
        //  
        // события происходят: стадии меняются, игроки меняются

        // старт меняет стадию и инициализирует игроков (у них есть поля и корабли), первый игрок - текущий
        // в упорядочении нельзя перейти к следующему игроку, если есть конфликты или не все корабли расставлены у первого игрока
        // в упорядочении нельзя перейти к бою, если есть конфликты или не все корабли расставлены у второго игрока (и у первого тоже)
        // переход хода в бою при промахе (первый и второй игрок)
        // ход остается при попадании (первый и второй игрок)
        // победа (первый и второй игрок, чистая и когда у каждого последний выстрел)

        // CanEndArrangingCurrentPlayerShips - false не на стадии упорядочения
        // CanBeginBattle - false не на стадии упорядочения

        // нельзя стрелять до боя
    }
}
