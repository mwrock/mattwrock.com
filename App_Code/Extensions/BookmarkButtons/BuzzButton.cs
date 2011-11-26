using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by Google Buzz http://www.google.com/buzz
/// Button and function description can be found at http://www.google.com/buzz/api/admin/configPostWidget
/// </summary>
public class BuzzButton : AdnButton
{
    public BuzzButton() : base("Buzz") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Buzz</span> is a counter and publishing service provided by Google (http://www.google.com/buzz). " +
                        "It can be displayed in a small and a large version, a static version with counter isn't supported by Google." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view."; 
    
        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");

        return settings; 
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        HtmlAnchor anchor = new HtmlAnchor(); 
        anchor.Attributes.Add("class", "google-buzz-button"); 
        anchor.Attributes.Add("data-url", HttpUtility.HtmlEncode(item.Link)); 
        anchor.HRef="http://www.google.com/buzz/post"; 
        anchor.Title = "Google Buzz"; 

        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            anchor.Attributes.Add("data-button-style", "small-count"); 
        }
        else
        {
            anchor.Attributes.Add("data-button-style", "normal-count"); 
        }

        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("src", "http://www.google.com/buzz/api/button.js");
        script.Attributes.Add("type", "text/javascript");

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Controls.Add(anchor);
        div.Controls.Add(script);

        return div;
    }
}