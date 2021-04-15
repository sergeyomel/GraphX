using System.Collections.Generic;
using System.Linq;
using System;

namespace Graph2
{
    class WorkWithGraph
    {
        PrintMatrix pm = new PrintMatrix();

        //Алгоритмы поиска элементов
        private bool SearchElement(Queue<(int, int)> q, int el)
        {
            foreach (var tup in q)
                if (tup.Item2 == el)
                    return true;
            return false;
        }
        private bool SearchElement(Stack<(int, int)> s, int el)
        {
            foreach (var tup in s)
                if (tup.Item2 == el)
                    return true;
            return false;
        }
        private int SearchPosition(List<(int from, int to)> lInput, int index)
        {
            for (int pos = 0; pos < lInput.Count; pos++)
                if (lInput[pos].to == index)
                    return pos;
            return -1;
        }

        //Поиск в ширину
        public List<(int, int)> BFS(Dictionary<int, List<(int, int)>> listAdj, int stID)
        {
            var lVisited = new List<int>();
            var lEdge = new List<(int from, int to)>();
            var lQueue = new Queue<(int from, int to)>();

            while (lVisited.Count != listAdj.Keys.Count)
            {
                lEdge.Add((-1, stID));
                lVisited.Add(stID);
                lQueue.Enqueue((-1, stID));

                while (lQueue.Count != 0)
                {
                    int nID = lQueue.Dequeue().to;
                    var tmplVisited = new List<int>(lVisited);
                    foreach (var id in listAdj[nID])
                    {
                        if (!tmplVisited.Contains(id.Item1) && !SearchElement(lQueue, id.Item1))
                        {
                            lEdge.Add((nID, id.Item1));
                            tmplVisited.Add(id.Item1);
                            lQueue.Enqueue((nID, id.Item1));
                        }
                    }
                    lVisited = new List<int>(tmplVisited);;
                }

                if (lVisited.Count != listAdj.Keys.Count)
                {
                    foreach (var key in listAdj.Keys)
                        if (!lVisited.Contains(key))
                        {
                            stID = key;
                            break;
                        }
                }
            }

            return lEdge;
        }

        //Поиск в глубину
        public List<(int,int)> DFS(Dictionary<int, List<(int, int)>> listAdj, int stID)
        {
            var lVisited = new List<int>();
            var lEdge = new List<(int, int)>();
            var lStack = new Stack<(int,int)>();

            while (lVisited.Count != listAdj.Keys.Count)
            {
                lVisited.Add(stID);
                lEdge.Add((-1, stID));
                lStack.Push((-1, stID));

                while (lStack.Count != 0)
                {
                    int nID = lStack.Peek().Item2;
                    bool notVisited = false;
                    if (listAdj[nID].Count == 0)
                        notVisited = true;
                    for (int index = 0; index < listAdj[nID].Count; index++)
                    {
                        var vert = listAdj[nID][index].Item1;
                        if (!lVisited.Contains(vert) && !SearchElement(lStack, vert))
                        {
                            lVisited.Add(vert);
                            var tmpL = (nID, vert);
                            lEdge.InsertRange(SearchPosition(lEdge, nID), new List<(int, int)> { tmpL });
                            lStack.Push(tmpL);
                            break;
                        }
                        if (index == listAdj[nID].Count - 1)
                            notVisited = true;
                    }
                    if (notVisited)
                        lStack.Pop();
                }

                if (lVisited.Count != listAdj.Keys.Count)
                {
                    foreach (var key in listAdj.Keys)
                        if (!lVisited.Contains(key))
                        {
                            stID = key;
                            break;
                        }
                }
            }
            lEdge.Reverse();

            return lEdge;
        }

        //Алгоритм Прима
        public List<(int, int, int)> AlgorithmPrima(List<Vertex> Vertexes, List<Edge> Edges, bool orient)
        {
            var lVisited = new List<Vertex>();
            var lEdges = SortedEdge(Edges);
            var lOutVertex = new List<(int from, int to, int weight)>();

            lVisited.Add(lEdges[0].GetFrom());

            while (lVisited.Count != Vertexes.Count)
            {
                Edge tmpEdge = null;
                int minW = 1000;
                bool flReverse = false;
                foreach (var vertex in lVisited)
                {
                    foreach (var edge in Edges)
                    {
                        if (edge.GetFrom().Equality(vertex) && edge.GetWeight() < minW && !lVisited.Contains(edge.GetTo()))
                        {
                            minW = edge.GetWeight();
                            tmpEdge = edge;
                            flReverse = false;
                        }
                        if (!orient)
                        {
                            if (edge.GetTo().Equality(vertex) && edge.GetWeight() < minW && !lVisited.Contains(edge.GetFrom()))
                            {
                                minW = edge.GetWeight();
                                tmpEdge = edge;
                                flReverse = true;
                            }
                        }
                    }
                }
                if(flReverse)
                    lVisited.Add(tmpEdge.GetFrom());
                else
                    lVisited.Add(tmpEdge.GetTo());
                lOutVertex.Add((tmpEdge.GetFrom().GetID(), tmpEdge.GetTo().GetID(), tmpEdge.GetWeight()));
            }
            return lOutVertex;
        }

        #region Алгоритм Крускала
        //Соритровка рёбер
        private List<Edge> SortedEdge(List<Edge> Edges)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                for (int j = Edges.Count - 1; j > i; j--)
                {
                    if (Edges[j - 1].GetWeight() > Edges[j].GetWeight())
                    {
                        var tmp = Edges[j - 1];
                        Edges[j - 1] = Edges[j];
                        Edges[j] = tmp;
                    }
                }
            }
            foreach (var edge in Edges)
            {
                Console.WriteLine(edge);
                Console.WriteLine(" weight - " + edge.GetWeight());
            }
            return Edges;
        }
        //Объединение списков
        private List<List<Vertex>> UnionList(List<List<Vertex>> lSet, Vertex v1, Vertex v2)
        {
            int index1 = 0;
            int index2 = 0;
            for (int i = 0; i < lSet.Count; i++)
                if (lSet[i].Contains(v1))
                {
                    index1 = i;
                    break;
                }
            for (int i = 0; i < lSet.Count; i++)
                if (lSet[i].Contains(v2))
                {
                    index2 = i;
                    break;
                }
            foreach (var v in lSet[index2])
                lSet[index1].Add(v);
            lSet.RemoveAt(index2);

            return lSet;
        }
        //Проверка на разность списков
        private bool DifferentSets(List<List<Vertex>> lSet, Vertex v1, Vertex v2)
        {
            foreach (var lset in lSet)
                if (lset.Contains(v1) && lset.Contains(v2))
                    return false;
            return true;
        }
        //Алгоритм Крускала
        public List<(int from, int to, int weight)> AlgorithmKruskal(List<Vertex> vertexes, List<Edge> nedges)
        {
            var edges = SortedEdge(nedges);
            var lSetVertex = new List<List<Vertex>>();
            var lOutput = new List<(int from, int to, int weight)>();

            foreach (var v in vertexes)
                lSetVertex.Add(new List<Vertex> { v });

            foreach(var edge in edges)
            {
                if (DifferentSets(lSetVertex, edge.GetFrom(), edge.GetTo()))
                {
                    lSetVertex = UnionList(lSetVertex, edge.GetFrom(), edge.GetTo());
                    lOutput.Add((edge.GetFrom().GetID(), edge.GetTo().GetID(), edge.GetWeight()));
                    if (lSetVertex.Count == 1)
                        break;
                }
            }
            return lOutput;
        }
        #endregion

        //Эйлеров граф
        public byte EulerGraph(Dictionary<int, List<(int, bool)>> lAdj)
        {
            int countOddPow = 0;
            int countEvenPow = 0;
            foreach (var lPow in lAdj.Keys)
            {
                if (lAdj[lPow].Count % 2 == 1)
                    countOddPow += 1;
                else
                    countEvenPow += 1;
            }

            if (countEvenPow == lAdj.Keys.Count)
                return 2;
            else if (countOddPow == 2)
                return 1;
            else
                return 0;
        }

        //Эйлеров цикл
        public List<int> EulerLoop(List<Vertex> lV, List<Edge> lE, Dictionary<int, List<(int, int)>> lAdj, int startVertex, bool orient)
        {
            List<int> answer = new List<int>();
            Stack<int> sVisitedEdge = new Stack<int>();
            List<Vertex> lVertex = new List<Vertex>();
            foreach (var v in lV)
                lVertex.Add(v);
            List<Edge> lEdge = new List<Edge>();
            foreach (var e in lE)
                lEdge.Add(e);

            foreach(var vertex in lAdj.Keys)
                if(lAdj[vertex].Count % 2 == 1)
                {
                    startVertex = vertex;
                    break;
                }

            sVisitedEdge.Push(startVertex);
            answer.Add(startVertex);

            while(sVisitedEdge.Count != 0)
            {
                int vertex = sVisitedEdge.Peek();
                bool foundEdge = false;
                Edge deleteEdge = null;
                foreach(var vert in lVertex)
                {
                    foreach(var edge in lEdge)
                    {
                        if( (edge.GetFrom().GetID() == vertex && edge.GetTo().GetID() == vert.GetID())
                            //||
                            //(!orient && edge.GetFrom().GetID() == vert.GetID() && edge.GetTo().GetID() == vertex)
                            )
                        {
                            sVisitedEdge.Push(vert.GetID());
                            answer.Add(vert.GetID());
                            deleteEdge = edge;
                            foundEdge = true;
                            break;
                        }
                    }
                    lEdge.Remove(deleteEdge);
                }
                if (!foundEdge)
                    sVisitedEdge.Pop();
            }

            return answer;
        }

        #region Алгоритм Дейкстры
        //Поиск индекса в массиве по ID вершины
        private int SearchIndexInArray(int[,] array, int ID)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[0,i] == ID)
                    return i;
            return -1;
        }

        //Алгоритм Дейкстры
        public List<(int, int)> AlgorithmDijkstra(int[,] MatrixWeight, Vertex start)
        {
            //Инициализация переменных
            List<(int, int)> answer = new List<(int, int)>();
            int rows = MatrixWeight.GetLength(0)-1;
            int col  = MatrixWeight.GetLength(1)-1;
            int currentRowsMD = 1;

            //Список посещённых вершин
            List<int> lVisited = new List<int>();

            //Матрица расстояний до каждой вершины
            int[,] matrixD = new int[rows, col];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    matrixD[r, c] = -1;
                }
            }
            for (int i = 0; i < col; i++)
                matrixD[0, i] = int.MaxValue;

            int index = SearchIndexInArray(MatrixWeight, start.GetID())-1;
            matrixD[0, index] = 0;
            int currentRowsMW = index+1;
            lVisited.Add(index);

            while(lVisited.Count != rows)
            {
                int startElement = matrixD[currentRowsMD-1, index];
                for (int pos = 0; pos < col; pos++)
                {
                    if (!lVisited.Contains(pos))
                    {
                        int currentElement = MatrixWeight[currentRowsMW, pos+1];
                        if (currentElement == 0)
                            matrixD[currentRowsMD, pos] = matrixD[currentRowsMD - 1, pos];
                        else if (startElement + currentElement < matrixD[currentRowsMD - 1, pos])
                        {
                            matrixD[currentRowsMD, pos] = startElement + currentElement;
                            //answer[pos + 1] = (index + 1, startElement + currentElement);
                        }
                        else
                            matrixD[currentRowsMD, pos] = matrixD[currentRowsMD - 1, pos];
                    }
                }
                Console.WriteLine(pm.printMatrix(matrixD));
                Console.WriteLine();
                int min = int.MaxValue;
                for (int pos = 0; pos < rows; pos++) 
                {
                    if (matrixD[currentRowsMD, pos] < min && matrixD[currentRowsMD, pos] >= 0) 
                    {
                        min = matrixD[currentRowsMD, pos];
                        index = pos;
                    }
                }
                currentRowsMW = index+1;
                currentRowsMD += 1;
                lVisited.Add(index);
            }

            bool flag = false;
            for (int c = 0; c < col; c++)
            {
                flag = false;
                for (int r = 0; r < rows; r++)
                {
                    if (matrixD[r, c] == -1) {
                        answer.Add((MatrixWeight[0, c + 1], matrixD[r - 1, c]));
                        flag = true;
                        break;
                    }
                }
                if(!flag)
                    answer.Add((MatrixWeight[0, c + 1], matrixD[rows-1, c]));
            }

            return answer;
        }

        public Dictionary<int, (int, int)> AlgorithmDijkstra2(int[,] MatrixWeight, Vertex start)
        {
            //Инициализация переменных
            Dictionary<int, (int, int)> answer = new Dictionary<int, (int, int)>();
            for (int number = 1; number < MatrixWeight.GetLength(0); number++)
                answer.Add(MatrixWeight[0, number], (-1, 0));
            int rows = MatrixWeight.GetLength(0) - 1;
            int col = MatrixWeight.GetLength(1) - 1;
            int currentRowsMD = 1;

            //Список посещённых вершин
            List<int> lVisited = new List<int>();

            //Матрица расстояний до каждой вершины
            int[,] matrixD = new int[rows, col];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    matrixD[r, c] = -1;
                }
            }
            for (int i = 0; i < col; i++)
                matrixD[0, i] = int.MaxValue;

            int index = SearchIndexInArray(MatrixWeight, start.GetID()) - 1;
            matrixD[0, index] = 0;
            int currentRowsMW = index + 1;
            lVisited.Add(index);

            while (lVisited.Count != rows)
            {
                int startElement = matrixD[currentRowsMD - 1, index];
                for (int pos = 0; pos < col; pos++)
                {
                    if (!lVisited.Contains(pos))
                    {
                        int currentElement = MatrixWeight[currentRowsMW, pos + 1];
                        if (currentElement == 0)
                            matrixD[currentRowsMD, pos] = matrixD[currentRowsMD - 1, pos];
                        else if (startElement + currentElement < matrixD[currentRowsMD - 1, pos])
                        {
                            matrixD[currentRowsMD, pos] = startElement + currentElement;
                            answer[pos + 1] = (index + 1, startElement + currentElement);
                        }
                        else
                            matrixD[currentRowsMD, pos] = matrixD[currentRowsMD - 1, pos];
                    }
                }
                Console.WriteLine(pm.printMatrix(matrixD));
                Console.WriteLine();
                int min = int.MaxValue;
                for (int pos = 0; pos < rows; pos++)
                {
                    if (matrixD[currentRowsMD, pos] < min && matrixD[currentRowsMD, pos] >= 0)
                    {
                        min = matrixD[currentRowsMD, pos];
                        index = pos;
                    }
                }
                currentRowsMW = index + 1;
                currentRowsMD += 1;
                lVisited.Add(index);
            }
            return answer;
        }

        #endregion

    }
}
