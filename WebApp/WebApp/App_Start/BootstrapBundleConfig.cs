using System.Web.Optimization;

namespace WebApp
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery-1.*",
                "~/Scripts/jquery-ui-1.*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "~/Scripts/jquery.dateFormat-1.0.js",
                "~/Scripts/underscore.js",
                "~/Scripts/jquery.event.drag.js",
                "~/Scripts/SlickGrid/slick.core.js",
                "~/Scripts/SlickGrid/slick.grid.js",
                "~/Scripts/SlickGrid/slick.dataview.js",
                "~/Scripts/SlickGrid/Plugins/slick.rowselectionmodel.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/slick.grid.css",
                "~/Content/grid.css"
                ));
            bundles.Add(new StyleBundle("~/content/css-responsive").Include(
                "~/Content/bootstrap-responsive.css"
                ));
        }
    }
}