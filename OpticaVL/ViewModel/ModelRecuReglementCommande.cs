using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelRecuReglementCommande
    {
        public int NumeroRecu { get; set; }
        public string RefCommande { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string MontantVerse { get; set; }
        public string MontantTotalVerse { get; set; }
        public string MontantAssurance { get; set; }
        public string MontantTotal { get; set; }
        public string MontantClient { get; set; } // charge à regler par le client
        public string ResteApayer { get; set; }
        public float ReductionClient { get; set; }
        public List<HistoriquePaiement> Hpaiement { get; set; }
        public string NomCaissier { get; set; }
    }

    public struct MontureInfo
    {
        public string Designation { get; set; }
        public string Numero { get; set; }
        public int Qte { get; set; }
        public float Prix { get; set; } //prix commande - Remise + Remise DG

    }

    public struct HistoriquePaiement
    {
        public string Montant { get; set; }
        public string ModePaiement { get; set; }
        public string Date { get; set; }
    }
    public struct VerreInfo
    {
        public string Teinte { get; set; }
        public string Nature { get; set; }
        public SideInfo OD { get; set; }
        public SideInfo OG { get; set; }
    }

    public struct SideInfo
    {
        public float Cyl { get; set; }
        public float Sph { get; set; }
        public float Axe { get; set; }
        public float Add { get; set; }
        public int Qte { get; set; }
        public float Prix { get; set; } //Montant - Remise + Remise DG
    }
}