using OpticaVL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
   [Authorize]
    public class GammeVerreController : BaseController
    {
        //
        [Route("gamme-verres/", Name="_listeGammeVerre")]
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView("GammeVerrePartialView", ctx.GammeVerres.OrderBy(x=>x.Libelle).ToList());
        }

        [Route("gamme-verres/add", Name="_gammeVerreAdd")]
        public ActionResult Add()
        {
            return View(new GammeVerre());
        }

        [Route("gamme-verres/add", Name="_gammeVerrePostadd")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostGammeVerre([Bind(Include="Libelle")] GammeVerre gv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _libelle = gv.Libelle.Trim().ToLower();
                    var _oldGv = ctx.GammeVerres.Where(x => x.Libelle.ToLower() == _libelle).FirstOrDefault();
                   if (_oldGv == null)
                   {
                       gv.Libelle = _libelle;
                       ctx.GammeVerres.Add(gv);
                       ctx.SaveChanges();
                   }
                    return RedirectToRoute("_gammeVerrePostadd");
                }
            }
            catch (DataException /* ex*/)
            {
                ModelState.AddModelError("","Une Erreur s'est produite lors de l'ajout");
            }
            return View("Add",gv);
        }

        [Route("gamme-verres/{id}", Name="_updateGammeVerre")]
        public ActionResult Update(int id) 
        {
            var gv = ctx.GammeVerres.Find(id);
            if (gv == null)
                return HttpNotFound();
            return View("Add",gv);
        }

        [Route("gamme-verres/{id}", Name="_updateGammeVerrePost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id, GammeVerre gamme /*, [Bind(Include="Libelle")] GammeVerre gv*/)
        {
            var gv = ctx.GammeVerres.Find(id);
            /*if (TryUpdateModel(gv, new string[] {"Libelle"}))
            {*/
                try
                {
                    var _libelle = gamme.Libelle.Trim().ToLower();
                    var oldgv = ctx.GammeVerres.Where(x => x.Libelle.ToLower() == _libelle && x.Id != id).FirstOrDefault();
                    if (oldgv == null)
                    {
                        gv.Libelle = _libelle;
                        ctx.SaveChanges();
                        return RedirectToRoute("_gammeVerrePostadd");
                    }
                }
                catch (RetryLimitExceededException /*e x*/)
                {
                    ModelState.AddModelError("", "Une Erreur s'est produite lors de la mise à jour");
                }

            //}
            return View("Add",gv);
        }
	}
}