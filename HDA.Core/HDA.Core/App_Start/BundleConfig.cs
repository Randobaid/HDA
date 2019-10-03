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
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/toastr.min.css"
                      )
                      );

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"
                ));


            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app.js",
                "~/Scripts/awesomplete.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/sweetalert2.all.min.js",
                "~/Scripts/highstock/highstock.js",
                "~/Scripts/toastr.min.js"
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
