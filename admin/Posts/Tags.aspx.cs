namespace Admin.Posts
{
    using System;
    using BlogEngine.Core;

    public partial class Tags : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Security.DemandUserHasRight(Rights.AccessAdminPages, true);
        }
    }
}