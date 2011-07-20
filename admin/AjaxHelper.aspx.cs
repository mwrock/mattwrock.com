using System.Collections.Specialized;
using System.IO;

namespace Admin
{
    using System;
    using System.Collections;
    using System.Web.Services;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;

    using BlogEngine.Core;
    using BlogEngine.Core.Json;

    public partial class AjaxHelper : System.Web.UI.Page
    {    

        [WebMethod]
        public static JsonComment GetComment(string id)
        {
            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                return null;
            }
            return JsonComments.GetComment(new Guid(id));
        }

        [WebMethod]
        public static JsonComment SaveComment(string[] vals)
        {
            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
            {
                return null;
            }
            var gId = new Guid(vals[0]);
            string author = vals[1];
            string email = vals[2];
            string website = vals[3];
            string cont = vals[4];

            foreach (Post p in Post.Posts.ToArray())
            {
                foreach (Comment c in p.Comments.ToArray())
                {
                    if (c.Id == gId)
                    {
                        c.Author = author;
                        c.Email = email;
                        c.Website = string.IsNullOrEmpty(website) ? null : new Uri(website);
                        c.Content = cont;

                        // need to mark post as "dirty"
                        p.DateModified = DateTime.Now;
                        p.Save();

                        return JsonComments.GetComment(gId);
                    }
                }
            }

            return new JsonComment();
        }

        [WebMethod]
        public static IEnumerable LoadPosts(int page, string  type, string filter, string title)
        {
            return JsonPosts.GetPosts(page, type, filter, title);
        }

        [WebMethod]
        public static IEnumerable LoadPages(string type)
        {
            return JsonPages.GetPages(type);
        }

        [WebMethod]
        public static IEnumerable LoadTags(int page)
        {
            var tags = new List<JsonTag>();
            foreach (var p in Post.Posts)
            {
                foreach (var t in p.Tags)
                {
                    var tg = tags.FirstOrDefault(tag => tag.TagName == t);
                    if (tg == null)
                    {
                        tags.Add(new JsonTag {TagName = t, TagCount = 1});
                    }
                    else
                    {
                        tg.TagCount++;
                    }
                }
            }
            return from t in tags orderby t.TagName select t;
        }

        [WebMethod]
        public static string LoadPostPager(int page)
        {
            return JsonPosts.GetPager(page);
        }

        [WebMethod]
        public static JsonResponse SavePost(
            string id,
            string content,
            string title,
            string desc,
            string slug,
            string tags,
            string author,
            bool isPublished,
            bool hasCommentsEnabled,
            string cats,
            string date,
            string time)
        {
            var response = new JsonResponse { Success = false };
            var settings = BlogSettings.Instance;

            if (string.IsNullOrEmpty(id) && !Security.IsAuthorizedTo(Rights.CreateNewPosts))
            {
                response.Message = "Not authorized to create new Posts.";
                return response;
            }

            try
            {
                var post = string.IsNullOrEmpty(id) ? new BlogEngine.Core.Post() : BlogEngine.Core.Post.GetPost(new Guid(id));
                if (post == null)
                {
                    response.Message = "Post to Edit was not found.";
                    return response;
                }
                else if (!string.IsNullOrEmpty(id) && !post.CanUserEdit)
                {
                    response.Message = "Not authorized to edit this Post.";
                    return response;
                }

                if (!post.IsPublished && isPublished)
                {
                    if (!post.CanPublish(author))
                    {
                        response.Message = "Not authorized to publish this Post.";
                        return response;
                    }
                }

                if (string.IsNullOrEmpty(content))
                {
                    content = "[No text]";
                }
                post.Author = author;
                post.Title = title;
                post.Content = content;
                post.Description = desc;

                if (!string.IsNullOrEmpty(slug))
                {
                    post.Slug = Utils.RemoveIllegalCharacters(slug.Trim());
                }

                post.DateCreated =
                DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH\\:mm", null).AddHours(
                    -BlogSettings.Instance.Timezone);

                post.IsPublished = isPublished;
                post.HasCommentsEnabled = hasCommentsEnabled;

                post.Tags.Clear();
                if (tags.Trim().Length > 0)
                {
                    var vtags = tags.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in
                        vtags.Where(tag => string.IsNullOrEmpty(post.Tags.Find(t => t.Equals(tag.Trim(), StringComparison.OrdinalIgnoreCase)))))
                    {
                        post.Tags.Add(tag.Trim());
                    }
                }

                post.Categories.Clear();
                if (cats.Trim().Length > 0)
                {
                    var vcats = cats.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cat in vcats)
                    {
                        post.Categories.Add(Category.GetCategory(new Guid(cat)));
                    }
                }
               
                post.Save();
                response.Data = post.RelativeLink;

                HttpContext.Current.Session.Remove("content");
                HttpContext.Current.Session.Remove("title");
                HttpContext.Current.Session.Remove("description");
                HttpContext.Current.Session.Remove("slug");
                HttpContext.Current.Session.Remove("tags");
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Admin.AjaxHelper.SavePost(): {0}", ex.Message));
                response.Message = string.Format("Could not save post: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Post saved";
            
            return response;
        }

        [WebMethod]
        public static JsonResponse SavePage(
            string id,
            string content,
            string title,
            string description,
            string keywords,
            string slug,
            bool isFrontPage,
            bool showInList,
            bool isPublished,
            string parent)
        {
            var response = new JsonResponse { Success = false };
            var settings = BlogSettings.Instance;

            if (string.IsNullOrEmpty(id) && !Security.IsAuthorizedTo(Rights.CreateNewPages))
            {
                response.Message = "Not authorized to create new Pages.";
                return response;
            }

            try
            {
                var page = string.IsNullOrEmpty(id) ? new BlogEngine.Core.Page() : BlogEngine.Core.Page.GetPage(new Guid(id));

                if (page == null)
                {
                    response.Message = "Page to Edit was not found.";
                    return response;
                }
                else if (!string.IsNullOrEmpty(id) && !page.CanUserEdit)
                {
                    response.Message = "Not authorized to edit this Page.";
                    return response;
                }

                if (!page.IsPublished && isPublished)
                {
                    if (!page.CanPublish())
                    {
                        response.Message = "Not authorized to publish this Page.";
                        return response;
                    }
                }

                page.Title = title;
                page.Content = content;
                page.Description = description;
                page.Keywords = keywords;

                if (isFrontPage)
                {
                    foreach (var otherPage in BlogEngine.Core.Page.Pages.Where(otherPage => otherPage.IsFrontPage))
                    {
                        otherPage.IsFrontPage = false;
                        otherPage.Save();
                    }
                }

                page.IsFrontPage = isFrontPage;
                page.ShowInList = showInList;
                page.IsPublished = isPublished;

                if (!string.IsNullOrEmpty(slug))
                {
                    page.Slug = Utils.RemoveIllegalCharacters(slug.Trim());
                }

                if (parent == string.Format("-- {0} --", Resources.labels.noParent))
                    page.Parent = Guid.Empty;
                else
                    page.Parent = new Guid(parent);

                page.Save();
                response.Data = page.RelativeLink;
            }
            catch (Exception ex)
            {
                Utils.Log(string.Format("Admin.AjaxHelper.SavePage(): {0}", ex.Message));
                response.Message = string.Format("Could not save page: {0}", ex.Message);
                return response;
            }

            response.Success = true;
            response.Message = "Page saved";
            return response;
        }

    }
}