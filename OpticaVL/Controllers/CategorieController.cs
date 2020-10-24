using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    [Authorize]
    public class CategorieController : BaseController
    {
        [Route("categories/", Name="_indexCategorie")]
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView("IndexPartialView", ctx.Categories.Where(x => x.Del == false).OrderBy(x=>x.Libelle).ToList());
        }

        [Route("categorie/add/", Name="_addCategorie")]
        public ActionResult Add()
        {
            return View("Categorie",new Categorie());
        }

        [Route("categorie/add/", Name="_addPostCategorie")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(/*[Bind(Include="Code,Libelle,")]*/ Categorie c)
        {
            if (ModelState.IsValid)
            {
                
                var code = c.Code.Trim().ToUpper();
                var libelle = c.Libelle.Trim().ToUpper();
                var oldCat = ctx.Categories.Where(x => x.Code.ToUpper() == code || x.Libelle.ToUpper() == libelle).FirstOrDefault();
                if (oldCat == null)
                {
                    var categorie = new Categorie
                    {
                        Code = code,
                        Libelle = libelle
                    };
                    ctx.Categories.Add(categorie);
                    ctx.SaveChanges();
                }
                return RedirectToRoute("_addCategorie");
            }
            return View("Categorie", new Categorie());
        }

        [Route("categorie/{id}", Name="_updateCategorie")]
        public ActionResult Update(int id)
        {
            Categorie categorie = ctx.Categories.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View("Categorie", categorie);     
        }

        [Route("categorie/{id}", Name="_updatePostCategorie")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id, Categorie Cat)
        {
            var categorie = ctx.Categories.Find(id);
            /*if (TryUpdateModel(categorie, new string[] { "Code", "Libelle" }))
            {*/
                try
                {
                   var _code = Cat.Code.ToUpper();
                    var _libelle = Cat.Libelle.ToUpper();
                    var oldCat = ctx.Categories.Where(x => (x.Code == _code || x.Libelle == _libelle) && x.Id != id).FirstOrDefault();
                    if (oldCat == null)
                    {
                        categorie.Libelle = _libelle;
                        categorie.Code = _code;
                        ctx.SaveChanges();
                        return RedirectToRoute("_addCategorie");
                    }
                }
                catch (RetryLimitExceededException /*dex*/)
                {
                    ModelState.AddModelError("", "Impossible de proceder à une mise à jour");
                }
            //}
            return View("Categorie", categorie);
        }
	}
}