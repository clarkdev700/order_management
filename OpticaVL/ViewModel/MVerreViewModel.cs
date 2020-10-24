using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class MVerreViewModel
    {
        public int? GammeVerreId { get; set; }
        public TypeVerre TypeVerre { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Add { get; set; }
        public Side Side { get; set; }
    }
}