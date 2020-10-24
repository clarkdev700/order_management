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
    public class ReceptionCommandeController : CommandeController
    {
        //
        // GET: /ReceptionCommande/
        public ActionResult Index()
        {
            return View();
        }

        [Route("liste-commande/reception/", Name="_IndexReceptionCommande")]
        public ActionResult IndexReception()
        {
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View("IndexReception2");
        }

        [Route("commande/liste-reception/", Name="_getlisteReceptionCommande")]
        public JsonResult ListeCommande(DateTime deb, DateTime end, string nom = null)
        {
            List<Commande> listeCommande = new List<Commande>();//ctx.Commandes.Where(x => x.Del == false && x.DateCmd >= deb && x.DateCmd <= end).ToList();
            if (!string.IsNullOrEmpty(nom))
            {
                nom = nom.ToLower();
                listeCommande = (from c in ctx.Commandes
                            join cl in ctx.Clients on c.ClientId equals cl.Id
                            join rc in ctx.ReceptionCommandes on c.Id equals rc.CommandeId
                            where (cl.Nom.ToLower() == nom || cl.Prenom.ToLower() == nom) || (cl.Nom.ToLower().Contains(nom) || cl.Prenom.ToLower().Contains(nom)) && c.Del == false
                            orderby cl.Nom
                            select c).Take(25).ToList();
            }
            else
            {
                listeCommande = (from c in ctx.Commandes
                                 join rc in ctx.ReceptionCommandes on c.Id equals rc.CommandeId
                                 where c.Del == false && rc.DateReception >= deb && rc.DateReception <= end
                                 orderby rc.DateReception
                                 select c).Take(25).ToList();
            }

            List<ModelCommande> Commandelivrer = new List<ModelCommande>();
            Commandelivrer = GetQCommande(listeCommande);
            /*if (listeCommande.Count > 0)
            {
                foreach (var cmde in listeCommande)
                {
                    var statutReception = ctx.ReceptionCommandes.Select(x => x.CommandeId).Contains(cmde.Id);
                    var client = cmde.Client.Nom.ToUpper() + " " + Title(cmde.Client.Prenom);
                    float _resteApayerAssur = 0;
                    if (cmde.MontantAssur > 0 && !cmde.PayeAssur)
                    {
                        var mttAssurVerse = cmde.Payements.Where(x => x.Del == false && x.MontantEncaisse && x.SourcePaye == Models.SourcePaye.Assurance).ToList().Sum(x=>x.MontantPaye);
                        _resteApayerAssur = cmde.MontantAssur - mttAssurVerse;
                    }
                    float _resteApayerClient = 0;
                    if (!cmde.PayeClient && cmde.MontantClient > 0)
                    {
                        var mttClVerse = cmde.Payements.Where(x => x.Del == false && x.MontantEncaisse && x.SourcePaye == Models.SourcePaye.Client).ToList().Sum(x=>x.MontantPaye);
                        _resteApayerClient = cmde.MontantClient - mttClVerse;
                    }
                    CommandeAlivrer.Add(new ModelCommandeALivrer {  ChargeAssurance = _resteApayerAssur, ChargeClient = _resteApayerClient, Date = cmde.DateCmd.ToString("dd/MM/yyyy"), Id = cmde.Id, Identite = client, Ref = cmde.RefCmd, StatutConception = cmde.StatutCmd, StatutLivraison = statutReception});
                }
            }*/
            return Json(Commandelivrer,JsonRequestBehavior.AllowGet);
        }

        [Route("fiche-commande/reception/{id}", Name="_addReceptionCmde")] //id de commande
        public ActionResult AddReception(int id)
        {
            var commande = ctx.Commandes.Find(id);
            if (commande == null)
                return HttpNotFound();
            ReceptionCommande ReceptionCmd;
            ReceptionCmd = ctx.ReceptionCommandes.Find(id);
            if (ReceptionCmd == null)
                ReceptionCmd = new ReceptionCommande { DateReception = DateTime.Now };
            ViewBag.Commande = commande;
            return View(ReceptionCmd);
        }

        [Route("fiche-commande/reception/{id}", Name="_addPostReceptionCommande")] //id de la commande
        [HttpPost]
        public ActionResult AddPostReception(int id, ReceptionCommande Rcmde) 
        {
            if (ModelState.IsValid)
            {
                var ReceptionCmde = ctx.ReceptionCommandes.Where(x => x.CommandeId == id).FirstOrDefault();
                if (ReceptionCmde == null)
                {
                    //creer une nvel reception Cmde
                    var Receptioncmde = new ReceptionCommande { CommandeId = id, Commentaire = Rcmde.Commentaire, DateEnreg = DateTime.Now, DateReception = Rcmde.DateReception, IdentiteReceptionnaire = Rcmde.IdentiteReceptionnaire };
                    ctx.ReceptionCommandes.Add(Receptioncmde);
                }
                else
                {
                    ReceptionCmde.Commentaire = Rcmde.Commentaire;
                    ReceptionCmde.DateReception = Rcmde.DateReception;
                    ReceptionCmde.IdentiteReceptionnaire = Rcmde.IdentiteReceptionnaire;
                }
                ctx.SaveChanges();
                return RedirectToRoute("_IndexReceptionCommande");
            }
            else
            {
                ModelState.AddModelError("", "Valeur saisi incorrect!");
            }
            return RedirectToRoute("_addReceptionCmde", new { id = id });
        }

       [Route("fiche-commande/annuler-reception/{id}/", Name="_annuleReceptionCmde")] //id commande
        public ActionResult AnnuleReception(int id)
        {
            var reception = ctx.ReceptionCommandes.Where(x=>x.CommandeId == id).FirstOrDefault();
            Reponse response = new Reponse { Message = "", Statut = 500 };
            if (reception != null)
            {
                ctx.ReceptionCommandes.Remove(reception);
                ctx.SaveChanges();
                response.Statut = 200;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
	}
}