using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Game Chess;

       

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            //make buttons visible and hide start game button
            Button btnClicked = (Button)sender;

            btnClicked.Visibility = Visibility.Collapsed;

            ResignButton.Visibility = Visibility.Visible;
            ResetButton.Visibility = Visibility.Visible;
            DrawButton.Visibility = Visibility.Visible;

            Chess = new Game(this.ChessBoard);
            
            Chess.initializeGame();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            //only allow if game is in play
            if (Game.gameStatus != GameStatus.Play)
                return;

            var Button = (Button)sender;
            Tile tileClicked = new Tile(Button.Name[1].ToString(),Button.Name[3].ToString());

            Chess.processCommand(tileClicked);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //reset the board and all pieces
            Game.gameStatus = GameStatus.Play;
            Chess.resetBoard(this.ChessBoard);
            Banner.Text = "";
            Chess.initializeGame();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            //end game with draw
            if (Game.gameStatus == GameStatus.Play) { 
                Game.gameStatus = GameStatus.Draw;
                Banner.Text = "Draw!";
            }
            
        }

        private void ResignButton_Click(object sender, RoutedEventArgs e)
        {
            //end game by resignation
            if (Game.playerTurn == PieceColour.White)
            { 
                Game.gameStatus = GameStatus.BlackWin;
                Banner.Text = "Black Wins by Resignation!";
            }              
            else
            { 
             Game.gameStatus = GameStatus.WhiteWin;
                Banner.Text = "White Wins by Resignation!";
            }
               
        }
    }
}
