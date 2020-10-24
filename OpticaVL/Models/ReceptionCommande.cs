using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class ReceptionCommande
    {
        public int Id { get; set; }
        public DateTime DateReception { get; set; }
        public string IdentiteReceptionnaire { get; set; }
        public string Commentaire { get; set; }
        public DateTime DateEnreg { get; set; }

        //
        public int CommandeId { get; set; }

        //
        public virtual Commande Commande { get; set; }
    }
}