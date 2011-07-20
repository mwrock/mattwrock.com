namespace Admin.Comments
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    using BlogEngine.Core;

    using Resources;

    using Page = System.Web.UI.Page;

    /// <summary>
    /// The editor.
    /// </summary>
    public partial class Editor : Page
    {
        #region Private members

        /// <summary>
        /// The editor id.
        /// </summary>
        private static string id;

        /// <summary>
        /// The comment.
        /// </summary>
        private static Comment comment;

        /// <summary>
        /// The url referrer.
        /// </summary>
        private static string urlReferrer;

        /// <summary>
        /// The gravatar image.
        /// </summary>
        private const string GravatarImage = "<img class=\"photo\" src=\"{0}\" alt=\"{1}\" />";

        /// <summary>
        /// The flag image.
        /// </summary>
        private const string FlagImage =
            "<span class=\"adr\"><img src=\"{0}pics/flags/{1}.png\" class=\"country-name flag\" title=\"{2}\" alt=\"{2}\" /></span>";

        #endregion

        /// <summary>
        /// Gets CurrentComment.
        /// </summary>
        public Comment CurrentComment
        {
            get
            {
                return comment;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            Security.DemandUserHasRight(BlogEngine.Core.Rights.AccessAdminPages, true);
            this.BtnDelete.OnClientClick = string.Format("return confirm('{0}');", labels.deleteConfirm);

            id = HttpContext.Current.Request.QueryString["id"];
            var urlref = HttpContext.Current.Request.UrlReferrer;
            if (urlref != null)
            {
                urlReferrer = urlref.ToString();
            }

            comment = this.GetComment(id);

            if (urlReferrer.Contains("/Comments/Default.aspx"))
            {
                if (BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.IsCommentsEnabled)
                {
                    // "Comments: Auto-Moderated";
                    // "Comments: Manual Moderation";
                    this.BtnAction.Text = BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Auto ? labels.spam : labels.approve;
                }
            }

            if (urlReferrer.Contains("/Comments/Approved.aspx"))
            {
                this.BtnAction.Text = labels.reject;
            }

            if (urlReferrer.Contains("/Comments/Spam.aspx"))
            {
                this.BtnAction.Text = labels.restore;
            }

            this.BtnAction.Visible = BlogSettings.Instance.EnableCommentsModeration &&
                                     BlogSettings.Instance.IsCommentsEnabled;

            if (comment.Website != null)
            {
                this.txtWebsite.Text = comment.Website.ToString();
            }

            this.txtAuthor.Text = comment.Author;
            this.txtEmail.Text = comment.Email;
            this.txtArea.Value = comment.Content;

            base.OnInit(e);
        }

        #region Button clicks

        /// <summary>
        /// BTNs the save click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            var found = false;

            // Cast ToArray so the original collection isn't modified. 
            foreach (var p in Post.Posts.ToArray())
            {
                // Cast ToArray so the original collection isn't modified. 
                foreach (var c in p.Comments.ToArray())
                {
                    if (c.Id.ToString() != id)
                    {
                        continue;
                    }

                    Uri website = null;
                    if (!string.IsNullOrEmpty(this.txtWebsite.Text))
                    {
                        Uri.TryCreate(this.txtWebsite.Text.Trim(), UriKind.Absolute, out website);
                    }

                    c.Content = this.txtArea.Value;
                    c.Author = this.txtAuthor.Text;
                    c.Website = website;
                    c.Email = this.txtEmail.Text;

                    // need to mark post as "dirty"
                    p.DateModified = DateTime.Now;
                    p.Save();

                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            this.Reload();
        }

        /// <summary>
        /// BTNs the action click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnActionClick(object sender, EventArgs e)
        {
            if (BlogSettings.Instance.EnableCommentsModeration && BlogSettings.Instance.IsCommentsEnabled)
            {
                var found = false;
                foreach (var p in Post.Posts.ToArray())
                {
                    foreach (var c in p.Comments.ToArray())
                    {
                        if (c.Id.ToString() != id)
                        {
                            continue;
                        }

                        var desc = p.Description;
                        p.Description = string.Format("{0} ", desc ?? string.Empty);
                        p.Description = desc;

                        if (urlReferrer.Contains("/Comments/Default.aspx"))
                        {
                            c.IsApproved = BlogSettings.Instance.ModerationType != BlogSettings.Moderation.Auto;
                        }

                        if (urlReferrer.Contains("/Comments/Approved.aspx"))
                        {
                            c.IsApproved = false;
                        }

                        if (urlReferrer.Contains("/Comments/Spam.aspx"))
                        {
                            c.IsApproved = true;
                        }

                        // moderator should match anti-spam service
                        if (BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Auto)
                        {
                            CommentHandlers.ReportMistake(c);
                        }

                        // now moderator can be set to admin role
                        c.ModeratedBy = HttpContext.Current.User.Identity.Name;

                        // need to mark post as "dirty"
                        p.DateModified = DateTime.Now;
                        p.Save();

                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }
            }

            this.Reload();
        }

        /// <summary>
        /// BTNs the delete click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnDeleteClick(object sender, EventArgs e)
        {
            this.DeleteCurrentComment();

            this.Reload();
        }

        /// <summary>
        /// Handles the Click event of the btnBlockIP control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnBlockIPClick(object sender, EventArgs e)
        {
            // CommentHandlers.AddIpToFilter(CurrentComment.IP, true, true);
        }

        /// <summary>
        /// Handles the Click event of the btnAllowIP control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnAllowIPClick(object sender, EventArgs e)
        {
            // CommentHandlers.AddIpToFilter(CurrentComment.IP, false, true);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns>The comment.</returns>
        protected Comment GetComment(string commentId)
        {
            return Post.Posts.SelectMany(p => p.Comments).FirstOrDefault(c => c.Id.ToString() == commentId);
        }

        /// <summary>
        /// Deletes the current comment.
        /// </summary>
        protected void DeleteCurrentComment()
        {
            var cmnts = from p in Post.Posts from c in p.Comments where c.Id == comment.Id select new { p, c };
            var cmnt = cmnts.FirstOrDefault();
            if (cmnt != null)
            {
                cmnt.p.RemoveComment(cmnt.c);
            }
        }

        /// <summary>
        /// Gravatars the specified size.
        /// </summary>
        /// <param name="size">The size of the gravatar.</param>
        /// <returns>The gravatar.</returns>
        protected string Gravatar(int size)
        {
            if (BlogSettings.Instance.Avatar == "none")
            {
                return null;
            }

            if (String.IsNullOrEmpty(comment.Email) || !comment.Email.Contains("@"))
            {
                if (comment.Website != null && comment.Website.ToString().Length > 0 &&
                    comment.Website.ToString().Contains("http://"))
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "<img class=\"thumb\" src=\"http://images.websnapr.com/?url={0}&amp;size=t\" alt=\"{1}\" />",
                        this.Server.UrlEncode(comment.Website.ToString()),
                        comment.Email);
                }

                return string.Format("<img src=\"{0}themes/{1}/noavatar.jpg\" alt=\"{2}\" />", Utils.AbsoluteWebRoot, BlogSettings.Instance.Theme, comment.Author);
            }

            var hashedPassword =
                FormsAuthentication.HashPasswordForStoringInConfigFile(comment.Email.ToLowerInvariant().Trim(), "MD5");
            if (hashedPassword != null)
            {
                var hash = hashedPassword.ToLowerInvariant();
                var gravatar = string.Format("http://www.gravatar.com/avatar/{0}.jpg?s={1}&amp;d=", hash, size);

                string link;
                switch (BlogSettings.Instance.Avatar)
                {
                    case "identicon":
                        link = string.Format("{0}identicon", gravatar);
                        break;

                    case "wavatar":
                        link = string.Format("{0}wavatar", gravatar);
                        break;

                    default:
                        link = string.Format("{0}monsterid", gravatar);
                        break;
                }

                return string.Format(CultureInfo.InvariantCulture, GravatarImage, link, comment.Author);
            }

            return null;
        }

        /// <summary>
        /// Gets Flag.
        /// </summary>
        protected string Flag
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CurrentComment.Country))
                {
                    return string.Format(
                        FlagImage,
                        Utils.RelativeWebRoot,
                        this.CurrentComment.Country,
                        FindCountry(this.CurrentComment.Country));
                }

                return null;
            }
        }

        /// <summary>
        /// Finds the country.
        /// </summary>
        /// <param name="isoCode">The iso code.</param>
        /// <returns>The ISO code.</returns>
        private static string FindCountry(string isoCode)
        {
            foreach (var ri in
                CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(ci => new RegionInfo(ci.Name)).Where(ri => ri.TwoLetterISORegionName.Equals(isoCode, StringComparison.OrdinalIgnoreCase)))
            {
                return ri.DisplayName;
            }

            return isoCode;
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        protected void Reload()
        {
            this.ClientScript.RegisterClientScriptBlock(
                this.GetType(), "ClientScript", "<script language='JavaScript'>parent.closeEditor(true);</script>");
        }

        #endregion
    }
}