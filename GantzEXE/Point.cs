using System;
using System.Collections.Generic;
using System.Text;

namespace GantZ
{
    class Point
    {
        int x;
        int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public override string ToString()
        {
            return "{" + X + ";" + Y + "}";
        }
        public override bool Equals(object obj)
        {
            Point p = (Point)obj;
            if (this.x == p.X && this.y == p.Y)
                return true;
            return false;
        }
    }
}
