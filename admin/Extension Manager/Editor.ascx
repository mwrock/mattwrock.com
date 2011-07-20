<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/admin/Extension Manager/Editor.ascx.cs" Inherits="Admin.ExtensionManager.UserControlsXmanagerSourceEditor" %>
<h1><%=Resources.labels.sourceViewer %>: <%=ExtensionName%></h1>
<div>
    <asp:TextBox ID="txtEditor" runat="server" TextMode="multiLine" Width="100%" Height="320"></asp:TextBox>
</div>
<div style="padding:5px 0 0 0">
    <asp:Button ID="btnSave" Visible="true" runat="server" Text="Save" OnClick="BtnSaveClick"  />
    <span style="padding-left:10px">[<%=GetExtFileName() %>]</span>
</div>