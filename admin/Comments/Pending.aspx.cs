namespace Admin.Comments
{
    using System;
    using System.Collections;
    using System.Web.Services;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class Pending : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);
        }

        /// <summary>
        /// Number of comments in the list
        /// </summary>
        protected static int CommentCounter { get; set; }

        [WebMethod]
        public static IEnumerable LoadComments(int page)
        {
            var commentList = JsonComments.GetComments(CommentType.Pending, page);
            CommentCounter = commentList.Count;
            return commentList;
        }

        [WebMethod]
        public static string LoadPager(int page)
        {
            return JsonComments.GetPager(page);
        }
    }

}