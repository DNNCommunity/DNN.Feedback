<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Moderation.ascx.vb" Inherits="DotNetNuke.Modules.Feedback.Moderation" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<div class="dnnForm FeedbackModeration dnnClear" id="FeedbackModeration"> 
    <div class="dnnFormItem">
        <asp:CheckBox ID="cbShowOnlyModeratedCategories" cssclass="dnnFormRadioButtons" TextAlign="Right" runat="server" Text="Show feedback only in unmoderated categories" ResourceKey="cbShowOnlyModeratedCategories" AutoPostBack="true" />
    </div>
    <h2 class="dnnFormSectionHead" id="dnnPanel-PendingFeedback"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnPendingFeedback")%></a></h2>
	<fieldset>
        <asp:DataGrid ID="dgPendingFeedback" runat="server" AutoGenerateColumns="false" AllowSorting="True" 
                CellPadding="6" GridLines="None" cssclass="DataGrid_Container" >
            <headerstyle cssclass="NormalBold" verticalalign="Top" Horizontalalign="left"/>
            <itemstyle cssclass="Normal" horizontalalign="left" />
            <edititemstyle cssclass="NormalTextBox" />
            <selecteditemstyle cssclass="NormalRed" />
            <Columns>
            <asp:BoundColumn DataField="FeedbackID" Visible = "false" ReadOnly="true"></asp:BoundColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPublic" CommandName="StatusPublic"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPrivate" CommandName="StatusPrivate"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetArchive" CommandName="StatusArchive"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetSpam" CommandName="StatusSpam"></asp:ButtonColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdPrint" runat="server" CommandName="Print" AlternateText="Print" IconKey="Print" />
	            </ItemTemplate>
	        </asp:TemplateColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" AlternateText="Delete" IconKey="Delete" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" IconKey="Cancel" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" AlternateText="Edit" IconKey="Edit" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdUpdate" runat="server" CommandName="Update" AlternateText="Update" IconKey="Save" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="CategoryName" HeaderText="Category" ReadOnly="true"></asp:BoundColumn>
		    <asp:TemplateColumn HeaderText="Subject" ItemStyle-Width="200px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="Normal" ID="lblSubject" Width="200px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="NormalTextBox" ID="txtSubject" Width="200px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:TemplateColumn HeaderText="Message" ItemStyle-Width="250px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="Normal" ID="lblMessage" Width="250px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="NormalTextBox" ID="txtMessage" Rows="10" Width="250px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:BoundColumn DataField="SenderName" HeaderText="Author" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="SenderEmail" HeaderText="From Email" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="DisplayCreatedOnDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="CreatedOnDate"></asp:BoundColumn>
		    </Columns>
		</asp:DataGrid>
		<dnn:PagingControl id="pgPendingFeedback" runat="server" Mode="PostBack"></dnn:PagingControl>             
     </fieldset>
    <h2 class="dnnFormSectionHead" id="dnnPanel-PrivateFeedback"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnPrivateFeedback")%></a></h2>
	<fieldset>
        <asp:DataGrid ID="dgPrivateFeedback" runat="server" AutoGenerateColumns="false" AllowSorting="True" 
                CellPadding="6" GridLines="None" cssclass="DataGrid_Container" >
            <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="left"/>
            <itemstyle cssclass="Normal" horizontalalign="left" />
            <alternatingitemstyle cssclass="Normal" />
            <edititemstyle cssclass="NormalTextBox" />
            <selecteditemstyle cssclass="NormalRed" />
            <footerstyle cssclass="DataGrid_Footer" />
            <pagerstyle cssclass="DataGrid_Pager" />
            <Columns>
            <asp:BoundColumn DataField="FeedbackID" Visible = "false" ReadOnly="true"></asp:BoundColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPublic" CommandName="StatusPublic"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetArchive" CommandName="StatusArchive"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetSpam" CommandName="StatusSpam"></asp:ButtonColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdPrint" runat="server" CommandName="Print" AlternateText="Print" IconKey="Print" />
	            </ItemTemplate>
	        </asp:TemplateColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" AlternateText="Delete" IconKey="Delete" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" IconKey="Cancel" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" AlternateText="Edit" IconKey="Edit" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdUpdate" runat="server" CommandName="Update" AlternateText="Update" IconKey="Save" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="CategoryName" HeaderText="Category" ReadOnly="true"></asp:BoundColumn>
		    <asp:TemplateColumn HeaderText="Subject" ItemStyle-Width="200px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="Normal" ID="lblSubject" Width="200px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="NormalTextBox" ID="txtSubject" Width="200px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:TemplateColumn HeaderText="Message" ItemStyle-Width="250px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="Normal" ID="lblMessage" Width="250px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="NormalTextBox" ID="txtMessage" Rows="10" Width="250px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:BoundColumn DataField="SenderName" HeaderText="Author" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="SenderEmail" HeaderText="From Email" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="DisplayCreatedOnDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="CreatedOnDate"></asp:BoundColumn>
		    </Columns>
		</asp:DataGrid>
		<dnn:PagingControl id="pgPrivateFeedback" runat="server" Mode="PostBack"></dnn:PagingControl>
     </fieldset>
    <h2 class="dnnFormSectionHead" id="dnnPanel-PublicFeedback"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnPublishedFeedback")%></a></h2>
	<fieldset>
        <asp:DataGrid ID="dgPublicFeedback" runat="server" AutoGenerateColumns="false" AllowSorting="True"   
                CellPadding="6" GridLines="None" cssclass="DataGrid_Container" width="100%">
            <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="left"/>
            <itemstyle cssclass="Normal" horizontalalign="left" />
            <alternatingitemstyle cssclass="Normal" />
            <edititemstyle cssclass="NormalTextBox" />
            <selecteditemstyle cssclass="NormalRed" />
            <footerstyle cssclass="DataGrid_Footer" />
            <pagerstyle cssclass="DataGrid_Pager" />
            <Columns>
            <asp:BoundColumn DataField="FeedbackID" Visible = "false" ReadOnly="true"></asp:BoundColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPrivate" CommandName="StatusPrivate"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetArchive" CommandName="StatusArchive"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetSpam" CommandName="StatusSpam"></asp:ButtonColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdPrint" runat="server" CommandName="Print" AlternateText="Print" IconKey="Print" />
	            </ItemTemplate>
	        </asp:TemplateColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" AlternateText="Delete" IconKey="Delete" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" IconKey="Cancel" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" AlternateText="Edit" IconKey="Edit" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdUpdate" runat="server" CommandName="Update" AlternateText="Update" IconKey="Save" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="CategoryName" HeaderText="Category" ReadOnly="true"></asp:BoundColumn>
		    <asp:TemplateColumn HeaderText="Subject" ItemStyle-Width="200px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="Normal" ID="lblSubject" Width="200px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="NormalTextBox" ID="txtSubject" Width="200px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:TemplateColumn HeaderText="Message" ItemStyle-Width="250px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="Normal" ID="lblMessage" Width="250px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="NormalTextBox" ID="txtMessage" Rows="10" Width="250px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:BoundColumn DataField="SenderName" HeaderText="Author" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="SenderEmail" HeaderText="From Email" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="DisplayCreatedOnDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="CreatedOnDate"></asp:BoundColumn>
		    </Columns>
		</asp:DataGrid>
		<dnn:PagingControl id="pgPublicFeedback" runat="server" Mode="PostBack" />
     </fieldset>
    <h2 class="dnnFormSectionHead" id="dnnPanel-ArchiveFeedback"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnArchivedFeedback")%></a></h2>
	<fieldset>
        <asp:DataGrid ID="dgArchiveFeedback" runat="server" AutoGenerateColumns="false" width="100%" 
                CellPadding="6" GridLines="None" cssclass="DataGrid_Container" AllowSorting="true" >
            <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="left"/>
            <itemstyle cssclass="Normal" horizontalalign="left" />
            <alternatingitemstyle cssclass="Normal" />
            <edititemstyle cssclass="NormalTextBox" />
            <selecteditemstyle cssclass="NormalRed" />
            <footerstyle cssclass="DataGrid_Footer" />
            <pagerstyle cssclass="DataGrid_Pager" />
            <Columns>
            <asp:BoundColumn DataField="FeedbackID" Visible = "false" ReadOnly="true"></asp:BoundColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPublic" CommandName="StatusPublic"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetPrivate" CommandName="StatusPrivate"></asp:ButtonColumn>
            <asp:ButtonColumn ButtonType="LinkButton" Text="SetSpam" CommandName="StatusSpam"></asp:ButtonColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdPrint" runat="server" CommandName="Print" AlternateText="Print" IconKey="Print" />
	            </ItemTemplate>
	        </asp:TemplateColumn>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" AlternateText="Delete" IconKey="Delete" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" IconKey="Cancel" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
	            <ItemTemplate>
	                <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" AlternateText="Edit" IconKey="Edit" />
	            </ItemTemplate>
	            <EditItemTemplate>
	                <asp:ImageButton ID="cmdUpdate" runat="server" CommandName="Update" AlternateText="Update" IconKey="Save" />
	            </EditItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="CategoryName" HeaderText="Category" ReadOnly="true"></asp:BoundColumn>
		    <asp:TemplateColumn HeaderText="Subject" ItemStyle-Width="200px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="Normal" ID="lblSubject" Width="200px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="NormalTextBox" ID="txtSubject" Width="200px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:TemplateColumn HeaderText="Message" ItemStyle-Width="250px">
		        <ItemTemplate>
                    <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="Normal" ID="lblMessage" Width="250px"/>
		        </ItemTemplate>
		        <EditItemTemplate>
                    <asp:textbox runat="server" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="NormalTextBox" ID="txtMessage" Rows="10" Width="250px"/>
		        </EditItemTemplate>
		    </asp:TemplateColumn>
		    <asp:BoundColumn DataField="SenderName" HeaderText="Author" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="SenderEmail" HeaderText="From Email" ReadOnly="true"></asp:BoundColumn>
		    <asp:BoundColumn DataField="DisplayCreatedOnDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="CreatedOnDate"></asp:BoundColumn>
		    </Columns>
		</asp:DataGrid>
		<dnn:PagingControl id="pgArchiveFeedback" runat="server" Mode="PostBack" />
     </fieldset>
     <asp:Panel ID="pnlSpam" runat="server">
        <h2 class="dnnFormSectionHead" id="dnnPanel-SpamFeedback"><a href="" class="dnnSectionExpanded"><%=LocalizeString("scnSpamFeedback")%></a></h2>
	    <fieldset>
            <asp:DataGrid ID="dgSpamFeedback" runat="server" AutoGenerateColumns="false" width="100%" 
                    CellPadding="6" GridLines="None" cssclass="DataGrid_Container" AllowSorting="true" >
                <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="left"/>
                <itemstyle cssclass="Normal" horizontalalign="left" />
                <alternatingitemstyle cssclass="Normal" />
                <edititemstyle cssclass="NormalTextBox" />
                <selecteditemstyle cssclass="NormalRed" />
                <footerstyle cssclass="DataGrid_Footer" />
                <pagerstyle cssclass="DataGrid_Pager" />
                <Columns>
                <asp:BoundColumn DataField="FeedbackID" Visible = "false" ReadOnly="true"></asp:BoundColumn>
                <asp:ButtonColumn ButtonType="LinkButton" Text="SetPublic" CommandName="StatusPublic"></asp:ButtonColumn>
                <asp:ButtonColumn ButtonType="LinkButton" Text="SetPrivate" CommandName="StatusPrivate"></asp:ButtonColumn>
                <asp:TemplateColumn>
	                <ItemTemplate>
	                    <asp:ImageButton ID="cmdPrint" runat="server" CommandName="Print" AlternateText="Print" IconKey="Print" />
	                </ItemTemplate>
	            </asp:TemplateColumn>
                <asp:TemplateColumn>
	                <ItemTemplate>
	                    <asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" AlternateText="Delete" IconKey="Delete" />
	                </ItemTemplate>
	                <EditItemTemplate>
	                    <asp:ImageButton ID="cmdCancel" runat="server" CommandName="Cancel" AlternateText="Cancel" IconKey="Cancel" />
	                </EditItemTemplate>
	            </asp:TemplateColumn>
	            <asp:TemplateColumn>
	                <ItemTemplate>
	                    <asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" AlternateText="Edit" IconKey="Edit" />
	                </ItemTemplate>
	                <EditItemTemplate>
	                    <asp:ImageButton ID="cmdUpdate" runat="server" CommandName="Update" AlternateText="Update" IconKey="Save" />
	                </EditItemTemplate>
	            </asp:TemplateColumn>
	            <asp:BoundColumn DataField="CategoryName" HeaderText="Category" ReadOnly="true"></asp:BoundColumn>
		        <asp:TemplateColumn HeaderText="Subject" ItemStyle-Width="200px">
		            <ItemTemplate>
                        <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="Normal" ID="lblSubject" Width="200px"/>
		            </ItemTemplate>
		            <EditItemTemplate>
                        <asp:textbox runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Subject") %>' CssClass="NormalTextBox" ID="txtSubject" Width="200px"/>
		            </EditItemTemplate>
		        </asp:TemplateColumn>
		        <asp:TemplateColumn HeaderText="Message" ItemStyle-Width="250px">
		            <ItemTemplate>
                        <asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="Normal" ID="lblMessage" Width="250px"/>
		            </ItemTemplate>
		            <EditItemTemplate>
                        <asp:textbox runat="server" TextMode="MultiLine" Text='<%#DataBinder.Eval(Container.DataItem, "Message") %>' CssClass="NormalTextBox" ID="txtMessage" Rows="10" Width="250px"/>
		            </EditItemTemplate>
		        </asp:TemplateColumn>
		        <asp:BoundColumn DataField="SenderName" HeaderText="Author" ReadOnly="true"></asp:BoundColumn>
		        <asp:BoundColumn DataField="SenderEmail" HeaderText="From Email" ReadOnly="true"></asp:BoundColumn>
		        <asp:BoundColumn DataField="DisplayCreatedOnDate" HeaderText="Created Date" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="CreatedOnDate"></asp:BoundColumn>
		        </Columns>
		    </asp:DataGrid>
		    <dnn:PagingControl id="pgSpamFeedback" runat="server" Mode="PostBack" />
         </fieldset>
    </asp:Panel>
    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton id="cmdReturn" resourcekey="cmdReturn" runat="server" cssclass="dnnPrimaryAction" causesvalidation="False">Return</asp:linkbutton></li>
    </ul>
</div>

<script language="javascript" type="text/javascript">

    /*globals jQuery, window, Sys */
    (function ($, sys) {
        function setupFeedbackModeration() {
            $('#FeedbackModeration').dnnPanels();
        }

        $(document).ready(function () {
            setupFeedbackModeration();
            sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupFeedbackModeration();
            });
        });
    } (jQuery, window.Sys));
</script>
