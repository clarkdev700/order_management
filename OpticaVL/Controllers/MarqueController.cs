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
    public class MarqueController : BaseController
    {
        //
        // GET: /Marque/
        [Route("marques/liste", Name="_listeMarque")]
        /*[ChildActionOnly]*/
        public ActionResult Index()
        {
           /*List<Marque> listeMarque = ctx.Marques.ToList();
            return PartialView("MarquePartial", listeMarque);*/
            return View("Index2");
        }

        [Route("marques/liste-marques/", Name="_getMarques")]
        public JsonResult GetListeMarque()
        {
            List<MarqueViewModel> listeMarque = ctx.Marques.OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList();
            return Json(listeMarque, JsonRequestBehavior.AllowGet);
        }

        [Route("marques/add", Name="_addMarque")]
        public ActionResult Add()
        {
            return View("Marque", new Marque());
        }

        [Route("marques/add", Name="_postAddMarque")]
        [HttpPost]
        public ActionResult Add(/*[Bind(Include = "Libelle")]*/Marque marque)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _lib = marque.Libelle.Trim().ToUpper();
                    marque.Libelle = _lib;
                    var oldMarque = ctx.Marques.Where(x => x.Libelle.ToUpper() == _lib).FirstOrDefault();
                    if (oldMarque == null)
                    {
                        ctx.Marques.Add(marque);
                        ctx.SaveChanges();
                    }
                    return RedirectToRoute("_listeMarque");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Impossible d'enregistrer une nouvelle marque");
            }
            return RedirectToRoute("_addMarque");
        }

        [Route("marques/{id}", Name="_updateMarque")]
        public ActionResult Update(int id)
        {
            var _marque = ctx.Marques.Find(id);
            if (_marque == null)
                return HttpNotFound();
            return View("Marque",_marque);
        }

        [Route("marques/{id}", Name="_updatePostMarque")]
        [HttpPost]
        public ActionResult UpdatePost(int id, Marque marque)
        {
            var marqueToUpdate = ctx.Marques.Find(id);
           /* if (TryUpdateModel(marqueToUpdate, "",
               new string[] { "Libelle" }))
            {*/
                try
                {
                    var _lib = marque.Libelle.Trim().ToUpper();
                    var oldMarque = ctx.Marques.Where(x => x.Libelle == _lib && x.Id != id).FirstOrDefault();
                    marqueToUpdate.Libelle = _lib;
                    ctx.SaveChanges();
                    return RedirectToRoute("_listeMarque");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Impossible de proceder à une mise à jour");
                }
           // }
            return View("Marque", marqueToUpdate);
        }
	}
}