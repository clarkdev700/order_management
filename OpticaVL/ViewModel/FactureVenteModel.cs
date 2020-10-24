using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class FactureVenteModel
    {
        public string IdentiteClient { get; set; }
        public float MontantFacture { get; set; }
        public int NumeroRecu { get; set; }
        public string NomAssurance { get; set; }
        public List<FactureVenteLigneModel> FactureVenteLigneModel { get; set; }
    }

    public struct FactureVenteLigneModel
    {
        public int Qte { get; set; }
        public float Pu { get; set; }
        public string Designation { get; set; }
        public float SousTotal { get; set; }
        public float Remise { get; set; }
    }
}