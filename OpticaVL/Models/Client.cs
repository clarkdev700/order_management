using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Nom { get; set; }
        public Civilite Civilite { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string Prenom { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Adresse { get; set; }
        public string Profession { get; set; }
        public bool Del { get; set; }

        //
        public virtual ICollection<Commande> Commandes { get; set; }
    }

    public enum Civilite
    {
        M,
        Mlle,
        Mme
    }
}