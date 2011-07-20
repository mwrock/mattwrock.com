namespace Admin.ExtensionManager
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;

    using BlogEngine.Core;

    using Resources;

    /// <summary>
    /// The user_controls_xmanager_ source editor.
    /// </summary>
    public partial class UserControlsXmanagerSourceEditor : UserControl
    {
        /// <summary>
        ///     The error msg.
        /// </summary>
        protected static string ErrorMsg = string.Empty;

        /// <summary>
        ///     The extension name.
        /// </summary>
        protected static string ExtensionName = string.Empty;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);

            this.btnSave.Enabled = true;
            this.btnSave.OnClientClick = string.Format("return confirm('{0}');", labels.siteUnavailableConfirm);
            ExtensionName = Path.GetFileName(this.Request.QueryString["ext"]);
            this.txtEditor.Text = this.ReadFile(GetExtFileName());

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (WriteFile(GetExtFileName(), this.txtEditor.Text))
            {
                this.Response.Redirect("default.aspx");
            }
            else
            {
                this.txtEditor.Text = ErrorMsg;
                this.txtEditor.ForeColor = Color.Red;
                this.btnSave.Enabled = false;
            }
        }

        /// <summary>
        /// Returns extension file name
        /// </summary>
        /// <returns>
        /// File name
        /// </returns>
        protected static string GetExtFileName()
        {
            var fileName = HttpContext.Current.Request.PhysicalApplicationPath;
            var codeAssemblies = Utils.CodeAssemblies();
            foreach (Assembly a in codeAssemblies)
            {
                var types = a.GetTypes();
                foreach (var type in types.Where(type => type.Name == ExtensionName))
                {
                    var assemblyName = type.Assembly.FullName.Split(".".ToCharArray())[0];
                    assemblyName = assemblyName.Replace("App_SubCode_", "App_Code\\");
                    var fileExt = assemblyName.Contains("VB_Code") ? ".vb" : ".cs";
                    fileName += Path.Combine(Path.Combine(assemblyName, "Extensions"), ExtensionName + fileExt);
                }
            }

            return fileName;
        }

        /// <summary>
        /// Read extension source file from disk
        /// </summary>
        /// <param name="fileName">
        /// File Name
        /// </param>
        /// <returns>
        /// Source file text
        /// </returns>
        private string ReadFile(string fileName)
        {
            var val = string.Format("Source for [{0}] not found", fileName);
            try
            {
                val = File.ReadAllText(fileName);
            }
            catch (Exception)
            {
                this.btnSave.Enabled = false;
            }

            return val;
        }

        /// <summary>
        /// Writes file to the disk
        /// </summary>
        /// <param name="fileName">
        /// File name
        /// </param>
        /// <param name="val">
        /// File source (text)
        /// </param>
        /// <returns>
        /// True if successful
        /// </returns>
        private static bool WriteFile(string fileName, string val)
        {
            try
            {
                using (var sw = File.CreateText(fileName))
                {
                    sw.Write(val);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }

            return true;
        }
    }
}