using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GantzEXE
{
    class AI
    {
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
        public static Move GenerateMove(bool[,] board, int size, bool oddBlockedCells, List<Point> unblockedFreeCells)
        {
            //decide odd or even moves to win
            bool isOdd;
            Move move = new Move(null, null);
            Point[] nb = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach(Point p in unblockedFreeCells)
            {
                for(int i = 0; i < 3; i++)
                {
                    Point p2 = new Point(Board.clampIndex(p.X + nb[i].X), Board.clampIndex(p.Y + nb[i].Y));
                    if(board[p2.X, p2.Y] == false)
                    {
                        move = new Move(p, p2);
                        isOdd = CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(board, move);
                        if (oddBlockedCells == isOdd)
                            return move;
                    }
                }
                //find free neighbours
                //for found neighbours analyze movment impact

            }


            return move;
        }
        public static bool CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(bool[,] blockOfBoard, Move move)
        {
            int blockedFreeCells = 0;
            Point[] closeFriends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            //main logic
            //for point 1
            //Check close friends
            foreach(Point p in closeFriends)
            {
                Point checkedFriend = new Point(Board.clampIndex(move.p1.X + p.X), Board.clampIndex(move.p1.Y + p.Y));
                if(checkedFriend != move.p2)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach(Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(Board.clampIndex(checkedFriend.X + pp.X), Board.clampIndex(checkedFriend.Y + p.Y));
                        if (checkedFriendOfFriend == move.p1) continue;
                        if(blockOfBoard[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }
            //p2
            foreach (Point p in closeFriends)
            {
                Point checkedFriend = new Point(Board.clampIndex(move.p2.X + p.X), Board.clampIndex(move.p2.Y + p.Y));
                if (checkedFriend != move.p1)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach (Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(Board.clampIndex(checkedFriend.X + pp.X), Board.clampIndex(checkedFriend.Y + p.Y));
                        if (checkedFriendOfFriend == move.p2) continue;
                        if (blockOfBoard[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }

            return (blockedFreeCells % 2 == 1 ? true : false);
        }
    }
}
