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
using System.Windows.Threading;

namespace BaggageSortertingSystemWpf
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private static CheckInManager _checkInManager = new CheckInManager();
        public Dashboard()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(UpdateList);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
            UpdateCheckIn();
        }
        private void UpdateList(object sender, EventArgs e)
        {
            SortSystemCount.Content = String.Format("{0}/{1}",MainWindow.Baggages.Count, 50);
            this.CheckInList.Items.Clear();
            this.TerminalList.Items.Clear();
            this.Planes.Items.Clear();
            var openList = MainWindow.CheckIns.Where(c => c.Alive == true).ToList();
            for (int i = 0; i < openList.Count; i++)
            {
                this.CheckInList.Items.Add(openList[i]);
            }
            //Console.WriteLine("Baggages in sorting system: {0}", MainWindow.Baggages.Count);
            if (Monitor.TryEnter(MainWindow.TerminalQueues))
            {
                for (int i = 0; i < MainWindow.TerminalQueues.Count; i++)
                {

                }
                foreach (var item in MainWindow.TerminalQueues)
                {
                    int id = item.Key;
                    if (MainWindow.Planes.ContainsKey(id))
                    {
                        TerminalList.Items.Add(MainWindow.TerminalQueues[id]);
                        Planes.Items.Add(MainWindow.Planes[id].Baggages.Count);
                        //Console.WriteLine("Terminal {0} BaggageCount: {1}", id, MainWindow.TerminalQueues[id].Count);
                        //Console.WriteLine("Distination {0}", MainWindow.Planes[id].Destination);
                        //Console.WriteLine("Takeoff: {0}", MainWindow.Planes[id].Time);
                        //Console.WriteLine("Plane {0} baggage onboard: {1}/{2}", id, MainWindow.Planes[id].Baggages.Count, MainWindow.Planes[id].MaxCount);
                    }
                }
                Monitor.Exit(MainWindow.TerminalQueues);
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            _checkInManager.Remove();
            UpdateCheckIn();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            _checkInManager.Add();
            UpdateCheckIn();
        }
        private void UpdateCheckIn()
        {
            
        }
    }
}
