using System;
using System.Linq;

namespace Battleships.Domain
{
    public class Game
    {
        private GameStage stage = GameStage.NotStarted;
        private Player firstPlayer = null;
        private Player secondPlayer = null;
        private bool isFirstPlayerCurrent = false;
        private GameOptions options = new GameOptions();

        public Game(Action<GameOptions> configureOptions = null)
        {
            configureOptions?.Invoke(options);
        }

        public GameStage Stage => stage;
        public event Action<GameStage> StageChanged;

        public IPlayer FirstPlayer => firstPlayer;
        public IPlayer SecondPlayer => secondPlayer;
        public IPlayer CurrentPlayer => isFirstPlayerCurrent ? firstPlayer : secondPlayer;
        public event Action<IPlayer> CurrentPlayerChanged;

        public event Action ReadyToShoot;

        public void Start(string firstPlayerName, string secondPlayerName)
        {
            firstPlayer = CreatePlayer(firstPlayerName);
            secondPlayer = CreatePlayer(secondPlayerName);
            isFirstPlayerCurrent = true;
            CurrentPlayerChanged?.Invoke(CurrentPlayer);
            ChangeStage(GameStage.ArrangingShips);
        }

        public bool CanEndArrangingCurrentPlayerShips =>
            Stage == GameStage.ArrangingShips && IsReadyForBattle(CurrentPlayer);

        public void EndArrangingCurrentPlayerShips()
        {
            if (!CanEndArrangingCurrentPlayerShips)
                return;

            if (!CanBeginBattle)
            {
                MoveToNextPlayer();
                return;
            }

            MoveToNextPlayer();
            ChangeStage(GameStage.Battle);
        }

        public bool CanBeginBattle =>
            Stage == GameStage.ArrangingShips
            && IsReadyForBattle(FirstPlayer) && IsReadyForBattle(SecondPlayer);

        public void ShootTo(Point point)
        {
            if (Stage != GameStage.Battle)
                throw new InvalidOperationException();

            var shotResult = GetNextPlayer().Field.ShootTo(point);
            switch (shotResult)
            {
                case ShotResult.Hit:
                    if (IsCurrentPlayerWin())
                        ChangeStage(GameStage.Finished);
                    else
                        ReadyToShoot?.Invoke();
                    return;
                case ShotResult.Miss:
                    MoveToNextPlayer();
                    ReadyToShoot?.Invoke();
                    return;
                case ShotResult.Cancel:
                    return;
                default:
                    throw new InvalidOperationException();
            }
        }

        private Player CreatePlayer(string name)
        {
            var field = options.CreateField();
            foreach (var ship in options.CreateFleet())
                field.AddShip(ship);
            return new Player(name, field);
        }

        private void ChangeStage(GameStage stage)
        {
            this.stage = stage;
            StageChanged?.Invoke(stage);
        }

        private Player GetNextPlayer() => isFirstPlayerCurrent ? secondPlayer : firstPlayer;

        private void MoveToNextPlayer()
        {
            isFirstPlayerCurrent = !isFirstPlayerCurrent;
            CurrentPlayerChanged?.Invoke(CurrentPlayer);
        }

        private static bool IsReadyForBattle(IPlayer player)
        {
            return player.Field.GetShipToPutOrNull() == null && !player.Field.GetConflictingPoints().Any();
        }

        private bool IsCurrentPlayerWin()
        {
            var nextPlayer = GetNextPlayer();
            return nextPlayer != null
                ? !nextPlayer.Field.HasAliveShips()
                : false;
        }
    }
}
