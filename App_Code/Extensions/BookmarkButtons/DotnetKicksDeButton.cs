using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by DotNetKicks Germany http://www.dotnet-kicks.de
/// Button and function description can be found at http://dotnet-kicks.de/docs/button
/// </summary>
public class DotNetKicksDeButton : AdnButton
{
    public DotNetKicksDeButton() : base("DotnetKicksDe")
    {
        Defaults.Alignment = AdnHelper.Alignment.BottomLeft;
        Defaults.StaticVersion = true;
        Defaults.OrderNr = 100;
        Defaults.Enabled = false; 
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">DotNet-Kicks</span> (http://www.dotnet-kicks.de) is a GERMAN counter and publishing service for .NET articles. " +
                        "The button can only be displayed in one version. It can be used in RSS feeds." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        return settings;
    }

    public override HtmlControl RenderStatic(AdnBookmarkingItem item)
    {
        HtmlImage image = new HtmlImage();
        image.Src = "http://dotnet-kicks.de/Services/Images/KickItImageGenerator.ashx?";
        image.Src += "url=" + item.Link;
        image.Alt = Configuration.Name;
        image.Border = 0;


        HtmlAnchor anchor = new HtmlAnchor();
        anchor.HRef = "http://dotnet-kicks.de/kick/";
        anchor.HRef += "?url=" + HttpUtility.UrlEncode(item.Link);
        anchor.HRef += "&title=" + HttpUtility.UrlEncode(item.Title);
        anchor.HRef += "&description=" + HttpUtility.UrlEncode(item.Description);
        anchor.Target = "_blank";
        anchor.Controls.Add(image);

        return anchor;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        return RenderStatic(item);
    }
}
