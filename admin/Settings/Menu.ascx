﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="admin.Settings.Menu" %>
<ul>
    <li <%=Current("Main.aspx")%>><a href="Main.aspx"><%=Resources.labels.basic %></a></li>
    <li <%=Current("Advanced.aspx")%>><a href="Advanced.aspx"><%=Resources.labels.advanced %></a></li>
    <li <%=Current("Feed.aspx")%>><a href="Feed.aspx">Feed</a></li>
    <li <%=Current("Email.aspx")%>><a href="Email.aspx"><%=Resources.labels.email %></a></li>
    <li <%=Current("HeadTrack.aspx")%>><a href="HeadTrack.aspx">Custom code</a></li>
    <li <%=Current("Comments.aspx")%>><a href="Comments.aspx"><%=Resources.labels.comments %></a></li>
    <li <%=Current("Rules.aspx")%>><a href="Rules.aspx"><%=Resources.labels.comments %> <%=Resources.labels.rules %> & <%=Resources.labels.filters %></a></li>
    <li <%=Current("PingServices.aspx")%>><a href="PingServices.aspx">Ping Services</a></li>
    <li <%=Current("Import.aspx")%>><a href="Import.aspx"><%=Resources.labels.import %> & <%=Resources.labels.export %></a></li>
</ul>