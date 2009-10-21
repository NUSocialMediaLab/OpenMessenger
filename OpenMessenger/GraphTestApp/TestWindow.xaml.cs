using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graph;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        bool started = false;

        public TestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Graph.Graph graph = Graph.Graph.GrowStar(6, 0.5f);
            Graph.Graph graph = Graph.Graph.GrowNetwork(6);
            graphCanvas.Model = graph;

            graphCanvas.ArcLayout(graph.FindNode(0),
                new Func<Graph.Graph.Edge, double>(
                    delegate(Graph.Graph.Edge edge) {
                        return 200f;
                    }),
                new Func<Graph.Graph.Edge, Brush>(
                    delegate(Graph.Graph.Edge edge) {
                        return Brushes.Black;
                    }));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!started)
            {
                started = true;
                graphCanvas.StartLayout(1f);
            }
            else
            {
                started = false;
                graphCanvas.StopLayout();
            }
        }

    }
}
