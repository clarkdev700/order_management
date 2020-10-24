using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class FicheCommandeModel
    {
        public Models.Civilite Civilite { get; set; }
        public int Id { get; set; } //id de la commande
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        //public int? AssuranceId { get; set; }
        public FCMonture Monture { get; set; }
        public FCVerre Verre { get; set; }
        public DateTime DateCommande { get; set; }
        public FCPayement Payement { get; set; }
        public string StatutNbPaye { get; set; }
        public List<int> AN { get; set; }
        public List<float> AM { get; set; }
    }

    public class FCVerre
    {
        public FCVerreFormat OD { get; set; }
        public FCVerreFormat OG { get; set; }
        public string Teinte { get; set; }
        public bool Mineraux { get; set; }
        public bool Organique { get; set; }
    }

    public struct FCMonture
    {
        public int MontureId { get; set; }
        public int MontureQte { get; set; }
        public float MonturePrix { get; set; }
        public float MontureRemise { get; set; }
        public float MontureRemiseDG { get; set; }
        public int MontureProdId { get; set; }
    }
    public struct FCVerreFormat
    {
        public FCTypeVision VL { get; set; }
        public FCTypeVision VP { get; set; }
        public int? GammeVerreId { get; set; }
        public string Add { get; set; }
        public Models.TypeVerre Type { get; set; }
        public Models.NatureVerre Nature { get; set; }
        public float Prix { get; set; } // prix unitaire
        public int Qte { get; set; }
        public float Remise { get; set; }
        public float RemiseDG { get; set; }
    }

    public struct FCPayement
    {
        public Models.ModeReglement ModeReglement { get; set; }
        public float MontantVerse { get; set; }
        public float MontantAssurance { get; set; }
        public float MontantClient { get; set; }
        public float ReductionClient { get; set; }
        public string RefPayement { get; set; }
    }

    public struct FCTypeVision
    {
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axe { get; set; }
    }

    public enum Civilite
    {
        M,
        Mlle,
        Mme
    }
}