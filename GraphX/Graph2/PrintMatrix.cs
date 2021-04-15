using System;
using System.Collections.Generic;

namespace Graph2
{
    public class PrintMatrix
    {
        public string printMatrix(Dictionary<int, Dictionary<int, int>> matrix)
        {
            string output = String.Format("{0,3}|", '-');
            //Шапка таблицы, содержащая втооые вершины
            foreach (int row in matrix.Keys) {
                foreach (var col in matrix[row].Keys)
                {
                    output += String.Format("{0,3}|", col);
                }
            }
            output += '\n';

            //тело таблицы
            foreach(int row in matrix.Keys)
            {
                //Линия между строками
                for (int i = 0; i < matrix[matrix.Count].Count; i++)
                    output += "-----";
                output += '\n';

                //Подпись строки таблицы
                output += String.Format("{0,3}|", row);
                output += '|';

                //Таблица
                foreach(int colomn in matrix[row].Keys)
                {
                    output += String.Format("{0,3}|", matrix[row][colomn]);
                }
                output += '\n';
            }
            //Нижняя линия
            for (int i = 0; i < matrix[matrix.Count].Count; i++)
                output += "-----";
            output += '\n';

            return output;
        }

        public string printMatrix(int[,] matrix)
        {
            string output = "";
            int row = matrix.GetLength(0);
            int col = matrix.GetLength(1);

            if (row == 1 && col == 1)
                return output;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if(i == 0 || j == 0)
                        output += String.Format("{0,4} ||", matrix[i, j]);
                    else
                        output += String.Format("{0,5} |", matrix[i, j]);
                }
                output += '\n';
                for (int c = 0; c < col; c++)
                    output += "-------";
                output += '\n';
            }

            return output;
        }

        public string printListAdjacency(Dictionary<int, List<(int, int)>> listAdjacency)
        {
            string output = "";
            foreach(var key in listAdjacency.Keys)
            {
                output += key+": ";
                if (listAdjacency[key].Count == 0)
                    output += " ----- ";
                else
                {
                    foreach(var tuple in listAdjacency[key])
                    {
                        if (tuple.Item2 == -1)
                            output += string.Format("{0} ", tuple.Item1);
                        else
                            output += string.Format("{0}->{1} ", tuple.Item1, tuple.Item2);
                    }
                }
                output += "\n";
            }
            return output;
        }

        public string printList(List<int> lVisited)
        {
            string output = "";
            foreach (var vert in lVisited)
                output += string.Format("Посетили вершину с ID {0}\n", vert);
            return output;
        }

        public string printList(List<(int from, int to)> lVisited)
        {
            string output = "";
            foreach (var vert in lVisited)
                if (vert.from != -1)
                    output += string.Format("Пришли из вершины с ID {0} в вершину с ID {1}\n", vert.from, vert.to);
            return output;
        }

        public string printList2(List<(int from, int to)> lVisited)
        {
            string output = "";
            foreach (var vert in lVisited)
                if (vert.from != -1)
                    output += string.Format("Расстояние до вершины {0} равно {1}\n", vert.from, vert.to);
            return output;
        }

        public string printList(List<(int, int, int)> lVisited)
        {
            string output = "";
            foreach (var vert in lVisited)
                output += string.Format("Вышли из вершины с ID {0} в вершину с ID {1} по ребру с весом {2}\n", vert.Item1, vert.Item2, vert.Item3);
            return output;
        }

        public string printPow(Dictionary<int, List<(int, bool)>> dInput, bool orient)
        {
            var outList = new List<int>();
            var inpList = new List<int>();
            string outputString = "";
            foreach(var key in dInput.Keys)
            {
                foreach(var vert in dInput[key])
                {
                    if (vert.Item2)
                        outList.Add(vert.Item1);
                    else
                        inpList.Add(vert.Item1);
                }

                outputString += $"Из вершины {key} пришли в вершины: ";
                foreach (var vert in outList)
                    outputString += $" {vert}";
                outputString += $"| {outList.Count}\n";

                if (orient) { 
                    outputString += $"В вершину {key} пришли из вершин: ";
                    foreach (var vert in inpList)
                        outputString += $" {vert}";
                    outputString += $"| {inpList.Count}\n";
                }

                outputString += "\n";
                outList = new List<int>();
                inpList = new List<int>();
            }
            return outputString;
        }

        public string printDijkstra(Dictionary<int, (int, int)> lD)
        {
            string answer = "";
            foreach (var key in lD.Keys)
                answer += $"Пришли в вершину {key} из {lD[key].Item1} . Общий путь = {lD[key].Item2}\n";
            return answer;
        }
    }
}
