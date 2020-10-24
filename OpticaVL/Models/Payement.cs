using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class Payement
    {
        public int Id { get; set; }
        public float MontantPaye { get; set; }
        public ModeReglement ModePaye { get; set; }
        public SourcePaye SourcePaye { get; set; }
        [Required(ErrorMessage="Champ obligatoire")]
        public DateTime DatePaye { get; set; }
        public bool MontantEncaisse { get; set; }
        public string RefPayement { get; set; }
        public DateTime DateEnreg { get; set; }
        public bool Del { get; set; }
        public DateTime? DateDel { get; set; }
        public string MotifDel { get; set; }
        public string User { get; set; }

        //
        public int? RefCmd { get; set; }
        public int? RefVente { get; set; }
        public int? AssuranceId { get; set; }

        //
        [ForeignKey("RefCmd")]
        public virtual Commande Commande { get; set; }
        [ForeignKey("RefVente")]
        public virtual Vente Vente { get; set; }
        public virtual Assurance Assurance { get; set; }
    }

    public enum SourcePaye
    {
        Assurance,
        Client
    }

    public enum ModeReglement
    {
        Espece,
        Cheque,
        Virement
    }
}