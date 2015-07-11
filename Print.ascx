<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Print.ascx.vb" Inherits="DotNetNuke.Modules.Feedback.Print" %>

<div class="Feedback_PrinterPage">
  <asp:Literal ID="litContents" runat="server"></asp:Literal>
</div>
<div style="width:100%; margin-top: 15px; margin-bottom: 10px; text-align:center">
    <asp:Button ID="btnClose" runat="server" Text="Close" />
</div>