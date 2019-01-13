using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GantzEXE
{
    class AI
    {

        public static int MovesToGameEnd(bool[,] board, int size,List<Point> freeCells, ref List<Point> freeBlockedCells)
        {
            int moves = 0;
            //friends to check
            Point[] friends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in freeCells) {
                    if (board[p.X, p.Y] == false && !freeBlockedCells.Contains(p))
                    {
                        bool freeBlocked = true;
                        foreach(Point friend in friends)
                            if (board[Board.clampIndex(p.X + friend.X), Board.clampIndex(p.Y + friend.Y)] == false)
                            {
                                freeBlocked = false;
                                break;
                            }
                    if (freeBlocked)
                    {
                        freeBlockedCells.Add(p);
                    }
                }

            }
            moves = (freeCells.Count - freeBlockedCells.Count);
            //debug
            //Console.WriteLine("blocked cells: " + freeBlockedCellsCount);
            return moves;
        }
        public static int NumberOfOddStructures(bool[,] board, int size, List<Point> unBlockedCells)
        {
            int oddStructures = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in unBlockedCells)
            {
                if (visited.Contains(p))
                    continue;
                int friendsCount = 0;
                int friendsOfFriendsBlocked = 0;
                List<Point> freeFriends = new List<Point>();
                foreach (Point f in friends)
                {

                    if(board[Board.clampIndex(p.X + f.X),Board.clampIndex(p.Y + f.Y)] == false)
                    {
                        freeFriends.Add(new Point(Board.clampIndex(p.X + f.X), Board.clampIndex(p.Y + f.Y)));
                        friendsCount++;
                    }
                }
                if(friendsCount == 2)
                {
                    foreach(Point freeF in freeFriends)
                    {
                        int blockedFriends = 0;
                        foreach(Point f in friends)
                        {
                            Point temp = new Point(Board.clampIndex(freeF.X + f.X), Board.clampIndex(freeF.Y + f.Y));
                            if(!(temp == p) && board[temp.X, temp.Y] == true)
                            {
                                blockedFriends++;
                            }

                        }
                        if (blockedFriends == 3)
                            friendsOfFriendsBlocked++;
                    }
                }
                if (friendsOfFriendsBlocked == 2)
                    oddStructures++;

            }
            return oddStructures;
        }
        public static Move GenerateMove(bool[,] board, int size, bool oddBlockedCells, List<Point> unblockedFreeCells)
        {
            
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
                        isOdd = CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(board, size, move, unblockedFreeCells);
                        if (oddBlockedCells == isOdd)
                            return move;
                    }
                }

            }

            return move;
        }
        public static bool CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(bool[,] board, int size, Move move, List<Point> freeCells)
        {
            int blockedFreeCells = 0;
            Point[] closeFriends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            //for point 1
            //Check close friends
            foreach(Point p in closeFriends)
            {
                Point checkedFriend = new Point(Board.clampIndex(move.p1.X + p.X), Board.clampIndex(move.p1.Y + p.Y));
                if(!checkedFriend.Equals(move.p2) && board[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach(Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(Board.clampIndex(checkedFriend.X + pp.X), Board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p1)) continue;
                        if(board[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
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
                if (!checkedFriend.Equals(move.p1) && board[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach (Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(Board.clampIndex(checkedFriend.X + pp.X), Board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p2)) continue;
                        if (board[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }


            if (blockedFreeCells >= 2) return true;
            //if (blockedFreeCells == 1)
            //{
            //    board[move.p1.X, move.p1.Y] = true;
            //    board[move.p2.X, move.p2.Y] = true;
            //    freeCells.RemoveAll(p => p.Equals(move.p1));
            //    freeCells.RemoveAll(p => p.Equals(move.p2));
            //    int moves = CalculateMaxMoves(board, size, freeCells);
            //    board[move.p1.X, move.p1.Y] = false;
            //    board[move.p2.X, move.p2.Y] = false;
            //    freeCells.Add(move.p1);
            //    freeCells.Add(move.p2);
            //    if (moves % 2 == 0)
            //        return true;
            //    else
            //        return false;
            //}
            else return false;
        }
        public static int CalculateMaxMoves(bool[,] board, int size, List<Point> freeCells)
        {
            int moves = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };
            foreach (Point free in freeCells)
            {
                if (visited.Contains(free))
                    continue;
                for(int i = 0; i < 4; i++)
                {
                    Point temp = new Point(Board.clampIndex(free.X + friends[i].X), Board.clampIndex(free.Y + friends[i].Y));
                    if(!visited.Contains(temp) && board[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
            }
            
            return moves;
        }
        public static int CalculateMaxMoves2(bool[,] board, int size, List<Point> freeCells)
        {
            int moves = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };
            foreach (Point free in freeCells)
            {
                if (visited.Contains(free))
                    continue;
                for (int i = 0; i < 4; i++)
                {
                    //border case, to be handled later
                    if (free.X + friends[i].X < 0 || free.X + friends[i].X >= size || free.Y + friends[i].Y < 0 || free.Y + friends[i].Y >= size)
                        continue;
                    Point temp = new Point(Board.clampIndex(free.X + friends[i].X), Board.clampIndex(free.Y + friends[i].Y));
                    if (!visited.Contains(temp) && board[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (!(free.X + friends[i].X < 0 || free.X + friends[i].X >= size || free.Y + friends[i].Y < 0 || free.Y + friends[i].Y >= size))
                        continue;
                    Point temp = new Point(Board.clampIndex(free.X + friends[i].X), Board.clampIndex(free.Y + friends[i].Y));
                    if (!visited.Contains(temp) && board[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
            }

            return moves;
        }
    }
}
