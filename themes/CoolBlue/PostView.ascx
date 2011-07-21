<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>


<div class="post">
    <div class="right" id="post<%=Index %>">

        <h2><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Server.HtmlEncode(Post.Title) %></a></h2>
        <p class="post-info">Filed under <%=CategoryLinks(" , ") %></p>

        <asp:PlaceHolder ID="BodyContent" runat="server" />

        <%=Rating %>

        <div class="share-box clear">
            <h4>Share This</h4>
            <ul>
                <li><a title="RSS" href="<%=CommentFeed %>" rel="nofollow"><img alt="RSS" title="RSS" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>rss_32.png" /></a> </li>
                <li><a title="del.icio.us" href="http://del.icio.us/post?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="del.icio.us" title="del.icio.us" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>delicious_32.png" /></a></li>
                <li><a title="StumbleUpon" href="http://stumbleupon.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="StumbleUpon" title="StumbleUpon" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>stumbleupon_32.png" /></a></li>
                <li><a title="Digg" href="http://digg.com/submit?phase=2&amp;url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="Digg" title="Digg" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>digg_32.png" /></a></li>
                <li><a title="Facebook" href="http://facebook.com/share.php?u=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;t=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="Facebook" title="Facebook" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>facebook_32.png" /></a></li>
			    <li><a title="Twitter" href="http://twitter.com/home?status=Currently reading <%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>" rel="nofollow"><img alt="Twitter" title="Twitter" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>twitter_32.png" /></a></li>
                <li><a title="Technorati" href="http://technorati.com/faves?add=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="Technorati" title="Technorati" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>technorati_32.png" /></a></li>
			    <li><a title="NewsVine" href="http://www.newsvine.com/_wine/save?u=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&h=title=<%=Server.UrlEncode(Post.Title) %>" rel="nofollow"><img alt="NewsVine" title="NewsVine" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>newsvine_32.png" /></a></li>
                <li><a title="LinkedIn" href="http://www.linkedin.com/shareArticle?mini=true&url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&title=<%=Server.UrlEncode(Post.Title) %>&source=reddybrek.com" rel="nofollow"><img alt="LinkedIn" title="LinkedIn" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>linkedin_32.png" /></a></li>
                <li><a title="E-mail this story to a friend!" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>" rel="nofollow"><img alt="E-mail this story to a friend!" title="E-mail this story to a friend!" src="<%=VirtualPathUtility.ToAbsolute("~/themes/CoolBlue/images/")%>email_32.png" /></a> </li>
            </ul>
        </div>
        
        <p class="post-info"><%=AdminLinks %></p>

    </div>

    <div class="left">
        <p class="dateinfo">
            <%=Post.DateCreated.ToString("MMM") %><span><%=Post.DateCreated.ToString("dd") %></span></p>
        <div class="post-meta">
            <h4>Post Info</h4>
            <ul>
                <li class="user"><a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + BlogEngine.Core.Utils.RemoveIllegalCharacters(Post.Author) %>.aspx"><%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName : Post.Author %></a></li>
                <li class="time"><a href="#"><%=Post.DateCreated.ToString("d. MMM yyyy HH:mm") %></a></li>
                <% if (BlogEngine.Core.BlogSettings.Instance.ModerationType == BlogEngine.Core.BlogSettings.Moderation.Disqus)
                { %>
                <li class="disqus"><a rel="nofollow" href="<%=Post.PermaLink %>#disqus_thread"><%=Resources.labels.comments %></a></li>
                <%}
                else
                { %>
                    <li class="comment"><a href="<%=Post.RelativeLink %>#comment"><%=Post.ApprovedComments.Count %> <%=Resources.labels.comments %></a></li>
                    <li class="permalink"><a href="<%=Post.PermaLink %>" title="<%=Server.HtmlEncode(Post.Title) %>">Permalink</a></li>  
                <%} %>
            </ul>
        </div>
        <div class="post-meta">
            <h4>Tags</h4>
            <ul class="tags">
                <li><%=TagLinks(", ") %></li>
            </ul>
        </div>
    </div>
</div>