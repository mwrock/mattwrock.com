using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by Google Buzz http://www.google.com/buzz
/// Button and function description can be found at http://www.google.com/buzz/api/admin/configPostWidget
/// </summary>
public class PlusOneButton : AdnButton
{
    public PlusOneButton() : base("PlusOne") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Google +1</span> is a counter and publishing service provided by Google (http://www.google.com/+1/button). " +
                        "It can be displayed in a medium and a large version, a static version with counter isn't supported by Google." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view."; 
    
        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddValue("Style", new string[] { "medium", "large" }, "medium");

        return settings; 
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        HtmlGenericControl anchor = new HtmlGenericControl("g:plusone"); 
        anchor.Attributes.Add("href", HttpUtility.HtmlEncode(item.Link));
  
        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("medium"))
        {
            anchor.Attributes.Add("size", "medium");
        }
		else if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("large"))
        {
            anchor.Attributes.Add("size", "tall"); 
        }



        HtmlGenericControl div = new HtmlGenericControl("div");
       
		div.Controls.Add(anchor);

        return div;
    }
}