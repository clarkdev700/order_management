using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class FactureProforma
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string ReferenceRecu { get; set; }
        public string NumeroProforma { get; set; }
        public bool Del { get; set; }
        public DateTime Date { get; set; }
    }
}