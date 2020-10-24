using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class CatProduit
    {
        public string Categorie { get; set; }
        public List<MProd> Produits { get; set; }
    }

    public class MProd
    {
        public string LibMarque { get; set; }
        public List<MarqueProd> MarqueProds { get; set; }
    }

    public class MarqueProd
    {
        public int Id { get; set; }
        public string RefProd { get; set; }
        public string Designation { get; set; }
        public float Prix { get; set; }
        public float QteStock { get; set; }
    }

    public class ProduitModel
    {
        public int Id { get; set; }
        public string RefProd { get; set; }
        public string Designation { get; set; }
        public string Marque { get; set; }
        public float Prix { get; set; }
        public int QteStock { get; set; }
        public string Categorie { get; set; }
        public ModelVerre Verre { get; set; }
        public ModelDivers Divers { get; set; }
        public ModelMonture Monture { get; set; }
    }

    public class HProduct
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string Categorie { get; set; }
        public string Marque { get; set; }
        public float Prix { get; set; }
        public int QteStock { get; set; }
    }

    public class CategorieProduit
    {
        public string LibCat { get; set; }
        public List<HProduct> Produits { get; set; }
    }

    public class ProductStock
    {
        public int Id { get; set; } // id du stock
        public string Designation { get; set; }
        public string Marque { get; set; }
        public string Fournisseur { get; set; }
        public int Qte { get; set; }
        public string  Date { get; set; }
    }
}