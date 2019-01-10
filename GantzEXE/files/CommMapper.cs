using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace GantZ
{
    class CommMaper
    {

        //send your move
        //send ok
        //listen on i/o for commands
        //command list
        //integer with map size
        //initil obstacle list; {0;1},{2;2},{2;3}
        //judge sends START or initial move {x,y},{x2,y2}
        public void SendOK()
        {
            Console.Out.WriteLine("ok");
        }
        public string CreateMessageFromPoints(List<Point> points)
        {
            string message = "";
            foreach(Point p in points)
            {
                message += "{" + p.X + ";" + p.Y + "},";
            }
            return message.TrimEnd(','); ;
        }
        public List<Point> CreatePointListFromMessage(string message)
        {
            string[] stringfOfPoints;
            List<Point> points = new List<Point>();

            stringfOfPoints = message.Split(",");
            foreach(string s in stringfOfPoints)
            {
                string[] point = s.Replace("{", "").Replace("}", "").Split(";");
                points.Add(new Point(int.Parse(point[0]), int.Parse(point[1])));
            }

            return points;
        }
    }
}
