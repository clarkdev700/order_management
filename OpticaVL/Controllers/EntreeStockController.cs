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
    public class EntreeStockController : BaseController
    {
        [Route("stocks/historique-mouvements/{id}/", Name="_listeStock")] // id du produit
        public ActionResult Index(int id)
        {
           /* var stocks = ctx.EntreeStocks.Where(x => x.Del == false && x.ProduitId == id).ToList();
            List<ProductStock> HpStocks = new List<ProductStock>();
            if (stocks.Count > 0)
            {
                foreach (var s in stocks)
                {
                    var prod = s.Produit;
                    var prodModel = new ProduitModel { Designation = prod.Designation, Categorie = prod.Categorie.Libelle, RefProd = prod.RefProd };
                    var _designation = FormatDesignation(prodModel);
                    var _marque = prod.Marque != null ? prod.Marque.Libelle : null;
                    HpStocks.Add(new ProductStock { Id = s.Id, Qte = s.Qte, Marque = _marque, Date = s.Date.ToString("dd/MM/yyyy"), Fournisseur = s.Fournisseur.Code, Designation = _designation });
                }
            }*/
            ViewBag.prodId = id;
            return View("Index2");
        }

        [Route("stocks/historique-entree/{id}", Name="_GetListeEntree")]
        public JsonResult GetListeEntree(int id)
        {
            var stocks = ctx.EntreeStocks.Where(x => x.Del == false && x.ProduitId == id).OrderByDescending(x=>x.Date).ToList();
            List<ProductStock> HpStocks = new List<ProductStock>();
            if (stocks.Count > 0)
            {
                foreach (var s in stocks)
                {
                    var prod = s.Produit;
                    var prodModel = new ProduitModel { Designation = prod.Designation, Categorie = prod.Categorie.Libelle, RefProd = prod.RefProd };
                    var _designation = FormatDesignation(prodModel);
                    var _marque = prod.Marque != null ? prod.Marque.Libelle : null;
                    HpStocks.Add(new ProductStock { Id = s.Id, Qte = s.Qte, Marque = _marque, Date = s.Date.ToString("dd/MM/yyyy"), Fournisseur = s.Fournisseur.Code, Designation = _designation });
                }
            }
            return Json(HpStocks, JsonRequestBehavior.AllowGet);
        }

        [Route("stocks/add/", Name="_addStock")]
        public ActionResult Add()
        {
            ViewBag.Fournisseur = new SelectList(ctx.Fournisseurs.Where(x => x.Del == false).ToList(), "Id", "Nom");
            ViewBag.Produit = new SelectList(ctx.Produits.Where(x => x.Del == false).ToList(), "Id", "Designation");
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle","Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
            //ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance}).ToList(), "Puissance", "Puissance");
           // ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            return View("Stock", new EntreeStock { Date = DateTime.Now, Produit = new Produit() });
        }

        [Route("stocks/add/", Name="_addPostStock")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost([Bind(Include="Qte, Date, FournisseurId, ProduitId")] EntreeStock stock)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (stock.Qte > 0)
                    {
                        stock.DateEnreg = DateTime.Now;
                        ctx.EntreeStocks.Add(stock);
                        ctx.SaveChanges();
                        var prod = ctx.Produits.Find(stock.ProduitId);
                        try
                        {
                            prod.QteStock += stock.Qte;
                            ctx.SaveChanges();
                        }
                        catch (DataException /* ex*/)
                        {
                            ModelState.AddModelError("", "Impossible  d'effectuer une mise à jour du stock du produit Id : " + stock.ProduitId);
                        }
                    }
                    return RedirectToRoute("_listeStock", new { id = stock.ProduitId});
                }
            }
            catch (DataException /* ex */)
            {
                ModelState.AddModelError("","Impossible d'enregistrer une nouvelle entree de stock.");
            }
            
            ViewBag.Fournisseur = new SelectList(ctx.Fournisseurs.Where(x => x.Del == false).ToList(), "Id", "Nom");
            ViewBag.Produit = new SelectList(ctx.Produits.Where(x => x.Del == false).ToList(), "Id", "Designation");
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
            //ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
           // ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            stock.Produit = new Produit();
            return View("Stock", stock);
        }

        [Route("stocks/{id}/", Name="_updateStock")]
        public ActionResult Update(int id)
        {
            var stock = ctx.EntreeStocks.Find(id);
            if (stock == null)
                return HttpNotFound();
            var prod = stock.Produit;
            /*var prodModel = new ProduitModel { Designation = prod.Designation, Divers = prod.ModelDivers, Monture = prod.ModelMonture, Verre = prod.ModelVerre };*/
            ViewBag.Fournisseur = new SelectList(ctx.Fournisseurs.Where(x => x.Del == false).ToList(), "Id", "Nom", stock.FournisseurId);
            ViewBag.Produit = new SelectList(ctx.Produits.Where(x => x.Del == false).ToList(), "Id", "Designation", stock.ProduitId);
            var _marque = prod.Marque != null ? prod.Marque.Libelle:null;
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle", "Libelle", _marque);
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
            //ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
           // ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            return View("Stock", stock);
        }

        [Route("stocks/{id}/", Name="_updatePostStock")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id, EntreeStock stock) //A REVOIR
        {
            var stockToUp = ctx.EntreeStocks.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    var _oldStock = stockToUp.Qte;
                    //stockToUp.ProduitId = stock.ProduitId;
                    stockToUp.FournisseurId = stock.FournisseurId;
                    stockToUp.Date = stock.Date;
                    stockToUp.Qte = stock.Qte;
                    //update produit entity
                    var prod = stockToUp.Produit;
                    if (prod != null)
                    {
                        prod.QteStock += (stock.Qte - _oldStock);
                    }
                    ctx.SaveChanges();
                    return RedirectToRoute("_listeStock", new { id = stockToUp.ProduitId });

                }
            }
            catch (DataException /* ex */)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour le stock.");
            }
            ViewBag.Fournisseur = new SelectList(ctx.Fournisseurs.Where(x => x.Del == false).ToList(), "Id", "Nom", stock.FournisseurId);
            ViewBag.Produit = new SelectList(ctx.Produits.Where(x => x.Del == false).ToList(), "Id", "Designation", stock.ProduitId);
            ViewBag.Marque = new SelectList(ctx.Marques.ToList(), "Libelle", "Libelle");
            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Libelle", "Libelle");
           // ViewBag.Puissance = new SelectList(ctx.ModelVerres.Select(x => new { Puissance = x.Puissance }).ToList(), "Puissance", "Puissance");
            //ViewBag.Angle = new SelectList(ctx.ModelVerres.Select(x => new { Angle = x.Angle }).ToList(), "Angle", "Angle");
            return View("Stock", stock);
        }

        [Route("stocks/inventaire/", Name="_inventaireStock")]
        public ActionResult InventaireStock()
        {
            return View("InventaireStock2");
        }

        [Route("stocks/get-liste-inventaires/", Name="_getInventaire")]
        public JsonResult GetInventaire()
        {
            List<InventaireViewModel> InventaireModel = new List<InventaireViewModel>();
            var listeprod = new List<HProduct>();
            var groupProd = (from c in ctx.Categories
                             join p in ctx.Produits on c.Id equals p.CategorieId
                             where p.Del == false /*p.Categorie.Libelle.ToUpper() == "MONTURE" || p.Categorie.Libelle.ToUpper() == "DIVERS"*/
                             group p by p.Categorie.Libelle).ToList();
            if (groupProd.Count > 0)
            {
                foreach (var catprod in groupProd)
                {
                    var _cat = catprod.Key;
                    var _produits = new List<Prod>();
                    
                    foreach (var p in catprod)
                    {
                        var _libmarque = p.Marque != null ? p.Marque.Libelle : "Sans marque";
                        var pm = new ProduitModel { Designation = p.Designation, Categorie = p.Categorie.Libelle, RefProd = p.RefProd };
                        var _designation = FormatDesignation(pm);
                        //_produits.Add(new Prod { Designation = Title(_designation), Numero = p.Id, Prix = p.Prix, QteStock = p.QteStock, Marque = _libmarque.ToUpper() });
                        listeprod.Add(new HProduct { Categorie = _cat.ToUpper(), Marque = _libmarque.ToUpper(), Designation = Title(_designation), Id = p.Id, Prix = p.Prix, QteStock = p.QteStock });
                    }
                    //InventaireModel.Add(new InventaireViewModel { Categorie = _cat, Produits = _produits.OrderBy(x=>x.Designation).ToList() });
                }
            }
            return Json(listeprod.OrderBy(x=>x.Designation).ToList(), JsonRequestBehavior.AllowGet);
        }

        [Route("stocks/annuler/{id}/", Name="_delEntreeStock")]
        public JsonResult AnnulerStock(int id)
        {
            Reponse _rep = new Reponse { Statut = 500, Message = "Impossible d'annuler le mouvement d'entree en stock" };
            var entreeStock = ctx.EntreeStocks.Find(id);
            if (entreeStock == null)
                return Json(_rep, JsonRequestBehavior.AllowGet);
            entreeStock.Del = true;
            entreeStock.DateDel = DateTime.Now;
            entreeStock.MotifDel = User.Identity.Name;
            var prod = entreeStock.Produit;
            prod.QteStock -= entreeStock.Qte;
            ctx.SaveChanges();
            _rep.Statut = 200; _rep.Message = "";
            return Json(_rep,JsonRequestBehavior.AllowGet);
        }
	}
}