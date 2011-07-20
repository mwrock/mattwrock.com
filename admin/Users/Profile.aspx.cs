namespace Admin.Users
{
    using System;
    using System.Linq;
    using System.Web.Services;
    using System.Web.Security;
    using BlogEngine.Core;

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

        /// <summary>
        /// The membership user for the id.
        /// </summary>
        private MembershipUser MembershipUser;

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
                var allRoles = System.Web.Security.Roles.GetAllRoles().Where(r => !r.Equals(BlogSettings.Instance.AnonymousRole, StringComparison.OrdinalIgnoreCase));
                return allRoles.Aggregate(ret, (current, r) => current + (System.Web.Security.Roles.IsUserInRole(theId, r) ? string.Format(Ptrn, r, "checked") : string.Format(Ptrn, r, string.Empty)));
            }
        }

        #endregion

        #region Public Methods

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
        public static AuthorProfile GetProfile(string id)
        {
            var pf = AuthorProfile.GetProfile(id) ?? new AuthorProfile
                {
                    DisplayName = string.Empty,
                    FirstName = string.Empty,
                    MiddleName = string.Empty,
                    LastName = string.Empty,
                    Birthday = new DateTime(1001, 1, 1),
                    PhotoUrl = string.Empty,
                    EmailAddress = string.Empty,
                    PhoneMobile = string.Empty,
                    PhoneMain = string.Empty,
                    PhoneFax = string.Empty,
                    CityTown = string.Empty,
                    RegionState = string.Empty,
                    Country = string.Empty,
                    AboutMe = string.Empty
                };

            return pf;
        }

        #endregion

        #region Methods

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

            if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
            {
                theId = this.Request.QueryString["id"];
                MembershipUser = Membership.GetUser(theId);
            }

            if (MembershipUser == null)
            {
                Response.Redirect("Users.aspx");
                return;
            }

            bool canEditRoles = false;
            bool membershipUserIsSelf = MembershipUser.UserName.Equals(Security.CurrentUser.Identity.Name, StringComparison.OrdinalIgnoreCase);

            if (membershipUserIsSelf)
                Security.DemandUserHasRight(BlogEngine.Core.Rights.EditOwnUser, true);
            else
                Security.DemandUserHasRight(BlogEngine.Core.Rights.EditOtherUsers, true);

            if (membershipUserIsSelf && Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOwnRoles))
                canEditRoles = true;
            else if (!membershipUserIsSelf && Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOtherUsersRoles))
                canEditRoles = true;

            phRoles.Visible = canEditRoles;
            phRightContentBox.Visible = Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages);

            base.OnInit(e);
        }

        #endregion
    }
}