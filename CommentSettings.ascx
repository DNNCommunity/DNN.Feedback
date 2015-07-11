<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="CommentSettings.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Feedback.CommentSettings" %>
<div class="dnnForm">
    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary"
        EnableClientScript="true" DisplayMode="List" />
    <div class="dnnFormItem">
        <dnn:label id="plScope" runat="server" controlname="rblScope" suffix=":"></dnn:label>
		<asp:RadioButtonList ID="rblScope" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Vertical" AutoPostBack="true">
			<asp:ListItem Text="Selected Modules Only" resourcekey="SelectedModules" Value="1"></asp:ListItem>
			<asp:ListItem Text="Portal (all Feedback modules of portal)" resourcekey="Portal" Value="2"></asp:ListItem>
		</asp:RadioButtonList>
	</div>
    <div id="divFeedbackModules" runat="server" class="dnnFormItem">
        <dnn:label id="plFeedbackModules" runat="server" controlname="gvFeedbackModules" suffix=":" />
        <asp:GridView ID="gvFeedbackModules" runat="server" DataKeyNames="ModuleId"  CellPadding="2" AutoGenerateColumns="false" ShowHeader="True">
            <RowStyle Font-Names="Arial" Font-Size="10px" />
            <HeaderStyle Font-Names="Arial" Font-Size="8px" Font-Bold="True" />
            <Columns>
                <asp:BoundField HeaderText="TabName" DataField="TabName" ReadOnly="True" />
                <asp:BoundField HeaderText="ModuleTitle" DataField="ModuleTitle" ReadOnly="True" />
                <asp:TemplateField HeaderText="Selected" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelected" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Selected") %>' />
                    </ItemTemplate>
                </asp:TemplateField>             
            </Columns>            
        </asp:GridView>
	</div>
    <div id="divCategories" runat="server" class="dnnFormItem">
        <dnn:label id="plCategories" runat="server" controlname="cblCategories" suffix=":" />
		<asp:CheckBoxList id="cblCategories" runat="server" cssclass="dnnFormRadioButtons" RepeatDirection="Horizontal" RepeatColumns="2" />
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plHeaderTemplate" runat="server" ControlName="txtHeaderTemplate" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtHeaderTemplate" CssClass="NormalTextBox" Width="350" Columns="30" TextMode="MultiLine"
            Rows="10" MaxLength="2000" runat="server" /><br />
        <asp:LinkButton ID="cmdLoadDefaultHeaderTemplate" runat="server" CausesValidation="False" CssClass="commandButton" resourcekey="LoadDefault" >Load Default</asp:LinkButton>
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plItemTemplate" runat="server" ControlName="txtItemTemplate" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtItemTemplate" CssClass="NormalTextBox" Width="350" Columns="30" TextMode="MultiLine"
            Rows="10" MaxLength="2000" runat="server" /><br />
        <asp:LinkButton ID="cmdLoadDefaultItemTemplate" runat="server" CausesValidation="False" CssClass="commandButton" resourcekey="LoadDefault">Load Default</asp:LinkButton>
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plAltItemTemplate" runat="server" ControlName="txtAltItemTemplate" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtAltItemTemplate" CssClass="NormalTextBox" Width="350" Columns="30" TextMode="MultiLine"
            Rows="10" MaxLength="2000" runat="server" /><br />
        <asp:LinkButton ID="cmdLoadDefAltItemTemplate" runat="server" CausesValidation="False" CssClass="commandButton" resourcekey="LoadDefault">Load Default</asp:LinkButton>
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plSeparatorTemplate" runat="server" ControlName="txtSeparatorTemplate" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtSeparatorTemplate" CssClass="NormalTextBox" Width="350" Columns="30" TextMode="MultiLine"
            Rows="10" MaxLength="2000" runat="server" /><br />
        <asp:LinkButton ID="cmdLoadDefaultSeparatorTemplate" runat="server" CausesValidation="False" CssClass="commandButton" resourcekey="LoadDefault">Load Default</asp:LinkButton>
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plFooterTemplate" runat="server" ControlName="txtFooterTemplate" Suffix=":"></dnn:Label>
        <asp:TextBox ID="txtFooterTemplate" CssClass="NormalTextBox" Width="350" Columns="30" TextMode="MultiLine"
            Rows="10" MaxLength="2000" runat="server" /><br />
        <asp:LinkButton ID="cmdLoadDefaultFooterTemplate" runat="server" CausesValidation="False" CssClass="commandButton" resourcekey="LoadDefault">Load Default</asp:LinkButton>
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plAvailableTokens" runat="server" ControlName="lblAvailableTokens" Suffix=":" />
        <table><tr><td>
            <asp:Label id="lblAvailableTokens" runat="server" ResourceKey="lblAvailableTokens"></asp:Label>
        </td></tr></table>
	</div>
    <div class="dnnFormItem">
        <dnn:label id="plEnablePager" runat="server" controlname="chkEnablePager" suffix=":" />
		<asp:CheckBox id="chkEnablePager" runat="server" AutoPostBack="True" />
	</div>
    <div class="dnnFormItem">
        <dnn:Label ID="plDefaultPageSize" runat="server" ControlName="txtDefaultPageSize" Suffix=":" />
	    <asp:TextBox ID="txtDefaultPageSize" runat="server" Width="50px" CssClass="NormalTextBox"></asp:TextBox>
	    <asp:RangeValidator ID="valDefaultPageSize" runat="server" ControlToValidate="txtDefaultPageSize" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"
	            ErrorMessage="Must be number 5 to 25" MinimumValue="5" MaximumValue="25" Type="Integer" ValidationGroup="CommentSettings"></asp:RangeValidator>
	</div>
</div>
