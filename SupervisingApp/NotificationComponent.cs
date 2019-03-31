using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using NajmDefault.Models;
using NajmDefault.LinqDataBase;

namespace NajmDefault
{
    public class NotificationComponent
    {
        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["ProjDBConnectionString"].ConnectionString;
            string sqlCommand = @"SELECT [id],[date],[text],[stationID] from [dbo].[alarmes] where [date] > @date";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@date", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                //re-register notification
                RegisterNotification(DateTime.Now);

            }
        }

        //fetch last 5 notification from database
        public List<Alarme> GetAlerts()
        {
            projetDataContext dc = new projetDataContext();
            var query = (from alarme in dc.alarmes orderby alarme.date descending select alarme).Take(5);
            List<Alarme> list = new List<Alarme>();

            foreach (var alarme in query)
            {
                list.Add(new Alarme
                {
                    date = alarme.date.Value.ToString(),
                    stationID = alarme.stationID.Value,
                    countrie = alarme.station.pay.Replace(" ", string.Empty)
                });
            }

            return list;
        }

        //fetch all notifications from database
        public List<Alarme> GetAllAlerts()
        {
            projetDataContext dc = new projetDataContext();
            var query = from alarme in dc.alarmes orderby alarme.date descending select alarme;
            List<Alarme> list = new List<Alarme>();

            foreach (var alarme in query)
            {
                if (alarme.state == null)
                    alarme.state = false;

                list.Add(new Alarme
                {
                    id = alarme.ID,
                    date = alarme.date.Value.ToString(),
                    stationID = alarme.stationID.Value,
                    countrie = alarme.station.pay.Replace(" ", string.Empty),
                    text = alarme.text,
                    state = (bool)alarme.state
                });
        }

            return list;
        }
}
}