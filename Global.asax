<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BlogEngine.Core" %>
<%@ Import Namespace="App_Code.Controls" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script RunAt="server">
  
  /// <summary>
  /// Application Error handler
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
    void Application_Error(object sender, EventArgs e)
    {
        HttpContext context = ((HttpApplication)sender).Context;
        Exception ex = context.Server.GetLastError();
        
        if (ex == null || !(ex is HttpException) || (ex as HttpException).GetHttpCode() == 404)
        {
            return;
        }
        var sb = new StringBuilder();

        try
        {
            sb.AppendLine("Url : " + context.Request.Url);
            sb.AppendLine("Raw Url : " + context.Request.RawUrl);

            while (ex != null)
            {
                sb.AppendLine("Message : " + ex.Message);
                sb.AppendLine("Source : " + ex.Source);
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine("TargetSite : " + ex.TargetSite);
                ex = ex.InnerException;
            }
        }
        catch (Exception ex2)
        {
            sb.AppendLine("Error logging error : " + ex2.Message);
        }

        if (BlogSettings.Instance.EnableErrorLogging)
        {
            Utils.Log(sb.ToString());
        }
        context.Items["LastErrorDetails"] = sb.ToString();
        context.Response.StatusCode = 500;
        
        // Custom errors section defined in the Web.config, will rewrite (not redirect)
        // this 500 error request to error.aspx.
    }

    void Application_Start(object sender, EventArgs e)
    {
        // intentionally not using Application_Start.  instead application
        // start code is below in FirstRequestInitialization.  this is to
        // workaround IIS7 integrated mode issue where HttpContext.Request
        // is not available during Application_Start.
    }

    void Application_BeginRequest(object source, EventArgs e)
    {
        HttpApplication app = (HttpApplication)source;
        HttpContext context = app.Context;
        
        // Attempt to perform first request initialization
        FirstRequestInitialization.Initialize(context);
    }
        
    /// <summary>
    /// Sets the culture based on the language selection in the settings.
    /// </summary>
    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        var culture = BlogSettings.Instance.Culture;
        if (!string.IsNullOrEmpty(culture) && !culture.Equals("Auto"))
        {
            CultureInfo defaultCulture = Utils.GetDefaultCulture();
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
            Thread.CurrentThread.CurrentCulture = defaultCulture;
        }
    }

    private class FirstRequestInitialization
    {
        private static bool _initializedAlready = false;
        private readonly static object _SyncRoot = new Object();

        // Initialize only on the first request
        public static void Initialize(HttpContext context)
        {
            if (_initializedAlready) { return; }

            lock (_SyncRoot)
            {
                if (_initializedAlready) { return; }

                WidgetZone.PreloadWidgetsAsync("be_WIDGET_ZONE"); 
                Utils.LoadExtensions();

                BundleConfig.RegisterBundles(BundleTable.Bundles);
                
                _initializedAlready = true;
            }
        }
    }
    
</script>