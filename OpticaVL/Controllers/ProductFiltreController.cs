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
    public class ProductFiltreController : BaseController
    {
        //
        // GET: /ProductFiltre/
        public ActionResult Index()
        {
            return View();
        }


        [Route("liste-product/get-cat-product/{categorie}", Name="_getListProductByCat")]
        public JsonResult ListeProducts(string categorie)
        {
            List<HProduct> ListeProducts = new List<HProduct>();
            //List<CategorieProduit> catProds = new List<CategorieProduit>();
            List<Produit> lProduit = new List<Produit>();
            switch (categorie) 
            {
                case "divers":
                    lProduit = ctx.Produits.Where(x => x.Categorie.Libelle.ToUpper() == "DIVERS" && x.Del == false).ToList();
                    break;
                case "monture":
                    lProduit = ctx.Produits.Where(x => x.Del == false && x.Categorie.Libelle.ToUpper() == "MONTURE").ToList();
                    break;
                case "verre":
                    lProduit = ctx.Produits.Where(x => x.Del == false && x.Categorie.Libelle.ToUpper() == "VERRE").ToList();
                    break;
            }
            if (lProduit.Count > 0)
            {
                foreach (var p in lProduit)
                {
                    var produitModel = new ProduitModel { Designation = p.Designation, Categorie = p.Categorie.Libelle, RefProd = p.RefProd };
                    var _designation = Title(FormatDesignation(produitModel));
                    _designation = _designation != null ? _designation.Trim() : _designation;
                    var _marque = p.Marque != null ? p.Marque.Libelle : "Sans marque";
                    ListeProducts.Add(new HProduct { Designation = _designation, Id = p.Id, Marque = _marque, Prix = p.Prix, QteStock = p.QteStock, Categorie = p.Categorie.Libelle});
                }
            }
            return Json(new CategorieProduit { LibCat = categorie, Produits = ListeProducts }, JsonRequestBehavior.AllowGet);
        }

        [Route("liste-product/", Name="_listeProducts")]
        public JsonResult GetListeProducts()
        {
                List<CatProduit> ListeCatProd = new List<CatProduit>();
                var _catproduits = (from prod in ctx.Produits
                                    where prod.Del == false && (prod.Categorie.Libelle == "DIVERS" || prod.Categorie.Libelle == "MONTURE")
                                    group prod by prod.Categorie.Libelle into prodgroup
                                    select prodgroup).ToList();

                foreach (var lprod in _catproduits)
                {
                    //keys
                    var ListeProd = new List<ProduitModel>();
                   /* foreach (var prod in lprod)
                    {
                        var pm = new ProduitModel { Id = prod.Id, RefProd = ""/*prod.RefPro, Designation = prod.Designation, Prix = prod.Prix, QteStock = prod.QteStock, Marque = prod.Marque == null ? "Sans marque" : prod.Marque.Libelle, Divers = prod.ModelDivers, Monture = prod.ModelMonture, Verre = prod.ModelVerre };
                        ListeProd.Add(pm);
                    }*/
                    var _tempgroup = ListeProd.GroupBy(x => x.Marque);
                    var listMprod = new List<MProd>();
                    foreach (var temp in _tempgroup)
                    {
                        //key 
                        var listProd = new List<MarqueProd>();
                        foreach (var p in temp)
                        {
                            var mp = new MarqueProd
                            {
                                Designation = FormatDesignation(p),
                                Id = p.Id,
                                Prix = p.Prix,
                                QteStock = p.QteStock,
                                RefProd = p.RefProd
                            };
                            listProd.Add(mp);
                        }
                        listMprod.Add(new MProd { LibMarque = temp.Key, MarqueProds = listProd });
                    }
                    ListeCatProd.Add(new CatProduit { Categorie = lprod.Key, Produits = listMprod });            
                }
                return Json(ListeCatProd, JsonRequestBehavior.AllowGet);
        }
	}
}