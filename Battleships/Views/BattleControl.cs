using System.Linq;
using System.Windows.Forms;
using Battleships.AI;
using Battleships.Domain;

namespace Battleships.Views
{
    public partial class BattleControl : UserControl
    {
        private Game game;

        public BattleControl()
        {
            InitializeComponent();
        }

        public void Configure(Game game)
        {
            if (this.game != null)
                return;

            this.game = game;

            humanFieldControl.Configure(game.FirstPlayer.Field, false);
            aiFieldControl.Configure(game.SecondPlayer.Field, true);

            aiFieldControl.ClickOnPoint += HumanFieldControl_ClickOnPoint;

            game.ReadyToShoot += Game_ReadyToShoot;
        }

        private void HumanFieldControl_ClickOnPoint(Point point, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                if (game.CurrentPlayer.Equals(game.FirstPlayer))
                    game.ShootTo(point);
            }
        }

        private void Game_ReadyToShoot()
        {
            var humanPlayer = game.FirstPlayer;
            var aiPlayer = game.SecondPlayer;
            if (game.Stage == GameStage.Battle && game.CurrentPlayer.Equals(aiPlayer))
            {
                var shot = humanPlayer.Field.GenerateRandomShot();
                game.ShootTo(shot);
            }
        }
    }
}
