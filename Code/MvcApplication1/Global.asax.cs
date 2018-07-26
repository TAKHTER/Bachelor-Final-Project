using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
//using System.Threading;
//using System.Threading.Timer;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Timers;

namespace MvcApplication1
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new AllEntitiesDbInitializer());
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 1000;
            timer.Start();
            timer.Enabled = true;

        }
        private static void timer_Elapsed(object state, EventArgs e)
        {
            AllEntitiesContext db = new AllEntitiesContext();
            //Routine aRoutine = db.Routines.Where(a => a.Day == "Saturday").Select(a => a).FirstOrDefault();
            List<TempRoutine> tempList = db.TempRoutines.Where(a => a.TempRoutineDate < DateTime.Now).Select(a => a).ToList();
            
            try
            {
                foreach(var a in tempList)
                {
                    BackupHelper aBackupHelper = new BackupHelper();
                    db.TempRoutineBackupModels.Add(aBackupHelper.GetTempRoputineBackupData(a));
                    db.TempRoutines.Remove(a);
                    db.SaveChanges();
                }
            }
            catch 
            {
                
            }
        }

        

    }
}