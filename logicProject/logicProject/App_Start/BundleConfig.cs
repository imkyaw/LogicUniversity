using System.Web;
using System.Web.Optimization;

namespace logicProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new Bundle("~/my-admin-lte/js").Include(
                    "~/my-admin-lte/js/adminlte.js",
                     //"~/my-admin-lte/plugins/datatables.net/js/jquery.dataTables.min.js",
                     //"~/my-admin-lte/plugins/datatables.net-bs/js/dataTables.bootstrap.min.js",
                     "~/my-admin-lte/plugins/fastclick/fastclick.js",
                     "~/my-admin-lte/plugins/input-mask/jquery.inputmask.js",
                     "~/my-admin-lte/plugins/input-mask/jquery.inputmask.date.extensions",
                     
                     "~/my-admin-lte/plugins/select2/dist/js/select2.full.min.js",
                     "~/my-admin-lte/plugins/moment/min/moment.min.js",
                     "~/my-admin-lte/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                     "~/my-admin-lte/js/jquery-ui.min.js",
                     "~/my-admin-lte/plugins/jquery-slimscroll/jquery.slimscroll.min.js"
                ));
        }
    }
}
