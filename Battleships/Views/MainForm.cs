using Battleships.Domain;
using System.Windows.Forms;

namespace Battleships.Views
{
    public partial class MainForm : Form
    {
        private Game game;

        public MainForm()
        {
            InitializeComponent();

            game = new Game();
            game.StageChanged += Game_OnStageChanged;

            ShowStartScreen();
        }

        private void Game_OnStageChanged(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.ArrangingShips:
                    ShowArrangingShipsScreen();
                    break;
                case GameStage.Battle:
                    ShowBattleScreen();
                    break;
                case GameStage.Finished:
                    ShowFinishedScreen();
                    break;
                case GameStage.NotStarted:
                default:
                    ShowStartScreen();
                    break;
            }
        }

        private void ShowStartScreen()
        {
            HideScreens();
            startControl.Configure(game);
            startControl.Show();
        }

        private void ShowArrangingShipsScreen()
        {
            HideScreens();
            arrangingControl.Configure(game);
            arrangingControl.Show();
        }

        private void ShowBattleScreen()
        {
            HideScreens();
            battleControl.Configure(game);
            battleControl.Show();
        }

        private void ShowFinishedScreen()
        {
            HideScreens();
            finishedControl.Configure(game);
            finishedControl.Show();
        }

        private void HideScreens()
        {
            startControl.Hide();
            arrangingControl.Hide();
            battleControl.Hide();
            finishedControl.Hide();
        }
    }
}
