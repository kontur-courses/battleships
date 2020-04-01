using Battleships.Domain;
using System.Windows.Forms;

namespace Battleships.Views
{
    public partial class FinishedControl : UserControl
    {
        private Game game;

        public FinishedControl()
        {
            InitializeComponent();
        }

        public void Configure(Game game)
        {
            if (this.game != null)
                return;

            this.game = game;

            humanFieldControl.Configure(game.FirstPlayer.Field, false);
            aiFieldControl.Configure(game.SecondPlayer.Field, false);

            if (game.FirstPlayer.Field.HasAliveShips())
            {
                winnerLabel.Text = "Победил человек";
            }
            else
            {
                winnerLabel.Text = "Победил AI";
            }
        }
    }
}
