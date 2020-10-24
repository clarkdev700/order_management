using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelCommande
    {
        public int Id { get; set; }
        public string RefCmde { get; set; }
        public string NomAssurance { get; set; }
        public string Date { get; set; }
        public IdentiteClient Identite { get; set; }
        public DetailsMonture DetailsMonture { get; set; }
        public DetailsVerres DetailsVerres { get; set; }
        public string DateLivraison { get; set; }
        public float ChargeAssurance { get; set; }
        public float ChargeClient { get; set; }
        public float ReductionClient { get; set; }
        public bool Valider { get; set; }
        public List<AssuranceCharge> AssuranceCharge { get; set; }
    }

    public struct IdentiteClient
    {
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
    }
    public struct DetailsMonture
    {
        public string Designation { get; set; }
        public string Numero { get; set; }
        public int Qte { get; set; }
        public float Montant { get; set; }
        public float Remise { get; set; }
    }

    public class DetailsVerres
    {
        public string Teinte { get; set; }
        public string Nature { get; set; }
        public ModelDeVerre OD { get; set; }
        public ModelDeVerre OG { get; set; }
    }

    public struct ModelDeVerre
    {
        public string Add { get; set; }
        public string Type { get; set; }
        public string GammeV { get; set; }
        public FCTypeVision VL { get; set; }
        public FCTypeVision VP { get; set; }
        public float Montant { get; set; }
        public float Remise { get; set; }
        public int Qte { get; set; }
    }

    public struct AssuranceCharge
    {
        public string CodeAssurance { get; set; }
        public float Montant { get; set; }
    }
}