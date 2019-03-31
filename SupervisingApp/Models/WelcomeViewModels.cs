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
        public float niveau { get; set; }
        public string state { get; set; }
        public int liter;
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
        public string stationPhone { get; set; }
    }

    public partial class Alarme
    {
        public int id { get; set; }
        public int stationID { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public string countrie { get; set; }
        public bool state { get; set; }
    }

    public class barémageModel
    {

        public string resID;
        public string vol1;
        public string vol2;
        public string vol3;
        public string vol4;
        public string vol5;
        public string vol6;
        public string vol7;
        public string vol8;
        public string vol9;
        public string vol10;
        public string vol11;
        public string vol12;
        public string vol13;
        public string vol14;
        public string vol15;
        public string vol16;
        public string vol17;
        public string vol18;
        public string vol19;
        public string vol20;

    }
}