namespace admin.Settings
{
    #region Using

    using System;
    using System.Collections.Specialized;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using BlogEngine.Core.Providers;
    using App_Code;

    #endregion

    public partial class PingServices : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            if (!Page.IsPostBack)
            {
                BindGrid();
            }

            grid.RowEditing += grid_RowEditing;
            grid.RowUpdating += grid_RowUpdating;
            grid.RowCancelingEdit += delegate { Response.Redirect(Request.RawUrl); };
            grid.RowDeleting += grid_RowDeleting;
            btnAdd.Click += btnAdd_Click;
            btnAdd.Text = Resources.labels.add + " ping service";
            btnBatchAdd.Click += BtnBatchAdd_Click;
            btnBatchAdd.Text = Resources.labels.add + " ping services";
        }

        /// <summary>
        /// Handles the Click event of the btnBatchAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnBatchAdd_Click(object sender, EventArgs eventArgs)
        {
            StringCollection col = BlogService.LoadPingServices();

            foreach (string service in txtBatchPingService.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!col.Contains(service.ToLower()))
                {
                    col.Add(service);
                }
                else
                {
                    liBatchOutput.Text += service + " " + Resources.labels.pingServiceNotUnique + ".<br/>";
                }
            }
            BlogService.SavePingServices(col);
			BindGrid();
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            StringCollection col = BlogService.LoadPingServices();
            string service = txtNewCategory.Text;
            if (!col.Contains(service))
            {
                col.Add(service);
                BlogService.SavePingServices(col);
            }
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Handles the RowDeleting event of the grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string service = grid.DataKeys[e.RowIndex].Value.ToString();
            StringCollection col = BlogService.LoadPingServices();
            col.Remove(service);
            BlogService.SavePingServices(col);
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Handles the RowUpdating event of the grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string service = grid.DataKeys[e.RowIndex].Value.ToString();
            TextBox textbox = (TextBox)grid.Rows[e.RowIndex].FindControl("txtName");

            StringCollection col = BlogService.LoadPingServices();
            col.Remove(service);
            col.Add(textbox.Text);
            BlogService.SavePingServices(col);

            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Handles the RowEditing event of the grid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        void grid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        private void BindGrid()
        {
            StringCollection col = BlogService.LoadPingServices();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            foreach (string services in col)
            {
                dic.Add(services, services);
            }

            grid.DataKeyNames = new string[] { "key" };
            grid.DataSource = dic;
            grid.DataBind();
        }

    }   
}