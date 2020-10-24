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
    public partial class CommandeController : BaseController
    {
        public static ProformaModel _proforma;

        [Route("commandes/{id}", Name="_ListeCommandeClient")] //id client
        public ActionResult CommandeClient(int id) 
        {
            var commandes = ctx.Commandes.Where(x => x.Del == false && x.ClientId == id).ToList();
            List<QCommandeModel> lQcmd = new List<QCommandeModel>();
            if (commandes.Count > 0) 
            {
                List<DetailsCommande> lproductCmd = new List<DetailsCommande>();
                var lCmdLivre = ctx.ReceptionCommandes.Select(x => x.CommandeId).ToList(); 
               foreach(var cmd in commandes) 
               {
                   /*if (cmd.LigneCommandes.Count > 0) {
                       foreach(var lcmd in cmd.LigneCommandes.Where(x => x.Del == false).ToList()) 
                       {
                           var produit = lcmd.Produit;
                           var produitModel = new ProduitModel { Designation = produit.Designation, Divers = produit.ModelDivers, Verre = produit.ModelVerre, Monture = produit.ModelMonture };
                           var _designation = FormatDesignation(produitModel);
                           var marque = produit.Marque != null ? produit.Marque.Libelle : null;
                           lproductCmd.Add(new DetailsCommande { Designation = _designation, Qte = lcmd.QteCmd, Marque = marque });
                       }
                   }*/
                   /*var statutlivraison = lCmdLivre.Contains(cmd.Id);
                   lQcmd.Add(new QCommandeModel { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), DetailsCommande = lproductCmd, Id = cmd.Id, Valider = cmd.StatutCmd, RefCommande = cmd.RefCmd, Livrer = statutlivraison, MontantAssur = cmd.MontantAssur, MontantClient = cmd.MontantClient });*/
               }
                
            }
           // ViewBag.ClientId = id;
            return View(lQcmd);
        }
        public ActionResult Index()
        {
            return View();
        }

        [Route("fiche-commande/add/", Name="_addCommande")] //id du client
        public ActionResult Add(/*int id*/)
        {
            ViewBag.Assurances = new SelectList(ctx.Assurances.Where(x => x.Del == false).ToList(), "Id", "Nom");
            //ViewBag.ClientId = id;
            var listeMarqueMonture = (from m in ctx.Marques
                                      join p in ctx.Produits on m.Id equals p.MarqueId
                                      join c in ctx.Categories on p.CategorieId equals c.Id
                                      where c.Del == false && c.Libelle.ToUpper() == "MONTURE"
                                      select m).Distinct().OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList();
            ViewBag.Marque = new SelectList(listeMarqueMonture, "Id", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle");
            ViewBag.Civilite = new SelectList(new List<QtypeValue>{ new  QtypeValue{ Id = 0, Value = "M" },new  QtypeValue{ Id = 1, Value = "Mlle" },new  QtypeValue{ Id = 2, Value = "Mme" }},"Id","Value");
            var listeTypeVerre = new List<QtypeValue> { new  QtypeValue{ Id = 0, Value = "Unifocaux"}, new QtypeValue{ Id = 1, Value = "Progressif"}, new QtypeValue{ Id = 2, Value = "Bifocaux"} };
            ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x=>x.Value).ToList(), "Id", "Value");
            ViewBag.Assurance = new SelectList(ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x=>new AssuranceModel { Code = x.Code.ToUpper(), Id = x.Id, Nom = x.Nom.ToUpper() }).ToList(), "Id", "Nom");
           // ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
            //ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            /*var _monture = new FCMonture();
            var _verre = new FCVerre { OD = new FCVerreFormat(), OG = new FCVerreFormat() };*/
            var ficheCmdModel = new FicheCommandeModel
            {
                DateCommande = DateTime.Now,
                Monture = new FCMonture(),
                Verre = new FCVerre
                {
                    OG = new FCVerreFormat
                    {
                        VP = new FCTypeVision(),
                        VL = new FCTypeVision(), 
                        Qte = 1
                    },
                    OD = new FCVerreFormat
                    {
                        VL = new FCTypeVision(),
                        VP = new FCTypeVision(), 
                        Qte = 1
                    }
                }
            };
            return View("Commande2", ficheCmdModel);
        }

        [Route("fiche-commande/add/", Name="_addPostCommande")]
        [HttpPost]
        public ActionResult AddPost(/*int id, CommandeViewModel commandeModel,[Bind(Include="Nom, Prenom, Contact, Contact2, Email, AssuranceId,")]*/FicheCommandeModel FicheCmde)
        {
            /*if (ModelState.IsValid)
            {

            }*/
            try 
            {
                //client
                Client client = new Client { Civilite = (Models.Civilite)FicheCmde.Civilite, Contact = FicheCmde.Contact, Contact2 = FicheCmde.Contact2, Nom = FicheCmde.Nom, Prenom = FicheCmde.Prenom, Adresse = FicheCmde.Email };
                ctx.Clients.Add(client);
                ctx.SaveChanges();

                //Commande
                Commande _cmde = new Commande { ClientId = client.Id, Teinte = FicheCmde.Verre.Teinte, DateEnreg = DateTime.Now, DateCmd = FicheCmde.DateCommande,  MontantClient = FicheCmde.Payement.MontantClient, DateLvrCmd = DateTime.Now, RefCmd = "--", ReductionClient = FicheCmde.Payement.ReductionClient};
                if (FicheCmde.Verre.Organique) _cmde.Nature = NatureVerre.Organique;
                if (FicheCmde.Verre.Mineraux) _cmde.Nature = NatureVerre.Mineraux;
                 ctx.Commandes.Add(_cmde);
                ctx.SaveChanges();
                if (FicheCmde.AN != null && FicheCmde.AN.Count > 0 && FicheCmde.AN[0] > 0)
                {
                    int index = 0;
                    foreach (var assId in FicheCmde.AN)
                    {
                        var mttAssur = FicheCmde.AM[index];
                        var statutPaye = mttAssur == 0 ? true : false;
                        ctx.AssuranceCommandes.Add(new AssuranceCommande { AssuranceId = assId, CommandeId = _cmde.Id, MontantAsssur =mttAssur, PayeAssur = false });
                        index++;
                    }
                    ctx.SaveChanges();
                }

                var ch = DateTime.Now;
                _cmde.RefCmd = "REF C" + ch.Year + "/" + ch.Day + ch.Month + _cmde.Id;
                //ligne de commande
                var _monture = FicheCmde.Monture;
                if (_monture.MontureId > 0 /*&& _monture.MontureQte > 0*/)
                {
                    var _lgCmde = new LigneCommande { CommandeId = _cmde.Id, PrixCmd = _monture.MonturePrix, ProduitId = _monture.MontureId, QteCmd = _monture.MontureQte, Rem = _monture.MontureRemise, RemDG = _monture.MontureRemiseDG };
                    ctx.LigneCommandes.Add(_lgCmde);
                    ctx.SaveChanges();
                }
                
                //Verre
                var _verre = FicheCmde.Verre;
                var _mvOD = getFormatModelVerre(_verre.OD, Side.OD,_cmde.Id); //new ModelVerre { Add = _verre.OD.Add, CommandeId = _cmde.Id, GammeVerreId = _verre.OD.GammeVerreId, Prix = _verre.OD.Prix, Qte = _verre.OD.Qte, Remise = _verre.OD.Remise, RemiseDG = _verre.OD.RemiseDG, VLAxe = _verre.OD.VL.Axe, VLCyl = _verre.OD.VL.Cyl, VLSph = _verre.OD.VL.Sph, VPAxe = _verre.OD.VP.Axe, VPCyl = _verre.OD.VP.Cyl, VPSph = _verre.OD.VP.Sph, Side = Side.OD, Type = _verre.OD.Type, Nature = _verre.OD.Nature };

                var _mvOG = getFormatModelVerre(_verre.OG,Side.OG,_cmde.Id); //new ModelVerre { Add = _verre.OG.Add, GammeVerreId = _verre.OG.GammeVerreId, Nature = _verre.OG.Nature, Qte = _verre.OG.Qte, Prix = _verre.OG.Prix, Remise = _verre.OG.Remise, RemiseDG = _verre.OG.RemiseDG, Type = _verre.OG.Type, VLAxe = _verre.OG.VL.Axe, VLCyl = _verre.OG.VL.Cyl, VLSph = _verre.OG.VL.Sph, VPAxe = _verre.OG.VP.Axe, VPCyl = _verre.OG.VP.Cyl, VPSph = _verre.OG.VP.Sph, Side = Side.OG, CommandeId = _cmde.Id };
                ctx.ModelVerres.Add(_mvOD);
                ctx.ModelVerres.Add(_mvOG);
                var _payement = FicheCmde.Payement;
                //if (_payement.MontantAssurance == 0) _cmde.PayeAssur = true;
                if (_payement.MontantClient == 0 || _payement.MontantClient == _payement.ReductionClient ) _cmde.PayeClient = true;
                ctx.SaveChanges();
                //Payement
                if (_payement.MontantVerse > 0)
                {
                    Payement p = new Payement { DatePaye = FicheCmde.DateCommande, MontantPaye = _payement.MontantVerse, RefCmd = _cmde.Id, DateEnreg = DateTime.Now, SourcePaye = SourcePaye.Client, RefPayement = _payement.RefPayement, ModePaye = _payement.ModeReglement };              
                    ctx.Payements.Add(p);
                    ctx.SaveChanges();
                }

                // creer les nvelle modele de verre
                MVerreViewModel OD = new MVerreViewModel { Add = _mvOD.Add, Cyl = _mvOD.VLCyl, GammeVerreId = _mvOD.GammeVerreId, Side = Side.OD, Sph = _mvOD.VLSph, TypeVerre = _mvOD.Type  };
                MVerreViewModel OG = new MVerreViewModel { Add = _mvOG.Add, Cyl = _mvOG.VLCyl, GammeVerreId = _mvOG.GammeVerreId, Side = Side.OG, Sph = _mvOG.VLSph, TypeVerre = _mvOG.Type};
                CreateVerre(OD, _mvOD.Qte);
                CreateVerre(OG, _mvOG.Qte); 
            }
            catch (DataException ex)
            {
                ViewBag.Assurances = new SelectList(ctx.Assurances.Where(x => x.Del == false).ToList(), "Id", "Nom");
                //ViewBag.ClientId = id;
                var listeMarqueMonture = getListeMarqueMonture(); 
                ViewBag.Marque = new SelectList(listeMarqueMonture, "Id", "Libelle");
                ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.OrderBy(x => x.Libelle).ToList(), "Id", "Libelle");
                var listeTypeVerre = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "Unifocaux" }, new QtypeValue { Id = 1, Value = "Progressif" }, new QtypeValue { Id = 2, Value = "Bifocaux" } };
                ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x => x.Value).ToList(), "Id", "Value");
                ViewBag.Civilite = new SelectList(new List<QtypeValue> { new QtypeValue { Id = 0, Value = "M" }, new QtypeValue { Id = 1, Value = "Mlle" }, new QtypeValue { Id = 2, Value = "Mme" } }, "Id", "Value");
                ViewBag.Assurance = new SelectList(ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x => new AssuranceModel { Code = x.Code.ToUpper(), Id = x.Id, Nom = x.Nom.ToUpper() }).ToList(), "Id", "Nom");
                return View("Commande2", FicheCmde);
            }
            //return View("Commande2", FicheCmde);
            return RedirectToRoute("_listeCommandeAllType");
        }

        [Route("fiche-commande/{id}/", Name="_updateCommande")] //id commande
        public ActionResult Update(int id)
        {
           var  cmd = ctx.Commandes.Find(id);
           if (cmd == null)
               return HttpNotFound();
           FCPayement _payement;
            var client = cmd.Client;
            var Monture = cmd.LigneCommandes.Where(x=>x.Del == false).FirstOrDefault();
            var ModelVerre = cmd.ModeleVerres.OrderBy(x=>x.Id).ToList();
            var payement = cmd.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client).ToList();
            var fpayement = payement.Where(x => x.DatePaye >= cmd.DateCmd).OrderBy(x => x.Id).FirstOrDefault();
            if (fpayement != null)
            {
                _payement = new FCPayement { /*MontantAssurance = cmd.MontantAssur,*/ MontantClient = cmd.MontantClient, ModeReglement = fpayement.ModePaye, MontantVerse = fpayement.MontantPaye, RefPayement = fpayement.RefPayement, ReductionClient = cmd.ReductionClient };
            }
            else
            {
                _payement = new FCPayement { /*MontantAssurance = cmd.MontantAssur,*/ MontantClient = cmd.MontantClient, ModeReglement = ModeReglement.Espece, MontantVerse = 0, RefPayement = null, ReductionClient = cmd.ReductionClient }; 
            }


            var OD = getFormatVerre(ModelVerre[0]); //new FCVerreFormat { Add = ModelVerre[0].Add, GammeVerreId = ModelVerre[0].GammeVerreId, Nature = ModelVerre[0].Nature, Prix = ModelVerre[0].Prix, Qte = ModelVerre[0].Qte, Remise = ModelVerre[0].Remise, RemiseDG = ModelVerre[0].RemiseDG, Type = ModelVerre[0].Type, VL = new FCTypeVision { Axe = ModelVerre[0].VLAxe, Cyl = ModelVerre[0].VLCyl, Sph = ModelVerre[0].VLSph }, VP = new FCTypeVision { Axe = ModelVerre[0].VPAxe, Cyl = ModelVerre[0].VPCyl, Sph = ModelVerre[0].VPSph } };

            var OG = getFormatVerre(ModelVerre[1]); 
            /*new FCVerreFormat{
                Add = ModelVerre[1].Add, GammeVerreId = ModelVerre[1].GammeVerreId, Nature = ModelVerre[1].Nature, Prix = ModelVerre[1].Prix, Qte = ModelVerre[1].Qte, Remise = ModelVerre[1].Remise, RemiseDG = ModelVerre[1].RemiseDG, Type = ModelVerre[1].Type, VL = new FCTypeVision{ Axe = ModelVerre[1].VLAxe, Cyl = ModelVerre[1].VLCyl, Sph = ModelVerre[1].VLSph}, VP = new FCTypeVision{ Axe = ModelVerre[1].VPAxe, Cyl = ModelVerre[1].VPCyl, Sph = ModelVerre[1].VPSph} 
            };*/

            var _natureMineraux = cmd.Nature == NatureVerre.Mineraux  ? true : false;
            var _natureOrganique = cmd.Nature == NatureVerre.Organique ? true : false;
            //var Assureurs = ctx.AssuranceCommandes.Where(x=>x.CommandeId == id).OrderBy(x=>x.Id).ToList();
            List<int> AN = cmd.AssuranceCommandes.Select(x => x.AssuranceId).ToList();
            List<float> AM = cmd.AssuranceCommandes.Select(x => x.MontantAsssur).ToList();
            var ficheModel = new FicheCommandeModel
            {
                Civilite = client.Civilite, 
                Id = id,
                Contact = client.Contact,
                Contact2 = client.Contact2,
                Email = client.Adresse,
                Nom = client.Nom,
                Prenom = client.Prenom,
                AN = AN, 
                AM = AM,
                //AssuranceId = cmd.AssuranceId,
                DateCommande = cmd.DateCmd,
                Monture = Monture != null ? new FCMonture { MontureId = Monture.Id, MonturePrix = Monture.PrixCmd, MontureQte = Monture.QteCmd, MontureRemise = Monture.Rem, MontureRemiseDG = Monture.RemDG, MontureProdId=Monture.ProduitId } : new FCMonture(),
                Verre = new FCVerre { OD = OD, OG = OG, Teinte = cmd.Teinte, Mineraux = _natureMineraux, Organique = _natureOrganique },
                Payement = _payement,
                StatutNbPaye = "OK"
            };

            if (payement.Count > 1)
                ficheModel.StatutNbPaye = "NOK";
            var montureSelected = Monture!= null ? ctx.Produits.Find(Monture.ProduitId):null;
            ViewBag.MontureSelected = montureSelected;
            ViewBag.Assurances = new SelectList(ctx.Assurances.Where(x => x.Del == false).ToList(), "Id", "Nom");
            var listeMarqueMonture = getListeMarqueMonture(); 
            var marqueId = montureSelected != null ? montureSelected.Marque.Id : 0;
            ViewBag.marqueId = marqueId;
            ViewBag.Marque = new SelectList(listeMarqueMonture, "Id", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle");
            var listeTypeVerre = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "Unifocaux" }, new QtypeValue { Id = 1, Value = "Progressif" }, new QtypeValue { Id = 2, Value = "Bifocaux" } };
            ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x=>x.Value).ToList(), "Id", "Value");
            ViewBag.Assurance = new SelectList(ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x => new AssuranceModel { Nom = x.Nom.ToUpper(), Id = x.Id, Code = x.Code.ToUpper() }).ToList(), "Id", "Nom");
            var civiliteId = Enum.Format(typeof(Models.Civilite), client.Civilite,"d");
            ViewBag.Civilite = new SelectList(new List<QtypeValue> { new QtypeValue { Id = 0, Value = "M" }, new QtypeValue { Id = 1, Value = "Mlle" }, new QtypeValue { Id = 2, Value = "Mme" } }, "Id", "Value", civiliteId);
            return View("UpdateCommande",ficheModel);
        }

        [Route("fiche-commande/{id}/", Name="_updatePostCommande")]
        [HttpPost]
        public ActionResult UpdatePost(int id , FicheCommandeModel FichecmdModel) 
        {
            var cmd = ctx.Commandes.Find(id);
            try
            {
                var clientToUp = cmd.Client;
                //client 
                clientToUp.Adresse = FichecmdModel.Email;
                clientToUp.Civilite = FichecmdModel.Civilite;
                clientToUp.Contact = FichecmdModel.Contact;
                clientToUp.Contact2 = FichecmdModel.Contact2;
                clientToUp.Nom = FichecmdModel.Nom;
                clientToUp.Prenom = FichecmdModel.Prenom;
                //commande
                //cmd.AssuranceId = FichecmdModel.AssuranceId;
                cmd.DateCmd = FichecmdModel.DateCommande;
                //cmd.MontantAssur = FichecmdModel.Payement.MontantAssurance;
                cmd.MontantClient = FichecmdModel.Payement.MontantClient;
                cmd.ReductionClient = FichecmdModel.Payement.ReductionClient;
                cmd.Teinte = FichecmdModel.Verre.Teinte;
                //var mttAssurance = FichecmdModel.AM.ToList().Sum();
                var mttClient = FichecmdModel.Payement.MontantClient - cmd.ReductionClient;
                
                if (cmd.AssuranceCommandes.ToList().Count > 0)
                {
                    foreach (AssuranceCommande item in cmd.AssuranceCommandes.ToList())
                    {
                        //check paiement
                        if ((FichecmdModel.AN != null && !FichecmdModel.AN.Contains(item.AssuranceId)) || FichecmdModel.AN == null)
                        {
                            var AssurPaye = cmd.Payements.Where(x => x.Del == false && x.AssuranceId == item.AssuranceId && x.SourcePaye == SourcePaye.Assurance).ToList();
                            if (AssurPaye.Count > 0)
                            {
                                foreach (var p in AssurPaye)
                                {
                                    p.Del = true;
                                }
                                ctx.SaveChanges();
                            }
                        }
                        var elt = cmd.AssuranceCommandes.Where(x=> x.AssuranceId == item.AssuranceId).FirstOrDefault();
                        ctx.AssuranceCommandes.Remove(elt);
                        ctx.SaveChanges();                          
                    }                   
                }

                if (FichecmdModel.AN != null && FichecmdModel.AN.Count > 0 && FichecmdModel.AN[0] > 0)
                {
                    int index = 0;
                    foreach (var item in FichecmdModel.AN)
                    {
                        bool statutPaye;
                        var mttAssurance = FichecmdModel.AM[index];
                        var mttReglerAss = cmd.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Assurance && x.MontantEncaisse && x.AssuranceId != null && x.AssuranceId == item).ToList().Sum(x => x.MontantPaye);
                        if (mttAssurance - mttReglerAss == 0) statutPaye = true;
                        else statutPaye = false;
                        ctx.AssuranceCommandes.Add(new AssuranceCommande { AssuranceId = item, CommandeId = id, MontantAsssur = mttAssurance, PayeAssur = statutPaye });
                        index++;
                    }
                    ctx.SaveChanges();
                }
                
                var mttReglerCl = cmd.Payements.Where(x=>x.Del == false && x.SourcePaye == SourcePaye.Client && x.MontantEncaisse).ToList().Sum(x=>x.MontantPaye);

                if (FichecmdModel.Verre.Mineraux) { cmd.Nature = NatureVerre.Mineraux; }
                if (FichecmdModel.Verre.Organique) { cmd.Nature = NatureVerre.Organique; }
                cmd.User = User.Identity.Name;
                //Monture
                 var monture = FichecmdModel.Monture;
                var montureToUp = cmd.LigneCommandes.Where(x => x.Del == false).FirstOrDefault();
               /* if (montureToUp == null)
                {
                    var lcmdeMonture = new LigneCommande{ CommandeId = id, PrixCmd = monture.MonturePrix, ProduitId=, QteCmd=, Rem=, RemDG=};
                }
                else
                {*/
                    if (montureToUp == null || montureToUp.ProduitId != FichecmdModel.Monture.MontureId)
                    {
                        //
                        if(montureToUp != null) montureToUp.Del = true;
                        ctx.LigneCommandes.Add(new LigneCommande { CommandeId = id, PrixCmd = monture.MonturePrix, ProduitId = monture.MontureId, QteCmd = monture.MontureQte, Rem = monture.MontureRemise, RemDG = monture.MontureRemiseDG });
                        ctx.SaveChanges();
                    }
                    else
                    {
                        montureToUp.ProduitId = monture.MontureId;
                        montureToUp.QteCmd = monture.MontureQte;
                        montureToUp.PrixCmd = monture.MonturePrix;
                        montureToUp.Rem = monture.MontureRemise;
                        montureToUp.RemDG = monture.MontureRemiseDG;
                    }
                /*}*/


                //Verres
                var verres = cmd.ModeleVerres.OrderBy(x=>x.Id).ToList();
                var OD = FichecmdModel.Verre.OD;
                var OG = FichecmdModel.Verre.OG;
                var verreODToUp = verres[0];
                var verreOGToUp = verres[1];
                //OD
                verreODToUp.Add = OD.Add;
                verreODToUp.GammeVerreId = OD.GammeVerreId;
                verreODToUp.Prix = OD.Prix;
                verreODToUp.Qte = OD.Qte;
                verreODToUp.Remise = OD.Remise;
                verreODToUp.RemiseDG = OD.RemiseDG;
                verreODToUp.Type = OD.Type;
                verreODToUp.VLAxe = OD.VL.Axe;
                verreODToUp.VLCyl = OD.VL.Cyl;
                verreODToUp.VLSph = OD.VL.Sph;
                verreODToUp.VPAxe = OD.VP.Axe;
                verreODToUp.VPCyl = OD.VP.Cyl;
                verreODToUp.VPSph = OD.VP.Sph;
                //OG
                verreOGToUp.Add = OG.Add;
                verreOGToUp.GammeVerreId = OG.GammeVerreId;
                verreOGToUp.Prix = OG.Prix;
                verreOGToUp.Qte = OG.Qte;
                verreOGToUp.Remise = OG.Remise;
                verreOGToUp.RemiseDG = OG.RemiseDG;
                verreOGToUp.Type = OG.Type;
                verreOGToUp.VLAxe = OG.VL.Axe;
                verreOGToUp.VLCyl = OG.VL.Cyl;
                verreOGToUp.VLSph = OG.VL.Sph;
                verreOGToUp.VPAxe = OG.VP.Axe;
                verreOGToUp.VPCyl = OG.VP.Cyl;
                verreOGToUp.VPSph = OG.VP.Sph;
                //Payement
                var Payement = FichecmdModel.Payement;
                var payements = cmd.Payements.Where(x => x.Del == false && x.SourcePaye == SourcePaye.Client && x.DatePaye >= cmd.DateCmd).OrderBy(x => x.Id).ToList();
                if (payements.Count == 1) 
                {
                    //
                    var payeToUp = payements.FirstOrDefault();
                    //if (payeToUp.MontantPaye != Payement.MontantVerse) payeToUp.MontantEncaisse = false;
                    payeToUp.ModePaye = Payement.ModeReglement;
                    payeToUp.MontantPaye = Payement.MontantVerse;
                    payeToUp.RefPayement = Payement.RefPayement;    
                }

                if (payements.Count == 0 && FichecmdModel.Payement.MontantVerse > 0)
                {
                    //une operation de paiement d'avance sur commande est effectué
                    ctx.Payements.Add(new Payement { 
                        MontantPaye = Payement.MontantVerse, 
                        RefPayement = Payement.RefPayement, 
                        ModePaye = Payement.ModeReglement,
                        RefCmd = id,
                        SourcePaye = SourcePaye.Client, 
                        DatePaye = FichecmdModel.DateCommande,
                        DateEnreg = DateTime.Now });
                    ctx.SaveChanges();
                }
                if (mttClient - mttReglerCl == 0) cmd.PayeClient = true;
                else cmd.PayeClient = false;

                ctx.SaveChanges();

                //verre check if exist
                MVerreViewModel _mvOD = new MVerreViewModel { Add = OD.Add, Cyl = OD.VL.Cyl, GammeVerreId = OD.GammeVerreId, Side = Side.OD, Sph = OD.VL.Sph, TypeVerre = OD.Type  };
                MVerreViewModel _mvOG = new MVerreViewModel { Add = OG.Add, Cyl = OG.VL.Cyl, GammeVerreId = OG.GammeVerreId, Side = Side.OG, Sph = OG.VL.Sph, TypeVerre = OG.Type  };
                CreateVerre(_mvOD, OD.Qte);
                CreateVerre(_mvOG, OG.Qte);
                return RedirectToRoute("_listeCommandeAllType");
            } 
            catch (Exception ex) 
            {
                ModelState.AddModelError("","Erreur s'est produite lors du traitement");
                return RedirectToRoute("_updateCommande", new { id = id });
            }
            //return View("UpdateCommande", FichecmdModel);
        }

        [Route("fiche-commande/{id}/ligne-commande/", Name="_getLigneCommande")] // id commande
        public JsonResult LigneCommande(int id)
        {
            List<LigneCommandeModel> LigneCommandes = new List<LigneCommandeModel>();
            /*var Commande = ctx.Commandes.Find(id);
            if (Commande == null)
                return Json(LigneCommandes, JsonRequestBehavior.AllowGet);
            foreach (var lcmd in Commande.LigneCommandes.Where(x => x.Del == false).ToList())
            {
                var _p = lcmd.Produit;
                var _produitmodel = new ProduitModel { Designation = _p.Designation, Divers = _p.ModelDivers, Id = _p.Id, Marque= _p.Marque != null ? _p.Marque.Libelle:"Sans marque", Monture = _p.ModelMonture, Prix = _p.Prix, QteStock = _p.QteStock, RefProd = "", Verre = _p.ModelVerre };
                LigneCommandes.Add(new LigneCommandeModel { Id = lcmd.ProduitId, Prix = lcmd.PrixCmd, Qte = lcmd.QteCmd, RefProd = FormatDesignation(_produitmodel) /*lcmd.Produit.RefProd, rem = lcmd.Rem, remdg = lcmd.RemDG});
            }*/
            return Json(LigneCommandes, JsonRequestBehavior.AllowGet);
        }

        
        [Route("commande/get-liste-Commande/{nom?}/", Name="_listeCommandeEnAttente")]
        public JsonResult ListeCommandeToValidate(string nom = null)
        {
            List<Commande> listeCmd = new List<Commande>();
            if (!string.IsNullOrEmpty(nom))
            {
                nom = nom.ToLower();
                listeCmd = (from c in ctx.Commandes
                            join cl in ctx.Clients on c.ClientId equals cl.Id
                            where ((cl.Nom.ToLower() == nom || cl.Prenom.ToLower() == nom) || (cl.Nom.ToLower().Contains(nom) || cl.Prenom.ToLower().Contains(nom))) && c.Del == false
                            select c).OrderBy(x=>x.Client.Nom).Take(30).ToList();
            }
            else
            {
                listeCmd = ctx.Commandes.Where(x => x.Del == false && x.StatutCmd == false).OrderBy(x=>x.DateCmd).Take(30).ToList();
            }
             
            List<ModelCommande> QCmdModel = new List<ModelCommande>();
            QCmdModel = GetQCommande(listeCmd);
            /*if (listeCmd.Count > 0)
            {
                foreach (var cmd in listeCmd)
                {
                    var lignecmd = cmd.LigneCommandes.Where(x => x.Del == false).ToList();
                    var _dateRLivraison = cmd.DateRLvrCmd != null ? cmd.DateRLvrCmd.Value.ToString("yyyy-MM-dd") : null;
                    var produit = lignecmd[0].Produit;
                    var Verres = cmd.ModeleVerres.ToList();
                    var OD = Verres[0];
                    var OG = Verres[1];
                    DetailsMonture DetailsMonture = new DetailsMonture { Designation = produit.Designation, Numero = produit.RefProd, Qte = lignecmd[0].QteCmd };
                    var _nature = Enum.Format(typeof(Models.NatureVerre),cmd.Nature,"g");
                    var _type = Enum.Format(typeof(Models.TypeVerre), OD.Type, "g");
                    var _typeOG = Enum.Format(typeof(Models.TypeVerre), OG.Type, "g");
                    var gamOD = OD.GammeVerre != null ? OD.GammeVerre.Libelle : "-";
                    var gamOG = OG.GammeVerre.Libelle != null ? OG.GammeVerre.Libelle : "-";
                    DetailsVerres DetailsVerre = new DetailsVerres { Nature = _nature, Teinte = cmd.Teinte,
                                                                     OD = new ModelDeVerre { Add = OD.Add, Type = _type, VL = new FCTypeVision { Axe = OD.VLAxe, Cyl = OD.VLCyl, Sph = OD.VLSph }, VP = new FCTypeVision { Axe = OD.VPAxe, Cyl = OD.VPCyl, Sph = OD.VPSph }, GammeV = gamOD },
                                                                     OG = new ModelDeVerre { Add = OG.Add, Type = _typeOG, VL = new FCTypeVision { Axe = OG.VLAxe, Cyl = OG.VLCyl, Sph = OG.VLSph }, VP = new FCTypeVision { Axe = OG.VPAxe, Cyl = OG.VPCyl??"", Sph = OG.VPSph??"" }, GammeV = gamOG }
                    };
                    IdentiteClient identiteCl = new IdentiteClient { Nom = cmd.Client.Nom.ToUpper(), Prenom = Title(cmd.Client.Prenom), Contact = cmd.Client.Contact, Contact2 = cmd.Client.Contact2, Email = cmd.Client.Adresse };
                    //var _datelivraison = cmd.DateLvrCmd != null ? cmd.DateLvrCmd.ToString("yyyy-MM-dd"):null;

                    QCmdModel.Add(new ModelCommande { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), DateLivraison = _dateRLivraison, DetailsMonture = DetailsMonture, DetailsVerres = DetailsVerre, Identite = identiteCl, Id = cmd.Id, Valider = cmd.StatutCmd});
                }
            }*/
            return Json(QCmdModel, JsonRequestBehavior.AllowGet);
        }

        [Route("fiche-commande/validation-commande/", Name="_validationCommande")]
        public ActionResult ValidateCommande()
        {
            return View();
        }

        [Route("fiche-commande/validation-commande/", Name="_valdidationCommandePost")]
        [HttpPost]
        public ActionResult ValidateCommandePost(List<QCommandeModel> qcmodel)
        {
            Reponse _response = new Reponse { Statut = 500, Message= "" }; 
            if (ModelState.IsValid)
            {
                if (qcmodel.Count > 0)
                {
                    foreach (var cmd in qcmodel)
                    {
                        var commande = ctx.Commandes.Find(cmd.Id); 
                        try
                        {
                            var oldStatut = commande.StatutCmd;
                            List<ModelVerre> Modelverre = commande.ModeleVerres.OrderBy(x=>x.Id).ToList();
                            var OD = Modelverre.Count > 0 ? Modelverre[0]:new ModelVerre();
                            var OG = Modelverre.Count >= 2 ? Modelverre[1] : new ModelVerre();
                            //Mise à jour des produits
                            if (cmd.Valider && !oldStatut)
                            {
                                var ligneCmde = commande.LigneCommandes.ToList();
                                UpdateStockProduit(ligneCmde, "down");
                                //update stock verre                               
                                if (OD != null) UpdateStockVerre(OD,Side.OD,"down");
                                if (OG != null) UpdateStockVerre(OG,Side.OG,"down");                            
                                //if (cmd.DateLivraison != null) commande.DateLvrCmd = DateTime.Parse(cmd.DateLivraison);
                                if (cmd.DateLivraison != null) commande.DateRLvrCmd = DateTime.Parse(cmd.DateLivraison/*cmd.DateRetardLivraison*/);
                                commande.User = User.Identity.Name;
                                commande.StatutCmd = cmd.Valider;
                                ctx.SaveChanges();
                            } 
                            else if (oldStatut && !cmd.Valider)
                            {
                                //mise à jour produit
                                var ligneCmde = commande.LigneCommandes.ToList();
                                UpdateStockProduit(ligneCmde, "up");
                                //update stock verre                               
                                if (OD != null) UpdateStockVerre(OD, Side.OD,"up");
                                if (OG != null) UpdateStockVerre(OG, Side.OG,"up");
                                commande.StatutCmd = cmd.Valider;
                                //verifier si cmde receptionnée alors supprimer
                                var receptioncommande = ctx.ReceptionCommandes.Where(x => x.CommandeId == cmd.Id).FirstOrDefault();
                                if (receptioncommande != null) ctx.ReceptionCommandes.Remove(receptioncommande);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                //commande.StatutCmd = cmd.Valider;
                            }        
                            //ctx.SaveChanges();
                        }
                        catch (DataException /* ex */)
                        {
                            _response.Message = "Une erreur s'est produite lors de la mise à jour de la commande Id: " + cmd.Id;
                            return Json(_response, JsonRequestBehavior.AllowGet);
                        }
                    }
                    _response.Statut = 200;
                }
            }
            return Json(_response, JsonRequestBehavior.AllowGet);
        }

        [Route("commandes/commande-disponible/", Name="_commandeDispo")]
        public ActionResult CommandeDisponible()
        {
            return View("CommandeDisponible4");
        }

        [Route("commandes/liste-commande-disponible/{nom?}/", Name="_listeCommandeDispo")]
        public ActionResult GetCommandeDisponible(string nom = null)
        {
            List<Commande> Commandes = new List<Commande>();
            if (!string.IsNullOrEmpty(nom))
            {
                nom = nom.ToLower();
                Commandes = (from c in ctx.Commandes
                            join cl in ctx.Clients on c.ClientId equals cl.Id
                             where ((cl.Nom.ToLower() == nom || cl.Prenom.ToLower() == nom) || (cl.Nom.ToLower().Contains(nom) || cl.Prenom.ToLower().Contains(nom))) && c.Del == false && c.StatutCmd && !ctx.ReceptionCommandes.Select(rc => rc.CommandeId).Contains(c.Id)
                            select c).Take(30).ToList();
            }
            else
            {
                Commandes = ctx.Commandes.Where(x => x.Del == false && x.StatutCmd && !ctx.ReceptionCommandes.Select(rc => rc.CommandeId).Contains(x.Id)).Take(30).ToList();
            }
             
            List<ModelCommande> QcmdModel = new List<ModelCommande>();
            QcmdModel = GetQCommande(Commandes);
            /*if (Commandes.Count > 0)
            {
                foreach (var cmd in Commandes)
                {
                    var _datelivr = cmd.DateLvrCmd != null ? cmd.DateLvrCmd.ToString("dd/MM/yyyy") : "";
                    var client = cmd.Client;
                    List<DetailsCommande> DetailsCmd = new List<DetailsCommande>();
                    IdentiteClient Idclient;
                    if (client != null)
                    {
                        Idclient = new IdentiteClient { Contact = client.Contact, Contact2 = client.Contact2, Email = client.Adresse, Nom = client.Nom, Prenom = client.Prenom };
                    }
                    else
                    {
                        Idclient = new IdentiteClient();
                    }
                    var ligneCmde = cmd.LigneCommandes.Where(x => x.Del == false).ToList()[0];
                    var produit = ligneCmde.Produit;
                    DetailsMonture DetailsMonture = new ViewModel.DetailsMonture {  Designation = produit.Designation, Numero = produit.RefProd, Qte = ligneCmde.QteCmd };

                    var Verres = cmd.ModeleVerres.ToList();
                    var MOD = Verres[0];
                    var MOG = Verres[1];
                    var gammeOD = MOD.GammeVerre != null ? MOD.GammeVerre.Libelle : "-";
                    var gammeOG = MOG.GammeVerre != null ? MOG.GammeVerre.Libelle : "-";
                    var typeOD = Enum.Format(typeof(Models.TypeVerre),MOD.Type,"g");
                    ModelDeVerre OD = new ModelDeVerre { Add = MOD.Add, GammeV = gammeOD, Type = typeOD , VL = new FCTypeVision{ Axe = MOD.VLAxe, Cyl = MOD.VLCyl, Sph = MOD.VLSph}, VP= new FCTypeVision{ Axe = MOD.VPAxe, Cyl = MOD.VPCyl, Sph = MOD.VPSph} };
                    var typeOG = Enum.Format(typeof(Models.TypeVerre), MOG.Type,"g");
                    ModelDeVerre OG = new ModelDeVerre { Add = MOG.Add, GammeV = gammeOG, Type = typeOG , VL = new FCTypeVision{ Axe = MOG.VLAxe, Cyl = MOG.VLCyl, Sph = MOG.VLSph} , VP = new FCTypeVision{ Axe = MOG.VPAxe, Cyl = MOG.VPCyl, Sph = MOG.VPSph} };
                    var natureVerre = Enum.Format(typeof(Models.NatureVerre),cmd.Nature,"g");
                    DetailsVerres DetailsVerre = new DetailsVerres { Nature = natureVerre, Teinte = cmd.Teinte, OD = OD, OG = OG };

                    QcmdModel.Add(new ModelCommande { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), Identite = Idclient, DetailsMonture = DetailsMonture, DetailsVerres = DetailsVerre, DateLivraison = "", Id = cmd.Id, Valider = cmd.StatutCmd, RefCmde = cmd.RefCmd });
                    //QcmdModel.Add(new ModelCommandeDispo { Client = Idclient, Date = cmd.DateCmd.ToString("dd/MM/yyyy"), DateDispo = _datelivr, Id = cmd.Id, RefCmde = cmd.RefCmd });
                }
            }*/
            return Json(QcmdModel, JsonRequestBehavior.AllowGet);
        }

        [Route("commande/{id}/details/", Name="_detailsCommande")]
        public ActionResult DetailsCommande(int id)
        {
            var cmd = ctx.Commandes.Find(id);
            if (cmd == null)
                return HttpNotFound();
            var _datelivr = cmd.DateLvrCmd != null ? cmd.DateRLvrCmd.ToString() : "";
            var client = cmd.Client;
            List<DetailsCommande> DetailsCmd = new List<DetailsCommande>();
            IdentiteClient Idclient;
            if (client != null)
            {
                Idclient = new IdentiteClient { Contact = client.Contact, Contact2 = client.Contact2, Email = client.Adresse, Nom = client.Nom, Prenom = client.Prenom };
            }
            else
            {
                Idclient = new IdentiteClient();
            }
            var ligneCmde = cmd.LigneCommandes.Where(x => x.Del == false).ToList()[0];
            var produit = ligneCmde.Produit;
            DetailsMonture DetailsMonture = new ViewModel.DetailsMonture {  Designation = produit.Designation, Numero = produit.RefProd, Qte = ligneCmde.QteCmd };

            var Verres = cmd.ModeleVerres.OrderBy(x=>x.Id).ToList();
            var MOD = Verres[0];
            var MOG = Verres[1];
            var gammeOD = MOD.GammeVerre != null ? MOD.GammeVerre.Libelle : "-";
            var gammeOG = MOG.GammeVerre != null ? MOG.GammeVerre.Libelle : "-";
            var typeOD = Enum.Format(typeof(Models.TypeVerre),MOD.Type,"g");
            ModelDeVerre OD = new ModelDeVerre { Add = MOD.Add, GammeV = gammeOD, Type = typeOD , VL = new FCTypeVision{ Axe = MOD.VLAxe, Cyl = MOD.VLCyl??"0", Sph = MOD.VLSph??"0"}, VP= new FCTypeVision{ Axe = MOD.VPAxe, Cyl = MOD.VPCyl??"0", Sph = MOD.VPSph??"0"} };
            var typeOG = Enum.Format(typeof(Models.TypeVerre), MOG.Type,"g");
            ModelDeVerre OG = new ModelDeVerre { Add = MOG.Add??"0", GammeV = gammeOG, Type = typeOG , VL = new FCTypeVision{ Axe = MOG.VLAxe, Cyl = MOG.VLCyl??"0", Sph = MOG.VLSph??"0"} , VP = new FCTypeVision{ Axe = MOG.VPAxe, Cyl = MOG.VPCyl??"0", Sph = MOG.VPSph??"0"} };
            var natureVerre = Enum.Format(typeof(Models.NatureVerre),cmd.Nature,"g");
            DetailsVerres DetailsVerre = new DetailsVerres { Nature = natureVerre, Teinte = cmd.Teinte, OD = OD, OG = OG };

            var details = new ModelCommande { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), Identite = Idclient, DetailsMonture = DetailsMonture, DetailsVerres = DetailsVerre, DateLivraison = ""/*, ChargeAssurance = cmd.MontantAssur*/, ChargeClient = cmd.MontantClient, RefCmde = cmd.RefCmd, Id = cmd.Id  };

            return View(details);
        }

        [Route("commande/liste-commande/", Name="_listeCommandeAllType")]
        public ActionResult ListCommande()
        {
            //
            var deb = DateTime.Now;  
            List<QtypeValue> _qtype = new List<QtypeValue> {
                new QtypeValue { Id = 1, Value = "Definitive" }, 
                new QtypeValue { Id = 2, Value = "Provisoire" }, 
                new QtypeValue { Id = 3, Value = "Tout" } };
             ViewBag.Qtype = new SelectList(_qtype,"Id","Value",3);
            return View(new QReglementModel { Deb = deb, End = deb});
        }

        [Route("commande/get-liste-commande/", Name="_QlisteCommandeAllType")]
        [HttpPost]
        public JsonResult QAllCommandeType(DateTime End, DateTime Deb, string nom = null)
        {
            List<QCommandeModel> QcmModel = new List<QCommandeModel>();
            if (ModelState.IsValid)
            {
                List<Commande> Commandes = new List<Commande>();
                List<int> assurances = new List<int>();
                if (!string.IsNullOrEmpty(nom))
                {
                   nom = nom.ToLower();
                    Commandes = (from c in ctx.Commandes
                                 join cl in ctx.Clients on c.ClientId equals cl.Id
                                 where ((cl.Nom.ToLower() == nom || cl.Prenom.ToLower() == nom) || (cl.Nom.ToLower().Contains(nom) || cl.Prenom.ToLower().Contains(nom))) && c.Del==false
                                 select c).OrderBy(x=>x.Client.Nom).Take(50).ToList();
                } else
                {
                    Commandes = ctx.Commandes.Where(x => x.Del == false && x.DateCmd >= Deb && x.DateCmd <= End).OrderBy(x => x.Id).Take(50).ToList();
                }
                
                var listCommandeRecu = ctx.ReceptionCommandes.Select(x => x.CommandeId).ToList();
                if (Commandes.Count > 0)
                {
                    foreach (var cmd in Commandes)
                    {
                        List<DetailsCommande> Details = new List<DetailsCommande>();
                        /*if (cmd.LigneCommandes.Count > 0)
                        {
                            foreach (var lc in cmd.LigneCommandes)
                            {
                                var produit = lc.Produit;
                                var prodModel = new ProduitModel { Designation = produit.Designation, Divers = produit.ModelDivers, Monture = produit.ModelMonture, Verre = produit.ModelVerre };
                                var _designation = FormatDesignation(prodModel);// produit != null ? produit.Designation : "--";
                                var _marque = (produit != null && produit.Marque != null) ? produit.Marque.Libelle : "Sans marque";
                                Details.Add(new DetailsCommande { Designation = _designation, Marque = _marque, Qte = lc.QteCmd });
                            }
                        }*/
                        var client = cmd.Client;
                        var _identite = client != null ? client.Nom.ToUpper() + " " + Title(client.Prenom) : "--";
                        var _adresse = client != null ? new Adresse { Contact = client.Contact, Contact2 = client.Contact2, Adr = client.Adresse } : new Adresse();
                        var _livrer = listCommandeRecu.Contains(cmd.Id);
                        var _estAssure = cmd.AssuranceCommandes.Count > 0 ? true : false;
                        assurances = cmd.AssuranceCommandes.Select(x=> x.AssuranceId).ToList();
                        QcmModel.Add(new QCommandeModel { Id = cmd.Id, Date = cmd.DateCmd.ToString("dd/MM/yyyy"), Valider = cmd.StatutCmd, DateLivraison = cmd.DateLvrCmd.ToString("dd/MM/yyyy"), DetailsCommande = Details, IdentiteClient = _identite, Adresse = _adresse, Livrer = _livrer, RefCommande = cmd.RefCmd, EstAssure = _estAssure, Assurances = assurances });
                    }
                    //Rep.Statut = 200;
                }
            }
            
            return Json(QcmModel, JsonRequestBehavior.AllowGet);
        }

        [Route("commandes/annuler/{id}/", Name="_annulerCommande")] //id de commande
        public JsonResult DelCommande(int id)
        {
            var Commande = ctx.Commandes.Find(id);
            Reponse _rep = new Reponse { Statut = 500, Message = "Impossible d'annuler cette commande" };
            try
            {
                Commande.Del = true;
                Commande.DateDel = DateTime.Now;
                Commande.MotifDel = User.Identity.Name;
               // Commande.PayeAssur = false;
                Commande.PayeClient = false;
                /*if (Commande.TypeCmd == TypeCommande.Definitive) {
                    foreach (var lc in Commande.LigneCommandes.Where(x => x.Del == false).ToList()) {
                        //update stock prod
                        //var stock = lc.QteCmd;
                        var produit = ctx.Produits.Find(lc.ProduitId);
                        try{
                            //var stock = produit.QteStock;
                            produit.QteStock += lc.QteCmd;
                            ctx.SaveChanges();
                        } 
                        catch (DataException /*ex)
                        {
                            _rep.Message = "Une erreur s'est produite lors de la mise à jour du stock du produit Id: " + lc.ProduitId;
                            return Json(_rep, JsonRequestBehavior.AllowGet);
                        }
                    }
                }*/

                var payements = ctx.Payements.Where(x => x.Del == false && x.RefCmd != null && x.RefCmd == id).ToList();
                if (payements.Count > 0)
                {
                    foreach (var p in payements)
                    {
                        p.Del = true;
                        p.MotifDel = User.Identity.Name;
                        p.DateDel = DateTime.Now;
                        ctx.SaveChanges();
                    }
                }
                ctx.SaveChanges();
                _rep.Statut = 200;
            }
            catch (DataException /*ex */)
            {
                return Json(_rep, JsonRequestBehavior.AllowGet);
            }
            return Json(_rep, JsonRequestBehavior.AllowGet);
        }

        [Route("commande/bon-de-commande/{id}/", Name="_GenerateBonDeCommande")] //id de la commande
        public ActionResult BondeCommande(int id)
        {
            var commande = ctx.Commandes.Find(id);
            if (commande == null)
                return HttpNotFound();
            List<LigneBonDeCommande> lignedeCommande = new List<LigneBonDeCommande>();
            /*var lcmd = commande.LigneCommandes.Where(x => x.Del == false).ToList();
            if (lcmd.Count > 0) 
            {
                foreach(var lc in lcmd) 
                {
                    var _mtt = lc.PrixCmd * lc.QteCmd;
                    var _rem = lc.RemDG + lc.Rem;
                    _mtt -= _rem;
                    var _designation = _getDesignation(lc.Produit);
                    lignedeCommande.Add( new LigneBonDeCommande{ Designation = _designation, Prix = lc.PrixCmd, Qte = lc.QteCmd, Rem = _rem, Montant = _mtt});
                }
            }*/
            var payement = ctx.Payements.Where(x => x.Del == false && x.RefCmd == id && x.MontantEncaisse).OrderBy(x=> x.Id).FirstOrDefault();
            float _mttVerse = 0, _resteApayer = 0, _mttTotal = 0;
            if (payement != null)
            {
                _mttVerse = payement.MontantPaye;
                _resteApayer = commande.MontantClient - _mttVerse;
            }
            var mttAssur = commande.AssuranceCommandes.ToList().Sum(x => x.MontantAsssur);
            _mttTotal = commande.MontantClient + mttAssur;
            var BondeCommande = new BonDeCommandeModel { MttAssur = mttAssur, MttClient = commande.MontantClient, Contact = commande.Client.Contact, Nom = commande.Client.Nom, Prenom = commande.Client.Prenom, Reference = commande.RefCmd, MontantTotal = _mttTotal, MttVerse = _mttVerse, ResteApayer = _resteApayer, LigneBonDeCommandes = lignedeCommande, ReductionClient = commande.ReductionClient };
            return View(BondeCommande);
        }

        [Route("commande/generate-bon-de-commande/{id}/{destinataire?}/", Name="__generateFicheBondeCommande")]
        public JsonResult GenerateFicheCommande(int id, int? destinataire = null)
        {
            var file_name = "bondecommande.pdf";
            var _Url = Url.RouteUrl("_FicheBondeCommande", new {id = id, destinataire = destinataire });
            var statut = generateReportFile(_Url, file_name);
            if (statut == "OK")
                return Json(new { Result = "OK", Message = file_name }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Result = "ERROR", Message = "Impossible de generer une facture" }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("commande/fiche-bon-de-commande/{id}/{destinataire?}/", Name = "_FicheBondeCommande")]
        public ActionResult FicheBondeCommande(int id, int? destinataire = null)
        {
            var cmd = ctx.Commandes.Find(id);
            if (cmd == null)
                return HttpNotFound();
            var _datelivr = cmd.DateLvrCmd != null ? cmd.DateRLvrCmd.ToString() : "";
            var client = cmd.Client;
            List<DetailsCommande> DetailsCmd = new List<DetailsCommande>();
            IdentiteClient Idclient;
            if (client != null)
            {
                var _civilite = Enum.Format(typeof(Models.Civilite),client.Civilite,"g");
                Idclient = new IdentiteClient { Contact = client.Contact, Contact2 = client.Contact2, Email = client.Adresse, Nom = client.Nom.ToUpper(), Prenom = Title(client.Prenom), Civilite = _civilite };
            }
            else
            {
                Idclient = new IdentiteClient();
            }
            var _ligneCmde = cmd.LigneCommandes.Where(x => x.Del == false).ToList();
            var ligneCmde = _ligneCmde.Count > 0 ? _ligneCmde[0] : new LigneCommande();
            var produit = _ligneCmde.Count > 0 ? ligneCmde.Produit: new Produit();
            var _remiseTotalMonture = ligneCmde.Rem + ligneCmde.RemDG;
            DetailsMonture DetailsMonture = new ViewModel.DetailsMonture { Designation = produit.Designation, Numero = produit.RefProd, Qte = ligneCmde.QteCmd, Montant = ligneCmde.PrixCmd, Remise = _remiseTotalMonture };

            var Verres = cmd.ModeleVerres.OrderBy(x=>x.Id).ToList();
            var MOD = Verres.Count > 0 ? Verres[0] : new ModelVerre();
            var MOG = Verres.Count >=2 ? Verres[1] : new ModelVerre();
            var gammeOD = MOD.GammeVerre != null ? MOD.GammeVerre.Libelle : "-";
            var gammeOG = MOG.GammeVerre != null ? MOG.GammeVerre.Libelle : "-";
            var typeOD = Enum.Format(typeof(Models.TypeVerre), MOD.Type, "g");
            var _remiseTotalOD = MOD.Remise + MOD.RemiseDG;
            ModelDeVerre OD = new ModelDeVerre { Add = MOD.Add, GammeV = gammeOD, Type = typeOD, VL = new FCTypeVision { Axe = MOD.VLAxe, Cyl = MOD.VLCyl, Sph = MOD.VLSph }, VP = new FCTypeVision { Axe = MOD.VPAxe, Cyl = MOD.VPCyl, Sph = MOD.VPSph }, Montant = MOD.Prix, Remise = _remiseTotalOD, Qte = MOD.Qte };
            var _remiseTotalOG = MOG.Remise + MOG.RemiseDG;
            var typeOG = Enum.Format(typeof(Models.TypeVerre), MOG.Type, "g");
            ModelDeVerre OG = new ModelDeVerre { Add = MOG.Add, GammeV = gammeOG, Type = typeOG, VL = new FCTypeVision { Axe = MOG.VLAxe, Cyl = MOG.VLCyl, Sph = MOG.VLSph }, VP = new FCTypeVision { Axe = MOG.VPAxe, Cyl = MOG.VPCyl, Sph = MOG.VPSph }, Montant = MOG.Prix, Remise = _remiseTotalOG, Qte = MOG.Qte };
            var natureVerre = Enum.Format(typeof(Models.NatureVerre), cmd.Nature, "g");
            DetailsVerres DetailsVerre = new DetailsVerres { Nature = natureVerre, Teinte = cmd.Teinte, OD = OD, OG = OG };
            var _nomAssurance = "";
            float montantAssurance = 0;
            List<AssuranceCharge> assuranceCharge = new List<AssuranceCharge>();
            if(destinataire != null) {
                var assurcmd = cmd.AssuranceCommandes.Where(x => x.AssuranceId == destinataire).FirstOrDefault();
                if (assurcmd != null)
                {
                    _nomAssurance = assurcmd.Assurance.Nom;
                    montantAssurance = assurcmd.MontantAsssur;
                }
            }
            else
            {
                int nbAssur = cmd.AssuranceCommandes.Count;
                if (nbAssur > 0)
                {
                    var index = 0;
                    foreach (var x in cmd.AssuranceCommandes)
                    {
                        index++;
                        var sepa = index < nbAssur ? " - " : "";
                        _nomAssurance += x.Assurance.Code + sepa;
                        assuranceCharge.Add(new AssuranceCharge{ CodeAssurance = x.Assurance.Code, Montant = x.MontantAsssur});
                    }
                } 
            }


            var details = new ModelCommande { Date = cmd.DateCmd.ToString("dd/MM/yyyy"), Identite = Idclient, DetailsMonture = DetailsMonture, DetailsVerres = DetailsVerre, DateLivraison = "", ChargeAssurance = montantAssurance, ChargeClient = cmd.MontantClient, RefCmde = cmd.RefCmd, Id = cmd.Id, NomAssurance = _nomAssurance, ReductionClient = cmd.ReductionClient, AssuranceCharge = assuranceCharge };
            ViewBag.destinataire = destinataire;
            return View(details);
        }

        [Route("commande/proformat/", Name="_addCommandeProforma")]
        public ActionResult AddCommandeProforma()
        {
            var proformat = new ProformaModel { Monture = new Monture(), Verre = new Verre() };
            return View("Proforma", proformat);
        }

        [Route("commande/proformat/post/", Name="_proformatCommandePost")]
        [HttpPost]
        public JsonResult PostFormat(ProformaModel pfModel)
        {
            _proforma = pfModel;
            var date = DateTime.Now;
            var numero = ctx.FactureProformas.OrderByDescending(x => x.Id).FirstOrDefault();// (int.Parse(date.Year.ToString().Substring(2)) + date.Month + date.Day) + "" + date.ToString("Hm");
            string _num = "";
            if (numero == null)
            {
                _num = 1.ToString();
            }
            else
            {
                _num = (numero.Id + 1).ToString();
            }
            pfModel.NumeroProforma = _num; 
            try
            {
                //return RedirectToRoute("_generateProforma");
                 var file_name = pfModel.Nom.ToUpper() + pfModel.Prenom.ToUpper() + date.Day + date.Month + date.Hour + date.Minute + ".pdf";
                var _Url = Url.RouteUrl("_generateProforma");
                var statut = generateReportFile(_Url, file_name);
                ctx.FactureProformas.Add(new FactureProforma { Nom = pfModel.Nom, Prenom = pfModel.Prenom, ReferenceRecu = file_name, Date = DateTime.Now, NumeroProforma = _num });
                ctx.SaveChanges();
                if (statut == "OK")
                    return Json(new { Result = "OK", Message = file_name }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Result = "ERROR", Message = "Impossible de generer une facture" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception /*ex*/)
            {
                return Json(new { Result = "ERROR", Message = "Impossible de generer une facture" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [Route("commande/generate-proforma/", Name="_generateProforma")]
        public ActionResult Generateproforma()
        {
            /*if (_proforma.Assurance != null)
                return View("ProformaAssurance",_proforma);
            else */
            var prenom = Title(_proforma.Prenom);
            _proforma.Prenom = prenom;
            return View("ProformaModel",_proforma);
        }

        [Route("commande/get-list-assurance/{id}/", Name="_getCommandeListeAssurance")]
        public JsonResult GetCommandeListAssurance(int id)
        {
            var result = ctx.AssuranceCommandes.Where(x => x.CommandeId == id).Select(x => new { assurId = x.AssuranceId, charge = x.MontantAsssur, Id = x.Id}).OrderBy(x => x.Id).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}