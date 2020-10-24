using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class LigneVente
    {
        public int Id { get; set; }
        public int QteVente { get; set; }
        public float PrixVente { get; set; }
        public float Rem { get; set; }
        public float Remdg { get; set; }
        public bool Del { get; set; }

        //
        public int VenteId { get; set; }
        public int ProduitId { get; set; }

        //
        public virtual Vente Vente { get; set; }
        public virtual Produit Produit { get; set; }
    }
}