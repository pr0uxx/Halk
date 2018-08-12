using System.Web.Optimization;

namespace HakunaMatataWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/input-focus.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/MarkdownCheatSheet", "https://gist.github.com/jonschlinkert/5854601.js").Include(
                "~/Scripts/MarkdownCheatSheet.js"));

            bundles.Add(new ScriptBundle("~/bundles/Fonts").Include(
                      "~/Scripts/FontAwesome/all.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                "~/Scripts/moment-js.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/scss/bootstrap/bootstrap.min.css",
                        "~/Content/site.css",
                        "~/Content/scss/main-site.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Sidebar/css").Include(
                      //"~/Content/scss/main-site.min.css",
                      "~/Content/scss/sidebar.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}