using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelFactureASolder
    {
        public int Id { get; set; } //id de vente ou commande
        public DateTime Date { get; set; }
        public int? AssurId { get; set; } //id assurance
        public float Mtt { get; set; }
        //public float MttCl { get; set; }
        public string Ref { get; set; } //ref commande ou vente
        public Commande Cmd { get; set; }
        public Vente Vte { get; set; }
        public string Origine { get; set; }
        public string AssLib { get; set; }

    }
}