using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class CommandeViewModel
    {
        public DateTime DateCommande { get; set; }
        public DateTime DateLvrCmd { get; set; }
        public float MontantAssur { get; set; }
        public float MontantClient { get; set; }
        public int? IdAssurance { get; set; }
        public string TypeCommande { get; set; }
        public List<LigneCommandeModel> LigneCommandes { get; set; }
    }

    public class LigneCommandeModel
    {
        public int Id { get; set; }
        public string RefProd { get; set; }
        public float Prix { get; set; }
        public int Qte { get; set; }
        public float rem { get; set; }
        public float remdg { get; set; }
    }
}