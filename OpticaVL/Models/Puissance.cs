using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Puissance
    {
        public int Id { get; set; }
        public string Sph { get; set; }
        public string Cycl { get; set; }
        public string Axe { get; set; }
        public string Addition { get; set; }
        public TypeVerre TypeVerre { get; set; }

        //
        public virtual ICollection<GammeVerrePuissance> GammeVerrePuissance { get; set; }
    }
}