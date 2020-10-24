using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class ModelVerre
    {
        public int Id { get; set; }
        public NatureVerre Nature { get; set; }
        public string VLAxe { get; set; }
        public string VLSph { get; set; }
        public string VLCyl { get; set; }
        public string VPAxe { get; set; }
        public string VPSph { get; set; }
        public string VPCyl { get; set; }
        public string Add { get; set; }
        public TypeVerre Type { get; set; }
        public Side Side { get; set; }
        public float Prix { get; set; }
        public float Remise { get; set; }
        public float RemiseDG { get; set; }
        public int Qte { get; set; }
        public bool Del { get; set; }

        //
        public int? GammeVerreId { get; set; }
        public int CommandeId { get; set; }
        //
        public virtual GammeVerre GammeVerre { get; set; }
        public virtual Commande Commande { get; set; }
    }

    public enum NatureVerre
    {
        Organique,
        Mineraux
    }

    public enum TypeVerre
    {
        Unifocaux,
        Progressif,
        Bifocaux
    }

    public enum Side
    {
        OD,
        OG
    }

    public enum FormatVerre
    {
        Cylindrique,
        Plan,
        Spherique
    }
}