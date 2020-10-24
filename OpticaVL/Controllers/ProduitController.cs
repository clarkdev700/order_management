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
    public class ProduitController : BaseController
    {
       [Route("produits/", Name="_listeProduits")]
        public ActionResult Index()
        {
            return View("Index2");
        }

       [Route("produits/liste-produits/", Name="_getAllproducts")]
       public JsonResult GetAllProducts()
       {
           var produits = ctx.Produits.Where(x => x.Del == false).ToList();
           List<HProduct> prodVM = new List<HProduct>();
           var proByCat = produits.GroupBy(x => x.Categorie).ToList();
           if (proByCat.Count > 0)
           {
               foreach (var item in proByCat)
               {
                   foreach (var p in item)
                   {
                       var prodModel = new ProduitModel { Designation = p.Designation, Categorie = p.Categorie.Libelle, RefProd = p.RefProd };
                       var _designation = FormatDesignation(prodModel);
                       var _marque = p.Marque != null ? p.Marque.Libelle : null;
                       prodVM.Add(new HProduct { Id = p.Id, Designation = _designation, QteStock = p.QteStock, Marque = _marque, Prix = p.Prix, Categorie = p.Categorie.Libelle });
                   }
               }
           }
           var _tempOrder = prodVM.OrderBy(x => x.Designation).GroupBy(x => x.Categorie).ToList();
           prodVM = new List<HProduct>();
           foreach (var elt in _tempOrder)
           {
               foreach (var item in elt)
               {
                   var _itemmarque = item.Marque != null ? item.Marque.ToUpper() : "";
                   prodVM.Add(new HProduct { Designation = Title(item.Designation), Categorie = item.Categorie.ToUpper(), Id = item.Id, Marque = _itemmarque, Prix = item.Prix, QteStock = item.QteStock });
               }
           }
           //return View(prodVM);
           return Json(prodVM, JsonRequestBehavior.AllowGet);
       }

        [Route("produits/all/", Name="_allproducts")]
       public ActionResult Allproducts()
       {
           var  Products = ctx.Produits.Where(x => x.Del == false)/*.Select(x => new ProduitModel { Id = x.Id, Divers = x.ModelDivers, Verre = x.ModelVerre, Monture = x.ModelMonture, QteStock = x.QteStock, Prix = x.Prix, Designation = FormatDesignation(x)})*/.ToList();
           List<ProduitModel> listp = new List<ProduitModel>();
           /*foreach (var p in Products)
           {
               ProduitModel pmodel = new ProduitModel { Designation = p.Designation, Divers = p.ModelDivers, Verre = p.ModelVerre, RefProd ="", Marque = p.Marque != null ? p.Marque.Libelle:"Sans marque", Id = p.Id, QteStock = p.QteStock, Monture = p.ModelMonture, Prix = p.Prix };
               pmodel.Designation = FormatDesignation(pmodel);
               listp.Add(pmodel);
           }*/
           return Json(listp, JsonRequestBehavior.AllowGet);
           //return RedirectToRoute("_listeProduitCat");
       }

        [Route("produits/add/", Name="_addProduit")]
       public ActionResult Add()
       {
           ViewBag.Marque = new SelectList(ctx.Marques.OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList(), "Id", "Libelle");
           ViewBag.Categorie = new SelectList(ctx.Categories.Where(x => x.Del == false).OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle");
           ViewBag.MarqueListe = ctx.Marques.ToList();
           List<QtypeValue> Formats = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Cylindrique" }, 
               new QtypeValue { Id = 1, Value = "Plan" }, 
               new QtypeValue { Id = 2, Value = "Spherique" } };

           ViewBag.FormatVerre = new SelectList(Formats, "Id","Value");

           List<QtypeValue> Natures = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Organique" }, 
               new QtypeValue { Id = 1, Value = "Mineraux" } };

           ViewBag.NatureVerre = new SelectList(Natures, "Id", "Value");

           List<QtypeValue> Type = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Unifocaux" }, 
               new QtypeValue { Id = 1, Value = "Progressif" } };

           ViewBag.TypeVerre = new SelectList(Type, "Id", "Value");

           ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Id","Libelle");
           return View("Produit2", new Produit());
       }

        [Route("produits/add/", Name="_addPostProduit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost([Bind(Include = "RefProd, Designation, Prix, QteSeuil, QteStock, MarqueId, CategorieId")] Produit produit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //ModelVerre Verre;
                    //ModelMonture Monture;
                    //ModelDivers Divers;

                    //var produit = new Produit() { Designation = pmodel.Designation, Prix = pmodel.Prix, QteSeuil = pmodel.QteSeuil, QteStock = pmodel.QteStock, MarqueId = pmodel.MarqueId, CategorieId = pmodel.CategorieId };

                    /*switch (pmodel.CategorieId)
                    {
                        case 1:
                            Monture = new ModelMonture { Reference = pmodel.ModelMonture.Reference };
                            ctx.ModelMontures.Add(Monture);
                            ctx.SaveChanges();
                            produit.ModelMontureId = Monture.Id;
                            break;
                        case 2:
                        case 3:
                            //VERRES
                            var _verre= pmodel.ModelVerre;
                            Verre = new ModelVerre { Angle = _verre.Angle, Puissance = _verre.Puissance, Format = _verre.Format, Nature = _verre.Nature, Type = _verre.Type, GammeVerreId = pmodel.ModelVerre.GammeVerreId };
                            ctx.ModelVerres.Add(Verre);
                            ctx.SaveChanges();
                            produit.ModelVerreId = Verre.Id;
                            break;
                        case 4:
                            //Divers
                            Divers = new ModelDivers { Numero = pmodel.ModelDivers.Numero, Reference = pmodel.ModelDivers.Reference };
                            ctx.ModelDivers.Add(Divers);
                            ctx.SaveChanges();
                            produit.ModelDiversId = Divers.Id;
                            break;
                    }*/

                    var design = produit.Designation;
                    if (design != null) design = design.Trim();
                    var refprod = produit.RefProd;
                    if (refprod != null) refprod = refprod.Trim().ToUpper();
                    produit.RefProd = refprod;
                    produit.Designation = design;
                    var produitSample = ctx.Produits.Where(x => x.CategorieId == produit.CategorieId && x.RefProd.ToLower() == refprod.ToLower() && x.Designation.ToLower() == design.ToLower() && x.MarqueId == produit.MarqueId && x.Del == false).FirstOrDefault();
                    if (produitSample != null)
                    {
                        produitSample.QteStock += produit.QteStock;
                    }
                    else
                    {
                        ctx.Produits.Add(produit);
                    }
                    ctx.SaveChanges();
                    return RedirectToRoute("_listeProduits");
                }
            }
            catch (DataException /* ex */)
            {
                ModelState.AddModelError("", "Impossible d'enregistrer un nouveau produit");
            }
            ViewBag.Marque = new SelectList(ctx.Marques.OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Libelle = x.Libelle.ToUpper(), Id = x.Id }).ToList(), "Id", "Libelle");
            ViewBag.Categorie = new SelectList(ctx.Categories.Where(x => x.Del == false).OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle");
            /*List<QtypeValue> Formats = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Cylindrique" }, 
               new QtypeValue { Id = 1, Value = "Plan" }, 
               new QtypeValue { Id = 2, Value = "Spherique" } };

            ViewBag.FormatVerre = new SelectList(Formats, "Id", "Value");

            List<QtypeValue> Natures = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Organique" }, 
               new QtypeValue { Id = 1, Value = "Mineraux" } };

            ViewBag.NatureVerre = new SelectList(Natures, "Id", "Value");

            List<QtypeValue> Type = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Unifocaux" }, 
               new QtypeValue { Id = 1, Value = "Progressif" } };

            ViewBag.TypeVerre = new SelectList(Type, "Id", "Value");

            ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Id", "Libelle");*/
            return View("Produit2", produit);
        }

        [Route("produits/{id}/", Name="_updateProduit")]
        public ActionResult Update(int id)
        {
            var produit = ctx.Produits.Find(id);
            if (produit == null)
                return HttpNotFound();
            ViewBag.Marque = new SelectList(ctx.Marques.OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList(), "Id", "Libelle", produit.MarqueId);
            ViewBag.Categorie = new SelectList(ctx.Categories.Where(x => x.Del == false).OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle", produit.CategorieId);

           /* var produitModel = new ProduitViewModel { CategorieId = produit.CategorieId, Designation = produit.Designation, MarqueId = produit.MarqueId, Id = produit.Id, Prix = produit.Prix, QteSeuil = produit.QteSeuil, QteStock = produit.QteStock, ModelDivers = produit.ModelDivers ?? new ModelDivers(), ModelMonture = produit.ModelMonture ?? new ModelMonture(), ModelVerre = produit.ModelVerre ?? new ModelVerre() };*/

            List<QtypeValue> Formats = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Cylindrique" }, 
               new QtypeValue { Id = 1, Value = "Plan" }, 
               new QtypeValue { Id = 2, Value = "Spherique" } };

            //ViewBag.FormatVerre = new SelectList(Formats, "Id", "Value", produitModel.ModelVerre.Format);

            List<QtypeValue> Natures = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Organique" }, 
               new QtypeValue { Id = 1, Value = "Mineraux" } };

            //ViewBag.NatureVerre = new SelectList(Natures, "Id", "Value", produitModel.ModelVerre.Nature);

            List<QtypeValue> Type = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Unifocaux" }, 
               new QtypeValue { Id = 1, Value = "Progressif" } };

           // ViewBag.TypeVerre = new SelectList(Type, "Id", "Value", produitModel.ModelVerre.Type);

            //var gammeVerreId = (produitModel.ModelVerre != null && produitModel.ModelVerre.GammeVerre != null) ? produitModel.ModelVerre.GammeVerreId : 0;
            //ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Id", "Libelle", gammeVerreId);
            return View("Produit2", produit);
        }

        [Route("produits/{id}/", Name="_updatePostProduit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int id, Produit produitModel)
        {
            var prodToUpdate = ctx.Produits.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    var oldCatId = prodToUpdate.CategorieId;
                    prodToUpdate.MarqueId = produitModel.MarqueId;
                    prodToUpdate.Designation = produitModel.Designation;
                    prodToUpdate.Prix = produitModel.Prix;
                    prodToUpdate.QteSeuil = produitModel.QteSeuil;
                    prodToUpdate.QteStock = produitModel.QteStock;
                    prodToUpdate.CategorieId = produitModel.CategorieId;
                    prodToUpdate.RefProd = produitModel.RefProd;
                    /*switch(produitModel.CategorieId) 
                    {
                        case 1:
                            //Monture
                            if (oldCatId == produitModel.CategorieId)
                            {
                                if (prodToUpdate.ModelMonture != null)
                                {
                                    prodToUpdate.ModelMonture.Reference = produitModel.ModelMonture.Reference;
                                    ctx.SaveChanges();
                                }
                            }
                            else
                            {
                                //del oldModel 
                                if (prodToUpdate.ModelDivers != null)
                                {
                                    try
                                    {
                                        prodToUpdate.ModelDivers.Del = true;
                                        prodToUpdate.ModelDiversId = null;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /*ex)
                                    {
                                        //definir l'erreur
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prodToUpdate.ModelVerre.Del = true;
                                        prodToUpdate.ModelVerreId = null;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /*ex)
                                    {
                                        //definir l'erreur
                                    }
                                }
                                
                                //nouveau monture
                                Monture = new ModelMonture { Reference = produitModel.ModelMonture.Reference };
                                ctx.ModelMontures.Add(Monture);
                                ctx.SaveChanges();
                                prodToUpdate.ModelMontureId = Monture.Id;
                                ctx.SaveChanges();
                            }
                            break;
                        case 2:
                            //Verre
                        case 3:
                            if (oldCatId == produitModel.CategorieId)
                            {
                                try
                                {
                                    prodToUpdate.ModelVerre.Nature = produitModel.ModelVerre.Nature;
                                    prodToUpdate.ModelVerre.Type = produitModel.ModelVerre.Type;
                                    prodToUpdate.ModelVerre.Format = produitModel.ModelVerre.Format;
                                    prodToUpdate.ModelVerre.Puissance = produitModel.ModelVerre.Puissance;
                                    prodToUpdate.ModelVerre.Angle = produitModel.ModelVerre.Angle;
                                    prodToUpdate.ModelVerre.GammeVerreId = produitModel.ModelVerre.GammeVerreId;
                                    ctx.SaveChanges();
                                }
                                catch (DataException /*ex)
                                {
                                    //erreur
                                }
                            }
                            else
                            {
                                //del old Model
                                if (prodToUpdate.ModelDivers != null)
                                {
                                    try
                                    {
                                        prodToUpdate.ModelDivers.Del = true;
                                        prodToUpdate.ModelDiversId = null;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /* ex)
                                    {
                                        //error
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prodToUpdate.ModelMonture.Del = true;
                                        prodToUpdate.ModelMontureId = null;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /*ex)
                                    {
                                        //error
                                    }
                                }
                                //new Model Verre
                                Verre = new ModelVerre { Angle = produitModel.ModelVerre.Angle, Format = produitModel.ModelVerre.Format, Nature = produitModel.ModelVerre.Nature, Puissance = produitModel.ModelVerre.Puissance, Type = produitModel.ModelVerre.Type, GammeVerreId = produitModel.ModelVerre.GammeVerreId };
                                ctx.ModelVerres.Add(Verre);
                                ctx.SaveChanges();
                                prodToUpdate.ModelVerreId = Verre.Id;
                                ctx.SaveChanges();
                            }
                            break;
                        case 4:
                            //Divers
                            if (oldCatId == produitModel.CategorieId)
                            {
                                try
                                {
                                    prodToUpdate.ModelDivers.Numero = produitModel.ModelDivers.Numero;
                                    prodToUpdate.ModelDivers.Reference = produitModel.ModelDivers.Reference;
                                    ctx.SaveChanges();
                                }
                                catch (DataException /* ex)
                                {
                                    //error
                                }
                            }
                            else
                            {
                                //del old model
                                if (prodToUpdate.ModelMonture != null)
                                {
                                    try
                                    {
                                        prodToUpdate.ModelMonture.Del = true;
                                        prodToUpdate.ModelMontureId = null;
                                        ctx.SaveChanges();
                                    } 
                                    catch(DataException /* ex )
                                    {
                                        //error
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        prodToUpdate.ModelVerre.Del = true;
                                        prodToUpdate.ModelVerreId = null;
                                        ctx.SaveChanges();
                                    }
                                    catch (DataException /* ex)
                                    {
                                        //error
                                    }
                                }
                                //
                                Divers = new ModelDivers { Numero = produitModel.ModelDivers.Numero, Reference = produitModel.ModelDivers.Reference };
                                ctx.ModelDivers.Add(Divers);
                                ctx.SaveChanges();
                                prodToUpdate.ModelDiversId = Divers.Id;
                                ctx.SaveChanges();
                            }
                            break;
                    }*/
                    
                   /* prodToUpdate.CategorieId = produit.CategorieId;
                    prodToUpdate.Designation = produit.Designation;
                    prodToUpdate.MarqueId = produit.MarqueId;
                    prodToUpdate.Prix = produit.Prix;*/
                    //prodToUpdate.Numero = produit.Numero;
                    //prodToUpdate.QteSeuil = produit.QteSeuil;
                    //prodToUpdate.RefProd = produit.RefProd.Trim().ToUpper();
                    ctx.SaveChanges();
                    return RedirectToRoute("_listeProduits");
                }
            }
            catch (DataException /* ex */)
            {
                ModelState.AddModelError("", "Impossible de mettre à jour le produit.");
            }
            ViewBag.Marque = new SelectList(ctx.Marques.OrderBy(x => x.Libelle).Select(x => new MarqueViewModel { Id = x.Id, Libelle = x.Libelle.ToUpper() }).ToList(), "Id", "Libelle", produitModel.MarqueId);
            ViewBag.Categorie = new SelectList(ctx.Categories.Where(x => x.Del == false).OrderBy(x=>x.Libelle).ToList(), "Id", "Libelle", produitModel.CategorieId);

            List<QtypeValue> Formats = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Cylindrique" }, 
               new QtypeValue { Id = 1, Value = "Plan" }, 
               new QtypeValue { Id = 2, Value = "Spherique" } };

            //ViewBag.FormatVerre = new SelectList(Formats, "Id", "Value", produitModel.ModelVerre.Format);

            List<QtypeValue> Natures = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Organique" }, 
               new QtypeValue { Id = 1, Value = "Mineraux" } };

            //ViewBag.NatureVerre = new SelectList(Natures, "Id", "Value", produitModel.ModelVerre.Nature);

            List<QtypeValue> Type = new List<QtypeValue> { 
               new QtypeValue { Id = 0, Value = "Unifocaux" }, 
               new QtypeValue { Id = 1, Value = "Progressif" } };

            //ViewBag.TypeVerre = new SelectList(Type, "Id", "Value", produitModel.ModelVerre.Type);

            //ViewBag.GammeVerre = new SelectList(ctx.GammeVerres.ToList(), "Id", "Libelle", produitModel.ModelVerre.GammeVerreId);
            return View("Produit2", produitModel);
        }

        [Route("produits/index-produits/")]
        public ActionResult indexProdCat() {
            List<CatProduit> ListeCatProd = new List<CatProduit>();
            var _catproduits = (from prod in ctx.Produits
                               where prod.Del == false /*&& prod.Categorie.Libelle.ToUpper() == "DIVERS" || prod.Categorie.Libelle.ToUpper() == "MONTURE"*/
                               group prod by prod.Categorie.Libelle into prodgroup
                               select prodgroup).ToList();

            foreach (var lprod in _catproduits)
            {
                //keys
                var ListeProd = new List<ProduitModel>();
                foreach (var prod in lprod)
                {
                    //produits
                    var pm = new ProduitModel { Id = prod.Id, RefProd = prod.RefProd, Designation = prod.Designation, Prix = prod.Prix, QteStock = prod.QteStock, Marque = prod.Marque == null ? "Sans marque" : prod.Marque.Libelle, Categorie = prod.Categorie.Libelle};
                    ListeProd.Add(pm);
                }
                var _tempgroup = ListeProd.OrderBy(x=>x.Marque).GroupBy(x => x.Marque).ToList();
                var listMprod = new List<MProd>();
                foreach (var temp in _tempgroup)
                {
                    //key 
                    var listProd = new List<MarqueProd>();
                    foreach (var p in temp)
                    {
                        var _designation = Title(FormatDesignation(p));
                        _designation = _designation != null ? _designation.Trim() : _designation;
                        var mp = new MarqueProd
                        {
                            Designation = _designation,
                            Id = p.Id,
                            Prix = p.Prix,
                            QteStock = p.QteStock,
                            RefProd = p.RefProd
                        };
                        listProd.Add(mp);
                    }
                    listMprod.Add(new MProd { LibMarque = temp.Key.ToUpper(), MarqueProds = listProd.OrderBy(x=>x.Designation).ToList() });
                    //listProd.Clear();
                }

                ListeCatProd.Add(new CatProduit { Categorie = lprod.Key.ToUpper(), Produits = listMprod });
                //ListeProd.Clear();               
            }

            return PartialView("CatProdTreeView", ListeCatProd);
        }

        [Route("produits-categorie/")]
        public ActionResult Cat()
        {
            return View("CatProdPartial");
        }
        [Route("produits/liste", Name="_listeProduitCat")]
        public JsonResult ProduitsCat()
        {
            List<CatProduit> ListeCatProd = new List<CatProduit>();
            var _catproduits = (from prod in ctx.Produits
                           where prod.Del == false /*&& prod.Categorie.Libelle.ToUpper() == "MONTURE" || prod.Categorie.Libelle.ToUpper() == "DIVERS"*/
                           group prod by prod.Categorie.Libelle into prodgroup
                           select prodgroup).ToList();

            foreach (var lprod in _catproduits)
            {
                //keys
                var ListeProd = new List<ProduitModel>();
                foreach (var prod in lprod)
                {
                    //produits
                    var  _marqueId = prod.MarqueId;
                    string _marquelib;
                    if (_marqueId != null)
                    {
                        Marque _marque = ctx.Marques.Where(x=>x.Id == _marqueId).FirstOrDefault();
                        if (_marque != null) _marquelib = _marque.Libelle;
                    }
                    var pm = new ProduitModel { Id = prod.Id, RefProd = prod.RefProd, Designation = prod.Designation, Prix = prod.Prix, QteStock = prod.QteStock, Marque = prod.Marque == null ? "Sans marque" : prod.Marque.Libelle, Categorie = prod.Categorie.Libelle };
                    ListeProd.Add(pm);
                }
                var _tempgroup = ListeProd.OrderBy(x=>x.Marque).GroupBy(x => x.Marque).ToList();
                var listMprod = new List<MProd>();
                foreach (var temp in _tempgroup) 
                {
                    //key 
                    var listProd = new List<MarqueProd>();
                    foreach (var p in temp) 
                    {
                       var _designation = Title(FormatDesignation(p));
                        _designation = _designation != null ? _designation.Trim():_designation;
                        var mp = new MarqueProd
                        {
                            Designation = _designation,
                            Id = p.Id,
                            Prix = p.Prix,
                            QteStock = p.QteStock,
                            RefProd = p.RefProd
                        };
                        listProd.Add(mp);
                    }
                    listMprod.Add(new MProd{ LibMarque = temp.Key.ToUpper(), MarqueProds = listProd.OrderBy(x=>x.Designation).ToList()});
                    //listProd.Clear();
                }

                ListeCatProd.Add(new CatProduit { Categorie = lprod.Key.ToUpper(), Produits = listMprod });
                //ListeProd.Clear();               
            }

            /*var gpr2 = from prod in ListeMarkModel
                       group prod by prod.ProduitModel.*/
                           /*select new ProduitModel { RefProd= prod.RefProd,  Designation = prod.Designation,  Prix = prod.Prix,  QteStock = prod.QteStock,  Marque = prod.Marque == null ? null :prod.Marque.Libelle, Categorie = prod.Categorie.Libelle };*/
             //= prod.Marque == null? null :prod.Marque.Libelle
            return Json(ListeCatProd, JsonRequestBehavior.AllowGet);
        }

        [Route("get-produit/{cat}/", Name="_GetCatProd")]
        public JsonResult GetCatProd(string cat)
        {
            List<catProd> Produits = new List<catProd>();
            
            if (cat == "monture")
            {
                var prods = ctx.Produits.Where(x => x.Del == false && x.Categorie.Libelle.ToUpper() == "MONTURE").ToList();
                if (prods.Count > 0)
                {
                    foreach (var p in prods)
                    {
                        var _marque = p.Marque != null ? p.Marque.Libelle : null;
                        var _refmarque = "";//p.ModelMonture.Reference; 
                        if (_marque != null) _refmarque += "("+ _marque + ")";
                         Produits.Add(new catProd { Prix = p.Prix , RefMarque = _refmarque });
                    }
                }
            }
            return Json(Produits, JsonRequestBehavior.AllowGet);
        }

        [Route("get-produit/marque/{id}", Name="_getProduitFromMarque")]
        public JsonResult GetProdFromMarque(int id)
        {
            var prod = ctx.Produits.Where(x => x.Del == false && x.MarqueId == id).OrderBy(x=>x.RefProd).Select(x => new { Prix = x.Prix, Ref = x.RefProd, Id= x.Id }).ToList();
            return Json(prod, JsonRequestBehavior.AllowGet);
        }
	}
    public struct catProd
    {
        public string RefMarque { get; set; }
        public float Prix { get; set; }
    }
}