using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class DashBoard
    {
        public float Caisse { get; set; }
        public double CompteASolder { get; set; }
        public int CmdeAttente { get; set; }
        public int CmdeDispo { get; set; }
        public double ChargeAssur { get; set; }
        public double ChargeClient { get; set; }
    }
}