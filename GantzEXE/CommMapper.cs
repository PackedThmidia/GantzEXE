﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace GantzEXE
{
    class CommMaper
    {
        public void SendOK()
        {
            Console.Out.WriteLine("ok");
        }
        public string CreateMessageFromMove(Move move)
        {
            return "{" + move.p1.X + ";" + move.p1.Y + "},{" + move.p2.X + ";" + move.p2.Y + "}";
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

            stringfOfPoints = message.Split(',');
            foreach(string s in stringfOfPoints)
            {
                string[] point = s.Replace("{", "").Replace("}", "").Split(';');
                points.Add(new Point(int.Parse(point[0]), int.Parse(point[1])));
            }

            return points;
        }
    }
}
