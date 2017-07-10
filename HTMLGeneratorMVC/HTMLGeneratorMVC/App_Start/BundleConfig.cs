using System.Web;
using System.Web.Optimization;

namespace HTMLGeneratorMVC
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                       "~/Scripts/toastr.js",
                       "~/Scripts/toastrImp.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                        "~/Scripts/javascript.js"));
            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include(
                       "~/Scripts/colorpicker.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                     "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/colorpicker").Include(
            "~/Content/stylez.css"));
            bundles.Add(new StyleBundle("~/Content/stylez").Include(
            "~/Content/stylez.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunob").Include(
            "~/Scripts/jquery.unobtrusive*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                   "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new StyleBundle("~/css/autocomplete").Include("~/Content/jquery-ui.css"));
            bundles.Add(new ScriptBundle("~/js/autocomplete").Include(
                      "~/Scripts/jquery-ui.js"));
        }
    }
}
