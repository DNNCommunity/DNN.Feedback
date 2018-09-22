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
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text.RegularExpressions
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace DotNetNuke.Modules.Feedback

    Partial Class Moderation
        Inherits FeedbackBase

#Region "Private Members"
        Private _pageSize As Integer = 5
        Private _moduleScope As Integer
        Private _deleteConfirmText As String
        Private _pagers As Generic.Dictionary(Of FeedbackInfo.FeedbackStatusType, PagerInfo)
#End Region

#Region "Public Properties"

        Private ReadOnly Property Pagers() As Generic.Dictionary(Of FeedbackInfo.FeedbackStatusType, PagerInfo)
            Get
                If _pagers Is Nothing Then
                    If ViewState("Pagers") Is Nothing Then
                        _pagers = New Generic.Dictionary(Of FeedbackInfo.FeedbackStatusType, PagerInfo)
                        With _pagers
                            For s As FeedbackInfo.FeedbackStatusType = FeedbackInfo.FeedbackStatusType.StatusPending To FeedbackInfo.FeedbackStatusType.StatusSpam
                                .Add(s, New PagerInfo)
                            Next
                        End With
                        ViewState("Pagers") = _pagers
                    Else
                        _pagers = CType(ViewState("Pagers"), Generic.Dictionary(Of FeedbackInfo.FeedbackStatusType, PagerInfo))
                    End If
                End If
                Return _pagers
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Sub BindData(ByVal status As FeedbackInfo.FeedbackStatusType)

            Dim categoryID As String = ""
            If cbShowOnlyModeratedCategories.Checked Then categoryID = MyConfiguration.ModeratedCategories

            Dim dg As DataGrid
            Select Case status
                Case FeedbackInfo.FeedbackStatusType.StatusPending
                    dg = dgPendingFeedback
                Case FeedbackInfo.FeedbackStatusType.StatusPrivate
                    dg = dgPrivateFeedback
                Case FeedbackInfo.FeedbackStatusType.StatusPublic
                    dg = dgPublicFeedback
                Case FeedbackInfo.FeedbackStatusType.StatusArchive
                    dg = dgArchiveFeedback
                Case FeedbackInfo.FeedbackStatusType.StatusSpam
                    dg = dgSpamFeedback
                Case Else
                    Exit Sub
            End Select

            'Grab a list of all the feedback for this status and present to the user in a datagrid.

            Dim oFb As New FeedbackController

            Dim arr As ArrayList = oFb.GetCategoryFeedback(PortalId, _moduleScope, categoryID, status, Pagers(status).CurrentPage, _pageSize, Pagers(status).CurrentSort.OrderByExpression)
            If arr.Count <= 0 Then
                Pagers(status).TotalRecords = 0
                dg.Visible = False
            Else
                Pagers(status).TotalRecords = CType(arr(0), FeedbackInfo).TotalRecords
                With dg
                    .Visible = True
                    .DataSource = arr
                    .DataBind()
                End With
            End If
        End Sub

        Private Sub RefreshPager(ByVal pager As UI.WebControls.PagingControl, ByVal status As FeedbackInfo.FeedbackStatusType)

            If pager Is Nothing Then
                Select Case status
                    Case FeedbackInfo.FeedbackStatusType.StatusPending
                        pager = pgPendingFeedback
                    Case FeedbackInfo.FeedbackStatusType.StatusPrivate
                        pager = pgPrivateFeedback
                    Case FeedbackInfo.FeedbackStatusType.StatusPublic
                        pager = pgPublicFeedback
                    Case FeedbackInfo.FeedbackStatusType.StatusArchive
                        pager = pgArchiveFeedback
                    Case FeedbackInfo.FeedbackStatusType.StatusSpam
                        pager = pgSpamFeedback
                    Case Else
                        Exit Sub
                End Select
            End If

            With pager
                .TotalRecords = Pagers(status).TotalRecords
                .PageSize = _pageSize
                .CurrentPage = Pagers(status).CurrentPage
                .QuerystringParams = "ctl=Feedback Moderation&mid=" & ModuleId.ToString & "&pagesize=" & _pageSize.ToString
                .TabID = TabId
                .Visible = (Pagers(status).TotalRecords > _pageSize)
            End With
        End Sub

        Private Sub ResetAllPagers()
            ViewState("Pagers") = Nothing
            _pagers = Nothing
        End Sub

        Private Sub RefreshAllPagers()
            RefreshPager(pgPendingFeedback, FeedbackInfo.FeedbackStatusType.StatusPending)
            RefreshPager(pgPrivateFeedback, FeedbackInfo.FeedbackStatusType.StatusPrivate)
            RefreshPager(pgPublicFeedback, FeedbackInfo.FeedbackStatusType.StatusPublic)
            RefreshPager(pgArchiveFeedback, FeedbackInfo.FeedbackStatusType.StatusArchive)
            RefreshPager(pgSpamFeedback, FeedbackInfo.FeedbackStatusType.StatusSpam)
        End Sub

        Private Sub SetSpamVisibility()
            If Not MyConfiguration.AkismetEnable Then
                dgPendingFeedback.Columns(4).Visible = False
                dgPrivateFeedback.Columns(3).Visible = False
                dgPublicFeedback.Columns(3).Visible = False
                dgArchiveFeedback.Columns(3).Visible = False
                pnlSpam.Visible = False
            End If
        End Sub

        Private Sub SetWorkFlowLinksVisibility(ByVal dg As DataGrid, ByVal cells As TableCellCollection, ByVal colVisible As Boolean)
            Dim c As TableCell
            For i As Integer = 1 To cells.Count - 1
                c = cells(i)
                If c.Controls.Count > 0 Then
                    Dim ctl As Control = c.Controls(0)
                    If ctl.GetType.FullName = "System.Web.UI.WebControls.DataGridLinkButton" Then dg.Columns(i).Visible = colVisible
                End If
            Next
        End Sub

        Private Function GetStatusTypeFromEventSource(ByVal ctrl As Control) As FeedbackInfo.FeedbackStatusType
            Dim status As String = Regex.Replace(ctrl.ID, "(dg|pg)(Pending|Private|Public|Archive|Spam)Feedback", "Status$2")
            Return CType([Enum].Parse(GetType(FeedbackInfo.FeedbackStatusType), status), FeedbackInfo.FeedbackStatusType)
        End Function

        Private Sub BindAllData()
            ResetAllPagers() 'Reset current page/total pages for all grids to 1, 0
            For status As FeedbackInfo.FeedbackStatusType = FeedbackInfo.FeedbackStatusType.StatusPending To FeedbackInfo.FeedbackStatusType.StatusSpam
                BindData(status)
            Next
            RefreshAllPagers()
            SetSpamVisibility()
        End Sub

        Private Sub LocalizeImageButton(ByVal item As DataGridItem, ByVal ctlID As String)
            Dim imgButton As ImageButton
            Dim ctl As Control = item.FindControl(ctlID)
            If ctl IsNot Nothing Then
                imgButton = CType(ctl, ImageButton)
                With imgButton
                    .AlternateText = Localization.GetString(.CommandName & ".AlternateText", LocalResourceFile)
                    .ToolTip = Localization.GetString(.CommandName & ".Tooltip", LocalResourceFile)
                    If .CommandName = "Delete" Then
                        UI.Utilities.ClientAPI.AddButtonConfirm(imgButton, _deleteConfirmText)
                    End If
                End With
            End If
        End Sub


#End Region
#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        'Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            'Localize the Headers and LinkButtons
            Localization.LocalizeDataGrid(dgPublicFeedback, Configuration.SharedResources)
            Localization.LocalizeDataGrid(dgPrivateFeedback, Configuration.SharedResources)
            Localization.LocalizeDataGrid(dgArchiveFeedback, Configuration.SharedResources)
            Localization.LocalizeDataGrid(dgPendingFeedback, Configuration.SharedResources)
            Localization.LocalizeDataGrid(dgSpamFeedback, Configuration.SharedResources)

            JavaScript.RequestRegistration(CommonJs.DnnPlugins)

        End Sub

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load

            'Check security here
            Dim modSecurity As New Security.ModuleSecurity(ModuleId, TabId)
            If Not modSecurity.IsAllowedToModeratePosts() Then Response.Redirect(AccessDeniedURL(), True)

            _pageSize = MyConfiguration.ModerationPageSize

            Select Case MyConfiguration.Scope
                Case Configuration.Scopes.Instance
                    _moduleScope = ModuleId
                Case Configuration.Scopes.Portal
                    _moduleScope = Null.NullInteger
            End Select

            _deleteConfirmText = Localization.GetString("DeleteItem", Localization.SharedResourceFile)

            If Not IsPostBack Then
                cbShowOnlyModeratedCategories.Visible = MyConfiguration.Moderated AndAlso (MyConfiguration.ModeratedCategories <> "")
                BindAllData()
            End If

        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReturn.Click
            Response.Redirect(NavigateURL())
        End Sub

        Private Sub dgFeedback_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles dgPendingFeedback.ItemCreated, dgPrivateFeedback.ItemCreated, dgPublicFeedback.ItemCreated, dgArchiveFeedback.ItemCreated, dgSpamFeedback.ItemCreated
            Dim dg As DataGrid = CType(sender, DataGrid)
            If e.Item.ItemType = ListItemType.Header Then
                For i As Integer = 0 To dg.Columns.Count - 1
                    Dim dgColumn As DataGridColumn = dg.Columns(i)
                    If Not String.IsNullOrEmpty(dgColumn.SortExpression) Then
                        Dim lbHeader As LinkButton = CType(e.Item.Cells(i).Controls(0), LinkButton)
                        Dim pt As Integer = lbHeader.Text.IndexOf("<img", StringComparison.Ordinal)
                        Dim headerText As String
                        If pt = -1 Then
                            headerText = lbHeader.Text
                        Else
                            headerText = lbHeader.Text.Substring(0, pt)
                        End If
                        Dim status As FeedbackInfo.FeedbackStatusType = GetStatusTypeFromEventSource(dg)
                        Dim dgCurrentSort As SortColumnInfo = Pagers(status).CurrentSort
                        If dgCurrentSort Is Nothing OrElse dgCurrentSort.Direction = SortColumnInfo.SortDirection.NotSorted Then
                            lbHeader.Text = headerText
                        Else
                            lbHeader.Text = headerText & dgCurrentSort.DirectionGlyph
                        End If
                    End If
                Next
            ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                LocalizeImageButton(e.Item, "cmdPrint")
                LocalizeImageButton(e.Item, "cmdDelete")
                LocalizeImageButton(e.Item, "cmdEdit")
            ElseIf e.Item.ItemType = ListItemType.EditItem Then
                LocalizeImageButton(e.Item, "cmdPrint")
                LocalizeImageButton(e.Item, "cmdCancel")
                LocalizeImageButton(e.Item, "cmdUpdate")
            End If
        End Sub

        Private Sub cbShowOnlyUnmoderatedCategories_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbShowOnlyModeratedCategories.CheckedChanged
            'Rebind all datagrids
            BindAllData()
        End Sub

        Private Sub pager_PageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles pgPendingFeedback.PageChanged, pgPrivateFeedback.PageChanged, pgPublicFeedback.PageChanged, pgArchiveFeedback.PageChanged, pgSpamFeedback.PageChanged
            Dim pager As UI.WebControls.PagingControl = CType(sender, UI.WebControls.PagingControl)
            Dim status As FeedbackInfo.FeedbackStatusType = GetStatusTypeFromEventSource(pager)
            Pagers(status).CurrentPage = pager.CurrentPage
            BindData(status)
            RefreshAllPagers()
        End Sub

        Private Sub dgFeedback_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dgPrivateFeedback.ItemCommand, dgPendingFeedback.ItemCommand, dgPublicFeedback.ItemCommand, dgArchiveFeedback.ItemCommand, dgSpamFeedback.ItemCommand
            If e.Item.ItemType = ListItemType.Item OrElse _
                    e.Item.ItemType = ListItemType.AlternatingItem OrElse _
                        e.Item.ItemType = ListItemType.EditItem Then
                Dim dg As DataGrid = CType(source, DataGrid)
                Dim statusFrom As FeedbackInfo.FeedbackStatusType = GetStatusTypeFromEventSource(dg)
                Dim feedbackID As Integer = CType(e.Item.Cells(0).Text, Integer)
                Dim objFeedbackController As New FeedbackController
                Dim objFeedbackInfo As FeedbackInfo = objFeedbackController.GetFeedback(feedbackID)
                Dim itmsRemoved As Integer = 0
                If Not objFeedbackInfo Is Nothing Then
                    If e.CommandName.StartsWith("Status") Then
                        Dim statusTo As FeedbackInfo.FeedbackStatusType = CType([Enum].Parse(GetType(FeedbackInfo.FeedbackStatusType), e.CommandName), FeedbackInfo.FeedbackStatusType)
                        Dim objFeedbackUpdateController As New FeedbackUpdateController(objFeedbackInfo.ModuleID, MyConfiguration, PortalSettings, LocalResourceFile, UserId)
                        objFeedbackUpdateController.FeedbackUpdateStatus(objFeedbackInfo.ModuleID, feedbackID, statusTo, Nothing)
                        Pagers(statusTo).CurrentPage = 1
                        BindData(statusTo)
                        itmsRemoved = 1
                    Else
                        Select Case e.CommandName
                            Case "Cancel"
                                dg.EditItemIndex = -1
                                SetWorkFlowLinksVisibility(dg, e.Item.Cells, True)
                            Case "Delete"
                                Dim objFeedbackUpdateController As New FeedbackUpdateController(objFeedbackInfo.ModuleID, MyConfiguration, PortalSettings, LocalResourceFile, UserId)
                                objFeedbackUpdateController.FeedbackUpdateStatus(objFeedbackInfo.ModuleID, feedbackID, FeedbackInfo.FeedbackStatusType.StatusDelete, Nothing)
                                itmsRemoved = 1
                            Case "Edit"
                                dg.EditItemIndex = e.Item.ItemIndex
                                SetWorkFlowLinksVisibility(dg, e.Item.Cells, False)
                            Case "Print"
                                Dim params As New Generic.List(Of String)
                                params.Add("mid=" & ModuleId.ToString)
                                params.Add("feedbackid=" & feedbackID.ToString)
                                params.Add("dnnprintmode=true")
                                params.Add("SkinSrc=" & Common.Globals.QueryStringEncode((("[G]" & SkinController.RootSkin) & "/_default/No Skin")))
                                params.Add("ContainerSrc=" & Common.Globals.QueryStringEncode((("[G]" & SkinController.RootContainer) & "/_default/No Container")))
                                If MyConfiguration.PrintAction = Configuration.PrintActions.InPlace Then
                                    params.Add("returnurl=" & HttpUtility.UrlEncode(Request.RawUrl))
                                    Response.Redirect(NavigateURL("print", params.ToArray), True)
                                Else
                                    UrlUtils.OpenNewWindow(Page, Me.GetType(), NavigateURL("print", params.ToArray))
                                End If
                            Case "Update"
                                Dim ps As New PortalSecurity
                                Dim subject As String = ps.InputFilter(CType(e.Item.FindControl("txtSubject"), TextBox).Text, _
                                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                                PortalSecurity.FilterFlag.NoMarkup)

                                Const allowHTML As Boolean = False 'In future release, allow HTML in message?

                                Dim filterFlags As PortalSecurity.FilterFlag = PortalSecurity.FilterFlag.NoScripting
                                If Not allowHTML Then
                                    filterFlags = filterFlags Or PortalSecurity.FilterFlag.NoMarkup
                                End If
                                Dim message As String = ps.InputFilter(CType(e.Item.FindControl("txtMessage"), TextBox).Text, filterFlags)

                                objFeedbackController.UpdateFeedback(_moduleScope, feedbackID, subject, message, UserId)

                                dg.EditItemIndex = -1
                                SetWorkFlowLinksVisibility(dg, e.Item.Cells, True)
                        End Select
                    End If
                    If (dg.Items.Count - itmsRemoved) = 0 Then
                        Pagers(statusFrom).CurrentPage = Math.Max(Pagers(statusFrom).CurrentPage - 1, 1)
                    End If
                End If
                BindData(statusFrom)
                RefreshAllPagers()
            End If
        End Sub

        Private Sub dgFeedback_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles dgPendingFeedback.SortCommand, dgPrivateFeedback.SortCommand, dgPublicFeedback.SortCommand, dgArchiveFeedback.SortCommand, dgSpamFeedback.SortCommand
            Dim dg As DataGrid = CType(source, DataGrid)
            Dim status As FeedbackInfo.FeedbackStatusType = GetStatusTypeFromEventSource(dg)
            Pagers(status).CurrentSort.ToggleDirection(True)
            BindData(status)
            RefreshAllPagers()
        End Sub

#End Region

#Region "Class PagerInfo"

        <Serializable()> _
        Private Class PagerInfo
            Private _currentPage As Integer
            Private _totalRecords As Integer
            Private ReadOnly _currentSort As SortColumnInfo

            Public Property CurrentPage() As Integer
                Get
                    Return _currentPage
                End Get
                Set(ByVal value As Integer)
                    _currentPage = value
                End Set
            End Property

            Public Property TotalRecords() As Integer
                Get
                    Return _totalRecords
                End Get
                Set(ByVal value As Integer)
                    _totalRecords = value
                End Set
            End Property

            Public ReadOnly Property CurrentSort() As SortColumnInfo
                Get
                    Return _currentSort
                End Get
            End Property

            Public Sub New()
                _currentPage = 1
                _totalRecords = 0
                _currentSort = New SortColumnInfo("CreatedOnDate", SortColumnInfo.SortDirection.Descending)
            End Sub

        End Class
#End Region

    End Class

End Namespace
