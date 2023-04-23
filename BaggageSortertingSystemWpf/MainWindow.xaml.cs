using BaggageSortertingSystemWpf.Models;
using Serilog;
using Serilog.Core;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Queue<Baggage> Baggages = new();
        public static Dictionary<int, Queue<Baggage>> TerminalQueues = new();
        public static Dictionary<int, Plane> Planes = new();
        public static Dictionary<int, Terminal> Terminals = new();
        public static CheckIn[] CheckIns = new CheckIn[3] { new CheckIn(), new CheckIn(), new CheckIn() };
        public static Queue<Baggage> CustomerLine = new();
        public static Queue<Baggage> LostBaggage = new();
        public static Logger Logger;
        public static Dictionary<ConsoleKey, Action> NavDictionary = new();
        public static ConsoleKey NavKey = ConsoleKey.A;
        public static FlyingPlan FlyingPlan;
        
        public MainWindow()
        {
            InitializeComponent();
            SideBar.Content = new SidePanel(Content);
            Content.Content = new ContentPanel();
            Startup();
        }
        private static void Startup()
        {
            Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            Customer customer = new Customer();
            BaggageHandling handling = new BaggageHandling();
            ThreadPool.QueueUserWorkItem(customer.AutoGenerate);
            ThreadPool.QueueUserWorkItem(handling.Sorting);
            ControlTower controlTower = new ControlTower();
            ThreadPool.QueueUserWorkItem(controlTower.ControlGates);
        }
    }
}
