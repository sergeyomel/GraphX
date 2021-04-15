using System.Windows.Controls;

namespace Graph2
{
    public partial class PageInformationAboutGraph : Page
    {
        private MainGraph mG;

        public PageInformationAboutGraph()
        {
        }

        public PageInformationAboutGraph(PageDrawGraph dg)
        {
            InitializeComponent();
        }

        public void AddGraph(MainGraph mg)
        {
            mG = mg;
        }

        private void changeOfState(object sender, System.Windows.RoutedEventArgs e)
        {
            mG.RemoveRepeatingEdge((bool)btnOrient.IsChecked);
            mG.DrawGraph();
            mG.printMatrixs();
        }

    }
}
