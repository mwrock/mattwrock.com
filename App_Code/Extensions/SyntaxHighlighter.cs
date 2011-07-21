﻿using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Page=System.Web.UI.Page;
using System.Collections.Generic;
using System;

[Extension("Adds <a target=\"_new\" href=\"http://alexgorbatchev.com/wiki/SyntaxHighlighter\">Alex Gorbatchev's</a> source code formatter", "2.5", "<a target=\"_new\" href=\"http://dotnetblogengine.net/\">BlogEngine.NET</a>")]
public class SyntaxHighlighter
{
    #region Private members
    private const string ExtensionName = "SyntaxHighlighter";
    static protected Dictionary<Guid, ExtensionSettings> _blogsOptions = new Dictionary<Guid, ExtensionSettings>();
    static protected Dictionary<Guid, ExtensionSettings> _blogsBrushes = new Dictionary<Guid, ExtensionSettings>();
    static protected Dictionary<Guid, ExtensionSettings> _blogsThemes = new Dictionary<Guid, ExtensionSettings>();
    #endregion

    /// <summary>
    ///     The sync root.
    /// </summary>
    private static readonly object syncRoot = new object();

    private static ExtensionSettings Options
    {
        get
        {
            Guid blogId = Blog.CurrentInstance.Id;
            ExtensionSettings options = null;
            _blogsOptions.TryGetValue(blogId, out options);

            if (options == null)
            {
                lock (syncRoot)
                {
                    _blogsOptions.TryGetValue(blogId, out options);

                    if (options == null)
                    {
                        // Initializes
                        //   (1) Options
                        //   (2) Brushes
                        //   (3) Themees
                        // for the current blog instance.

                        // options
                        options = new ExtensionSettings("Options");
                        options.IsScalar = true;
                        options.Help = OptionsHelp();

                        options.AddParameter("gutter", "Gutter");
                        options.AddParameter("smart-tabs", "Smart tabs");
                        options.AddParameter("auto-links", "Auto links");
                        options.AddParameter("collapse", "Collapse");
                        options.AddParameter("light", "Light");
                        options.AddParameter("tab-size", "Tab size");
                        options.AddParameter("toolbar", "Toolbar");
                        options.AddParameter("wrap-lines", "Wrap lines");

                        options.AddValue("gutter", true);
                        options.AddValue("smart-tabs", true);
                        options.AddValue("auto-links", true);
                        options.AddValue("collapse", false);
                        options.AddValue("light", false);
                        options.AddValue("tab-size", 4);
                        options.AddValue("toolbar", true);
                        options.AddValue("wrap-lines", true);

                        _blogsOptions[blogId] = ExtensionManager.InitSettings(ExtensionName, options);

                        // brushes
                        ExtensionSettings brushes = new ExtensionSettings("Brushes");
                        brushes.IsScalar = true;

                        brushes.AddParameter("shBrushBash", "Bash (bash, shell)", 100, false);
                        brushes.AddParameter("shBrushCpp", "C++ (cpp, c)", 100, false);
                        brushes.AddParameter("shBrushCSharp", "C# (c-sharp, csharp)", 100, false);
                        brushes.AddParameter("shBrushCss", "Css (css)", 100, false);
                        brushes.AddParameter("shBrushDelphi", "Delphi (delphi, pas, pascal)", 100, false);
                        brushes.AddParameter("shBrushDiff", "Diff (diff, patch)", 100, false);
                        brushes.AddParameter("shBrushGroovy", "Groovy (groovy)", 100, false);
                        brushes.AddParameter("shBrushJava", "Java (java)", 100, false);
                        brushes.AddParameter("shBrushJScript", "JScript (js, jscript, javascript)", 100, false);
                        brushes.AddParameter("shBrushPhp", "PHP (php)", 100, false);
                        brushes.AddParameter("shBrushPlain", "Plain (plain, text)", 100, false);
                        brushes.AddParameter("shBrushPython", "Python (py, python)", 100, false);
                        brushes.AddParameter("shBrushRuby", "Ruby (rails, ror, ruby)", 100, false);
                        brushes.AddParameter("shBrushScala", "Scala (scala)", 100, false);
                        brushes.AddParameter("shBrushSql", "SQL (sql)", 100, false);
                        brushes.AddParameter("shBrushVb", "VB (vb, vbnet)", 100, false);
                        brushes.AddParameter("shBrushXml", "XML (xml, xhtml, xslt, html, xhtml)", 100, false);
                        brushes.AddParameter("shBrushColdFusion", "Cold Fusion (cf, coldfusion)", 100, false);
                        brushes.AddParameter("shBrushErlang", "Erlang (erlang, erl)", 100, false);
                        brushes.AddParameter("shBrushJavaFX", "JavaFX (jfx, javafx)", 100, false);
                        brushes.AddParameter("shBrushPerl", "Perl (perl, pl)", 100, false);
                        brushes.AddParameter("shBrushPowerShell", "PowerSell (ps, powershell)", 100, false);

                        brushes.AddValue("shBrushBash", false);
                        brushes.AddValue("shBrushCpp", false);
                        brushes.AddValue("shBrushCSharp", true);
                        brushes.AddValue("shBrushCss", true);
                        brushes.AddValue("shBrushDelphi", false);
                        brushes.AddValue("shBrushDiff", false);
                        brushes.AddValue("shBrushGroovy", false);
                        brushes.AddValue("shBrushJava", false);
                        brushes.AddValue("shBrushJScript", true);
                        brushes.AddValue("shBrushPhp", false);
                        brushes.AddValue("shBrushPlain", true);
                        brushes.AddValue("shBrushPython", false);
                        brushes.AddValue("shBrushRuby", false);
                        brushes.AddValue("shBrushScala", false);
                        brushes.AddValue("shBrushSql", true);
                        brushes.AddValue("shBrushVb", true);
                        brushes.AddValue("shBrushXml", true);
                        brushes.AddValue("shBrushColdFusion", false);
                        brushes.AddValue("shBrushErlang", false);
                        brushes.AddValue("shBrushJavaFX", false);
                        brushes.AddValue("shBrushPerl", false);
                        brushes.AddValue("shBrushPowerShell", false);

                        _blogsBrushes[blogId] = ExtensionManager.InitSettings(ExtensionName, brushes);

                        // themes
                        ExtensionSettings themes = new ExtensionSettings("Themes");
                        themes.IsScalar = true;
                        themes.AddParameter("SelectedTheme", "Themes", 20, false, false, ParameterType.ListBox);
                        themes.AddValue("SelectedTheme", new string[] { "Default", "Django", "Eclipse", "Emacs", "FadeToGrey", "MDUltra", "Midnight", "Dark" }, "Default");
                        _blogsThemes[blogId] = ExtensionManager.InitSettings(ExtensionName, themes);
                    }
                }
            }

            return options;
        }
    }

    private static ExtensionSettings Brushes
    {
        get
        {
            // by invoking the "Options" property getter, we are ensuring
            // that an entry is put into _blogsBrushes for the current blog instance.
            ExtensionSettings options = Options;
            return _blogsBrushes[Blog.CurrentInstance.Id];
        }
    }

    private static ExtensionSettings Themes
    {
        get
        {
            // by invoking the "Options" property getter, we are ensuring
            // that an entry is put into _blogsThemes for the current blog instance.
            ExtensionSettings options = Options;
            return _blogsThemes[Blog.CurrentInstance.Id];
        }
    }

    static SyntaxHighlighter()
    {
        Post.Serving += AddSyntaxHighlighter;
        InitSettings();
    }

    private static void AddSyntaxHighlighter(object sender, ServingEventArgs e)
    {
        if (!ExtensionManager.ExtensionEnabled("SyntaxHighlighter"))
            return;

		if(e.Location == ServingLocation.Feed) 
            return;
	
        HttpContext context = HttpContext.Current;
		
        Page page = (Page)context.CurrentHandler;

        if ((context.CurrentHandler is Page == false) || (context.Items[ExtensionName] != null))
        {
            return;
        }

        AddCssStyles(page);
        AddJavaScripts(page);
        AddOptions(page);

        context.Items[ExtensionName] = 1;
    }

    private static void AddCssStyles(Page page)
    {
        AddStylesheet("shCore.css", page);

        if (Themes != null)
        {
            switch (Themes.GetSingleValue("SelectedTheme"))
            {
                case "Django":
                    AddStylesheet("shThemeDjango.css", page);
                    break;
                case "Eclipse":
                    AddStylesheet("shThemeEclipse.css", page);
                    break;
                case "Emacs":
                    AddStylesheet("shThemeEmacs.css", page);
                    break;
                case "FadeToGrey":
                    AddStylesheet("shThemeFadeToGrey.css", page);
                    break;
                case "MDUltra":
                    AddStylesheet("shThemeMDUltra.css", page);
                    break;
                case "Midnight":
                    AddStylesheet("shThemeMidnight.css", page);
                    break;
                case "Dark":
                    AddStylesheet("shThemeRDark.css", page);
                    break;
                default:
                    AddStylesheet("shThemeDefault.css", page);
                    break;
            }
        }       
    }

    private static void AddJavaScripts(Page page)
    {
        AddJavaScript("shCore.js", page);

        if (Brushes != null)
        {
            if (Brushes.GetSingleValue("shBrushBash").ToLowerInvariant() == "true")
                AddJavaScript("shBrushBash.js", page);

            if (Brushes.GetSingleValue("shBrushCpp").ToLowerInvariant() == "true")
                AddJavaScript("shBrushCpp.js", page);

            if (Brushes.GetSingleValue("shBrushCSharp").ToLowerInvariant() == "true")
                AddJavaScript("shBrushCSharp.js", page);

            if (Brushes.GetSingleValue("shBrushCss").ToLowerInvariant() == "true")
                AddJavaScript("shBrushCss.js", page);

            if (Brushes.GetSingleValue("shBrushDelphi").ToLowerInvariant() == "true")
                AddJavaScript("shBrushDelphi.js", page);

            if (Brushes.GetSingleValue("shBrushDiff").ToLowerInvariant() == "true")
                AddJavaScript("shBrushDiff.js", page);

            if (Brushes.GetSingleValue("shBrushGroovy").ToLowerInvariant() == "true")
                AddJavaScript("shBrushGroovy.js", page);

            if (Brushes.GetSingleValue("shBrushJava").ToLowerInvariant() == "true")
                AddJavaScript("shBrushJava.js", page);

            if (Brushes.GetSingleValue("shBrushJScript").ToLowerInvariant() == "true")
                AddJavaScript("shBrushJScript.js", page);

            if (Brushes.GetSingleValue("shBrushPhp").ToLowerInvariant() == "true")
                AddJavaScript("shBrushPhp.js", page);

            if (Brushes.GetSingleValue("shBrushPlain").ToLowerInvariant() == "true")
                AddJavaScript("shBrushPlain.js", page);

            if (Brushes.GetSingleValue("shBrushPython").ToLowerInvariant() == "true")
                AddJavaScript("shBrushPython.js", page);

            if (Brushes.GetSingleValue("shBrushRuby").ToLowerInvariant() == "true")
                AddJavaScript("shBrushRuby.js", page);

            if (Brushes.GetSingleValue("shBrushScala").ToLowerInvariant() == "true")
                AddJavaScript("shBrushScala.js", page);

            if (Brushes.GetSingleValue("shBrushSql").ToLowerInvariant() == "true")
                AddJavaScript("shBrushSql.js", page);

            if (Brushes.GetSingleValue("shBrushVb").ToLowerInvariant() == "true")
                AddJavaScript("shBrushVb.js", page);

            if (Brushes.GetSingleValue("shBrushXml").ToLowerInvariant() == "true")
                AddJavaScript("shBrushXml.js", page);

            if (Brushes.GetSingleValue("shBrushColdFusion").ToLowerInvariant() == "true")
                AddJavaScript("shBrushColdFusion.js", page);

            if (Brushes.GetSingleValue("shBrushErlang").ToLowerInvariant() == "true")
                AddJavaScript("shBrushErlang.js", page);

            if (Brushes.GetSingleValue("shBrushJavaFX").ToLowerInvariant() == "true")
                AddJavaScript("shBrushJavaFX.js", page);

            if (Brushes.GetSingleValue("shBrushPerl").ToLowerInvariant() == "true")
                AddJavaScript("shBrushPerl.js", page);

            if (Brushes.GetSingleValue("shBrushPowerShell").ToLowerInvariant() == "true")
                AddJavaScript("shBrushPowerShell.js", page);
        }
    }

    #region Script/Style adding

    private static void AddJavaScript(string src, Page page)
    {
        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes["type"] = "text/javascript";
        script.Attributes["src"] = GetUrl(ScriptsFolder(), src);
        page.Header.Controls.Add(script);
    }

    private static void AddStylesheet(string href, Page page)
    {
        HtmlLink css = new HtmlLink();
        css.Attributes["type"] = "text/css";
        css.Attributes["rel"] = "stylesheet";
        css.Attributes["href"] = GetUrl(StylesFolder(), href);
        page.Header.Controls.Add(css);
    }

    private static void AddOptions(Page page)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine("\n\n<script type=\"text/javascript\">");
        sb.AppendLine(string.Format("\tSyntaxHighlighter.config.clipboardSwf='{0}';", GetUrl(ScriptsFolder(), "clipboard.swf")));

        if (Options != null)
        {
            sb.AppendLine(GetOption("gutter"));
            sb.AppendLine(GetOption("smart-tabs"));
            sb.AppendLine(GetOption("auto-links"));
            sb.AppendLine(GetOption("collapse"));
            sb.AppendLine(GetOption("light"));
            sb.AppendLine(GetOption("tab-size"));
            sb.AppendLine(GetOption("toolbar"));
            sb.AppendLine(GetOption("wrap-lines"));
        }  
        
        sb.AppendLine("\tSyntaxHighlighter.all();");
        sb.AppendLine("</script>\n\n");
        page.ClientScript.RegisterStartupScript(page.GetType(), "SyntaxHighlighter", sb.ToString(), false);
    }

    private static string GetUrl(string folder, string url)
    {
        string s = HttpContext.Current.Server.UrlPathEncode(string.Format("{0}{1}", folder, url));
        s = Utils.ApplicationRelativeWebRoot + s;
        return s;
    }
    
    #endregion

    #region Private methods

    private static void InitSettings()
    {
        // call Options getter so default settings are loaded on application start.
        var s = Options;
    }

    static string OptionsHelp()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<p>This extension implements excellent Alex Gorbatchev's syntax highlighter JS library for source code formatting. Please refer to <a target=\"_new\" href=\"http://alexgorbatchev.com/wiki/SyntaxHighlighter:Usage\">this site</a> for usage.</p>");
        sb.AppendLine("<p><b>auto-links</b>: Allows you to turn detection of links in the highlighted element on and off. If the option is turned off, URLs won't be clickable.</p>");
        sb.AppendLine("<p><b>collapse</b>: Allows you to force highlighted elements on the page to be collapsed by default.</p>");
        sb.AppendLine("<p><b>gutter</b>:	Allows you to turn gutter with line numbers on and off.</p>");
        sb.AppendLine("<p><b>light</b>: Allows you to disable toolbar and gutter with a single property.</p>");
        sb.AppendLine("<p><b>smart-tabs</b>:	Allows you to turn smart tabs feature on and off.</p>");
        sb.AppendLine("<p><b>tab-size</b>: Allows you to adjust tab size.</p>");
        sb.AppendLine("<p><b>toolbar</b>: Toggles toolbar on/off.</p>");
        sb.AppendLine("<p><b>wrap-lines</b>: Allows you to turn line wrapping feature on and off.</p>");
        sb.AppendLine("<p><a target=\"_new\" href=\"http://alexgorbatchev.com/wiki/SyntaxHighlighter:Configuration\">more...</a></p>");
        return sb.ToString();
    }

    static string GetOption(string opt)
    {
        if (Options != null)
        {
            string pattern = "\tSyntaxHighlighter.defaults['{0}'] = {1};";
            string val = Options.GetSingleValue(opt).ToLowerInvariant();
            return string.Format(pattern, opt, val);
        }
        return "";
    }

    static string ScriptsFolder()
    {
        if (Options != null)
        {
            return "Scripts/syntaxhighlighter/";
        }
        return "";
    }

    static string StylesFolder()
    {
        if (Options != null)
        {
            return "Styles/syntaxhighlighter/";
        }
        return "";
    }

    #endregion
}