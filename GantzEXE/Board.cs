using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace GantzEXE
{
    class Board
    {
        public bool[,] grid;
        public int size;

        public List<Point> occupiedCells = new List<Point>();
        public List<Point> freeCells = new List<Point>();
        public List<Point> freeBlockedCells = new List<Point>();
        public List<Point> freeUnblockedCells = new List<Point>();

        Random rnd = new Random();

        public Board(int _size)
        {
            size = _size;
            InitBoard(size);
        }
        public void InitBoard(int size)
        {
            grid = new bool[size,size];
        }
        public void InitBoardObstacles(List<Point> obstacles)
        {
            OccupyCells(obstacles);
            AddFreeCellsToList();
            //calculate FreeBlocked and FreeUnblocked
        }
        public void OccupyCells(List<Point> points)
        {
            foreach (Point p in points)
            {
                occupiedCells.Add(p);
                freeCells.Remove(p);
                grid[p.X, p.Y] = true;
            }
        }
        public void OccupyCells(Move move)
        {
            occupiedCells.Add(move.p1);
            occupiedCells.Add(move.p2);
            freeCells.Remove(move.p1);
            freeCells.Remove(move.p2);
            grid[move.p1.X, move.p1.Y] = true;
            grid[move.p2.X, move.p2.Y] = true;
            
        }
        public bool isOccupied(Point p)
        {
            return grid[p.X, p.Y];
        }
        public int clampIndex(int index)
        {
            if (index < 0)
                return size + index;
            else if (index >= size)
                return index % size;
            return index;
        }

        public Move nextMove()
        {
            Move move;
            UpdateUnblockedFreeCellsList();
            // int unblocked = AI.MovesToGameEnd(board, size, freeCells, ref freeUnblockedCells);
            int total = AI.CalculateMaxMoves2(this);
            bool isOdd = total % 2 == 0 ? false : true;
            move = AI.GenerateMove2(this, isOdd);
            OccupyCells(move);

            return move;
        }

        public void UpdateUnblockedFreeCellsList()
        {
            AI.UpdateFreeBlockedCells(this);
            freeUnblockedCells = freeCells.Except(freeBlockedCells).ToList();
        }
        void AddFreeCellsToList()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (grid[i, j] == false)
                        freeCells.Add(new Point(i, j));
        }







        public Move randomMove()
        {
            Move move;
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
            move = new Move(p1, p2);
            OccupyCells(move);
            return move;
        }
    }
}
