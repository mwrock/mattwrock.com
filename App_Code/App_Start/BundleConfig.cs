using System.Web.Optimization;

/// <summary>
/// Summary description for BundleConfig
/// </summary>
public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        // for anonymous users
        bundles.Add(new StyleBundle("~/Content/Auto/css").Include(
            "~/Content/Auto/*.css")
        );
        bundles.Add(new ScriptBundle("~/Scripts/Auto/js").Include(
            "~/Scripts/Auto/*.js")
        );

        // for authenticated users
        bundles.Add(new StyleBundle("~/Content/Auto/cssauth").Include(
            "~/Content/Auto/*.css",
            "~/Modules/QuickNotes/Qnotes.css")
        );  
        bundles.Add(new ScriptBundle("~/Scripts/Auto/jsauth").Include(
            "~/Scripts/Auto/*.js")
        );

        // administration
        bundles.Add(new StyleBundle("~/admin/css").Include(
            "~/admin/style.css",
            "~/admin/colorbox.css",
            "~/admin/tipsy.css")
        );
        bundles.Add(new ScriptBundle("~/Scripts/adminjs").Include(
            "~/Scripts/jquery-1.8.2.js",
            "~/Scripts/jquery.cookie.js",
            "~/Scripts/jquery.validate.js",
            "~/Scripts/jquery-jtemplates.js",
            "~/admin/admin.js")
        );

        // syntax highlighter 
        var shRoot = "~/editors/tiny_mce_3_5_8/plugins/syntaxhighlighter/";
        bundles.Add(new StyleBundle("~/Content/highlighter").Include(
            shRoot + "styles/shCore.css",
            shRoot + "styles/shThemeDefault.css")
        );
        bundles.Add(new ScriptBundle("~/Scripts/highlighter").Include(
            shRoot + "scripts/XRegExp.js",
            shRoot + "scripts/shCore.js",
            shRoot + "scripts/shAutoloader.js",
            shRoot + "shActivator.js")
        );

        // syntax FileManager 
        bundles.Add(new StyleBundle("~/Content/filemanager").Include(
            "~/admin/FileManager/FileManager.css",
            "~/admin/uploadify/uploadify.css",
            "~/admin/FileManager/jqueryui/jquery-ui.css",
            "~/admin/FileManager/JCrop/css/jquery.Jcrop.css")
        );
        bundles.Add(new ScriptBundle("~/Scripts/filemanager").Include(
            "~/admin/uploadify/swfobject.js",
            "~/admin/uploadify/jquery.uploadify.v2.1.4.min.js",
            "~/admin/FileManager/jqueryui/jquery-ui.min.js",
            "~/admin/FileManager/jquery.jeegoocontext.min.js",
            "~/admin/FileManager/JCrop/js/jquery.Jcrop.min.js",
            "~/admin/FileManager/FileManager-mini.js")
        );

    }
}