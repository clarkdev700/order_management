using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class AssuranceCommande
    {
        public int Id { get; set; }
        public float MontantAsssur { get; set; }
        public bool PayeAssur { get; set; }

        public int CommandeId { get; set; }
        public int AssuranceId { get; set; }

        public virtual Commande Commande { get; set; }
        public  virtual Assurance Assurance { get; set; }
    }
}