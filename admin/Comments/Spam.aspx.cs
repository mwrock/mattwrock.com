namespace Admin.Comments
{
    using System;
    using System.Collections;
    using System.Web.Services;
    using System.Web.UI;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    /// <summary>
    /// The spam settings.
    /// </summary>
    public partial class Spam : System.Web.UI.Page
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

        /// <summary>
        /// Loads the comments.
        /// </summary>
        /// <param name="page">
        /// The page number.
        /// </param>
        /// <returns>
        /// An enumerable of comments.
        /// </returns>
        [WebMethod]
        public static IEnumerable LoadComments(int page)
        {
            var commentList = JsonComments.GetComments(CommentType.Spam, page);
            CommentCounter = commentList.Count;
            return commentList;
        }

        /// <summary>
        /// Loads the pager.
        /// </summary>
        /// <param name="page">
        /// The page number.
        /// </param>
        /// <returns>
        /// The pager.
        /// </returns>
        [WebMethod]
        public static string LoadPager(int page)
        {
            return JsonComments.GetPager(page);
        }
    }
}