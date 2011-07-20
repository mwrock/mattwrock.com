<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin.Settings.Menu" %>
<ul>
    <li <%=Current("Blogroll.aspx")%>><a href="Blogroll.aspx">Blogroll</a></li>
    <li <%=Current("Controls.aspx")%>><a href="Controls.aspx">Common controls</a></li>
</ul>