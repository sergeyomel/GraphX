using System;
using System.Windows;
using System.Windows.Media;

namespace Graph2
{
    public partial class InputWeight : Window
    {
        private MainGraph mGraph = null;
        private Point click;
        private Edge edge;
        int weight = 0;

        public InputWeight(MainGraph mG, Point cl, Edge ed)
        {
            InitializeComponent();
            mGraph = mG;
            click = cl;
            edge = ed;
            weight = edge.GetWeight();

            ed.SetColor(Brushes.Red);
            mG.DrawGraph();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mGraph.RemoveEdge(edge);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                weight = Convert.ToInt16(tbWeight.Text);
                if (weight < 0)
                    MessageBox.Show("Значение веса ребра не может быть меньше, чем 0");
                else
                    this.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            edge.SetColor(Brushes.Black);
            edge.SetWeight(weight);
            mGraph.DrawGraph();
            mGraph.printMatrixs();
        }
    }
}
