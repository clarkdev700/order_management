using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class JournalVenteViewModel
    {
        public string Date { get; set; }
        public DateTime trieDate { get; set; }
        public string RefProd { get; set; }
        public string Designation { get; set; }
        public int? Numero { get; set; }
        public int QteVendu { get; set; }
        //public float Prix { get; set; }
        public float Montant { get; set; }
        public float TotalRem { get; set; }
        public string Marque { get; set; }
    }
}