using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class GammeVerrePuissanceModel
    {
        public int IdGVP { get; set; } //gammeVerre puissance
        //public string Axe { get; set; }
        public string Sph { get; set; }
        public string Cycl { get; set; }
        public string Addition { get; set; }
        public string TypeVerre { get; set; }
        public string Side { get; set; }
        public int Qte { get; set; }
        public string GammeVerre { get; set; }
    }
}