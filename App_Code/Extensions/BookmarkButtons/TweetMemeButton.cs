using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Counter button provided by TweetMeme http://www.tweetmeme.com
/// Button and function description can be found at http://help.tweetmeme.com/2009/04/06/tweetmeme-button/
/// </summary>
public class TweetMemeButton : AdnButton
{
    public TweetMemeButton() : base("TweetMeme")
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
        Defaults.OrderNr = 98;
        Defaults.Enabled = false; 
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Tweetmeme</span> (http://www.tweetmeme.com/) is a Twitter counter and publishing service. " +
        "The button can be displayed in a small and a large version, a static version with counter isn't supported.<br/><br/>" +
                        "If you enter a username is entered this user will be @ mentioned in the suggested Tweet (leave &lt;anonymous&gt; if you don't want to mention someone)." +
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";

        settings.AddParameter("Twitter Name", "Twitter Name", 50, false, false, ParameterType.String);
        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);

        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");
        settings.AddValue("Twitter Name", "<anonymous>");

        return settings;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        ExtensionSettings settings = Configuration.ExtensionSettings;
        
        string source = string.Empty;
        string sourcevalue = AdnHelper.To<string>("Twitter Name", Configuration.ExtensionSettings);

        if (!string.IsNullOrEmpty(sourcevalue) && !source.Equals("<anonymous>"))
        {
            source = "tweetmeme_source='" + sourcevalue + "';";
        }

        string style = string.Empty;
        string stylevalue = AdnHelper.To<string>("Style", Configuration.ExtensionSettings);

        if (stylevalue.Equals("compact"))
        {
            style = "tweetmeme_style='" + stylevalue + "';";
        }

        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("type", "text/javascript");
        script.InnerText = "tweetmeme_url = '" + HttpUtility.HtmlEncode(item.Link) + "';"+ style + source;

        HtmlGenericControl scriptimport = new HtmlGenericControl("script");
        scriptimport.Attributes.Add("type", "text/javascript");
        scriptimport.Attributes.Add("src", "http://tweetmeme.com/i/scripts/button.js");

        HtmlGenericControl container = new HtmlGenericControl("div");
        container.Controls.Add(script);
        container.Controls.Add(scriptimport);

        return container;
    }
}