using System.Web.Optimization;

namespace HakunaMatataWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var fullCalendarJS = "https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.9.0/fullcalendar.min.js";
            var fullCalendarCSS = "https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.9.0/fullcalendar.min.css";
            var fullCalendarBundle = new ScriptBundle("~/bundles/calendar", fullCalendarJS);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/FontAwesome/all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                "~/Scripts/moment-js.min.js"));

            bundles.Add(fullCalendarBundle);

            bundles.Add(new StyleBundle("~/Calendar/css", fullCalendarCSS));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/scss/bootstrap/bootstrap.min.css",
                        "~/Content/site.css",
                        "~/Content/scss/main-site.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Sidebar/css").Include(
                      //"~/Content/scss/main-site.min.css",
                      "~/Content/scss/sidebar.min.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}