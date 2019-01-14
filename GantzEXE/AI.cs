using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GantzEXE
{
    class AI
    {

        public static int MovesToGameEnd(Board board)
        {
            int moves = 0;
            //friends to check
            Point[] friends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in board.freeCells) {
                    if (board.grid[p.X, p.Y] == false && board.freeBlockedCells.Contains(p))
                    {
                        bool freeBlocked = true;
                        foreach(Point friend in friends)
                            if (board.grid[board.clampIndex(p.X + friend.X), board.clampIndex(p.Y + friend.Y)] == false)
                            {
                                freeBlocked = false;
                                break;
                            }
                    if (freeBlocked)
                    {
                        board.freeBlockedCells.Add(p);
                    }
                }

            }
            moves = (board.freeCells.Count - board.freeBlockedCells.Count);
            return moves;
        }
        public static int NumberOfOddStructures(Board board)
        {
            int oddStructures = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in board.freeUnblockedCells)
            {
                if (visited.Contains(p))
                    continue;
                int friendsCount = 0;
                int friendsOfFriendsBlocked = 0;
                List<Point> freeFriends = new List<Point>();
                foreach (Point f in friends)
                {

                    if(board.grid[board.clampIndex(p.X + f.X),board.clampIndex(p.Y + f.Y)] == false)
                    {
                        freeFriends.Add(new Point(board.clampIndex(p.X + f.X), board.clampIndex(p.Y + f.Y)));
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
                            Point temp = new Point(board.clampIndex(freeF.X + f.X), board.clampIndex(freeF.Y + f.Y));
                            if(!(temp == p) && board.grid[temp.X, temp.Y] == true)
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
        public static Move GenerateMove(Board board, bool oddBlockedCells)
        {
            
            bool isOdd;
            Move move = new Move(null, null);
            Point[] nb = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach(Point p in board.freeUnblockedCells)
            {
                for(int i = 0; i < 3; i++)
                {
                    Point p2 = new Point(board.clampIndex(p.X + nb[i].X), board.clampIndex(p.Y + nb[i].Y));
                    if(board.grid[p2.X, p2.Y] == false)
                    {
                        move = new Move(p, p2);
                        isOdd = CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(board, move);
                        if (oddBlockedCells == isOdd)
                            return move;
                    }
                }

            }
            return move;
        }
        public static bool CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd(Board board, Move move)
        {
            int blockedFreeCells = 0;
            Point[] closeFriends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            //for point 1
            //Check close friends
            foreach(Point p in closeFriends)
            {
                Point checkedFriend = new Point(board.clampIndex(move.p1.X + p.X), board.clampIndex(move.p1.Y + p.Y));
                if(!checkedFriend.Equals(move.p2) && board.grid[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach(Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(board.clampIndex(checkedFriend.X + pp.X), board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p1)) continue;
                        if(board.grid[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
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
                Point checkedFriend = new Point(board.clampIndex(move.p2.X + p.X), board.clampIndex(move.p2.Y + p.Y));
                if (!checkedFriend.Equals(move.p1) && board.grid[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach (Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(board.clampIndex(checkedFriend.X + pp.X), board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p2)) continue;
                        if (board.grid[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }


            if (blockedFreeCells >= 2) return true;
            if (blockedFreeCells == 1)
            {
                board.grid[move.p1.X, move.p1.Y] = true;
                board.grid[move.p2.X, move.p2.Y] = true;
                board.freeCells.RemoveAll(p => p.Equals(move.p1));
                board.freeCells.RemoveAll(p => p.Equals(move.p2));
                int moves = CalculateMaxMoves2(board);
                board.grid[move.p1.X, move.p1.Y] = false;
                board.grid[move.p2.X, move.p2.Y] = false;
                board.freeCells.Add(move.p1);
                board.freeCells.Add(move.p2);
                if (moves % 2 == 0)
                    return true;
                else
                    return false;
            }
            else return false;
        }
        public static int CalculateMaxMoves(Board board)
        {
            int moves = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };
            foreach (Point free in board.freeCells)
            {
                if (visited.Contains(free))
                    continue;
                for(int i = 0; i < 4; i++)
                {
                    Point temp = new Point(board.clampIndex(free.X + friends[i].X), board.clampIndex(free.Y + friends[i].Y));
                    if(!visited.Contains(temp) && board.grid[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
            }
            
            return moves;
        }
        public static int CalculateMaxMoves2(Board board)
        {
            int moves = 0;
            List<Point> visited = new List<Point>();
            Point[] friends = { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };
            foreach (Point free in board.freeCells)
            {
                if (visited.Contains(free))
                    continue;
                for (int i = 0; i < 4; i++)
                {
                    //border case, to be handled later
                    if (free.X + friends[i].X < 0 || free.X + friends[i].X >= board.size || free.Y + friends[i].Y < 0 || free.Y + friends[i].Y >= board.size)
                        continue;
                    Point temp = new Point(board.clampIndex(free.X + friends[i].X), board.clampIndex(free.Y + friends[i].Y));
                    if (!visited.Contains(temp) && board.grid[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (!(free.X + friends[i].X < 0 || free.X + friends[i].X >= board.size || free.Y + friends[i].Y < 0 || free.Y + friends[i].Y >= board.size))
                        continue;
                    Point temp = new Point(board.clampIndex(free.X + friends[i].X), board.clampIndex(free.Y + friends[i].Y));
                    if (!visited.Contains(temp) && board.grid[temp.X, temp.Y] == false)
                    {
                        moves++;
                        visited.Add(free); visited.Add(temp);
                        break;
                    }
                }
            }

            return moves;
        }
        public static void UpdateFreeBlockedCells(Board board) {
            Point[] friends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in board.freeCells)
            {
                if (board.grid[p.X, p.Y] == false && board.freeBlockedCells.Contains(p))
                {
                    bool freeBlocked = true;
                    foreach (Point friend in friends)
                        if (board.grid[board.clampIndex(p.X + friend.X), board.clampIndex(p.Y + friend.Y)] == false)
                        {
                            freeBlocked = false;
                            break;
                        }
                    if (freeBlocked)
                    {
                        board.freeBlockedCells.Add(p);
                    }
                }

            }
        }


        public static Move GenerateMove2(Board board, bool oddBlockedCells)
        {
            List<Move> checkedMoves = new List<Move>();
            List<int> blockedCellsByMoves = new List<int>();
            int blockedCells;
            Move move = new Move(null, null);
            Point[] nb = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in board.freeUnblockedCells)
            {
                for (int i = 0; i < 3; i++)
                {
                    Point p2 = new Point(board.clampIndex(p.X + nb[i].X), board.clampIndex(p.Y + nb[i].Y));
                    if (board.grid[p2.X, p2.Y] == false)
                    {
                        move = new Move(p, p2);
                        blockedCells = CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd2(board, move);
                        checkedMoves.Add(move);
                        blockedCellsByMoves.Add(blockedCells);
                    }
                }

            }
            if (oddBlockedCells)
            {
                if (blockedCellsByMoves.IndexOf(0) >= 0)
                    return checkedMoves[blockedCellsByMoves.IndexOf(0)];
                else
                    if (blockedCellsByMoves.IndexOf(1) >= 0)
                    return checkedMoves[blockedCellsByMoves.IndexOf(1)];
            }
            else
            {
                if (blockedCellsByMoves.IndexOf(2) >= 0)
                    return checkedMoves[blockedCellsByMoves.IndexOf(2)];
                if (blockedCellsByMoves.IndexOf(3) >= 0)
                    return checkedMoves[blockedCellsByMoves.IndexOf(3)];
                if (blockedCellsByMoves.IndexOf(4) >= 0)
                    return checkedMoves[blockedCellsByMoves.IndexOf(4)];
            }
            return move;
        }
        public static int CheckIfNumberOfBlockedFreeCellsCreatedByMoveIsOdd2(Board board, Move move)
        {
            int blockedFreeCells = 0;
            Point[] closeFriends = { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            foreach (Point p in closeFriends)
            {
                Point checkedFriend = new Point(board.clampIndex(move.p1.X + p.X), board.clampIndex(move.p1.Y + p.Y));
                if (!checkedFriend.Equals(move.p2) && board.grid[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach (Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(board.clampIndex(checkedFriend.X + pp.X), board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p1)) continue;
                        if (board.grid[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }
            foreach (Point p in closeFriends)
            {
                Point checkedFriend = new Point(board.clampIndex(move.p2.X + p.X), board.clampIndex(move.p2.Y + p.Y));
                if (!checkedFriend.Equals(move.p1) && board.grid[checkedFriend.X, checkedFriend.Y] == false)
                {
                    //check close friends of that close friend
                    bool blockedCell = true;
                    foreach (Point pp in closeFriends)
                    {
                        Point checkedFriendOfFriend = new Point(board.clampIndex(checkedFriend.X + pp.X), board.clampIndex(checkedFriend.Y + pp.Y));
                        if (checkedFriendOfFriend.Equals(move.p2)) continue;
                        if (board.grid[checkedFriendOfFriend.X, checkedFriendOfFriend.Y] == false)
                        {
                            blockedCell = false;
                            break;
                        }

                    }
                    if (blockedCell) blockedFreeCells++;
                }
            }


            return blockedFreeCells;
        }
    }

}
