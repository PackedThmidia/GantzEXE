using System;
using System.Collections.Generic;
using System.Text;
namespace GantZ
{
    class Board
    {
        bool[,] board;
        int size;
        List<Point> occupiedCells = new List<Point>();
        //temp random generator
        Random rnd = new Random();

        public Board(int size)
        {
            InitBoard(size);
        }
        public void InitBoard(int size)
        {
            board = new bool[size,size];
            //init with false value
        }
        public void InitBoardObstacles(List<Point> obstacles)
        {
            OccupyCells(obstacles);
        }
        public void OccupyCells(List<Point> points)
        {
            foreach (Point p in points)
            {
                occupiedCells.Add(p);
                board[p.X, p.Y] = true;
            }
        }
        public bool isOccupied(Point p)
        {
            return board[p.X, p.Y];
        }
        public List<Point> randomMove()
        {
            List<Point> points = new List<Point>();
            //initial point
            Point p1 = null;
            Point p2 = null;
            while (p2 == null || p1 == null)
            {
                while (!isOccupied(p1 = new Point(rnd.Next(0, size), rnd.Next(0, size)))) ;
                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                    {
                        if (!isOccupied(p2 = new Point(p1.X + i, p1.Y + j)))
                            break;
                    }
            }
            points.Add(p1);
            points.Add(p2);

            return points;
        }
    }
}
