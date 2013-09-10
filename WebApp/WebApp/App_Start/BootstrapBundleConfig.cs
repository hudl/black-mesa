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
                "~/Scripts/select2.js",
                "~/Scripts/jquery.simplemodal-1.4.4.js",
                "~/Scripts/jquery.event.drag.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/slick.grid.css",
                "~/Content/toastr-responsive.css",
                "~/Content/toastr.css",
                "~/Content/Site.css",
                "~/Content/select2.css",
                "~/Content/grid.css"
                ));
            bundles.Add(new StyleBundle("~/content/css-responsive").Include(
                "~/Content/bootstrap-responsive.css"
                ));
        }
    }
}