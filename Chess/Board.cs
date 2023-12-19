using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess
{
    public class Board
    {
        static public int WIDTH_MAX = 8;
        static public int HEIGHT_MAX = 8;
        static public int WIDTH_MIN = 0;
        static public int HEIGHT_MIN = 0;

        private Piece[,] pieceSet;
        public Tile[,] tileSet;

        public List<Tile> tilesToBlock;

        private Grid grid;

        private List<Tile> highlightedTiles;

        public Board(Grid grid) {
            this.grid = grid;
            this.highlightedTiles = new List<Tile>();
            this.pieceSet = new Piece[WIDTH_MAX, HEIGHT_MAX];
            this.tileSet = new Tile[WIDTH_MAX, HEIGHT_MAX];
            this.tilesToBlock = new List<Tile>();

            for (int x = WIDTH_MIN; x < WIDTH_MAX; x++) {
                for (int y = HEIGHT_MIN; y < HEIGHT_MAX; y++) {
                    tileSet[x, y] = new Tile(x, y);
                }
            }
        }

        public Piece[,] getPieceSet() { return this.pieceSet; }

        public Piece[,] setPieceSet() { throw new NotImplementedException(); }

        public List<Tile> getHighLightedTiles() {
            return this.highlightedTiles;
        }

        public void clearBoard() {
          
            //clear board of the images for each piece
            foreach (Piece piece in pieceSet)
            {
                if (piece != null)
                    getImageInTile(getTileOnBoard(piece.tile)).Source = null;
            }
        }

        public void updateBoard(Move move) {
            //get the to and from 
            Tile from = move.getFrom();
            Tile to = move.getTo();
            Piece movingPiece = getPiece(from);

            //update data
            movingPiece.tile = to;
            setPiece(movingPiece);
            pieceSet[from.x, from.y] = null;

            //update front end
            setImage(movingPiece);

            getImageInTile(getTileOnBoard(from)).Source = null;
        }

        public bool isOccupied(Tile tile) {
            return pieceSet[tile.x, tile.y] != null;
        }

        public bool SetInitialBoard(List<Piece> white, List<Piece> black) {

            //clear the board
            if (Game.gameStatus != GameStatus.Neutral) {
                clearBoard();
            }
                
            //set each piece and its image
            foreach (Piece piece in white) {
                setPiece(piece);
                setImage(piece);
            }
            foreach (Piece piece in black)
            {
                setPiece(piece);
                setImage(piece);
            }
            return false;


        }

        public Piece getPiece(Tile tile) {
            //gets piece according to the tile
            return pieceSet[tile.x,tile.y];
        }

        public bool displayHighlightedTiles(List<Tile> validTiles) {
            //display moves of a clicked piece
            this.highlightedTiles = validTiles;
            Button btn;
            foreach (Tile tile in validTiles) {
                btn = getTileOnBoard(tile);
                btn.Background = (Brush)new BrushConverter().ConvertFrom("#FFFF00DC");
            }
            return true;
        }

        public void clearBoardofHighlights() {
            //clear the highlights from the front end
            Button btn;
            foreach (Tile tile in this.highlightedTiles) {
                //get rtght colour for each cell
                btn = getTileOnBoard(tile);
                if (tile.y % 2 != 0)
                {
                    if (tile.x % 2 != 0)
                        btn.Background = Brushes.White;
                    else
                        btn.Background = (Brush)new BrushConverter().ConvertFrom("#7d8796");
                }
                else {
                    if (tile.x % 2 != 0)
                        btn.Background = (Brush)new BrushConverter().ConvertFrom("#7d8796");
                    else
                        btn.Background = Brushes.White;
                }
            }

            this.highlightedTiles.Clear();
        }

        private Button getTileOnBoard(Tile tile) {
            //get the button from the front end according to tile
            Button btn = (Button)grid.FindName("x" + tile.x + "y" + tile.y);
            return btn;
        }

        private Image getImageInTile(Button btn) {
            //get image object from button
            string imgName = "image" + btn.Name[1].ToString() + btn.Name[3].ToString();
            return (Image)btn.FindName(imgName);
        }

        public void setImage(Piece piece) {

            Button btn = getTileOnBoard(piece.tile);
            Image img = getImageInTile(btn);
            //BitmapImage image = new BitmapImage(new Uri("/ChessPieceSource" + piece.imgSource(), UriKind.Relative));
            //img.Source = image;
            img.Source = new BitmapImage(new Uri(@"/ChessPieceSource" + piece.imgSource(), UriKind.Relative));
        }

        public static bool isSameTile(Tile tile1, Tile tile2) {
            //check if 2 tiles are the same position
            return tile1.x == tile2.x && tile1.y == tile2.y;
        }

        private void setPiece(Piece piece) {
            //set piece in our 2d array
                pieceSet[piece.tile.x, piece.tile.y] = piece;          
        }

        public bool canBlock(List<Tile> tiles) {
            //check if a pieces valid moves can block the path of an offensive move during check
            foreach (Tile tile in tiles) {
                foreach (Tile attack in this.tilesToBlock) {                   
                        if (isSameTile(attack, tile))
                            return true;
                } 
            }
            return false;
        }
    }
}
