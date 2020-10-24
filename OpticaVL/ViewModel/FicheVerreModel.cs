using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class FicheVerreModel
    {
        public int Id { get; set; } //Id du verre
        public string Sph { get; set; }
        public string Cycl { get; set; }
        //public string Axe { get; set; }
        public string Addition { get; set; }
        public Side? Side { get; set; }
        //public int PuissanceId { get; set; }
        public int? GammeVerreId { get; set; }
        public TypeVerre TypeVerre { get; set; }
        public int Qte { get; set; }
    }
}