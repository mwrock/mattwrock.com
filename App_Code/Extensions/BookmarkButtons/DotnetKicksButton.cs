using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by DotNetKicks http://www.dotnetkicks.com
/// Button and function description can be found at http://dotnetkicks.com/docs/kickitbadge
/// </summary>
public class DotNetKicksButton : AdnButton
{
    public DotNetKicksButton() : base("DotnetKicks")
    {
        Defaults.Alignment = AdnHelper.Alignment.BottomLeft;
        Defaults.StaticVersion = true;
        Defaults.OrderNr = 100;
        Defaults.Enabled = false; 
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">DotNetKicks</span> (http://www.dotnetkicks.com) counter and publishing service for .NET articles. " +
                        "The button can only be displayed in one version. It can be used in RSS feeds." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        return settings;

    }

    public override HtmlControl RenderStatic(AdnBookmarkingItem item)
    {
        HtmlImage image = new HtmlImage();
        image.Src = "http://dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?";
        image.Src += "url=" + item.Link;
        image.Alt = Configuration.Name;
        image.Border = 0;


        HtmlAnchor anchor = new HtmlAnchor();
        anchor.HRef = "http://www.addthis.com/bookmark.php?v=250";
        anchor.HRef += "&url=" + HttpUtility.UrlEncode(item.Link);
        anchor.HRef += "&title=" + HttpUtility.UrlEncode(item.Title);
        anchor.HRef += "&description=" + HttpUtility.UrlEncode(item.Description);
        anchor.HRef += "&s=dotnetkicks";
        anchor.Target = "_blank";
        anchor.Controls.Add(image);

        return anchor;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        return RenderStatic(item);
    }
}
