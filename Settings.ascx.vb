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
Imports System.Web.UI.WebControls
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
    Partial Public Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Private Members"
        Private ReadOnly _requiredText As String = Localization.GetString("Required", Configuration.SharedResources)
        Private ReadOnly _optionalText As String = Localization.GetString("Optional", Configuration.SharedResources)
        Private ReadOnly _hiddenText As String = Localization.GetString("Hidden", Configuration.SharedResources)
        Private _myConfiguration As Configuration
#End Region

#Region "Private Properties"
        Private ReadOnly Property MyConfiguration() As Configuration
            Get
                If _myConfiguration Is Nothing Then
                    _myConfiguration = New Configuration(ModuleId, Settings)
                End If
                Return _myConfiguration
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

            Dim li As ListItem

            Try
                If Not Page.IsPostBack Then

                    'bind the orphaned data grid and set visibility of orphaned data row

                    divOrphanedData.Visible = False
                    If DotNetNuke.Security.Permissions.ModulePermissionController.CanAdminModule(ModuleConfiguration) Then
                        Dim objFeedbackController As New FeedbackController

                        Dim orphanedData As Generic.Dictionary(Of String, Integer) = objFeedbackController.GetOrphanedData()
                        If orphanedData.Count > 0 Then
                            divOrphanedData.Visible = True
                            Localization.LocalizeDataGrid(dgOrphanedData, LocalResourceFile)
                            dgOrphanedData.DataSource = orphanedData
                            dgOrphanedData.DataBind()
                            UI.Utilities.ClientAPI.AddButtonConfirm(cmdDeleteOrphanedData, Localization.GetString("DeleteOrphanedDataConfirm", LocalResourceFile))
                        End If
                    End If

                    'bind the categories from the Feedback lists table
                    Dim oLists As New FeedbackController
                    Dim categoryList As ArrayList = oLists.GetFeedbackList(False, PortalId, -1, FeedbackList.Type.Category, True, ModuleId, False)
                    Dim subjectList As ArrayList = oLists.GetFeedbackList(False, PortalId, -1, FeedbackList.Type.Subject, True, ModuleId, False)

                    With cboCategory
                        .DataSource = categoryList
                        .DataTextField = "Name"
                        .DataValueField = "ListID"
                        .DataBind()
                    End With

                    'Load the moderation category here too
                    With cblModerationCategories
                        .DataSource = categoryList
                        .DataTextField = "Name"
                        .DataValueField = "ListID"
                        .DataBind()
                    End With

                    'Load the unmoderated status drop down list
                    BindUnmoderatedStatusList()

                    With cboSubject
                        .DataSource = subjectList
                        .DataTextField = "Name"
                        .DataValueField = "ListID"
                        .DataBind()
                    End With

                    BindRedirectTabs()

                    BindFieldVisibility(rblEmailField)
                    BindFieldVisibility2(rblEmailConfirmField)
                    BindFieldVisibility(rblNameField)
                    BindFieldVisibility(rblMessageField)
                    BindFieldVisibility(rblStreetField)
                    BindFieldVisibility(rblCityField)
                    BindFieldVisibility(rblRegionField)
                    BindFieldVisibility(rblCountryField)
                    BindFieldVisibility(rblPostalCodeField)
                    BindFieldVisibility(rblTelephoneField)

                    'Add the defaults
                    cboCategory.Items.Insert(0, New ListItem(Localization.GetString("NoneSelected", Configuration.SharedResources), ""))
                    cboSubject.Items.Insert(0, New ListItem(Localization.GetString("NoneSelected", Configuration.SharedResources), ""))

                    'get the settings

                    chkCategoryMailto.Checked = MyConfiguration.CategoryAsSendTo
                    chkCategory.Checked = MyConfiguration.CategorySelect
                    chkCategoryReq.Checked = MyConfiguration.CategoryRequired

                    'SM check whether categories have been setup or else disable the checkboxes for subjects and categories.
                    If cboCategory.Items.Count = 1 Then
                        chkCategory.Enabled = False
                        chkCategoryMailto.Enabled = False
                        chkCategory.Checked = False
                        chkCategoryMailto.Checked = False
                    Else
                        cboCategory.SelectedIndex = cboCategory.Items.IndexOf(cboCategory.Items.FindByValue(MyConfiguration.Category))
                    End If

                    txtSendFrom.Text = MyConfiguration.SendFrom
                    txtSendTo.Text = MyConfiguration.SendTo

                    Dim selectedRoleNames As New ArrayList
                    For Each roleName As String In MyConfiguration.SendToRoles.Split(";"c)
                        selectedRoleNames.Add(roleName)
                    Next
                    dgSelectedRoles.SelectedRoleNames = selectedRoleNames

                    rblLabelDisplay.SelectedValue = CInt(MyConfiguration.LabelDisplayPosition).ToString
                    txtWidth.Text = MyConfiguration.Width.ToString.Replace("px", "")
                    txtrows.Text = MyConfiguration.Rows.ToString
                    txtMaxMessage.Text = MyConfiguration.MaxMessage.ToString

                    rblSubjectEditField.SelectedValue = CInt(MyConfiguration.SubjectFieldType).ToString

                    If subjectList.Count > 1 Then
                        rblSubjectEditField.Items(0).Enabled = True
                        cboSubject.SelectedIndex = cboSubject.Items.IndexOf(cboSubject.Items.FindByValue(MyConfiguration.Subject))
                    Else
                        If rblSubjectEditField.SelectedValue = "1" Then
                            rblSubjectEditField.SelectedValue = "2"
                        End If
                        rblSubjectEditField.Items(0).Enabled = False
                    End If

                    chkSendCopy.Checked = MyConfiguration.SendCopy
                    If chkSendCopy.Checked Then
                        chkOptout.Enabled = True
                        chkOptout.Checked = MyConfiguration.OptOut
                    Else
                        chkOptout.Checked = False
                        chkOptout.Enabled = False
                    End If

                    chkModerated.Checked = MyConfiguration.Moderated
                    chkModerationAdminEmails.Checked = MyConfiguration.ModerationEmailsToAdmins
                    chkEmailOnly.Checked = MyConfiguration.EmailOnly
                    ddlModerationPageSize.Items.FindByValue(CStr(MyConfiguration.ModerationPageSize)).Selected = True
                    chkSendWhenPublished.Checked = MyConfiguration.SendWhenPublished
                    rblScope.SelectedValue = CInt(MyConfiguration.Scope).ToString
                    chkAsync.Checked = MyConfiguration.SendAsync
                    rblCaptchaVisibility.SelectedValue = CInt(MyConfiguration.CaptchaVisibility).ToString
                    BindToEnum(GetType(Telerik.Web.UI.CaptchaLineNoiseLevel), ddlCaptchaLineNoise)
                    BindToEnum(GetType(Telerik.Web.UI.CaptchaBackgroundNoiseLevel), ddlCaptchaBackgroundNoise)

                    ' Issue 22, NoCaptcha support
                    chkNoCaptcha.Checked = MyConfiguration.UseNoCaptcha
                    txtNoCaptchaSiteKey.Text = MyConfiguration.NoCaptchaSiteKey
                    txtNoCaptchaSecretKey.Text = MyConfiguration.NoCaptchaSecretKey
                    If MyConfiguration.UseNoCaptcha Then
                        valNoCaptchaSiteKey.Enabled = True
                        valNoCaptchaSecretKey.Enabled = True
                    Else
                        valNoCaptchaSiteKey.Enabled = False
                        valNoCaptchaSecretKey.Enabled = False
                    End If

                    rblRepeatSubmissionFilter.SelectedValue = CInt(MyConfiguration.RepeatSubmissionFilter).ToString
                    txtMinSubmissionInteval.Text = MyConfiguration.RepeatSubmissionInteval.ToString
                    chkDuplicateSubmission.Checked = MyConfiguration.DuplicateSubmission

                    li = ddlRedirectTabOnSubmission.Items.FindByValue(MyConfiguration.RedirectTabOnSubmission.ToString)
                    If li Is Nothing Then
                        ddlRedirectTabOnSubmission.SelectedValue = "-1"
                    Else
                        ddlRedirectTabOnSubmission.SelectedIndex = ddlRedirectTabOnSubmission.Items.IndexOf(li)
                    End If

                    LoadRequiredVisibilitySetting(rblEmailField, MyConfiguration.EmailFieldVisibility)
                    LoadRequiredVisibilitySetting2(rblEmailConfirmField, MyConfiguration.EmailConfirmFieldVisibility)
                    LoadRequiredVisibilitySetting(rblNameField, MyConfiguration.NameFieldVisibility)
                    LoadRequiredVisibilitySetting(rblMessageField, MyConfiguration.MessageFieldVisibility)
                    LoadRequiredVisibilitySetting(rblStreetField, MyConfiguration.StreetFieldVisibility)
                    LoadRequiredVisibilitySetting(rblCityField, MyConfiguration.CityFieldVisibility)
                    LoadRequiredVisibilitySetting(rblRegionField, MyConfiguration.RegionFieldVisibility)
                    LoadRequiredVisibilitySetting(rblCountryField, MyConfiguration.CountryFieldVisibility)
                    LoadRequiredVisibilitySetting(rblPostalCodeField, MyConfiguration.PostalCodeFieldVisibility)
                    LoadRequiredVisibilitySetting(rblTelephoneField, MyConfiguration.TelephoneFieldVisibility)

                    If chkModerated.Checked Then
                        divSendWhenPublished.Visible = True
                        divModerationCategories.Visible = True
                        For Each listId As String In MyConfiguration.ModeratedCategories.Split(";"c)
                            li = cblModerationCategories.Items.FindByValue(listId)
                            If li IsNot Nothing Then
                                li.Selected = True
                            End If
                        Next
                        ddlUnmoderatedStatus.SelectedValue = CInt(MyConfiguration.UnmoderatedStatus).ToString
                        divEmailOnly.Visible = False
                    Else
                        cblModerationCategories.ClearSelection()
                        ddlUnmoderatedStatus.SelectedValue = CInt(FeedbackInfo.FeedbackStatusType.StatusPublic).ToString
                        divSendWhenPublished.Visible = False
                        divModerationCategories.Visible = False
                        divUnmoderatedStatus.Visible = False
                        divEmailOnly.Visible = True
                    End If

                    txtEmailRegex.Text = MyConfiguration.EmailRegex
                    txtPostalCodeRegex.Text = MyConfiguration.PostalCodeRegex
                    txtTelephoneRegex.Text = MyConfiguration.TelephoneRegex
                    txtPrintTemplate.Text = MyConfiguration.PrintTemplate
                    rblPrintAction.SelectedValue = CInt(MyConfiguration.PrintAction).ToString

                    chkCleanupPending.Checked = MyConfiguration.CleanupPending
                    chkCleanupPrivate.Checked = MyConfiguration.CleanupPrivate
                    chkCleanupPublished.Checked = MyConfiguration.CleanupPublished
                    chkCleanupArchived.Checked = MyConfiguration.CleanupArchived
                    chkCleanupSpam.Checked = MyConfiguration.CleanupSpam
                    txtCleanupDaysBefore.Text = MyConfiguration.CleanupDaysBefore.ToString
                    txtCleanupMaxEntries.Text = MyConfiguration.CleanupMaxEntries.ToString

                    chkAkismetEnable.Checked = MyConfiguration.AkismetEnable
                    txtAkismetKey.Text = MyConfiguration.AkismetKey
                    chkAkismetSendModerator.Checked = MyConfiguration.AkismetSendModerator
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
            Try

                If Page.IsValid Then
                    Dim sb As StringBuilder
                    With MyConfiguration
                        .SendFrom = txtSendFrom.Text
                        .SendTo = txtSendTo.Text

                        sb = New StringBuilder
                        For Each roleName As String In dgSelectedRoles.SelectedRoleNames
                            sb.Append(roleName)
                            sb.Append(";")
                        Next
                        If sb.Length > 0 Then sb.Length -= 1 'remove trailing semi-colon
                        .SendToRoles = sb.ToString

                        .LabelDisplayPosition = CType(rblLabelDisplay.SelectedValue, Configuration.LabelDisplayPositions)
                        .Width = Unit.Parse(txtWidth.Text)
                        .Rows = Integer.Parse(txtrows.Text)
                        .MaxMessage = Integer.Parse(txtMaxMessage.Text)
                        .Subject = cboSubject.SelectedItem.Value
                        .SubjectFieldType = CType(rblSubjectEditField.SelectedValue, Configuration.SubjectFieldTypes)
                        .SendCopy = chkSendCopy.Checked
                        .OptOut = chkOptout.Checked
                        .Moderated = chkModerated.Checked
                        .ModerationEmailsToAdmins = chkModerationAdminEmails.Checked
                        .Scope = CType(rblScope.SelectedValue, Configuration.Scopes)
                        .SendAsync = chkAsync.Checked
                        .CategoryAsSendTo = chkCategoryMailto.Checked
                        .CategorySelect = chkCategory.Checked
                        .CategoryRequired = chkCategoryReq.Checked
                        .Category = cboCategory.SelectedValue

                        sb = New StringBuilder
                        For Each li As ListItem In cblModerationCategories.Items
                            If li.Selected Then
                                sb.Append(li.Value)
                                sb.Append(";")
                            End If
                        Next
                        If sb.Length > 0 Then sb.Length -= 1 'remove trailing semi-colon
                        .ModeratedCategories = sb.ToString

                        .UnmoderatedStatus = CType(ddlUnmoderatedStatus.SelectedValue, FeedbackInfo.FeedbackStatusType)
                        .EmailOnly = chkEmailOnly.Checked
                        .ModerationPageSize = CInt(ddlModerationPageSize.SelectedItem.Value)
                        .SendWhenPublished = chkSendWhenPublished.Checked
                        .CaptchaVisibility = CType(rblCaptchaVisibility.SelectedValue, Configuration.CaptchaVisibilities)

                        .UseNoCaptcha = chkNoCaptcha.Checked
                        .NoCaptchaSiteKey = txtNoCaptchaSiteKey.Text
                        .NoCaptchaSecretKey = txtNoCaptchaSecretKey.Text

                        .RepeatSubmissionFilter = CType(rblRepeatSubmissionFilter.SelectedValue, Configuration.RepeatSubmissionFilters)
                        .RepeatSubmissionInteval = Integer.Parse(txtMinSubmissionInteval.Text)
                        .DuplicateSubmission = chkDuplicateSubmission.Checked
                        .RedirectTabOnSubmission = CType(ddlRedirectTabOnSubmission.SelectedValue, Integer)
                        .EmailFieldVisibility = CType(rblEmailField.SelectedValue, Configuration.FieldVisibility)
                        .EmailConfirmFieldVisibility = CType(rblEmailConfirmField.SelectedValue, Configuration.FieldVisibility2)
                        .NameFieldVisibility = CType(rblNameField.SelectedValue, Configuration.FieldVisibility)
                        .MessageFieldVisibility = CType(rblMessageField.SelectedValue, Configuration.FieldVisibility)
                        .StreetFieldVisibility = CType(rblStreetField.SelectedValue, Configuration.FieldVisibility)
                        .CityFieldVisibility = CType(rblCityField.SelectedValue, Configuration.FieldVisibility)
                        .RegionFieldVisibility = CType(rblRegionField.SelectedValue, Configuration.FieldVisibility)
                        .CountryFieldVisibility = CType(rblCountryField.SelectedValue, Configuration.FieldVisibility)
                        .PostalCodeFieldVisibility = CType(rblPostalCodeField.SelectedValue, Configuration.FieldVisibility)
                        .TelephoneFieldVisibility = CType(rblTelephoneField.SelectedValue, Configuration.FieldVisibility)
                        .EmailRegex = txtEmailRegex.Text
                        .PostalCodeRegex = txtPostalCodeRegex.Text
                        .TelephoneRegex = txtTelephoneRegex.Text
                        .PrintTemplate = txtPrintTemplate.Text
                        .PrintAction = CType(rblPrintAction.SelectedValue, Configuration.PrintActions)
                        .CleanupPending = chkCleanupPending.Checked
                        .CleanupPrivate = chkCleanupPrivate.Checked
                        .CleanupPublished = chkCleanupPublished.Checked
                        .CleanupArchived = chkCleanupArchived.Checked
                        .CleanupSpam = chkCleanupSpam.Checked
                        .CleanupDaysBefore = CInt(txtCleanupDaysBefore.Text)
                        .CleanupMaxEntries = CInt(txtCleanupMaxEntries.Text)
                        .AkismetEnable = chkAkismetEnable.Checked
                        .AkismetKey = txtAkismetKey.Text
                        .AkismetSendModerator = chkAkismetSendModerator.Checked
                        .SaveSettings()
                    End With
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub BindRedirectTabs()
            Dim portalTabs As Generic.List(Of Entities.Tabs.TabInfo) = Entities.Tabs.TabController.GetPortalTabs(PortalId, TabId, True, True)
            With ddlRedirectTabOnSubmission
                .DataTextField = "TabName"
                .DataValueField = "TabId"
                .DataSource = portalTabs
                .DataBind()
            End With
        End Sub

        Private Sub BindFieldVisibility(ByVal rbl As RadioButtonList)
            With rbl.Items
                .Add(New ListItem(_requiredText, "1"))
                .Add(New ListItem(_optionalText, "2"))
                .Add(New ListItem(_hiddenText, "3"))
            End With
        End Sub

        Private Sub BindFieldVisibility2(ByVal rbl As RadioButtonList)
            With rbl.Items
                .Add(New ListItem(_requiredText, "1"))
                .Add(New ListItem(_hiddenText, "2"))
            End With
        End Sub

        Private Sub BindUnmoderatedStatusList()
            For Each v As Integer In [Enum].GetValues(GetType(FeedbackInfo.FeedbackStatusType))
                If [Enum].GetName(GetType(FeedbackInfo.FeedbackStatusType), v) <> "StatusSpam" Then
                    ddlUnmoderatedStatus.Items.Add(New ListItem(Localization.GetString([Enum].GetName(GetType(FeedbackInfo.FeedbackStatusType), v), Configuration.SharedResources), v.ToString))
                End If
            Next
        End Sub

        Private Sub LoadRequiredVisibilitySetting(ByVal rbl As RadioButtonList, ByVal visibility As Configuration.FieldVisibility)
            rbl.SelectedValue = CInt(visibility).ToString
        End Sub

        Private Sub LoadRequiredVisibilitySetting2(ByVal rbl As RadioButtonList, ByVal visibility As Configuration.FieldVisibility2)
            rbl.SelectedValue = CInt(visibility).ToString
        End Sub

        Public Sub BindToEnum(enumType As Type, ByVal ddl As DropDownList)
            ' get the names from the enumeration
            Dim names As String() = [Enum].GetNames(enumType)
            ' get the values from the enumeration
            Dim values As Array = [Enum].GetValues(enumType)
            ' turn it into a hash table
            ddl.Items.Clear()
            For i As Integer = 0 To names.Length - 1
                ' note the cast to integer here is important
                ' otherwise we'll just get the enum string back again
                ddl.Items.Add(New ListItem(Localization.GetString(names(i), Configuration.SharedResources), CStr(CInt(values.GetValue(i)))))
            Next
            ' return the dictionary to be bound to
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

        Private Sub chkModerated_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkModerated.CheckedChanged
            If chkModerated.Checked Then
                divModerationCategories.Visible = True
                For Each itm As ListItem In cblModerationCategories.Items
                    itm.Selected = True
                Next
                divSendWhenPublished.Visible = True
                divUnmoderatedStatus.Visible = True
                ddlUnmoderatedStatus.SelectedValue = CInt(FeedbackInfo.FeedbackStatusType.StatusPublic).ToString
                divEmailOnly.Visible = False
                chkEmailOnly.Checked = False
            Else
                cblModerationCategories.ClearSelection()
                ddlUnmoderatedStatus.SelectedValue = CInt(FeedbackInfo.FeedbackStatusType.StatusPublic).ToString
                divSendWhenPublished.Visible = False
                divModerationCategories.Visible = False
                divUnmoderatedStatus.Visible = False
                divEmailOnly.Visible = True
            End If
        End Sub

        Private Sub chkSendCopy_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSendCopy.CheckedChanged
            If chkSendCopy.Checked Then
                chkOptout.Checked = True
                chkOptout.Enabled = True
            Else
                chkOptout.Checked = False
                chkOptout.Enabled = False
            End If
        End Sub

        Private Sub btnResetEmailRegex_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetEmailRegex.Click
            txtEmailRegex.Text = MyConfiguration.DefaultEMailRegex
        End Sub

        Private Sub btnResetPostalCodeRegex_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetPostalCodeRegex.Click
            txtPostalCodeRegex.Text = Configuration.DefaultPostalCodeRegex
        End Sub

        Private Sub btnResetTelephoneRegex_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetTelephoneRegex.Click
            txtTelephoneRegex.Text = Configuration.DefaultTelephoneRegex
        End Sub

        Private Sub btnLoadDefaultPrintTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadDefaultPrintTemplate.Click
            txtPrintTemplate.Text = MyConfiguration.DefaultPrintTemplate
        End Sub

        Private Sub cmdDeleteOrphanedData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDeleteOrphanedData.Click
            Dim objFeedbackController As New FeedbackController
            objFeedbackController.DeleteOrphanedData()
            divOrphanedData.Visible = False
        End Sub

        Private Sub valCategory_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valCategory.ServerValidate
            If ((chkCategory.Checked OrElse chkCategoryMailto.Checked) AndAlso (cboCategory.Items.Count = 1)) Then
                args.IsValid = False
            End If
        End Sub

        Private Sub valCategoryMailto_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valCategoryMailto.ServerValidate
            If (chkCategoryMailto.Checked AndAlso Not (chkCategory.Checked OrElse cboCategory.SelectedValue <> "")) Then
                args.IsValid = False
            End If
        End Sub

        Private Sub valAkismetKey_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valAkismetKey.ServerValidate
            If chkAkismetEnable.Checked Then
                Dim akismetApi As New Akismet(txtAkismetKey.Text, NavigateURL())
                args.IsValid = akismetApi.VerifyKey()
            End If
        End Sub

        ' Issue #22 NoCaptcha support
        Protected Sub chkNoCaptcha_CheckedChanged(sender As Object, e As EventArgs)
            If chkNoCaptcha.Checked Then
                valNoCaptchaSiteKey.Enabled = True
                valNoCaptchaSecretKey.Enabled = True
            Else
                valNoCaptchaSiteKey.Enabled = False
                valNoCaptchaSecretKey.Enabled = False
            End If
        End Sub

#End Region


    End Class

End Namespace