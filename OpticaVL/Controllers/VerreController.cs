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
    public class VerreController : BaseController
    {
        //
        // GET: /Verre/
        [Route("verres/", Name="_listeVerre")]
        public ActionResult Index()
        {
            return View();
        }

       [Route("verres/modele-verre-list/", Name="_getVerreList")]
       public ActionResult GetVerreListe()
        {
            List<GammeVerrePuissanceModel> GVP = new List<GammeVerrePuissanceModel>();
            var listVerre = (from mv in ctx.MVerres
                             join gv in ctx.GammeVerres on mv.GammeVerreId equals gv.Id into mvGroup
                             from subq in mvGroup.DefaultIfEmpty()
                             select new { mv = mv, gv = (subq == null ? null : subq) }).ToList();
            if (listVerre.Count > 0)
            {
                foreach (var item in listVerre.OrderBy(x=>x.mv.Id).ToList())
                {
                    var mv = item.mv;
                    var gvLib = item.gv != null ? item.gv.Libelle : null;
                    string typeverre = Enum.Format(typeof(TypeVerre),mv.TypeVerre,"g");
                    string sideVerre = mv.Side != null ? Enum.Format(typeof(Side),mv.Side,"g") : null; 
                    GVP.Add(new GammeVerrePuissanceModel { Addition = mv.Add, Cycl = mv.Cyl, Sph = mv.Sph, GammeVerre = gvLib, Qte = mv.Qte, TypeVerre = typeverre, IdGVP = mv.Id, Side = sideVerre });
                }
            }

            return Json(GVP, JsonRequestBehavior.AllowGet);
        }

        [Route("verres/add", Name="_AddVerre")]
        public ActionResult Add()
        {
            var listeTypeVerre = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "Unifocaux" }, new QtypeValue { Id = 1, Value = "Progressif" }, new QtypeValue { Id = 2, Value = "Bifocaux" } };
            ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x => x.Value).ToList(), "Id", "Value");
            List<GammeVerreModel> GammeVerres = ctx.GammeVerres.OrderBy(x => x.Libelle).Select(x => new GammeVerreModel { Id = x.Id, Libelle = x.Libelle.ToLower() }).ToList();
            ViewBag.GammeVerre = new SelectList(GammeVerres,"Id","Libelle");
            ViewBag.Side = new SelectList(new List<QtypeValue> { new QtypeValue { Id = 0, Value = "OD" }, new QtypeValue { Id = 1, Value = "OG" } }, "Id", "Value");
            ViewBag.State = true;
            ViewBag.Dverre = new Dverre();
            return View(new FicheVerreModel() { });
        }

        [Route("verres/add", Name="_AddVerrePost")]
        [HttpPost]
        public ActionResult AddPost(FicheVerreModel fvModel)
        {
            if (ModelState.IsValid)
            {
                if (VerrExiste(fvModel, true) == null)
                {
                    //on cree un nouveau model de verre
                    var _mverre = new MVerre { TypeVerre = fvModel.TypeVerre, Sph = fvModel.Sph, Cyl = fvModel.Cycl, Add = fvModel.Addition, GammeVerreId = fvModel.GammeVerreId, Qte = fvModel.Qte, Side = fvModel.Side};
                    ctx.MVerres.Add(_mverre);
                    ctx.SaveChanges();
                }
                else
                {
                    var VerreTrouve = VerrExiste(fvModel, true);
                    if (VerreTrouve != null)
                    {
                        VerreTrouve.Qte += fvModel.Qte;
                        ctx.SaveChanges();
                    }
                }
                return RedirectToRoute("_listeVerre");
            }
            return View(fvModel);
        }

        [Route("verres/{id}/", Name="_UpdateVerre")] //id Mverre
        public ActionResult UpdateVerre(int id)
        {
            var _mverre = ctx.MVerres.Find(id);
            if (_mverre == null)
                return HttpNotFound();
            string typeVerre =  ((int)_mverre.TypeVerre).ToString();
            string gammeVerre = _mverre.GammeVerreId != null ? ((int)_mverre.GammeVerreId).ToString() : null;
            string sideVerre = _mverre.Side != null ? ((int)_mverre.Side).ToString() : null;
            var ficheverreModel = new FicheVerreModel { Addition = _mverre.Add, Cycl = _mverre.Cyl, Qte = _mverre.Qte, Sph = _mverre.Sph };
            var listeTypeVerre = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "Unifocaux" }, new QtypeValue { Id = 1, Value = "Progressif" }, new QtypeValue { Id = 2, Value = "Bifocaux" } };
            ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x => x.Value).ToList(), "Id", "Value");
            List<GammeVerreModel> GammeVerres = ctx.GammeVerres.OrderBy(x => x.Libelle).Select(x => new GammeVerreModel { Id = x.Id, Libelle = x.Libelle.ToLower() }).ToList();
            ViewBag.GammeVerre = new SelectList(GammeVerres, "Id", "Libelle");
            var listeSide = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "OD" }, new QtypeValue { Id = 1, Value = "OG" } };
            ViewBag.Side = new SelectList(listeSide, "Id", "Value");
            ViewBag.Dverre = new Dverre { GammeVerre = gammeVerre, SideVerre = sideVerre, TypeVerre = typeVerre };
            ViewBag.State = true;
            return View("Add", ficheverreModel);
        }

        [Route("verres/{id}/", Name="_UpdatePostVerre")]
        [HttpPost]
        public ActionResult UpdatePost(int id, FicheVerreModel fvModel)
        {
            var _mverre = ctx.MVerres.Find(id);
            try
            {
                _mverre.Add = fvModel.Addition;
                _mverre.Cyl = fvModel.Cycl;
                _mverre.GammeVerreId = fvModel.GammeVerreId;
                _mverre.Qte = fvModel.Qte;
                _mverre.Side = fvModel.Side;
                _mverre.Sph = fvModel.Sph;
                _mverre.TypeVerre = fvModel.TypeVerre;
                ctx.SaveChanges();
                return RedirectToRoute("_listeVerre");
            }
            catch (DataException ex)
            {

            }
            return RedirectToRoute("_UpdateVerre", new { id = id});
        }

        [Route("verres/update-stock/{id}/", Name = "_UpdateStockVerre")]
        public ActionResult UpdateStock(int id)
        {
            var _mverre = ctx.MVerres.Find(id);
            if (_mverre == null)
                return HttpNotFound();
            string typeVerre = _mverre.TypeVerre != null ? ((int)_mverre.TypeVerre).ToString() : null;
            string gammeVerre = _mverre.GammeVerreId != null ? ((int)_mverre.GammeVerreId).ToString() : null;
            string sideVerre = _mverre.Side != null ? ((int)_mverre.Side).ToString() : null;
            var ficheverreModel = new FicheVerreModel { Addition = _mverre.Add, Cycl = _mverre.Cyl, Qte = _mverre.Qte, Sph = _mverre.Sph };
            var listeTypeVerre = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "Unifocaux" }, new QtypeValue { Id = 1, Value = "Progressif" }, new QtypeValue { Id = 2, Value = "Bifocaux" } };
            ViewBag.TypeVerre = new SelectList(listeTypeVerre.OrderBy(x => x.Value).ToList(), "Id", "Value");
            List<GammeVerreModel> GammeVerres = ctx.GammeVerres.OrderBy(x => x.Libelle).Select(x => new GammeVerreModel { Id = x.Id, Libelle = x.Libelle.ToLower() }).ToList();
            ViewBag.GammeVerre = new SelectList(GammeVerres, "Id", "Libelle");
            var listeSide = new List<QtypeValue> { new QtypeValue { Id = 0, Value = "OD" }, new QtypeValue { Id = 1, Value = "OG" } };
            ViewBag.Side = new SelectList(listeSide, "Id", "Value");
            ViewBag.Dverre = new Dverre { GammeVerre = gammeVerre, SideVerre = sideVerre, TypeVerre = typeVerre };
            ViewBag.State = false;
            return View("Add", ficheverreModel);
        }

        [Route("verres/update-stock/{id}/", Name="_UpdateStockVerrePost")]
        [HttpPost]
        public ActionResult UpdatePostStock(int id, FicheVerreModel fvModel)
        {
            var mverre = ctx.MVerres.Find(id);
            try
            {
                mverre.Qte += fvModel.Qte;
                ctx.SaveChanges();      
            } catch(DataException ex){
            
            }
            return RedirectToRoute("_listeVerre");
        }


        [Route("verres/liste-variete/", Name="_getVarieteVerre")]
        public ActionResult GetVarieteVerre(int tv , int? gv)
        {
            TypeVerre _tv = (TypeVerre)tv;
            var listv = from mv in ctx.MVerres    
                        where mv.GammeVerreId == gv && mv.TypeVerre == _tv
                        select new PuissanceModelVerre { Add = mv.Add, Cycl = mv.Cyl, Sph = mv.Sph };
            return Json(listv.ToList(), JsonRequestBehavior.AllowGet);
        }
        


        [Route("verres/supprimer-verre/{id}/", Name="_deleteMVerre")]
        public ActionResult DelPuissance(int id)
        {
            Reponse resp = new Reponse { Statut = 500 };
            var _mverre = ctx.MVerres.Find(id);
            if (_mverre != null)
            {
                ctx.MVerres.Remove(_mverre);
                ctx.SaveChanges();
                resp.Statut = 200;
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        private MVerre VerrExiste(FicheVerreModel fvmodel, bool statut = false)
        {
            MVerre q = new MVerre();
            if (fvmodel != null && statut)
            {
                 q = ctx.MVerres.Where(x=> x.Side == fvmodel.Side && x.Sph == fvmodel.Sph && x.Cyl == fvmodel.Cycl && x.Add == fvmodel.Addition && x.GammeVerreId == fvmodel.GammeVerreId && x.TypeVerre == fvmodel.TypeVerre).FirstOrDefault();
            }
            else
            {
                if (fvmodel != null)
                {
                     q = ctx.MVerres.Where(x => x.Side == fvmodel.Side && x.Sph == fvmodel.Sph && x.Cyl == fvmodel.Cycl && x.Add == fvmodel.Addition && x.GammeVerreId == fvmodel.GammeVerreId && x.TypeVerre == fvmodel.TypeVerre && x.Id != fvmodel.Id).FirstOrDefault();
                }
            }
            return q;
        }

        public struct Dverre
        {
            public string TypeVerre { get; set; }
            public string GammeVerre { get; set; }
            public string SideVerre { get; set; }
        }
	}
}