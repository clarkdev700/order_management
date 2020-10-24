using OpticaVL.Models;
using OpticaVL.ViewModel;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpticaVL.Controllers
{
    public class BaseController : Controller
    {
        public AppContext ctx;

        public BaseController()
        {
            ctx = new AppContext();
        }

        public struct Reponse
        {
            public int Statut { get; set; }
            public string Message { get; set; }
        }

        public string GetNumeroRecu(Payement lastpayement)
        {
            var _chbase = lastpayement.DatePaye;
            var _chp = (_chbase.Day + _chbase.Month + int.Parse(_chbase.Year.ToString().Substring(2))).ToString() + (_chbase.Hour + _chbase.Minute).ToString();
            return _chp + lastpayement.Id;
        }

        public string Title(string prenom)
        {
            string[] ch = prenom != null ? prenom.Split(' '/*new char[]{ '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries*/) : new string[]{};
            string chfinal = "";
            int chLength = ch.Count();
            if (chLength > 0)
            {
                for (int i = 0; i < chLength; i++)
                {
                    if (!string.IsNullOrEmpty(ch[i]))
                    {
                        string x = ch[i].ToLower();
                        chfinal += " " + x[0].ToString().ToUpper() + x.Substring(1);  //string.Format(chfinal, ch[0].ToUpper());
                    }
                }
            }
            return chfinal;
        }

        public string FormatDesignation(ProduitModel pm)
        {
            string ch;
            ch = pm.Designation;
            if (pm.RefProd != null) ch += "(" + pm.RefProd + ")";

            /*if (pm.Categorie.ToUpper() == "MONTURE")
            {
                ch = pm.Designation;
                if (pm.RefProd != null) 
                    ch +="(" + pm.RefProd + ")";
            }
            else if (pm.Verre != null)
            {
                var _format = "";//(Enum.Format(typeof(FormatVerre),pm.Verre.Format,"g")).ToString().Substring(0,1);
                var _nature = (Enum.Format(typeof(NatureVerre),pm.Verre.Nature,"g")).ToString().Substring(0,1);
                var _type = (Enum.Format(typeof(TypeVerre), pm.Verre.Type,"g")).ToString().Substring(0,1);
                var _gamme = pm.Verre.GammeVerre.Libelle;
                ch ="" ;//_gamme + "("+_nature+_type+_format+"-["+ pm.Verre.Puissance+"]-"+ pm.Verre.Angle +"°)";
            }
            else if (pm.Categorie.ToUpper() == "DIVERS")
            {
                var _ref = pm.RefProd != null ? pm.RefProd : null;
                ch = pm.Designation + _ref;
            }
            else
            {
                ch = pm.Designation;
            }*/
            return ch;
        }

        public string _getDesignation(Produit prod)
        {
            /*var prodModel = new ProduitModel { Designation = prod.Designation, Divers = prod.ModelDivers, Monture = prod.ModelMonture, Verre = prod.ModelVerre };
            var _designation = FormatDesignation(prodModel);*/
            return null; //_designation;
        }

        
        /*public Puissance VerrePuissanceExiste(Puissance fvmodel)
        {
            Puissance result = ctx.Puissances.Where(x => x.Addition == fvmodel.Addition && x.Axe == fvmodel.Axe && x.Cycl == fvmodel.Cycl && x.Sph == fvmodel.Sph && x.TypeVerre == fvmodel.TypeVerre).FirstOrDefault();
            return result;
        }*/

       /* public GammeVerrePuissance VerrExiste(FicheVerreModel fvmodel, bool statut = false)
        {
            GammeVerrePuissance result = null;
            if (fvmodel != null && statut)
            {
                var q = from gvp in ctx.GammeVerrePuissances
                        join g in ctx.GammeVerres on gvp.GammeVerreId equals g.Id
                        join p in ctx.Puissances on gvp.PuissanceId equals p.Id
                        where p.Sph == fvmodel.Sph && p.Cycl == fvmodel.Cycl && p.Axe == fvmodel.Axe && p.Addition == fvmodel.Addition && p.TypeVerre == fvmodel.TypeVerre
                        select gvp;
                result = q.FirstOrDefault();
            }
            else
            {
                if (fvmodel != null)
                {
                    var q = from gvp in ctx.GammeVerrePuissances
                            join g in ctx.GammeVerres on gvp.GammeVerreId equals g.Id
                            join p in ctx.Puissances on gvp.PuissanceId equals p.Id
                            where p.Sph == fvmodel.Sph && p.Cycl == fvmodel.Cycl && p.Axe == fvmodel.Axe && p.Addition == fvmodel.Addition && p.TypeVerre == fvmodel.TypeVerre && gvp.PuissanceId != fvmodel.PuissanceId && gvp.GammeVerreId != fvmodel.GammeVerreId
                            select gvp;
                    result = q.FirstOrDefault();
                }
            }
            return result;
        }*/

        public string generateReportFile(string _url, string filename, string footer = null)
        {
            var url = _url;//Url.RouteUrl(_url);
            url = "localhost:80"/*Request.Url.Authority*/ + url;
            //new Converter
            HtmlToPdf Converter = new HtmlToPdf();
            //document parameters
            //var enteteUrl = Server.MapPath("~/Views/Shared/DocEntete.html");
            var footerUrl = Server.MapPath("~/Views/Shared/DocFooter.html");
            //var footerUrl = Server.MapPath("~/Datadir/FooterPartial.html");
            //Converter.Options.CssMediaType = 
            //header params
            /*Converter.Options.DisplayHeader = true;
            Converter.Header.DisplayOnFirstPage = true;
            Converter.Header.DisplayOnEvenPages = true;
            Converter.Header.DisplayOnOddPages = true;
            Converter.Header.Height = 200;
            PdfHtmlSection headerHtml = new PdfHtmlSection(enteteUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            Converter.Header.Add(headerHtml);*/

            //footer params
            if (footer != null)
            {
                Converter.Options.DisplayFooter = true;
                Converter.Footer.DisplayOnFirstPage = true;
                Converter.Footer.DisplayOnOddPages = true;
                Converter.Footer.DisplayOnEvenPages = true;
                Converter.Footer.Height = 30;
                PdfHtmlSection footerhtml = new PdfHtmlSection(footerUrl);
                footerhtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                Converter.Footer.Add(footerhtml);
                //add page number element to footer
                PdfTextSection text = new PdfTextSection(0, 4, "Page: {page_number} sur {total_pages} ", new System.Drawing.Font("Helvetica", 8));
                text.HorizontalAlign = PdfTextHorizontalAlign.Center;
                //PdfTextSection text2 = new PdfTextSection(0,4,"Lomé,"+DateTime.Now.ToString("dd/MM/yyyy"), new System.Drawing.Font("Helvetica",8));
                //text2.HorizontalAlign = PdfTextHorizontalAlign.Right;
                Converter.Footer.Add(text);
                //Converter.Footer.Add(text2);
            }

            //conversion delay
            Converter.Options.MinPageLoadTime = 2;
            Converter.Options.MaxPageLoadTime = 90;
            Converter.Options.PdfPageSize = PdfPageSize.A4;
            Converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
           // Converter.Options.MarginTop = 20;
            //Converter.Options.MarginBottom = 57;//20
            //Converter.Options.MarginRight = 94; //25
          // Converter.Options.MarginLeft = 94; //25
            //new document
            string statut = "ERROR";
            try
            {
                PdfDocument doc = Converter.ConvertUrl(url);
                //string filename = "factureSauvegarde.pdf"/*_url == "_journalCaisseReport" ? "caisseReport.pdf" : "TraitementReport.pdf"*/;
                string saveDir = Server.MapPath(System.IO.Path.Combine("~/Datadir/", filename));
                //saveDocument
                doc.Save(saveDir);
                doc.Close();
                statut = "OK";
            }
            catch (Exception e)
            {
                //
            }
            return statut;
        }
	}
}