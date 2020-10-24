using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelCommandeALivrer
    {
        public string Date { get; set; }
        public string Ref { get; set; }
        public int Id { get; set; }
        public string Identite { get; set; }
        public float ChargeClient { get; set; }
        public float ChargeAssurance { get; set; }
        public bool StatutConception { get; set; }
        public bool StatutLivraison { get; set; }
    }
}