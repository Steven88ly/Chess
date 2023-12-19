using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct Move
    {
        private Tile From;
        private Tile To;

        public Move(Tile from, Tile to) {
            this.From = from;
            this.To = to;
        }

        public Tile getFrom() { return this.From; }
        public Tile getTo() { return this.To; }
    }
}
