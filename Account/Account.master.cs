namespace Account
{
    using System;
    using System.Web.UI;

    using BlogEngine.Core;

    /// <summary>
    /// The account_ account.
    /// </summary>
    public partial class AccountMasterPage : MasterPage
    {
        #region Public Methods

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="msg">
        /// The message.
        /// </param>
        public void SetStatus(string status, string msg)
        {
            this.AdminStatus.Attributes.Clear();
            this.AdminStatus.Attributes.Add("class", status);
            this.AdminStatus.InnerHtml =
                string.Format(
                    "{0}<a href=\"javascript:HideStatus()\" style=\"width:20px;float:right\">X</a>", 
                    this.Server.HtmlEncode(msg));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.AddFolderJavaScripts(this.Page, "Scripts", true);
            Utils.AddJavaScriptInclude(this.Page, string.Format("{0}Account/Account.js", Utils.RelativeWebRoot), false, false);
        }

        #endregion
    }
}