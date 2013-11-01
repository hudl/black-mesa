using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/SlickGrid/slick.core.js",
                "~/Scripts/SlickGrid/slick.grid.js",
                "~/Scripts/SlickGrid/slick.dataview.js",
                "~/Scripts/SlickGrid/slick.editors.js",
                "~/Scripts/SlickGrid/Plugins/slick.*",
                "~/Scripts/main.js",
                "~/Scripts/slick-grid-custom-editor.js",
                "~/Scripts/toastr.js",
                "~/Scripts/deployform.js",
                "~/Scripts/hotfix.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/login.js",
                "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/history").Include(
                "~/Scripts/SlickGrid/slick.core.js",
                "~/Scripts/SlickGrid/slick.grid.js",
                "~/Scripts/SlickGrid/slick.dataview.js",
                "~/Scripts/SlickGrid/slick.editors.js",
                "~/Scripts/SlickGrid/Plugins/slick.*",
                "~/Scripts/history.js"));

            bundles.Add(new ScriptBundle("~/bundles/components").Include(
                "~/Scripts/components.js"));

            bundles.Add(new ScriptBundle("~/bundles/projects").Include(
                "~/Scripts/projects.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}