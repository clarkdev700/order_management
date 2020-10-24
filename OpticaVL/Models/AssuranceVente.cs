using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class AssuranceVente
    {
        public int Id { get; set; }
        public float MontantAssur { get; set; }
        public bool PayeAssur { get; set; }

        public int VenteId { get; set; }
        public int AssuranceId { get; set; }

        public virtual Assurance Assurance { get; set; }
        public virtual Vente Vente { get; set; }
    }
}