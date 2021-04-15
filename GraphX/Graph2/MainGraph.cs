using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Graph2
{
    public class MainGraph
    {
        WorkWithGraph wg = new WorkWithGraph();
        //индексы для ID
        private int countVertex = 0;
        private int currentCountVertex = 0;
        private int countEdge = 0;
        private int currentCountEdge = 0;

        //спики для вершин и рёбер
        private List<Vertex> Vertexes = new List<Vertex>();
        private List<Edge> Edges = new List<Edge>();

        public List<Vertex> GetVertexes() { return Vertexes; }
        public List<Edge> GetEdge() { return Edges; }
        public Vertex SearchVertex(int ID)
        {
            foreach(var vert in Vertexes)
            {
                if (vert.GetID() == ID)
                    return vert;
            }
            return null;
        }

        private Canvas canvas;
        PrintMatrix pM = new PrintMatrix();
        public PageInformationAboutGraph pInfo;

        public MainGraph(Canvas c, PageInformationAboutGraph pageInfo)
        {
            canvas = c;
            pInfo = pageInfo;
        }

        #region Add/Remove
        #region Add/Remove Vertex
        public void AddVertex(Point click)
        {
            if (PushOnVertex(click) == null)
            {
                if (currentCountVertex == 0)
                    countVertex = 0;
                ++currentCountVertex;
                Vertex vertex = new Vertex(click, ++countVertex, canvas);
                Vertexes.Add(vertex);
                vertex.Draw();
                printMatrixs();
            }
        }
        public void RemoveVertex(Point click)
        {
            Vertex tVertex = PushOnVertex(click);
            if (tVertex != null)
            {
                --currentCountVertex;
                if (tVertex.GetID() == countVertex)
                    --countVertex;
                Vertexes.Remove(tVertex);
                List<Edge> tlEdge = new List<Edge>();
                foreach (var edge in Edges)
                {
                    if (!edge.GetFrom().Equality(tVertex) && !edge.GetTo().Equality(tVertex))
                        tlEdge.Add(edge);
                }
                Edges = tlEdge;
                DrawGraph();
                printMatrixs();
            }
        }
        #endregion

        #region Add/Remove Edge
        public void AddEdge(Vertex from, Vertex to, int weight = 1)
        {
            if (currentCountEdge == 0)
                countEdge = 0;
            ++currentCountEdge;
            Edge tEdge = new Edge(from, to, ++countEdge, canvas, weight);
            Edges.Add(tEdge);
            DrawGraph();
            printMatrixs();
        }
        public void RemoveEdge(Edge edge)
        {
            --currentCountEdge;
            if (edge.GetID() == countEdge)
                --countEdge;
            Edges.Remove(edge);
            DrawGraph();
            printMatrixs();
        }
        //удаление рёбер при неориентированном графе из ориентированного
        public void RemoveRepeatingEdge(bool orient)
        {
            if (!orient)
            {
                var newEdges = new List<Edge>();
                bool flag = false;
                foreach (var e1 in Edges)
                {
                    flag = false;
                    foreach (Edge e2 in newEdges)
                    {
                        if (e1.GetFrom().Equality(e2.GetTo()) && e1.GetTo().Equality(e2.GetFrom()))
                            flag = true;
                    }
                    if (!flag)
                        newEdges.Add(e1);
                }
                Edges = newEdges;
            }
        }
        #endregion
        #endregion

        #region Matrix

        //матрица смежности - вершины вершины
        public int[,] GetMatrixAdjacency(bool orient)
        {
            var matrix = new int[Vertexes.Count + 1, Vertexes.Count + 1];
            //заполнение 0
            for (int i = 0; i < Vertexes.Count + 1; i++)
            {
                for (int j = 0; j < Vertexes.Count + 1; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            //верхняя строчка матрицы заполнена айдишниками вершин
            for (int i = 0; i < Vertexes.Count; i++)
            {
                matrix[0, i + 1] = Vertexes[i].GetID();
            }
            //левая строчка матрицы заполнена айдишниками вершин
            for (int i = 0; i < Vertexes.Count; i++)
            {
                matrix[i + 1, 0] = Vertexes[i].GetID();
            }

            //заполнение 1
            foreach (var edge in Edges)
            {
                var row = Vertexes.IndexOf(edge.GetFrom()) + 1;
                var column = Vertexes.IndexOf(edge.GetTo()) + 1;

                matrix[row, column] = 1;
                if (!orient)
                    matrix[column, row] = 1;
            }
            return matrix;
        }

        //матриц инциденстности - ребра вершины
        public int[,] GetMatrixIncidence(bool orient)
        {
            var matrix = new int[Vertexes.Count + 1, Edges.Count + 1];
            //заполнение 0
            for (int i = 0; i < Vertexes.Count + 1; i++)
            {
                for (int j = 0; j < Edges.Count + 1; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            //верхняя строчка матрицы заполнена айдишниками вершин
            for (int i = 0; i < Edges.Count; i++)
            {
                matrix[0, i + 1] = Edges[i].GetID();
            }
            //левая строчка матрицы заполнена айдишниками рёбер
            for (int i = 0; i < Vertexes.Count; i++)
            {
                matrix[i + 1, 0] = Vertexes[i].GetID();
            }
            //заполнение путями
            for (int i = 0; i < Edges.Count; i++)
            {
                var column = i + 1;
                var rowFrom = Vertexes.IndexOf(Edges[i].GetFrom()) + 1;
                var rowTo = Vertexes.IndexOf(Edges[i].GetTo()) + 1;

                matrix[rowFrom, column] = 1;
                if (orient)
                    matrix[rowTo, column] = -1;
                else
                    matrix[rowTo, column] = 1;
            }
            return matrix;
        }

        //матриц весов - ребра вершины
        public int[,] GetMatrixWeight(bool orient, bool weighted)
        {
            var matrix = new int[Vertexes.Count + 1, Vertexes.Count + 1];
            //заполнение 0
            for (int i = 0; i < Vertexes.Count + 1; i++)
            {
                for (int j = 0; j < Vertexes.Count + 1; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            //верхняя строчка матрицы заполнена айдишниками вершин
            for (int i = 0; i < Vertexes.Count; i++)
            {
                matrix[0, i + 1] = Vertexes[i].GetID();
            }
            //левая строчка матрицы заполнена айдишниками вершин
            for (int i = 0; i < Vertexes.Count; i++)
            {
                matrix[i + 1, 0] = Vertexes[i].GetID();
            }
            //заполнение весами
            if (!weighted)
                return matrix;
            else
            {
                foreach (var edge in Edges)
                {
                    var row = Vertexes.IndexOf(edge.GetFrom()) + 1;
                    var column = Vertexes.IndexOf(edge.GetTo()) + 1;

                    matrix[row, column] = edge.GetWeight();
                    if (!orient)
                        matrix[column, row] = edge.GetWeight();
                }
                return matrix;
            }
        }

        private List<(int, int)> sortesList(List<(int, int)> lS)
        {
            for (int x = 0; x < lS.Count-1; x++)
            {
                for (int y = 0; y < lS.Count- x - 1; y++)
                {
                    if (lS[y].Item1 > lS[y + 1].Item1)
                    {
                        var tup = lS[y];
                        lS[y] = lS[y + 1];
                        lS[y + 1] = tup;
                    }
                }
            }
            return lS;
        }

        //список смежности
        public Dictionary<int, List<(int, int)>> GetListAdjacency(bool orient, bool weighted)
        {
            var listAdjancy = new Dictionary<int, List<(int, int)>>();
            foreach (var vert in Vertexes)
                listAdjancy.Add(vert.GetID(), new List<(int, int)>());
            foreach (var vert in Vertexes)
            {
                foreach (var edge in Edges)
                {
                    if (edge.GetFrom().Equality(vert))
                    {
                        int weight = -1;
                        if (weighted)
                            weight = edge.GetWeight();
                        listAdjancy[vert.GetID()].Add((edge.GetTo().GetID(), weight));
                        if (!orient)
                            listAdjancy[edge.GetTo().GetID()].Add((vert.GetID(), weight));
                    }
                }
            }

            var lKeys = new List<int>();
            foreach (var key in listAdjancy.Keys)
                lKeys.Add(key);
            for (int key = 0; key < lKeys.Count; key++)
                listAdjancy[lKeys[key]] = sortesList(listAdjancy[lKeys[key]]); 
            
            return listAdjancy;
        }

        //список рёбер
        public string GetListEdges()
        {
            string output = "";
            foreach (var edge in Edges)
                output += edge.ToString() + "\n";
            return output;
        }

        //степени вершин
        public Dictionary<int, List<(int,bool)>> GetPowVertex()
        {
            bool orient = (bool)pInfo.btnOrient.IsChecked;
            var output = new Dictionary<int, List<(int, bool)>>();
            foreach(var vertex in Vertexes)
            {
                output.Add(vertex.GetID(), new List<(int, bool)>());
                foreach(var edge in Edges)
                {
                    if (!orient)
                    {
                        if (edge.GetFrom().Equality(vertex))
                            output[vertex.GetID()].Add((edge.GetTo().GetID(), true));
                        if (edge.GetTo().Equality(vertex))
                            output[vertex.GetID()].Add((edge.GetFrom().GetID(), true));
                    }
                    else
                    {
                        if (edge.GetFrom().GetID() == vertex.GetID())
                            output[vertex.GetID()].Add((edge.GetTo().GetID(), true));
                        if (edge.GetTo().GetID() == vertex.GetID())
                            output[vertex.GetID()].Add((edge.GetFrom().GetID(), false));
                        
                    }
                }
            }
            return output;
        }

        #endregion

        #region Нажатие на объект
        // нажатие на верщину
        public Vertex PushOnVertex(Point click)
        {
            double minlength = 9999;
            Vertex tVertex = null;

            foreach (var vertex in Vertexes)
            {
                double length = Math.Sqrt(Math.Pow(vertex.GetCenter().X - click.X, 2) + Math.Pow(vertex.GetCenter().Y - click.Y, 2));
                if ((length <= vertex.GetR()) && (length < minlength))
                {
                    minlength = length;
                    tVertex = vertex;
                }
            }
            return tVertex;
        }

        // нажатие на ребро
        public double Distance(double x0, double y0, double x1, double y1)
        {
            return Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
        }

        public Edge PushOnEdge(Point click)
        {
            double x = click.X;
            double y = click.Y;
            foreach(var edge in Edges)
            {
                double x1 = edge.GetFrom().GetCenter().X;
                double y1 = edge.GetFrom().GetCenter().Y;
                double x2 = edge.GetTo().GetCenter().X;
                double y2 = edge.GetTo().GetCenter().Y;

                double length = Distance(x1, y1, x2, y2);
                double tOne = Distance(x1, y1, x, y);
                double tTwo = Distance(x, y, x2, y2);

                if ((bool)pInfo.btnWeight.IsChecked)
                {
                    if ((tOne >= 1.5 * tTwo - 30) && (tOne <= 1.5 * tTwo + 30) && (tOne + tTwo <= length + 1.5))
                        return edge;
                }
                else
                {
                    if (tOne + tTwo <= length + 0.1)
                        return edge;
                }
            }
            return null;
        }
            #endregion

        //проверка ребра на вхождение
        public Edge searchEdge(Edge sEdge, bool orient)
        {
            if (orient)
            {
                foreach (var edge in Edges)
                {
                    if (edge.Equality(sEdge))
                        return edge;
                }
                return null;
            }
            else
            {
                foreach (var edge in Edges)
                {
                    var nEdge = new Edge(edge.GetTo(), edge.GetFrom(), -1, edge.GetCanvas());
                    if (edge.Equality(sEdge) || nEdge.Equality(sEdge))
                        return edge;
                }
                return null;
            }
        }

        //печать выходных данных
        public void printMatrixs()
        {
            bool orient = (bool)pInfo.btnOrient.IsChecked;
            bool weight = (bool)pInfo.btnWeight.IsChecked;
            pInfo.tbAdjacencyMatrix.Text = pM.printMatrix(GetMatrixAdjacency(orient));
            pInfo.tbIncidentMatrix.Text = pM.printMatrix(GetMatrixIncidence(orient));
            pInfo.tbLibraMatrix.Text = pM.printMatrix(GetMatrixWeight(orient, weight));
            pInfo.tbListAdjacency.Text = pM.printListAdjacency(GetListAdjacency(orient, weight));
            pInfo.tbListEdges.Text = GetListEdges();
            pInfo.tbPowVertex.Text = pM.printPow(GetPowVertex(), orient);
            pInfo.tbGraph.Text = TypeGraph();
        }

        private string TypeGraph()
        {
            var type = wg.EulerGraph(GetPowVertex());
            bool orient = (bool)pInfo.btnOrient.IsChecked;
            bool weight = (bool)pInfo.btnWeight.IsChecked;
            switch (type)
            {
                case 0:
                    return "Граф общего вида";
                case 1:
                    {
                        string answer = "Граф является полуэйлеровым\n";
                        if (Vertexes.Count > 0)
                            answer += pM.printList(wg.EulerLoop(Vertexes, Edges, GetListAdjacency(orient, weight), Vertexes[0].GetID(), orient));
                        return answer;
                    }
                case 2:
                    {
                        string answer = "Граф является Эйлеровым\n";
                        if(Vertexes.Count > 0)
                            answer += pM.printList(wg.EulerLoop(GetVertexes(), GetEdge(), GetListAdjacency(orient, weight), Vertexes[0].GetID(), orient));
                        return answer;
                    }
                    
            }
            return "";
        }

        //отрисовка графа
        public void DrawGraph()
        {
            canvas.Children.Clear();
            foreach (var vertex in Vertexes)
                vertex.Draw();
            foreach (var edge in Edges)
                edge.Draw((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked);
        }

    }
}
