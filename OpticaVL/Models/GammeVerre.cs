using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpticaVL.Models
{
    public class GammeVerre
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Libelle { get; set; }
    }
}