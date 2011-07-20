<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<%if (this.Location == BlogEngine.Core.ServingLocation.SinglePost )
        { %>
<div id="postpath">
    
	<a title="" href="<%=BlogEngine.Core.Utils.AbsoluteWebRoot %>"><%=Resources.labels.home %></a>
	 &gt; <%=CategoryLinks(", ") %>
</div>
<%} %>	
<div class="post xfolkentry">
    <h2><a href="<%=Post.RelativeLink %>" class="title"><%=Server.HtmlEncode(Post.Title) %></a></h2>
    <div class="info">
        <span class="date">
            <%=Post.DateCreated.ToString("d. MMMM yyyy") %></span>
        <div class="act">
            <span class="comments"><a rel="nofollow" href="<%=Post.RelativeLink %>#comment">
                <%=Resources.labels.comments %>
                (<%=Post.ApprovedComments.Count %>)</a></span>
                <%if (AdminLinks.Length > 0)
                  { %>
            <span class="editpost"><%=AdminLinks%></span> 
            <%} %>
            <div class="fixed">
            </div>
        </div>
        <div class="fixed">
        </div>
    </div>
    <div class="content"><asp:PlaceHolder ID="BodyContent" runat="server" />
<%=Rating %>
<p class="under">
	<span class="categories"><%=CategoryLinks(", ") %></span>
	<span class="tags"><%=TagLinks(", ") %></span>
</p>

<div class="fixed"></div>
</div>

</div>