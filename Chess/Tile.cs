using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Tile
    {

        private int _x;
        public int x { get { return _x; }
            set {
                if (x >= Board.WIDTH_MIN && x <= Board.WIDTH_MAX)
                    _x = value;
                else
                    throw new ArgumentException();
            }
        }

        private int _y;

        public int y
        {
            get { return _y; }
            set
            {
                if (y >= Board.HEIGHT_MIN && y <= Board.HEIGHT_MAX)
                    _y = value;
                else
                    throw new ArgumentException();
            }
        }

        private int _isProtectedByWhite;

        public int isProtectedByWhite { get { return _isProtectedByWhite; } set { _isProtectedByWhite = value; } }

        private int _isProtectedByBlack;

        public int isProtectedByBlack { get { return _isProtectedByBlack; } set { _isProtectedByBlack = value; } }

        public Tile(int x, int y) {
            this.x = x;
            this.y = y;

            isProtectedByBlack = 0;
            isProtectedByWhite = 0;
        }

        public Tile(string x, string y) {
            this.x = int.Parse(x);
            this.y = int.Parse(y);

            isProtectedByBlack = 0;
            isProtectedByWhite = 0;
        }
    }
}
