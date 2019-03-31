using NajmDefault.LinqDataBase;
using NajmDefault.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace NajmDefault
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MsgWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MsgWcfService.svc or MsgWcfService.svc.cs at the Solution Explorer and start debugging.
    public class MsgWcfService : IMsgWcfService
    {
        string _connectionString = ConfigurationManager.ConnectionStrings["ProjDBConnectionString"].ConnectionString;

        public string SetLevel(string message)
        {

            projetDataContext dc = new projetDataContext();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string ret = "";
                while (message.Length != 0)
                {
                    int length = message.Length;
                    int id = int.Parse(message.Substring(message.IndexOf("k") + 1, message.IndexOf("=") - message.IndexOf("k") - 1));
                    string levelstr = (message.Substring(message.IndexOf("=") + 1, message.IndexOf(",") - message.IndexOf("=") - 1));
                    levelstr = levelstr.Replace(".", ",");
                    float level = float.Parse(levelstr);
                    var query = "update [dbo].[reservoirs] set niveau=@level where id=" + id;
                    var query2 = "INSERT INTO [dbo].[mouvReservoir] ([date], [reservoirID], [niveau]) VALUES (@date, @reservoirID, @niveau)";
                    //var query3 = "SELECT TOP 1 * FROM mouvReservoir ORDER BY date DESC where reservoirID=" + id;
                    var query3 = from reservoir in dc.reservoirs
                                 where reservoir.id == id
                                 select reservoir;
                    try
                    {
                        message = message.Substring(message.IndexOf(",") + 1, length - message.IndexOf(",") - 1);
                        var reservoir = query3.First();
                        //si le niveau de reservoir a augmenté on ajoute un mouvement de reservoir
                        if (reservoir.niveau < level && reservoir.niveau >= 0)
                        {
                            using (SqlCommand command = new SqlCommand(query2, connection))
                            {
                                command.Parameters.AddWithValue("@date", System.DateTime.Now);
                                command.Parameters.AddWithValue("@reservoirID", id);
                                command.Parameters.AddWithValue("@niveau", LevelToLiter(reservoir.id, level) - LevelToLiter(reservoir.id, (int)reservoir.niveau));
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                        //metre a jour le niveau du reservoir
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@level", level);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                        ret = "wcfservice est executé avec succès !";
                    }
                    catch (Exception e)
                    {
                        return (e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
                return ret;

            }

        }

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public int LevelToLiter(int ResID, float level)
        {
            projetDataContext dc = new projetDataContext();

            var query1 = from reservoir in dc.reservoirs
                         where reservoir.id == ResID
                         select reservoir;
            var query2 = from barem in dc.barémages
                         where barem.reservoirID == ResID
                         select barem;
            var res = query1.First();
            var barémage = query2.First();

            //int niv = (int)res.niveau;
            double a = 0,
                   b = 0;

            if (level > 0 && level <= 5)
            {
                a = (double)barémage.vol1 / 5;
                b = 0;
            }

            else if (level > 5 && level <= 10)
            {
                a = ((double)(barémage.vol2 - barémage.vol1) / (10 - 5));
                b = (barémage.vol1 - a * 5).Value;
            }
            else if (level > 10 && level <= 15)
            {
                a = ((double)(barémage.vol3 - barémage.vol2) / (15 - 10));
                b = (barémage.vol2 - a * 10).Value;
            }
            else if (level > 15 && level <= 20)
            {
                a = ((double)(barémage.vol4 - barémage.vol3) / (20 - 15));
                b = ((double)barémage.vol3 - a * 15);
            }
            else if (level > 20 && level <= 25)
            {
                a = ((double)(barémage.vol5 - barémage.vol4) / (25 - 20));
                b = ((double)barémage.vol4 - a * 20);
            }
            else if (level > 25 && level <= 30)
            {
                a = ((double)(barémage.vol6 - barémage.vol5) / (30 - 25));
                b = ((double)barémage.vol5 - a * 25);
            }
            else if (level > 30 && level <= 35)
            {
                a = ((double)(barémage.vol7 - barémage.vol6) / (35 - 30));
                b = ((double)barémage.vol6 - a * 30);
            }
            else if (level > 35 && level <= 40)
            {
                a = ((double)(barémage.vol8 - barémage.vol7) / (40 - 35));
                b = ((double)barémage.vol7 - a * 35);
            }
            else if (level > 40 && level <= 45)
            {
                a = ((double)(barémage.vol9 - barémage.vol8) / (45 - 40));
                b = ((double)barémage.vol8 - a * 40);
            }
            else if (level > 45 && level <= 50)
            {
                a = ((double)(barémage.vol10 - barémage.vol9) / (50 - 45));
                b = ((double)barémage.vol9 - a * 45);
            }
            else if (level > 50 && level <= 55)
            {
                a = ((double)(barémage.vol11 - barémage.vol10) / (55 - 50));
                b = ((double)barémage.vol10 - a * 50);
            }
            else if (level > 55 && level <= 60)
            {
                a = ((double)(barémage.vol12 - barémage.vol11) / (60 - 55));
                b = ((double)barémage.vol11 - a * 55);
            }
            else if (level > 60 && level <= 65)
            {
                a = ((double)(barémage.vol13 - barémage.vol12) / (65 - 60));
                b = ((double)barémage.vol12 - a * 60);
            }
            else if (level > 65 && level <= 70)
            {
                a = ((double)(barémage.vol14 - barémage.vol13) / (70 - 65));
                b = ((double)barémage.vol13 - a * 65);
            }
            else if (level > 70 && level <= 75)
            {
                a = ((double)(barémage.vol15 - barémage.vol14) / (75 - 70));
                b = ((double)barémage.vol14 - a * 70);
            }
            else if (level > 75 && level <= 80)
            {
                a = ((double)(barémage.vol16 - barémage.vol15) / (80 - 75));
                b = ((double)barémage.vol15 - a * 75);
            }
            else if (level > 80 && level <= 85)
            {
                a = ((double)(barémage.vol17 - barémage.vol16) / (85 - 80));
                b = ((double)barémage.vol16 - a * 80);
            }
            else if (level > 85 && level <= 90)
            {
                a = ((double)(barémage.vol18 - barémage.vol17) / (90 - 85));
                b = ((double)barémage.vol17 - a * 85);
            }
            else if (level > 90 && level <= 95)
            {
                a = ((double)(barémage.vol19 - barémage.vol18) / (95 - 90));
                b = ((double)barémage.vol18 - a * 90);
            }
            else if (level > 95 && level <= 100)
            {
                a = ((double)(barémage.vol20 - barémage.vol19) / (100 - 95));
                b = ((double)barémage.vol19 - a * 95);
            }

            double niv = Math.Round(a * level + b);
            int liter = (int)niv;
            return liter;
        }

        public string UpdateSignalForce(string state)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string ret = "";
                try
                {
                    con.Open();
                    string query = "update [dbo].[data] set signalForce=@state where id=1";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@state", state);
                        command.ExecuteNonQuery();
                        ret = "signal updated";
                    }

                }
                catch (Exception e)
                {
                    //ret = "exception occured";
                    ret = e.Message;
                }
                finally
                {
                    con.Close();
                }
                return ret;
            }
        }

        public SeqModel chkSeq(string stationID)
        {
            projetDataContext dc = new projetDataContext();
            SeqModel seq = new SeqModel();
            var query = from alime in dc.alimentations
                        where alime.stationID == int.Parse(stationID) && alime.state == "/images/icons/wait.png                                                                              "
                        && alime.sent == null
                        orderby alime.date descending
                        select alime;

            if (query.Any())
            {
                var alim = query.First();
                seq.id = alim.id;
                seq.date = alim.date.Value.ToShortDateString();
                seq.product = alim.produit.nom;
                seq.qte = Convert.ToInt32(alim.qte);
                seq.site = alim.site.adr;
                seq.station = alim.station.adresse;
                seq.state = alim.state;
                alim.sent = true;
            }
            dc.SubmitChanges();
            return seq;
        }

        public string ConfirmSeq(string seqID, string state)
        {
            string ret = "";
            try
            {
                projetDataContext dc = new projetDataContext();
                Table<alimentation> tb = dc.alimentations;
                var query = from alime in tb
                            where alime.id == int.Parse(seqID)
                            select alime;
                if (query.Any())
                {
                    var alim = query.First();
                    alim.state = "/images/icons/" + state + ".png";
                }
                //dc.alimentations.InsertAllOnSubmit(tb);
                dc.SubmitChanges();
                ret = "wcfservice est executé avec succès !";
            }
            catch (Exception e)
            {
                ret = e.Message;
            }
            return ret;
        }

        public string SetAlarm(int stationID, string message)
        {
            try
            {
                projetDataContext dc = new projetDataContext();

                alarme alarme = new alarme();
                var query = from station in dc.stations where station.id == stationID select station;
                if (query.Any())
                {
                    var stationn = query.First();
                    alarme.text = message + ", " + stationn.adresse;
                    alarme.date = System.DateTime.Now;
                    alarme.stationID = stationID;
                    alarme.state = false;
                    dc.alarmes.InsertOnSubmit(alarme);

                    var query2 = from data in dc.datas select data;
                    foreach (var data in query2)
                    {
                        data.newAlert = true;
                    }
                    dc.SubmitChanges();
                }
                else
                    return "wrong stationID";

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "wcfservice est executé avec succès !";
        }

        public List<SeqModel> chkAllSeq()
        {
            projetDataContext dc = new projetDataContext();

            List<SeqModel> list = new List<SeqModel>();
            var query = from alime in dc.alimentations
                        where alime.state == "/images/icons/wait.png                                                                              "
                        && alime.sent == null
                        orderby alime.date descending
                        select alime;

            if (query.Any())
            {
                foreach (var alim in query)
                {
                    SeqModel seq = new SeqModel();
                    seq.id = alim.id;
                    seq.date = alim.date.Value.ToShortDateString();
                    seq.product = alim.produit.nom;
                    seq.qte = Convert.ToInt32(alim.qte);
                    seq.site = alim.site.adr;
                    seq.station = alim.station.adresse;
                    seq.state = alim.state;
                    seq.stationPhone = alim.station.tel;
                    list.Add(seq);
                }
            }
            return list;
        }
    }
}
