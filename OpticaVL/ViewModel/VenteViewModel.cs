using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class VenteViewModel
    {
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public DateTime DateVente { get; set; }
        public string ModePayement { get; set; }
        //public int? AssuranceId { get; set; }
        //public float MontantAssurance { get; set; }
        public float MontantClient { get; set; }
        public float ReductionClient { get; set; }
        public float MontantVerse { get; set; }
        public string StatutReglement { get; set; }
        public string RefPayement { get; set; }
        public List<int> AN { get; set; }
        public List<float> AM { get; set; }
        public List<AssuranceMontant> AssuranceMontant { get; set; }
        public List<LigneVenteModel> LigneVentes { get; set; }
    }

    public class LigneVenteModel
    {
        public int prodid { get; set; }
        public string refprod { get; set; }
        public string designation { get; set; }
        public int qte { get; set; }
        public float price { get; set; }
        public float Rem { get; set; }
        public float RemDg { get; set; }
    }

    public class OpVenteModel
    {
        public int Id { get; set; }
        public string RefVente { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public float Montant { get; set; }
        public string Date { get; set; }
        public List<DetailsCommande> DetailsVente { get; set; }
        public bool StatutDel { get; set; }
        public List<int> Assurances { get; set; }
    }

    public struct AssuranceMontant
    {
        public int Id { get; set; }
        public int AssuranceId { get; set; }
        public float Montant { get; set; }
    }
}