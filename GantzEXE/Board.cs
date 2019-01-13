using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace GantzEXE
{
    class Board
    {
        public bool[,] board;
        public static int size;

        List<Point> occupiedCells = new List<Point>();
        List<Point> freeCells = new List<Point>();
        List<Point> freeBlockedCells = new List<Point>();
        List<Point> freeUnblockedCells = new List<Point>();

        //temp random generator
        Random rnd = new Random();

        public Board(int _size)
        {
            size = _size;
            InitBoard(size);
        }
        public void InitBoard(int size)
        {
            board = new bool[size,size];
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
                board[p.X, p.Y] = true;
            }
        }
        public void OccupyCells(Move move)
        {
            occupiedCells.Add(move.p1);
            occupiedCells.Add(move.p2);
            freeCells.Remove(move.p1);
            freeCells.Remove(move.p2);
            board[move.p1.X, move.p1.Y] = true;
            board[move.p2.X, move.p2.Y] = true;
            
        }
        public bool isOccupied(Point p)
        {
            return board[p.X, p.Y];
        }
        public static int clampIndex(int index)
        {
            if (index < 0)
                return size + index;
            else if (index >= size)
                return index % size;
            return index;
        }
        public static Move FindMoveBorder(Move move)
        {
            //to be done
            return move;
        }
        void AddFreeCellsToList()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j] == false)
                        freeCells.Add(new Point(i, j));
        }
        public Move nextMove()
        {
           // if (freeCells.Count > 30)
            //    return randomMove();
            Move move;
            // int unblocked = AI.MovesToGameEnd(board, size, freeCells, ref freeUnblockedCells);
            int total = TestNumberOfMoves2();
            bool isOdd = total % 2 == 0 ? true : false;
            move = AI.GenerateMove(board, size, isOdd, freeCells);
            OccupyCells(move);
            return move;
        }
        public int testnumberofmoves()
        {
            int unblocked = AI.MovesToGameEnd(board, size, freeCells, ref freeUnblockedCells);
            int trios = TEST();
            int total = ((unblocked - 3 * trios) / 2) + trios;
            return total;
        }

        public int TEST()
        {
            freeUnblockedCells = freeCells.Except(freeBlockedCells).ToList();
            return AI.NumberOfOddStructures(board, size, freeUnblockedCells);
        }
        public int TestNumberOfMoves2()
        {
            return AI.CalculateMaxMoves2(board, size, freeCells);
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
