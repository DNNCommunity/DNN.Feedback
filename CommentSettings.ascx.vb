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
Imports System.Text

Namespace DotNetNuke.Modules.Feedback

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings ModuleSettingsBase is used to manage the settings for the Feedback Module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	11/11/2004  created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class CommentSettings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Private Members"
        Private _template As Template
        Private _selectedModules As String
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
#End Region

#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/11/2004  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    Dim cfgSettings As New ConfigurationView(ModuleId, TabModuleId)

                    'bind the available Feedback modules
                    If cfgSettings.SelectedModules Is Nothing Then
                        rblScope.SelectedValue = "2"
                    Else
                        rblScope.SelectedValue = "1"
                    End If

                    Localization.LocalizeGridView(gvFeedbackModules, Configuration.SharedResources)
                    BindFeedbackModulesGrid()


                    'bind the categories from the Lists table
                    Dim oLists As New FeedbackController
                    Dim aList As ArrayList = oLists.GetFeedbackList(False, PortalId, -1, FeedbackList.Type.Category, True, ModuleId, True)
                    If aList.Count = 0 Then
                        divCategories.Attributes.Add("style", "display:none;")
                    End If
                    With cblCategories
                        .DataSource = aList
                        .DataTextField = "Name"
                        .DataValueField = "ListID"
                        .DataBind()
                    End With

                    If Not cfgSettings.ViewCategory Is Nothing Then
                        For Each listId As String In cfgSettings.ViewCategory
                            Dim li As System.Web.UI.WebControls.ListItem = cblCategories.Items.FindByValue(listId)
                            If li IsNot Nothing Then
                                li.Selected = True
                            End If
                        Next
                    End If

                    txtHeaderTemplate.Text = Template.HeaderTemplate
                    txtItemTemplate.Text = Template.ItemTemplate
                    txtSeparatorTemplate.Text = Template.SeparatorTemplate
                    txtAltItemTemplate.Text = Template.AltItemTemplate
                    txtFooterTemplate.Text = Template.FooterTemplate

                    chkEnablePager.Checked = cfgSettings.EnablePager

                    txtDefaultPageSize.Text = cfgSettings.DefaultPageSize.ToString

                    txtDefaultPageSize.Enabled = chkEnablePager.Checked

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/11/2004  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Dim cfgSettings As New ConfigurationView(ModuleId, TabModuleId)

            Try
                If Page.IsValid Then
                    Dim sb As New StringBuilder

                    If rblScope.SelectedValue = "1" Then
                        For Each dataRow As System.Web.UI.WebControls.GridViewRow In gvFeedbackModules.Rows
                            If dataRow.RowType = System.Web.UI.WebControls.DataControlRowType.DataRow Then
                                Dim cbSelected As System.Web.UI.WebControls.CheckBox = CType(dataRow.FindControl("cbSelected"), System.Web.UI.WebControls.CheckBox)
                                If cbSelected.Checked Then
                                    sb.Append(gvFeedbackModules.DataKeys(dataRow.DataItemIndex).Value.ToString)
                                    sb.Append(";")
                                End If
                            End If
                        Next
                        If sb.Length > 0 Then sb.Length -= 1
                    Else
                        sb.Append("-1")
                    End If
                    cfgSettings.SelectedModules = cfgSettings.StringToArray(sb.ToString)

                    sb.Length = 0
                    For Each li As System.Web.UI.WebControls.ListItem In cblCategories.Items
                        If li.Selected Then
                            sb.Append(li.Value)
                            sb.Append(";")
                        End If
                    Next
                    If sb.Length > 0 Then sb.Length -= 1
                    cfgSettings.ViewCategory = cfgSettings.StringToArray(sb.ToString)

                    With Template
                        .HeaderTemplate = txtHeaderTemplate.Text
                        .ItemTemplate = txtItemTemplate.Text.Trim
                        .AltItemTemplate = txtAltItemTemplate.Text
                        .SeparatorTemplate = txtSeparatorTemplate.Text
                        .FooterTemplate = txtFooterTemplate.Text
                        .UpdateTemplate()
                    End With

                    cfgSettings.EnablePager = chkEnablePager.Checked

                    If Not String.IsNullOrEmpty(txtDefaultPageSize.Text) AndAlso IsNumeric(txtDefaultPageSize.Text) Then
                        cfgSettings.DefaultPageSize = CInt(txtDefaultPageSize.Text)
                    End If

                    cfgSettings.SaveSettings(ModuleId, TabModuleId)

                    Entities.Modules.ModuleController.SynchronizeModule(ModuleId)

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region "Private Methods"
        Private Sub BindFeedbackModulesGrid()
            If rblScope.SelectedValue = "1" Then
                If Settings("Feedback_SelectedModules") Is Nothing Then
                    _selectedModules = ""
                Else
                    _selectedModules = CType(Settings("Feedback_SelectedModules"), String)
                End If
                divFeedbackModules.Visible = True
                Dim objController As New FeedbackController
                gvFeedbackModules.DataSource = objController.GetFeedbackModules(PortalId, ModuleConfiguration.DesktopModuleID, _selectedModules)
                gvFeedbackModules.DataBind()

            Else
                _selectedModules = "-1"
                divFeedbackModules.Visible = False
            End If
        End Sub
#End Region

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

#Region "Event Handlers"

        Private Sub cmdLoadDefaultHeaderTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoadDefaultHeaderTemplate.Click
            Template.HeaderTemplate = Template.GetTemplate(Template.TemplateTypes.Header, True)
            txtHeaderTemplate.Text = Template.HeaderTemplate
        End Sub

        Private Sub cmdLoadDefaultItemTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoadDefaultItemTemplate.Click
            Template.ItemTemplate = Template.GetTemplate(Template.TemplateTypes.Item, True)
            txtItemTemplate.Text = Template.ItemTemplate
        End Sub

        Private Sub cmdLoadDefaultSeparatorTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoadDefaultSeparatorTemplate.Click
            Template.SeparatorTemplate = Template.GetTemplate(Template.TemplateTypes.Separator, True)
            txtSeparatorTemplate.Text = Template.SeparatorTemplate
        End Sub

        Private Sub cmdLoadDefAltItemTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoadDefAltItemTemplate.Click
            Template.AltItemTemplate = Template.GetTemplate(Template.TemplateTypes.AltItem, True)
            txtAltItemTemplate.Text = Template.AltItemTemplate
        End Sub

        Private Sub cmdLoadDefaultFooterTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLoadDefaultFooterTemplate.Click
            Template.FooterTemplate = Template.GetTemplate(Template.TemplateTypes.Footer, True)
            txtFooterTemplate.Text = Template.FooterTemplate
        End Sub

        Private Sub chkEnablePager_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnablePager.CheckedChanged
            txtDefaultPageSize.Enabled = chkEnablePager.Checked
        End Sub

        Private Sub rblScope_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblScope.SelectedIndexChanged
            BindFeedbackModulesGrid()
        End Sub

#End Region

    End Class

End Namespace