using System;
using System.Collections.Generic;
using System.Text;
namespace GantzEXE
{
    class Board
    {
        bool[,] board;
        int size;
        List<Point> occupiedCells = new List<Point>();
        List<Point> freeCells = new List<Point>();
        List<Point> freeBlockedCells = new List<Point>();
        List<Point> freeUnblockedCells = new List<Point>();
        //temp random generator
        Random rnd = new Random();

        public Board(int size)
        {
            this.size = size;
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
            AddFreeCellsToList();
        }
        public void OccupyCells(List<Point> points)
        {
            foreach (Point p in points)
            {
                occupiedCells.Add(p);
                freeCells.Remove(p);
                board[p.X, p.Y] = true;
            }
        }
        public bool isOccupied(Point p)
        {
            return board[p.X, p.Y];
        }
        void AddFreeCellsToList()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == false)
                        freeCells.Add(new Point(i, j));
        }
        public List<Point> randomMove()
        {
            List<Point> points = new List<Point>();
            //initial point
            Point p1 = null;
            Point p2 = null;
            while (p2 == null || p1 == null)
            {
                while (true)
                {
                    p1 = freeCells[rnd.Next(0, freeCells.Count)];
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (i == j || i == -j) continue;
                            p2 = new Point(p1.X + i, p1.Y + j);
                            if (p2.X < 0 || p2.X >= size || p2.Y < 0 || p2.Y >= size)
                            {
                                p2 = null;
                                continue;
                            }

                            if (!occupiedCells.Contains(p2))
                                break;
                            else p2 = null;
                        }
                        if (p2 != null)
                            break;
                    }
                    if (p2 != null)
                        break;
                }
            }

            freeCells.Remove(p1);
            freeCells.Remove(p2);
            points.Add(p1);
            points.Add(p2);
            OccupyCells(points);
            return points;
        }
    }
}
