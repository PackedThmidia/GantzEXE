using System;
using System.IO;
namespace GantzEXE
{
    class Program
    {
        static void Main(string[] args)
        {
            bool startedFirst = false;
            CommMaper mapper = new CommMaper();

            //listen for size
            string size = Console.In.ReadLine();
            //create board
            Board board = new Board(int.Parse(size));
            mapper.SendOK();

            //listen fo obstacles
            string obstacles = Console.In.ReadLine();
            board.InitBoardObstacles(mapper.CreatePointListFromMessage(obstacles));
            mapper.SendOK();




            //TESTING
            //foreach (Point p in mapper.CreatePointListFromMessage("{0;1},{2;2},{2;3}"))
            //{
            //    Console.WriteLine(p);
            //}
            //Console.WriteLine(mapper.CreateMessageFromPoints(mapper.CreatePointListFromMessage("{0;1},{2;2},{2;3}")));


            //main loop
            while (true)
            {
                string opponentMove = Console.In.ReadLine();
                if (opponentMove == "start") {
                    startedFirst = true;
                    //board.OccupyCells(mapper.CreatePointListFromMessage(opponentMove));
                    Console.Out.WriteLine(mapper.CreateMessageFromPoints(board.randomMove()));
                }
                else
                {
                    board.OccupyCells(mapper.CreatePointListFromMessage(opponentMove));
                    Console.Out.WriteLine(mapper.CreateMessageFromPoints(board.randomMove()));
                }
                    
                //fill board with opp movements
                //find best move
                //send best move
                //listen for end and break loop if found
                if (opponentMove == "end")
                    break;
            }
        }
    }
}
