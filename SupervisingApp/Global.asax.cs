using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NajmDefault
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //string con = ConfigurationManager.ConnectionStrings["ProjDBConnectionString"].ConnectionString;
        ////string con = "Data Source=.\\SQLEXPRESS2017;Initial Catalog=PROJDB;User Id = najm; Password=0000"; 
        protected void Application_Start()
        {
            //    SqlConnection sqlConnection = new SqlConnection(con);
            //    SqlCommand cmd = new SqlCommand();
            //    //SqlDataReader reader;

            //    //cmd.CommandText = "ALTER DATABASE[ProjDB] SET ENABLE_BROKER";
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = sqlConnection;

            //    sqlConnection.Open();
            //    //reader = cmd.ExecuteReader();
            //    sqlConnection.Close();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //    //here in Application Start we will start Sql Dependency
            //    SqlDependency.Start(con);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    NotificationComponent NC = new NotificationComponent();
        //    var currentTime = DateTime.Now;
        //    HttpContext.Current.Session["LastUpdated"] = currentTime;
        //    NC.RegisterNotification(currentTime);
        //}

        //protected void Application_End()
        //{
        //    //here we will stop Sql Dependency
        //    SqlDependency.Stop(con);
        //}
    }
}
