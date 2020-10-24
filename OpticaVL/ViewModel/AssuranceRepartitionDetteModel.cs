using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class AssuranceRepartitionDetteModel
    {
        public int Id { get; set; } //Id assurance
        public string Nom { get; set; }
        public string Code { get; set; }
        public string RefFacture { get; set; }
        public float Montant { get; set; }
        public double Taux { get; set; }
    }
}