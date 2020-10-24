using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class RecuViewModel
    {
        public string Ref { get; set; }
        public string NumeroRecu { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public float MttAssur { get; set; }
        public float MttClient { get; set; }
        public float MttVerse { get; set; }
        public float MttResteApayer { get; set; }
        public List<LigneBonDeCommande> LigneFacture { get; set; }
        public List<HReglement> HReglements { get; set; }
    }

    public class HReglement
    {
        public string Date { get; set; }
        public float Montant { get; set; }
        public string Mregl { get; set; }
    }

    public class LFacture
    {

    }
}