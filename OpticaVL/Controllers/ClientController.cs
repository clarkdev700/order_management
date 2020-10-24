using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        [Route("clients/", Name="_listeClient")]
        public ActionResult Index()
        {
          
            return View(ctx.Clients.Where(x=> x.Del == false).ToList());
        }

        [Route("clients/add", Name="_addClient")]
        public ActionResult Add()
        {
            return View("Client", new Client());
        }

     [Route("clients/add", Name="_addPostClient")]
     [HttpPost]
     [ValidateAntiForgeryToken]
        public ActionResult AddPost([Bind(Include = "Nom, Prenom, Contact, Contact2, Adresse, Profession")] Client client)
        {
            try {
                if (ModelState.IsValid)
                {
                    var _nom = client.Nom.Trim().ToUpper();
                    var _prenom = client.Prenom.Trim();
                    var _prof = client.Profession == null ? client.Profession : client.Profession.Trim();
                    client.Profession = _prof;
                    client.Nom = _nom;
                    client.Prenom = _prenom;
                    ctx.Clients.Add(client);
                    ctx.SaveChanges();
                    return RedirectToRoute("_listeClient");
                }

            }
            catch (DataException /*ex*/)
            {
                ModelState.AddModelError("", "Impossible d'ajouter un nouveau client");
            }
            return View("Client", client);
        }

     [Route("clients/{id}", Name="_updateClient")]
     public ActionResult Update(int id)
     {
         var client = ctx.Clients.Find(id);
         if (client == null)
             return HttpNotFound();
         return View("Client", client);
     }

    [Route("clients/{id}", Name="_updatePostClient")]
    [HttpPost]
    [ValidateAntiForgeryToken]
     public ActionResult UpdatePost(int id, Client client)
     {
         Client clientToUp = ctx.Clients.Find(id);
         try
         {
             clientToUp.Nom = client.Nom.Trim().ToUpper();
             clientToUp.Prenom = client.Prenom.Trim();
             clientToUp.Contact = client.Contact;
             clientToUp.Contact2 = client.Contact2;
             clientToUp.Profession = client.Profession;
             clientToUp.Adresse = client.Adresse;
             ctx.SaveChanges();
             return RedirectToRoute("_listeClient");
         }
         catch (DataException /* ex */)
         {
             ModelState.AddModelError("","Impossible de modifier les informations du client.");
         }
         return View("Client", client);
     }
	}
}