using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chess
{
    public class Game
    {
        private List<Move> _moveshistory;
        public List<Move> movesHistory { get { return _moveshistory; } set { _moveshistory = value; } }

        static public PieceColour playerTurn;

        private Board board;
        private Grid grid;
        private TextBlock textBlock;

        private Tile tileContext;

        private List<Piece> whiteSet;
        private List<Piece> blackSet;

        private int isHighlightedMode;

        static public GameStatus gameStatus = GameStatus.Neutral;

        static public CheckStatus checkStatus;

        public Game(Grid grid) { 
            this.board = new Board(grid);
            this.isHighlightedMode = -1;
            gameStatus = GameStatus.Neutral;
            this.grid = grid;
            textBlock = (TextBlock)grid.FindName("Banner");
        }

        public void resetBoard(Grid grid) {
            board.clearBoard();
            this.board = new Board(grid);
        }

        public bool initializeGame() {
            whiteSet = new List<Piece>();
            blackSet = new List<Piece>();

            playerTurn = PieceColour.White;

            //create pawns
            for (int sides = 0; sides <= (int)PieceColour.White; sides++)
            {
                for (int i = 0; i < Board.WIDTH_MAX; i++)
                {
                    if (sides == (int)PieceColour.White)
                    {
                        ChessPiece cp = new ChessPiece(PieceColour.White, PieceType.Pawn);
                        whiteSet.Add(new Pawn(false, i, 6, cp));
                    }
                    else {
                        ChessPiece cp = new ChessPiece(PieceColour.Black, PieceType.Pawn);
                        blackSet.Add(new Pawn(true, i, 1, cp));
                    }
                }
            }

            //create other pieces
            whiteSet.Add(new Rook(false, 0, 7, new ChessPiece(PieceColour.White, PieceType.Rook)));
            whiteSet.Add(new Rook(false, 7, 7, new ChessPiece(PieceColour.White, PieceType.Rook)));

            blackSet.Add(new Rook(true, 0, 0, new ChessPiece(PieceColour.Black, PieceType.Rook)));
            blackSet.Add(new Rook(true, 7, 0, new ChessPiece(PieceColour.Black, PieceType.Rook)));

            whiteSet.Add(new Knight(false, 1, 7, new ChessPiece(PieceColour.White, PieceType.Knight)));
            whiteSet.Add(new Knight(false, 6, 7, new ChessPiece(PieceColour.White, PieceType.Knight)));

            blackSet.Add(new Knight(true, 1, 0, new ChessPiece(PieceColour.Black, PieceType.Knight)));
            blackSet.Add(new Knight(true, 6, 0, new ChessPiece(PieceColour.Black, PieceType.Knight)));

            whiteSet.Add(new Bishop(false, 2, 7, new ChessPiece(PieceColour.White, PieceType.Bishop)));
            whiteSet.Add(new Bishop(false, 5, 7, new ChessPiece(PieceColour.White, PieceType.Bishop)));

            blackSet.Add(new Bishop(true, 2, 0, new ChessPiece(PieceColour.Black, PieceType.Bishop)));
            blackSet.Add(new Bishop(true, 5, 0, new ChessPiece(PieceColour.Black, PieceType.Bishop)));

            whiteSet.Add(new King(false, 4, 7, new ChessPiece(PieceColour.White, PieceType.King)));
            blackSet.Add(new King(true, 4, 0, new ChessPiece(PieceColour.Black, PieceType.King)));

            whiteSet.Add(new Queen(false, 3, 7, new ChessPiece(PieceColour.White, PieceType.Queen)));
            blackSet.Add(new Queen(true, 3, 0, new ChessPiece(PieceColour.Black, PieceType.Queen)));

            board.SetInitialBoard(whiteSet, blackSet);

            foreach (Piece piece in whiteSet) {
                //generate valid moves
                piece.validMoves(board);
                piece.protectedTiles(board);
                
            }

            foreach (Piece piece in blackSet) {
                //generate valid moves
                piece.validMoves(board);
                piece.protectedTiles(board);
                
            }

            incrementAllProtectedTiles();
            gameStatus = GameStatus.Play;
            return true;
        }

        public void processCommand(Tile tileClicked)
        {
            Piece piece = board.getPiece(tileClicked);

                
            //clicked an empty tile
            if (piece == null) {
                if(!isHighlightedTile(tileClicked)) { 
                    if(this.isHighlightedMode == 1)
                        switchMode();
                    board.clearBoardofHighlights();
                    this.tileContext = null;
                    return;
                }                                
            }

            //clicked piece that was occupied
            if (piece != null) {
                if (piece.chessPiece.pieceColour != Game.playerTurn)
                {
                    //scenarie player clicked an enemy piece that was not highlighted
                    if (this.isHighlightedMode == 1)
                    {
                        if (!isHighlightedTile(tileClicked))
                        {
                            switchMode();
                            board.clearBoardofHighlights();
                            return;
                        }

                    }
                    else
                        return;
                }
                else if (this.tileContext != null) {
                    //if they clicked the same piece again just exit highlighted mode
                    if (Board.isSameTile(tileContext, tileClicked))
                    {
                        if(this.isHighlightedMode == 1)
                            switchMode();
                        board.clearBoardofHighlights();
                        this.tileContext = null;
                        return;
                    }

                    //switch highlighted tiles to piece clicked
                    if (board.getPiece(tileClicked).chessPiece.pieceColour == Game.playerTurn && this.isHighlightedMode == 1)
                    {
                        board.clearBoardofHighlights();
                        this.tileContext = tileClicked;
                        switchMode();
                    }
                }
            }
             

            //click on highlighted tiles
            if (this.isHighlightedMode == 1)
            {
                piece = board.getPiece(tileContext);
                Move move;

                //decrement all protected tiles               
                decrementAllProtectedTiles();

                //check if capturing piece
                if (board.getPiece(tileClicked) != null)
                {
                    Piece capturedPiece = board.getPiece(tileClicked);

                    //remove piece from set
                    if (capturedPiece.chessPiece.pieceColour == PieceColour.Black)
                        blackSet.Remove(capturedPiece);
                    else
                        whiteSet.Remove(capturedPiece);
                }

                //checking if chosen castle move otherwise normal move
                if (board.getPiece(tileContext).chessPiece.pieceType == PieceType.King)
                {
                    King king = (King)board.getPiece(tileContext);
                    Tile tile;
                    //checking if tile clicked is castling move
                    if (king.canCastle(board) && Board.isSameTile(tileClicked, tile = new Tile(king.tile.x + 2, king.tile.y))) {
                        //move rook
                        Rook rook = (Rook)board.getPiece(new Tile(king.tile.x + 3, king.tile.y));                         
                        move = new Move(new Tile(rook.tile.x, rook.tile.y), new Tile(king.tile.x +1, king.tile.y));
                        board.updateBoard(move);
                    }
                    //move king 
                    move = new Move(king.tile, tileClicked);
                    board.updateBoard(move);
                }
                else { 
                    //create move ticket
                    move = new Move(tileContext,tileClicked);

                    //move piece
                    board.updateBoard(move);
                }

                

                //update all protected tiles
                generateAllProtectedTiles();

                //increment all protected tiles from new spot
                incrementAllProtectedTiles();

                //flag if piece has moved
                if(!piece.hasMoved)
                    piece.hasMoved = true;
                
                //switch turns
                playerTurn = (PieceColour) ((int)playerTurn * -1);
          
                //check if enemy king is checked
                piece.isEnemyKingInCheck(board);

                //clear highlighted tiles
                board.clearBoardofHighlights();

                //check for checkmate
                if (isCheckMate())
                {
                    textBlock.Text = ((gameStatus == GameStatus.WhiteWin ? "White" : "Black") + " Wins by Checkmate!");
                }
                else if (Game.checkStatus == CheckStatus.BlackCheck || Game.checkStatus == CheckStatus.WhiteCheck)
                {
                    Game.checkStatus = CheckStatus.None;
                    textBlock.Text = "";
                }
                
            }
            else {
                Tile[] validMoves = piece.validMoves(this.board);
                board.displayHighlightedTiles(validMoves.ToList<Tile>());
                this.tileContext = tileClicked;
            }

            switchMode();
        }

        private void switchMode() {
            this.isHighlightedMode *= -1;
        }

        //decrement value of all protected tiles
        private void decrementAllProtectedTiles() {
            foreach (Piece piece in whiteSet) {
                piece.decrementProtectedTiles(board.tileSet);
            }
            foreach (Piece piece in blackSet) {
                piece.decrementProtectedTiles(board.tileSet);
            }
        }

        //increment value of all protected values
        private void incrementAllProtectedTiles() {
            foreach (Piece piece in whiteSet)
            {
                piece.incrementProtectedTiles(board.tileSet);
            }
            foreach (Piece piece in blackSet)
            {
                piece.incrementProtectedTiles(board.tileSet);
            }
        }

        //regenerate/update all protected tiles
        private void generateAllProtectedTiles() {
            foreach (Piece piece in whiteSet)
            {
                piece.protectedTiles(board);
            }
            foreach (Piece piece in blackSet)
            {
                piece.protectedTiles(board);
            }
        }

        private bool isCheckMate() {
            Piece king = null;
            //check game is in check mode and if the king can move
            if (Game.checkStatus == CheckStatus.BlackCheck)
            {
                foreach (Piece piece in blackSet) {
                    if (piece.chessPiece.pieceType == PieceType.King)
                        king = piece;
                }
                //check if king has no moves to make
                if (king.validMoves(board).Count() == 0)
                {
                    Game.gameStatus = GameStatus.WhiteWin;
                    Game.checkStatus = CheckStatus.BlackCheckMate;
                    return true;
                }
            }
            else if (Game.checkStatus == CheckStatus.WhiteCheck) {
                foreach (Piece piece in blackSet)
                {
                    if (piece.chessPiece.pieceType == PieceType.King)
                        king = piece;
                }

                //check if king has no moves to make
                if (king.validMoves(board).Count() == 0)
                {
                    Game.gameStatus = GameStatus.BlackWin;
                    Game.checkStatus = CheckStatus.WhiteCheckMate;
                    return true;
                }
                    
            }

            return false;
        }

        private bool isHighlightedTile(Tile tileClicked)
        {
            //check if the tile clicked is a highlighted tile
            bool doesExist = false;
            board.getHighLightedTiles().ForEach(delegate(Tile tile) {
                if (tile.x == tileClicked.x && tile.y == tileClicked.y)
                    doesExist = true;
            });

            return doesExist;
        }
    }
}
