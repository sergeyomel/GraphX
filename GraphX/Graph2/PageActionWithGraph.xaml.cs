using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Graph2
{
    public partial class PageActionWithGraph : Page
    {

        PageDrawGraph pDG = null;
        PageInformationAboutGraph pInfo;
        WorkWithGraph WwG;
        MainGraph mG;
        PrintMatrix pr;

        public PageActionWithGraph(PageDrawGraph p, PageInformationAboutGraph pi)
        {
            InitializeComponent();
            pInfo = pi;
            WwG = new WorkWithGraph();
            pr = new PrintMatrix();
            pDG = p;
        }

        public void AddGraph(MainGraph mg)
        {
            mG = mg;
        }

        private void DeleteGraph(object sender, EventArgs e)
        {
            pDG.graphCanvas.Children.Clear();
            pDG.DELETE_GRAPH();
        }

        private void ClearGraph(object sender, EventArgs e)
        {
            mG.DrawGraph();
        }

        private void ClearText(object sender, EventArgs e)
        {
            tbBFS.Text = "";
            tbDFS.Text = "";
            tbPrim.Text = "";
            tbKrascal.Text = "";
            tbDeykstra.Text = "";
        }

        private void DrawRoad(List<(int from,int to)> lEdge)
        {
            pDG.from.SetStrokeColor(0, 0, 0);
            pDG.graphCanvas.Children.RemoveRange(pDG.graphCanvas.Children.Count - 2, 2);
            pDG.from.Draw();
            pDG.from = null;
            pDG.rightButtonDown = false;

            foreach (var edge in lEdge)
            {
                if (edge.from != -1)
                {
                    Edge tmpEdge = new Edge(mG.SearchVertex(edge.from), mG.SearchVertex(edge.to), -1, pDG.graphCanvas, 1);
                    tmpEdge.SetColor(Brushes.Red);
                    tmpEdge.Draw((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked);
                }
            }

            
        }

        private void DrawRoad(List<(int from, int to, int weight)> lEdge)
        {
            foreach (var edge in lEdge)
            {
                    Edge tmpEdge = new Edge(mG.SearchVertex(edge.from), mG.SearchVertex(edge.to), -1, pDG.graphCanvas, edge.weight);
                    tmpEdge.SetColor(Brushes.Red);
                    tmpEdge.Draw((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked);
            }
        }

        private void SearchBFS(object sender, RoutedEventArgs e)
        {
            if (pDG.from != null)
            {
                var answer = WwG.BFS(mG.GetListAdjacency((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked), pDG.from.GetID());
                tbBFS.Text = pr.printList(answer);
                DrawRoad(answer);
            }
            else
                MessageBox.Show("Пожалуйста, выберите стартовую вершину.", MessageBoxImage.Exclamation.ToString());
        }

        private void SearchDFS(object sender, RoutedEventArgs e)
        {
            if (pDG.from != null)
            {
                var answer = WwG.DFS(mG.GetListAdjacency((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked), pDG.from.GetID());
                tbDFS.Text = pr.printList(answer);
                DrawRoad(answer);
            }
            else
                MessageBox.Show("Пожалуйста, выберите стартовую вершину.", MessageBoxImage.Exclamation.ToString());
        }

        private void SearchPrim(object sender, RoutedEventArgs e)
        {
            if ((bool)pInfo.btnWeight.IsChecked)
            {
                var answer = WwG.AlgorithmPrima(mG.GetVertexes(), mG.GetEdge(), (bool)pInfo.btnOrient.IsChecked);
                tbPrim.Text = pr.printList(answer);
                DrawRoad(answer);
            }
            else
                MessageBox.Show("К сожалению, данный граф не является взвешенным.", MessageBoxImage.Exclamation.ToString());
        }

        private void SearchKruskal(object sender, RoutedEventArgs e)
        {
            if ((bool)pInfo.btnWeight.IsChecked)
            {
                var answer = WwG.AlgorithmKruskal(mG.GetVertexes(), mG.GetEdge());
                tbKrascal.Text = pr.printList(answer);
                DrawRoad(answer);
            }
            else
                MessageBox.Show("К сожалению, данный граф не является взвешенным.", MessageBoxImage.Exclamation.ToString());
        }

        private void SearchDijkstra(object sender, RoutedEventArgs e)
        {
            if (!(bool)pInfo.btnWeight.IsChecked)
                MessageBox.Show("К сожалению, данный граф не является взвешенным.", MessageBoxImage.Exclamation.ToString());
            else if (pDG.from == null)
                MessageBox.Show("Пожалуйста, выберите стартовую вершину.", MessageBoxImage.Exclamation.ToString());
            else
            {
                var d = WwG.AlgorithmDijkstra2(mG.GetMatrixWeight((bool)pInfo.btnOrient.IsChecked, (bool)pInfo.btnWeight.IsChecked), pDG.from);
                tbDeykstra.Text = pr.printDijkstra(d);
                pDG.from.SetStrokeColor(0, 0, 0);
                pDG.graphCanvas.Children.RemoveRange(pDG.graphCanvas.Children.Count - 2, 2);
                mG.DrawGraph();
                pDG.from = null;
                new TreeRoad(d, mG.pInfo).Show();
            }
        }

    }
}
