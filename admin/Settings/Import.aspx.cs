namespace admin.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using BlogEngine.Core;
    using BlogEngine.Core.API.BlogML;
    using App_Code;

    public partial class Import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);
        }

        /// <summary>
        /// Handles the Click event of the btnBlogMLImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnBlogMlImportClick(object sender, EventArgs e)
        {
            var fileName = this.txtUploadFile.FileName;

            if (string.IsNullOrEmpty(fileName))
            {
                //this.Master.SetStatus("warning", "File name is required");
            }
            else
            {
                var reader = new BlogReader();

                var stm = this.txtUploadFile.FileContent;
                var rdr = new StreamReader(stm);
                reader.XmlData = rdr.ReadToEnd();

                reader.Import();

                //this.Master.SetStatus(reader.Import() ? "success" : "warning", reader.Message);
            }
        }
    }
}