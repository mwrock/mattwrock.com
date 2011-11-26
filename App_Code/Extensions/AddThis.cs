using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using BlogEngine.Core.Web.Extensions;
/* 
 * To implement a new bookmarking button, simply create a new button class, implement the abstract  
 * BtnButton class and copy the File it into the BookmarkButtons folder in your extension folder, thats it! 
 * 
 * Feature list of the version 5 of AddThis.NET:
 * 
 * - Align each button individually (top-left, top-right, buttom-left, bottom-right)
 * - Customizable order of your buttons
 * - Enable or disable each button individually
 * - Enable buttons per post by adding a HTML comment
 * - Different button styles for each button
 * - Hide each button individually in post list and show them only in the single post view
 * - Css class to attributes foreach button and button list
 * 
 * The following Bookmark and Counter Buttons are supported out of the box:
 * 
 * - Google +1
 * - Facebook Like 
 * - Facebook Share 
 * - Twitter Counter
 * - Google Buzz Counter
 * - StumbeUpon Counter
 * - Digg Counter
 * - Tweetmeme Counter 
 * - AddThis Bookmarking button
 * - DotnetShoutout Counter
 * - DotnetKicks Counter
 * - DotnetKicks.de Counter
*/


/// <summary>
/// The extensions adds bookmarking buttons to each post, based on the configuration of the buttons. 
/// </summary>
[Extension("AddThis.NET, the extensible Social Bookmarking Extension", "5", "<a href=\"http://www.mbaldinger.com\" target=\"_blank\">Mattia Baldinger</a>")]
public class AddThisDotNetv5
{
    /// <summary>
    /// Initializes the extension, registers to events
    /// </summary>
    public AddThisDotNetv5()
    {
        AdnHelper.TypeName = GetType().Name; 
        InitializeConfiguration(); 
        Post.Serving +=new EventHandler<ServingEventArgs>(Render);
        
    }

 

    /// <summary>
    /// The setting keys which are used to store the global settings. 
    /// </summary>
    private class Keys
    {
        public const string Group = "Bookmark.NET Global Settings";
        public const string ShowPostList = "HidePostList";
    }

    public AdnHelper AdnHelper
    {
        get
        {
            return new AdnHelper();
        }
    }

    /// <summary>
    /// Analyzes if it is the correct location renders the top and bottom buttons
    /// </summary>
    private void Render(object sender, ServingEventArgs e)
    {
        Post post = (Post)sender;
       
        //initialize and load the containsers
        ExtensionSettings customSettings = ExtensionManager.GetSettings(AdnHelper.TypeName, Keys.Group);
        AdnBookmarkingItem item = new AdnBookmarkingItem(post.Title, post.Description, post.AbsoluteLink);

        if (post == null) return;
        if (e.Location == ServingLocation.PostList && AdnHelper.To<bool>(Keys.ShowPostList, customSettings)) return;
  
        //rendering begins
        HtmlGenericControl topcontainer = new HtmlGenericControl("div");
        topcontainer.ID = "AdnTop";

        HtmlGenericControl bottomcontainer = new HtmlGenericControl("div");
        bottomcontainer.ID = "AdnBottom";

        string top = RenderGroups(item, topcontainer, e.Location, AdnHelper.Alignment.TopRight, AdnHelper.Alignment.TopLeft, e.Body);
        string bottom = RenderGroups(item, bottomcontainer, e.Location, AdnHelper.Alignment.BottomRight, AdnHelper.Alignment.BottomLeft, e.Body); 
        
        e.Body = top + e.Body + bottom;
    }

    /// <summary>
    /// Renders the buttons with the specified alignents in seperate groups
    /// </summary>
    private string RenderGroups(AdnBookmarkingItem item, HtmlControl sidecontrol, ServingLocation location, AdnHelper.Alignment right, AdnHelper.Alignment left, string post)
    {
        HtmlControl leftcontrol = new HtmlGenericControl("div");
        leftcontrol.Attributes.Add("class", "Adn" + left.ToString());
        leftcontrol.Attributes.Add("style", "float:left");

        HtmlControl rightcontrol = new HtmlGenericControl("div");
        rightcontrol.Attributes.Add("class", "Adn" + right.ToString());
        rightcontrol.Attributes.Add("style", "float:right");

        RenderButtons(item, leftcontrol, location, left, post);
        RenderButtons(item, rightcontrol, location, right, post);

        HtmlControl removeall = new HtmlGenericControl("div");
        removeall.Attributes.Add("style", "clear:both");

        sidecontrol.Controls.Add(leftcontrol);
        sidecontrol.Controls.Add(rightcontrol);
        sidecontrol.Controls.Add(removeall);
        
        StringBuilder sb = new StringBuilder();
        HtmlTextWriter hw = new HtmlTextWriter(new StringWriter(sb));
        sidecontrol.RenderControl(hw);

        return sb.ToString(); 
    }

    /// <summary>
    /// Renders all buttons with the specified alignment
    /// </summary>
    private void RenderButtons(AdnBookmarkingItem item, HtmlControl sidecontrol, ServingLocation location, AdnHelper.Alignment alignment, string post)
    {
        List<AdnButton> services = AdnHelper.ImplementedBookmarkingServices();
        services.Sort(); 

        foreach (AdnButton service in services)
        {
            bool render = ((service.Configuration.Enabled || post.ToLower().Contains("<!--[" + service.Configuration.Name.ToLower() + "]-->")) 
                        && service.Configuration.Alignment.Equals(alignment) //service not enabled
                        && (!service.Configuration.PostList || location != ServingLocation.PostList) //hidden in postlist
                        && (service.Configuration.ShowFeed || location != ServingLocation.Feed)); //hidden in feed

            bool staticversion = location == ServingLocation.Feed;

            if (render)
            {
                string style = string.Empty;

                if (alignment.ToString().ToLower().Contains("left"))
                {
                    style = "float:left; padding-right:10px";
                }
                else
                {
                    style = "float:right; padding-left:10px";
                }

                HtmlControl control = new HtmlGenericControl("div");
                control.Attributes.Add("style", style);
                control.Attributes.Add("class", service.Configuration.Name + "Button");
                HtmlControl buttoncontrol = service.Render(item, staticversion);

                HtmlControl script = service.RegisterScript();

                if (script != null)
                {
                    script.ID = service.Configuration.Name;
                    bool containsScript = false;

                    foreach (Control cntr in sidecontrol.Controls)
                    {
                        if (cntr.ID == service.Configuration.Name)
                        {
                            containsScript = true;
                            break;
                        }
                    }

                    if (!containsScript)
                    {
                        sidecontrol.Controls.Add(script);
                    }
                }

                if (buttoncontrol != null && staticversion)
                {
                    buttoncontrol.Attributes.Add("style", style);
                    sidecontrol.Controls.Add(buttoncontrol);
                    HtmlGenericControl a = new HtmlGenericControl("span");
                    a.InnerText = " ";
                    sidecontrol.Controls.Add(a);
                }
                else if (buttoncontrol!=null)
                {
                    control.Controls.Add(buttoncontrol);
                    sidecontrol.Controls.Add(control);

                }
            }
        }
    }

    /// <summary>
    /// Initializes and loads the configuration 
    /// </summary>
    private void InitializeConfiguration()
    {
        //initializes the global configuration
        ExtensionSettings customsettings = new ExtensionSettings(Keys.Group);

        customsettings.AddParameter(Keys.ShowPostList, "Hide all buttons in post list", 50);
        customsettings.AddValue(Keys.ShowPostList, false);
        customsettings.IsScalar = true;

        ExtensionManager.ImportSettings(AdnHelper.TypeName, customsettings);

        //initializes the service configuration
        List<AdnButton> services = AdnHelper.ImplementedBookmarkingServices();
        services.Sort();

        foreach (AdnButton service in services)
        {
           service.InitializeConfiguration();
            
        }
    }
}


/// <summary>
/// Abstract class which describes a social bookmarking 
/// button and contains all base methods and information.
/// </summary>
public abstract class AdnButton : IComparable<AdnButton>
{
    private AdnButtonConfiguration configuration;

    /// <summary>
    /// Adds additional configuration fields to the defaults
    /// </summary>
    public virtual ExtensionSettings AddServiceConfiguration(ExtensionSettings settings) { return settings;  }

    /// <summary>
    /// Renders the dynamic version of this button
    /// </summary>
    public abstract HtmlControl RenderDynamic(AdnBookmarkingItem item);

    /// <summary>
    /// Renders the static version of this button
    /// </summary>
    public virtual HtmlControl RenderStatic(AdnBookmarkingItem item) { return null;  }

    public virtual HtmlControl RegisterScript() { return null; }

    /// <summary>
    /// Initializes the social bookmarking button
    /// </summary>
    public AdnButton(string name)
    {
        configuration = new AdnButtonConfiguration(name);
    }

    /// <summary>
    /// The configuration of this service (initial and current)
    /// </summary>
    public AdnButtonConfiguration Configuration
    {
        get
        {
            return this.configuration;
        }
    }

    /// <summary>
    /// Class which provides helper methods
    /// </summary>
    public AdnHelper AdnHelper
    {
        get 
        {
            return new AdnHelper(); 
        }
    }

    /// <summary>
    /// The configuration of this service (initial and current)
    /// </summary>
    public AdnButtonDefaults Defaults
    {
        get
        {
            return configuration.Defaults;
        }
    }

    /// <summary>
    /// Compares two SocialBookmarking button to bring them in the right order
    /// </summary>
    public int CompareTo(AdnButton other)
    {
        return Comparer<int>.Default.Compare(this.Configuration.OrderNr, other.Configuration.OrderNr);
    }

    /// <summary>
    /// Creates the standard settings for this Socialbookmarking buttons
    /// </summary>
    public virtual void InitializeConfiguration()
    {
        ExtensionSettings settings = new ExtensionSettings(Configuration.Name);
        settings = AddServiceConfiguration(settings); 

        settings.AddParameter(AdnButtonConfiguration.Keys.Alignment, AdnButtonConfiguration.Keys.Alignment, 30, false, false, ParameterType.DropDown);
        settings.AddParameter(AdnButtonConfiguration.Keys.OrderNr, "Order Number", 5, false, false, ParameterType.Integer);
        settings.AddParameter(AdnButtonConfiguration.Keys.Enabled, AdnButtonConfiguration.Keys.Enabled, 30, false, false, ParameterType.Boolean);
        settings.AddParameter(AdnButtonConfiguration.Keys.PostList, "Hide in post list", 30, false, false, ParameterType.Boolean);
        string[] alignments = new string[]{ AdnHelper.Alignment.BottomLeft.ToString(), AdnHelper.Alignment.BottomRight.ToString(), AdnHelper.Alignment.TopLeft.ToString(), AdnHelper.Alignment.TopRight.ToString()};

        settings.AddValue(AdnButtonConfiguration.Keys.Alignment, alignments, Configuration.Alignment.ToString());
        settings.AddValue(AdnButtonConfiguration.Keys.OrderNr, Configuration.OrderNr);
        settings.AddValue(AdnButtonConfiguration.Keys.Enabled, Configuration.Enabled);
        settings.AddValue(AdnButtonConfiguration.Keys.PostList, Configuration.PostList);

        settings.IsScalar = true;

        if (Defaults.StaticVersion)
        {
            settings.AddParameter(AdnButtonConfiguration.Keys.ShowFeed, "Show in feed", 30, false, false, ParameterType.Boolean);
            settings.AddValue(AdnButtonConfiguration.Keys.ShowFeed, false);
        }
        
        ExtensionManager.ImportSettings(AdnHelper.TypeName, settings);
        ExtensionManager.InitSettings(AdnHelper.TypeName, settings);
    }

    /// <summary>
    /// Calls the correct render method
    /// </summary>
    public HtmlControl Render(AdnBookmarkingItem item, bool staticversion)
    {
        if (staticversion)
        {
            return this.RenderStatic(item);
        }
        else
        {
            return this.RenderDynamic(item);
        }
    }

}


/// <summary>
/// Represents the configuration of the button. It contains
/// the current and also the initial values; 
/// </summary>
public class AdnButtonConfiguration
{
    /// <summary>
    /// Initializes the bookmarking item with the default values
    /// </summary>
    public AdnButtonConfiguration(string name)
    {
        this.name = name;
        this.defaults = new AdnButtonDefaults(); 
    }

    /// <summary>
    /// The setting keys which are used to store the list settings.
    /// </summary>
    public class Keys
    {
        public const string Alignment = "Alignment";
        public const string OrderNr = "OrderNumber";
        public const string ShowFeed = "ShowFeed";
        public const string Enabled = "Enabled";
        public const string PostList = "HidePostList";
    }

    private string name;

    /// <summary>
    /// Returns the name of the service
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
    }
    public AdnHelper AdnHelper
    {
        get
        {
            return new AdnHelper();
        }
    }
    /// <summary>
    /// Should the button also be shown in feeds
    /// </summary>
    public bool ShowFeed
    {
        get
        {
            return AdnHelper.To<bool>(Keys.ShowFeed, ExtensionSettings);
        }
    }

    /// <summary>
    /// Returns the extension settings for this button service
    /// </summary>
    public ExtensionSettings ExtensionSettings
    {
        get
        {
            return ExtensionManager.GetSettings(AdnHelper.TypeName, Name);
            
        }
    }

    private AdnButtonDefaults defaults;

    /// <summary>
    /// Returns the default value collection
    /// </summary>
    public AdnButtonDefaults Defaults
    {
        get
        {
            return defaults;
        }
    }

    /// <summary>
    /// Alignment of this button
    /// </summary>
    public AdnHelper.Alignment Alignment
    {
        get
        {
            if (ExtensionSettings != null)
            {
                return (AdnHelper.Alignment)Enum.Parse(typeof(AdnHelper.Alignment), AdnHelper.To<string>(Keys.Alignment, ExtensionSettings));
            }
            else
            {
                return defaults.Alignment;
            }
        }
    }

    /// <summary>
    /// Should be button in common be shown
    /// </summary>
    public bool Enabled
    {
        get
        {
            if (ExtensionSettings != null)
            {
                return AdnHelper.To<bool>(Keys.Enabled, ExtensionSettings);
            }
            else
            {
                return defaults.Enabled;
            }
        }
    }

    /// <summary>
    /// Order number of the button
    /// </summary>
    public int OrderNr
    {
        get
        {
            if (ExtensionSettings != null && !string.IsNullOrEmpty(ExtensionSettings.GetSingleValue(Keys.OrderNr)))
            {
                return AdnHelper.To<int>(Keys.OrderNr, ExtensionSettings);
            }
            else
            {
                return defaults.OrderNr;
            }
        }
    }

    public bool PostList
    {
        get
        {
            if (ExtensionSettings != null && !string.IsNullOrEmpty(ExtensionSettings.GetSingleValue(Keys.PostList)))
            {
                return AdnHelper.To<bool>(Keys.PostList, ExtensionSettings);
            }
            else
            {
                return defaults.PostList;
            }
        }
    }
}


/// <summary>
/// Represents the default values of a button and also
/// some internal configuration values. 
/// </summary>
public class AdnButtonDefaults
{
    /// <summary>
    /// Initializes the default values
    /// </summary>
    public AdnButtonDefaults()
    {
        this.alignment = AdnHelper.Alignment.BottomLeft;
        this.enabled = true;
        this.orderNr = 99;
        this.staticVersion = false;
        this.postlist = false; 
    }

    private bool staticVersion;

    /// <summary>
    /// Indicates if the button provides a static version (for the feed)
    /// </summary>
    public bool StaticVersion
    {
        get
        {
            return staticVersion;
        }
        set
        {
            staticVersion = value;
        }
    }

    private AdnHelper.Alignment alignment;

    /// <summary>
    /// Returns the default alignment
    /// </summary>
    public AdnHelper.Alignment Alignment
    {
        get
        {
            return alignment; 
        }
        set
        {
            alignment = value;
        }
    }

    private int orderNr;

    /// <summary>
    /// Returns the default order number
    /// </summary>
    public int OrderNr
    {
        get
        {
            return orderNr;
        }
        set
        {
            orderNr = value;
        }
    }

    private bool enabled;

    /// <summary>
    /// Returns if this service should be enabled by default
    /// </summary>
    public bool Enabled
    {
        get
        {
            return enabled; 
        }
        set
        {
            enabled = value;
        }
    }

    private bool postlist; 

    public bool PostList
    {
        get
        {
            return postlist;
        }
        set
        {
            postlist = value;
        }
    }
}


/// <summary>
/// Contains all information which a bookmarking button needs
/// to render the button with all its information. 
/// </summary>
public class AdnBookmarkingItem
{ 
    /// <summary>
    /// Initializes the bookmarking item with the correct values
    /// </summary>
    public AdnBookmarkingItem(string title, string description, Uri link)
    {
        this.title = title;
        this.description = description;
        this.link = link.ToString();
    }

    private string link;

    /// <summary>
    /// The title of the post
    /// </summary>
    public string Title
    {
        get
        {
            return title;
        }
    }

    private string description;

    /// <summary>
    /// Description of the post
    /// </summary>
    public string Description
    {
        get
        {
            return description;
        }
    }

    private string title;

    /// <summary>
    /// Link which points to the post
    /// </summary>
    public string Link
    {
        get
        {
            return link;
        }
    }
}


/// <summary>
/// contains the static methods which are used
/// by several classes of AddThis.NET. 
/// </summary>
public class AdnHelper
{
    /// <summary>
    /// Iitialized on startup, contains the tech. name of the extension
    /// </summary>
    public static string TypeName;

    /// <summary>
    /// Converts the value from the extension settings into the specified type
    /// </summary>
    public T To<T>(string key, ExtensionSettings settings)
    {   
        try
        {
            if (!string.IsNullOrEmpty(settings.GetSingleValue(key)))
            {
                object value = settings.GetSingleValue(key).Trim();
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
        catch(Exception e)
        {
            Utils.Log(string.Format("Error AddThis.Net - Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace));
        }

        return default(T);
    }

    /// <summary>
    /// Possibles alignements of the bookmarking buttons. 
    /// </summary>
    public enum Alignment
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight
    }

    /// <summary>
    /// Returns a list of instances of bookmarking buttons which are implemented
    /// </summary>
    public List<AdnButton> ImplementedBookmarkingServices()
    {
        List<AdnButton> services = new List<AdnButton>();

        foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(AdnButton)))
            {
                services.Add((AdnButton)Activator.CreateInstance(type));
            }
        }

        return services;
    }
}
