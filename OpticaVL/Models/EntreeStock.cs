using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class EntreeStock
    {
        public int Id { get; set; }
        public int Qte { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }
        public DateTime? DateDel { get; set; }
        public string MotifDel { get; set; }

        //
        public int FournisseurId { get; set; }
        public int ProduitId { get; set; }

        //
        public virtual Produit Produit { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
    }
}