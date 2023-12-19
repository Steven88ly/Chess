using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct ChessPiece
    {
        public PieceColour pieceColour;
        public PieceType pieceType;

        public ChessPiece(PieceColour pc, PieceType pt) {

            this.pieceColour = pc;
            this.pieceType = pt;
        }
    }
}
