using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for FlyingPlanView.xaml
    /// </summary>
    public partial class FlyingPlanView : Page
    {
        public FlyingPlanView()
        {
            InitializeComponent();
            for (int i = 0; i < MainWindow.FlyingPlan.Flyveplan.Length; i++)
            {
                this.DataList.Items.Add(MainWindow.FlyingPlan.Flyveplan[i]);
            }
        }
    }
}
