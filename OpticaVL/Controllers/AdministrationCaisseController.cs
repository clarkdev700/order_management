using OpticaVL.Models;
using OpticaVL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class AdministrationCaisseController : BaseController
    {
        //
        // GET: /AdministrationCaisse/
        [Route("administration/facture-a-solder/", Name = "_factureASolder")]
        public ActionResult Index()
        {
            return View("CommandeAsolder2");
        }

        [Route("administration/liste-facture-a-solder/", Name="_getFactureAsolder")]
        public ActionResult CommandeASolder()
        {
            List<CommandeAReglerViewModel> CmdARModel = new List<CommandeAReglerViewModel>();
            var listeCommande = (from c in ctx.Commandes
                                 join cl in ctx.Clients on c.ClientId equals cl.Id
                                 where c.Del == false && c.PayeClient == false
                                 select new ModelFactureASolder{ AssLib = "--", AssurId = null, Cmd = c, Date = c.DateCmd, Id = c.Id, Mtt = c.MontantClient, Origine = "commande", Ref = c.RefCmd}).Concat(
                                 from c in ctx.Commandes 
                                 join cl in ctx.Clients on c.ClientId equals cl.Id 
                                 join ac in ctx.AssuranceCommandes on c.Id equals ac.CommandeId
                                 join a in ctx.Assurances on ac.AssuranceId equals a.Id
                                 where c.Del == false && ac.PayeAssur == false
                                 select new ModelFactureASolder{ AssLib = a.Nom, AssurId = a.Id, Cmd = c, Date = c.DateCmd, Id = c.Id, Mtt = ac.MontantAsssur, Origine = "commande", Ref = c.RefCmd}
                                 ).ToList();

                                /*join ac in ctx.AssuranceCommandes on c.Id equals ac.CommandeId
                                 join a in ctx.Assurances on ac.AssuranceId equals a.Id into cmdgpr
                                where c.Del == false && (ac.PayeAssur == false || c.PayeClient == false)
                                from item in cmdgpr.DefaultIfEmpty()
                                select new ModelFactureASolder {Id = c.Id, Date = c.DateCmd, Ref = c.RefCmd, MttAss = c.MontantAssur, MttCl = c.MontantClient, AssLib = item != null? item.Nom:"--", Cmd = c, Origine = "commande" }).ToList();*/
            var listeVente = (from v in ctx.Ventes
                              where v.Del == false && v.PayeClient == false
                              select new ModelFactureASolder{ AssLib = "--", AssurId = null, Date = v.DateVente, Id = v.Id, Mtt = v.MontantClient, Origine = "vente", Ref = v.RefVente, Vte = v}).Concat(
                              from v in ctx.Ventes
                              join av in ctx.AssuranceVentes on v.Id equals av.VenteId
                              join a in ctx.Assurances on av.AssuranceId equals a.Id
                              where v.Del == false && av.PayeAssur == false
                              select new ModelFactureASolder{ AssLib = a.Nom, AssurId = a.Id, Date = v.DateVente, Id = v.Id, Mtt = av.MontantAssur, Origine = "vente", Ref = v.RefVente, Vte = v}
                              ).ToList();

                              /*join av in ctx.AssuranceVentes on v.Id equals av.VenteId
                              join a in ctx.Assurances on av.AssuranceId equals a.Id into vtegpr
                              where v.Del == false && (av.PayeAssur == false || v.PayeClient == false)
                              from item in vtegpr.DefaultIfEmpty()
                              select new ModelFactureASolder { Id = v.Id, Date = v.DateVente, Ref = v.RefVente, MttAss = v.MontantAssurance, MttCl = v.MontantClient, AssLib = item != null ? item.Nom : "--", Vte = v, Origine = "vente" }).ToList();*/
            var listeFacture = listeCommande.Concat(listeVente);
            var grpFacture = (from x in listeFacture group x by x.AssLib).ToList();
            
            if (grpFacture.Count > 0)
            {
                int index = 0;
                foreach (var facture in grpFacture)
                {
                    foreach (var f in facture.OrderBy(x=>x.Date).ToList())
                    {
                        index++;
                        if (f.Origine == "commande")
                        {
                            var commande = f.Cmd;
                            var mttAR = f.AssurId != null ? commande.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == f.AssurId).ToList().Sum(x => x.MontantPaye) : commande.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + commande.ReductionClient;
                            var _resteApayer = f.Mtt - mttAR;
                            //var _resteCl = f.MttCl - (mttClR + commande.ReductionClient);
                            var _clIdentite = f.Cmd.Client.Nom.ToUpper() + " " + Title(f.Cmd.Client.Prenom);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "commande", DateOp = f.Date, AssurId = f.AssurId, indexKey = index });
                        }
                        else
                        {
                            //vente
                            var vente = f.Vte;
                            var mttR =f.AssurId != null ?  vente .Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == f.AssurId).ToList().Sum(x => x.MontantPaye) : vente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + vente.ReductionClient;
                            //var mttClR = ;
                            //var _resteAss = f.MttAss - mttAssR;
                            var _resteApayer = f.Mtt - mttR;
                            var _clIdentite = f.Vte.NomClient.ToUpper() + " " + Title(f.Vte.PrenomClient);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, AssurId = f.AssurId, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "vente", DateOp = f.Date, indexKey = index });
                        }
                    }
                }
            }
            return Json(CmdARModel, JsonRequestBehavior.AllowGet);
        }

        [Route("administration/liste-client-facture-à-solder/", Name="_getListeClientFacture")]
        public JsonResult GetFactureClientAsolder()
        {
            List<CommandeAReglerViewModel> CmdARModel = new List<CommandeAReglerViewModel>();
            var listeCommande = (from c in ctx.Commandes
                                 join cl in ctx.Clients on c.ClientId equals cl.Id
                                 where c.Del == false && c.PayeClient == false
                                 select new ModelFactureASolder { AssLib = "--", AssurId = null, Cmd = c, Date = c.DateCmd, Id = c.Id, Mtt = c.MontantClient, Origine = "commande", Ref = c.RefCmd }).ToList();


            var listeVente = (from v in ctx.Ventes
                              where v.Del == false && v.PayeClient == false
                              select new ModelFactureASolder { AssLib = "--", AssurId = null, Date = v.DateVente, Id = v.Id, Mtt = v.MontantClient, Origine = "vente", Ref = v.RefVente, Vte = v }).ToList();

            var listeFacture = listeCommande.Concat(listeVente);
            var grpFacture = (from x in listeFacture group x by x.AssLib).ToList();

            if (grpFacture.Count > 0)
            {
                int index = 0;
                foreach (var facture in grpFacture)
                {
                    foreach (var f in facture.OrderBy(x => x.Date).ToList())
                    {
                        index++;
                        if (f.Origine == "commande")
                        {
                            var commande = f.Cmd;
                            var mttAR = commande.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + commande.ReductionClient;
                            var _resteApayer = f.Mtt - mttAR;
                            //var _resteCl = f.MttCl - (mttClR + commande.ReductionClient);
                            var _clIdentite = f.Cmd.Client.Nom.ToUpper() + " " + Title(f.Cmd.Client.Prenom);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "commande", DateOp = f.Date, AssurId = f.AssurId, indexKey = index });
                        }
                        else
                        {
                            //vente
                            var vente = f.Vte;
                            var mttR = vente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + vente.ReductionClient;
                            var _resteApayer = f.Mtt - mttR;
                            var _clIdentite = f.Vte.NomClient.ToUpper() + " " + Title(f.Vte.PrenomClient);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, AssurId = f.AssurId, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "vente", DateOp = f.Date, indexKey = index });
                        }
                    }
                }
            }
            return Json(CmdARModel, JsonRequestBehavior.AllowGet);
        }

        [Route("administration/assurance-facture-à-solder/", Name = "_factureASolderAssurance")]
        public ActionResult AssuranceFacture()
        {
            return View(new InfoReglement());
        }

        [Route("administrattion/get-list-facture-assurance/", Name="_getListFactureAssurance")]
        public JsonResult GetListFactureAssurance()
        {
            List<CommandeAReglerViewModel> CmdARModel = new List<CommandeAReglerViewModel>();
            var listeCommande = (
                                 from c in ctx.Commandes
                                 join cl in ctx.Clients on c.ClientId equals cl.Id
                                 join ac in ctx.AssuranceCommandes on c.Id equals ac.CommandeId
                                 join a in ctx.Assurances on ac.AssuranceId equals a.Id
                                 where c.Del == false && ac.PayeAssur == false
                                 select new ModelFactureASolder { AssLib = a.Nom, AssurId = a.Id, Cmd = c, Date = c.DateCmd, Id = c.Id, Mtt = ac.MontantAsssur, Origine = "commande", Ref = c.RefCmd }
                                 ).ToList();

            var listeVente = (
                              from v in ctx.Ventes
                              join av in ctx.AssuranceVentes on v.Id equals av.VenteId
                              join a in ctx.Assurances on av.AssuranceId equals a.Id
                              where v.Del == false && av.PayeAssur == false
                              select new ModelFactureASolder { AssLib = a.Nom, AssurId = a.Id, Date = v.DateVente, Id = v.Id, Mtt = av.MontantAssur, Origine = "vente", Ref = v.RefVente, Vte = v }
                              ).ToList();

            var listeFacture = listeCommande.Concat(listeVente);
            var grpFacture = (from x in listeFacture group x by x.AssLib).ToList();

            if (grpFacture.Count > 0)
            {
                int index = 0;
                foreach (var facture in grpFacture)
                {
                    foreach (var f in facture.OrderBy(x => x.Date).ToList())
                    {
                        index++;
                        if (f.Origine == "commande")
                        {
                            var commande = f.Cmd;
                            var mttAR = commande.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == f.AssurId).ToList().Sum(x => x.MontantPaye);
                            var _resteApayer = f.Mtt - mttAR;
                            var _clIdentite = f.Cmd.Client.Nom.ToUpper() + " " + Title(f.Cmd.Client.Prenom);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "commande", DateOp = f.Date, AssurId = f.AssurId, indexKey = index });
                        }
                        else
                        {
                            //vente
                            var vente = f.Vte;
                            var mttR =  vente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == f.AssurId).ToList().Sum(x => x.MontantPaye);       
                            var _resteApayer = f.Mtt - mttR;
                            var _clIdentite = f.Vte.NomClient.ToUpper() + " " + Title(f.Vte.PrenomClient);
                            CmdARModel.Add(new CommandeAReglerViewModel { Date = f.Date.ToString("dd/MM/yyyy"), Id = f.Id, AssurId = f.AssurId, Montant = f.Mtt, ResteAPayer = _resteApayer, RefCommande = f.Ref, AssuranceNom = facture.Key.ToUpper(), ClientIdentite = _clIdentite, Origine = "vente", DateOp = f.Date, indexKey = index });
                        }
                    }
                }
            }
            return Json(CmdARModel, JsonRequestBehavior.AllowGet);
        }
	}  
}