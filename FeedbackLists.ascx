<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Codebehind="FeedbackLists.ascx.vb" Inherits="DotNetNuke.Modules.Feedback.FeedbackLists" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="dnnForm FeedbackLists">
    <div class="dnnFormItem fbListType">
	    <dnn:label id="plListType" runat="server" controlname="rbListType" Suffix=":"/>
	   	<asp:RadioButtonList ID="rbListType" runat="server" RepeatDirection="Horizontal" cssclass="dnnFormRadioButtons" AutoPostBack="true">
		    <asp:ListItem  Value="1" Selected="true" resourceKey="Categories"/>
		    <asp:ListItem  Value="2" resourceKey="Subjects"/>
		</asp:RadioButtonList>
    </div>
    <div id="divFeedback_Lists"> 
        <table cellspacing="0" class="Feedback_Lists" cellpadding="4" border="0" width="100%" summary="Feedback Lists Table">
            <tr>
                <td valign="top">
                    <asp:DataGrid ID="dgItems" runat="server"
                        AutoGenerateColumns="false" width="100%" 
                        CellPadding="2" GridLines="None" cssclass="DataGrid_Container" >
                        <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="Center"/>
                        <itemstyle cssclass="Normal" horizontalalign="center" />
                        <alternatingitemstyle cssclass="Normal" />
                        <edititemstyle cssclass="NormalTextBox" />
                        <selecteditemstyle cssclass="NormalRed" />
                        <footerstyle cssclass="DataGrid_Footer" />
                        <pagerstyle cssclass="DataGrid_Pager" />
                        <Columns>
                            <dnn:imagecommandcolumn CommandName="MoveUp" IconKey="Up" EditMode="URL" KeyField="ListID" />
		                    <dnn:imagecommandcolumn CommandName="MoveDown" IconKey="Dn" EditMode="URL" KeyField="ListID" />
		                    <dnn:imagecommandcolumn CommandName="Edit" IconKey="Edit" EditMode="URL" KeyField="ListID" />
		                    <dnn:imagecommandcolumn Commandname="Delete" IconKey="Delete" keyfield="ListID" />
	                        <dnn:textcolumn  datafield="Name" HeaderText="Name" />
		                    <dnn:textcolumn datafield="ListValue" HeaderText="ListValue" HeaderStyle-CssClass="NormalBold Listcolumn" />
		                    <asp:TemplateColumn HeaderText="IsActive">
						        <HeaderStyle HorizontalAlign="center" CssClass="NormalBold Listcolumn"></HeaderStyle>
						        <ItemStyle HorizontalAlign="center" CssClass="Normal"></ItemStyle>
						        <ItemTemplate>
							        <asp:Image runat="server" AlternateText='' IconKey='<%# IIf(DataBinder.Eval(Container.DataItem, "IsActive") = 1, "Checked", "Unchecked") %>' ID="Image2"/>
						        </ItemTemplate>
						    </asp:TemplateColumn>
		                    <asp:TemplateColumn HeaderText="Portal">
						        <HeaderStyle HorizontalAlign="center" CssClass="NormalBold Listcolumn"></HeaderStyle>
						        <ItemStyle HorizontalAlign="center" CssClass="Normal"></ItemStyle>
						        <ItemTemplate>
							        <asp:Image runat="server" AlternateText='' IconKey='<%# IIf(DataBinder.Eval(Container.DataItem, "Portal") = True, "Checked", "Unchecked") %>' ID="Image3"/>
						        </ItemTemplate>
						    </asp:TemplateColumn>
	                        <dnn:textcolumn  datafield="ModuleID" HeaderText="ModuleID" Visible="False" />
		                </Columns>
		            </asp:DataGrid>
                </td>
                <td style="width:60%;">
                    <div class="dnnFormItem">
                        <dnn:label id="plListName" runat="server" controlname="txtBoxListName"/>
                        <asp:HiddenField ID="txtListID" runat="server" />
                        <asp:TextBox width="150px" ID="txtBoxListName" runat="server" TextMode="SingleLine" MaxLength="50" cssclass="NormalTextBox" />
                        <asp:requiredfieldvalidator id="valtxtBoxListName" runat="server" cssclass="dnnFormMessage dnnFormError" display="Dynamic"
    	                    controltovalidate="txtBoxListName" resourcekey="valtxtBoxListName" ValidationGroup="FeedbackList" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label id="plListValue" runat="server" controlname="txtBoxListValue"/>
                        <asp:TextBox ID="txtBoxListValue" width="150px" runat="server" TextMode="SingleLine" MaxLength="100" cssclass="NormalTextBox" />
                        <asp:requiredfieldvalidator id="valtxtBoxListValue" runat="server" cssclass="dnnFormMessage dnnFormError" display="Dynamic"
				            controltovalidate="txtBoxListValue" resourcekey="valtxtBoxListValue" ValidationGroup="FeedbackList" />  
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label id="plIsActive" runat="server" controlname="checkBoxIsActive"/>
                        <asp:Checkbox ID="checkBoxIsActive" runat="server" ></asp:Checkbox>
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label id="plPortal" runat="server" controlname="checkBoxPortal"/>
                        <asp:HiddenField ID="txtModuleID" runat="server" />
                        <asp:Checkbox ID="checkBoxPortal" runat="server" ></asp:Checkbox>
                    </div>
                </td>
           </tr>
        </table>
    </div>
    <div id="divErrorRow" runat="server" visible="false">
        <asp:label id="plErrorMsg" runat="server" resourcekey="plErrorMsg" cssclass="dnnFormMessage dnnFormWarning"></asp:label>
    </div>
    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton id="cmdSaveEntry" ValidationGroup="FeedbackList" resourcekey="cmdSave" runat="server" cssclass="dnnPrimaryAction" causesvalidation="True" /></li>
	    <li><asp:linkbutton id="cmdReturn" ValidationGroup="FeedbackList" resourcekey="cmdReturn" runat="server" cssclass="dnnSecondaryAction" causesvalidation="False" /></li>
    </ul>

</div>