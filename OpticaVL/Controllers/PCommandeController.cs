using OpticaVL.Models;
using OpticaVL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    public partial  class CommandeController : BaseController
    {

        private FCVerreFormat getFormatVerre(ModelVerre mv)
        {
            return new FCVerreFormat { Add = mv.Add, GammeVerreId = mv.GammeVerreId, Nature = mv.Nature, Prix = mv.Prix, Qte = mv.Qte, Remise = mv.Remise, RemiseDG = mv.RemiseDG, Type = mv.Type, VL = new FCTypeVision { Axe = mv.VLAxe, Cyl = mv.VLCyl, Sph = mv.VLSph }, VP = new FCTypeVision { Axe = mv.VPAxe, Cyl = mv.VPCyl, Sph = mv.VPSph } };
        }

        private ModelVerre getFormatModelVerre(FCVerreFormat fcv,Side _side, int cmdeId)
        {
            return new ModelVerre { Add = fcv.Add, CommandeId = cmdeId, GammeVerreId = fcv.GammeVerreId, Prix = fcv.Prix, Qte = fcv.Qte, Remise = fcv.Remise, RemiseDG = fcv.RemiseDG, VLAxe = fcv.VL.Axe, VLCyl = fcv.VL.Cyl, VLSph = fcv.VL.Sph, VPAxe = fcv.VP.Axe, VPCyl = fcv.VP.Cyl, VPSph = fcv.VP.Sph, Side = _side, Type = fcv.Type, Nature = fcv.Nature };
        }
        private List<MarqueViewModel> getListeMarqueMonture()
        {
            var listeMarqueMonture = (from m in ctx.Marques
                                      join p in ctx.Produits on m.Id equals p.MarqueId
                                      join c in ctx.Categories on p.CategorieId equals c.Id
                                      where c.Del == false && c.Libelle.ToUpper() == "MONTURE"
                                      select m).Distinct().OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList();
            return listeMarqueMonture;
        }


        private void UpdateStockProduit(List<LigneCommande> ligneCmde, string step)
        {
            if (ligneCmde.Count > 0)
            {
                if (step == "down")
                {
                    foreach (var p in ligneCmde)
                    {
                        var produit = p.Produit;
                        produit.QteStock -= p.QteCmd;
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    foreach (var p in ligneCmde)
                    {
                        var produit = p.Produit;
                        produit.QteStock += p.QteCmd;
                        ctx.SaveChanges();
                    }
                }  
            }
        }

        private void UpdateStockVerre(ModelVerre mv,Side side ,string step)
        {
            Side? _side = null;
            if (mv.Type == TypeVerre.Progressif) _side = mv.Side;
            var _mv = ctx.MVerres.Where(x => x.Add.ToUpper() == mv.Add.ToUpper() && x.Cyl.ToUpper() == mv.VLCyl.ToUpper() && x.GammeVerreId == mv.GammeVerreId && x.Sph.ToUpper() == mv.VLSph.ToUpper() && x.TypeVerre == mv.Type && x.Side == _side).FirstOrDefault();
            if (_mv != null && step == "down")
            {
                _mv.Qte -= mv.Qte;
                ctx.SaveChanges();
            }

            if (_mv != null && step == "up")
            {
                _mv.Qte += mv.Qte;
                ctx.SaveChanges();
            }
        }


        private MVerre VerreExiste(MVerreViewModel mvm)
        {
            Side? side = null;
            if (mvm.TypeVerre == TypeVerre.Progressif) side = mvm.Side;
            return ctx.MVerres.Where(x => x.Add.ToUpper() == mvm.Add.ToUpper() && x.Cyl.ToUpper() == mvm.Cyl.ToUpper() && x.GammeVerreId == mvm.GammeVerreId && x.Side == side && x.Sph.ToUpper() == mvm.Sph.ToUpper() && x.TypeVerre == mvm.TypeVerre).OrderByDescending(x => x.Id).FirstOrDefault();
        }

       
        private void CreateVerre(MVerreViewModel mvm, int qte)
        {
            if (VerreExiste(mvm) == null && qte > 0)
            {
                Side? side = null;
                if (mvm.TypeVerre == TypeVerre.Progressif) side = mvm.Side;
                ctx.MVerres.Add(new MVerre { Add = mvm.Add, Cyl = mvm.Cyl, GammeVerreId = mvm.GammeVerreId, Qte = 0, Side = side, Sph = mvm.Sph, TypeVerre = mvm.TypeVerre });
                ctx.SaveChanges();
            }
        }

        protected List<ModelCommande> GetQCommande(List<Commande> listeCmd)
        {
            List<ModelCommande> QCmdModel = new List<ModelCommande>();
            if (listeCmd.Count > 0)
            {
                foreach (var cmd in listeCmd)
                {
                    var lignecmd = cmd.LigneCommandes.Where(x => x.Del == false).ToList();
                    var _dateRLivraison = cmd.DateRLvrCmd != null ? cmd.DateRLvrCmd.Value.ToString("yyyy-MM-dd") : null;
                    var produit = lignecmd.Count > 0 ? lignecmd[0].Produit : new Produit();
                    var Verres = cmd.ModeleVerres.OrderBy(x=>x.Id).ToList();
                    var OD = Verres.Count > 0 ? Verres[0] : new ModelVerre();
                    var OG = Verres.Count >=2 ? Verres[1] : new ModelVerre();
                    DetailsMonture DetailsMonture = new DetailsMonture { Designation = produit.Designation, Numero = produit.RefProd, Qte = lignecmd.Count > 0? lignecmd[0].QteCmd:0 };
                    var _nature = Enum.Format(typeof(Models.NatureVerre), cmd.Nature, "g");
                    var _type = Enum.Format(typeof(Models.TypeVerre), OD.Type, "g");
                    var _typeOG = Enum.Format(typeof(Models.TypeVerre), OG.Type, "g");
                    var gamOD = OD.GammeVerre != null ? OD.GammeVerre.Libelle : "-";
                    var gamOG = OG.GammeVerre != null ? OG.GammeVerre.Libelle : "-";
                    DetailsVerres DetailsVerre = new DetailsVerres
                    {
                        Nature = _nature,
                        Teinte = cmd.Teinte,
                        OD = new ModelDeVerre { Add = OD.Add, Type = _type, VL = new FCTypeVision { Axe = OD.VLAxe, Cyl = OD.VLCyl, Sph = OD.VLSph }, VP = new FCTypeVision { Axe = OD.VPAxe, Cyl = OD.VPCyl, Sph = OD.VPSph }, GammeV = gamOD },
                        OG = new ModelDeVerre { Add = OG.Add, Type = _typeOG, VL = new FCTypeVision { Axe = OG.VLAxe, Cyl = OG.VLCyl, Sph = OG.VLSph }, VP = new FCTypeVision { Axe = OG.VPAxe, Cyl = OG.VPCyl, Sph = OG.VPSph }, GammeV = gamOG }
                    };
                    IdentiteClient identiteCl = new IdentiteClient { Nom = cmd.Client.Nom.ToUpper(), Prenom = Title(cmd.Client.Prenom), Contact = cmd.Client.Contact, Contact2 = cmd.Client.Contact2, Email = cmd.Client.Adresse };
                    //var _datelivraison = cmd.DateLvrCmd != null ? cmd.DateLvrCmd.ToString("yyyy-MM-dd"):null;

                    QCmdModel.Add(new ModelCommande { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), DateLivraison = _dateRLivraison, DetailsMonture = DetailsMonture, DetailsVerres = DetailsVerre, Identite = identiteCl, Id = cmd.Id, Valider = cmd.StatutCmd, RefCmde = cmd.RefCmd });
                }
            }
            return QCmdModel;
        }
	}
}