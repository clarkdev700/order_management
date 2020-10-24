using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelVenteRecu
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int ReferenceVente { get; set; }
        //public float Remise { get; set; }
        public float MontantTotal { get; set; }
        public float MontantVerse { get; set; }
        public float ChargeAssurance { get; set; }
        public float ChargeClient { get; set; }
        public string ResteApayerClient { get; set; }
        public float ReductionClient { get; set; }
        public float MontanTotalVerse { get; set; }
        public int NumRecu { get; set; } //id du paiement
        //public List<ligneVente> LigneVente { get; set; }
        public List<HReglement> Reglements { get; set; }

    }

    public struct ligneVente
    {
        public string  Designation { get; set; }
        public int Qte { get; set; }
        public float Prix { get; set; }
        public float Remise { get; set; }
        public float ssTotal { get; set; }
    }
}