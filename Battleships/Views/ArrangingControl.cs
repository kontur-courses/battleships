using Battleships.AI;
using Battleships.Domain;
using System.Linq;
using System.Windows.Forms;

namespace Battleships.Views
{
    public partial class ArrangingControl : UserControl
    {
        private Game game;

        public ArrangingControl()
        {
            InitializeComponent();
        }

        public void Configure(Game game)
        {
            if (this.game != null)
                return;

            this.game = game;

            endArrangingButton.Click += EndArrangingButton_Click;
            endArrangingButton.Enabled = game.CanEndArrangingCurrentPlayerShips;
            game.FirstPlayer.Field.Updated += () =>
            {
                endArrangingButton.Enabled = game.CanEndArrangingCurrentPlayerShips;
            };

            fieldControl.Configure(game.FirstPlayer.Field, false);
            fieldControl.ClickOnPoint += FieldControl_ClickOnPoint;
        }

        private void FieldControl_ClickOnPoint(Point point, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Right)
            {
                fieldControl.SetSelectedShip(null);
                return;
            }

            if (args.Button == MouseButtons.Left)
            {
                var selectedShip = fieldControl.SelectedShip;
                if (selectedShip != null)
                {
                    if (selectedShip.GetPositionPoints().Contains(point))
                        game.CurrentPlayer.Field.ChangeShipDirection(selectedShip);
                    else
                        game.CurrentPlayer.Field.PutShip(selectedShip, point);
                    fieldControl.SetSelectedShip(null);
                    return;
                }

                var shipAt = game.CurrentPlayer.Field.GetShipsAt(point).FirstOrDefault();
                if (shipAt != null)
                {
                    fieldControl.SetSelectedShip(shipAt);
                    return;
                }

                var shipToPut = game.CurrentPlayer.Field.GetShipToPutOrNull();
                if (shipToPut != null)
                {
                    game.CurrentPlayer.Field.PutShip(shipToPut, point);
                    return;
                }
            }
        }

        private void EndArrangingButton_Click(object sender, System.EventArgs e)
        {
            if (game.CurrentPlayer.Equals(game.FirstPlayer))
                game.EndArrangingCurrentPlayerShips();

            if (!game.CurrentPlayer.Equals(game.FirstPlayer))
            {
                if (game.CurrentPlayer.Field.ArrangeShipsAutomatically())
                    game.EndArrangingCurrentPlayerShips();
                else
                    MessageBox.Show("Не удалось разместить корабли AI, попробуйте еще раз", "Сообщение", MessageBoxButtons.OK);
            }
        }
    }
}
