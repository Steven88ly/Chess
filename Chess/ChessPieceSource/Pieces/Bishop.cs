using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Bishop : Piece
    {
        public Bishop(bool isBlack, int x, int y, ChessPiece chessPiece) : base(isBlack,x,y,chessPiece) {}

        public override string imgSource()
        {

            return isBlack ? "/ChessPieceImage/BlackBishop.png" : "/ChessPieceImage/WhiteBishop.png";
        }

        public override Tile[] protectedTiles(Board board)
        {
            ProtectedTiles.Clear();
            int x;
            int y = this.tile.y;
            Tile tile;
            // Left top side
            for (x = this.tile.x - 1; x >= Board.WIDTH_MIN; x--)
            {
                tile = new Tile(x, --y);
                if (y >= Board.HEIGHT_MIN)
                {
                    // If the square is empty
                    ProtectedTiles.Add(tile);
                    if (board.getPiece(tile) != null)
                    {
                        break;
                    }

                }
                else
                    break;
            }
            // Right top side
            y = this.tile.y;
            for (x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, --y);
                if (y >= Board.HEIGHT_MIN)
                {
                    // If the square is empty
                    ProtectedTiles.Add(tile);
                    if (board.getPiece(tile) != null)
                    {
                        break;
                    }
                }
                else
                    break;
            }
            // Left bottom side
            y = this.tile.y;
            for (x = this.tile.x - 1; x >= Board.WIDTH_MIN; x--)
            {
                tile = new Tile(x, ++y);
                if (y < Board.HEIGHT_MAX)
                {
                    // If the square is empty
                    ProtectedTiles.Add(tile);
                    if (board.getPiece(tile) != null)
                    {
                        break;
                    }
                }
                else
                    break;
            }
            // Right bottom side
            y = this.tile.y;
            for (x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, ++y);
                if (y < Board.HEIGHT_MAX)
                {
                    // If the square is empty
                    ProtectedTiles.Add(tile);
                    if (board.getPiece(tile) != null)
                    {
                        break;
                    }
                
                }
                else
                    break;
            }

            return PossibleTiles.ToArray();
        }

        public override Tile[] validMoves(Board board)
        {
            PossibleTiles.Clear();
            int x;
            int y = this.tile.y;
            Tile tile;
            // Left top side
            for (x = this.tile.x -1; x >= Board.WIDTH_MIN; x--)
            {
                tile = new Tile(x, --y);
                if (y >= Board.HEIGHT_MIN)
                {
                    // If the square is empty
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);
                    }
                    else // Square has a piece
                    {
                        if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour) // Piece is enemy
                        {
                            PossibleTiles.Add(tile);
                        }
                        break;
                    }
                }
                else
                    break;
            }
            // Right top side
            y = this.tile.y;
            for (x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, --y);
                if (y >= Board.HEIGHT_MIN)
                {
                    // If the square is empty
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);
                    }
                    else // Square has a piece
                    {
                        if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour) // Piece is enemy
                        {
                            PossibleTiles.Add(tile);
                        }
                        break;
                    }
                }
                else
                    break;
            }
            // Left bottom side
            y = this.tile.y;
            for (x = this.tile.x - 1; x >= Board.WIDTH_MIN; x--)
            {
                tile = new Tile(x, ++y);
                if (y < Board.HEIGHT_MAX)
                {
                    // If the square is empty
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);
                    }
                    else // Square has a piece
                    {
                        if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour) // Piece is enemy
                        {
                            PossibleTiles.Add(tile);
                        }
                        break;
                    }
                }
                else
                    break;
            }
            // Right bottom side
            y = this.tile.y;
            for (x = this.tile.x + 1; x < Board.WIDTH_MAX; x++)
            {
                tile = new Tile(x, ++y);
                if (y < Board.HEIGHT_MAX)
                {
                    // If the square is empty
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);
                    }
                    else // Square has a piece
                    {
                        if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour) // Piece is enemy
                        {
                            PossibleTiles.Add(tile);
                        }
                        break;
                    }
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

            return PossibleTiles.ToArray();
        }
    }
}
