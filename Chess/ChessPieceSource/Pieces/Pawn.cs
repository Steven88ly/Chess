using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Pawn : Piece
    {
        public Pawn(bool isBlack, int x, int y, ChessPiece chessPiece) : base(isBlack, x, y, chessPiece) {}
        public override string imgSource()
        {
            return isBlack ? "/ChessPieceImage/BlackPawn.png" : "/ChessPieceImage/WhitePawn.png";
        }

        public override Tile[] protectedTiles(Board board)
        {
            ProtectedTiles.Clear();
            if (chessPiece.pieceColour == PieceColour.Black)
            {
                //check if pawn isnt in the last row
                if (this.tile.y != Board.HEIGHT_MAX - 1)
                {
                    if (this.tile.x != Board.WIDTH_MIN)
                    {
                        // On the left attack square is a piece?
                        ProtectedTiles.Add(new Tile(this.tile.x - 1, this.tile.y + 1));
                    }

                    // Checking whether the pawn isn't in the right column
                    if (this.tile.x != Board.WIDTH_MAX - 1)
                    {
                        // On the right attack square is a piece?
                        ProtectedTiles.Add(new Tile(this.tile.x + 1, this.tile.y + 1));
                    }
                }
            }
            else
            {
                if (this.tile.y != Board.HEIGHT_MIN)
                {
                    if (this.tile.x != Board.WIDTH_MIN)
                    {
                        // On the left attack square is a piece?
                        ProtectedTiles.Add(new Tile(this.tile.x - 1, this.tile.y - 1));
                    }

                    // Checking whether the pawn isn't in the right column
                    if (this.tile.x != Board.WIDTH_MAX - 1)
                    {
                        // On the right attack square is a piece?
                        ProtectedTiles.Add(new Tile(this.tile.x + 1, this.tile.y - 1));
                    }
                }
            }

            return ProtectedTiles.ToArray();
        }

        public override Tile[] validMoves(Board board)
        {
            Tile tile;

            PossibleTiles.Clear();
           

            //piece is black and moves downwards
            if (chessPiece.pieceColour == PieceColour.Black)
            {
                //check if pawn isnt in the last row
                if (this.tile.y != Board.HEIGHT_MAX - 1)
                {
                    tile = new Tile(this.tile.x, this.tile.y + 1);
                    //1st square ahead of pawn is free
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);

                        //if pawns first move
                        if (!this.hasMoved)
                        {
                            tile = new Tile(this.tile.x, this.tile.y + 2);
                            //2nd square ahead of pawn is free
                            if (!board.isOccupied(tile))
                                PossibleTiles.Add(tile);
                        }
                    }


                    //check if pawn isnt in the left column
                    if (this.tile.x != Board.WIDTH_MIN)
                    {
                        tile = new Tile(this.tile.x - 1, this.tile.y + 1);
                        // On the left attack square is a piece?
                        if (board.isOccupied(tile))
                        {
                            // Piece is enemy
                            if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                            {
                                PossibleTiles.Add(tile);
                            }
                        }
                    }

                    // Checking whether the pawn isn't in the right column
                    if (this.tile.x != Board.WIDTH_MAX - 1)
                    {
                        tile = new Tile(this.tile.x + 1, this.tile.y + 1);
                        // On the right attack square is a piece?
                        if (board.isOccupied(tile))
                        {
                            // Piece is enemy
                            if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                            {
                                PossibleTiles.Add(tile);
                            }
                        }
                    }
                }

              
            }
            else { //the pawn is white and moves upwards
                //check if pawn isnt in the last row
                if (this.tile.y != Board.HEIGHT_MIN)
                {
                    tile = new Tile(this.tile.x, this.tile.y - 1);
                    //1st square ahead of pawn is free
                    if (!board.isOccupied(tile))
                    {
                        PossibleTiles.Add(tile);

                        //if pawns first move
                        if (!this.hasMoved)
                        {
                            tile = new Tile(this.tile.x, this.tile.y - 2);
                            //2nd square ahead of pawn is free
                            if (!board.isOccupied(tile))
                                PossibleTiles.Add(tile);
                        }
                    }


                    //check if pawn isnt in the left column
                    if (this.tile.x != Board.WIDTH_MIN)
                    {
                        tile = new Tile(this.tile.x - 1, this.tile.y - 1);
                        // On the left attack square is a piece?
                        if (board.isOccupied(tile))
                        {
                            // Piece is enemy
                            if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                            {
                                PossibleTiles.Add(tile);
                            }
                        }
                    }

                    // Checking whether the pawn isn't in the right column
                    if (this.tile.x != Board.WIDTH_MAX - 1)
                    {
                        tile = new Tile(this.tile.x + 1, this.tile.y - 1);
                        // On the right attack square is a piece?
                        if (board.isOccupied(tile))
                        {
                            // Piece is enemy
                            if (board.getPiece(tile).chessPiece.pieceColour != this.chessPiece.pieceColour)
                            {
                                PossibleTiles.Add(tile);
                            }
                        }
                    }
                }

               
            }

            //check if the king is in check or possible check after move of this piece
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
