namespace Admin.ExtensionManager
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using BlogEngine.Core;
    using BlogEngine.Core.Web.Extensions;

    /// <summary>
    /// The user_controls_xdashboard_ default.
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);
            UserControl uc;

            switch (this.Request.QueryString["ctrl"])
            {
                case "params":
                    var extname = this.Request.QueryString["ext"];

                    foreach (var setting in from x in ExtensionManager.Extensions
                                            where x.Name == extname
                                            from setting in x.Settings
                                            where !string.IsNullOrEmpty(setting.Name) && !setting.Hidden
                                            select setting)
                    {
                        uc = (UserControl)this.Page.LoadControl("Settings.ascx");
                        uc.ID = setting.Name;
                        this.ucPlaceHolder.Controls.Add(uc);
                    }

                    break;
                case "editor":
                    uc = (UserControl)this.Page.LoadControl("Editor.ascx");
                    this.ucPlaceHolder.Controls.Add(uc);
                    break;
                default:
                    uc = (UserControl)this.Page.LoadControl("Extensions.ascx");
                    this.ucPlaceHolder.Controls.Add(uc);
                    break;
            }

            base.OnInit(e);
        }
    }
}