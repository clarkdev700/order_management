using OpticaVL.Models;
using OpticaVL.ViewModel;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class DashBoardController : BaseController
    {
        //
        // GET: /DashBoard/
        [Route("dashboard/", Name="_dashboardhome")]
        public ActionResult Index()
        {
            var _day = DateTime.Now.Date;
            var montantEncaisse = ctx.Payements.Where(x => x.Del == false && x.DatePaye == _day && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
            var cmdeAttente = ctx.Commandes.Where(x => x.Del == false && x.StatutCmd == false).ToList().Count;
            var cmdeDispo = ctx.Commandes.Where(x => x.Del == false && x.StatutCmd && !ctx.ReceptionCommandes.Select(rc => rc.CommandeId).Contains(x.Id)).ToList().Count;
            var listeCommande = ctx.Commandes.Where(x => x.Del == false && x.PayeClient == false).Union(
                from c in ctx.Commandes
                join ac in ctx.AssuranceCommandes on c.Id equals ac.CommandeId
                join a in ctx.Assurances on ac.AssuranceId equals a.Id
                where c.Del == false && ac.PayeAssur == false
                select c
                ).ToList();

            var listeVente = ctx.Ventes.Where(x => x.Del == false && x.PayeClient == false).Union(
                    from v in ctx.Ventes
                    join av in ctx.AssuranceVentes on v.Id equals av.VenteId
                    join a in ctx.AssuranceVentes on av.AssuranceId equals a.Id
                    where v.Del == false && av.PayeAssur == false
                    select v
                ).ToList();

            float MontantTotal = 0, chargeTotAssur = 0, chargeTotClient = 0;
            if (listeCommande.Count > 0)
            {
                foreach (var c in listeCommande)
                {
                    var cmde = c;
                    float _resteApayerFacture = 0; 
                    float _resteApayerAssur = 0;
                    if (c.AssuranceCommandes.Count > 0)
                    {
                        
                        foreach (var ac in c.AssuranceCommandes)
                        {
                            float factmontantAssur = 0;
                            if (!ac.PayeAssur)
                            {
                                var chargeAssur = ac.MontantAsssur;
                                var mttVerse = c.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.AssuranceId == ac.AssuranceId && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                                factmontantAssur = chargeAssur - mttVerse; 
                            }
                            _resteApayerAssur += factmontantAssur;
                        }
                    }
                    float mttVersecl = 0;
                    float _restApayercl = 0;
                    if (!c.PayeClient)
                    {
                        mttVersecl = c.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                        _restApayercl = c.MontantClient -(mttVersecl + c.ReductionClient);
                    }
                    _resteApayerFacture += _restApayercl + _resteApayerAssur;
                    MontantTotal += _resteApayerFacture;
                    chargeTotAssur += _resteApayerAssur;
                    chargeTotClient += _restApayercl;

                    /*var mttAssurRegler = cmde.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                    var mttClientRegler = cmde.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                    MontantTotal += (cmde.MontantAssur + cmde.MontantClient) - (mttAssurRegler + mttClientRegler + cmde.ReductionClient);*/
                }
            }

            if (listeVente.Count > 0)
            {
                foreach (var v in listeVente)
                {
                    var vte = v;
                    float _resteApayerFacture = 0;
                    float _resteApayerAssur = 0;
                    if (vte.AssuranceVentes.Count > 0)
                    {
                        foreach (var av in vte.AssuranceVentes)
                        {
                            float factmontantAssur = 0;
                            if (!av.PayeAssur)
                            {
                                var chargeAssur = av.MontantAssur;
                                var mttVerseAssur = vte.Payements.Where(x=> x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.AssuranceId == av.AssuranceId && x.MontantEncaisse).ToList().Sum(x=> x.MontantPaye);
                                factmontantAssur = chargeAssur - mttVerseAssur; 
                            }
                            _resteApayerAssur += factmontantAssur;
                        }
                    }
                    float mttVersecl = 0;
                    float _restApayercl = 0;
                    if (!vte.PayeClient)
                    {
                        mttVersecl = vte.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                        _restApayercl += vte.MontantClient - (mttVersecl + vte.ReductionClient);
                    }
                    _resteApayerFacture = _restApayercl + _resteApayerAssur;
                    MontantTotal += _resteApayerFacture;
                    chargeTotAssur += _resteApayerAssur;
                    chargeTotClient += _restApayercl;
                    /*var mttAssurRegler = vte.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                    var mttClientRegler = vte.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                    MontantTotal += (vte.MontantAssurance + vte.MontantClient) - (mttAssurRegler + mttClientRegler + vte.ReductionClient);*/
                }
            }
            DashBoard dash = new DashBoard { Caisse = montantEncaisse, CmdeAttente = cmdeAttente, CmdeDispo = cmdeDispo, CompteASolder = MontantTotal, ChargeAssur = chargeTotAssur, ChargeClient = chargeTotClient };
            return View(dash);
        }

        [Route("repartition/assurance-dette/", Name="_statAssuranceDette")]
        public JsonResult RepartitionDette()
        {
            List<AssuranceRepartitionDetteModel> AssurDette = new List<AssuranceRepartitionDetteModel>();
            List<AssuranceRepartitionDetteModel> AssurDette2 = new List<AssuranceRepartitionDetteModel>();
            var listcmdeAregler = (from c in ctx.Commandes
                                  join ac in ctx.AssuranceCommandes on c.Id equals ac.CommandeId
                                   join a in ctx.Assurances on ac.AssuranceId equals a.Id
                                  where c.Del == false && ac.PayeAssur == false
                                  select c).Distinct().ToList();

            var listvteAregler = (from v in ctx.Ventes
                                  join av in ctx.AssuranceVentes on v.Id equals av.VenteId
                                  join a in ctx.Assurances on av.AssuranceId equals a.Id
                                  where v.Del == false && av.PayeAssur == false
                                  /*group v by a.Id*/ select v).Distinct().ToList();

            if (listcmdeAregler.Count > 0)
            {
                foreach (var cmde in listcmdeAregler)
                {
                    foreach (var item in cmde.AssuranceCommandes)
                    {
                        float mttverse = cmde.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.AssuranceId == item.AssuranceId && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                        float _resteApayer = item.MontantAsssur - mttverse;
                        AssurDette.Add(new AssuranceRepartitionDetteModel { Code = item.Assurance.Code, Id = item.AssuranceId, Montant = _resteApayer, Nom = item.Assurance.Nom, RefFacture = cmde.RefCmd });
                    }

                    /*{
                        var mttRegler = item.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse).ToList().Sum(x=>x.MontantPaye);
                        var mtttResteApayer = item.MontantAssur - mttRegler;
                        _ResteApayer += mtttResteApayer;
                        _assurance = item.Assurance;
                    }
                    AssurDette.Add(new AssuranceRepartitionDetteModel { Code = _assurance.Code.ToUpper(), Id = grpc.Key, Montant = _ResteApayer.ToString("N0", CultureInfo.CurrentCulture), Nom = _assurance.Nom.ToUpper() });*/
                }
            }

            if (listvteAregler.Count > 0)
            {
                foreach (var vte in listvteAregler)
                {

                    foreach (var item in vte.AssuranceVentes)
                    {
                        float mttVerse = vte.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == item.AssuranceId).ToList().Sum(x=> x.MontantPaye);
                        float _resteApayer = item.MontantAssur - mttVerse;
                        AssurDette.Add(new AssuranceRepartitionDetteModel { Code = item.Assurance.Code, Id = item.AssuranceId, Montant = _resteApayer, Nom = item.Assurance.Nom, RefFacture = vte.RefVente });
                    }

                    /*{
                        var mttRegler = item.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                        var mtttResteApayer = item.MontantAssurance - mttRegler;
                        _ResteApayer += mtttResteApayer;
                        _assurance = item.Assurance;
                    }
                    AssurDette2.Add(new AssuranceRepartitionDetteModel { Code = _assurance.Code.ToUpper(), Id = grpv.Key, Montant = _ResteApayer.ToString("N0", CultureInfo.CurrentCulture), Nom = _assurance.Nom.ToUpper() });*/
                }
            }
            var MontantTotal = AssurDette.ToList().Sum(x=> x.Montant);
            if (AssurDette.Count > 0)
            {
                var tmp = AssurDette.GroupBy(x => x.Id).ToList();
                foreach (var item in tmp)
                {
                    var mttotal = item.Sum(x => x.Montant);
                    var taux = ((double)mttotal / MontantTotal) * 100;
                    var a = item.FirstOrDefault();
                    AssurDette2.Add(new AssuranceRepartitionDetteModel { Code = a.Code, Id = a.Id, Montant = mttotal, Nom = a.Nom, Taux = Math.Round(taux,1) });
                }
            }
            /*var AllFacture = AssurDette.Concat(AssurDette2);
            var Facturemtt = AllFacture.GroupBy(x => x.Nom).ToList();
            AssurDette = new List<AssuranceRepartitionDetteModel>();
            if (Facturemtt.Count > 0)
            {
                
                foreach (var grp in Facturemtt)
                {
                    float mtt = 0;
                    AssuranceRepartitionDetteModel elt = new AssuranceRepartitionDetteModel();
                    foreach (var item in grp)
                    {
                        mtt += float.Parse(item.Montant);
                        elt = item;
                    }
                    AssurDette.Add(new AssuranceRepartitionDetteModel { Code = elt.Code, Id = elt.Id, Nom = elt.Nom, Montant = mtt.ToString("N0", CultureInfo.CurrentCulture)});
                }
            }*/
            return Json(AssurDette2, JsonRequestBehavior.AllowGet);
        }
	}
}