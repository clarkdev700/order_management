using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class AssurancePayementViewModel
    {
        public int Id { get; set; }
        public float Montant { get; set; }
        public int IdAssurance { get; set; }
        public string Origine { get; set; }
    }

    public class InfoReglement
    {
        public ModeReglement ModeReglement { get; set; }
        public string RefPayement { get; set; }
        public DateTime DatePaye { get; set; }
    }
}