using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Commande
    {
        public int Id { get; set; }
        [Required()]
        public string RefCmd { get; set; }
        public DateTime DateCmd { get; set; }
        public DateTime DateLvrCmd { get; set; }
        public DateTime? DateRLvrCmd { get; set; }
        public bool StatutCmd { get; set; }
        public string Commentaire { get; set; }
        public float MontantAssur { get; set; }
        public float MontantClient { get; set; }
        public float ReductionClient { get; set; }
        //public bool PayeAssur { get; set; }
        public bool PayeClient { get; set; }
        //partie propre au verre
        public string Teinte { get; set; }
        public NatureVerre Nature { get; set; }
        //public TypeCommande TypeCmd { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }
        public DateTime? DateDel { get; set; }
        public string MotifDel { get; set; }
        public string User { get; set; }

        //
        //public int? AssuranceId { get; set; }
        public int ClientId { get; set; }

        //
        //public virtual Assurance Assurance { get; set; }
        public virtual Client Client { get; set; }
        //public virtual Produit Produit { get; set; }
        public virtual ICollection<ModelVerre> ModeleVerres { get; set; }
        public virtual ICollection<LigneCommande> LigneCommandes { get; set; }
        public virtual ICollection<Payement> Payements { get; set; }
        public virtual ICollection<AssuranceCommande> AssuranceCommandes { get; set; }
    }

    public enum TypeCommande
    {
        Provisoire,
        Definitive
    }
}