namespace admin.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Globalization;
    using System.Web.Services;
    using System.Threading;
    using System.Web.Security;

    using Resources;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;
    using BlogEngine.Core.API.BlogML;
    using App_Code;

    using Page = System.Web.UI.Page;

    public partial class HeadTrack : Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            this.BindSettings();

            this.Page.MaintainScrollPositionOnPostBack = true;
            this.Page.Title = labels.settings;
            base.OnInit(e);
        }

        /// <summary>
        /// The bind settings.
        /// </summary>
        private void BindSettings()
        {
            // -----------------------------------------------------------------------
            // HTML header section
            // -----------------------------------------------------------------------
            this.txtHtmlHeader.Text = BlogSettings.Instance.HtmlHeader;

            // -----------------------------------------------------------------------
            // Visitor tracking settings
            // -----------------------------------------------------------------------
            this.txtTrackingScript.Text = BlogSettings.Instance.TrackingScript;
        }

        /// <summary>
        /// Save settings
        /// </summary>
        /// <param name="hdr">Header script</param>
        /// <param name="ftr">Tracking script</param>
        /// <returns>Json response</returns>
        [WebMethod]
        public static JsonResponse Save(string hdr, string ftr)
        {
            JsonResponse response = new JsonResponse();
            response.Success = false;

            if (!WebUtils.CheckRightsForAdminSettingsPage(true))
            {
                response.Message = "Not authorized";
                return response;
            }

            try
            {
                BlogSettings.Instance.HtmlHeader = hdr;
                BlogSettings.Instance.TrackingScript = ftr;
                BlogSettings.Instance.Save();
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("admin.Settings.HeadTrack.Save(): {0}", ex.Message));
                response.Message = string.Format("Could not save settings: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Settings saved";
            return response;
        }
    }
}