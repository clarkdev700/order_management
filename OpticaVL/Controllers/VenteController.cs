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
    public class VenteController : BaseController
    {

        public static List<JournalVenteViewModel> _journalReportVente;
        public static PeriodeRecette _periode;
        //
        // GET: /Vente/
        public ActionResult Index()
        {
            return View();
        }

        [Route("ventes/add", Name="_addVente")]
        public ActionResult Add()
        {
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
            ViewBag.Assurance = new SelectList(ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x => new AssuranceModel { Code = x.Code.ToUpper(), Id = x.Id, Nom = x.Nom.ToUpper() }).ToList(),"Id","Nom");
            //ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
            //ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            return View();
        }

        [Route("ventes/add", Name="_addVentePoste")]
        [HttpPost]
        public JsonResult AddPost(VenteViewModel VteModel)
        {
            Reponse response = new Reponse();
            if (ModelState.IsValid)
            {
                var ch = VteModel.DateVente;
                try
                {
                    var vte = new Vente { DateEnreg = DateTime.Now, DateVente = VteModel.DateVente, NomClient = VteModel.NomClient, PrenomClient = VteModel.PrenomClient, RefVente = "--", MontantClient = VteModel.MontantClient, ReductionClient = VteModel.ReductionClient };
                    if (VteModel.MontantClient == 0 || VteModel.ReductionClient == VteModel.MontantClient) vte.PayeClient = true;
                    //if (VteModel.MontantAssurance == 0) vte.PayeAssurance = true;
                    ctx.Ventes.Add(vte);
                    ctx.SaveChanges();
                    if (VteModel.AN != null && VteModel.AN.Count > 0 && VteModel.AN[0] > 0)
                    {
                        int index = 0;
                        foreach (var item in VteModel.AN)
                        {
                            var mttAssurance = VteModel.AM[index];
                            var statutPaye = mttAssurance == 0 ? true : false;
                            ctx.AssuranceVentes.Add(new AssuranceVente { AssuranceId = item, MontantAssur = mttAssurance, PayeAssur = statutPaye, VenteId = vte.Id });
                            index++;
                        }
                        ctx.SaveChanges();
                    }
                    var _refVente = "REF V" + ch.Year + "/" + ch.Day + ch.Month + vte.Id;
                    vte.RefVente = _refVente;
                    //ligneVente 
                    float montantFacture = 0;
                    foreach (var lv in VteModel.LigneVentes)
                    {
                        ctx.LigneVentes.Add(
                            new LigneVente { ProduitId = lv.prodid, PrixVente = lv.price, QteVente = lv.qte, Rem = lv.Rem, Remdg = lv.RemDg, VenteId = vte.Id }
                            );
                        montantFacture += ((lv.price * lv.qte) - (lv.Rem + lv.RemDg));
                        //update produit stock
                        var produit = ctx.Produits.Find(lv.prodid);
                        try
                        {
                            produit.QteStock -= lv.qte;
                            ctx.SaveChanges();
                        }
                        catch (DataException /* ex */)
                        {
                            //abort all process
                            vte.Del = true;
                            vte.DateDel = DateTime.Now;
                            vte.MotifDel = "Une erreur s'est produit lors de la mise à jour du stock d'un produit" + lv.prodid + "reference: " + lv.refprod + "designation :" + lv.designation;
                            ctx.SaveChanges();
                            var ligneventeSaved = ctx.LigneVentes.Where(x => x.Del == false && x.VenteId == vte.Id).ToList();
                            if (ligneventeSaved.Count > 0)
                            {
                                foreach (var lvsaved in ligneventeSaved)
                                {
                                    var prod = ctx.Produits.Find(lvsaved.ProduitId);
                                    try
                                    {
                                        //update stock if necessary
                                        prod.QteStock += lvsaved.QteVente;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /*ex*/)
                                    {
                                        response.Statut = 500;
                                        response.Message = "Impossible de mettre à jour le stock du produit Id: " + lvsaved.ProduitId;
                                        return Json(response, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                            response.Statut = 500;
                            response.Message = "Impossible de mettre à jour le stock du produit Id: " + lv.prodid;
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }

                    //payement
                    if (VteModel.MontantVerse > 0)
                    {
                        ModeReglement moderegl;
                        //var paye = montantFacture == VteModel.MontantVerse;
                        //vte.PayeClient = montantFacture == VteModel.MontantVerse;
                        Enum.TryParse(VteModel.ModePayement, out moderegl);
                        ctx.Payements.Add(new Payement { MontantPaye = VteModel.MontantVerse, SourcePaye = SourcePaye.Client, DatePaye = VteModel.DateVente, RefVente = vte.Id, ModePaye = moderegl, DateEnreg = DateTime.Now, RefPayement = VteModel.RefPayement });
                        ctx.SaveChanges();
                    }
                    response.Statut = 200;
                }
                catch (Exception ex)
                {
                    response.Statut = 500;
                    response.Message = "Une erreur s'est produite veuillez renseigner tous les champs obligatoires *.";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Route("ventes/{id}", Name="_updateVente")]
        public ActionResult Update(int id) 
        {
            var vente = ctx.Ventes.Find(id);
            if (vente == null)
                return HttpNotFound();
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
            ViewBag.Assurance = new SelectList(ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x => new AssuranceModel { Code = x.Code.ToUpper(), Id = x.Id, Nom = x.Nom.ToUpper() }).ToList(), "Id", "Nom"/*,vente.AssuranceId*/);
            //ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
            //ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            return View(vente);
        }

        [Route("ventes/get-details/{id}", Name="_getVenteDetails")]
        public JsonResult GetVenteDetails(int id)
        {
            var vente = ctx.Ventes.Find(id);
            if (vente == null)
                return Json(new VenteViewModel { }, JsonRequestBehavior.AllowGet);
            var lreglement = ctx.Payements.Where(x => x.Del == false && x.RefVente!= null && x.RefVente == vente.Id && x.MontantEncaisse == true).OrderByDescending(x=>x.Id).ToList();
            var statutReglement = lreglement.Count > 1 ? "POK" : "OK";
            var reglement = lreglement.Take(1).FirstOrDefault();
            var montantverse = reglement == null ? 0 : lreglement.Sum(x => x.MontantPaye);//reglement.MontantPaye;
            var modereglement = reglement != null ? Enum.Format(typeof(ModeReglement), reglement.ModePaye,"g"): "Espece";
            var Refpayement = reglement != null ? reglement.RefPayement: "";
            var lignevente = new List<LigneVenteModel>();
            var AssuranceMontant = vente.AssuranceVentes.Select(x => new AssuranceMontant{ Id = x.Id, Montant = x.MontantAssur, AssuranceId = x.AssuranceId }).OrderBy(x => x.Id).ToList();
            var listelignevente = vente.LigneVentes.Where(x => x.Del == false).ToList();
            foreach (var l in listelignevente) {
                var produit = l.Produit;
                var modelProd = new ProduitModel { Designation = produit.Designation, Categorie = produit.Categorie.Libelle, RefProd = produit.RefProd };
                var _designation = FormatDesignation(modelProd);
                lignevente.Add(new LigneVenteModel{ price = l.PrixVente, prodid = l.ProduitId, qte = l.QteVente, Rem = l.Rem, RemDg = l.Remdg, designation = _designation, refprod = ""/*l.Produit.RefProd*/});
            }
            var vteModel = new VenteViewModel { DateVente = vente.DateVente, NomClient = vente.NomClient, PrenomClient = vente.PrenomClient, ModePayement = modereglement, MontantVerse = montantverse, LigneVentes = lignevente, StatutReglement = statutReglement, RefPayement = Refpayement, AssuranceMontant = AssuranceMontant, MontantClient = vente.MontantClient, ReductionClient = vente.ReductionClient };
            return Json(vteModel, JsonRequestBehavior.AllowGet);
        }

        [Route("ventes/{id}", Name="_updatePosteVente")]
        [HttpPost]
        public JsonResult UpdateVente(int id, VenteViewModel vtemodel)
        {
            Reponse response = new Reponse { Statut = 500 };
            var oldvente = ctx.Ventes.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    //del old lignevente
                    var _oldlgvte = oldvente.LigneVentes.Where(x => x.Del == false && x.VenteId == id).ToList();
                    if (_oldlgvte.Count > 0)
                    {
                        foreach (var _oldlv in _oldlgvte)
                        {
                            _oldlv.Del = true;
                            //update stock
                            var prod = ctx.Produits.Find(_oldlv.ProduitId);
                            try
                            {
                                prod.QteStock += _oldlv.QteVente;
                                ctx.SaveChanges();
                            }
                            catch (DataException /*ex */)
                            {
                                response.Message = "Impossible de mettre à jour e stock du produit Id: " + _oldlv.ProduitId;
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    //Assurance 
                    List<int> oldAssurance = new List<int>();
                    if (oldvente.AssuranceVentes.Count > 0)
                    {
                        foreach (var olditem in oldvente.AssuranceVentes.ToList())
                        {
                            oldAssurance.Add(olditem.AssuranceId);     
                            if ((vtemodel.AN != null && !vtemodel.AN.Contains(olditem.AssuranceId)) || vtemodel.AN == null)
                            {
                                var oldpAssur = oldvente.Payements.Where(x => x.AssuranceId == olditem.AssuranceId && x.Del == false && x.SourcePaye == SourcePaye.Assurance).ToList();
                                if (oldpAssur.Count > 0)
                                {
                                    foreach (var p in oldpAssur)
                                    {
                                        ctx.Payements.Remove(p);
                                        ctx.SaveChanges();
                                    }
                                }
                            }
                            ctx.AssuranceVentes.Remove(olditem);
                            ctx.SaveChanges();
                        }
                    }

                    if (vtemodel.AN != null && vtemodel.AN.Count > 0 && vtemodel.AN[0] > 0)
                    {
                        var index = 0;
                        foreach (var item in vtemodel.AN)
                        {
                            var montant = vtemodel.AM[index];
                            bool statutPaye = false;
                            var mttVerse = oldvente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == item).ToList().Sum(x => x.MontantPaye);
                            statutPaye = montant - mttVerse == 0 ? true : false;
                            ctx.AssuranceVentes.Add(new AssuranceVente { AssuranceId = item, MontantAssur = montant, PayeAssur = statutPaye, VenteId = id });
                            index++;
                        }
                        ctx.SaveChanges();
                    }
                    //add new ligne vente
                    oldvente.NomClient = vtemodel.NomClient;
                    oldvente.PrenomClient = vtemodel.PrenomClient;
                    //oldvente.RefVente = vtemodel.RefPayement;
                    oldvente.DateVente = vtemodel.DateVente;
                    oldvente.MontantClient = vtemodel.MontantClient;
                    oldvente.ReductionClient = vtemodel.ReductionClient;
                    var mttClient = oldvente.MontantClient - oldvente.ReductionClient;
                    /***/
                    foreach (var nlv in vtemodel.LigneVentes)
                    {
                        ctx.LigneVentes.Add(new LigneVente {  ProduitId = nlv.prodid, PrixVente = nlv.price, QteVente = nlv.qte, Rem = nlv.Rem, Remdg = nlv.RemDg, VenteId = id});
                        //update prod
                        var prod = ctx.Produits.Find(nlv.prodid);
                        try
                        {
                            prod.QteStock -= nlv.qte;
                            ctx.SaveChanges();
                        } catch(DataException /*ex*/){
                            response.Message = "Impossible de mettre à jour le produit Id: " + nlv.prodid;
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //ctx.SaveChanges();
                    if (vtemodel.StatutReglement == "OK" && vtemodel.MontantVerse > 0) 
                    {
                        var _oldpayement = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id).OrderByDescending(x => x.Id).FirstOrDefault();
                        ModeReglement _modeReglement;
                        Enum.TryParse(vtemodel.ModePayement, out _modeReglement);
                        if (_oldpayement != null)
                        {
                            //if (_oldpayement.MontantPaye != vtemodel.MontantVerse) _oldpayement.MontantEncaisse = false;
                            _oldpayement.MontantPaye = vtemodel.MontantVerse;                          
                            _oldpayement.ModePaye = _modeReglement;
                            _oldpayement.RefPayement = vtemodel.RefPayement;                          
                        }
                        else
                        {
                            ctx.Payements.Add(new Payement { ModePaye = _modeReglement, RefVente = id, MontantPaye = vtemodel.MontantVerse, SourcePaye = SourcePaye.Client, DateEnreg = DateTime.Now, DatePaye = vtemodel.DateVente, RefPayement = vtemodel.RefPayement });
                        }
                        /*oldvente.PayeClient = mttClient == oldvente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye)? true:false;
                        ctx.SaveChanges();*/
                    }
                    
                    if (mttClient == oldvente.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye)) oldvente.PayeClient = true; else oldvente.PayeClient = false;
                    ctx.SaveChanges();
                    response.Statut = 200;
                    response.Message = "OK";
                }
            }
            catch (DataException /*ex*/)
            {
                response.Message = "Donnée invalide";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Route("ventes/annuler/{id}", Name="_annulerVente")]
        public JsonResult DelVente(int id)
        {
            Reponse _rep = new Reponse{ Statut = 500, Message = "Impossible d'annuler la vente"};
            var vente = ctx.Ventes.Find(id);
            try
            {
                vente.DateDel = DateTime.Now;
                vente.Del = true;
                vente.MotifDel = "";
                if (vente.LigneVentes.Count > 0)
                {
                    foreach (var vl in vente.LigneVentes.Where(x => x.Del == false))
                    {
                        //update produit
                        var produit = ctx.Produits.Find(vl.ProduitId);
                        try
                        {
                            produit.QteStock += vl.QteVente;
                            ctx.SaveChanges();
                        }
                        catch (DataException /*ex*/)
                        {
                            _rep.Message = "Impossible de mettre à jour le stock du produit Id : "+vl.ProduitId;
                            return Json(_rep, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                var payements = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == vente.Id).ToList();
                if (payements.Count > 0)
                {
                    foreach (var p in payements)
                    {
                        p.Del = true;
                        p.DateDel = DateTime.Now;
                        p.MotifDel = "";
                        ctx.SaveChanges();
                    }
                }
                _rep.Statut = 200;
            }
            catch (DataException /*ex*/)
            {
                return Json(_rep, JsonRequestBehavior.AllowGet);
            }
            return Json(_rep,JsonRequestBehavior.AllowGet);
        }


        [Route("ventes/journal-vente/", Name="_journalVente")]
        public ActionResult JournalVente()
        {
            ViewBag.QDate = DateTime.Now;
            return View();
        }

        [Route("ventes/query-journal-vente/{print?}", Name="_QJournaldeVente")]
        public JsonResult JournalVenteQuery(DateTime Deb, DateTime End, string print = null)
        {
            List<JournalVenteViewModel> JventeModel = new List<JournalVenteViewModel>();
            if (ModelState.IsValid)
            {
                var _venteProdGroup = (from lv in ctx.LigneVentes
                                       join p in ctx.Produits on lv.ProduitId equals p.Id
                                       join v in ctx.Ventes on lv.VenteId equals v.Id
                                       where lv.Del == false && v.Del == false && v.DateVente >= Deb && v.DateVente <= End 
                                       group lv by new { p.Designation, v.DateVente }).ToList();
                if (_venteProdGroup.Count > 0)
                {
                    foreach (var vte in _venteProdGroup)
                    {
                        var _Qtevendu = vte.Sum(x => x.QteVente);
                        var _TotalRem = vte.Sum(x => x.Rem + x.Remdg);
                        var _Montant = vte.Sum(x => x.PrixVente * x.QteVente);
                        var prod = vte.FirstOrDefault().Produit;
                        var _marque = (prod != null && prod.Marque != null) ? prod.Marque.Libelle : "Sans marque";
                        var prodModel = new ProduitModel { Designation = prod.Designation, Categorie = prod.Categorie.Libelle, RefProd = prod.RefProd };
                        var _designation = FormatDesignation(prodModel);
                        JventeModel.Add(new JournalVenteViewModel { Designation = Title(_designation), RefProd = ""/*prod.RefProd*/, Date = vte.FirstOrDefault().Vente.DateVente.ToString("dd/MM/yyyy"), Marque = Title(_marque), Montant = _Montant, Numero = 0/*prod.Numero*/, QteVendu = _Qtevendu, TotalRem = _TotalRem, trieDate = vte.FirstOrDefault().Vente.DateVente });
                    }
                }
            }
            var ModelVente = JventeModel.OrderBy(x=>x.trieDate).ToList();
            if (!string.IsNullOrEmpty(print))
            {
                _journalReportVente = ModelVente;
                _periode = new PeriodeRecette { Deb = Deb.ToString("dd/MM/yyyy"), End = End.ToString("dd/MM/yyyy") };
                var url = Url.RouteUrl("_admistrationVenteReport");
                var ConvertStatut = generateReportFile(url, "venteReport.pdf", "footer");
                return Json(new { statut = ConvertStatut }, JsonRequestBehavior.AllowGet);
            }
            return Json(ModelVente, JsonRequestBehavior.AllowGet);
        }

        [Route("vente/operation-ventes/", Name="_operationVentes")]
        public ActionResult OperationsVente()
        {
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [Route("vente/operation-vente/query/", Name="_operationQvente")]
        public JsonResult QOperationVente(DateTime Deb, DateTime End) 
        {
            var listeVente = ctx.Ventes.Where(x => x.DateVente >= Deb && x.DateVente <= End).OrderBy(x=>x.Id).ToList();
            List<OpVenteModel> lOpVenteModel = new List<OpVenteModel>();
            if (listeVente.Count > 0)
            {
                foreach (var v in listeVente)
                {
                    //
                    var lignesVente = v.LigneVentes.Where(x => x.Del == false).ToList();
                    float mtt = 0;
                    List<DetailsCommande> ldv = new List<DetailsCommande>();
                    List<int> listeAssurance = v.AssuranceVentes.Select(x => x.AssuranceId).ToList();
                    foreach (var lv in lignesVente)
                    {
                        var prod = lv.Produit;
                        var produitModel = new ProduitModel { Designation = prod.Designation, RefProd = prod.RefProd, Categorie = prod.Categorie.Libelle };
                        var _designation = FormatDesignation(produitModel);
                        var _marque = prod.Marque != null ? prod.Marque.Libelle : "Sans marque";
                        var remtotal = lv.Rem + lv.Remdg;
                        var montant = lv.QteVente * lv.PrixVente;
                        mtt +=  (montant - remtotal);
                        ldv.Add(new DetailsCommande { Designation = Title(_designation), Marque = Title(_marque), Qte = lv.QteVente });
                    }
                    lOpVenteModel.Add(new OpVenteModel { Id = v.Id, Nom = v.NomClient.ToUpper(), Prenom = Title(v.PrenomClient), RefVente = v.RefVente, StatutDel = v.Del, Montant = mtt, DetailsVente = ldv, Date = v.DateVente.ToString("dd/MM/yyyy"), Assurances = listeAssurance });
                }
            }
            return Json(lOpVenteModel, JsonRequestBehavior.AllowGet);
        }

        [Route("vente/generate-facture-vente/{id}/{assurance}/", Name="_generateFactureVente")]
        public ActionResult GenerateFactureVente(int id, int assurance)
        {
            var url = Url.RouteUrl("_factureAssuranceVente", new { id = id, assurance = assurance });
            var ConvertStatut = generateReportFile(url, "Facturevente.pdf");
            return Json(new { statut = ConvertStatut }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("vente/facture-assurance/{id}/{assurance}/", Name="_factureAssuranceVente")]
        public ActionResult FatureVente(int id, int assurance)  //vente id
        {

            Vente vente = ctx.Ventes.Find(id);
            if (vente == null)
                return HttpNotFound();
            List<FactureVenteLigneModel> LigneVentes = new List<FactureVenteLigneModel>();
            FactureVenteModel FactureVenteModel = new FactureVenteModel();
            var _assurance = ctx.Assurances.Find(assurance);
            var nomAssurance = _assurance != null ? _assurance.Nom.ToUpper(): null;
            if (vente.LigneVentes.Count > 0)
            {
                float _mttTotal = 0;
                float _remTotal = 0;
                foreach (var lg in vente.LigneVentes.Where(x => x.Del == false).ToList())
                {
                    var prod = lg.Produit;
                    var produitModel = new ProduitModel { Categorie = prod.Categorie.Libelle, RefProd = prod.RefProd, Designation = prod.Designation };
                    var _designation = FormatDesignation(produitModel);
                    var _remise = lg.Rem + lg.Remdg;
                    var _sstotal = (lg.PrixVente * lg.QteVente) - _remise;
                    _mttTotal += _sstotal;
                    _remTotal += _remise;
                    LigneVentes.Add(new FactureVenteLigneModel { Designation = _designation, Pu = lg.PrixVente, Qte = lg.QteVente, Remise = _remise, SousTotal = _sstotal });
                }

                var prenom = vente.PrenomClient != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vente.PrenomClient) : vente.PrenomClient;
                var identite = vente.NomClient.ToUpper() + " "+prenom;
                FactureVenteModel = new FactureVenteModel { IdentiteClient = identite, MontantFacture = _mttTotal, NumeroRecu = id, FactureVenteLigneModel = LigneVentes, NomAssurance = nomAssurance};
            }
            return View(FactureVenteModel);
        }

        [AllowAnonymous]
        [Route("administration-journal/vente-report/", Name="_admistrationVenteReport")]
        public ActionResult VenteReport()
        {
            ViewBag.Periode = _periode;
            return View(_journalReportVente);
        }
	}
}