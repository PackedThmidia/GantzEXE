using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GantzEXE
{
    class AI
    {
        bool[,] pattern_vertical =
        {

        };
        bool[,] pattern_horizontal =
        {

        };
        public static int MovesToGameEnd(bool[,] board, int size, int freeCells, ref List<Point> freeBlockedCells)
        {
            int freeBlockedCellsCount = freeBlockedCells.Count;
            int moves = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++) {
                    //corner cases to be hadled later
                    if ((i == 0 && j == 0) || (i == 0 && j == size - 1) || (i == size - 1 && j == 0) || (i == size - 1 && j == size - 1))
                        continue;
                    if (board[i, j] == false && !freeBlockedCells.Exists(p => p.X == i && p.Y == j))
                    {
                        // border continuity xD
                        int k = i - 1 < 0 ? size - 1 : i - 1;
                        int l = j - 1 < 0 ? size - 1 : j - 1;
                        int m = i + 1 == size ? 0 : i + 1; 
                        int n = j + 1 == size ? 0 : j + 1; 

                        if (board[i, n] == true && board[i, l] == true
                            && board[m, j] == true && board[k, j] == true)
                        {
                            freeBlockedCellsCount++;
                            freeBlockedCells.Add(new Point(i, j));
                        }
                    }
                }
            moves = (freeCells - freeBlockedCellsCount) / 2;

            return moves;
        }
        public Move generateMove(Board[,] board, int size, bool createFreeBlockedCell, List<Point> unblockedFreeCells)
        {
            Move move;
            foreach(Point p in unblockedFreeCells)
            {
                //find free neighbours
                //for found neighbours analyze movment impact

            }


            return move;
        }
    }
}
