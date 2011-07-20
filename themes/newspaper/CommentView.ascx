<%@ Control Language="C#" EnableViewState="False" Inherits="BlogEngine.Core.Web.Controls.CommentViewBase" %>
<div id="id_<%=Comment.Id %>" class="comment<%= Post.Author.Equals(Comment.Author, StringComparison.OrdinalIgnoreCase) ? " self" : "" %>">
    <div class="gravatar">
        <%= Gravatar(80)%></div>
    <div class="text">
        <span id="header">
            <span class="author"><%= Comment.Website != null ? "<a href=\"" + Comment.Website + "\">" + Comment.Author + "</a>" : Comment.Author %></span>
            <%= Flag %>
            on
            <%= Comment.DateCreated.ToShortDateString() + " " + Comment.DateCreated.ToShortTimeString() %><%= AdminLinks %>
        </span>
        <%= Text%>
    </div>
</div>
