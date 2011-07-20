<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="Admin.ExtensionManager.Default" Title="Extensions" %>
<%@ Reference Control = "Extensions.ascx" %>
<%@ Reference Control = "Editor.ascx" %>
<%@ Reference Control = "Settings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <div class="content-box-outer">
        <div class="content-box-full">
            <asp:PlaceHolder ID="ucPlaceHolder" runat="server"></asp:PlaceHolder>
            <asp:HiddenField ID="args" runat="server" />
        </div>
    </div>
</asp:Content>
