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
        HtmlGenericControl tag = new HtmlGenericControl("div");
        tag.Attributes.Add("data-send", "true");
        tag.Attributes.Add("data-layout", "button_count");
        tag.Attributes.Add("data-width", "80");
        tag.Attributes.Add("data-show-faces", "false");
        tag.Attributes.Add("class", "fb-like");
        tag.Attributes.Add("data-href", HttpUtility.HtmlEncode(item.Link));
        return tag;
    }
}