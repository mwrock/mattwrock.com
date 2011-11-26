using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by StumbeUpon http://www.stumbleupon.com
/// Button and function description can be found at http://www.stumbleupon.com/badges/landing/
/// </summary>
public class StumbeUponButton : AdnButton
{
    public StumbeUponButton() : base("StumbeUpon") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">StumbleUpon</span> (http://www.stumbleupon.com) is a rapidly growing social bookmarking and counter service. " +
               "The button can be displayed in a small and a large version, a static version with counter isn't supported by StumbleUpon." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");

        return settings;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {

        string src = "http://www.stumbleupon.com/hostedbadge.php";
        src+="?r="+HttpUtility.HtmlEncode(item.Link); 
     
        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            src+="&s=1"; 
        }
        else
        {
            src+="&s=5";
        }

        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("src", src);
        script.Attributes.Add("type", "text/javascript");

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Controls.Add(script);

        return div;
    }

    /// <summary>
    /// There is no static version
    /// </summary>
    public override HtmlControl RenderStatic(AdnBookmarkingItem item)
    {
        return null;
    }
}