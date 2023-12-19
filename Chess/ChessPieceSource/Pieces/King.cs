using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class King : Piece
    {
        public King(bool isBlack, int x, int y, ChessPiece chessPiece) : base(isBlack, x, y, chessPiece) {
        
        }
        public override string imgSource()
        {
            return isBlack ? "/ChessPieceImage/BlackKing.png" : "/ChessPieceImage/WhiteKing.png";
        }

        private List<Tile> updatePossibleTiles() {
            List<Tile> possibleTiles = new List<Tile>();

            //get all tiles that the king can attack
            possibleTiles.Add(new Tile(tile.x -1, tile.y -1));
            possibleTiles.Add(new Tile(tile.x, tile.y -1));
            possibleTiles.Add(new Tile(tile.x +1, tile.y - 1));

            possibleTiles.Add(new Tile(tile.x -1, tile.y));
            possibleTiles.Add(new Tile(tile.x +1, tile.y));

            possibleTiles.Add(new Tile(tile.x + 1, tile.y+1));
            possibleTiles.Add(new Tile(tile.x, tile.y+1));
            possibleTiles.Add(new Tile(tile.x - 1, tile.y+1));

           return possibleTiles;
        }

        public override Tile[] validMoves(Board board)
        {
            PossibleTiles = updatePossibleTiles();
            List<Tile> RemovedTiles = new List<Tile>();

            //filter out of bounds and ally occupied tiles
            filterStandardValidTiles(PossibleTiles, board.getPieceSet());
            
            //check for spots that are threatened by opposing pieces
            foreach (Tile Possibletile in PossibleTiles) {
                foreach (Tile tile in board.tileSet) {
                    if (Board.isSameTile(Possibletile, tile)) {
                        if (this.chessPiece.pieceColour == PieceColour.Black && tile.isProtectedByWhite > 0)
                            RemovedTiles.Add(Possibletile);
                        else if (this.chessPiece.pieceColour == PieceColour.White && tile.isProtectedByBlack > 0)
                            RemovedTiles.Add(Possibletile);
                    }
                }
            }

            removeTiles(RemovedTiles);

            if (canCastle(board)) {
                if (this.chessPiece.pieceColour == PieceColour.White)
                    PossibleTiles.Add(new Tile(this.tile.x + 2, this.tile.y));
                else
                    PossibleTiles.Add(new Tile(this.tile.x + 2, this.tile.y));
            }
                

            return PossibleTiles.ToArray();
        }

        public override Tile[] protectedTiles(Board board)
        {
            ProtectedTiles.Clear();
            foreach (Tile tile in updatePossibleTiles()) {
                if (tile.x >= Board.WIDTH_MIN &&
                   tile.x < Board.WIDTH_MAX &&
                   tile.y >= Board.HEIGHT_MIN &&
                   tile.y < Board.HEIGHT_MAX)
                    ProtectedTiles.Add(tile);                   
            }
            return ProtectedTiles.ToArray();
        }

        public bool canCastle(Board board) {
            Piece rook;
            Piece piece;
            Tile tile;
            if (this.chessPiece.pieceColour == PieceColour.White)
            {
                //check if spot where rook is, is occupied
                if (!board.isOccupied(new Tile(Board.WIDTH_MAX - 1, Board.HEIGHT_MAX - 1)))
                    return false;

                //check if rook is in right spot{
                if (board.getPieceSet()[Board.WIDTH_MAX - 1, Board.HEIGHT_MAX - 1].chessPiece.pieceType != PieceType.Rook)
                    return false;

                rook = board.getPieceSet()[Board.WIDTH_MAX - 1, Board.HEIGHT_MAX - 1];

                //check if rook and king has not moved
                if (rook.hasMoved || this.hasMoved)
                    return false;

                //check if king is not in check
                if (Game.checkStatus == CheckStatus.WhiteCheck)
                    return false;

                //checking tiles between rook and king
                for (int x = this.tile.x + 1; x < rook.tile.x; x++)
                {

                    piece = board.getPiece(tile = new Tile(x, Board.HEIGHT_MAX - 1));

                    //check if tiles are empty and not protected by black
                    if ( piece != null)
                        return false;

                    if (board.tileSet[tile.x, tile.y].isProtectedByBlack > 0)
                        return false;
                }                                      
                
            }           
            else {

                //check if spot where rook is, is occupied
                if (!board.isOccupied(new Tile(Board.WIDTH_MAX - 1, Board.HEIGHT_MIN)))
                    return false;

                //check if rook is in right spot{
                if (board.getPieceSet()[Board.WIDTH_MAX - 1, Board.HEIGHT_MIN].chessPiece.pieceType != PieceType.Rook)
                    return false;

                rook = board.getPieceSet()[Board.WIDTH_MAX - 1, Board.HEIGHT_MIN];

                //check if rook and king has not moved
                if (rook.hasMoved || this.hasMoved)
                    return false;

                //check if king is not in check
                if (Game.checkStatus == CheckStatus.BlackCheck)
                    return false;

                //checking tiles between rook and king
                for (int x = this.tile.x + 1; x < rook.tile.x; x++)
                {
                    
                    piece = board.getPiece( tile = new Tile(x, Board.HEIGHT_MIN));

                    //check if tiles are empty and not protected by black
                    if (piece != null)
                        return false;


                    if (board.tileSet[tile.x, tile.y].isProtectedByBlack > 0)
                        return false;
                }
            }

            return true;
        }
    }
}
