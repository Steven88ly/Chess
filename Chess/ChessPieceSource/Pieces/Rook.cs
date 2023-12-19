using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Rook : Piece
    {
        public Rook(bool isBlack, int x, int y, ChessPiece chessPiece) : base(isBlack, x, y, chessPiece) {}
        public override string imgSource()
        {
            return isBlack ? "/ChessPieceImage/BlackRook.png" : "/ChessPieceImage/WhiteRook.png";
        }

        public override Tile[] protectedTiles(Board board)
        {
            ProtectedTiles.Clear();
            Tile tile;

            //top side
            for (int y = this.tile.y - 1; y >= Board.HEIGHT_MIN; y--)
            {
                tile = new Tile(this.tile.x, y);
                //check if square is empty
                ProtectedTiles.Add(tile);
                if (board.getPiece(tile) != null )
                {                   
                    break;
                }
            }

            //right side
            for (int x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, this.tile.y);
                // If the square is empty
                //if (!board.isOccupied(tile))
                //    ProtectedTiles.Add(tile);
                //check if enemy piece
                ProtectedTiles.Add(tile);
                if (board.getPiece(tile) != null)
                {
                    break;
                }
            }
            //bottom side
            for (int y = this.tile.y + 1; y < Board.HEIGHT_MAX; y++)
            {
                tile = new Tile(this.tile.x, y);
                // If the square is empty
                //if (!board.isOccupied(tile))
                //    ProtectedTiles.Add(tile);
                //check if enemy piece
                ProtectedTiles.Add(tile);
                if (board.getPiece(tile) != null)
                {
                    break;
                }
            }

            //left side
            for (int x = this.tile.x - 1; x > Board.WIDTH_MIN - 1; x--)
            {
                tile = new Tile(x, this.tile.y);
                // If the square is empty
                //if (!board.isOccupied(tile))
                //    ProtectedTiles.Add(tile);
                //check if enemy piece
                ProtectedTiles.Add(tile);
                if (board.getPiece(tile) != null)
                {
                    break;
                }
            }

            return this.ProtectedTiles.ToArray();
        }

        public override Tile[] validMoves(Board board)
        {
            PossibleTiles.Clear();
            Tile tile;

            //top side
            for (int y = this.tile.y - 1; y >= Board.HEIGHT_MIN; y--) {
                tile = new Tile(this.tile.x, y);
                // If the square is empty
                if (!board.isOccupied(tile))
                    PossibleTiles.Add(tile);
                //check if enemy piece
                else if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                {
                    PossibleTiles.Add(tile);
                    break;
                }
                else
                    break;

            
            }

            //right side
            for (int x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, this.tile.y);
                // If the square is empty
                if (!board.isOccupied(tile))
                    PossibleTiles.Add(tile);
                //check if enemy piece
                else if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                {
                    PossibleTiles.Add(tile);
                    break;
                }
                else
                    break;
            }
            //bottom side
            for (int y = this.tile.y + 1; y < Board.HEIGHT_MAX; y++)
            {
                tile = new Tile(this.tile.x, y);
                // If the square is empty
                if (!board.isOccupied(tile))
                    PossibleTiles.Add(tile);
                //check if enemy piece
                else if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                {
                    PossibleTiles.Add(tile);
                    break;
                }
                else
                    break;
            }

            //left side
            for (int x = this.tile.x - 1; x > Board.WIDTH_MIN - 1; x--)
            {
                tile = new Tile(x, this.tile.y);
                // If the square is empty
                if (!board.isOccupied(tile))
                    PossibleTiles.Add(tile);
                //check if enemy piece
                else if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                {
                    PossibleTiles.Add(tile);
                    break;
                }
                else
                    break;
            }

            if (Game.checkStatus != CheckStatus.None)
            {
                if (board.canBlock(this.PossibleTiles))
                {
                    if (this.chessPiece.pieceColour == PieceColour.Black && Game.checkStatus == CheckStatus.BlackCheck)
                        validTilesDuringCheck(board.tilesToBlock);
                    else if (this.chessPiece.pieceColour == PieceColour.White && Game.checkStatus == CheckStatus.WhiteCheck)
                        validTilesDuringCheck(board.tilesToBlock);
                }
                else
                    PossibleTiles.Clear();

            }

            return this.PossibleTiles.ToArray();
        }
   }
}
