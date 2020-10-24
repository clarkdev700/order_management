using OpticaVL.Models;
using OpticaVL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class RecuController : BaseController
    {
        //
        // GET: /Recu/
        [Route("generate-recu-file/{id}", Name="_generateRecuPaiement")]
        public ActionResult Index(int id)
        {
            var url = Url.RouteUrl("_recupaiementCommande", new { id = id });
            var ConvertStatut = generateReportFile(url,"recuPaiement.pdf");
            return Json(new { statut = ConvertStatut}, JsonRequestBehavior.AllowGet);
        }

        [Route("generate-recu-vente/{id}", Name="_generateRecuVente")]
        public JsonResult generateRecuVente(int id)
        {
            var url = Url.RouteUrl("_recupaiementVente", new { id = id });
            var ConvertStatut = generateReportFile(url, "recuPaiementvente.pdf");
            return Json(new { statut = ConvertStatut }, JsonRequestBehavior.AllowGet);
        }

        [Route("recu/{id}/", Name="_recuFacture")] //id du paiement
        public ActionResult GenerateRecu(int id)
        {
            RecuViewModel recuvm;
            List<LigneBonDeCommande> ligneFacture = new List<LigneBonDeCommande>();
            List<HReglement> HReglements = new List<HReglement>();
            string _view = "";

            var payement = ctx.Payements.Find(id);
            if (payement == null)
                return HttpNotFound();
            if (payement.Commande != null)
            {
                //commande 
                var _cmd = payement.Commande;
                try
                {
                    var _mttclient = _cmd.MontantClient;
                    var _mttassur = _cmd.AssuranceCommandes.ToList().Sum(x=> x.MontantAsssur);
                    var payements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == _cmd.Id && x.SourcePaye == Models.SourcePaye.Client && x.DatePaye <= payement.DatePaye).OrderByDescending(x => x.Id).ToList();
                    float _mttVerserclient = 0;
                    var _numero = GetNumeroRecu(payement);
                    if (payements.Count > 0)
                    {
                        _mttVerserclient = payements.Sum(x => x.MontantPaye);
                        foreach (var p in payements)
                        {
                            var modeRegl = Enum.Format(typeof(ModeReglement), p.ModePaye, "g").Substring(0, 2).ToUpper();
                            HReglements.Add(new HReglement { Date = p.DatePaye.ToString("dd/MM/yyyy"), Montant = p.MontantPaye, Mregl = modeRegl });
                        }
                        
                    }
                    
                    var _resteApayer = _mttclient - _mttVerserclient;
                    /*foreach (var lcmd in _cmd.LigneCommandes.Where(x => x.Del == false).ToList())
                    {
                        var prod = lcmd.Produit;
                        var _marque = prod.Marque != null ? prod.Marque.Libelle : "Sans marque";
                        var _mttligne = (lcmd.PrixCmd * lcmd.QteCmd) - (lcmd.RemDG + lcmd.Rem);
                        var _designation = _getDesignation(prod);
                        ligneFacture.Add(new LigneBonDeCommande { Designation = _designation, Prix = lcmd.PrixCmd, Qte = lcmd.QteCmd, Rem = lcmd.Rem + lcmd.RemDG, Montant = _mttligne });
                    }*/

                    recuvm = new RecuViewModel { Nom = _cmd.Client.Nom, Prenom = _cmd.Client.Prenom, MttAssur = _mttassur, MttClient = _mttclient, MttVerse = _mttVerserclient, MttResteApayer = _resteApayer, Ref = _cmd.Id.ToString(), NumeroRecu = _numero, LigneFacture = ligneFacture, HReglements = HReglements };
                    _view = "RecuCommande";
                    return View(_view, recuvm);
                }
                catch (DataException /* ex*/)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                //vente
                var _vente = payement.Vente;
                try
                {
                    var _mttclient = _vente.LigneVentes.Where(x => x.Del == false).Sum(x => (x.PrixVente * x.QteVente) - (x.Rem + x.Remdg));
                    var Payements = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == _vente.Id).OrderByDescending(x => x.Id).ToList();
                    float _mttverse = 0;
                    if (Payements.Count > 0)
                    {
                        _mttverse = Payements.Sum(x => x.MontantPaye);
                        foreach (var p in Payements)
                        {
                            var _modeRegl = Enum.Format(typeof(ModeReglement), p.ModePaye, "g").Substring(0, 2).ToUpper();
                            HReglements.Add(new HReglement { Date = p.DatePaye.ToString("dd/MM/yyyy"), Montant = p.MontantPaye, Mregl = _modeRegl });
                        }
                    }
                    var _resteApayer = _mttclient - _mttverse;
                    var _numeroRecu = GetNumeroRecu(payement);
                    foreach (var lv in _vente.LigneVentes.Where(x => x.Del == false).ToList())
                    {
                        var prod = lv.Produit;
                        var _marque = prod.Marque != null ? prod.Marque.Libelle : "Sans marque";
                        var _mttLigne = (lv.QteVente * lv.PrixVente) - (lv.Remdg + lv.Rem);
                        var _designation = _getDesignation(prod);
                        ligneFacture.Add(new LigneBonDeCommande { Designation = _designation, Prix = lv.PrixVente, Qte = lv.QteVente, Rem = lv.Rem + lv.Remdg, Montant = _mttLigne });
                    }
                    recuvm = new RecuViewModel { Nom = _vente.NomClient, Prenom = _vente.PrenomClient, NumeroRecu = _numeroRecu, Ref = _vente.RefVente, MttAssur = 0, MttClient = _mttclient, MttVerse = _mttverse, MttResteApayer = _resteApayer, LigneFacture = ligneFacture };
                    _view = "GenerateRecu";
                    return View(_view, recuvm);
                }
                catch (DataException /* ex */)
                {
                    return HttpNotFound();
                }
            }
            /*if (type == "vente")
            {
                var _vente = ctx.Ventes.Find(id);
                if (_vente == null)
                    return HttpNotFound(); 
                var _mttclient = _vente.LigneVentes.Where(x => x.Del == false).Sum(x=> (x.PrixVente * x.QteVente) - (x.Rem + x.Remdg)); 
                var Payements = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id).OrderByDescending(x => x.Id).ToList();
                float _mttverse = 0;
                if (Payements.Count > 0)
                {
                    _mttverse = Payements.Sum(x => x.MontantPaye);
                    foreach (var p in Payements)
                    {
                        var _modeRegl = Enum.Format(typeof(ModeReglement),p.ModePaye,"g").Substring(0,2).ToUpper();
                        HReglements.Add(new HReglement { Date = p.DatePaye.ToString("dd/MM/yyyy"), Montant = p.MontantPaye, Mregl = _modeRegl});
                    }
                }
                var _resteApayer = _mttclient - _mttverse;
                var _numeroRecu = GetNumeroRecu(Payements.FirstOrDefault()) ;
                foreach(var lv in _vente.LigneVentes.Where(x => x.Del == false).ToList()) 
                {
                    var prod = lv.Produit;
                    var _marque = prod.Marque != null ? prod.Marque.Libelle : "Sans marque";
                    var _mttLigne = (lv.QteVente * lv.PrixVente) - (lv.Remdg + lv.Rem);
                    var _designation = _getDesignation(prod);
                    ligneFacture.Add(new LigneBonDeCommande{ Designation = _designation, Prix = lv.PrixVente, Qte = lv.QteVente,  Rem = lv.Rem + lv.Remdg, Montant = _mttLigne});
                }
                 recuvm = new RecuViewModel{ Nom = _vente.NomClient, Prenom = _vente.PrenomClient, NumeroRecu = _numeroRecu, Ref = _vente.RefVente, MttAssur = 0, MttClient = _mttclient, MttVerse = _mttverse, MttResteApayer = _resteApayer, LigneFacture = ligneFacture};
                 _view = "GenerateRecu";
                 return View(_view, recuvm);
            }
            else
            {
                var _cmd = ctx.Commandes.Find(id);
                if (_cmd == null)
                    return HttpNotFound();
                var _mttclient = _cmd.MontantClient;
                var _mttassur = _cmd.MontantAssur;
                var payements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id && x.SourcePaye == Models.SourcePaye.Client).OrderByDescending(x => x.Id).ToList();
                float _mttVerserclient = 0;
                string _numero = "";
                if (payements.Count > 0) 
                {
                    _mttVerserclient = payements.Sum(x => x.MontantPaye);
                    foreach(var p in payements) {
                        var modeRegl = Enum.Format(typeof(ModeReglement), p.ModePaye, "g").Substring(0,2).ToUpper();
                        HReglements.Add( new HReglement{ Date = p.DatePaye.ToString("dd/MM/yyyy"), Montant = p.MontantPaye, Mregl = modeRegl });
                    }
                   _numero = GetNumeroRecu(payements.FirstOrDefault());
                }
                else
                {
                    return HttpNotFound();
                }
                var _resteApayer = _mttclient - _mttVerserclient;
                foreach(var lcmd in _cmd.LigneCommandes.Where(x => x.Del == false).ToList()) 
                {
                    var prod = lcmd.Produit;
                    var _marque = prod.Marque != null ? prod.Marque.Libelle: "Sans marque";
                    var _mttligne = (lcmd.PrixCmd * lcmd.QteCmd) - (lcmd.RemDG + lcmd.Rem);
                    var _designation = _getDesignation(prod);
                    ligneFacture.Add(new LigneBonDeCommande{ Designation = _designation, Prix = lcmd.PrixCmd, Qte = lcmd.QteCmd, Rem = lcmd.Rem + lcmd.RemDG, Montant = _mttligne});
                }
                
                recuvm = new RecuViewModel { Nom = _cmd.Client.Nom, Prenom = _cmd.Client.Prenom, MttAssur = _mttassur, MttClient = _mttclient, MttVerse = _mttVerserclient, MttResteApayer = _resteApayer, Ref =_cmd.RefCmd, NumeroRecu = _numero, LigneFacture = ligneFacture, HReglements = HReglements  };
                _view = "RecuCommande";
                return View(_view,recuvm);
            }*/
            
        }

        [AllowAnonymous]
        [Route("recu-paiement/commande/{id}/", Name="_recupaiementCommande")]
        public ActionResult RecuPaiementCommmande(int id)
        {
            var paiement = ctx.Payements.Find(id);
            if (paiement == null)
                return HttpNotFound();
            var commande = paiement.Commande;
            var client = commande.Client;
            var dteVers = paiement.DatePaye;
            var Hpaiements = commande.Payements.Where(x => x.Del == false && x.DatePaye <= dteVers && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse && x.Id != id).ToList();
            List<HistoriquePaiement> _historique = new List<HistoriquePaiement>();
            var _mttRestant = commande.MontantClient;
            float totalVers = 0;
            if (Hpaiements.Count > 0)
            {
                foreach (var p in Hpaiements)
                {
                    totalVers += p.MontantPaye;
                    var _modePaiement = Enum.Format(typeof(ModeReglement),p.ModePaye, "g");
                    _historique.Add(new HistoriquePaiement { Date = p.DatePaye.ToString("dd/MM/yyyy"), ModePaiement = _modePaiement, Montant = p.MontantPaye.ToString("N0", CultureInfo.CurrentCulture) });
                }
            }
            totalVers += paiement.MontantPaye;
            _mttRestant -= (totalVers + commande.ReductionClient);

            var mttR = _mttRestant == 0 ? "Facture Soldée" : _mttRestant.ToString("N0", CultureInfo.CurrentCulture);
            var mttAssur = commande.AssuranceCommandes.ToList().Sum(x => x.MontantAsssur);
            var montantTotalFacture = mttAssur + commande.MontantClient;
            ModelRecuReglementCommande modelReglement = new ModelRecuReglementCommande { Hpaiement = _historique, Nom = client.Nom.ToUpper(), Prenom = Title(client.Prenom), RefCommande = commande.Id.ToString(), MontantVerse = paiement.MontantPaye.ToString("N0", CultureInfo.CurrentCulture), MontantTotalVerse = totalVers.ToString("N0", CultureInfo.CurrentCulture), ResteApayer = mttR, NumeroRecu = paiement.Id, MontantClient = commande.MontantClient.ToString("N0", CultureInfo.CurrentCulture), MontantAssurance = mttAssur.ToString("N0", CultureInfo.CurrentCulture), MontantTotal = montantTotalFacture.ToString("N0", CultureInfo.CurrentCulture), NomCaissier = paiement.User, ReductionClient = commande.ReductionClient };
            return View(modelReglement);
        }

        [AllowAnonymous]
        [Route("recu-paiement/vente/{id}/", Name="_recupaiementVente")]
        public ActionResult RecuPaiementVente(int id)
        {
            Payement paiement = ctx.Payements.Find(id);
            //var vente = ctx.Ventes.Find(id);
            if (paiement == null)
                return HttpNotFound();
            var vente = paiement.Vente;
            //List<ligneVente> LigneVentes = new List<ligneVente>();
            ModelVenteRecu mdlVente = new ModelVenteRecu();
            //int _numRecu = id;
           // float _mttVerse = 0;
            
            /*if (paiement != null)
            {
                _numRecu = paiement.Id;
                _mttVerse = paiement.MontantPaye;
            }*/
            float _mttTotal = vente.AssuranceVentes.ToList().Sum(x=> x.MontantAssur) + vente.MontantClient;
            //float _remTotal = 0;

            if (vente.LigneVentes.Count > 0)
            {
               // float _mttTotal = 0;
               // float _remTotal = 0;
                /*foreach (var lg in vente.LigneVentes.Where(x=>x.Del == false).ToList())
                {
                    var prod = lg.Produit;
                    var produitModel = new ProduitModel { Categorie = prod.Categorie.Libelle, RefProd = prod.RefProd, Designation = prod.Designation };
                    var _designation = FormatDesignation(produitModel);
                    var _remise = lg.Rem + lg.Remdg;
                    var _sstotal = (lg.PrixVente * lg.QteVente) - _remise;
                    _mttTotal += _sstotal;
                    _remTotal += _remise;
                    LigneVentes.Add(new ligneVente { Designation = _designation, Prix = lg.PrixVente, Qte = lg.QteVente, Remise = _remise, ssTotal = _sstotal});
                }*/
                /*float mttVerse = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == vente.Id && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                var resteApayer = vente.MontantClient - (mttVerse + vente.ReductionClient);
                List<HReglement> listeReglement = new List<HReglement>();
                listeReglement = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == vente.Id && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse && x.Id != id).OrderBy(x=>x.Id).Select(x => new HReglement { Date = x.DatePaye.ToString("dd/MM/yyyy"), Montant = x.MontantPaye, Mregl = x.ModePaye.ToString().Substring(0,3) }).ToList();
                var prenom = vente.PrenomClient != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vente.PrenomClient) : vente.PrenomClient;
                 mdlVente = new ModelVenteRecu { Nom = vente.NomClient.ToUpper(), Prenom = prenom, MontantTotal = _mttTotal, MontantVerse = _mttVerse, NumRecu = id, ChargeAssurance = vente.MontantAssurance, ChargeClient = vente.MontantClient, ResteApayerClient = resteApayer, Reglements = listeReglement, ReductionClient = vente.ReductionClient, ReferenceVente = id };*/
            }
            float mttVerse = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == vente.Id && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
            var resteApayer = vente.MontantClient - (mttVerse + vente.ReductionClient);
            string _reste = resteApayer > 0 ? resteApayer.ToString("N0", CultureInfo.CurrentCulture) : "Facture soldée";
            List<HReglement> listeReglement = new List<HReglement>();
            listeReglement = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == vente.Id && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse && x.Id != id).OrderBy(x => x.Id).Select(x => new HReglement { Date = x.DatePaye.ToString("dd/MM/yyyy"), Montant = x.MontantPaye, Mregl = x.ModePaye.ToString().Substring(0, 3) }).ToList();
            var prenom = vente.PrenomClient != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vente.PrenomClient) : vente.PrenomClient;
            var chargeAssur = vente.AssuranceVentes.ToList().Sum(x => x.MontantAssur);
            mdlVente = new ModelVenteRecu { Nom = vente.NomClient.ToUpper(), Prenom = prenom, MontantTotal = _mttTotal, MontantVerse = paiement.MontantPaye, NumRecu = id, ChargeAssurance = chargeAssur, ChargeClient = vente.MontantClient, ResteApayerClient = _reste, Reglements = listeReglement, ReductionClient = vente.ReductionClient, ReferenceVente = id, MontanTotalVerse = mttVerse };
            return View("RecuPaiementVente2", mdlVente);
        }
	}
}