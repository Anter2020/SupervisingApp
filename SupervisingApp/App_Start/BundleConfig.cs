using System.Web.Optimization;

namespace NajmDefault
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/style.css", "~/Content/fontawesome-all.css"));

            bundles.Add(new StyleBundle("~/Content/css2").Include("~/Content/w3.css", "~/Content/style2.css", "~/Content/bootstrap-superhero.css", "~/Content/fontawesome-all.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include("~/Scripts/jquery-2.2.3.min.js", "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts2").Include("~/Scripts/jquery-3.3.1.js", "~/Scripts/bootstrap-4.0.0.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

        }
    }
}
