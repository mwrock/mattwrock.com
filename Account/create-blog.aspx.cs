namespace Account
{
    using BlogEngine.Core;
    using System;
    using Page = System.Web.UI.Page;

    public partial class CreateBlog : Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void CreateUserButton_Click(object sender, EventArgs e)
        {
            string blogName = BlogName.Text.Trim().ToLower();

            string msg = CreateNewBlog();

            if (string.IsNullOrEmpty(msg))
            {
                this.Response.Redirect(Utils.ApplicationRelativeWebRoot + blogName);
            }
            else
            {
                this.Master.SetStatus("warning", msg);
            }
        }

        string CreateNewBlog()
        {
            string message = string.Empty;
            Blog blog = null;

            if (!BlogGenerator.ValidateProperties(BlogName.Text, UserName.Text, Email.Text, out message))
            {
                if (string.IsNullOrWhiteSpace(message)) { message = "Validation for new blog failed."; }
                return message;
            }

            blog = BlogGenerator.CreateNewBlog(BlogName.Text, UserName.Text, Email.Text, Password.Text, out message);

            if (blog == null || !string.IsNullOrWhiteSpace(message))
            {
                return message ?? "Failed to create the new blog.";
            }

            return message;
        }
    }
}