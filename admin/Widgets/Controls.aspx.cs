// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The admin pages controls.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Admin.Pages
{
    using System;
    using System.Globalization;
    using BlogEngine.Core;
    using App_Code;
    using Resources;

    /// <summary>
    /// The admin pages controls.
    /// </summary>
    public partial class Controls : System.Web.UI.Page
    {
        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            this.BindSettings();

            this.btnSave.Click += this.BtnSaveClick;
            this.btnSave.Text = string.Format("{0} {1}", labels.save, labels.settings);
            this.Page.Title = labels.controls;

            base.OnInit(e);
        }

        /// <summary>
        /// Binds the settings.
        /// </summary>
        private void BindSettings()
        {
            this.txtNumberOfPosts.Text = BlogSettings.Instance.NumberOfRecentPosts.ToString();
            this.cbDisplayComments.Checked = BlogSettings.Instance.DisplayCommentsOnRecentPosts;
            this.cbDisplayRating.Checked = BlogSettings.Instance.DisplayRatingsOnRecentPosts;

            this.txtNumberOfComments.Text = BlogSettings.Instance.NumberOfRecentComments.ToString();

            this.txtSearchButtonText.Text = BlogSettings.Instance.SearchButtonText;
            this.txtCommentLabelText.Text = BlogSettings.Instance.SearchCommentLabelText;
            this.txtDefaultSearchText.Text = BlogSettings.Instance.SearchDefaultText;
            this.cbEnableCommentSearch.Checked = BlogSettings.Instance.EnableCommentSearch;

            this.txtThankMessage.Text = BlogSettings.Instance.ContactThankMessage;
            this.txtFormMessage.Text = BlogSettings.Instance.ContactFormMessage;
            this.cbEnableAttachments.Checked = BlogSettings.Instance.EnableContactAttachments;
            this.cbEnableRecaptcha.Checked = BlogSettings.Instance.EnableRecaptchaOnContactForm;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            BlogSettings.Instance.NumberOfRecentPosts = int.Parse(this.txtNumberOfPosts.Text, CultureInfo.InvariantCulture);
            BlogSettings.Instance.DisplayCommentsOnRecentPosts = this.cbDisplayComments.Checked;
            BlogSettings.Instance.DisplayRatingsOnRecentPosts = this.cbDisplayRating.Checked;

            BlogSettings.Instance.NumberOfRecentComments = int.Parse(
                this.txtNumberOfComments.Text, CultureInfo.InvariantCulture);

            BlogSettings.Instance.SearchButtonText = this.txtSearchButtonText.Text;
            BlogSettings.Instance.SearchCommentLabelText = this.txtCommentLabelText.Text;
            BlogSettings.Instance.SearchDefaultText = this.txtDefaultSearchText.Text;
            BlogSettings.Instance.EnableCommentSearch = this.cbEnableCommentSearch.Checked;

            BlogSettings.Instance.ContactFormMessage = this.txtFormMessage.Text;
            BlogSettings.Instance.ContactThankMessage = this.txtThankMessage.Text;
            BlogSettings.Instance.EnableContactAttachments = this.cbEnableAttachments.Checked;
            BlogSettings.Instance.EnableRecaptchaOnContactForm = this.cbEnableRecaptcha.Checked;

            BlogSettings.Instance.Save();
        }

        #endregion
    }
}