using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Fournisseur
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Code { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Nom { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public bool Del { get; set; }

        // 
        public virtual ICollection<EntreeStock> EntreeStocks { get; set; }
    }
}