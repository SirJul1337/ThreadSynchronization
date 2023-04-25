using BagageSorteringsSystem;
using BagageSorteringsSystem.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Collections.Concurrent;
using System.Reflection;
namespace BagageSorteringsSystem;

public class Program
{
    public static ConsoleKey NavKey = ConsoleKey.A;
    private static AirPortManager airPortManager = new AirPortManager();

    public static void Main()
    {
        airPortManager.Run();


        while (true)
        {

            Console.Clear();
            Header();
            if (AirPortManager.NavDictionary.ContainsKey(NavKey))
            {
               AirPortManager.NavDictionary[NavKey].Invoke(); //TODO: fix problem with checkin Add and remove
            }
            NavInstructions();
            Thread.Sleep(100);

        }

    }
    private static void Header()
    {
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("             {0}                          ", DateTime.Now);
        Console.WriteLine("----------------------------------------------");
    }
    private static void NavInstructions()
    {
        Console.WriteLine("A. System overview  B. FlyingPlan  C. Checkin overview");
    }

    /// <summary>
    /// Method to write out the dashboard to see informations of different Buffers and planes
    /// </summary>
    public static void ViewOverview()
    {
        DashboardOverviewDTO dashboard = airPortManager.GetDashboardInfo();
        Console.WriteLine("Customers in queue: {0}", dashboard.customerLine.Count);
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("Baggages in sorting system: {0}", dashboard.systemBaggages.Count);
        Console.WriteLine("----------------------------------------------");

        foreach (var item in dashboard.TerminalQueues)
        {
            int id = item.Key;
            Console.WriteLine("Terminal {0} BaggageCount: {1}", id, dashboard.TerminalQueues[id].Count);
            if (dashboard.Planes.ContainsKey(id))
            {
                Console.WriteLine("Distination {0}", dashboard.Planes[id].Destination);
                Console.WriteLine("Takeoff: {0}", dashboard.Planes[id].Time);
                Console.WriteLine("Plane {0} baggage onboard: {1}/{2}", id, dashboard.Planes[id].Baggages.Count, dashboard.Planes[id].MaxCount);
            }

            Console.WriteLine("----------------------------------------------");
        }
    }
    /// <summary>
    /// Method to write in console the flying plan
    /// </summary>
    public static void ViewFlyPlan()
    {
        List<Flyveplan> FlyvePlaner = airPortManager.GetFlyvePlan();
        for (int i = 0; i < FlyvePlaner.Count; i++)
        {
            Console.WriteLine("Gate {0} | Destination {1} | Time {2}", FlyvePlaner[i].GateId,FlyvePlaner[i].Destination, FlyvePlaner[i].Afgangstid);
            Console.WriteLine("---------------------------------------------------------------------------");

        }


    }
    /// <summary>
    /// Method to write in console to see all opened Checkins
    /// </summary>
    public static void ViewCheckIns()
    {
        CheckIn[] checkIns = airPortManager.GetCheckins();
        for (int i = 0; i < checkIns.Length; i++)
        {
            Console.WriteLine("|----------------|");
            Console.WriteLine("|Check In box {0}  |", i);
            Console.WriteLine("|----------------|");
        }
        Console.WriteLine("P. Open checkin  M. Close checkin ");

    }
    
}