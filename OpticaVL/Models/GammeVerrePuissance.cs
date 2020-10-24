using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class GammeVerrePuissance
    {
        public int Id { get; set; }
        public int Qte { get; set; }
        public bool Del { get; set; }
        //
        public int GammeVerreId { get; set; }
        public int PuissanceId { get; set; }

        //
        public virtual GammeVerre GammeVerre { get; set; }
        public virtual Puissance Puissance { get; set; }
    }
}