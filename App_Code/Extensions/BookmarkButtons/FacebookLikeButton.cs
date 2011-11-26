using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by Facebook http://www.facebook.com
/// Button and function description can be found at http://developers.facebook.com/docs/reference/plugins/like
/// </summary>
public class FacebookLikeButton : AdnButton
{
    public FacebookLikeButton() : base("FacebookLike") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Facebook Like</span> (http://www.facebook.com/) is a counter service (no bookmarking service) and shows how many readers like your article. " +
                        "The button can be displayed in a small and a large version, a static version with counter isn't supported by facebook." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");

        return settings;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        string src = "http://www.facebook.com/plugins/like.php?href="+HttpUtility.HtmlEncode(item.Link);
        src += "&show_faces=false";
    //    src += "&width=80";
        src += "&action=like";
        src += "&colorscheme=light";
        src += "&height=21";

        string style = "border:none;";
        style += "overflow:hidden;"; 

        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            src += "&layout=button_count";
                src += "&width=80";
                style += "width:80px;";
                style += "height:21px"; 
        }
        else
        {
            src += "&layout=box_count";
            src += "&width=57";
            style += "width:57px;";
            style += "height:62px"; 

        }

        HtmlGenericControl iframe = new HtmlGenericControl("iframe");
        iframe.Attributes.Add("src", src);
        iframe.Attributes.Add("scrolling", "no");
        iframe.Attributes.Add("frameborder", "0");
        iframe.Attributes.Add("allowTransparency", "true");
        iframe.Attributes.Add("style", style); //"border:none; overflow:hidden; width:80px;");

        return iframe;
    }
}