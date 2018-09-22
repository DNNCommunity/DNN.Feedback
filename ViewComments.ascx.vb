'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.Security.Permissions

Namespace DotNetNuke.Modules.Feedback
    Partial Class ViewComments
        Inherits Entities.Modules.PortalModuleBase

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Private Members"

        Private _template As Template
        Private _pageSize As Integer = 10

#End Region

#Region "Properties"

        Protected ReadOnly Property Template() As Template
            Get
                If _template Is Nothing Then
                    _template = Template.LoadTemplate(ModuleId, TabModuleId, Settings)
                End If
                Return _template
            End Get
        End Property

        Private Property CurrentPage() As Integer
            Get
                If ViewState("CurrentPage") Is Nothing Then
                    Return 1
                Else
                    Return CType(ViewState("CurrentPage"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("CurrentPage") = value
            End Set
        End Property

#End Region

#Region "Private Methods"

        Private Sub BindData()

            Dim totalRecords As Integer = 0

            With rptComments
                .HeaderTemplate = Template.CreateTemplate(System.Web.UI.WebControls.ListItemType.Header)
                .ItemTemplate = Template.CreateTemplate(System.Web.UI.WebControls.ListItemType.Item)
                .AlternatingItemTemplate = Template.CreateTemplate(System.Web.UI.WebControls.ListItemType.AlternatingItem)
                .SeparatorTemplate = Template.CreateTemplate(System.Web.UI.WebControls.ListItemType.Separator)
                .FooterTemplate = Template.CreateTemplate(System.Web.UI.WebControls.ListItemType.Footer)
            End With

            Dim oFb As New FeedbackController
            Dim cfgSettings As New ConfigurationView(ModuleId, TabModuleId)

            Dim category As String = cfgSettings.ArrayToString(cfgSettings.ViewCategory)
            Dim selectedModules As String() = cfgSettings.SelectedModules
            Dim arr As New ArrayList

            If selectedModules Is Nothing Then       'legacy behavior/portal scope
                arr = oFb.GetCategoryFeedback(PortalId, -1, category, FeedbackInfo.FeedbackStatusType.StatusPublic, CurrentPage, _pageSize)
                If arr.Count > 0 Then
                    totalRecords = CType(arr(0), FeedbackInfo).TotalRecords
                End If
            Else
                For Each selectedModule As String In selectedModules
                    If IsNumeric(selectedModule) Then
                        Dim selectedModuleId As Integer = CType(selectedModule, Integer)
                        Dim mc As New Entities.Modules.ModuleController
                        Dim mi As Entities.Modules.ModuleInfo = mc.GetModule(selectedModuleId)
                        If ModulePermissionController.CanViewModule(mi) Then
                            Dim moduleItems As ArrayList = oFb.GetCategoryFeedback(PortalId, selectedModuleId, category, FeedbackInfo.FeedbackStatusType.StatusPublic, CurrentPage, _pageSize)
                            If moduleItems.Count > 0 Then
                                totalRecords += CType(moduleItems(0), FeedbackInfo).TotalRecords
                                arr.AddRange(moduleItems)
                            End If
                        End If
                    End If
                Next
            End If

            If arr IsNot Nothing AndAlso arr.Count > 0 Then
                rptComments.DataSource = arr
                rptComments.DataBind()

                With ctlPagingControl
                    .TotalRecords = totalRecords
                    .PageSize = _pageSize
                    .CurrentPage = CurrentPage
                    .TabID = TabId
                    .Visible = (totalRecords > _pageSize)
                End With
            Else
                totalRecords = 0
            End If

            Dim pagingEnabled As Boolean = cfgSettings.EnablePager

            If pagingEnabled AndAlso totalRecords > CInt(ddlPageSize.Items(0).Value) Then
                trPager.Visible = True
                litMessage.Visible = False
            Else
                trPager.Visible = False
                If totalRecords = 0 Then
                    litMessage.Visible = True
                    litMessage.Text = Localization.GetString("NoFeedback", LocalResourceFile)
                End If
            End If

        End Sub

#End Region

#Region "Event Handlers"

        Public Sub Page_Load(ByVal source As System.Object, ByVal e As EventArgs) Handles MyBase.Load

            If Not Page.IsPostBack Then
                Dim cfgSettings As New ConfigurationView(ModuleId, TabModuleId)
                trPager.Visible = cfgSettings.EnablePager

                If trPager.Visible Then
                    _pageSize = cfgSettings.DefaultPageSize

                    Dim start As Integer = Math.Max(_pageSize - 25, 5)
                    Dim finish As Integer = Math.Min(_pageSize + 25, 50)
                    ddlPageSize.Items.Clear()
                    For i As Integer = start To finish Step 5
                        ddlPageSize.Items.Add(i.ToString)
                    Next
                    ddlPageSize.SelectedValue = _pageSize.ToString
                Else
                    _pageSize = Integer.MaxValue
                End If
                BindData()
            Else
                If trPager.Visible Then
                    Integer.TryParse(ddlPageSize.SelectedValue, _pageSize)
                Else
                    _pageSize = Integer.MaxValue
                End If
            End If
        End Sub

        Private Sub ddlRecordsPerPage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
            CurrentPage = 1
            BindData()
        End Sub

        Private Sub ctlPagingControl_PageChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ctlPagingControl.PageChanged
            CurrentPage = ctlPagingControl.CurrentPage
            BindData()
        End Sub

        Private Sub rptComments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptComments.ItemDataBound
            With e.Item
                If .ItemType = System.Web.UI.WebControls.ListItemType.Item Or .ItemType = System.Web.UI.WebControls.ListItemType.AlternatingItem Then
                    If .HasControls Then
                        If .Controls(0).HasControls Then
                            Dim litControl As System.Web.UI.LiteralControl = CType(e.Item.Controls(0).Controls(0), System.Web.UI.LiteralControl)
                            Dim tr As New FeedbackTokenReplace(CType(e.Item.DataItem, FeedbackInfo), Configuration.SharedResources)
                            tr.AccessingUser = UserController.Instance.GetCurrentUserInfo()
                            tr.DebugMessages = Not (PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.View)
                            tr.ModuleId = ModuleId
                            litControl.Text = tr.ReplaceFeedbackTokens(litControl.Text)
                        End If
                    End If
                End If
            End With
        End Sub

#End Region

    End Class
End Namespace
