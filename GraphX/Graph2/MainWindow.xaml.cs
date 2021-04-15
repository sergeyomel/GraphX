using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Graph2
{
    public partial class MainWindow : Window
    {
        PageDrawGraph pDrawGraph = new PageDrawGraph();
        PageSettings pSettings = new PageSettings();
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PageDrawGraph());
        }

        private void GoToGraph(object sender, EventArgs e)
        {
            MainFrame.Navigate(pDrawGraph);
        }

        private void GoToSettings(object sender, EventArgs e)
        {
            MainFrame.Navigate(pSettings);
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {

        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "txt files (*.txt)|*.txt";
            saveFile.DefaultExt = "bmp";
            saveFile.InitialDirectory = @"C:\";

            using (var path_dialog = new FolderBrowserDialog())
                if (path_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter fs = new StreamWriter($"{path_dialog.SelectedPath}\\Graph.txt"))
                        {
                            fs.WriteLine("Матрица смежности:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbAdjacencyMatrix.Text);
                            fs.WriteLine("Матрица инцидентности:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbIncidentMatrix.Text);
                            fs.WriteLine("Матрица весов:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbLibraMatrix.Text);
                            fs.WriteLine("Список смежности:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbListAdjacency.Text);
                            fs.WriteLine("Список рёбер:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbListEdges.Text);
                            fs.WriteLine("Список степеней вершин:\n");
                            fs.WriteLine(pDrawGraph.GetPInfo().tbPowVertex.Text);
                            fs.WriteLine();
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                };
        }
    }
}
