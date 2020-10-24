using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Produit
    {
        public int Id { get; set; }
        //public string RefProd { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Designation { get; set; }
        public string RefProd { get; set; }
        //public string Marque { get; set; }
        //public int? Numero { get; set; }
        public float Prix { get; set; }
        public int QteSeuil { get; set; }
        public int QteStock { get; set; }
        public bool Del { get; set; }
        
        //
        public int? MarqueId { get; set; }
        public int CategorieId { get; set; }
        //public int? ModelDiversId { get; set; }
        //public int? ModelMontureId { get; set; }
        //public int? ModelVerreId { get; set; }

        //
        public virtual Categorie Categorie { get; set; }
        public virtual Marque Marque { get; set; }
        //public virtual ModelDivers ModelDivers { get; set; }
        //public virtual ModelMonture ModelMonture { get; set; }
        //public virtual ModelVerre ModelVerre { get; set; }
    }
}