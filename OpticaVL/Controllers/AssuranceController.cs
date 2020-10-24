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
    public class AssuranceController : BaseController 
    {
       [Route("assurances/", Name="_ListeAssurance")]
       /*[ChildActionOnly]*/
        public ActionResult Index()
        {  
           //return PartialView("IndexPartialView",ctx.Assurances.Where(x => x.Del == false).OrderBy(x=>x.Nom).ToList());
            return View("Index2");
        }

        [Route("assurances/liste-assurances/", Name="_getListeAssurance")]
       public JsonResult getListeAssurance()
       {
           List<AssuranceModel> listeAssurance = ctx.Assurances.Where(x => x.Del == false).OrderBy(x => x.Nom).Select(x => new AssuranceModel { Id = x.Id, Code = x.Code.ToUpper(), Nom = x.Nom.ToUpper() }).ToList();
            return Json(listeAssurance, JsonRequestBehavior.AllowGet);
       }

        [Route("assurances/add", Name="_addAssurance")]
        public ActionResult Add()
        {
            return View("Assurance", new Assurance());
        }

        [Route("assurances/add", Name="_addPostAssurance")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(Assurance assurance)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _code = assurance.Code.Trim().ToUpper();
                    var _nom = assurance.Nom.Trim();
                    var oldAssurance = ctx.Assurances.Where(x=>x.Code.ToUpper() == _code || x.Nom.ToLower() == _nom.ToLower()).FirstOrDefault();
                    if (oldAssurance == null)
                    {
                        var _assurance = new Assurance
                        {
                            Code = _code,
                            Nom = _nom.ToUpper()
                        };
                        ctx.Assurances.Add(_assurance);
                        ctx.SaveChanges();
                    }
                    return RedirectToRoute("_ListeAssurance");
                }
            }
            catch (DataException /*ex*/)
            {
                ModelState.AddModelError("", "Impossible d'enregistrer une nouvelle assurance");
            }
            return View("Assurance", assurance);
        }

        [Route("assurances/{id}", Name="_updateAssurance")]
        public ActionResult Update(int id)
        {
            var assurance = ctx.Assurances.Find(id);
            if (assurance == null)
                return HttpNotFound();
            return View("Assurance", assurance);
        }

        [Route("assurances/{id}", Name="_updatePostAssurance")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id, Assurance assurance)
        {
            var assuranceToUpdate = ctx.Assurances.Find(id);
            try
            {
                assuranceToUpdate.Code = assurance.Code.Trim().ToUpper();
                assuranceToUpdate.Nom = assurance.Nom.Trim().ToUpper();
                ctx.SaveChanges();
                return RedirectToRoute("_ListeAssurance");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Impossible de proceder à une mise à jour");
            }
            return View("Assurance", assurance);
        }
	}
}