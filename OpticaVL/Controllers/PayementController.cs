using OpticaVL.Models;
using OpticaVL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
   [Authorize]
    public class PayementController : BaseController
    {
        public static List<JournalCaisse> _cReport;
        public static PeriodeRecette _periode;
        //
        // GET: /Payement/
        [Route("test/facturation/")]
        public ActionResult Index()
        {
            return View("~/Views/Shared/DocEntete.cshtml");
        }

        [Route("fiche-payement/{type}/{id}/{source?}/", Name = "_addPayement")] // id de vente ou commande
        public ActionResult Add(int id, string type, int? source = null)
        {
            var sourcepaye = source != null ? SourcePaye.Assurance : SourcePaye.Client; 
            float _montantAssurance = 0;
            float _montantClient = 0;
            float _resteApayer = 0;
            float _totalverser = 0;
            //float _montantTotalApayer = 0;
            ReglementViewModel ReglModel;// = new ReglementViewModel();
            if (type == "vente")
            {
                
               var _vente = ctx.Ventes.Find(id);
                if (_vente == null)
                    return HttpNotFound();
                List<LigneFacture>ligneFactures = new List<LigneFacture>(); 
                if (_vente.LigneVentes.Count > 0)
                {
                    /*foreach (var lv in _vente.LigneVentes.Where(x => x.Del == false).ToList())
                    {
                        var _montantligne = lv.PrixVente * lv.QteVente;
                        _montantTotalApayer += (_montantligne - (lv.Remdg + lv.Rem));
                        var prod = lv.Produit;
                        var produitModel = new ProduitModel { Verre = prod.ModelVerre, Monture = prod.ModelMonture, Divers = prod.ModelDivers, Designation = prod.Designation };
                        var _designation = FormatDesignation(produitModel);
                        ligneFactures.Add(new LigneFacture { RefProd = _designation, Pu = lv.PrixVente, Qte = lv.QteVente, Rem = lv.Rem, RemDg = lv.Remdg, MontantLigne = _montantligne });
                    }*/
                }
                List<HistoriqueReglement> HistoriqueRegl = new List<HistoriqueReglement>();
                List<Payement> Payements = source != null ? ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id /*&& x.MontantEncaisse*/ && x.SourcePaye == sourcepaye && x.AssuranceId == source).ToList() : ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id /*&& x.MontantEncaisse*/ && x.SourcePaye == sourcepaye).ToList();
                if (Payements.Count > 0)
                {
                    foreach (var p in Payements)
                    {
                        HistoriqueRegl.Add(new HistoriqueReglement { Id = p.Id, DatePayement = p.DatePaye.ToString("yyyy-MM-dd"), MontantVerse = p.MontantPaye, RefPayement = p.RefPayement, ModeReglement = Enum.Format(typeof(ModeReglement),p.ModePaye,"g") });
                         if (p.MontantEncaisse) _totalverser += p.MontantPaye; 
                    }
                }
                //_resteApayer = _montantTotalApayer - _totalverser;
                //_montantClient = _montantTotalApayer;
                _montantClient = _vente.MontantClient;
                _montantAssurance = source != null?_vente.AssuranceVentes.Where(x=>x.AssuranceId == source.Value).Select(x=>x.MontantAssur).FirstOrDefault() : 0;
                var _origine = source != null ? "assurance":"client";
                _resteApayer = _origine == "client" ? _montantClient - (_totalverser + _vente.ReductionClient) : _montantAssurance - _totalverser;
                ReglModel = new ReglementViewModel { NomClient = _vente.NomClient, PrenomClient = _vente.PrenomClient, MontantAssurance = _montantAssurance, MontantClient = _montantClient, ResteApayerClient = _resteApayer, HistoriqueReglement = HistoriqueRegl, LigneFactures = ligneFactures, Payement = new Payement(), Source = source, Type =  type, TypeId = id, Contact = "", Contact2 = "", Email = "", Reference = _vente.RefVente};
            }
            else
            { 
                // cas d'une commande
                var _commande = ctx.Commandes.Find(id);
                if (_commande == null)
                    return HttpNotFound();
                List<LigneFacture> ligneFacture = new List<LigneFacture>();
                /*if (_commande.LigneCommandes.Count > 0)
                {
                    _montantTotalApayer = 0;
                    foreach (var lc in _commande.LigneCommandes.Where(x=>x.Del == false).ToList())
                    {
                        var _montantlignecmd = lc.PrixCmd * lc.QteCmd;
                        _montantTotalApayer += (_montantlignecmd - (lc.RemDG + lc.Rem));
                        var prod = lc.Produit;
                        var produitModel = new ProduitModel { Designation = prod.Designation, Divers = prod.ModelDivers, Monture = prod.ModelMonture, Verre = prod.ModelVerre };
                        var _designation = FormatDesignation(produitModel);
                        ligneFacture.Add(new LigneFacture {  RefProd = _designation/*lc.Produit.RefProd, Pu = lc.PrixCmd, Qte = lc.QteCmd, Rem = lc.Rem, RemDg = lc.RemDG, MontantLigne = _montantlignecmd});
                    }
                }*/

                List<HistoriqueReglement> HistoriqueRegl = new List<HistoriqueReglement>();
                List<Payement> Payements = new List<Payement>();
                if (source == null)
                {
                   Payements  = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id && x.SourcePaye == sourcepaye /*&& x.MontantEncaisse*/).ToList();
                }
                else
                {
                    Payements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id && x.SourcePaye == sourcepaye && x.AssuranceId == source /*&& x.MontantEncaisse*/).ToList();
                }
                
                if (Payements.Count > 0)
                {
                    _totalverser = 0;
                    foreach (var p in Payements)
                    {
                        HistoriqueRegl.Add(new HistoriqueReglement {  Id = p.Id, DatePayement = p.DatePaye.ToString("yyyy-MM-dd"), MontantVerse = p.MontantPaye, RefPayement = p.RefPayement, ModeReglement = Enum.Format(typeof(ModeReglement), p.ModePaye,"g"), AssuranceId = p.AssuranceId});
                        if (p.MontantEncaisse) _totalverser += p.MontantPaye;
                    }
                }
                _montantClient = _commande.MontantClient;
                _montantAssurance = source != null ? _commande.AssuranceCommandes.Where(x=> x.AssuranceId == source).Select(x=>x.MontantAsssur).FirstOrDefault():0;
                var _origine = source == null ? "client":"assurance";
                _resteApayer = _origine == "client" ? _montantClient - (_totalverser + _commande.ReductionClient ): _montantAssurance - _totalverser;
                ReglModel = new ReglementViewModel { NomClient = _commande.Client.Nom, PrenomClient = _commande.Client.Prenom, MontantAssurance = _montantAssurance, MontantClient = _montantClient, ResteApayerClient = _resteApayer, LigneFactures = ligneFacture, HistoriqueReglement = HistoriqueRegl, Payement = new Payement(), Source = source, Type = type, TypeId = id, Contact = _commande.Client.Contact, Contact2 = _commande.Client.Contact2, Email = _commande.Client.Adresse, Reference = _commande.RefCmd };
            }
            return View(ReglModel);
        }

        [Route("fiche-payement/{type}/{id}/{source?}/", Name="_addPostPayement")]
        [HttpPost]
        public ActionResult AddPost(int id, string type, int? source, Payement p) 
        {
            if (ModelState.IsValid)
            {
                //Payement p = Reglementvm.Payement;
                SourcePaye _sourcepaye = source == null ? SourcePaye.Client:SourcePaye.Assurance;
                Payement payement = new Payement { DatePaye = p.DatePaye, ModePaye = p.ModePaye, MontantPaye = p.MontantPaye, RefPayement = p.RefPayement, SourcePaye = _sourcepaye, DateEnreg = DateTime.Now, AssuranceId = source};
                if (type == "vente")
                {
                    payement.RefVente = id;
                }
                else
                {
                    payement.RefCmd = id;
                }
                ctx.Payements.Add(payement);
                ctx.SaveChanges();
            }
            return RedirectToRoute("_addPayement", new { type=type, id = id, source = source});
        }

        [Route("fiche-payement/{id}", Name="_updatePayement")]
        [HttpPost]
        public ActionResult Update(int id, Payement p)
        {
            var payement = ctx.Payements.Find(id);
            /*if (payement == null)
                return HttpNotFound();*/
            try
            {
                payement.MontantPaye = p.MontantPaye;
                payement.ModePaye = p.ModePaye;
                payement.RefPayement = p.RefPayement;
                payement.DatePaye = p.DatePaye;
                payement.MontantEncaisse = false;  
                ctx.SaveChanges();
            }
            catch (DataException /*ex */) 
            {
                //redirect to page principal kelkonque???? 
            }
            string _type;
            int? _id; // id commande ou vente
            int? _source = payement.SourcePaye != SourcePaye.Client ? payement.AssuranceId : null;            
            if (payement.RefCmd == null)
            {
                //cas d"une vente 
                _id = payement.RefVente;
                _type = "vente";
                var vente = payement.Vente;
                if (_source == null)
                {
                    var mttR = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + vente.ReductionClient;
                    vente.PayeClient = vente.MontantClient == mttR ? true : false;
                }
                else
                {
                    var mttR = vente.Payements.Where(x => x.Del == false && x.RefVente != null && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == _source).ToList().Sum(x=> x.MontantPaye);
                    var assurvente = vente.AssuranceVentes.Where(x => x.AssuranceId == _source).FirstOrDefault();
                    if (assurvente != null) assurvente.PayeAssur = assurvente.MontantAssur == mttR ? true : false;
                }
                ctx.SaveChanges(); 

            }
            else
            {
                _id = payement.RefCmd;
                _type = "commande";
                var commande = payement.Commande;
                if (_source == null)
                {
                    var mttR = commande.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.RefCmd != null && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye) + commande.ReductionClient;
                    commande.PayeClient = commande.MontantClient == mttR ? true : false;
                }
                else
                {
                    var mttR = commande.Payements.Where(x => x.Del == false && x.AssuranceId == _source && x.SourcePaye == SourcePaye.Assurance && x.RefCmd != null && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                    var assurcmd = commande.AssuranceCommandes.Where(x => x.AssuranceId == _source).FirstOrDefault();
                    if (assurcmd != null) assurcmd.PayeAssur = assurcmd.MontantAsssur == mttR ? true : false;
                }
                ctx.SaveChanges();
            }
            return RedirectToRoute("_addPayement", new { id = _id, type = _type, source = _source });
        }

        [Route("payements/{id}", Name="_getPayement")]
        public JsonResult Details(int id)
        {
            
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [Route("payements/{type}/{id}/{source?}/", Name="_historiquePayement")] // id de commande ou vente
        public JsonResult historiqueReglement(string type, int id, int? source = null)
        {
            SourcePaye _sourcepaye = source == null? SourcePaye.Client : SourcePaye.Assurance;
            List<HistoriqueReglement> historiqueRegl = new List<HistoriqueReglement>();
            List<Payement> paiements = new List<Payement>();
            if (type == "vente")
            {
                
                if (source == null)
                {
                   paiements = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id && x.SourcePaye == _sourcepaye /*&& x.MontantEncaisse*/).ToList();
                }
                else
                {
                   paiements = ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == id && x.SourcePaye == _sourcepaye && x.AssuranceId == source).ToList();
                }
                
                foreach (var item in paiements)
                {
                    historiqueRegl.Add(new HistoriqueReglement { Id = item.Id, RefPayement = item.RefPayement, MontantVerse = item.MontantPaye, ModeReglement = Enum.Format(typeof(ModeReglement), item.ModePaye,"g"), DatePayement = item.DatePaye.ToString("yyyy-MM-dd"), AssuranceId = item.AssuranceId });
                }
            }
            else
            {
                if (source == null)
                {
                    paiements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id && x.SourcePaye == _sourcepaye /*&& x.MontantEncaisse*/).ToList();
                }
                else
                {
                    paiements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id && x.SourcePaye == _sourcepaye && x.AssuranceId == source).ToList();
                }
                
                foreach (var item in paiements)
                {
                    historiqueRegl.Add(new HistoriqueReglement { Id = item.Id, RefPayement = item.RefPayement, MontantVerse = item.MontantPaye, ModeReglement = Enum.Format(typeof(ModeReglement), item.ModePaye, "g"), DatePayement = item.DatePaye.ToString("yyyy-MM-dd"), AssuranceId = item.AssuranceId });
                }
            }
            return Json(historiqueRegl, JsonRequestBehavior.AllowGet);
        }

        [Route("payements/liste-reglement/", Name="_listeReglement")]
        public JsonResult ListeReglement(QReglementModel QRModel, string _name)
        {
            List<Payement> payements  = new List<Payement> ();
            List<ValidationCaisseModel> VlCaisseModel = new List<ValidationCaisseModel>();
            if (QRModel.Deb != null && QRModel.End != null && !string.IsNullOrEmpty(QRModel.Qtype)) 
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    _name = _name.Trim().ToUpper();
                    payements = (from c in ctx.Commandes
                                 join p in ctx.Payements on c.Id equals p.RefCmd
                                 where (c.Client.Nom.ToUpper() == _name || c.Client.Nom.ToUpper().StartsWith(_name) || c.Client.Prenom.ToUpper().StartsWith(_name) || c.Client.Prenom.ToUpper() == _name) && c.Del == false && p.Del == false
                                 select p).ToList().Union(
                                from v in ctx.Ventes join p in ctx.Payements on v.Id equals p.RefVente where (v.NomClient.ToUpper() == _name || v.NomClient.ToUpper().StartsWith(_name) || v.PrenomClient.ToUpper() == _name || v.PrenomClient.ToUpper().StartsWith(_name)) && v.Del == false && p.Del == false select p
                                ).ToList();
                }
                else
                {
                    switch (QRModel.Qtype)
                    {
                        case "2":
                            payements = ctx.Payements.Where(x => x.Del == false && x.DatePaye >= QRModel.Deb.Value && x.DatePaye <= QRModel.End.Value && x.MontantEncaisse).ToList();
                            break;
                        case "1":
                            payements = ctx.Payements.Where(x => x.Del == false && x.DatePaye >= QRModel.Deb.Value && x.DatePaye <= QRModel.End.Value && x.MontantEncaisse == false).ToList();
                            break;
                        case "3":
                            payements = ctx.Payements.Where(x => x.Del == false && x.DatePaye >= QRModel.Deb.Value && x.DatePaye <= QRModel.End.Value).ToList();
                            break;
                    }
                }
            }
            
            if (payements.Count > 0)
            {
                foreach(var p in payements.OrderBy(x=>x.DatePaye).ToList())
                {
                    var _source = Enum.Format(typeof(SourcePaye), p.SourcePaye, "g") == "Client" ? null : "assurance";
                    string _type, _refFacture, _refRecuCaisse, identite;
                    int? _idFacture;
                    if (p.RefCmd > 0)
                    {
                        _type = "commande";
                        _idFacture = p.RefCmd;
                        _refFacture = (ctx.Commandes.Find(_idFacture)).RefCmd;
                        identite = p.Commande.Client != null ? p.Commande.Client.Nom.ToUpper() + " "+ Title(p.Commande.Client.Prenom): "";
                    }
                    else
                    {
                        _type = "vente";
                        _idFacture = p.RefVente;
                        _refFacture = (ctx.Ventes.Find(_idFacture)).RefVente;
                        identite = p.Vente.NomClient.ToUpper() + " " + Title(p.Vente.PrenomClient);
                    }
                    _refRecuCaisse = GetNumeroRecu(p); //p.Id.ToString();
                    VlCaisseModel.Add(new ValidationCaisseModel { Date = p.DatePaye.ToString("dd/MM/yyyy"), RefFacture = _refFacture, RefRecuCaisse = _refRecuCaisse, MontantVerse = p.MontantPaye, Source = _source, Type = _type, IdFacture = _idFacture, IdPayement = p.Id, Valider = p.MontantEncaisse, Identite = identite });
                }
                return Json(VlCaisseModel, JsonRequestBehavior.AllowGet);
            }
            return Json(VlCaisseModel, JsonRequestBehavior.AllowGet);
        }

        [Route("payements/caisse-validation/", Name="_caisseValidation")]
        public ActionResult ValidationCaisse()
        {
            var _dte = DateTime.Now;
            var _QtypeValue = new List<QtypeValue> { 
                new QtypeValue{ Id = 1, Value="Non Encaisse"}, 
                new QtypeValue{ Id = 2, Value="Encaisse"}, 
                new QtypeValue{ Id = 3, Value="Tout"} 
            };
            ViewBag.Qtype = new SelectList(_QtypeValue, "Id", "Value", 1);
            return View(new QReglementModel { Deb = _dte, End = _dte});
        }

        [Route("payements/caisse-validation/", Name="_caisseValidationPoste")]
        [HttpPost]
        public JsonResult ValidationCaissePost(List<ValidationCaisseModel> _vcm)
        {
            Reponse _response = new Reponse { Statut = 500 , Message = "Format de donnée invalide"};
            if (ModelState.IsValid)
            { 
                foreach (var _p in _vcm)
                {
                    bool initPayeStatut = false;
                    var payement = ctx.Payements.Find(_p.IdPayement);
                    
                    try
                    {
                        initPayeStatut = payement.MontantEncaisse;
                        payement.MontantEncaisse = _p.Valider;
                        if (_p.Valider) payement.User = User.Identity.Name;
                        if (_p.Type == "commande")
                        {
                            float totalVerse = 0; 
                            var commande = ctx.Commandes.Find(_p.IdFacture);
                            //effectuer une mise à jour de statut de reglement
                            if (_p.Valider && !initPayeStatut) totalVerse = _p.MontantVerse;
                            if (!_p.Valider && initPayeStatut) totalVerse -= _p.MontantVerse;
                            if (_p.Source == null)
                            {
                                totalVerse += ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == _p.IdFacture && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                                if (commande.MontantClient - (totalVerse + commande.ReductionClient) == 0)
                                    commande.PayeClient = true;
                                else
                                    commande.PayeClient = false;
                            }
                            else
                            {
                                //assurance
                                //totalVerse = _p.MontantVerse;
                                var assuranceId = payement.AssuranceId;
                                totalVerse += ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == _p.IdFacture && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == assuranceId).ToList().Sum(x => x.MontantPaye);
                               var assurCmd =  commande.AssuranceCommandes.Where(x=> x.AssuranceId == assuranceId).FirstOrDefault();
                               if (assurCmd != null)
                               {
                                   if (assurCmd.MontantAsssur - totalVerse == 0)
                                       assurCmd.PayeAssur = true;
                                   else
                                       assurCmd.PayeAssur = false;
                               }
                                
                            }
                        }
                        else
                        {
                            //il s'agit d'une vente
                            var vente = ctx.Ventes.Find(_p.IdFacture);
                            float totalVerse = 0;
                            if (_p.Valider && !initPayeStatut) totalVerse = _p.MontantVerse;
                            if (!_p.Valider && initPayeStatut) totalVerse -= _p.MontantVerse;
                            if (_p.Source == null)
                            {
                                //reglement effectuer par le client                             
                                totalVerse += ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == _p.IdFacture && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x => x.MontantPaye);
                                if (vente.MontantClient - (totalVerse + vente.ReductionClient) == 0)
                                    vente.PayeClient = true;
                                else
                                    vente.PayeClient = false;

                            }
                            else
                            {
                               // totalVerse = _p.MontantVerse;
                                var assurId = payement.AssuranceId;
                                totalVerse += ctx.Payements.Where(x => x.Del == false && x.RefVente != null && x.RefVente == _p.IdFacture && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId == assurId).ToList().Sum(x => x.MontantPaye);
                                var assurVente = vente.AssuranceVentes.Where(x => x.AssuranceId == assurId).FirstOrDefault();
                                if (assurVente != null)
                                {
                                    if (assurVente.MontantAssur - totalVerse == 0)
                                        assurVente.PayeAssur = true;
                                    else
                                        assurVente.PayeAssur = false;
                                }
                                
                            }
                        }
                        ctx.SaveChanges();
                    }
                    catch (DataException ex)
                    {
                        _response.Message = "Une Erreur s'est produite lors de la validation du paiement";
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                }
                _response.Statut = 200; 
                return Json(_response, JsonRequestBehavior.AllowGet);
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        [Route("payements/annuler/{id}", Name="_annulerPayement")] // id du paiement
        public JsonResult DelPayement(int id)
        {
            Reponse _rep = new Reponse { Statut = 500, Message = "Impossible d'annuler ce reglement" };
            var payement = ctx.Payements.Find(id);
            try
            {
                //ici on suppose que c'est une commande
                var commande = payement.Commande;
                if (commande != null)
                {
                    if (payement.SourcePaye == SourcePaye.Assurance) 
                    {
                        var assurId = payement.AssuranceId;
                        var assurCmd = commande.AssuranceCommandes.Where(x => x.AssuranceId == assurId).FirstOrDefault();
                        if (assurCmd != null)
                        {
                            assurCmd.PayeAssur = false;
                        }
                    }
                    else commande.PayeClient = false;
                }
                else
                {
                    //il s'agit d'une vente
                    var vente = payement.Vente;
                    if (payement.SourcePaye == SourcePaye.Assurance) 
                    {
                        var assurId = payement.AssuranceId;
                        var assurVente = vente.AssuranceVentes.Where(x=> x.AssuranceId == assurId).FirstOrDefault();
                        if (assurVente != null) {
                            assurVente.PayeAssur = false;
                        }
                    } 
                    else vente.PayeClient = false;
                }

                payement.Del = true;
                payement.DateDel = DateTime.Now;
                payement.MotifDel = User.Identity.Name;
                ctx.SaveChanges();
                _rep.Statut = 200;
            }
            catch (DataException /*ex*/)
            {
                return Json(_rep, JsonRequestBehavior.AllowGet);
            }
            return Json(_rep, JsonRequestBehavior.AllowGet);
        }

        [Route("payements/journal-caisse/", Name = "_journalCaisse")]
        public ActionResult JournalCaisse()
        {
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [Route("payements/query-journal-caisse/{print?}", Name="_QjournalCaisse")]
        public ActionResult GetJournalCaisse(DateTime Deb, DateTime End, string  print = null) 
        {
            //Reponse resp = new Reponse { Statut = 500, Message = "Veuillez renseigner la date debut et fin" };
            List<JournalCaisse> JournalCaisses = new List<JournalCaisse>();
            try
            {
                var payements = ctx.Payements.Where(x => x.DatePaye >= Deb && x.DatePaye <= End && x.MontantEncaisse).OrderBy(x=>x.Id).ToList();
                if (payements.Count > 0)
                {
                    JournalCaisse jcs;
                    foreach (var p in payements)
                    {
                        var _numRecu = p.Id.ToString();//GetNumeroRecu(p);
                        var _refFacture = "";
                        var _identite = "";
                        if (p.Vente != null) 
                        {
                            _refFacture = p.Vente.RefVente;
                            _identite = p.Vente.NomClient.ToUpper() + " "+ Title(p.Vente.PrenomClient);
                        }
                        else
                        {
                            _refFacture = p.Commande.RefCmd;
                            if (p.SourcePaye == SourcePaye.Assurance) {
                                _identite = "";// p.Commande.Assurance != null ? p.Commande.Assurance.Code.ToUpper(): "--";
                            }
                            else
                            {
                                _identite = p.Commande.Client.Nom.ToUpper() + " " + Title(p.Commande.Client.Prenom);  
                            }
                        }
                        jcs = new JournalCaisse { Date = p.DatePaye.ToString("dd/MM/yyyy"), RefFacture = _refFacture, Identite = _identite, MontantVerse = p.MontantPaye, NumRecu = _numRecu, Id = p.Id , DelState = p.Del};
                        JournalCaisses.Add(jcs);
                    }
                }
            }
            catch (DataException /* ex */)
            {

            }
            if (!string.IsNullOrEmpty(print))
            {
                _cReport = JournalCaisses;
                _periode = new PeriodeRecette { Deb = Deb.ToString("dd/MM/yyyy"), End = End.ToString("dd/MM/yyyy") };
                var url = Url.RouteUrl("_adminCaisseReport");
                var ConvertStatut = generateReportFile(url, "caisseReport.pdf","footer");
                return Json(new { statut = ConvertStatut }, JsonRequestBehavior.AllowGet);
            }
            return Json(JournalCaisses, JsonRequestBehavior.AllowGet);
        }

       [AllowAnonymous]
        [Route("administration/caisse-report/", Name="_adminCaisseReport")]
        public ActionResult CaisseReport()
        {
            ViewBag.periode = _periode;
            return View(_cReport);
        }

       [Route("caisse/assurance-payements/", Name="_caisseAssuranceReglement")]
       public JsonResult CaisseAsuranceReglement(List<AssurancePayementViewModel> apmodel, InfoReglement infoRegl)
       {
           Reponse rep = new Reponse { Statut = 200 };
           if (apmodel.Count > 0)
           {
               foreach (var p in apmodel)
               {
                   var payement = new Payement { AssuranceId = p.IdAssurance, MontantEncaisse= true, MontantPaye=p.Montant, DateEnreg=DateTime.Now, SourcePaye = SourcePaye.Assurance, User = User.Identity.Name, DatePaye= infoRegl.DatePaye, ModePaye =  infoRegl.ModeReglement, RefPayement = infoRegl.RefPayement};
                   if (p.Origine == "commande")
                   {
                       payement.RefCmd = p.Id;
                       var assurCmde = ctx.AssuranceCommandes.Where(x => x.CommandeId == p.Id && x.AssuranceId == p.IdAssurance).FirstOrDefault();
                       if (assurCmde != null) assurCmde.PayeAssur = true;
                   }
                   else {
                       payement.RefVente = p.Id;
                       var assurVte = ctx.AssuranceVentes.Where(x => x.AssuranceId == p.IdAssurance && x.VenteId == p.Id).FirstOrDefault();
                       if (assurVte != null) assurVte.PayeAssur = true;
                   } 
                   ctx.Payements.Add(payement);
                   ctx.SaveChanges();
               }
           }
           return Json(rep,JsonRequestBehavior.AllowGet);
       }

	}

    public struct PeriodeRecette
    {
        public string Deb { get; set; }
        public string End { get; set; }
    }
}