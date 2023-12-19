using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Knight : Piece
    {
        public Knight(bool isBlack, int x, int y, ChessPiece chessPiece) : base(isBlack, x, y, chessPiece) { }
        public override string imgSource()
        {
            return isBlack ? "/ChessPieceImage/BlackKnight.png" : "/ChessPieceImage/WhiteKnight.png";
        }

        public override Tile[] protectedTiles(Board board)
        {
            ProtectedTiles.Clear();

            foreach (Tile tile in updatePossibleTiles())
            {
                if (tile.x >= Board.WIDTH_MIN &&
                   tile.x < Board.WIDTH_MAX &&
                   tile.y >= Board.HEIGHT_MIN &&
                   tile.y < Board.HEIGHT_MAX)
                    ProtectedTiles.Add(tile);
            }
            return ProtectedTiles.ToArray();
        }

        public override Tile[] validMoves(Board board)
        {
            Piece[,] pieceSet = board.getPieceSet();
            PossibleTiles = updatePossibleTiles();
            
            //filter out of bounds and tiles occupied by friendly pieces
           filterStandardValidTiles(PossibleTiles, pieceSet);

            //filter out moves that cant block for the king in check
            if (Game.checkStatus != CheckStatus.None) {
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

    

        private List<Tile> updatePossibleTiles() {
            PossibleTiles.Clear();
            PossibleTiles.Add(new Tile(tile.x - 1, tile.y - 2));
            PossibleTiles.Add(new Tile(tile.x + 1, tile.y - 2));
            PossibleTiles.Add(new Tile(tile.x + 2, tile.y - 1));
            PossibleTiles.Add(new Tile(tile.x + 2, tile.y + 1));
            PossibleTiles.Add(new Tile(tile.x - 1, tile.y + 2));
            PossibleTiles.Add(new Tile(tile.x + 1, tile.y + 2));
            PossibleTiles.Add(new Tile(tile.x - 2, tile.y - 1));
            PossibleTiles.Add(new Tile(tile.x - 2, tile.y + 1));
            return PossibleTiles;
        }
    }
}
