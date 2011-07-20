namespace Admin.Comments
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using BlogEngine.Core;
    using BlogEngine.Core.Web.Extensions;
    using App_Code;

    public partial class Rules : System.Web.UI.Page
    {
        static protected ExtensionSettings _filters;
        static protected ExtensionSettings _customFilters;

        /// <summary>
        /// Usually filter implemented as extension and can be turned
        /// on and off. If it is not extension, defaulted to enabled.
        /// </summary>
        /// <param name="filter">Filter (extension) name</param>
        /// <returns>True if enabled</returns>
        public bool CustomFilterEnabled(string filter)
        {
            var ext = ExtensionManager.GetExtension(filter);
            return ext == null ? true : ext.Enabled;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            _filters = ExtensionManager.GetSettings("MetaExtension", "BeCommentFilters");
            _customFilters = ExtensionManager.GetSettings("MetaExtension", "BeCustomFilters");

            if (!IsPostBack)
            {
                BindFilters();
                BindCustomFilters();
            }

            Page.MaintainScrollPositionOnPostBack = true;
            Page.Title = Resources.labels.comments;

            btnSave.Click += btnSave_Click;
            btnSave.Text = Resources.labels.saveSettings;
        }

        protected void BindFilters()
        {
            gridFilters.DataKeyNames = new string[] { _filters.KeyField };
            gridFilters.DataSource = _filters.GetDataTable();
            gridFilters.DataBind();

            // rules
            cbTrustAuthenticated.Checked = BlogSettings.Instance.TrustAuthenticatedUsers;
            ddWhiteListCount.SelectedValue = BlogSettings.Instance.CommentWhiteListCount.ToString();
            ddBlackListCount.SelectedValue = BlogSettings.Instance.CommentBlackListCount.ToString();
            cbReportMistakes.Checked = BlogSettings.Instance.CommentReportMistakes;
            cbBlockOnDelete.Checked = BlogSettings.Instance.BlockAuthorOnCommentDelete;
            cbAddIpToWhitelistFilterOnApproval.Checked = BlogSettings.Instance.AddIpToWhitelistFilterOnApproval;
            cbAddIpToBlacklistFilterOnRejection.Checked = BlogSettings.Instance.AddIpToBlacklistFilterOnRejection;
        }

        protected void BindCustomFilters()
        {
            gridCustomFilters.DataKeyNames = new string[] { _customFilters.KeyField };

            DataTable dt = _customFilters.GetDataTable();
            DataTable unsorted = dt.Clone();
            DataTable sorted = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                int i = int.TryParse(row["Priority"].ToString(), out i) ? i : 0;

                if (i > 0)
                    sorted.ImportRow(row);
                else
                    unsorted.ImportRow(row);
            }

            foreach (DataRow row in unsorted.Rows)
            {
                row["Priority"] = sorted.Rows.Count + 1;
                sorted.ImportRow(row);

                for (int i = 0; i < _customFilters.Parameters[0].Values.Count; i++)
                {
                    if (_customFilters.Parameters[0].Values[i] == row["FullName"].ToString())
                    {
                        _customFilters.Parameters[5].Values[i] = row["Priority"].ToString();
                    }
                }
            }

            ExtensionManager.SaveSettings("MetaExtension", _customFilters);

            sorted.DefaultView.Sort = "Priority";
            gridCustomFilters.DataSource = sorted;
            gridCustomFilters.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // rules
            BlogSettings.Instance.TrustAuthenticatedUsers = cbTrustAuthenticated.Checked;
            BlogSettings.Instance.CommentWhiteListCount = int.Parse(ddWhiteListCount.SelectedValue);
            BlogSettings.Instance.CommentBlackListCount = int.Parse(ddBlackListCount.SelectedValue);

            BlogSettings.Instance.CommentReportMistakes = cbReportMistakes.Checked;
            BlogSettings.Instance.BlockAuthorOnCommentDelete = cbBlockOnDelete.Checked;
            BlogSettings.Instance.AddIpToWhitelistFilterOnApproval = cbAddIpToWhitelistFilterOnApproval.Checked;
            BlogSettings.Instance.AddIpToBlacklistFilterOnRejection = cbAddIpToBlacklistFilterOnRejection.Checked;

            //-----------------------------------------------------------------------
            //  Persist settings
            //-----------------------------------------------------------------------
            BlogSettings.Instance.Save();

            Response.Redirect(Request.RawUrl, true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            int pageIdx = gridFilters.PageIndex;
            int pageSize = gridFilters.PageSize;
            int rowIndex = grdRow.RowIndex;

            if (pageIdx > 0) rowIndex = pageIdx * pageSize + rowIndex;


            foreach (ExtensionParameter par in _filters.Parameters)
            {
                par.DeleteValue(rowIndex);
            }

            ExtensionManager.SaveSettings("MetaExtension", _filters);
            Response.Redirect(Request.RawUrl);
        }

        protected void btnPriorityUp_click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            string s = gridCustomFilters.DataKeys[grdRow.RowIndex].Value.ToString();
            ChangePriority(s, true);
        }

        protected void btnPriorityDwn_click(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow grdRow = (GridViewRow)btn.Parent.Parent;

            string s = gridCustomFilters.DataKeys[grdRow.RowIndex].Value.ToString();
            ChangePriority(s, false);
        }

        protected void ChangePriority(string filterName, bool up)
        {
            for (int i = 0; i < _customFilters.Parameters[0].Values.Count; i++)
            {
                if (_customFilters.Parameters[0].Values[i] == filterName)
                {
                    int curPriority = int.Parse(_customFilters.Parameters[5].Values[i].ToString());

                    if (up && curPriority > 1)
                        curPriority--;
                    else
                        curPriority++;

                    _customFilters.Parameters[5].Values[i] = curPriority.ToString();
                }
            }

            ExtensionManager.SaveSettings("MetaExtension", _customFilters);
            Response.Redirect(Request.RawUrl);
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridFilters.PageIndex = e.NewPageIndex;
            BindFilters();
        }

        protected void btnAddFilter_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string id = Guid.NewGuid().ToString();
                string[] f = new string[] { id, 
                    ddAction.SelectedValue, 
                    ddSubject.SelectedValue, 
                    ddOperator.SelectedValue, 
                    txtFilter.Text };

                _filters.AddValues(f);
                ExtensionManager.SaveSettings("MetaExtension", _filters);
                Response.Redirect(Request.RawUrl);
            }
        }

        protected bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                FilterValidation.InnerHtml = "Filter is a required field";
                return false;
            }

            return true;
        }

        public static string ApprovedCnt(object total, object cought)
        {
            try
            {
                int t = int.Parse(total.ToString());
                int c = int.Parse(cought.ToString());

                int a = t - c;

                return a.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static string Accuracy(object total, object mistakes)
        {
            try
            {
                double t = double.Parse(total.ToString());
                double m = double.Parse(mistakes.ToString());

                if (m == 0 || t == 0) return "100";

                double a = 100 - (m / t * 100);

                return String.Format("{0:0.00}", a);
            }
            catch (Exception)
            {
                return "";
            }
        }

        protected void gridCustomFilters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string filterName = e.CommandArgument.ToString();

            if (!string.IsNullOrEmpty(filterName))
            {
                // reset statistics for this filter
                for (int i = 0; i < _customFilters.Parameters[0].Values.Count; i++)
                {
                    if (_customFilters.Parameters[0].Values[i] == filterName)
                    {
                        _customFilters.Parameters[2].Values[i] = "0";
                        _customFilters.Parameters[3].Values[i] = "0";
                        _customFilters.Parameters[4].Values[i] = "0";
                    }
                }

                ExtensionManager.SaveSettings("MetaExtension", _customFilters);
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}