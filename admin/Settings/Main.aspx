﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="admin.Settings.Main" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var frm = document.forms.aspnetForm;
            $(frm).validate({
                onsubmit: false
            });

            $("#btnSave").click(function (evt) {
                if ($(frm).valid())
                    SaveSettings();

                evt.preventDefault();
            });

            $("#<%=cbShowDescriptionInPostListForPostsByTagOrCategory.ClientID %>").change(function () {
                $("#DescriptionCharactersForPostsByTagOrCategory").toggle();
            });

            $("#<%=cbShowDescriptionInPostList.ClientID %>").change(function () {
                $("#DescriptionCharacters").toggle();
            });

        });
        function PreviewTheme() {
            var theme = document.getElementById('<%=ddlTheme.ClientID %>').value;
            var path = '../../?theme=' + theme;
            window.open(path);
        }
        function SaveSettings() {
            $('.loader').show();
            var dto = { 
				"name": $("[id$='_txtName']").val(),
				"desc": $("[id$='_txtDescription']").val(),
				"postsPerPage": $("[id$='_txtPostsPerPage']").val(),
				"theme": $("[id$='_ddlTheme']").val(),
				"mobileTheme": $("[id$='_ddlMobileTheme']").val(),
				"themeCookieName": $("[id$='_txtThemeCookieName']").val(),
				"useBlogNameInPageTitles": $("[id$='_cbUseBlogNameInPageTitles']").attr('checked'),
				"enableRelatedPosts": $("[id$='_cbShowRelatedPosts']").attr('checked'),
				"enableRating": $("[id$='_cbEnableRating']").attr('checked'),
				"showDescriptionInPostList": $("[id$='_cbShowDescriptionInPostList']").attr('checked'),
				"descriptionCharacters": $("[id$='_txtDescriptionCharacters']").val(),
				"showDescriptionInPostListForPostsByTagOrCategory": $("[id$='_cbShowDescriptionInPostListForPostsByTagOrCategory']").attr('checked'),
				"descriptionCharactersForPostsByTagOrCategory": $("[id$='_txtDescriptionCharactersForPostsByTagOrCategory']").val(),
				"timeStampPostLinks": $("[id$='_cbTimeStampPostLinks']").attr('checked'),
				"showPostNavigation": $("[id$='_cbShowPostNavigation']").attr('checked'),
				"culture": $("[id$='_ddlCulture']").val(),
				"timezone": $("[id$='_txtTimeZone']").val(),
				"enableSelfRegistration": $("[id$='_cbEnableSelfRegistration']").attr('checked'),
				"selfRegistrationInitialRole": $("[id$='_ddlSelfRegistrationInitialRole']").val()
			};
			
            $.ajax({
                url: "Main.aspx/Save",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dto),
                success: function (result) {
                    var rt = result.d;
                    if (rt.Success)
                        ShowStatus("success", rt.Message);
                    else
                        ShowStatus("warning", rt.Message);
                }
            });
            $('.loader').hide();
            return false;
        }  
    </script>
    
 
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1 ><%=Resources.labels.basic %> <%=Resources.labels.settings %></h1>

                <ul class="fl leftaligned">
                    <li>
                        <label class="lbl" for="<%=txtName.ClientID %>"><%=Resources.labels.name %></label>
                        <asp:TextBox width="300" runat="server" ID="txtName" CssClass="required" /></li>
                    <li>
                        <label class="lbl" for="<%=txtDescription.ClientID %>"><%=Resources.labels.description %></label>
                        <asp:TextBox width="300" runat="server" ID="txtDescription" />
                    </li>
                    <li>
                        <label class="lbl" for="<%=ddlTheme.ClientID %>"><%=Resources.labels.theme %></label>
                        <asp:DropDownList CssClass="txt" Width="212" runat="server" ID="ddlTheme" />
                        <a href="javascript:void(PreviewTheme());"><%=Resources.labels.preview %></a> | <a href="http://www.dotnetblogengine.net/page/themes.aspx" target="_blank"><%=Resources.labels.download %></a>
                    </li>
                    <li>
                        <label class="lbl" for="<%=ddlMobileTheme.ClientID %>"><%=Resources.labels.mobileTheme %></label>
                        <asp:DropDownList CssClass="txt" Width="212" runat="server" ID="ddlMobileTheme" />
                    </li>
                    <li>
                        <label class="lbl" for="<%=txtThemeCookieName.ClientID %>"><%=Resources.labels.themeCookieName %></label>
                        <asp:TextBox CssClass="w300" runat="server" ID="txtThemeCookieName" />
                    </li>
                    <li>
                        <label class="lbl" for="<%=ddlCulture.ClientID %>"><%=Resources.labels.language %></label>
                        <asp:DropDownList runat="Server" ID="ddlCulture" Style="text-transform: capitalize">
                            <asp:ListItem Text="Auto" />
                            <asp:ListItem Text="english" Value="en" />
                        </asp:DropDownList>
                    </li>
                    <li>
                        <label class="lbl" for="<%=txtTimeZone.ClientID %>"><%=Resources.labels.timezone %></label>
                        <asp:TextBox runat="Server" ID="txtTimeZone" Width="30" CssClass="number" />
                        <span>Server time: <%=DateTime.Now.ToShortTimeString() %></span>
                    </li>
                    <li>
                        <label class="lbl" for="<%=txtPostsPerPage.ClientID %>"><%=Resources.labels.postPerPage %></label>
                        <asp:TextBox runat="server" ID="txtPostsPerPage" Width="50" MaxLength="4" CssClass="required number" />
                    </li>
                    <li>
                        <label class="lbl">Appearance</label>
                        <asp:CheckBox runat="server" ID="cbShowDescriptionInPostList" />
                        <label for="<%=cbShowDescriptionInPostList.ClientID %>"><%=Resources.labels.showDescriptionInPostList %></label>
                        <div class="insetForm" id="DescriptionCharacters" style=" display:none;">
                            <label for="<%=txtDescriptionCharacters.ClientID %>"><%=Resources.labels.numberOfCharacters %></label>
                            <asp:TextBox runat="server" ID="txtDescriptionCharacters" Width="40" CssClass="number" />      
                        </div>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbShowDescriptionInPostListForPostsByTagOrCategory" />
                        <label for="<%=cbShowDescriptionInPostListForPostsByTagOrCategory.ClientID %>"><%=Resources.labels.showDescriptionInPostListForPostsByTagOrCategory %></label>
                        <div class="insetForm" id="DescriptionCharactersForPostsByTagOrCategory" style=" display:none;">
                            <label for="<%=txtDescriptionCharactersForPostsByTagOrCategory.ClientID %>" style="float: none; position: relative; top: -2px;"><%=Resources.labels.numberOfCharacters %></label>
                            <asp:TextBox runat="server" ID="txtDescriptionCharactersForPostsByTagOrCategory" Width="40" CssClass="number" />
                        </div>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbShowRelatedPosts" />
                        <label for="<%=cbShowRelatedPosts.ClientID %>"><%=Resources.labels.showRelatedPosts %></label>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbShowPostNavigation" />
                        <label for="<%=cbShowPostNavigation.ClientID %>"><%=Resources.labels.showPostNavigation %></label>
                    </li>
                    <li>
                        <label class="lbl">Other settings</label>
                        <asp:CheckBox runat="server" ID="cbUseBlogNameInPageTitles" />
                        <label for="<%=cbUseBlogNameInPageTitles.ClientID %>"><%=Resources.labels.useBlogNameInPageTitles%></label>
                        <span class="insetHelp">(<%=Resources.labels.useBlogNameInPageTitlesDescription%>)</span>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbEnableRating" />
                        <label for="<%=cbEnableRating.ClientID %>"><%=Resources.labels.enableRating %></label>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbTimeStampPostLinks" />
                        <label for="<%=cbTimeStampPostLinks.ClientID %>"><%=Resources.labels.timeStampPostLinks %></label>
                    </li>
                    <li>
                        <span class="filler"></span>
                        <asp:CheckBox runat="server" ID="cbEnableSelfRegistration" />
                        <label for="<%=cbEnableSelfRegistration.ClientID %>"><%=Resources.labels.enableSelfRegistration %></label>
                    </li>
                    <li>
                        <label class="lbl" for="<%=ddlSelfRegistrationInitialRole.ClientID %>"><%=Resources.labels.selfRegistrationInitialRole%></label>
                        <asp:DropDownList runat="Server" ID="ddlSelfRegistrationInitialRole" Style="text-transform: capitalize">
                            <asp:ListItem Text="Select" />
                        </asp:DropDownList>
                    </li>
                </ul>

            <div class="action_buttons">
                <input type="submit" id="btnSave" class="btn primary" value="Save settings" />
            </div>
		</div>
	</div>    
</asp:Content>