using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class QCommandeModel
    {
        public int Id { get; set; } //id de la commande
        public string RefCommande { get; set; }
        public string Date { get; set; }
        public string DateLivraison { get; set; }
        public string DateRetardLivraison { get; set; }
        public bool Valider { get; set; }
        public bool Livrer { get; set; }
        public string IdentiteClient { get; set; }
        public Adresse Adresse { get; set; }
        public float MontantAssur { get; set; }
        public float MontantClient { get; set; }
        public bool EstAssure { get; set; }
        public List<DetailsCommande> DetailsCommande { get; set; }
        public List<int> Assurances { get; set; }
    }

    public struct DetailsCommande
    {
        public string Designation { get; set; }
        public string Marque { get; set; }
        public int Qte { get; set; }
    }

    public struct Adresse
    {
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Adr { get; set; }
    }

    public struct Commandetype
    {
        public int Id { get; set; }
        public string Valeur { get; set; }
    }
}