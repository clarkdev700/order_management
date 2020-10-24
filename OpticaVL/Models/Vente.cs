using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Vente
    {
        public int Id { get; set; }
        public string RefVente { get; set; }
        public DateTime DateVente { get; set; }
        public DateTime DateEnreg { get; set; }
        //public string IdentiteClient { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        //public bool Paye { get; set; }
        public float MontantClient { get; set; }
        public float MontantAssurance { get; set; }
        public float ReductionClient { get; set; }
        //public bool PayeAssurance { get; set; }
        public bool PayeClient { get; set; }
        public bool Del { get; set; }
        public DateTime? DateDel { get; set; }
        public string MotifDel { get; set; }

        //
        //public int? AssuranceId { get; set; }

        //
        //public virtual Assurance Assurance { get; set; }
        public virtual ICollection<LigneVente> LigneVentes { get; set; }
        public virtual ICollection<Payement> Payements { get; set; }
        public virtual ICollection<AssuranceVente> AssuranceVentes { get; set; }
    }
}