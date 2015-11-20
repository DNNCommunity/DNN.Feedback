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
Imports DotNetNuke.Security
Imports System.Web

Namespace DotNetNuke.Modules.Feedback

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Feedback Class provides the UI for displaying the Feedback
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/22/2004	Moved Feedback to a separate Project
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Public Class Feedback
        Inherits FeedbackBase
        Implements Entities.Modules.IActionable
        'Implements Entities.Modules.IPortable

#Region "Private Members"

        Private _isAdministrator As Integer = -2
        Private _validationGroup As String
        Private Enum MessageLevel As Integer
            DNNSuccess = 1
            DNNInformation
            DNNWarning
            DNNError
        End Enum

#End Region

#Region "Public Properties"

        'Uses a tri-state check to avoid repeated calls to IsInRoles
        Public ReadOnly Property IsAdministrator() As Boolean
            Get
                If _isAdministrator = -1 Then
                    _isAdministrator = CType(Permissions.ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Admin, "", ModuleConfiguration), Integer)
                End If
                Return CType(_isAdministrator, Boolean)
            End Get
        End Property
#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetUser fills the Email/Name fields if a user is logged in
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/22/2004	Moved Feedback to a separate Project
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub GetUser()

            If Request.IsAuthenticated AndAlso UserId <> -1 Then
                With UserInfo
                    txtEmail.Text = .Email
                    txtEmailConfirm.Text = .Email
                    txtName.Text = .Profile.FullName
                    txtStreet.Text = .Profile.Street
                    txtCity.Text = .Profile.City
                    txtRegion.Text = .Profile.Region
                    txtCountry.Text = .Profile.Country
                    txtPostalCode.Text = .Profile.PostalCode
                    txtTelephone.Text = .Profile.Telephone
                End With
            Else
                txtEmail.Text = ""
                txtName.Text = ""
                txtStreet.Text = ""
                txtCity.Text = ""
                txtRegion.Text = ""
                txtCountry.Text = ""
                txtPostalCode.Text = ""
                txtTelephone.Text = ""
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InitializeForm sets the form up
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/22/2004	Moved Feedback to a separate Project
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub InitializeForm()
            Try
                GetUser()
                txtSubject2.Text = ""
                txtBody.Text = ""
                chkCopy.Checked = True
                divFeedbackFormContent.Visible = True
                _validationGroup = "FeedbackForm_" & ModuleId.ToString
                cmdSend.ValidationGroup = _validationGroup
                valSummary.ValidationGroup = _validationGroup
                SetValidationGroup(Me)
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub SetValidationGroup(ByVal parentControl As System.Web.UI.Control)
            For Each ctl As System.Web.UI.Control In parentControl.Controls
                If TypeOf ctl Is System.Web.UI.WebControls.BaseValidator Then
                    CType(ctl, System.Web.UI.WebControls.BaseValidator).ValidationGroup = _validationGroup
                ElseIf TypeOf ctl Is System.Web.UI.WebControls.CustomValidator Then
                    CType(ctl, System.Web.UI.WebControls.CustomValidator).ValidationGroup = _validationGroup
                ElseIf ctl.HasControls() And ctl.Visible = True Then
                    SetValidationGroup(ctl)
                End If
            Next
        End Sub

        Private Sub SetRequiredVisibility(ByVal div As System.Web.UI.HtmlControls.HtmlGenericControl, _
                                          ByVal ctl As Object, _
                                          ByVal ctlLabel As UserControls.LabelControl, _
                                          ByVal requiredValidator As System.Web.UI.WebControls.RequiredFieldValidator, _
                                          ByVal visibility As Configuration.FieldVisibility)
            div.Visible = visibility <> 3
            If TypeOf ctl Is System.Web.UI.WebControls.TextBox Then
                Dim ctlText As System.Web.UI.WebControls.TextBox = CType(ctl, System.Web.UI.WebControls.TextBox)
                If visibility = 1 Then
                    requiredValidator.Enabled = True
                    ctlText.Attributes.Add("class", "dnnFormRequired NormalTextBox Feedback_ControlWidth2")
                    SetSpanVisibility(ctlLabel)
                Else
                    ctlText.Attributes.Add("class", "NormalTextBox Feedback_ControlWidth")
                End If
            ElseIf TypeOf ctl Is System.Web.UI.WebControls.DropDownList Then
                Dim ctlDDL As System.Web.UI.WebControls.DropDownList = CType(ctl, System.Web.UI.WebControls.DropDownList)
                If visibility = 1 Then
                    requiredValidator.Enabled = True
                    ctlDDL.CssClass = "dnnFormRequired NormalTextBox Feedback_ControlWidth2"
                    SetSpanVisibility(ctlLabel)
                Else
                    ctlDDL.CssClass = "NormalTextBox Feedback_ControlWidth"
                End If
            End If

        End Sub

        Private Sub SetRequiredVisibility2(ByVal div As System.Web.UI.HtmlControls.HtmlGenericControl, _
                                          ByVal ctl As Object, _
                                          ByVal ctlLabel As UserControls.LabelControl, _
                                          ByVal requiredValidator As System.Web.UI.WebControls.RequiredFieldValidator, _
                                          ByVal visibility As Configuration.FieldVisibility2)
            div.Visible = visibility <> 2
            If TypeOf ctl Is System.Web.UI.WebControls.TextBox Then
                Dim ctlText As System.Web.UI.WebControls.TextBox = CType(ctl, System.Web.UI.WebControls.TextBox)
                If visibility = 1 Then
                    requiredValidator.Enabled = True
                    ctlText.Attributes.Add("class", "dnnFormRequired NormalTextBox Feedback_ControlWidth2")
                    SetSpanVisibility(ctlLabel)
                End If
            ElseIf TypeOf ctl Is System.Web.UI.WebControls.DropDownList Then
                Dim ctlDDL As System.Web.UI.WebControls.DropDownList = CType(ctl, System.Web.UI.WebControls.DropDownList)
                If visibility = 1 Then
                    requiredValidator.Enabled = True
                    ctlDDL.CssClass = "dnnFormRequired NormalTextBox Feedback_ControlWidth2"
                    SetSpanVisibility(ctlLabel)
                End If
            End If

        End Sub

        Private Sub SetSpanVisibility(ByVal ctlLabel As UserControls.LabelControl)
            ctlLabel.CssClass = "dnnFormRequired"
        End Sub

        Private Sub ShowMessage(ByVal msg As String, ByVal messageLevel As MessageLevel)
            lblMessage.Text = msg

            'Hide the rest of the form fields.
            InitializeForm()
            divFeedbackFormContent.Visible = False
            divConfirmation.Visible = True
            Select Case messageLevel
                Case messageLevel.DNNSuccess
                    divResponseMessage.Attributes.Add("class", "dnnFormMessage dnnFormSuccess")
                Case messageLevel.DNNInformation
                    divResponseMessage.Attributes.Add("class", "dnnFormMessage dnnFormInfo")
                Case messageLevel.DNNWarning
                    divResponseMessage.Attributes.Add("class", "dnnFormMessage dnnFormWarning")
                Case messageLevel.DNNError
                    divResponseMessage.Attributes.Add("class", "dnnFormMessage dnnFormValidationSummary")
            End Select
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/22/2004	Moved Feedback to a separate Project
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
            Try

                'Set validation regular expressions for Postal Code and Telephone fields

                If String.IsNullOrEmpty(MyConfiguration.EmailRegex) Then
                    valEmail2.Enabled = False
                Else
                    valEmail2.ValidationExpression = MyConfiguration.EmailRegex
                End If
                If String.IsNullOrEmpty(MyConfiguration.PostalCodeRegex) Then
                    valPostalCode2.Enabled = False
                Else
                    valPostalCode2.ValidationExpression = MyConfiguration.PostalCodeRegex
                End If
                If String.IsNullOrEmpty(MyConfiguration.TelephoneRegex) Then
                    valTelephone2.Enabled = False
                Else
                    valTelephone2.ValidationExpression = MyConfiguration.TelephoneRegex
                End If

                'Show / Hide fields and enable required field validators, set localized AlternateText and Title for required image

                If MyConfiguration.SendCopy And Not MyConfiguration.OptOut Then
                    SetRequiredVisibility(divEmail, txtEmail, plEmail, valEmail1, Configuration.FieldVisibility.Required)
                Else
                    SetRequiredVisibility(divEmail, txtEmail, plEmail, valEmail1, MyConfiguration.EmailFieldVisibility)
                End If
                SetRequiredVisibility2(divEmailConfirm, txtEmailConfirm, plEmailConfirm, valEmailConfirm, MyConfiguration.EmailConfirmFieldVisibility)
                SetRequiredVisibility(divName, txtName, plName, valName, MyConfiguration.NameFieldVisibility)
                SetRequiredVisibility(divStreet, txtStreet, plStreet, valStreet, MyConfiguration.StreetFieldVisibility)
                SetRequiredVisibility(divCity, txtCity, plCity, valCity, MyConfiguration.CityFieldVisibility)
                SetRequiredVisibility(divRegion, txtRegion, plRegion, valRegion, MyConfiguration.RegionFieldVisibility)
                SetRequiredVisibility(divCountry, txtCountry, plCountry, valCountry, MyConfiguration.CountryFieldVisibility)
                SetRequiredVisibility(divPostalCode, txtPostalCode, plPostalCode, valPostalCode, MyConfiguration.PostalCodeFieldVisibility)
                SetRequiredVisibility(divTelephone, txtTelephone, plTelephone, valTelephone, MyConfiguration.TelephoneFieldVisibility)
                SetRequiredVisibility(divMessage, txtBody, plMessage, valMessage, MyConfiguration.MessageFieldVisibility)

                If MyConfiguration.CaptchaVisibility = Configuration.CaptchaVisibilities.AllUsers _
                       OrElse (MyConfiguration.CaptchaVisibility = Configuration.CaptchaVisibilities.AnonymousUsers AndAlso UserId = -1) Then
                    divCaptcha.Visible = True
                    ctlCaptcha.ErrorMessage = Localization.GetString("InvalidCaptcha", LocalResourceFile)
                    ctlCaptcha.CaptchaTextBoxLabel = Localization.GetString("CaptchaText", LocalResourceFile)
                    ctlCaptcha.CaptchaLinkButtonText = Localization.GetString("RefreshCaptcha", LocalResourceFile)
                    ctlCaptcha.CaptchaAudioLinkButtonText = Localization.GetString("CaptchaAudioText", LocalResourceFile)
                    ctlCaptcha.CaptchaImage.EnableCaptchaAudio = MyConfiguration.CaptchaAudio
                    ctlCaptcha.IgnoreCase = MyConfiguration.CaptchaCase
                    ctlCaptcha.CaptchaImage.LineNoise = MyConfiguration.CaptchaLineNoise
                    ctlCaptcha.CaptchaImage.BackgroundNoise = MyConfiguration.CaptchaBackgroundNoise
                    _validationGroup = "FeedbackForm_" & ModuleId.ToString
                    ctlCaptcha.ValidationGroup = _validationGroup
                Else
                    divCaptcha.Visible = False
                End If

                Dim cssClass As String = "dnnForm FeedbackForm dnnClear"
                Select Case MyConfiguration.LabelDisplayPosition
                    Case Configuration.LabelDisplayPositions.AboveField
                        cssClass = "dnnForm NarrowFeedbackForm dnnClear"
                    Case Configuration.LabelDisplayPositions.SameLineAsField
                        cssClass = "dnnForm FeedbackForm dnnClear"
                End Select
                divFeedbackForm.Attributes.Add("class", cssClass)
                divFeedbackForm.Style.Add("width", MyConfiguration.Width.ToString)

                If Not Page.IsPostBack Then
                    InitializeForm()
                    txtBody.Rows = MyConfiguration.Rows
                    valMessageLength.ValidationExpression = "^[\s\S]{0," + MyConfiguration.MaxMessage.ToString & "}$"
                    valMessageLength.Text = String.Format(Localization.GetString("valMessageLength.Error", LocalResourceFile), MyConfiguration.MaxMessage.ToString)

                    'Bind the categories from the Feedback Lists table
                    Dim oLists As New FeedbackController
                    Dim aList As ArrayList = oLists.GetFeedbackList(False, PortalId, -1, FeedbackList.Type.Category, True, ModuleId, False)
                    With cboCategory
                        .DataSource = aList
                        .DataTextField = "Name"
                        .DataValueField = "ListID"
                        .DataBind()
                    End With

                    Dim category As String = MyConfiguration.Category

                    ' CategoryID is entered via querystring, use that
                    If HttpContext.Current.Request.QueryString("CategoryID") <> "" And MyConfiguration.CategorySelect Then
                        Dim tmpCategory As String = HttpContext.Current.Request.QueryString("CategoryID")
                        Dim selectedCategory As System.Web.UI.WebControls.ListItem = cboCategory.Items.FindByValue(tmpCategory)
                        If selectedCategory IsNot Nothing Then category = tmpCategory
                    End If

                    If MyConfiguration.CategorySelect AndAlso String.IsNullOrEmpty(category) Then
                        cboCategory.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("PleaseSelect", Configuration.SharedResources), ""))
                        cboCategory.SelectedIndex = 0
                    Else
                        Dim selectedCategory As System.Web.UI.WebControls.ListItem = cboCategory.Items.FindByValue(category)
                        If selectedCategory IsNot Nothing Then cboCategory.SelectedValue = category
                    End If

                    divCategory.Visible = MyConfiguration.CategorySelect AndAlso (aList.Count > 0)

                    If divCategory.Visible And MyConfiguration.CategoryRequired Then
                        SetRequiredVisibility(divCategory, cboCategory, plCategory, valCategory, Configuration.FieldVisibility.Required)
                    End If

                    Dim subject As String = MyConfiguration.Subject

                    'Set both rows to false and then show one based on whether the values from the settings are correct.
                    divSubject.Visible = False
                    divSubject2.Visible = False

                    Select Case MyConfiguration.SubjectFieldType
                        Case Configuration.SubjectFieldTypes.List
                            Dim subjectList As ArrayList = oLists.GetFeedbackList(False, PortalId, -1, FeedbackList.Type.Subject, True, ModuleId, False)
                            If subjectList.Count > 0 Then
                                cboSubject.DataSource = subjectList
                                cboSubject.DataTextField = "Name"
                                cboSubject.DataValueField = "ListID"
                                cboSubject.DataBind()

                                If String.IsNullOrEmpty(subject) Then
                                    cboSubject.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Localization.GetString("PleaseSelect", Configuration.SharedResources), ""))
                                    cboSubject.SelectedIndex = 0
                                Else
                                    Dim selectedSubject As System.Web.UI.WebControls.ListItem = cboSubject.Items.FindByValue(subject)
                                    If selectedSubject IsNot Nothing Then cboSubject.SelectedValue = subject
                                End If
                                SetRequiredVisibility(divSubject, cboSubject, plSubject, valSubject, Configuration.FieldVisibility.Required)
                            Else
                                SetRequiredVisibility(divSubject2, txtSubject2, plSubject2, valSubject2, Configuration.FieldVisibility.Required)
                            End If

                        Case Configuration.SubjectFieldTypes.Textbox
                            SetRequiredVisibility(divSubject2, txtSubject2, plSubject2, valSubject2, Configuration.FieldVisibility.Required)

                        Case Configuration.SubjectFieldTypes.Hidden
                            'Hide the subject rows - already set to Visibility=False above
                    End Select

                    If Not divCategory.Visible And Not divSubject.Visible And Not divSubject2.Visible And Not divMessage.Visible Then
                        hContactInfo.Visible = False
                        hFeedback.Visible = False
                    End If

                    'If the administrator has chosen to allow the user to optout then make the trCopy row visible

                    chkCopy.Checked = MyConfiguration.SendCopy
                    divCopy.Visible = MyConfiguration.OptOut

                End If

                txtBody.Attributes.Add("onkeyup", "javascript:showchars('" + txtBody.ClientID + "','" + CharCount.ClientID + "'," + MyConfiguration.MaxMessage.ToString + ")")
                CharCount.Text = (MyConfiguration.MaxMessage - txtBody.Text.Length).ToString

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the cancel button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/22/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(TabId))
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdSend_Click runs when the send button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/22/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click

            If Not Page.IsValid Then Exit Sub

            If Not divCaptcha.Visible OrElse ctlCaptcha.IsValid Then
                Try
                    Dim objPortalSecurity As New PortalSecurity
                    Dim objFeedbackController As New FeedbackController()

                    'Get the senders IP Address and Email Address
                    Dim remoteAddr As String = objPortalSecurity.InputFilter(Security.ModuleSecurity.GetUserIPAddress(), _
                                                  PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)
                    Dim senderEmail As String = objPortalSecurity.InputFilter(txtEmail.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)

                    Dim repeatSubmission As Boolean = False
                    If MyConfiguration.RepeatSubmissionInteval > 0 Then
                        'This assumes that the web server time and database time are the same - not always the case, though
                        Dim repeatWhenAfter As DateTime = DateTime.UtcNow.AddMinutes(-CDbl(MyConfiguration.RepeatSubmissionInteval))
                        Select Case MyConfiguration.RepeatSubmissionFilter
                            Case Configuration.RepeatSubmissionFilters.None
                                ' Do nothing as repeatSubmission is False
                            Case Configuration.RepeatSubmissionFilters.UserID
                                If UserId > 0 Then
                                    repeatSubmission = objFeedbackController.GetLastSubmissionDateForUserId(PortalId, UserId) > repeatWhenAfter
                                End If
                            Case Configuration.RepeatSubmissionFilters.UserIP
                                If remoteAddr <> "[]" Then
                                    repeatSubmission = objFeedbackController.GetLastSubmissionDateForUserIP(PortalId, remoteAddr) > repeatWhenAfter
                                End If
                            Case Configuration.RepeatSubmissionFilters.UserEMail
                                repeatSubmission = objFeedbackController.GetLastSubmissionDateForUserEmail(PortalId, senderEmail) > repeatWhenAfter
                        End Select
                    End If

                    If repeatSubmission Then
                        ShowMessage(Localization.GetString("SubmissionTooSoon", LocalResourceFile), MessageLevel.DNNWarning)
                        Exit Sub
                    End If

                    Dim categoryID As String = MyConfiguration.Category
                    Dim categoryText As String = ""

                    If (MyConfiguration.CategorySelect AndAlso cboCategory.SelectedValue <> "") Then
                        categoryID = cboCategory.SelectedItem.Value
                        categoryText = cboCategory.SelectedItem.Text
                    ElseIf categoryID <> "" Then
                        Dim li As System.Web.UI.WebControls.ListItem = cboCategory.Items.FindByValue(categoryID)
                        If li IsNot Nothing Then categoryText = li.Text
                    End If

                    Dim oFeedback As New FeedbackInfo
                    With oFeedback
                        .ModuleID = ModuleId
                        .PortalID = PortalId
                        .CategoryID = categoryID

                        'NOTE: v 4.4.3 opened major security hole for HTML injection attack
                        '      by And'ing instead of Or'ing filter flags rendering InputFilter ineffective!

                        .SenderName = objPortalSecurity.InputFilter(txtName.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderEmail = senderEmail
                        .SenderStreet = objPortalSecurity.InputFilter(txtStreet.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderCity = objPortalSecurity.InputFilter(txtCity.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderRegion = objPortalSecurity.InputFilter(txtRegion.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderCountry = objPortalSecurity.InputFilter(txtCountry.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderPostalCode = objPortalSecurity.InputFilter(txtPostalCode.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)
                        .SenderTelephone = objPortalSecurity.InputFilter(txtTelephone.Text, _
                                                PortalSecurity.FilterFlag.NoScripting Or _
                                                PortalSecurity.FilterFlag.NoMarkup)


                        .SenderRemoteAddr = remoteAddr

                    End With

                    Const allowHTML As Boolean = False

                    Dim filterFlags As PortalSecurity.FilterFlag = PortalSecurity.FilterFlag.NoScripting
                    If Not allowHTML Then
                        filterFlags = filterFlags Or PortalSecurity.FilterFlag.NoMarkup
                    End If

                    Dim msgText As String = ""
                    If MyConfiguration.MessageFieldVisibility = Configuration.FieldVisibility.Required Or Not txtBody.Text Is Nothing Then
                        msgText = objPortalSecurity.InputFilter(txtBody.Text, filterFlags)
                    End If

                    oFeedback.Message = msgText

                    'Check whether we're getting the subject from the dropdownlist or the text box.
                    If divSubject.Visible Then
                        'grab it from the drop down list.
                        If cboSubject.SelectedValue = "" Then
                            oFeedback.Subject = ""
                        Else
                            oFeedback.Subject = cboSubject.SelectedItem.Text
                        End If
                    ElseIf divSubject2.Visible Then
                        'grab it from the text box.
                        oFeedback.Subject = objPortalSecurity.InputFilter(txtSubject2.Text, _
                                            PortalSecurity.FilterFlag.NoScripting Or _
                                            PortalSecurity.FilterFlag.NoMarkup)
                    Else
                        'The admin might have chosen a subject but not made it visible.
                        If Not String.IsNullOrEmpty(MyConfiguration.Subject) Then
                            If IsNumeric(MyConfiguration.Subject) Then
                                'Grab the subject value and not the id.
                                Dim arryFeedbackItem As ArrayList = objFeedbackController.GetFeedbackList(True, PortalId, Convert.ToInt32(MyConfiguration.Subject), FeedbackList.Type.Subject, False, ModuleId, False)
                                If (arryFeedbackItem.Count > 0) Then
                                    Dim objFeedbackSubjectItem As FeedbackList = CType(arryFeedbackItem(0), FeedbackList)
                                    oFeedback.Subject = objFeedbackSubjectItem.Name
                                End If
                            Else
                                oFeedback.Subject = MyConfiguration.Subject
                            End If
                        Else
                            oFeedback.Subject = String.Empty
                        End If
                    End If

                    'If the Module is moderated and the category is one that is to be moderated then flag it appropriately
                    'other wise it will have StatusPublic and may appear immediately in the guest book.
                    Dim blModerated As Boolean = False

                    Dim status As FeedbackInfo.FeedbackStatusType = MyConfiguration.UnmoderatedStatus
                    If MyConfiguration.Moderated Then
                        ' ReSharper disable LoopCanBeConvertedToQuery
                        For Each moderatedCategory As String In MyConfiguration.ModeratedCategories.Split(";"c)
                            ' ReSharper restore LoopCanBeConvertedToQuery
                            If categoryID.Equals(moderatedCategory.Trim(), StringComparison.InvariantCultureIgnoreCase) Then
                                blModerated = True
                                status = FeedbackInfo.FeedbackStatusType.StatusPending
                                Exit For
                            End If
                        Next
                    End If

                    oFeedback.Referrer = Request.UserAgent
                    oFeedback.UserAgent = Request.UrlReferrer.ToString
                    oFeedback.Status = status

                    'Akisemet spam check if required
                    If MyConfiguration.AkismetEnable Then
                        Dim akismetApi As New Akismet(MyConfiguration.AkismetKey, NavigateURL())
                        If akismetApi.VerifyKey() Then
                            Dim comment As AkismetComment = akismetApi.CreateComment(oFeedback)
                            If akismetApi.CommentCheck(comment) Then
                                oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam
                            End If
                        End If
                    End If


                    'Now we try to save the feedback in the feedback table.
                    'If the status is pending then send an email to the Administrator asking him to log in and
                    'approve the message. In all other cases send the email directly to user if requested.

                    Try
                        'Only do this if there is an email address that the user has entered - this is to prevent
                        'users from clicking on the Send button repeatedly.
                        If senderEmail <> "" Or MyConfiguration.EmailFieldVisibility <> Configuration.FieldVisibility.Required Then

                            ' Admin has option of not saving unmoderated feedback categories to the database - only send emails in such case
                            If status <> FeedbackInfo.FeedbackStatusType.StatusDelete Then
                                ' Check to see if this is a duplicate submission if required before saving
                                If MyConfiguration.DuplicateSubmission Then
                                    If Not objFeedbackController.GetDuplicateSubmissionForUserEmail(PortalId, senderEmail, oFeedback.Message) = Nothing Then
                                        ShowMessage(Localization.GetString("DuplicateSubmission", LocalResourceFile), MessageLevel.DNNWarning)
                                        Exit Sub
                                    End If
                                End If
                                ' If email only, feedback is added to DB then deleted by scheduled process
                                If MyConfiguration.EmailOnly Then
                                    oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusDelete
                                End If
                                objFeedbackController.CreateFeedback(oFeedback, UserId)
                            End If

                            'If sendCopy is checked then assume that we have to send a copy to the user.
                            'If the user opts out then don't send.

                            'Create a copy of the email object

                            Dim objFeedbackEmail As New FeedbackEmail

                            Dim sendFromEmail As String
                            If String.IsNullOrEmpty(MyConfiguration.SendFrom) Then
                                sendFromEmail = PortalSettings.Email
                            Else
                                sendFromEmail = MyConfiguration.SendFrom
                            End If

                            objFeedbackEmail.SendFromEmail = sendFromEmail
                            objFeedbackEmail.SendToEmail = senderEmail

                            Dim emailSubject As String
                            If String.IsNullOrEmpty(oFeedback.Subject) Then
                                emailSubject = ModuleConfiguration.ModuleTitle
                            Else
                                emailSubject = oFeedback.Subject
                            End If
                            objFeedbackEmail.Subject = emailSubject

                            objFeedbackEmail.Message = oFeedback.Message

                            Dim errorMsg As String = String.Empty

                            Try
                                If MyConfiguration.SendCopy Then
                                    'Check whether the user has checked or unchecked the option.
                                    If chkCopy.Checked OrElse Not MyConfiguration.OptOut Then
                                        'User wants a copy of the feedback that was just submitted - Send it as an email from the site to the user.
                                        If MyConfiguration.SendAsync Then
                                            Dim objThread As New Threading.Thread(AddressOf objFeedbackEmail.SendEmail)
                                            objThread.Start()
                                        Else
                                            errorMsg = objFeedbackEmail.SendEmail()
                                        End If
                                    End If
                                Else
                                    'the admin may have chosen not to choose sendcopy but due to optout the chkCopy is visible
                                    If MyConfiguration.OptOut And chkCopy.Checked Then
                                        'User wants a copy of the feedback that was just submitted. - Send it as an email from the site to the user.
                                        If MyConfiguration.SendAsync Then
                                            Dim objThread As New Threading.Thread(AddressOf objFeedbackEmail.SendEmail)
                                            objThread.Start()
                                        Else
                                            errorMsg = objFeedbackEmail.SendEmail()
                                        End If
                                    End If
                                End If
                            Catch ex As Exception
                                errorMsg &= ex.Message
                            End Try

                            'Send a copy of the feedback to whatever addresses are in the SendTo or users in roles in SendToRole
                            If MyConfiguration.SendWhenPublished And blModerated _
                              And (oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusPending Or oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam) Then
                                'do nothing
                            Else
                                Dim objFeedbackUpdateController As New FeedbackUpdateController(ModuleId, MyConfiguration, PortalSettings, LocalResourceFile, UserId)
                                objFeedbackEmail = New FeedbackEmail
                                objFeedbackEmail.ReplyToEmail = senderEmail
                                objFeedbackEmail.SendFromEmail = objFeedbackUpdateController.GetSendFromEmail(senderEmail)

                                Dim objFeedbackCategoryItem As FeedbackList = objFeedbackUpdateController.GetCategory(categoryID)
                                objFeedbackEmail.SendToEmail = objFeedbackUpdateController.GetSendToEmail(objFeedbackCategoryItem.ListValue)

                                objFeedbackEmail.SendToRoles = MyConfiguration.SendToRoles
                                objFeedbackEmail.Subject = emailSubject
                                objFeedbackEmail.Message = objFeedbackUpdateController.CreateMsg(oFeedback, categoryText)

                                Try
                                    If MyConfiguration.SendAsync Then
                                        Dim objThread As New Threading.Thread(AddressOf objFeedbackEmail.SendEmail)
                                        objThread.Start()
                                    Else
                                        errorMsg &= objFeedbackEmail.SendEmail()
                                    End If

                                Catch ex As Exception
                                    errorMsg &= ex.Message
                                End Try
                            End If

                            'Send a message to those in roles having moderate permissions if this is a moderated Feedback Module and there are posts that need to be moderated.
                            If oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusPending _
                              Or (oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam And MyConfiguration.AkismetSendModerator) _
                              Or (oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam And blModerated) Then
                                Dim moderationLink As String = "<a href='" & NavigateURL(TabId, "Moderation", "mid=" & ModuleId.ToString) & "'>" & Localization.GetString("Moderate", LocalResourceFile) & "</a>"
                                Dim moderationMessage As String
                                If oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusPending Then
                                    moderationMessage = String.Format(Localization.GetString("MessagePending", LocalResourceFile), PortalSettings.PortalName, moderationLink)
                                Else
                                    moderationMessage = String.Format(Localization.GetString("MessageSpam", LocalResourceFile), PortalSettings.PortalName, moderationLink)
                                End If
                                Dim sendToRoles As String = Security.ModuleSecurity.ModeratorRoles(ModuleConfiguration.ModulePermissions)
                                If Not MyConfiguration.ModerationEmailsToAdmins Then
                                    sendToRoles = sendToRoles.Replace(PortalSettings.AdministratorRoleName & ";", "")
                                End If
                                If Right(sendToRoles, 1) = ";" Then
                                    sendToRoles = Left(sendToRoles, Len(sendToRoles) - 1)
                                End If

                                Try
                                    Dim contextkey As String
                                    Dim objNotification As New Components.Integration.Notifications
                                    contextkey = objNotification.SendFeedbackModerationNotification(sendToRoles.Split(";"c), PortalSettings.AdministratorId, emailSubject, moderationMessage, PortalId, TabId, ModuleId, oFeedback.FeedbackID)
                                    objFeedbackController.UpdateContextKey(ModuleId, oFeedback.FeedbackID, contextkey, UserId)
                                Catch ex As Exception
                                    errorMsg &= ex.Message
                                End Try

                            End If

                            If MyConfiguration.RedirectTabOnSubmission <> -1 Then
                                Response.Redirect(NavigateURL(MyConfiguration.RedirectTabOnSubmission), True)
                            Else
                                If errorMsg.Length > 0 Then
                                    'Only show detailed email send error messages when Admin user is testing
                                    If Not IsAdministrator Then
                                        errorMsg = String.Empty
                                    End If
                                    ShowMessage(Localization.GetString("SendMessageError", LocalResourceFile) & errorMsg, MessageLevel.DNNError)
                                Else
                                    If oFeedback.Status <> FeedbackInfo.FeedbackStatusType.StatusSpam Then
                                        ShowMessage(Localization.GetString("MessageSent", LocalResourceFile), MessageLevel.DNNSuccess)
                                    Else
                                        ShowMessage(Localization.GetString("SendMessageSpam", LocalResourceFile), MessageLevel.DNNWarning)
                                    End If
                                End If
                            End If

                        End If
                    Catch ex As Exception
                        ProcessModuleLoadException(Me, ex)
                    End Try
                Catch exc As Exception           'Module failed to load
                    ProcessModuleLoadException(Me, exc)
                End Try
            End If

        End Sub

        Private Sub valCopy_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles valCopy.ServerValidate
            If MyConfiguration.SendCopy And MyConfiguration.OptOut And chkCopy.Checked And txtEmail.Text = "" Then
                args.IsValid = False
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
            ' base module properties
            HelpURL = "http://www.dotnetnuke.com/" & glbDefaultPage & "?tabid=787"

        End Sub

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                ' ReSharper disable InconsistentNaming
                ' ReSharper disable LocalVariableHidesMember
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                ' ReSharper restore LocalVariableHidesMember
                ' ReSharper restore InconsistentNaming
                Dim modSecurity As New Security.ModuleSecurity(ModuleId, TabId)
                'check whether we have permission to moderate posts.
                Try
                    If modSecurity.IsAllowedToModeratePosts Then 'user is allowed to moderate posts
                        Actions.Add(GetNextActionID, Localization.GetString("MenuModeration", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", EditUrl("", "", "Moderation"), False, SecurityAccessLevel.View, True, False)
                    End If
                    If modSecurity.IsAllowedToManageLists Then
                        Actions.Add(GetNextActionID, Localization.GetString("MenuEditLists", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.EditContent, "", "", EditUrl("", "", "EditLists"), False, SecurityAccessLevel.View, True, False)
                    End If
                Catch
                    ' This try/catch is to avoid loosing control about your current Feedback module
                End Try
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace
