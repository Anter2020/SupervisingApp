using NajmDefault.LinqDataBase;
using NajmDefault.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Dynamic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace NajmDefault.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        private SerialPort _serialPort;

        //action return Home view
        public ActionResult Home()
        {
            return View();
        }

        //method to search a countrie in database
        private bool SearchCountrie(string countrieName)
        {
            projetDataContext dc = new projetDataContext();
            var query = from countrie in dc.pays where countrie.name == countrieName select countrie;
            if (query.Any())
                return true;
            else
                return false;
        }

        //action to check countrie existance before add
        [HttpPost]
        public JsonResult VerifCountrie(string countrieName)
        {
            bool verif = false;
            verif = SearchCountrie(countrieName);
            return Json(verif, JsonRequestBehavior.AllowGet);
        }

        //method to delete a countrie
        public void deleteCountrie(string countrieName)
        {
            projetDataContext dc = new projetDataContext();
            Table<pay> tb = dc.GetTable<pay>();
            var deleteCountrie =
            from countrie in tb
            where countrie.name == countrieName
            select countrie;

            foreach (var countrie in deleteCountrie)
            {
                tb.DeleteOnSubmit(countrie);
            }
            dc.SubmitChanges();

        }

        public ActionResult addDeleteCountrie()
        { return View(LoadCountries()); }

        [Authorize(Roles = "Admin")]
        //action to add countrie
        [HttpPost]
        public ActionResult addCountrie(string inputCountrie)
        {
            //to delete spaces from string
            inputCountrie = inputCountrie.Replace(" ", string.Empty);

           
                //if (SearchCountrie(inputCountrie) == false)
                //{
                    projetDataContext dc = new projetDataContext();
                    pay tb = new pay();
                    tb.name = inputCountrie;
                    tb.flag = "/images/pays/flags/" + inputCountrie + ".png";

                    dc.pays.InsertOnSubmit(tb);
                    dc.SubmitChanges();
                //}
                //else
                //    MessageBox.Show(new Form { TopMost = true }, "Countrie " + inputCountrie + " already exist ! ", "Add information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            
            //refreshCountries();

            return Redirect(Request.UrlReferrer.PathAndQuery);
            //return null;

        }

        [Authorize(Roles = "Admin")]
        //action to delete countrie
        [HttpPost]
        public ActionResult DeleteCountrie(string inputCountrie)
        {
            //to delete spaces from string
            inputCountrie = inputCountrie.Replace(" ", string.Empty);

            deleteCountrie(inputCountrie);

            //refreshCountries();
            return Redirect(Request.UrlReferrer.PathAndQuery);
            //return null;
        }

        //method that return a list of existing countries
        private List<CountrieModel> LoadCountries()
        {
            projetDataContext dc = new projetDataContext();
            Table<pay> tb = dc.GetTable<pay>();
            var query = from countrie in tb select countrie;

            List<CountrieModel> myCountries = new List<CountrieModel>();
            foreach (var countrie in query)
            {
                CountrieModel msvc = new CountrieModel();
                msvc.flag = countrie.flag;
                msvc.name = countrie.name;
                myCountries.Add(msvc);
            }
            return myCountries;
        }

        //action to show existing countries
        public ActionResult refreshCountries()
        {
            return View(LoadCountries());

        }

        //method to get specific countrie
        public CountrieModel GetCountrie(string countrieName)
        {
            CountrieModel model = new CountrieModel();
            model.flag = "/images/pays/flags/" + countrieName + ".png";
            model.name = countrieName;

            projetDataContext dc = new projetDataContext();
            Table<pay> tb = dc.GetTable<pay>();
            var query =
            from countrie in tb
            where countrie.name == countrieName
            select countrie;

            foreach (var countrie in query)
            {
                model.map = countrie.map;
            }
            return (model);

        }

        //method to get all countries
        public List<CountrieModel> GetCountries()
        {
            List<CountrieModel> modelList = new List<CountrieModel>();

            projetDataContext dc = new projetDataContext();
            Table<pay> tb = dc.GetTable<pay>();
            var query = from countrie in tb select countrie;

            foreach (var countrie in query)
            {
                CountrieModel model = new CountrieModel();
                model.map = countrie.map;
                model.flag = countrie.flag;
                model.name = countrie.name;

                modelList.Add(model);
            }
            return (modelList);

        }

        //methode pour retourner une liste des models station (d'un pay)
        public List<StationModel> GetStations(string countrieName)
        {
            List<StationModel> modelList = new List<StationModel>();
            projetDataContext dc = new projetDataContext();
            Table<station> tb = dc.GetTable<station>();
            var query =
            from station in tb
            where station.pay == countrieName
            select station;

            foreach (var station in query)
            {
                string ch1 = station.adresse;
                string ch2 = station.tel.Replace(" ", string.Empty);
                string ch3 = station.responsable.cin.Replace(" ", string.Empty);
                modelList.Add(new StationModel
                {
                    id = station.id,
                    x = station.x,
                    y = station.y,
                    adresse = ch1,
                    tel = ch2,
                    contact = ch3
                });
            }
            return (modelList);

        }

        //methode pour retourner une liste des models station (tous)
        public List<StationModel> GetStations()
        {
            List<StationModel> modelList = new List<StationModel>();
            projetDataContext dc = new projetDataContext();
            Table<station> tb = dc.GetTable<station>();
            var query = from station in tb orderby station.adresse select station;

            foreach (var station in query)
            {
                string ch1 = station.adresse;
                    //.Replace(" ", string.Empty);
                string ch2 = station.tel.Replace(" ", string.Empty);
                string ch3 = station.responsable.cin.Replace(" ", string.Empty);
                modelList.Add(new StationModel
                {
                    id = station.id,
                    x = station.x,
                    y = station.y,
                    adresse = ch1,
                    tel = ch2,
                    contact = ch3
                });
            }
            return (modelList);

        }

        //methode pour retourner une liste des models produit
        public List<ProductModel> GetProducts()
        {
            List<ProductModel> modelList = new List<ProductModel>();
            projetDataContext dc = new projetDataContext();
            Table<produit> tb = dc.GetTable<produit>();
            var query = from produit in tb orderby produit.nom select produit;
            foreach (var prod in query)
            {
                string ch1 = prod.nom;
                    //.Replace(" ", string.Empty);
                string ch2 = prod.id.ToString();
                modelList.Add(new ProductModel
                {
                    name = ch1,
                    id = ch2
                });
            }
            return (modelList);
        }

        //methode pour retourner une liste des models site d'un pay
        public List<SiteModel> GetSites(string countrieName)
        {
            List<SiteModel> modelList = new List<SiteModel>();
            projetDataContext dc = new projetDataContext();
            Table<site> tb = dc.GetTable<site>();
            Table<SiteProduit> tb2 = dc.GetTable<SiteProduit>();
            Table<SiteStation> tb3 = dc.GetTable<SiteStation>();

            var query = from site in tb where site.pay == countrieName orderby site.adr select site;

            foreach (var site in query)
            {
                int i = 0;
                var query2 = from SiteProduit in tb2 where (SiteProduit.site == site) select SiteProduit;
                var query3 = from SiteStation in tb3 where (SiteStation.site == site) select SiteStation;

                SiteModel model = new SiteModel();

                model.id = site.id;
                model.address = site.adr;
                model.contact = site.contact;
                List<string> list1 = new List<string>();
                foreach (var SiteStation in query3)
                {
                    string ch = "ID: " + SiteStation.station.id + ", " + SiteStation.station.adresse;
                        //.Replace(" ", string.Empty);
                    list1.Add(ch);
                }

                model.stationList = list1;
                List<string> list2 = new List<string>();

                i = 0;

                foreach (var SiteProduit in query2)
                {
                    list2.Add("ID: " + SiteProduit.produit.id + ", " + SiteProduit.produit.nom);
                    i++;
                }
                model.productList = list2;
                modelList.Add(model);
            }
            return (modelList);
        }

        //methode pour retourner une liste des models de tous les sites
        public List<SiteModel> GetSites()
        {
            List<SiteModel> modelList = new List<SiteModel>();
            projetDataContext dc = new projetDataContext();
            Table<site> tb = dc.GetTable<site>();
            Table<SiteProduit> tb2 = dc.GetTable<SiteProduit>();
            Table<SiteStation> tb3 = dc.GetTable<SiteStation>();

            var query = from site in tb orderby site.adr select site;

            foreach (var site in query)
            {
                int i = 0;
                var query2 = from SiteProduit in tb2 where (SiteProduit.site == site) select SiteProduit;
                var query3 = from SiteStation in tb3 where (SiteStation.site == site) select SiteStation;

                SiteModel model = new SiteModel();

                model.id = site.id;
                model.address = site.adr;
                model.contact = site.contact;
                List<string> list1 = new List<string>();
                foreach (var SiteStation in query3)
                {
                    string ch = "ID: " + SiteStation.station.id + ", " + SiteStation.station.adresse;
                        //.Replace(" ", string.Empty);
                    list1.Add(ch);
                }

                model.stationList = list1;
                List<string> list2 = new List<string>();

                i = 0;

                foreach (var SiteProduit in query2)
                {
                    list2.Add("ID: " + SiteProduit.produit.id + ", " + SiteProduit.produit.nom);
                    i++;
                }
                model.productList = list2;
                modelList.Add(model);
            }
            return (modelList);
        }

        //methode pour retourner une liste des models (remplissages ou alimentations)
        public List<SeqModel> GetSeq(string type, string countrieName)
        {
            projetDataContext dc = new projetDataContext();
            List<SeqModel> sequences = new List<SeqModel>();
            if (type == "fill")
            {
                var query = from remp in dc.remplissages where remp.site.pay == countrieName orderby remp.date descending select remp;
                foreach (var remp in query)
                {
                    sequences.Add(new SeqModel()
                    {
                        id = remp.id,
                        date = remp.date.Value.ToShortDateString(),
                        product = remp.produit.nom,
                        qte = Convert.ToInt32(remp.qte),
                        site = remp.site.adr,
                        responsible = remp.responsable
                    });
                }
            }
            else if (type == "supply")
            {
                var query = from alim in dc.alimentations where alim.site.pay == countrieName orderby alim.date descending select alim;
                foreach (var alim in query)
                {
                    sequences.Add(new SeqModel()
                    {
                        id = alim.id,
                        date = alim.date.Value.ToShortDateString(),
                        product = alim.produit.nom,
                        qte = Convert.ToInt32(alim.qte),
                        site = alim.site.adr,
                        station = alim.station.adresse ,
                        state = alim.state
                    });
                }
            }
            return sequences;
        }

        //action qui retourne un model dynamique(list of stations models ,countrie model)
        [HttpGet]
        public ActionResult countrieDetails(string countrieName)
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.countrie = GetCountrie(countrieName);
            mymodel.stations = GetStations(countrieName);
            mymodel.products = GetProducts();
            mymodel.sites = GetSites(countrieName);
            mymodel.alimentations = GetSeq("supply", countrieName);
            mymodel.remplissages = GetSeq("fill", countrieName);
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        //action qui retourne un model dynamique(list of stations models ,countrie model) et changer la map
        [HttpPost]
        public ActionResult countrieDetails(HttpPostedFileBase postedFile, string countrieName)
        {
            dynamic mymodel = new ExpandoObject();

            CountrieModel model = new CountrieModel();

            if (postedFile != null)
            {
                string path = Server.MapPath("/images/pays/maps/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = postedFile.FileName.Replace(" ", "_");
                postedFile.SaveAs(path + Path.GetFileName(filename));

                projetDataContext dc = new projetDataContext();
                Table<pay> tb = dc.GetTable<pay>();
                var updateCountrie =
                from countrie in tb
                where countrie.name == countrieName
                select countrie;

                foreach (var countrie in updateCountrie)
                {
                    countrie.map = "/images/pays/maps/" + filename;
                }
                dc.SubmitChanges();
                model.map = "/images/pays/maps/" + filename;
                model.name = countrieName;
                model.flag = "/images/pays/flags/" + countrieName + ".png";
            }
            else model = GetCountrie(countrieName);
            mymodel.countrie = model;
            mymodel.stations = GetStations(countrieName);
            mymodel.products = GetProducts();
            mymodel.sites = GetSites(countrieName);
            mymodel.alimentations = GetSeq("supply", countrieName);
            mymodel.remplissages = GetSeq("fill", countrieName);
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        //method to delete stations from database
        [HttpPost]
        public void deleteStation(string pay, string address)
        {
            projetDataContext dc = new projetDataContext();
            Table<station> tb = dc.GetTable<station>();
            var query = from station in tb where (station.pay == pay && station.adresse == address) select station;

            //if (query.Any())
            //{
            //    DialogResult result = MessageBox.Show(new Form { TopMost = true }, "Deleting this station (" + address + ") will remove all its previous contents !",
            //    "Critical Warning",
            //    MessageBoxButtons.OKCancel,
            //    MessageBoxIcon.Warning);
            //    if (result == DialogResult.OK)
            //    {
                    foreach (var station in query)
                    {
                        ////delete tanks mouvements before tank
                        //Table<mouvReservoir> tb3 = dc.GetTable<mouvReservoir>();
                        //var query3 = from mouvReservoir in tb3 where (mouvReservoir.reservoir.station == station) select mouvReservoir;
                        //foreach (var mouvReservoir in query3)
                        //{
                        //    tb3.DeleteOnSubmit(mouvReservoir);
                        //}
                        ////delete tanks before station
                        //Table<reservoir> tb2 = dc.GetTable<reservoir>();
                        //var query2 = from reservoir in tb2 where (reservoir.station == station) select reservoir;
                        //foreach (var reservoir in query2)
                        //{
                        //    tb2.DeleteOnSubmit(reservoir);
                        //}
                
                        //delete station
                        tb.DeleteOnSubmit(station);
                     }
                     dc.SubmitChanges();
            //    }
            //}
        }

        //method that return a string between 2 existing string
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        [Authorize(Roles = "Admin")]
        // action to add station,responsible,tanks to database when clic on save
        [HttpPost]
        public void AddStation(string x, string y, string note, string pay, string address, string resTel, string mail, string selec, string num, List<TankModel> tanks)
        {
            if (num == "" || int.Parse(num) < 0) num = "0";
            string adr = getBetween(note, "Address: [", "]" + " Phone: [");
            if (address == adr)
            {
                projetDataContext dc = new projetDataContext();
                Table<station> tb1 = dc.GetTable<station>();
                Table<responsable> tb2 = dc.GetTable<responsable>();

                //searching for existing station address
                //var query =
                //    from station in tb1
                //    where (station.x == x && station.y == y)
                //    select station;

                //if (!query.Any())
                //{
                    station tb3 = new station();
                    responsable tb5 = new responsable();
                    //searching if responsible CIN already exist
                    var CIN = getBetween(note, "]" + " RespensibleID: [", "]");
                    var query2 =
                    from responsable in tb2
                    where (responsable.cin == CIN)
                    select responsable;
                    if (!query2.Any())
                    {
                        //add responsible to database
                        tb5.cin = CIN;
                        tb5.email = mail;
                        tb5.tel = resTel;
                        //add station to database
                        tb3.x = x;
                        tb3.y = y;
                        tb3.adresse = adr;
                        tb3.tel = getBetween(note, "]" + " Phone: [", "]" + " RespensibleID: [");
                        tb3.pay = pay;
                        tb3.tnkNbr = num;
                        tb3.connexion = selec;
                        // assigner la(les) cle primaire de la table responsable a la(les) cle etrangere de la table station
                        tb3.responsable = tb5;
                        dc.responsables.InsertOnSubmit(tb5);
                        dc.stations.InsertOnSubmit(tb3);
                    }
                    else
                    {
                        /*DialogResult result = MessageBox.Show(new Form { TopMost = true }, "This reponsible already exist. His informations can be modified only in station interface",
                        "Critical Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);*/

                        //add station to database
                        tb3.x = x;
                        tb3.y = y;
                        tb3.adresse = adr;
                        tb3.tel = getBetween(note, "]" + " Phone: [", "]" + " RespensibleID: [");
                        tb3.pay = pay;
                        tb3.tnkNbr = num;
                        tb3.connexion = selec;
                        foreach (responsable resp in query2)
                        {
                            // assigner la(les) cle primaire de la table responsable (existante) a la(les) cle etrangere de la table station
                            tb3.responsable = resp;
                        }
                        dc.stations.InsertOnSubmit(tb3);
                    }

                    //add tanks to database
                    if (tanks != null)
                    {
                        foreach (var tank in tanks)
                        {
                            reservoir tb4 = new reservoir();
                            tb4.max = tank.max;
                            tb4.min = tank.min;
                            tb4.productID = int.Parse(getBetween(tank.productID, "ID: ", ","));
                            // assigner la(les) cle primaire de la table station (existante) a la(les) cle etrangere de la table reservoire
                            tb4.station = tb3;
                            dc.reservoirs.InsertOnSubmit(tb4);
                        }
                    }
                    dc.SubmitChanges();
                //}
                //else
                //{
                //    DialogResult result = MessageBox.Show(new Form { TopMost = true }, "Updating this station will remove all its previous informations.",
                //    "Critical Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                //    if (result == DialogResult.OK)
                //    {
                //        foreach (var station in query)
                //        {
                //            tb1.DeleteOnSubmit(station);
                //        }
                //        dc.SubmitChanges();
                //        station tb = new station();
                //        tb.x = x;
                //        tb.y = y;
                //        tb.adresse = adr;
                //        tb.tel = getBetween(note, "]" + " Telephone: [", "]" + " RespensibleID: [");
                //        tb.responsable.tel = getBetween(note, "]" + " RespensibleID: [", "]");
                //        tb.pay = pay;
                //        dc.stations.InsertOnSubmit(tb);
                //        dc.SubmitChanges();
                //    }
                //};
            }
        }

        //model refer to VerifAddress action
        public class verif
        {
            public string exist { get; set; }
            public string pay { get; set; }
            public string address { get; set; }
            public bool confirm { get; set; }
        }

        //action to check address existance
        [HttpPost]
        public JsonResult VerifAddress(verif verif)
        {
            string address = null;
            if (verif.address != null) address = verif.address;
                    //.Replace(" ", string.Empty);

            if (address == null || address == "")
            {
                verif.exist = "empty";
                return Json(verif, JsonRequestBehavior.AllowGet);
            }

            for (var i = address.Length; i < 100; i++)
            {
                address = address + " ";
            }

            projetDataContext dc = new projetDataContext();
            Table<station> tb = dc.GetTable<station>();
            var query = from station in tb where station.pay == verif.pay select station;

            foreach (var station in query)
            {
                if (station.adresse == address)
                {
                    verif.exist = "true";

                    //DialogResult result = MessageBox.Show(new Form { TopMost = true }, "Address already exist continue ?",
                    //"Critical Warning",
                    //MessageBoxButtons.OKCancel,
                    //MessageBoxIcon.Warning);

                    //if (result == DialogResult.OK)
                    //    verif.confirm = true;

                    return Json(verif, JsonRequestBehavior.AllowGet);
                }
            }
            verif.exist = "false";
            return Json(verif, JsonRequestBehavior.AllowGet);
        }

        //action to verify responsible
        [HttpPost]
        public JsonResult VerifResponsible(string verifResp)
        {
            //int id = int.Parse(verifResp);
            projetDataContext dc = new projetDataContext();
            var query = from resp in dc.responsables where resp.cin == verifResp select resp;
            if (query.Any())
                verifResp = "true";
            else verifResp = "false";
            return Json(verifResp, JsonRequestBehavior.AllowGet);
        }

        //action to return details of selected station
        [HttpGet]
        public ActionResult stationDetails(string stationID)
        {
            dynamic mymodel = new ExpandoObject();
            StationModel model = new StationModel();
            ResponsibleModel model2 = new ResponsibleModel();
            List<TankModel> model3 = new List<TankModel>();
            projetDataContext dc = new projetDataContext();
            Table<station> tb = dc.GetTable<station>();
            Table<reservoir> tb2 = dc.GetTable<reservoir>();

            var query = from station in tb where station.id.ToString() == stationID select station;
            foreach (var station in query)
            {
                model.id = station.id;
                model.pay = station.pay;
                model.adresse = station.adresse;
                model.tel = station.tel;
                model.tnkNbr = station.tnkNbr;
                model.connexion = station.connexion;

                model2.tel = station.responsable.tel;
                model2.email = station.responsable.email;
                model2.cin = station.responsable.cin;

                var query2 = from reservoir in tb2 where reservoir.station == station select reservoir;
                foreach (var reservoir in query2)
                {
                    TankModel tnk = new TankModel();
                    tnk.id = reservoir.id;
                    tnk.max = reservoir.max;
                    tnk.min = reservoir.min;
                    tnk.productID = reservoir.produit.id.ToString();
                    tnk.productName = reservoir.produit.nom;
                    if (reservoir.niveau != null)
                    {
                        tnk.niveau = reservoir.niveau.Value;
                    }
                    model3.Add(tnk);
                }
                if (station.etat != null) model.etat = (bool)station.etat;
                if (station.statut != null) model.statut = (bool)station.statut;

            }

            mymodel.station = model;
            mymodel.responsible = model2;
            mymodel.tanks = model3;
            mymodel.products = GetProducts();
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        //action add a site in database
        [HttpGet]
        public ActionResult addSite(string adrsite, string contsite, string[] stationsSelect, string[] productsSelect, string countriename)
        {
            projetDataContext dc = new projetDataContext();
            site tb = new site();

            tb.adr = adrsite;
            tb.contact = contsite;
            tb.pay = countriename;
            dc.sites.InsertOnSubmit(tb);

            foreach (var station in stationsSelect)
            {
                SiteStation tb2 = new SiteStation();
                string id = getBetween(station, "ID: ", ",");
                tb2.site = tb;
                tb2.stationID = int.Parse(id);
                dc.SiteStations.InsertOnSubmit(tb2);
            }

            foreach (var produit in productsSelect)
            {
                SiteProduit tb3 = new SiteProduit();
                string id = getBetween(produit, "ID: ", ",");
                tb3.site = tb;
                tb3.produitID = int.Parse(id);
                dc.SiteProduits.InsertOnSubmit(tb3);
            }

            dc.SubmitChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action delete a site from database
        [HttpGet]
        public ActionResult deletesite(string sitetodelete)
        {
            string id = getBetween(sitetodelete, "ID: ", ",");
            projetDataContext dc = new projetDataContext();
            Table<SiteStation> tb = dc.GetTable<SiteStation>();
            Table<site> tb2 = dc.GetTable<site>();
            Table<SiteProduit> tb3 = dc.GetTable<SiteProduit>();
            var query = from SiteStation in tb where SiteStation.siteID.ToString() == id select SiteStation;
            var query2 = from SiteProduit in tb3 where SiteProduit.siteID.ToString() == id select SiteProduit;
            bool deleted = false;
            foreach (var SiteStation in query)
            {
                tb.DeleteOnSubmit(SiteStation);
            }
            foreach (var SiteProduit in query2)
            {
                tb3.DeleteOnSubmit(SiteProduit);
                if (deleted == false)
                {
                    tb2.DeleteOnSubmit(SiteProduit.site);
                    deleted = true;
                }
            }
            dc.SubmitChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action add a product in database
        [HttpGet]
        public ActionResult addProduct(string producttoadd)
        {
            projetDataContext dc = new projetDataContext();
            produit tb = new produit();
            tb.nom = producttoadd;
            dc.produits.InsertOnSubmit(tb);
            dc.SubmitChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action delete a product from database
        [HttpGet]
        public ActionResult deleteProduct(string producttodelete)
        {
            string id = getBetween(producttodelete, "ID: ", ",");
            projetDataContext dc = new projetDataContext();
            Table<produit> tb = dc.GetTable<produit>();
            var query = from product in tb where product.id.ToString() == id select product;
            foreach (var product in query)
            {
                tb.DeleteOnSubmit(product);
            }
            dc.SubmitChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action to delete tank from database
        [HttpGet]
        public ActionResult deleteTank(string tankToDelete)
        {
            int nbr;
            tankToDelete = tankToDelete + ".";
            projetDataContext dc = new projetDataContext();
            Table<reservoir> tb = dc.GetTable<reservoir>();
            var query = from tank in tb where tank.id.ToString() == getBetween(tankToDelete, "TankID: ", ".") select tank;
            foreach (var tank in query)
            {
                nbr = int.Parse(tank.station.tnkNbr);
                nbr--;
                tank.station.tnkNbr = nbr.ToString();
                tb.DeleteOnSubmit(tank);
            }
            dc.SubmitChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action to add tank to database
        [HttpGet]
        public ActionResult addTank(string min, string max, string productList, string stationID)
        {
            string id = getBetween(productList, "ID: ", ",");
            productList = productList + ".";
            projetDataContext dc = new projetDataContext();
            reservoir tb = new reservoir();
            tb.min = min;
            tb.max = max;
            tb.productID = int.Parse(id);
            tb.stationID = int.Parse(stationID);
            dc.reservoirs.InsertOnSubmit(tb);
            dc.SubmitChanges();
            int nbr = int.Parse(tb.station.tnkNbr);
            nbr++;
            tb.station.tnkNbr = nbr.ToString();
            dc.SubmitChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        //action that return history view
        [HttpGet]
        public ActionResult history(string selectedProduct, string selectedCountrie, string selectedSite, string selectedStation, string startDate, string finDate, string searchPer)
        {
            bool exist = false;

            projetDataContext dc = new projetDataContext();
            Table<alimentation> tb = dc.GetTable<alimentation>();
            List<DataPoint> dataPoints = new List<DataPoint>();

            //search per day
            if (searchPer == "Day")
            {
                if (selectedStation == "All" && selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { day = alimentation.date }
                                into g
                                select new
                                { date = g.Key.day, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;
                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.Value.ToShortDateString(), (double)alimentation.qte_total));
                    }
                }
                else if (selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { day = alimentation.date }
                                into g
                                select new
                                { date = g.Key.day, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
                else if (selectedStation == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { day = alimentation.date }
                                into g
                                select new
                                { date = g.Key.day, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
                //this case if I select a site and a station
                else
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { day = alimentation.date }
                                into g
                                select new
                                { date = g.Key.day, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
            }

            //search per month
            else if (searchPer == "Month")
            {
                if (selectedStation == "All" && selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                group alimentation by new { month = alimentation.date.Value.ToString().Substring(0, 7) }
                                into g
                                orderby g.Key.month
                                select new
                                { date = g.Key.month, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;
                   
                    foreach (var alimentation in query)
                    {
                        var length = alimentation.date.Length;
                        string year = alimentation.date.Substring(0, 4);
                        string month = alimentation.date.Substring(5, 2);

                        dataPoints.Add(new DataPoint(month+"/"+year, (double)alimentation.qte_total));
                    }
                }
                else if (selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { month = alimentation.date.Value.ToString().Substring(0, 7) }
                                into g
                                select new
                                { date = g.Key.month, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        var length = alimentation.date.Length;
                        string year = alimentation.date.Substring(0, 4);
                        string month = alimentation.date.Substring(5, 2);

                        dataPoints.Add(new DataPoint(month + "/" + year, (double)alimentation.qte_total));
                    }
                }
                else if (selectedStation == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { month = alimentation.date.Value.ToString().Substring(0, 7) }
                                into g
                                select new
                                { date = g.Key.month, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        var length = alimentation.date.Length;
                        string year = alimentation.date.Substring(0, 4);
                        string month = alimentation.date.Substring(5, 2);

                        dataPoints.Add(new DataPoint(month + "/" + year, (double)alimentation.qte_total));
                    }
                }
                //this case if I select a site and a station
                else
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { month = alimentation.date.Value.ToString().Substring(0, 7) }
                                into g
                                select new
                                { date = g.Key.month, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        var length = alimentation.date.Length;
                        string year = alimentation.date.Substring(0, 4);
                        string month = alimentation.date.Substring(5, 2);

                        dataPoints.Add(new DataPoint(month + "/" + year, (double)alimentation.qte_total));
                    }
                }
            }

            //search per year
            else if (searchPer == "Year")
            {

                if (selectedStation == "All" && selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { Year = alimentation.date.Value.Year }
                                into g
                                select new
                                { date = g.Key.Year, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
                else if (selectedSite == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { Year = alimentation.date.Value.Year }
                                into g
                                select new
                                { date = g.Key.Year, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
                else if (selectedStation == "All")
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { Year = alimentation.date.Value.Year }
                                into g
                                select new
                                { date = g.Key.Year, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
                //this case if I select a site and a station
                else
                {
                    var query = from alimentation in dc.alimentations
                                where
                                  alimentation.produitID == int.Parse(selectedProduct) &&
                                  alimentation.siteID == int.Parse(selectedSite) &&
                                  alimentation.stationID == int.Parse(selectedStation) &&
                                  alimentation.station.pay == selectedCountrie &&
                                  alimentation.date >= DateTime.Parse(startDate) &&
                                  alimentation.date <= DateTime.Parse(finDate)
                                orderby alimentation.date
                                group alimentation by new { Year = alimentation.date.Value.Year }
                                into g
                                select new
                                { date = g.Key.Year, qte_total = g.Sum(p => p.qte) };
                    if (query.Any()) exist = true;

                    foreach (var alimentation in query)
                    {
                        dataPoints.Add(new DataPoint(alimentation.date.ToString(), (double)alimentation.qte_total));
                    }
                }
            }

            if (selectedProduct == null)
                ViewBag.DataPoints = null;
            else if (exist == false)
                ViewBag.DataPoints = "nodata";
            else
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            dynamic mymodel = new ExpandoObject();
            mymodel.countries = GetCountries();
            mymodel.stations = GetStations();
            mymodel.products = GetProducts();
            mymodel.sites = GetSites();
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        //action to add fillSequence
        [HttpGet]
        public ActionResult addFillSeq(string Sitetofill, string productToFill, string qteToFill)
        {            
            if (qteToFill == "") qteToFill = "0";
            projetDataContext dc = new projetDataContext();
            remplissage tb = new remplissage();
            tb.siteID = int.Parse(Sitetofill);
            tb.productID = int.Parse(productToFill);
            tb.qte = int.Parse(qteToFill);
            tb.date = DateTime.Now;
            dc.remplissages.InsertOnSubmit(tb);
            dc.SubmitChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action to delete fillSequence
        [HttpGet]
        public ActionResult deleteFillSeq(string seqToDelete)
        {
            projetDataContext dc = new projetDataContext();
            var query = from fillSeq in dc.remplissages where fillSeq.id == int.Parse(seqToDelete) select fillSeq;

            foreach (var fillSeq in query)
            {
                dc.remplissages.DeleteOnSubmit(fillSeq);
            }

            dc.SubmitChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        //model refer to supplySequence action
        public class supplySequencedata
        {
            public string fromSite { get; set; }
            public string toStation { get; set; }
            public string qteToFill { get; set; }
            public string productToFill { get; set; }
            public string error { get; set; }
        }

        [Authorize(Roles = "Admin")]
        //action to add supplySequence
        //post method is obligatory to receive json data
        [HttpPost]
        public JsonResult addSupplySeq(supplySequencedata supplySequencedata)
        //public JsonResult addSupplySeq(string fromSite ,
        //string toStation ,
        //string qteToFill ,
        //string productToFill)
        {
            string fromSite = supplySequencedata.fromSite;
            string toStation = supplySequencedata.toStation;
            string qteToFill = supplySequencedata.qteToFill;
            string productToFill = supplySequencedata.productToFill;

            if (qteToFill == "") qteToFill = "0";
            projetDataContext dc = new projetDataContext();

            string number = "", message, prodName = "", stationAdr = "";
            var query = from station in dc.stations where station.id == int.Parse(toStation) select station;
            foreach (var station in query)
            {
                number = station.tel;
                stationAdr = station.adresse.Replace(" ",string.Empty);
            }
            var query2 = from produit in dc.produits where produit.id == int.Parse(productToFill) select produit;
            foreach (var produit in query2)
            {
                prodName = produit.nom;
            }
            message = "seq,"+prodName+","+ qteToFill;
            number = number.Replace(" ",string.Empty);
            try { 
                //envoie SMS
                _serialPort = new SerialPort("COM6", 9600);
                Thread.Sleep(1000);
                _serialPort.Open();
                Thread.Sleep(1000);
                _serialPort.Write("AT+CMGF=1\r");
                Thread.Sleep(1000);
                _serialPort.Write("AT+CMGS=\"" + number + "\"\r\n");
                Thread.Sleep(1000);
                _serialPort.Write(message + "\x1A");
                Thread.Sleep(1000);
                _serialPort.Close();

                //archivage de sequence
                alimentation tb = new alimentation();
                tb.siteID = int.Parse(fromSite);
                tb.stationID = int.Parse(toStation);
                tb.produitID = int.Parse(productToFill);
                tb.qte = int.Parse(qteToFill);
                tb.date = DateTime.Now;
                dc.alimentations.InsertOnSubmit(tb);
                dc.SubmitChanges();
            }
            catch(Exception e)
            {
                supplySequencedata.error = "SMS cannot be sent to station ("+ stationAdr+" ),error ("+ e.Message+")";
                //MessageBox.Show("SMS cannot be sent, exception ("+e.Message+")");
                return Json(supplySequencedata, JsonRequestBehavior.AllowGet);
            }

            supplySequencedata.error = "Confirmation message was sent to station ("+stationAdr+") succecfully !";
            return Json(supplySequencedata, JsonRequestBehavior.AllowGet);

            //return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [Authorize(Roles = "Admin")]
        //action to delete supplySequence
        [HttpGet]
        public ActionResult deleteSupplySeq(string seqToDelete)
        {
            projetDataContext dc = new projetDataContext();
            var query = from supplySeq in dc.alimentations where supplySeq.id == int.Parse(seqToDelete) select supplySeq;

            foreach (var supplySeq in query)
            {
                dc.alimentations.DeleteOnSubmit(supplySeq);
            }

            dc.SubmitChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        //notification bar action
        public JsonResult GetNotificationContacts()
        {
            //var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            //var list = NC.GetContacts(notificationRegisterTime);
            var list = NC.GetAlerts();
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //action return contact view
        public ActionResult contact()
        {
            return View();
        }
        
        //action return about view
        public ActionResult about()
        {
            return View();
        }

        //action return alerts view
        public ActionResult alerts()
        {
            NotificationComponent NC = new NotificationComponent();
            dynamic mymodel = new ExpandoObject();
            mymodel.alerts = NC.GetAllAlerts();
            return View(mymodel);
        }
    }
}