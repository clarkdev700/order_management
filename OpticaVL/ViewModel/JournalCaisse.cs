using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class JournalCaisse
    {
        public int Id { get; set; }
        public string RefFacture { get; set; }
        public string NumRecu { get; set; }
        public string Identite { get; set; }
        public float MontantVerse { get; set; }
        public string Date { get; set; }
        public bool DelState { get; set; }
    }
}