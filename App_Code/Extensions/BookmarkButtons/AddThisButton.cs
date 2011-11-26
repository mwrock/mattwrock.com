using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Bookmarking Button for the Addthis Service, http://www.addthis.com 
/// It provides a lot bookmarking services but no counters. 
/// </summary>
public class AddThisButton : AdnButton
{
    public const string AddThisId = "AddThisID";
    public const string Buttons = "Buttons";


    public AddThisButton() : base("AddThis")
    {
        Defaults.Alignment = AdnHelper.Alignment.BottomLeft;
        Defaults.StaticVersion = true;
        Defaults.OrderNr = 0; 
    }

    /// <summary>
    /// Adds the custom configuration settings for AddThis
    /// </summary>
    public override ExtensionSettings AddServiceConfiguration(ExtensionSettings settings)
    {
        settings.Help = "<span  style=\"font-weight: bold;\">AddThis</span> (http://www.addthis.com) is a bookmarking button provider. " +
                "The button can only be displayed in various versions. It can be used in RSS feeds. <br/><br/>" +
                "With the input box 'Small Button List' you can customize the buttons which should be shown next to the main AddThis button. "+
                "You will find a complete list at of possible buttons and their keys at http://www.addthis.com/services/list. "+
                "Per default the preferred (if there are no preferred, the most famous ones) buttons will be used. To set a fix list, enter the keys from the AddThis service list. <br/><br/>"+
                "If you enter a AddThis Id the clicks will be tracked (default is &lt;anonymous&gt;), so you can analyze it. You can create a AddThis user account at http://www.addthis.com/."+
                        "<br/><br/>If you want to add the button only in some posts, simply add the HTML comment &lt;!--[" + Configuration.Name + "]--&gt; to the your post in the HTML view. <br/><br/>" +
                "<span  style=\"font-weight: bold;\">Know Issue with Safari and Chrome:</span> When a style other than plus or counter is used, the first item of the small button list will be displayed in a seperate row." ;

        settings.AddParameter(AddThisId, "AddThis ID", 50, false, false, ParameterType.String);
        settings.AddParameter(Buttons, "Small Button List (comma separated)", 300, false, false, ParameterType.String);
        settings.AddParameter("Style", "Style", 50, false, false, ParameterType.DropDown);
        settings.AddParameter("Static Style", "Static Style", 50, false, false, ParameterType.DropDown);

        settings.AddValue(AddThisId, "<anonymous>");
        settings.AddValue("Style", new string[] { "plus", "counter", "share", "bookmark", "share large" }, "plus");
        settings.AddValue("Static Style", new string[] { "plus", "share", "bookmark", "share large" }, "share");
        settings.AddValue(Buttons, "preferred_1, preferred_2, preferred_3, preferred_4, preferred_5");

        return settings; 
    }

    /// <summary>
    /// Renders the dynmic version for your page
    /// </summary>
    public override HtmlControl RenderDynamic(AdnBookmarkingItem item)
    {
        ExtensionSettings settings = Configuration.ExtensionSettings;
        string id = AdnHelper.To<string>(AddThisId, settings).Equals("<anonymous>") ? "xa-4cb7e4d77c1aa8bd" : AdnHelper.To<string>(AddThisId, settings);
        string style = AdnHelper.To<string>("Style", settings);

        HtmlGenericControl script = new HtmlGenericControl("script");
        script.Attributes.Add("type", "text/javascript");
        script.Attributes.Add("src", string.Format("http://s7.addthis.com/js/250/addthis_widget.js#username={0}", id));

        HtmlGenericControl panel = new HtmlGenericControl("div");


        if (style.Equals("share") || style.Equals("bookmark") || style.Equals("share large"))
        {
            HtmlAnchor addthisButton = new HtmlAnchor();
            addthisButton.Attributes.Add("class", "addthis_button");
            addthisButton.HRef = "http://www.addthis.com/bookmark.php?v=250";
            addthisButton.HRef += "&username=" + id;

            HtmlImage image = new HtmlImage();
            image.Border = 0;
            addthisButton.Controls.Add(image);
            
            if (style.Equals("share"))
            {
                image.Src = "http://s7.addthis.com/static/btn/v2/sm-share-en.gif";
            }
            
            if (style.Equals("bookmark"))
            {
                image.Src = "http://s7.addthis.com/static/btn/v2/sm-bookmark-en.gif";
            }

            if (style.Equals("share large"))
            {
                image.Src = "http://s7.addthis.com/static/btn/v2/lg-share-en.gif";
            }


            HtmlGenericControl left = new HtmlGenericControl("div");
            addthisButton.Attributes.Add("addthis:title", HttpUtility.HtmlEncode(item.Title));
            addthisButton.Attributes.Add("addthis:url", HttpUtility.HtmlEncode(item.Link));
            addthisButton.Attributes.Add("addthis:description", HttpUtility.HtmlEncode(item.Description));

            left.Attributes.Add("style", "float:left");
            left.Controls.Add(addthisButton);
            left.Controls.Add(script);

            panel.Controls.Add(left);
        }
        

        HtmlGenericControl container = new HtmlGenericControl("div");
        container.Attributes.Add("class", "addthis_toolbox addthis_default_style");
        container.Attributes.Add("addthis:title", HttpUtility.HtmlEncode(item.Title));
        container.Attributes.Add("addthis:url", HttpUtility.HtmlEncode(item.Link));
        container.Attributes.Add("addthis:description", HttpUtility.HtmlEncode(item.Description));

        if (style.Equals("plus"))
        {
            HtmlAnchor servicelink = new HtmlAnchor();
            servicelink.Attributes.Add("class", "addthis_button_compact");
            container.Controls.Add(servicelink);
        }

        if (style.Equals("counter"))
        {
            HtmlAnchor servicelink = new HtmlAnchor();
            servicelink.Attributes.Add("class", "addthis_counter addthis_pill_style");
            container.Controls.Add(servicelink);
        }
        
        foreach (string service in AdnHelper.To<string>(Buttons, settings).Split(','))
        {
            HtmlAnchor servicelink = new HtmlAnchor();
            servicelink.Attributes.Add("class", string.Format("addthis_button_{0}", service.ToLower().Trim()));
            container.Controls.Add(servicelink);
        }


        HtmlGenericControl right = new HtmlGenericControl("div");
        right.Attributes.Add("style", "float:left");
        right.Controls.Add(container);
        right.Controls.Add(script);

        
        panel.Controls.Add(right); 

        return panel;
    }

    public override HtmlControl RenderStatic(AdnBookmarkingItem item)
    {
        ExtensionSettings settings = Configuration.ExtensionSettings;
        string id = AdnHelper.To<string>(AddThisId, settings).Equals("<anonymous>") ? "4a00fc5d6a261f84" : AdnHelper.To<string>(AddThisId, settings);
        string style = AdnHelper.To<string>("Static Style", settings);


        HtmlImage image = new HtmlImage();
        image.Border = 0;
        image.Alt = Configuration.Name; 

        if (style.Equals("share"))
        {
            image.Src = "http://s7.addthis.com/static/btn/v2/sm-share-en.gif";
        }

        else if (style.Equals("bookmark"))
        {
            image.Src = "http://s7.addthis.com/static/btn/v2/sm-bookmark-en.gif";
        }

        else if (style.Equals("share large"))
        {
            image.Src = "http://s7.addthis.com/static/btn/v2/lg-share-en.gif";
        }
        else
        {
            image.Src = "http://s7.addthis.com/static/btn/v2/sm-plus.gif";
        }


        HtmlAnchor anchor = new HtmlAnchor();
        anchor.HRef = "http://www.addthis.com/bookmark.php?v=250";
        anchor.HRef += "&username=" + id;
        anchor.HRef += "&url=" + HttpUtility.UrlEncode(item.Link);
        anchor.HRef += "&title=" + HttpUtility.UrlEncode(item.Title);
        anchor.HRef += "&description=" + HttpUtility.UrlEncode(item.Description);
        anchor.Target = "parent";
        anchor.Controls.Add(image);

        return anchor;
    }
}
