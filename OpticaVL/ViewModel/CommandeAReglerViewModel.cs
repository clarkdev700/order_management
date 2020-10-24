using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class CommandeAReglerViewModel
    {
        public int Id { get; set; }
        public int? AssurId { get; set; }
        public string RefCommande { get; set; }
        public float Montant { get; set; }
        //public float MontantClient { get; set; }
        //public float ResteApayerAssurance { get; set; }
        public float ResteAPayer { get; set; }
        public string Date { get; set; }
        public string ClientIdentite { get; set; }
        public string AssuranceNom { get; set; }
        public string Origine { get; set; }
        public DateTime DateOp { get; set; }
        public int indexKey { get; set; }
    }
}