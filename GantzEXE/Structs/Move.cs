﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GantzEXE
{
    struct Move
    {
        public Point p1;
        public Point p2;
        public Move(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        public override string ToString()
        {
            return "{" + p1.X + ";" + p1.Y + "}," + "{" + p2.X + ";" + p2.Y + "}";
        }
    }
}
