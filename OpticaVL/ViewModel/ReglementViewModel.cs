using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ReglementViewModel
    {
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public float MontantClient { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public float MontantAssurance { get; set; }
        public float ResteApayerClient { get; set; }
        public Payement Payement { get; set; }
        public string Type { get; set; }
        public int? Source { get; set; }
        public int TypeId { get; set; }
        public string Reference { get; set; } //Reference commande ou vente
        public List<LigneFacture> LigneFactures { get; set; }
        public List<HistoriqueReglement> HistoriqueReglement { get; set; }
    }

    public class LigneFacture
    {
        public string RefProd { get; set; }
        public string Designation { get; set; }
        public int? Numero { get; set; }
        public string Marque { get; set; }
        public float Pu { get; set; }
        public int Qte { get; set; }
        public float MontantLigne { get; set; }
        public float Rem { get; set; }
        public float RemDg { get; set; }
    }

    public class HistoriqueReglement 
    {
        public int Id { get; set; } //id payement
        public string DatePayement { get; set; }
        public float MontantVerse { get; set; }
        public string ModeReglement { get; set; }
        public string RefPayement { get; set; }
        public int? AssuranceId { get; set; }
    }
}