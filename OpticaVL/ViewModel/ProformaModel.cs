using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpticaVL.ViewModel
{
    public class ProformaModel
    {
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Assurance { get; set; }
        public Verre Verre { get; set; }
        public Monture Monture { get; set; }
        public string NumeroProforma { get; set; }
    }

    public class Monture 
    {
        public string Reference { get; set; }
        public string Numero { get; set; }
        public float Prix { get; set; }
        public int Qte { get; set; }
        public float Remise { get; set; }
    }

    public class Verre
    {
        public VerreFormat OD { get; set; }
        public VerreFormat OG { get; set; }
       /* public string Addition { get; set; }
        public string Designation { get; set; }*/
        public string Teinte { get; set; }
        public bool Transition { get; set; }
        public bool Organique { get; set; }
        public bool Polycarbonate { get; set; }
        public bool Mineraux { get; set; }
        /*public int Qte { get; set; }
        public float Prix { get; set; }
        public float Remise { get; set; }*/
    }

    public struct VerreFormat
    {
        public TypeVision VL { get; set; }
        public TypeVision VP { get; set; }
        public string Add { get; set; }
        public string Type { get; set; }
        public float Prix { get; set; } // prix unitaire
        public int Qte { get; set; }
        public float Remise { get; set; }
    }

    public struct TypeVision
    {
        public string Sph { get; set; }
        public string Cyl { get; set; }
        public string Axe { get; set; }
    }
}