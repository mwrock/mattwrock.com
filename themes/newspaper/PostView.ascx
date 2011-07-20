<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post xfolkentry">
	<div class="postTitle">
		<h1><a href="<%=Post.RelativeLink %>"><%=Post.Title %></a></h1>
	</div>
	<div class="description">
			<span>
			    <% if (TagLinks(", ") != null)
                { %>
			        Tags: <%=TagLinks(", ")%> | 
			    <%} %>
			    <% if (CategoryLinks(" | ") != null)
                {
                    if (CategoryLinks(", ").Length > 0)
                    { %>
                        Categories: <%=CategoryLinks(", ")%>
                        <% }   } %>
			</span>
		<%=AdminLinks %>Posted by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx"><%=Post.Author %></a> on 
		<%=Post.DateCreated.ToShortDateString() + " " + Post.DateCreated.ToShortTimeString() %> | 
		<%=Resources.labels.comments %> (<%=Post.ApprovedComments.Count %>)
	</div>
    <asp:PlaceHolder ID="BodyContent" runat="server" />
    
	<div class="rating"><%=Rating %></div>
	<div class="share">
		<span>
		    <a rel="bookmark" href="<%=Post.PermaLink %>">Permalink</a> | 
		    <a rel="nofollow" href="<%=CommentFeed %>">Comments RSS</a>
		</span>
		Share it: 
        <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | 
        <a rel="nofollow" href="http://www.dotnetkicks.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">Kick it!</a> | 
        <a rel="nofollow" href="http://www.dzone.com/links/add.html?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">DZone it!</a> | 
        <a rel="nofollow" href="http://del.icio.us/post?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">del.icio.us</a>      
	</div>
</div>