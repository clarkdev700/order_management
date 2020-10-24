﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Marque
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Libelle { get; set; }

        //
        public virtual ICollection<Produit> Produits { get; set; }
    }
}