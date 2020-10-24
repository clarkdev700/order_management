using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class QReglementModel
    {
        public DateTime? Deb { get; set; }
        public DateTime? End { get; set; }
        public string Qtype { get; set; }
    }

    public class ValidationCaisseModel
    {
        public int IdPayement { get; set; } // id de la table paiement
        public string Date { get; set; }
        public string RefFacture { get; set; } // chaine generer à partir du ref commande ou vente
        public string RefRecuCaisse { get; set; }
        public float MontantVerse { get; set; }
        public string Source { get; set; } // valeur possible client == null ou assurance
        public string Type { get; set; } // valeur possible vente ou commande
        public int? IdFacture { get; set; } // id de commande ou vente
        public bool Valider { get; set; } //statut montant valider ou non 
        public string Identite { get; set; }
    }

    public class RUP
    {
        public int Id { get; set; }
        public bool Valider { get; set; }
    }

    struct QtypeValue
    {
        public int Id { get; set; }
        public String Value { get; set; }
    }
}