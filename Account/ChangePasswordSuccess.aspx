<%@ Page Title="Change Password" Language="C#" MasterPageFile="Account.master" AutoEventWireup="true" CodeFile="ChangePasswordSuccess.aspx.cs" Inherits="Account.ChangePasswordSuccess" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        <%=Resources.labels.changePassword %>
    </h2>
    <div style="padding: 25px 0">
        <div id="ChangePwd" class="success">
            <%=Resources.labels.passwordChangeSuccess %> <a href="javascript:Hide('ChangePwd')" style="width:20px;float:right">X</a>
        </div>
    </div>
</asp:Content>