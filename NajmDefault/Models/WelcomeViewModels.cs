using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NajmDefault.Models
{
    public class CountrieModel
    {
        public string name { get; set; }
        public string flag { get; set; }
        public string map { get; set; }

    }

    public class StationModel
    {
        public int id { get; set; }
        public string pay { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string adresse { get; set; }
        public bool statut { get; set; }
        public bool etat { get; set; }
        public string tel { get; set; }
        public string contact { get; set; }
        public string tnkNbr { get; set; }
        public string connexion { get; set; }

    }

    public class TankModel
    {
        public int id { get; set; }
        public string productID { get; set; }
        public string productName { get; set; }
        public string max { get; set; }
        public string min { get; set; }
        public int stationID { get; set; }
        public int niveau { get; set; }
    }

    public class ResponsibleModel
    {
        public string cin { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
    }

    public class ProductModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SiteModel
    {
        public int id { get; set; }
        public string address { get; set; }
        public string pay { get; set; }
        public string contact { get; set; }
        public List<string> stationList { get; set; }
        public List<string> productList { get; set; }
    }

    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public System.Nullable<double> Y = null;
    }

    public class SeqModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string site { get; set; }
        public string station { get; set; }
        public string responsible { get; set; }
        public string product { get; set; }
        public string state { get; set; }
        public int qte { get; set; }
    }

    public partial class Alarme
    {
        public int stationID { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public string countrie { get; set; }
    }
}