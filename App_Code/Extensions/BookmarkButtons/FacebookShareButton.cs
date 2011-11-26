using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by Facebook http://www.facebook.com
/// Button and function description can be found at http://www.facebook.com/share/
/// </summary>
public class FacebookShareButton : AdnButton
{
    public FacebookShareButton() : base("FacebookShare") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
      
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Facebook Share</span> (http://www.facebook.com/) is a counter and publishing service. " +
                "The button can be displayed in a small and a large version, a static version with counter isn't supported by facebook." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");

        return settings;
    }

    public override HtmlControl RegisterScript()
    {
        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("src", "http://static.ak.fbcdn.net/connect.php/js/FB.Share");
        script.Attributes.Add("type", "text/javascript");


        return script; 
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        HtmlAnchor anchor = new HtmlAnchor(); 
        anchor.Name = "fb_share";  
        anchor.Attributes.Add("share_url", HttpUtility.HtmlEncode(item.Link)); 
        anchor.HRef="http://www.facebook.com/sharer.php"; 
        anchor.InnerText = "Share";

        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            anchor.Attributes.Add("type", "button_count");
        }
        else
        {
            anchor.Attributes.Add("type", "box_count");
        }



        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Controls.Add(anchor);
   

        return div;
    }
}