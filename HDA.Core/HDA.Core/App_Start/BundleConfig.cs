using System.Web;
using System.Web.Optimization;

namespace HDA.Core
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
               
                "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/awesomplete.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/datatables.css",
                      "~/Content/pivot.css",
                      "~/Content/bootstrap-multiselect.css"
                      )
                      );

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"
                ));


            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app.js",
                "~/Scripts/awesomplete.js",
                "~/Scripts/datatables.js",
                "~/Scripts/pivot.js",
                "~/Scripts/bootstrap-multiselect.js",
                //"~/Scripts/highcharts/7.1.2/highcharts.js",
                "~/Scripts/sweetalert2.all.min.js",
                "~/Scripts/highstock/highstock.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/daterangepicker.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-rtl-css").Include(
                "~/Content/bootstrap-rtl/css/rtl/bootstrap.css"
                ));
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap-rtl-js").Include(
            //    "~/Content/bootstrap-rtl/js/bootstrap.js"
            //    ));
        }
    }
}
