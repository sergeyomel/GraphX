using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graph2
{
    public partial class PageDrawGraph : Page
    {

        PageInformationAboutGraph pInfo = null;
        PageActionWithGraph pAction = null;
        MainGraph mG;

        public PageDrawGraph()
        {
            InitializeComponent();

            pInfo = new PageInformationAboutGraph(this);
            pAction = new PageActionWithGraph(this, pInfo);

            mG = new MainGraph(graphCanvas, pInfo);

            pInfo.AddGraph(mG);
            pAction.AddGraph(mG);
            FrameInfo.Navigate(pInfo);

            mG.printMatrixs();
        }

        public void DELETE_GRAPH()
        {
            mG = new MainGraph(graphCanvas, pInfo);
            pInfo.AddGraph(mG);
            pAction.AddGraph(mG);
            mG.printMatrixs();
        }

        #region навигация
        private void GoToInfoPage(object sender, EventArgs e)
        {
            FrameInfo.Navigate(pInfo);
        }

        private void GoToActionPage(object sender, EventArgs e)
        {
            FrameInfo.Navigate(pAction);
        }
        #endregion

        public PageInformationAboutGraph GetPInfo()
        {
            return pInfo;
        }
        public MainGraph GetMainGraph()
        {
            return mG;
        }
        public PageActionWithGraph GetPAction()
        {
            return pAction;
        }

        public bool rightButtonDown = false;
        public Vertex from;
        private Vertex to;

        private void LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rightButtonDown)
            {
                Edge resE = mG.PushOnEdge(e.GetPosition(graphCanvas));
                if (resE != null)
                {
                    if ((bool)pInfo.btnWeight.IsChecked)
                        new InputWeight(mG, e.GetPosition(graphCanvas), resE).ShowDialog();
                    else
                        mG.RemoveEdge(resE);
                }
                else
                {
                    Vertex resV = mG.PushOnVertex(e.GetPosition(graphCanvas));
                    if (resV == null)
                        mG.AddVertex(e.GetPosition(graphCanvas));
                    else
                        mG.RemoveVertex(e.GetPosition(graphCanvas));
                }
            }
        }

        private void RightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rightButtonDown && mG.PushOnVertex(e.GetPosition(graphCanvas)) != null)
            {
                rightButtonDown = true;
                from = mG.PushOnVertex(e.GetPosition(graphCanvas));
                from.SetStrokeColor(0, 145, 255);
                from.Draw();
            }
            else
            {
                rightButtonDown = false;
                if (from != null)
                {
                    from.SetStrokeColor(0, 0, 0);
                    graphCanvas.Children.RemoveRange(graphCanvas.Children.Count - 2, 2);
                    to = mG.PushOnVertex(e.GetPosition(graphCanvas));
                    if (to != null) {
                        Edge tmpEdge = new Edge(from, to, -1, null, 1);
                        Edge sEdge = mG.searchEdge(tmpEdge, (bool)pInfo.btnOrient.IsChecked);
                        if (sEdge == null)
                            mG.AddEdge(from, to);
                        else
                        {
                            if((bool)pInfo.btnWeight.IsChecked)
                                new InputWeight(mG, e.GetPosition(graphCanvas), sEdge).ShowDialog();
                            else
                                mG.RemoveEdge(sEdge);
                        }    
                    }
                    from = null;
                }   
            }


        }

    }
}
