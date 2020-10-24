using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ModelCommandeDispo
    {
        public string Date { get; set; }
        public IdentiteClient Client { get; set; }
        public string RefCmde { get; set; }
        public string DateDispo { get; set; }
        public int Id { get; set; }
    }
}