using System;
using System.Collections.Generic;
using System.Text;

namespace GantzEXE
{
    class Point
    {
        private int x;
        private int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Point(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }
        public override string ToString()
        {
            return "{" + X + ";" + Y + "}";
        }
        public override bool Equals(object obj)
        {
            Point p = (Point)obj;
            if (this.X == p.X && this.Y == p.Y)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
