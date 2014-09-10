namespace Admin.Users
{
    using System;
    using System.Linq;
    using System.Web.Services;
    using System.Web.Security;
    using BlogEngine.Core;
    using BlogEngine.Core.Json;
    using System.Web;
    using System.IO;

    /// <summary>
    /// The admin pages profile.
    /// </summary>
    public partial class ProfilePage : System.Web.UI.Page
    {
        #region Constants and Fields

        /// <summary>
        /// The id string.
        /// </summary>
        private string theId = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets RolesList.
        /// </summary>
        protected string RolesList
        {
            get
            {
                var ret = string.Empty;
                const string Ptrn = "<input type=\"checkbox\" id=\"{0}\" class=\"chkRole\" {1} /><span class=\"lbl\">{0}</span>";
                var allRoles = System.Web.Security.Roles.GetAllRoles().Where(r => !r.Equals(BlogConfig.AnonymousRole, StringComparison.OrdinalIgnoreCase));
                return allRoles.Aggregate(ret, (current, r) => current + (System.Web.Security.Roles.IsUserInRole(theId, r) ? string.Format(Ptrn, r, "checked") : string.Format(Ptrn, r, string.Empty)));
            }
        }

        protected string AvatarImage
        {
            get
            {
                var pf = AuthorProfile.GetProfile(theId) ?? new AuthorProfile(theId);
                return Avatar.GetAvatar(pf.EmailAddress, null, "", "", 48, 48).ImageTag;
            }
        }

        #endregion

        #region Public Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.Enctype = "multipart/form-data";
            if (Page.IsPostBack)
            {
                try
                {
                    HttpPostedFile file = Request.Files["file"];

                    if (file != null && file.ContentLength > 0)
                    {
                        string id = Request.QueryString["id"];
                        string login = string.IsNullOrEmpty(id) ? HttpContext.Current.User.Identity.Name : id;
                        string dir = Server.MapPath(Path.Combine(BlogConfig.StorageLocation, "files/Avatars", Blog.CurrentInstance.Name, login));

                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        string fname = Path.GetFileName(file.FileName);
                        fname = Path.Combine(dir, fname);

                        file.SaveAs(fname);

                        var pf = AuthorProfile.GetProfile(login) ?? new AuthorProfile(login);
                        pf.PhotoUrl = Blog.CurrentInstance.Name + "/" + login + "/" + file.FileName;

                        pf.Save();

                        Master.SetStatus("success", "File saved");
                    }
                }
                catch (Exception ex)
                {
                    Master.SetStatus("warning", ex.Message);
                }
            }
        }

        /// <summary>
        /// The get profile.
        /// </summary>
        /// <param name="id">
        /// The profile id.
        /// </param>
        /// <returns>
        /// An AuthorProfile.
        /// </returns>
        [WebMethod]
        public static JsonProfile GetProfile(string id)
        {
            if (!Utils.StringIsNullOrWhitespace(id))
            { 
                bool canEditRoles;
                if (!CanUserEditProfile(id, out canEditRoles))
                    return null;

                var pf = AuthorProfile.GetProfile(id);

                if (pf == null)
                {
                    pf = new AuthorProfile(id);
                    pf.Birthday = DateTime.Parse("01/01/1900");
                    pf.DisplayName = id;
                    pf.EmailAddress = Utils.GetUserEmail(id);
                    pf.FirstName = id;
                    pf.Private = true;
                    pf.Save();
                }

                return AuthorProfile.ToJson(id);
            }

            return null;
        }

        #endregion

        #region Methods

        private static bool CanUserEditProfile(string id, out bool canEditRoles)
        {
            canEditRoles = false;

            if (Utils.StringIsNullOrWhitespace(id))
                return false;

            MembershipUser user = Membership.GetUser(id);
            if (user == null)
                return false;

            bool membershipUserIsSelf = user.UserName.Equals(Security.CurrentUser.Identity.Name, StringComparison.OrdinalIgnoreCase);

            if (membershipUserIsSelf && Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOwnRoles))
                canEditRoles = true;
            else if (!membershipUserIsSelf && Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOtherUsersRoles))
                canEditRoles = true;

            if (membershipUserIsSelf)
                return Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOwnUser);
            else
                return Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOtherUsers);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // Rights.AccessAdminPages isn't needed here.  If self-registration is turned
            // on, we will allow a user who cannot AccessAdminPages to edit their profile.
            if (!Security.IsAuthenticated)
            {
                Security.RedirectForUnauthorizedRequest();
                return;
            }

            bool canEditRoles = false;
            if (!CanUserEditProfile(Request.QueryString["id"], out canEditRoles))
            {
                //Response.Redirect("Users.aspx");
                //return;
            }

            this.theId = Request.QueryString["id"];

            phRoles.Visible = canEditRoles;
            phRightContentBox.Visible = Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages);

            base.OnInit(e);
        }

        #endregion
    }
}