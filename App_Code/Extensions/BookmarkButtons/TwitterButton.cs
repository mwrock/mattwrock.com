using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;
/// <summary>
/// Counter button provided by Twitter http://www.twitter.com
/// Button and function description can be found at http://twitter.com/goodies/tweetbutton
/// </summary>
public class TwitterButton : AdnButton
{
    public TwitterButton() : base("Twitter") 
    {
        Defaults.Alignment = AdnHelper.Alignment.TopRight;
        Defaults.OrderNr = 98;
    }

    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">Twitter</span> (http://www.twitter.com/) provides counter and publishing service. " +
                        "The button can be displayed in a small and a large version, a static version with counter isn't supported by Twitter. <br/><br/>" +
                        "If you enter a username is entered this user will be @ mentioned in the suggested Tweet (leave &lt;anonymous&gt; if you don't want to mention someone)."+
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view.";
        settings.AddParameter("Twitter Name", "Twitter Name", 50, false, false, ParameterType.String);
        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        
        settings.AddValue("Style", new string[] { "compact", "large" }, "compact");
        settings.AddValue("Twitter Name", "<anonymous>");

        return settings;
    }

    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        /*
var count = AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact") ? "horizontal" : "vertical";
var twitterUrl =
    string.Format("http://platform.twitter.com/widgets/tweet_button.html?url={0}&text={1}&count={2}",
                  HttpUtility.HtmlEncode(item.Link), HttpUtility.HtmlEncode(item.Title), count);
var twitterid = AdnHelper.To<string>("Twitter Name", Configuration.ExtensionSettings);
if (!string.IsNullOrEmpty(twitterid) && !twitterid.Equals("<anonymous>"))
    twitterUrl += string.Format("&via={0}", HttpUtility.HtmlEncode(twitterid));
HtmlGenericControl frame = new HtmlGenericControl("iframe");
frame.Attributes.Add("class", "twitter-share-button");
frame.Attributes.Add("src", twitterUrl);
frame.Attributes.Add("style", "width:130px; height:20px;");
frame.Attributes.Add("allowtransparency", "true");
frame.Attributes.Add("frameborder", "0");
frame.Attributes.Add("scrolling", "no");

HtmlGenericControl div = new HtmlGenericControl("div");
div.Controls.Add(frame);
*/

        HtmlAnchor anchor = new HtmlAnchor(); 
        anchor.Attributes.Add("class", "twitter-share-button");
        anchor.Attributes.Add("data-url", HttpUtility.HtmlEncode(item.Link));
        anchor.Attributes.Add("data-text", HttpUtility.HtmlEncode(item.Title)); 
        anchor.HRef="http://twitter.com/share"; 
        anchor.InnerText = "Tweet";


        if (AdnHelper.To<string>("Style", Configuration.ExtensionSettings).Equals("compact"))
        {
            anchor.Attributes.Add("data-count", "horizontal"); 
        }
        else
        {
            anchor.Attributes.Add("data-count", "vertical"); 
        }

        string twitterid = AdnHelper.To<string>("Twitter Name", Configuration.ExtensionSettings);

        if (!string.IsNullOrEmpty(twitterid) && !twitterid.Equals("<anonymous>"))
        {
            anchor.Attributes.Add("data-via", HttpUtility.HtmlEncode(twitterid)); 

        }

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Controls.Add(anchor);

        return div;
    }
}