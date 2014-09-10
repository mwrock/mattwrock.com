<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainHeader.ascx.cs" Inherits="MichaelJBaird.Themes.JQMobile.Controls.MainHeader" %>
<%@ Import Namespace="BlogEngine.Core" %>

<div data-role="header">
  <h1><%=BlogSettings.Instance.Name %> - <%= BlogSettings.Instance.Description %></h1>
  <a href="<%=Utils.RelativeWebRoot%>default.aspx" data-icon="home" data-iconpos="notext"><%= Resources.labels.home %></a>
  <div data-role="navbar" data-iconpos="top">
	  <ul>
      <li><a href="<%=Utils.RelativeWebRoot%>archive<%= BlogConfig.FileExtension %>" data-icon="grid" data-transition="slide"><%= Resources.labels.archive %></a></li>
		  <li><a href="<%=Utils.RelativeWebRoot%>contact<%= BlogConfig.FileExtension %>" id="contact" data-icon="custom" data-transition="slide" data-ajax="false"><%= Resources.labels.contact %></a></li>
		  <li><a href="<%=Utils.RelativeWebRoot%>search<%= BlogConfig.FileExtension %>" data-icon="search" data-transition="slide"><%= Resources.labels.search %></a></li>
		  <li><a href="<%=Utils.FeedUrl%>" id="rss" data-icon="custom" data-transition="slide"><%= Resources.labels.feed %></a></li>
	  </ul>
  </div>
</div>