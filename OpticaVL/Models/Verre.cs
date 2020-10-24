using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class MVerre
    {
        public int Id { get; set; }
        public TypeVerre TypeVerre { get; set; }
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Add { get; set; }
        public Side? Side { get; set; }
        public int Qte { get; set; }

        public int? GammeVerreId { get; set; }

        public virtual GammeVerre GammeVerre { get; set; }
    }
}