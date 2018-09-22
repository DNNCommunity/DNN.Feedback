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


Imports DotNetNuke.Entities.Portals

Namespace DotNetNuke.Modules.Feedback

    Public Class Configuration

        Public Enum LabelDisplayPositions As Integer
            SameLineAsField = 1
            AboveField
        End Enum

        Public Enum SubjectFieldTypes As Integer
            List = 1
            Textbox
            Hidden
        End Enum

        Public Enum FieldVisibility As Integer
            Required = 1
            [Optional]
            Hidden
        End Enum

        Public Enum FieldVisibility2 As Integer
            Required = 1
            Hidden
        End Enum

        Public Enum CaptchaVisibilities As Integer
            AllUsers = 1
            AnonymousUsers
            Disabled
        End Enum

        Public Enum RepeatSubmissionFilters As Integer
            None = 1
            UserID
            UserIP
            UserEMail
        End Enum

        Public Enum PrintActions As Integer
            InPlace = 1
            PopUp
        End Enum

        Public Enum Scopes As Integer
            Instance = 1
            Portal
        End Enum

        Public Const PathOfModule As String = "DesktopModules/Feedback/"
        Public Const ModuleFriendlyName As String = "Feedback"
        Public Const SharedResources As String = "~/" & PathOfModule & "App_LocalResources/" & "SharedResources.resx"

        '"\b[a-zA-Z0-9._%\-+']+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,4}\b"
        Public Const DefaultTelephoneRegex As String = "^\d?(?:(?:[\+]?(?:[\d]{1,3}(?:[ ]+|[\-.])))?[(]?(?:[\d]{3})[\-/)]?(?:[ ]+)?)?(?:[a-zA-Z2-9][a-zA-Z0-9 \-.]{6,})(?:(?:[ ]+|[xX]|(i:ext[\.]?)){1,2}(?:[\d]{1,5}))?$"
        Public Const DefaultPostalCodeRegex As String = "^((\d{5}-\d{4})|(\d{5})|([A-Z]\d[A-Z](\s|-)\d[A-Z]\d))$"

        Private _moduleId As Integer = -1
        Private ReadOnly _portalSettings As Entities.Portals.PortalSettings
        Private ReadOnly _portalID As Integer = -1

        Private ReadOnly _settings As Hashtable = Nothing

        Private _categoryAsSendTo As Boolean
        Private _categorySelect As Boolean
        Private _categoryRequired As Boolean
        Private _category As String

        Private _moderated As Boolean
        Private _moderationemailstoadmins As Boolean
        Private _scope As Scopes
        Private _moderatedCategories As String
        Private _unmoderatedStatus As FeedbackInfo.FeedbackStatusType = FeedbackInfo.FeedbackStatusType.StatusPublic
        Private _emailOnly As Boolean
        Private _sendWhenPublished As Boolean
        Private _moderationPageSize As Integer

        Private _labelDisplayPosition As LabelDisplayPositions
        Private _width As System.Web.UI.WebControls.Unit
        Private _rows As Integer
        Private _captchaVisibility As CaptchaVisibilities
        Private _captchaAudio As Boolean
        Private _captchaCase As Boolean
        Private _captchaLineNoise As Telerik.Web.UI.CaptchaLineNoiseLevel
        Private _captchaBackgroundNoise As Telerik.Web.UI.CaptchaBackgroundNoiseLevel
        Private _repeatSubmissionFilter As RepeatSubmissionFilters
        Private _repeatSubmissionInteval As Integer
        Private _duplicateSubmission As Boolean

        Private _subjectFieldType As SubjectFieldTypes
        Private _subject As String

        Private _sendTo As String
        Private _sendToRoles As String
        Private _sendFrom As String
        Private _sendCopy As Boolean
        Private _optOut As Boolean
        Private _sendAsync As Boolean

        Private _nameFieldVisibility As FieldVisibility = FieldVisibility.Optional
        Private _messageFieldVisibility As FieldVisibility
        Private _streetFieldVisibility As FieldVisibility
        Private _cityFieldVisibility As FieldVisibility
        Private _regionFieldVisibility As FieldVisibility
        Private _countryFieldVisibility As FieldVisibility
        Private _postalCodeFieldVisibility As FieldVisibility
        Private _telephoneFieldVisibility As FieldVisibility
        Private _emailFieldVisibility As FieldVisibility
        Private _emailConfirmFieldVisibility As FieldVisibility2

        Private _emailRegex As String
        Private _telephoneRegex As String
        Private _postalCodeRegex As String
        Private _redirectTabOnSubmission As Integer = -1
        Private _printTemplate As String
        Private _printAction As PrintActions
        Private _maxMessage As Integer

        Private _cleanupPublished As Boolean
        Private _cleanupPrivate As Boolean
        Private _cleanupArchived As Boolean
        Private _cleanupPending As Boolean
        Private _cleanupSpam As Boolean
        Private _cleanupDaysBefore As Integer
        Private _cleanupMaxEntries As Integer

        Private _akismetEnable As Boolean
        Private _akismetKey As String
        Private _akismetSendModerator As Boolean

        ' Issue #22 NoCaptcha support
        Private _useNoCaptcha As Boolean
        Private _noCaptchaSiteKey As String
        Private _noCaptchaSecretKey As String

#Region "Public ReadOnly Properties"

        Public ReadOnly Property ModuleId() As Integer
            Get
                Return _moduleId
            End Get
        End Property

        Public ReadOnly Property PortalSettings() As Entities.Portals.PortalSettings
            Get
                Return _portalSettings
            End Get
        End Property

        Public ReadOnly Property PortalID() As Integer
            Get
                Return _portalID
            End Get
        End Property

        Public ReadOnly Property Settings() As Hashtable
            Get
                Return _settings
            End Get
        End Property
#End Region

#Region "Public Properties - Configuration"

        Public Property CleanupSpam() As Boolean
            Get
                Return _cleanupSpam
            End Get
            Set(ByVal value As Boolean)
                _cleanupSpam = value
            End Set
        End Property

        Public Property CleanupPending() As Boolean
            Get
                Return _cleanupPending
            End Get
            Set(ByVal value As Boolean)
                _cleanupPending = value
            End Set
        End Property

        Public Property CleanupPrivate() As Boolean
            Get
                Return _cleanupPrivate
            End Get
            Set(ByVal value As Boolean)
                _cleanupPrivate = value
            End Set
        End Property

        Public Property CleanupPublished() As Boolean
            Get
                Return _cleanupPublished
            End Get
            Set(ByVal value As Boolean)
                _cleanupPublished = value
            End Set
        End Property

        Public Property CleanupArchived() As Boolean
            Get
                Return _cleanupArchived
            End Get
            Set(ByVal value As Boolean)
                _cleanupArchived = value
            End Set
        End Property

        Public Property CleanupDaysBefore() As Integer
            Get
                Return _cleanupDaysBefore
            End Get
            Set(ByVal value As Integer)
                _cleanupDaysBefore = value
            End Set
        End Property

        Public Property CleanupMaxEntries() As Integer
            Get
                Return _cleanupMaxEntries
            End Get
            Set(ByVal value As Integer)
                _cleanupMaxEntries = value
            End Set
        End Property

        Public Property CategoryAsSendTo() As Boolean
            Get
                Return _categoryAsSendTo
            End Get
            Set(ByVal value As Boolean)
                _categoryAsSendTo = value
            End Set
        End Property

        Public Property CategorySelect() As Boolean
            Get
                Return _categorySelect
            End Get
            Set(ByVal value As Boolean)
                _categorySelect = value
            End Set
        End Property

        Public Property CategoryRequired() As Boolean
            Get
                Return _categoryRequired
            End Get
            Set(ByVal value As Boolean)
                _categoryRequired = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property

        Public Property Moderated() As Boolean
            Get
                Return _moderated
            End Get
            Set(ByVal value As Boolean)
                _moderated = value
            End Set
        End Property

        Public Property EmailOnly() As Boolean
            Get
                Return _emailOnly
            End Get
            Set(ByVal value As Boolean)
                _emailOnly = value
            End Set
        End Property

        Public Property SendWhenPublished() As Boolean
            Get
                Return _sendWhenPublished
            End Get
            Set(ByVal value As Boolean)
                _sendWhenPublished = value
            End Set
        End Property

        Public Property ModerationEmailsToAdmins() As Boolean
            Get
                Return _moderationemailstoadmins
            End Get
            Set(ByVal value As Boolean)
                _moderationemailstoadmins = value
            End Set
        End Property

        Public Property Scope() As Scopes
            Get
                Return _scope
            End Get
            Set(ByVal value As Scopes)
                _scope = value
            End Set
        End Property

        Public Property ModeratedCategories() As String
            Get
                Return _moderatedCategories
            End Get
            Set(ByVal value As String)
                _moderatedCategories = value
            End Set
        End Property

        Public Property UnmoderatedStatus() As FeedbackInfo.FeedbackStatusType
            Get
                Return _unmoderatedStatus
            End Get
            Set(ByVal value As FeedbackInfo.FeedbackStatusType)
                _unmoderatedStatus = value
            End Set
        End Property

        Public Property ModerationPageSize() As Integer
            Get
                Return _moderationPageSize
            End Get
            Set(ByVal value As Integer)
                _moderationPageSize = value
            End Set
        End Property

        Public Property LabelDisplayPosition() As LabelDisplayPositions
            Get
                Return _labelDisplayPosition
            End Get
            Set(ByVal value As LabelDisplayPositions)
                _labelDisplayPosition = value
            End Set
        End Property

        Public Property Width() As System.Web.UI.WebControls.Unit
            Get
                Return _width
            End Get
            Set(ByVal value As System.Web.UI.WebControls.Unit)
                _width = value
            End Set
        End Property

        Public Property Rows() As Integer
            Get
                Return _rows
            End Get
            Set(ByVal value As Integer)
                _rows = value
            End Set
        End Property

        Public Property CaptchaVisibility() As CaptchaVisibilities
            Get
                Return _captchaVisibility
            End Get
            Set(ByVal value As CaptchaVisibilities)
                _captchaVisibility = value
            End Set
        End Property

        ' Issue #22 NoCaptcha support
        Public Property UseNoCaptcha() As Boolean
            Get
                Return _useNoCaptcha
            End Get
            Set(value As Boolean)
                _useNoCaptcha = value
            End Set
        End Property

        Public Property NoCaptchaSiteKey() As String
            Get
                Return _noCaptchaSiteKey
            End Get
            Set(value As String)
                _noCaptchaSiteKey = value
            End Set
        End Property

        Public Property NoCaptchaSecretKey() As String
            Get
                Return _noCaptchaSecretKey
            End Get
            Set(value As String)
                _noCaptchaSecretKey = value
            End Set
        End Property
        ' End of Issue #22

        Public Property CaptchaAudio() As Boolean
            Get
                Return _captchaAudio
            End Get
            Set(ByVal value As Boolean)
                _captchaAudio = value
            End Set
        End Property

        Public Property CaptchaCase() As Boolean
            Get
                Return _captchaCase
            End Get
            Set(ByVal value As Boolean)
                _captchaCase = value
            End Set
        End Property

        Public Property CaptchaLineNoise() As Telerik.Web.UI.CaptchaLineNoiseLevel
            Get
                Return _captchaLineNoise
            End Get
            Set(ByVal value As Telerik.Web.UI.CaptchaLineNoiseLevel)
                _captchaLineNoise = value
            End Set
        End Property

        Public Property CaptchaBackgroundNoise() As Telerik.Web.UI.CaptchaBackgroundNoiseLevel
            Get
                Return _captchaBackgroundNoise
            End Get
            Set(ByVal value As Telerik.Web.UI.CaptchaBackgroundNoiseLevel)
                _captchaBackgroundNoise = value
            End Set
        End Property

        Public Property DuplicateSubmission As Boolean
            Get
                Return _duplicateSubmission
            End Get
            Set(ByVal value As Boolean)
                _duplicateSubmission = value
            End Set
        End Property

        Public Property RepeatSubmissionFilter() As RepeatSubmissionFilters
            Get
                Return _repeatSubmissionFilter
            End Get
            Set(ByVal value As RepeatSubmissionFilters)
                _repeatSubmissionFilter = value
            End Set
        End Property

        Public Property RepeatSubmissionInteval() As Integer
            Get
                Return _repeatSubmissionInteval
            End Get
            Set(ByVal value As Integer)
                _repeatSubmissionInteval = value
            End Set
        End Property

        Public Property SubjectFieldType() As SubjectFieldTypes
            Get
                Return _subjectFieldType
            End Get
            Set(ByVal value As SubjectFieldTypes)
                _subjectFieldType = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                _subject = value
            End Set
        End Property

        Public Property SendFrom() As String
            Get
                Return _sendFrom
            End Get
            Set(ByVal value As String)
                _sendFrom = value
            End Set
        End Property

        Public Property SendTo() As String
            Get
                Return _sendTo
            End Get
            Set(ByVal value As String)
                _sendTo = value
            End Set
        End Property

        Public Property SendToRoles() As String
            Get
                Return _sendToRoles
            End Get
            Set(ByVal value As String)
                _sendToRoles = value
            End Set
        End Property

        Public Property SendCopy() As Boolean
            Get
                Return _sendCopy
            End Get
            Set(ByVal value As Boolean)
                _sendCopy = value
            End Set
        End Property

        Public Property OptOut() As Boolean
            Get
                Return _optOut
            End Get
            Set(ByVal value As Boolean)
                _optOut = value
            End Set
        End Property

        Public Property SendAsync() As Boolean
            Get
                Return _sendAsync
            End Get
            Set(ByVal value As Boolean)
                _sendAsync = value
            End Set
        End Property

        Public Property NameFieldVisibility() As FieldVisibility
            Get
                Return _nameFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _nameFieldVisibility = value
            End Set
        End Property

        Public Property MessageFieldVisibility() As FieldVisibility
            Get
                Return _messageFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _messageFieldVisibility = value
            End Set
        End Property

        Public Property StreetFieldVisibility() As FieldVisibility
            Get
                Return _streetFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _streetFieldVisibility = value
            End Set
        End Property

        Public Property CityFieldVisibility() As FieldVisibility
            Get
                Return _cityFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _cityFieldVisibility = value
            End Set
        End Property

        Public Property RegionFieldVisibility() As FieldVisibility
            Get
                Return _regionFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _regionFieldVisibility = value
            End Set
        End Property

        Public Property CountryFieldVisibility() As FieldVisibility
            Get
                Return _countryFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _countryFieldVisibility = value
            End Set
        End Property

        Public Property PostalCodeFieldVisibility() As FieldVisibility
            Get
                Return _postalCodeFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _postalCodeFieldVisibility = value
            End Set
        End Property

        Public Property TelephoneFieldVisibility() As FieldVisibility
            Get
                Return _telephoneFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _telephoneFieldVisibility = value
            End Set
        End Property

        Public Property EmailFieldVisibility() As FieldVisibility
            Get
                Return _emailFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility)
                _emailFieldVisibility = value
            End Set
        End Property

        Public Property EmailConfirmFieldVisibility() As FieldVisibility2
            Get
                Return _emailConfirmFieldVisibility
            End Get
            Set(ByVal value As FieldVisibility2)
                _emailConfirmFieldVisibility = value
            End Set
        End Property

        Public ReadOnly Property DefaultEMailRegex() As String            '"\b[a-zA-Z0-9._%\-+']+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,4}\b"
            Get
                If _portalID <> -1 Then
                    Return CStr(Entities.Users.UserController.GetUserSettings(_portalID)("Security_EmailValidation"))
                Else
                    Return Entities.Controllers.HostController.Instance.GetSettingsDictionary("Security_EmailValidation")
                End If
            End Get
        End Property

        Public Property EmailRegex() As String
            Get
                Return _emailRegex
            End Get
            Set(ByVal value As String)
                _emailRegex = value
            End Set
        End Property

        Public Property PostalCodeRegex() As String
            Get
                Return _postalCodeRegex
            End Get
            Set(ByVal value As String)
                _postalCodeRegex = value
            End Set
        End Property

        Public Property TelephoneRegex() As String
            Get
                Return _telephoneRegex
            End Get
            Set(ByVal value As String)
                _telephoneRegex = value
            End Set
        End Property

        Public Property RedirectTabOnSubmission() As Integer
            Get
                Return _redirectTabOnSubmission
            End Get
            Set(ByVal value As Integer)
                _redirectTabOnSubmission = value
            End Set
        End Property

        Public ReadOnly Property DefaultPrintTemplate() As String
            Get
                Return Localization.GetString("DefaultPrintTemplate", SharedResources)
            End Get
        End Property

        Public Property PrintTemplate() As String
            Get
                Return _printTemplate
            End Get
            Set(ByVal value As String)
                _printTemplate = value
            End Set
        End Property

        Public Property PrintAction() As PrintActions
            Get
                Return _printAction
            End Get
            Set(ByVal value As PrintActions)
                _printAction = value
            End Set
        End Property

        Public Property MaxMessage() As Integer
            Get
                Return _maxMessage
            End Get
            Set(ByVal value As Integer)
                _maxMessage = value
            End Set
        End Property

        Public Property AkismetEnable() As Boolean
            Get
                Return _akismetEnable
            End Get
            Set(ByVal value As Boolean)
                _akismetEnable = value
            End Set
        End Property

        Public Property AkismetKey() As String
            Get
                Return _akismetKey
            End Get
            Set(ByVal value As String)
                _akismetKey = value
            End Set
        End Property

        Public Property AkismetSendModerator() As Boolean
            Get
                Return _akismetSendModerator
            End Get
            Set(ByVal value As Boolean)
                _akismetSendModerator = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            _settings = New Hashtable
        End Sub

        Public Sub New(ByVal moduleId As Integer)
            _moduleId = moduleId
            _portalSettings = Entities.Portals.PortalController.Instance.GetCurrentPortalSettings()
            _portalID = _portalSettings.PortalId
            Dim mc As New Entities.Modules.ModuleController
            Dim moduleSettings As Hashtable = Entities.Modules.ModuleController.Instance.GetModule(moduleId, Null.NullInteger, False).ModuleSettings

            'Merge the TabModuleSettings and ModuleSettings
            _settings = New Hashtable

            For Each strKey As String In moduleSettings.Keys
                _settings(strKey) = moduleSettings(strKey)
            Next

            LoadSettings()
        End Sub

        Public Sub New(ByVal moduleid As Integer, ByVal portalid As Integer)
            _moduleId = moduleid
            _portalID = portalid
            Dim mc As New Entities.Modules.ModuleController
            Dim moduleSettings As Hashtable = Entities.Modules.ModuleController.Instance.GetModule(moduleid, Null.NullInteger, False).ModuleSettings
            'Merge the TabModuleSettings and ModuleSettings
            _settings = New Hashtable

            For Each strKey As String In moduleSettings.Keys
                _settings(strKey) = moduleSettings(strKey)
            Next

            LoadSettings()
        End Sub

        Public Sub New(ByVal moduleId As Integer, ByVal settings As Hashtable)
            _moduleId = moduleId
            _portalSettings = PortalController.Instance.GetCurrentPortalSettings()
            _portalID = _portalSettings.PortalId
            _settings = settings

            LoadSettings()
        End Sub

#End Region

#Region "Public Methods"
        Public Sub LoadSettings()

            If Not Settings Is Nothing Then
                _categoryAsSendTo = GetSetting("Feedback_CategoryAsSendTo", False)
                _categorySelect = GetSetting("Feedback_CategorySelect", True)
                _categoryRequired = GetSetting("Feedback_CategoryRequired", False)
                _category = GetSetting("Feedback_Category", String.Empty)
                _moderated = GetSetting("Feedback_Moderated", False)
                _moderationemailstoadmins = GetSetting("Feedback_ModerationEmailsToAdmins", True)
                _scope = GetSetting("Feedback_Scope", Scopes.Portal)
                _moderatedCategories = GetModeratedCategories() 'Handles legacy Feedback_ModCategory setting
                _unmoderatedStatus = GetSetting("Feedback_UnmoderatedStatus", FeedbackInfo.FeedbackStatusType.StatusPublic)
                _emailOnly = GetSetting("Feedback_EmailOnly", False)
                _sendWhenPublished = GetSetting("Feedback_SendWhenPublished", False)
                _moderationPageSize = GetSetting("Feedback_ModerationPageSize", 5)
                _labelDisplayPosition = GetSetting("Feedback_LabelDisplayPosition", LabelDisplayPositions.SameLineAsField)
                _width = System.Web.UI.WebControls.Unit.Parse(GetSetting("Feedback_Width", "100%"))
                _rows = GetSetting("Feedback_Rows", 20)
                _captchaVisibility = GetCaptchaVisibility() 'Handles legacy UseCaptcha setting
                _captchaAudio = GetSetting("Feedback_CaptchaAudio", False)
                _captchaCase = GetSetting("Feedback_CaptchaCase", True)
                _captchaLineNoise = GetCaptchaLineNoise()
                _captchaBackgroundNoise = GetCaptchaBackgroundNoise()

                'Issue #22 NoCaptcha support
                _useNoCaptcha = GetSetting("Feedback_UseNoCaptcha", False)
                _noCaptchaSiteKey = GetSetting("Feedback_NoCaptchaSiteKey", String.Empty)
                _noCaptchaSecretKey = GetSetting("Feedback_NoCaptchaSecretKey", String.Empty)

                _repeatSubmissionFilter = GetSetting("Feedback_RepeatSubmissionFilter", RepeatSubmissionFilters.None)
                _repeatSubmissionInteval = GetSetting("Feedback_RepeatSubmissionInteval", 0)
                _duplicateSubmission = GetSetting("Feedback_DuplicateSubmission", False)
                _subjectFieldType = GetSubjectFieldType() 'Handles legacy Feedback_SubjectEdit setting
                _subject = GetSetting("Feedback_Subject", String.Empty)
                _sendFrom = GetSetting("Feedback_SendFrom", String.Empty)
                _sendTo = GetSetting("Feedback_SendTo", String.Empty)
                _sendToRoles = GetSetting("Feedback_SendToRoles", String.Empty)
                _sendCopy = GetSetting("Feedback_SendCopy", False)
                _optOut = GetSetting("Feedback_OptOut", False)
                _sendAsync = GetSetting("Feedback_SendAsync", False)
                _nameFieldVisibility = GetNameFieldVisibility() 'Handles legacy Feedback_NameField setting
                _messageFieldVisibility = GetSetting("Feedback_MessageFieldVisibility", FieldVisibility.Required)
                _streetFieldVisibility = GetSetting("Feedback_StreetFieldVisibility", FieldVisibility.Hidden)
                _cityFieldVisibility = GetSetting("Feedback_CityFieldVisibility", FieldVisibility.Hidden)
                _regionFieldVisibility = GetSetting("Feedback_RegionFieldVisibility", FieldVisibility.Hidden)
                _countryFieldVisibility = GetSetting("Feedback_CountryFieldVisibility", FieldVisibility.Hidden)
                _postalCodeFieldVisibility = GetSetting("Feedback_PostalCodeFieldVisibility", FieldVisibility.Hidden)
                _telephoneFieldVisibility = GetSetting("Feedback_TelephoneFieldVisibility", FieldVisibility.Hidden)
                _emailFieldVisibility = GetSetting("Feedback_EmailFieldVisibility", FieldVisibility.Required)
                _emailConfirmFieldVisibility = GetSetting("Feedback_EmailConfirmFieldVisibility", FieldVisibility2.Hidden)
                _emailRegex = GetSetting("Feedback_EmailRegex", DefaultEMailRegex)
                _postalCodeRegex = GetSetting("Feedback_PostalCodeRegex", DefaultPostalCodeRegex)
                _telephoneRegex = GetSetting("Feedback_TelephoneRegex", DefaultTelephoneRegex)
                _redirectTabOnSubmission = GetSetting("Feedback_RedirectTabOnSubmission", -1)
                _printTemplate = GetSetting("Feedback_PrintTemplate", DefaultPrintTemplate)
                _printAction = GetSetting("Feedback_PrintAction", PrintActions.InPlace)
                _maxMessage = GetSetting("Feedback_MaxMessage", 1000)
                _cleanupSpam = GetSetting("Feedback_CleanupSpam", False)
                _cleanupPending = GetSetting("Feedback_CleanupPending", False)
                _cleanupPrivate = GetSetting("Feedback_CleanupPrivate", False)
                _cleanupPublished = GetSetting("Feedback_CleanupPublished", False)
                _cleanupArchived = GetSetting("Feedback_CleanupArchived", False)
                _cleanupDaysBefore = GetSetting("Feedback_CleanupDaysBefore", 365)
                _cleanupMaxEntries = GetSetting("Feedback_CleanupMaxEntries", 1000)
                _akismetEnable = GetSetting("Feedback_AkismetEnable", False)
                _akismetKey = GetSetting("Feedback_AkismetKey", String.Empty)
                _akismetSendModerator = GetSetting("Feedback_AkismetSendModerator", False)
            End If
        End Sub

        Private Function GetCaptchaVisibility() As CaptchaVisibilities
            If _settings("Feedback_UseCaptcha") IsNot Nothing Then
                Dim cv As CaptchaVisibilities = CaptchaVisibilities.Disabled
                If CType(_settings("Feedback_UseCaptcha"), Boolean) Then
                    cv = CaptchaVisibilities.AllUsers
                End If
                Dim mc As New Entities.Modules.ModuleController
                With mc
                    .DeleteModuleSetting(_moduleId, "Feedback_UseCaptcha")
                    .UpdateModuleSetting(_moduleId, "Feedback_CaptchaVisibility", cv.ToString)
                End With
            Else
                Return GetSetting("Feedback_CaptchaVisibility", CaptchaVisibilities.Disabled)
            End If
        End Function

        Private Function GetSubjectFieldType() As SubjectFieldTypes
            If _settings("Feedback_SubjectEdit") IsNot Nothing Then
                Dim s As String = _settings("Feedback_SubjectEdit").ToString.ToUpper
                Dim sft As SubjectFieldTypes
                Select Case s
                    Case "FALSE"
                        'this is for legacy feedback modules
                        sft = SubjectFieldTypes.Hidden
                    Case "TRUE"
                        'this is for legacy feedback modules
                        sft = SubjectFieldTypes.Textbox
                    Case "1", "2", "3"
                        'this is for legacy feedback modules
                        sft = CType(s, SubjectFieldTypes)
                    Case Else
                        sft = SubjectFieldTypes.Textbox
                End Select
                Dim mc As New Entities.Modules.ModuleController
                With mc
                    .DeleteModuleSetting(_moduleId, "Feedback_SubjectEdit")
                    .UpdateModuleSetting(_moduleId, "Feedback_SubjectFieldType", sft.ToString)
                End With
                Return sft
            Else
                Return GetSetting("Feedback_SubjectFieldType", SubjectFieldTypes.Textbox)
            End If
        End Function

        Private Function GetNameFieldVisibility() As FieldVisibility
            If _settings("FeedBack_RequireName") IsNot Nothing Then
                Dim nfv As FieldVisibility
                If CType(_settings("FeedBack_RequireName"), Boolean) Then
                    nfv = FieldVisibility.Required
                Else
                    nfv = FieldVisibility.Optional
                End If
                Dim mc As New Entities.Modules.ModuleController
                With mc
                    .DeleteModuleSetting(_moduleId, "Feedback_RequireName")
                    .UpdateModuleSetting(_moduleId, "Feedback_NameFieldVisibility", nfv.ToString)
                End With
                Return nfv
            Else
                Return GetSetting("Feedback_NameFieldVisibility", FieldVisibility.Optional)
            End If
        End Function

        Private Function GetModeratedCategories() As String
            If _settings("Feedback_ModCategory") IsNot Nothing Then
                Dim modCategory As String = CType(_settings("Feedback_ModCategory"), String)
                Dim mc As New Entities.Modules.ModuleController
                With mc
                    .DeleteModuleSetting(_moduleId, "Feedback_ModCategory")
                    .UpdateModuleSetting(_moduleId, "Feedback_ModeratedCategories", modCategory)
                End With
                Return modCategory
            Else
                Return GetSetting("Feedback_ModeratedCategories", String.Empty)
            End If
        End Function

        Private Function GetCaptchaLineNoise() As Telerik.Web.UI.CaptchaLineNoiseLevel
            If _settings("Feedback_CaptchaLineNoise") IsNot Nothing Then
                Return CType(_settings("Feedback_CaptchaLineNoise"), Telerik.Web.UI.CaptchaLineNoiseLevel)
            Else
                Return Telerik.Web.UI.CaptchaLineNoiseLevel.Low
            End If
        End Function

        Private Function GetCaptchaBackgroundNoise() As Telerik.Web.UI.CaptchaBackgroundNoiseLevel
            If _settings("Feedback_CaptchaBackgroundNoise") IsNot Nothing Then
                Return CType(_settings("Feedback_CaptchaBackgroundNoise"), Telerik.Web.UI.CaptchaBackgroundNoiseLevel)
            Else
                Return Telerik.Web.UI.CaptchaBackgroundNoiseLevel.Low
            End If
        End Function

        Public Sub SaveSettings()
            SaveSettings(_moduleId)
        End Sub

        Public Sub SaveSettings(ByVal inModuleId As Integer)
            _moduleId = inModuleId
            Dim mc As New Entities.Modules.ModuleController
            With mc
                .UpdateModuleSetting(_moduleId, "Feedback_CategoryAsSendTo", _categoryAsSendTo.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CategorySelect", _categorySelect.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CategoryRequired", _categoryRequired.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_Category", _category)
                .UpdateModuleSetting(_moduleId, "Feedback_Moderated", _moderated.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_ModerationEmailsToAdmins", _moderationemailstoadmins.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_Scope", _scope.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_ModeratedCategories", _moderatedCategories)
                .UpdateModuleSetting(_moduleId, "Feedback_UnmoderatedStatus", _unmoderatedStatus.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_EmailOnly", _emailOnly.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_ModerationPageSize", ModerationPageSize.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_SendWhenPublished", _sendWhenPublished.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_LabelDisplayPosition", _labelDisplayPosition.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_Width", _width.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_Rows", _rows.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CaptchaVisibility", _captchaVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CaptchaAudio", _captchaAudio.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CaptchaCase", _captchaCase.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CaptchaLineNoise", CInt(_captchaLineNoise).ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CaptchaBackgroundNoise", CInt(_captchaBackgroundNoise).ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_DuplicateSubmission", _duplicateSubmission.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_RepeatSubmissionFilter", _repeatSubmissionFilter.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_RepeatSubmissionInteval", _repeatSubmissionInteval.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_SubjectFieldType", _subjectFieldType.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_Subject", _subject)
                .UpdateModuleSetting(_moduleId, "Feedback_SendTo", _sendTo)
                .UpdateModuleSetting(_moduleId, "Feedback_SendToRoles", _sendToRoles)
                .UpdateModuleSetting(_moduleId, "Feedback_SendFrom", _sendFrom)
                .UpdateModuleSetting(_moduleId, "Feedback_SendCopy", _sendCopy.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_OptOut", _optOut.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_SendAsync", _sendAsync.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_NameFieldVisibility", _nameFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_MessageFieldVisibility", _messageFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_StreetFieldVisibility", _streetFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CityFieldVisibility", _cityFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_RegionFieldVisibility", _regionFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CountryFieldVisibility", _countryFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_PostalCodeFieldVisibility", _postalCodeFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_TelephoneFieldVisibility", _telephoneFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_EmailFieldVisibility", _emailFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_EmailConfirmFieldVisibility", _emailConfirmFieldVisibility.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_EmailRegex", _emailRegex)
                .UpdateModuleSetting(_moduleId, "Feedback_PostalCodeRegex", _postalCodeRegex)
                .UpdateModuleSetting(_moduleId, "Feedback_TelephoneRegex", _telephoneRegex)
                .UpdateModuleSetting(_moduleId, "Feedback_RedirectTabOnSubmission", _redirectTabOnSubmission.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_PrintTemplate", _printTemplate)
                .UpdateModuleSetting(_moduleId, "Feedback_PrintAction", _printAction.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_MaxMessage", _maxMessage.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupSpam", _cleanupSpam.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupPending", _cleanupPending.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupPrivate", _cleanupPrivate.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupPublished", _cleanupPublished.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupArchived", _cleanupArchived.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupDaysBefore", _cleanupDaysBefore.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_CleanupMaxEntries", _cleanupMaxEntries.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_AkismetEnable", _akismetEnable.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_AkismetKey", _akismetKey)
                .UpdateModuleSetting(_moduleId, "Feedback_AkismetSendModerator", _akismetSendModerator.ToString)

                ' Issue #22 - add NoCaptcha support
                .UpdateModuleSetting(_moduleId, "Feedback_UseNoCaptcha", _useNoCaptcha.ToString)
                .UpdateModuleSetting(_moduleId, "Feedback_NoCaptchaSiteKey", _noCaptchaSiteKey)
                .UpdateModuleSetting(_moduleId, "Feedback_NoCaptchaSecretKey", _noCaptchaSecretKey)


            End With
            Entities.Modules.ModuleController.SynchronizeModule(_moduleId)
        End Sub

#End Region

#Region "Private Methods"

        Private Function GetSetting(Of T)(ByVal key As String, ByVal defaultValue As T) As T

            If _settings.ContainsKey(key) Then
                Dim obj As Object
                obj = _settings(key)
                If TypeOf defaultValue Is System.Enum Then
                    Try
                        Return CType([Enum].Parse(GetType(T), CType(obj, String)), T)
                    Catch ex As ArgumentException
                        Return defaultValue
                    End Try
                ElseIf TypeOf defaultValue Is DateTime Then
                    Dim objDateTime As Object
                    Try
                        objDateTime = DateTime.Parse(CType(obj, String))
                    Catch ex As FormatException
                        Dim dt As DateTime
                        If Not DateTime.TryParse(CType(obj, String), Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None, dt) Then
                            dt = DateTime.Now
                        End If
                        objDateTime = dt
                    End Try
                    Return CType(objDateTime, T)
                Else
                    Try
                        Return CType(obj, T)
                    Catch ex As InvalidCastException
                        Return defaultValue
                    End Try
                End If
            Else
                Return defaultValue
            End If
        End Function

#End Region

    End Class

End Namespace
