using System.Web;
using System.Web.Optimization;

namespace OpticaVL
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/alertifyjs/alertify.min.css",
                      "~/Content/alertifyjs/themes/default.min.css",
                      "~/Content/alertifyjs/themes/semantic.min.css",
                      "~/Content/alertifyjs/themes/bootstrap.min.css",
                      "~/Content/site.css"));

            //mes styles
            bundles.Add(new StyleBundle("~/Template/css").Include(
                    "~/Template/bootstrap/css/bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css_").Include(
                "~/Content/font-awesome.min.css",
                "~/Content/alertifyjs/alertify.min.css",
                "~/Content/alertifyjs/themes/bootstrap.min.css",
                "~/Content/ionicons.min.css"
                ));
            //Theme Style
            bundles.Add(new StyleBundle("~/Template/css_").Include(
                "~/Template/dist/css/AdminLTE.min.css",
                "~/Template/dist/css/skins/_all-skins.min.css",
                "~/Template/plugins/iCheck/flat/blue.css",
                "~/Template/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css__").Include(
                    "~/Content/jqx.base.css",
                    "~/Content/jqx.darkblue.css",
                    "~/Content/jqx.energyblue.css",
                    "~/Content/jqx.fresh.css",
                    "~/Content/jqx.metro.css",
                    "~/Content/jqx.summer.css",
                    "~/Content/jqx.shinyblack.css",
                    "~/Content/jqx.glacier.css",
                    "~/Content/jqx.highcontrast.css"
                ));

            bundles.Add(new StyleBundle("~/Content/loginStyle").Include(
                    "~/Template/bootstrap/css/bootstrap.min.css",
                    "~/Content/font-awesome.min.css",
                    "~/Template/dist/css/AdminLTE.min.css",
                    "~/Template/plugins/iCheck/square/blue.css"
                ));


            //mes scripts "~/Template/bootstrap/js/bootstrap.min.js",
            bundles.Add(new ScriptBundle("~/Template/js").Include(
                    "~/Template/plugins/sparkline/jquery.sparkline.min.js",
                    "~/Template/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                    "~/Template/plugins/iCheck/icheck.min.js",
                    "~/Template/plugins/slimScroll/jquery.slimscroll.min.js",
                    "~/Template/plugins/fastclick/fastclick.min.js",
                    "~/Template/dist/js/app.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                "~/Scripts/globalization/globalize.js",
                "~/Scripts/globalization/globalize/globalize.culture.fr-FR.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/knockout-3.3.0.js",
                "~/Scripts/alertify.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/js_").Include(
                    "~/Scripts/jqxcore.js",
                    "~/Scripts/jqxdata.js",
                    "~/Scripts/jqxknockout.js",
                    "~/Scripts/jqxscrollbar.js",
                    "~/Scripts/jqxlistbox.js",
                    "~/Scripts/jqxexpander.js",
                    "~/Scripts/jqxbuttons.js",
                    "~/Scripts/jqxpanel.js",
                    "~/Scripts/jqxdatatable.js",
                    "~/Scripts/jqxdropdownlist.js",
                    "~/Scripts/jqxtree.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/loginJs").Include(
                    "~/Template/plugins/jQuery/jQuery-2.1.3.min.js",
                    "~/Template/bootstrap/js/bootstrap.min.js",
                    "~/Template/plugins/iCheck/icheck.js"
                ));
        }
    }
}
