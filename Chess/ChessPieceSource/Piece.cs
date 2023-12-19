using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Piece
    {
        public bool isBlack;
        public bool hasMoved;
        public bool isSafe { get; set; }
        public ChessPiece chessPiece { get; set; }

        protected List<Tile> PossibleTiles;
        protected List<Tile> ProtectedTiles;

        private bool isPinned;

        private Tile _tile;

        public Tile tile { get { return _tile; }
            set {
                _tile = value;
            }
        }

        public Piece(bool isBlack,int x, int y, ChessPiece chessPiece) {
            this.isBlack = isBlack;
            tile = new Tile(x, y);
            this.chessPiece = chessPiece;
            this.hasMoved = false;
            this.ProtectedTiles = new List<Tile>();
            this.PossibleTiles = new List<Tile>();
        }

        protected void removeTiles(List<Tile> removedTiles)
        {
            foreach (Tile tile in removedTiles)
                PossibleTiles.Remove(tile);
        }

        protected void validTilesDuringCheck(List<Tile> tilesToBlock) {
            //checking if any of the moves can block one of the thiles to block during check mode
            List<Tile> removedTiles = new List<Tile>();
            bool doesExist = false;
            if (this.chessPiece.pieceColour == PieceColour.Black && Game.checkStatus == CheckStatus.BlackCheck)
            {
                for (int i = 0; i < PossibleTiles.Count; i++)
                {
                    doesExist = false;
                    foreach (Tile tile in tilesToBlock) {

                        if (Board.isSameTile(tile, PossibleTiles.ElementAt(i)))
                            doesExist = true;
                    }
                    if (!doesExist)
                        removedTiles.Add(PossibleTiles.ElementAt(i));
                }
            }
            else if (this.chessPiece.pieceColour == PieceColour.White && Game.checkStatus == CheckStatus.WhiteCheck)
            {
                for (int i = 0; i < PossibleTiles.Count; i++)
                {
                    doesExist = false;
                    foreach (Tile tile in tilesToBlock)
                    {
                        if (Board.isSameTile(tile, PossibleTiles.ElementAt(i)))
                            doesExist = true;
                    }
                    if(!doesExist)
                        removedTiles.Add(PossibleTiles.ElementAt(i));
                }
            }

            removeTiles(removedTiles);
        }

        public List<Tile> filterStandardValidTiles(List<Tile> possibleTiles, Piece[,] pieceSet)
        {
            //checking if out of bounds and if friendly pieces in the way
            List<Tile> removedTiles = new List<Tile>();
            foreach (Tile tile in possibleTiles)
            {
                if (tile.x >= Board.WIDTH_MAX || tile.x < Board.WIDTH_MIN || tile.y >= Board.HEIGHT_MAX || tile.y < Board.HEIGHT_MIN)
                { removedTiles.Add(tile); continue; }

                //check if colliding with friendly pieces
                if (pieceSet[tile.x, tile.y] != null)
                    if (pieceSet[tile.x, tile.y].chessPiece.pieceColour == Game.playerTurn)
                        removedTiles.Add(tile);
            }

            removeTiles(removedTiles);

            return possibleTiles;
        }

        public void incrementProtectedTiles(Tile[,] tileSet) {
            foreach (Tile tile in ProtectedTiles) {
                if (this.chessPiece.pieceColour == PieceColour.Black)
                    tileSet[tile.x, tile.y].isProtectedByBlack++;
                else
                    tileSet[tile.x, tile.y].isProtectedByWhite++;
            }
        }

        public void decrementProtectedTiles(Tile[,] tileSet) {
            foreach (Tile tile in ProtectedTiles)
            {
                if (this.chessPiece.pieceColour == PieceColour.Black)
                    tileSet[tile.x, tile.y].isProtectedByBlack--;
                else
                    tileSet[tile.x, tile.y].isProtectedByWhite--;
            }
        }

        public bool isEnemyKingInCheck(Board board) {
            //check all the protected areas
           for(int i =0; i < ProtectedTiles.Count; i++) {
                if (!board.isOccupied(ProtectedTiles.ElementAt(i)))
                    continue;
                ChessPiece cp = board.getPiece(ProtectedTiles.ElementAt(i)).chessPiece;
                //check if piece is a king
                if (cp.pieceType == PieceType.King && cp.pieceColour != this.chessPiece.pieceColour) {
                    board.tilesToBlock.Clear();
                    if (cp.pieceColour == PieceColour.Black)
                        Game.checkStatus = CheckStatus.BlackCheck;
                    else
                        Game.checkStatus = CheckStatus.WhiteCheck;

                    //get all tiles that can be blocked
                    for (int y = i; y > 0; y--) {
                        //check if tile is adjacent to piece 
                        if (ifAdjacentTile(ProtectedTiles.ElementAt(y))) {   
                            if(y!=i)
                                board.tilesToBlock.Add(ProtectedTiles.ElementAt(y));
                            break;
                        }
                        if(y != i)
                            board.tilesToBlock.Add(ProtectedTiles.ElementAt(y));
                    }

                    return true;
                }
           }

            return false;
        }

        //checking if tile is adjacent to piece
        private bool ifAdjacentTile(Tile tile) {

            if (this.tile.x == tile.x && this.tile.y + 1 == tile.y)
                return true;
            else if (this.tile.x + 1 == tile.x && this.tile.y + 1 == tile.y)
                return true;
            else if(this.tile.x - 1 == tile.x && this.tile.y + 1 == tile.y)
                return true;
            else if(this.tile.x + 1 == tile.x && this.tile.y == tile.y)
                return true;
            else if(this.tile.x + 1 == tile.x && this.tile.y - 1 == tile.y)
                return true;
            else if(this.tile.x  == tile.x && this.tile.y - 1 == tile.y)
                return true;
            else if (this.tile.x - 1 == tile.x && this.tile.y - 1 == tile.y)
                return true;
            else if (this.tile.x - 1 == tile.x && this.tile.y == tile.y)
                return true;

            return false;

        }

        public abstract string imgSource();

        public abstract Tile[] protectedTiles(Board board);
        public abstract Tile[] validMoves(Board board);


    }
}
