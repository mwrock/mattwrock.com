<%@ Control Language="C#" EnableViewState="False" Inherits="BlogEngine.Core.Web.Controls.CommentViewBase" %>

<div id="id_<%=Comment.Id %>" class="vcard comment<%= Post.Author.Equals(Comment.Author, StringComparison.OrdinalIgnoreCase) ? " admincomment" : "" %>">
  <div class="author">
  <div class="pic"><%= Gravatar(32)%></div> 
  <div class="name"><%= Comment.Website != null ? "<a href=\"" + Comment.Website + "\" class=\"url fn\">" + Comment.Author + "</a>" : "<span class=\"fn\">" +Comment.Author + "</span>" %></div> 
  </div> 
  <div class="info">
    <div class="date"><%= Comment.DateCreated %> <a href="#id_<%=Comment.Id %>">#</a></div>
    <div class="act"><%= AdminLinks %></div> 
    
  
  <div class="fixed"></div> 
  <div class="content"><%= Text %></div>
  </div> 
    <div class="fixed"></div> 
</div>
