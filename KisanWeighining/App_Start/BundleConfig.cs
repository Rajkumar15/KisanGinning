using System.Web;
using System.Web.Optimization;

namespace KisanWeighining
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/DEVJS").Include(
                       "~/assets/lib/jquery/jquery.js",
                       "~/assets/lib/bootstrap/js/bootstrap.js",
                       "~/assets/lib/metismenu/metisMenu.js",
                       "~/assets/lib/onoffcanvas/onoffcanvas.js",
                       "~/assets/lib/screenfull/screenfull.js",
                       "~/assets/js/core.js",
                       "~/assets/js/app.js",
                       "~/Content/Rajfile/jsalert/jquery.alerts.js",
                       "~/Content/Rajfile/jquery.dataTables.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/DEVcss").Include(
                     "~/assets/lib/bootstrap/css/bootstrap.css",
                     "~/assets/lib/font-awesome/css/font-awesome.css",
                     "~/assets/css/main.css",
                     "~/assets/lib/metismenu/metisMenu.css",
                     "~/assets/lib/onoffcanvas/onoffcanvas.css",
                     "~/assets/lib/animate.css/animate.css",
                     "~/assets/less/theme.less",
                     "~/Content/Rajfile/jsalert/jquery.alerts.css"));
        }
    }
}
