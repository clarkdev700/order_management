using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ProduitViewModel
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public float Prix { get; set; }
        public int QteStock { get; set; }
        public int QteSeuil { get; set; }
        public int CategorieId { get; set; }
        public int? MarqueId { get; set; }
        public ModelMonture ModelMonture { get; set; }
        public ModelDivers ModelDivers { get; set; }
        public ModelVerre ModelVerre { get; set; }

    }
}