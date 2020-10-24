using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class InventaireViewModel
    {
        public string Categorie { get; set; }
        public List<Prod> Produits { get; set; }
    }

    public class Prod
    {
        public string RefProd { get; set; }
        public string Designation { get; set; }
        public int? Numero { get; set; }
        public int QteStock { get; set; }
        public float Prix { get; set; }
        public string Marque { get; set; }
    }
}