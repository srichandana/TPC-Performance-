using System.Web;
using System.Web.Optimization;

namespace TPC.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                 "~/Scripts/js/jquery/jquery.min.js", 
                 "~/Scripts/js/jquery/jquery.widget.min.js",
                        "~/Scripts/js/jquery/jquery.mousewheel.js", 
                        "~/Scripts/js/jquery/jquery.ui.core.min.js",
                        "~/Scripts/jquery-{version}.js", 
                        "~/Scripts/jquery.magnific-popup.js",
                        "~/Scripts/jquery-ui-{version}.js", 
                        "~/Scripts/jquery-ui-1.10.4.custom.min.js", 
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*", 
                        "~/Scripts/js/jquery/jquery.ui.accordion.min.js", 
                        "~/Scripts/js/tablesorter/prettify.js",
                        "~/Scripts/js/tablesorter/docs.js",
                        "~/Scripts/js/tablesorter/jquery.tablesorter.js",
                        "~/Scripts/js/tablesorter/jquery.tablesorter.widgets.js",
                        "~/Scripts/js/tablesorter/jquery.quicksearch.js",
                        "~/Scripts/js/metro/metro-carousel.js",
                        "~/Scripts/js/metro/metro-dialog.js",
                        "~/Scripts/js/metro/metro-dropdown.js",
                        "~/Scripts/js/metro/metro-progressbar.js",
                        "~/Scripts/js/metro/metro-treeview.js", 
                        "~/Scripts/js/metro/metro-accordion.js",
                        "~/Scripts/js/DataTable/jquery.dataTables.js",
                        "~/Scripts/js/DataTable/fnSortNeutral.js"
                        //, "~/Scripts/js/metro/metro-autocarousel.js"
                         ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        ));

            bundles.Add(new StyleBundle("~/Content/TableSorter/css").Include(
                                                                        "~/Content/TableSorter/jq.css",
                                                                        "~/Content/TableSorter/prettify.css",
                                                                        "~/Content/TableSorter/theme.blue.css",
                                                                        "~/Content/TableSorter/style.css", "~/Content/Layout.css",
                                                                        "~/Content/magnific-popup.css",
                                                                        "~/Content/perfect-scrollbar.css",
                                                                        "~/Content/Metro/metro-bootstrap.css",
                                                                        "~/Content/themes/base/jquery.ui.accordion.css",
                                                                        "~/Content/themes/base/jquery.ui.theme.css",
                                                                        "~/Content/DataTables/jquery.dataTables.css"));


            bundles.Add(new ScriptBundle("~/bundles/commonscript").Include("~/Scripts/js/PenworthyScript.js"));
        }
    }
}