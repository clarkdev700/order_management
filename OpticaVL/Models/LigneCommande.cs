using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class LigneCommande
    {
        public int Id { get; set; }
        public int QteCmd { get; set; }
        public float Rem { get; set; }
        public float RemDG { get; set; }
        public float PrixCmd { get; set; }
        public bool Del { get; set; }

        //
        public int CommandeId { get; set; }
        public int ProduitId { get; set; }

        //
        public virtual Commande Commande { get; set; }
        public virtual Produit Produit { get; set; }
    }
}