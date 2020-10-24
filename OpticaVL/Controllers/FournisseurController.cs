using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class FournisseurController : BaseController
    {
        [Route("fournisseurs/", Name="_indexFournisseur")]
        public ActionResult Index()
        {
            var fournisseur = ctx.Fournisseurs.Where(x => x.Del == false).OrderBy(x=>x.Nom).ToList();
            return View(fournisseur);
        }

        [Route("fournisseurs/add", Name="_addFournisseur")]
        public ActionResult Add()
        {
           
            return View("Fournisseur",new Fournisseur());
        }

        [Route("fournisseurs/add", Name="_addPostFournisseur")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(Fournisseur _fournisseur)
        {
            if (ModelState.IsValid)
            {
                var oldfournisseur = ctx.Fournisseurs.Where(x => x.Code.ToLower() == _fournisseur.Code.ToLower()).FirstOrDefault();
                if (oldfournisseur == null)
                {
                    var fournisseur = new Fournisseur
                    {
                        Adresse = !string.IsNullOrEmpty(_fournisseur.Adresse) ? _fournisseur.Adresse.Trim() : null,
                        Code = _fournisseur.Code.Trim().ToUpper(),
                        Contact = !string.IsNullOrEmpty(_fournisseur.Contact) ? _fournisseur.Contact.Trim() : null,
                        Contact2 = !string.IsNullOrEmpty(_fournisseur.Contact2) ? _fournisseur.Contact2.Trim() : null,
                        Email = !string.IsNullOrEmpty(_fournisseur.Email) ? _fournisseur.Email.Trim() : null,
                        Nom = _fournisseur.Nom.Trim()
                    };
                    ctx.Fournisseurs.Add(fournisseur);
                    ctx.SaveChanges();
                    return RedirectToRoute("_indexFournisseur");
                }
                ModelState.AddModelError("", "Veuillez attribuer un nouveau code au fournisseur.");
            }
            ModelState.AddModelError("","Une erreur s'est produite merci de ressayer");
            return View("Fournisseur", _fournisseur);
        }

        [Route("fournisseurs/{id}", Name="_updateFournisseur")]
        public ActionResult Update(int id)
        {
            var fournisseur = ctx.Fournisseurs.Find(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View("Fournisseur",fournisseur);
        }

        [Route("fournisseurs/{id}", Name="_updatePostFournisseur")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id)
        {
            var fournisseur = ctx.Fournisseurs.Find(id);
            if (TryUpdateModel(fournisseur,new string[]{ "Nom","Contact","Contact2","Email","Adresse"})) 
            {
                try {
                    //fournisseur.Nom = 
                    ctx.SaveChanges();
                    return RedirectToRoute("_indexFournisseur");
                } 
                catch(RetryLimitExceededException) {

                }
            }
            return View(fournisseur);
        }
	}
}