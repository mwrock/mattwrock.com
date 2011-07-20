<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" ValidateRequest="False" CodeFile="Posts.aspx.cs" Inherits="Admin.Posts.Posts" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript" src="../jquery.tipsy.js"></script>
    <script type="text/javascript">
        LoadPostsForPage(1);
        $(document).ready(function () {
            $(".tableToolBox a").click(function () {
                $(".tableToolBox a").removeClass("current");
                $(this).addClass("current");
            });
        });
    </script>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1>Posts</h1>
            <div class="tableToolBox">
                Show : <a class="current" href="#" onclick="ChangePostFilterType('All')">All</a> | 
                <a href="#" onclick="ChangePostFilterType('Draft')">Drafts</a> | 
                <a href="#" onclick="ChangePostFilterType('Published')">Published</a>
                <span id="filteredby"></span>
                <div class="Pager"></div>
            </div>
            <div id="Container"></div>
            <div class="Pager"></div>
        </div>
    </div>
</asp:Content>
