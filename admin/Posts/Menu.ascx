<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin.Settings.Menu" %>
<ul>
    <li <%=Current("Add_entry.aspx")%>><a href="Add_entry.aspx" class="new">Write new post</a></li>
    <li <%=Current("Posts.aspx")%>><a href="Posts.aspx">Posts</a></li>
    <li <%=Current("Categories.aspx")%>><a href="Categories.aspx">Categories</a></li>
    <li <%=Current("Tags.aspx")%>><a href="Tags.aspx">Tags</a></li>
</ul>