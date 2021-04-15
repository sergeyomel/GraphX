using System.Collections.Generic;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Graph2
{
    
    public partial class TreeRoad : Window
    {
        
        int startX = 325;
        int startY = 325;
        double angle = 0;
        double radius = 225;

        Dictionary<int, (int, int)> answer;
        MainGraph mg;
        PageInformationAboutGraph pInfo;

        public TreeRoad(Dictionary<int, (int, int)> ans, PageInformationAboutGraph pi)
        {
            InitializeComponent();

            pInfo = pi;
            mg = new MainGraph(mCanvas, pInfo);
            answer = ans;
            Console.WriteLine(answer.Keys.Count);

            PrintMatrix pm = new PrintMatrix();
            Console.WriteLine(pm.printDijkstra(answer));

            AddVertexes();
            AddEdges();
            drawGraph();
        }

        private void AddVertexes()
        {
            double rotate = 360.0 / answer.Keys.Count;
            foreach (var key in answer.Keys)
            { 
                mg.AddVertex(new Point(Math.Cos(angle / 180 * Math.PI) * radius + startX, Math.Sin(angle / 180 * Math.PI) * radius + startY));
                angle += rotate;
            }
        }

        private Vertex SearchVertex(int id)
        {
            List<Vertex> lV = mg.GetVertexes();
            for (int number = 0; number < answer.Count; number++)
            {
                if (lV[number].GetID() == id)
                {
                    Console.WriteLine("нашёл");
                    return lV[number];
                }
            }
            return null;
        }

        private void AddEdges()
        {
            foreach(var vertex in answer.Keys)
            {
                if (answer[vertex].Item1 == -1)
                    continue;
                else
                {
                    Console.WriteLine($"start - {vertex}");
                    Console.WriteLine($"start - {answer[vertex].Item1}");
                    mg.AddEdge(SearchVertex(answer[vertex].Item1), SearchVertex(vertex), answer[vertex].Item2);
                }
            }
        }

        private void drawGraph()
        {
            foreach (var vertex in mg.GetVertexes())
                vertex.Draw();
            foreach (var edge in mg.GetEdge())
                edge.Draw(true, true);
        }

    }
}
