using System.Web;
using System.Web.Optimization;
using System.Web.Optimization.React;

namespace WageCalculator
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //js bundles
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                          "~/Scripts/dist/app.bundle.js"));

            //css bundles
            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                       "~/Content/stylesheets/styles.css"));
        }
    }
}
