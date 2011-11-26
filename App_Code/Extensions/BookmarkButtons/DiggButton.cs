using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by Digg http://www.digg.com
/// Button and function description can be found at http://about.digg.com/downloads/button/smart
/// </summary>
public class DiggButton : AdnButton
{
    public DiggButton() : base("Digg") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Digg</span> (http://www.digg.com) is a famous social bookmarking service. " +
                       "The button can be displayed in a small and a large version, a static version with counter isn't supported by digg." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view."; 

        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");

        return settings;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerText = HttpUtility.HtmlEncode(item.Description);
        span.Attributes.Add("style", " display:none");

        HtmlAnchor anchor = new HtmlAnchor();
        anchor.HRef = "http://digg.com/submit?";
        anchor.HRef += "url=" + HttpUtility.UrlEncode(item.Link);
        anchor.HRef += "&title=" + HttpUtility.UrlEncode(item.Title);
        anchor.Controls.Add(span); 

        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            anchor.Attributes.Add("class", "DiggThisButton DiggCompact"); 
        }
        else
        {
            anchor.Attributes.Add("class", "DiggThisButton DiggMedium"); 
        }

        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("type", "text/javascript");
        script.InnerText = "(function() {" +
                            "var s = document.createElement('SCRIPT'), s1 = document.getElementsByTagName('SCRIPT')[0];" +
                            "s.type = 'text/javascript';" +
                            "s.async = true;" +
                            "s.src = 'http://widgets.digg.com/buttons.js';" +
                            "s1.parentNode.insertBefore(s, s1);" +
                            "})();";

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Controls.Add(script);
        div.Controls.Add(anchor);

        return div;
    }
}