using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class BonDeCommandeModel
    {
        public string Reference { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Contact { get; set; }
        public float MttAssur { get; set; }
        public float MttClient { get; set; }
        public float ReductionClient { get; set; }
        public float MttVerse { get; set; }
        public float ResteApayer { get; set; }
        public float MontantTotal { get; set; }
        public List<LigneBonDeCommande> LigneBonDeCommandes { get; set; }
    }

    public class LigneBonDeCommande
    {
        public string Designation { get; set; }
        public int Qte { get; set; }
        public float Prix { get; set; }
        public float Rem { get; set; } // somme Rem et RemDg
        public float Montant { get; set; }
    }
}