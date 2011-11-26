using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by DotNetShoutout http://dotnetshoutout.com/
/// Button and function description can be found at http://dotnetshoutout.com/Faq
/// </summary>
public class DotNetShoutoutButton : AdnButton
{
    public DotNetShoutoutButton() : base("DotNetShoutout")
    {
        Defaults.Alignment = AdnHelper.Alignment.BottomLeft;
        Defaults.StaticVersion = true;
        Defaults.OrderNr = 100;
        Defaults.Enabled = false; 
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">DotNetShoutout</span> (http://dotnetshoutout.com/) counter and publishing service for .NET articles. " +
                        "The button can only be displayed in one version. It can be used in RSS feeds." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        return settings;
    }

    public override HtmlControl RenderStatic(AdnBookmarkingItem item)
    {
        HtmlImage image = new HtmlImage();
        image.Src = "http://dotnetshoutout.com/image.axd?";
        image.Src += "url=" + item.Link;
        image.Alt = Configuration.Name;
        image.Border = 0;
        image.Height = 19;
        image.Attributes.Add("style", "height:19px");

        HtmlAnchor anchor = new HtmlAnchor();
        anchor.HRef = "http://www.addthis.com/bookmark.php?v=250";
        anchor.HRef += "&url=" + HttpUtility.UrlEncode(item.Link);
        anchor.HRef += "&title=" + HttpUtility.UrlEncode(item.Title);
        anchor.HRef += "&description=" + HttpUtility.UrlEncode(item.Description);
        anchor.HRef += "&s=dotnetshoutout";
        anchor.Target = "_blank";
        anchor.Controls.Add(image);

        return anchor;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        return RenderStatic(item);
    }
}