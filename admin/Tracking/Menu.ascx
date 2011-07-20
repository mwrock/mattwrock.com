<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin.Comments.Menu" %>
<%@ Import Namespace="BlogEngine.Core" %>
<ul>
    <li <%=Current("Pingbacks.aspx")%>><a href="Pingbacks.aspx">Pingbacks & Trackbacks</a></li>
    <li <%=Current("referrers.aspx")%>><a href="referrers.aspx">Referrers</a></li>
</ul>