using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaggageSortertingSystemWpf
{
    /// <summary>
    /// Interaction logic for SidePanel.xaml
    /// </summary>
    public partial class SidePanel : Page
    {
        private Frame _mainFrame;
        public SidePanel(Frame MainFrame)
        {
            InitializeComponent();
            _mainFrame = MainFrame;

        }
        private void UpdateClock(object callback)
        {

            Clock.Content = DateTime.Now;
            
        }

        private void FlyingPlanButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new FlyingPlanView();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Content = new Dashboard();
        }
    }
}
