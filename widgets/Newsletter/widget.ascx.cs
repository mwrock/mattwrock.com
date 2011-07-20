// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The widget.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Widgets.Newsletter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Xml;

    using App_Code.Controls;

    using BlogEngine.Core;
    using BlogEngine.Core.Providers;

    /// <summary>
    /// The widget.
    /// </summary>
    public partial class Widget : WidgetBase, ICallbackEventHandler
    {
        #region Constants and Fields

        /// <summary>
        ///     The xml doc.
        /// </summary>
        private static XmlDocument doc;

        /// <summary>
        ///     The filename.
        /// </summary>
        private static string fileName;

        /// <summary>
        ///     The callback.
        /// </summary>
        private string callback;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref = "Widget" /> class.
        /// </summary>
        static Widget()
        {
            Post.Saved += PostSaved;
            Post.Saving += PostSaving;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether IsEditable.
        /// </summary>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     Gets Name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Newsletter";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method works as a substitute for Page_Load. You should use this method for
        ///     data binding etc. instead of Page_Load.
        /// </summary>
        public override void LoadWidget()
        {
        }

        #endregion

        #region Implemented Interfaces

        #region ICallbackEventHandler

        /// <summary>
        /// Returns the results of a callback event that targets a control.
        /// </summary>
        /// <returns>
        /// The result of the callback.
        /// </returns>
        public string GetCallbackResult()
        {
            return this.callback;
        }

        /// <summary>
        /// Processes a callback event that targets a control.
        /// </summary>
        /// <param name="eventArgument">
        /// A string that represents an event argument to pass to the event handler.
        /// </param>
        public void RaiseCallbackEvent(string eventArgument)
        {
            this.callback = eventArgument;
            this.AddEmail(eventArgument);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Creates the email.
        /// </summary>
        /// <param name="post">
        /// The post to mail.
        /// </param>
        /// <returns>
        /// The email.
        /// </returns>
        private static MailMessage CreateEmail(Post post)
        {
            var mail = new MailMessage
                {
                    Subject = post.Title, 
                    Body = FormatBodyMail(post), 
                    From = new MailAddress(BlogSettings.Instance.Email, BlogSettings.Instance.Name)
                };
            return mail;
        }

        /// <summary>
        /// Does the email exist.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// Whether the email exists.
        /// </returns>
        private static bool DoesEmailExist(string email)
        {
            return doc.SelectSingleNode(string.Format("emails/email[text()='{0}']", email)) != null;
        }

        /// <summary>
        /// Replace tags below in newsletter.html theme
        ///     [TITLE]
        ///     [LINK_DESCRIPTION]
        ///     [LINK]
        ///     [WebRoot]
        ///     [httpBase]
        /// </summary>
        /// <param name="post">
        /// The post to format.
        /// </param>
        /// <returns>
        /// The format body mail.
        /// </returns>
        private static string FormatBodyMail(Post post)
        {
            var body = new StringBuilder();
            var urlbase = Path.Combine(
                Path.Combine(Utils.AbsoluteWebRoot.AbsoluteUri, "themes"), BlogSettings.Instance.Theme);
            var filePath = string.Format("~/themes/{0}/newsletter.html", BlogSettings.Instance.Theme);
            filePath = HostingEnvironment.MapPath(filePath);
            if (File.Exists(filePath))
            {
                body.Append(File.ReadAllText(filePath));
            }
            else
            {
                // if custom theme doesn't have email template
                // use email template from standard theme
                filePath = HostingEnvironment.MapPath("~/themes/Standard/newsletter.html");
                if (File.Exists(filePath))
                {
                    body.Append(File.ReadAllText(filePath));
                }
                else
                {
                    Utils.Log(
                        "When sending newsletter, newsletter.html does not exist " +
                        "in theme folder, and does not exist in the Standard theme " +
                        "folder.");
                }
            }

            body = body.Replace("[TITLE]", post.Title);
            body = body.Replace("[LINK]", post.AbsoluteLink.AbsoluteUri);
            body = body.Replace("[LINK_DESCRIPTION]", post.Description);
            body = body.Replace("[WebRoot]", Utils.AbsoluteWebRoot.AbsoluteUri);
            body = body.Replace("[httpBase]", urlbase);
            return body.ToString();
        }

        /// <summary>
        /// Gets the send newsletters context data.
        /// </summary>
        /// <returns>
        /// A dictionary.
        /// </returns>
        private static Dictionary<Guid, bool> GetSendNewslettersContextData()
        {
            const string SendNewsletterEmailsContextItemKey = "SendNewsletterEmails";
            Dictionary<Guid, bool> data;

            if (HttpContext.Current.Items.Contains(SendNewsletterEmailsContextItemKey))
            {
                data = HttpContext.Current.Items[SendNewsletterEmailsContextItemKey] as Dictionary<Guid, bool>;
            }
            else
            {
                data = new Dictionary<Guid, bool>();
                HttpContext.Current.Items[SendNewsletterEmailsContextItemKey] = data;
            }

            return data;
        }

        /// <summary>
        /// Gets the send send newsletter emails.
        /// </summary>
        /// <param name="postId">
        /// The post id.
        /// </param>
        /// <returns>
        /// Whether send newsletter emails.
        /// </returns>
        private static bool GetSendSendNewsletterEmails(Guid postId)
        {
            var data = GetSendNewslettersContextData();

            return data.ContainsKey(postId) && data[postId];
        }

        /// <summary>
        /// Loads the emails.
        /// </summary>
        private static void LoadEmails()
        {
            if (doc != null && fileName != null)
            {
                return;
            }

            fileName = Path.Combine(BlogSettings.Instance.StorageLocation, "newsletter.xml");
            fileName = HostingEnvironment.MapPath(fileName);

            if (File.Exists(fileName))
            {
                doc = new XmlDocument();
                doc.Load(fileName);
            }
            else
            {
                doc = new XmlDocument();
                doc.LoadXml("<emails></emails>");
            }
        }

        /// <summary>
        /// Handles the Saved event of the Post control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="BlogEngine.Core.SavedEventArgs"/> instance containing the event data.
        /// </param>
        private static void PostSaved(object sender, SavedEventArgs e)
        {
            var post = (Post)sender;

            if (!GetSendSendNewsletterEmails(post.Id))
            {
                return;
            }

            LoadEmails();
            var emails = doc.SelectNodes("emails/email");
            if (emails == null)
            {
                return;
            }

            foreach (XmlNode node in emails)
            {
                var mail = CreateEmail(post);
                mail.To.Add(node.InnerText);
                Utils.SendMailMessageAsync(mail);
            }
        }

        /// <summary>
        /// Handles the Saving event of the Post control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="BlogEngine.Core.SavedEventArgs"/> instance containing the event data.
        /// </param>
        private static void PostSaving(object sender, SavedEventArgs e)
        {
            // Set SendNewsletterEmails to true whenever a post is changing from an unpublished
            // state to a published state.  To check the published state of this Post before
            // it was changed, it's necessary to retrieve the post from the datastore since the
            // post in memory (via Post.GetPost()) will already have the updated values about
            // to be saved.
            var post = (Post)sender;

            SetSendNewsletterEmails(post.Id, false); // default to not sending

            if (e.Action == SaveAction.Insert && post.IsVisibleToPublic)
            {
                SetSendNewsletterEmails(post.Id, true);
            }
            else if (e.Action == SaveAction.Update && post.IsVisibleToPublic)
            {
                var preUpdatePost = BlogService.SelectPost(post.Id);
                if (preUpdatePost != null && !preUpdatePost.IsVisibleToPublic)
                {
                    SetSendNewsletterEmails(post.Id, true);
                }
            }
        }

        /// <summary>
        /// Saves the emails.
        /// </summary>
        private static void SaveEmails()
        {
            using (var ms = new MemoryStream())
            using (var fs = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                doc.Save(ms);
                ms.WriteTo(fs);
            }
        }

        /// <summary>
        /// Sets the send newsletter emails.
        /// </summary>
        /// <param name="postId">
        /// The post id.
        /// </param>
        /// <param name="send">
        /// if set to <c>true</c> [send].
        /// </param>
        private static void SetSendNewsletterEmails(Guid postId, bool send)
        {
            var data = GetSendNewslettersContextData();
            data[postId] = send;
        }

        /// <summary>
        /// Adds the email.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        private void AddEmail(string email)
        {
            try
            {
                LoadEmails();

                if (!DoesEmailExist(email))
                {
                    XmlNode node = doc.CreateElement("email");
                    node.InnerText = email;
                    doc.FirstChild.AppendChild(node);

                    this.callback = "true";
                    SaveEmails();
                }
                else
                {
                    var emailNode = doc.SelectSingleNode(string.Format("emails/email[text()='{0}']", email));
                    if (emailNode != null)
                    {
                        doc.FirstChild.RemoveChild(emailNode);
                    }

                    this.callback = "false";
                    SaveEmails();
                }
            }
            catch
            {
                this.callback = "false";
            }
        }

        #endregion
    }
}