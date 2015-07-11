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

Imports DotNetNuke.Services.Mail
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Feedback

    Public Class FeedbackEmail

        Private _sendMethod As SendMethods = SendMethods.BCC
        Private _mailPriority As MailPriority = MailPriority.Normal

        Private _sendToEmail As String = String.Empty
        Private _sendToRoles As String = String.Empty
        Private _sendFromEmail As String = String.Empty
        Private _replyToEmail As String = String.Empty
        Private _removeDuplicates As Boolean = True
        Private _replaceTokens As Boolean = True
        Private _mailFormat As MailFormat = MailFormat.Text
        Private _subject As String
        Private _message As String

        Private _feedbackPropertySource As FeedbackInfo
        Private ReadOnly _portalSettings As Entities.Portals.PortalSettings
        Private ReadOnly _recipients As Generic.List(Of String)

        Private Shared ReadOnly IsHtmlRegex As New Regex("<\w*[^>]*>", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

        Public Enum SendMethods As Integer
            [To] = 1
            [CC]
            [BCC]
        End Enum

        Public Property SendMethod() As SendMethods
            Get
                Return _sendMethod
            End Get
            Set(ByVal value As SendMethods)
                _sendMethod = value
            End Set
        End Property

        Public Property MailPriority() As MailPriority
            Get
                Return _mailPriority
            End Get
            Set(ByVal value As MailPriority)
                _mailPriority = value
            End Set
        End Property

        Public Property SendToEmail() As String
            Get
                Return _sendToEmail
            End Get
            Set(ByVal value As String)
                _sendToEmail = value
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

        Public Property SendFromEmail() As String
            Get
                Return _sendFromEmail
            End Get
            Set(ByVal value As String)
                _sendFromEmail = value
            End Set
        End Property

        Public Property ReplyToEmail() As String
            Get
                Return _replyToEmail
            End Get
            Set(ByVal value As String)
                _replyToEmail = value
            End Set
        End Property

        Public Property RemoveDuplicates() As Boolean
            Get
                Return _removeDuplicates
            End Get
            Set(ByVal value As Boolean)
                _removeDuplicates = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then Throw New NullReferenceException("Subject must not be null or empty string")
                _subject = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then Throw New NullReferenceException("Message must not be null")
                If IsHTMLMail(value) Then
                    value = ConvertCRLFToHTML(value)
                    _mailFormat = MailFormat.Html
                Else
                    _mailFormat = MailFormat.Text
                End If
                _message = value
            End Set
        End Property

        Public Property MailFormat() As MailFormat
            Get
                Return _mailFormat
            End Get
            Set(ByVal value As MailFormat)
                _mailFormat = value
            End Set
        End Property

        Public Property FeedbackPropertySource() As FeedbackInfo
            Get
                Return _feedbackPropertySource
            End Get
            Set(ByVal value As FeedbackInfo)
                _feedbackPropertySource = value
            End Set
        End Property

        Public Property ReplaceTokens() As Boolean
            Get
                Return _replaceTokens And (_feedbackPropertySource IsNot Nothing)
            End Get
            Set(ByVal value As Boolean)
                _replaceTokens = value
            End Set
        End Property

        Public ReadOnly Property Recipients() As Generic.List(Of String)
            Get
                Return _recipients
            End Get
        End Property

        Public Sub New()
            _portalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings()
            _recipients = New Generic.List(Of String)
        End Sub

        Public Sub New(ByVal feedbackPropertySource As FeedbackInfo)
            Me.New()
            _feedbackPropertySource = feedbackPropertySource
        End Sub

        Public Sub ClearRecipients()
            _recipients.Clear()
        End Sub

        Public Sub AddUserToRecipients(ByVal userId As Integer)
            If (userId > 0) Then
                Dim user As UserInfo = UserController.GetUserById(_portalSettings.PortalId, userId)
                If user IsNot Nothing AndAlso (user.Membership.Approved _
                    AndAlso Not user.IsDeleted) Then
                    AddEmailToRecipients(user.Email)
                End If
            End If
        End Sub

        Public Overloads Sub AddEmailToRecipients(ByVal email As String)
            If Not (String.IsNullOrEmpty(email) OrElse _recipients.Contains(email)) Then
                _recipients.Add(email)
            End If
        End Sub

        Public Overloads Sub AddEmailToRecipients(ByVal emailAddresses As String())
            For Each email As String In emailAddresses
                AddEmailToRecipients(email)
            Next
        End Sub

        Public Sub AddUsersInRolesToRecipients(ByVal roleNames As String())
            Dim rgx As New Regex("^\[(\d+)\]$", RegexOptions.Compiled)
            For Each roleName As String In roleNames
                If roleName <> String.Empty Then
                    Dim m As Match = rgx.Match(roleName)
                    If m.Success Then
                        Dim uid As Integer = Integer.Parse(m.Groups(1).Value)
                        AddUserToRecipients(uid)
                    Else
                        Dim rc As New DotNetNuke.Security.Roles.RoleController
                        For Each user As UserInfo In rc.GetUsersByRoleName(_portalSettings.PortalId, roleName)
                            AddUserToRecipients(user.UserID)
                        Next
                    End If
                End If
            Next
        End Sub

        Public Function SendEmail() As String

            AddEmailToRecipients(SendToEmail.Split(";"c))
            AddUsersInRolesToRecipients(SendToRoles.Split(";"c))

            If Subject = String.Empty OrElse _recipients.Count = 0 Then Return String.Empty

            Dim tokenizedMessage As String

            If ReplaceTokens Then
                Dim tr As New FeedbackTokenReplace(_feedbackPropertySource)
                tokenizedMessage = tr.ReplaceFeedbackTokens(_message)
            Else
                tokenizedMessage = _message
            End If

            If tokenizedMessage = String.Empty Then
                Return String.Empty
            End If

            If MailFormat = MailFormat.Html Then
                _message = "<base href='" & AddHTTP(_portalSettings.PortalAlias.HTTPAlias) & "' />" & _message   'Add Base Href for any images inserted in to the email.
            End If

            Dim bcc As New StringBuilder
            Dim cc As New StringBuilder
            Dim [to] As New StringBuilder(_recipients(0)) 'Get the first (and perhaps only) recipient email
            Dim i As Integer

            Select Case SendMethod
                Case SendMethods.To
                    For i = 1 To _recipients.Count - 1
                        [to].Append(";")
                        [to].Append(_recipients(i))
                    Next
                Case SendMethods.CC
                    For i = 1 To _recipients.Count - 1
                        cc.Append(_recipients(i))
                        cc.Append(";")
                    Next
                    If cc.Length > 0 Then cc.Length -= 1 'remove trailing comma
                Case SendMethods.BCC
                    For i = 1 To _recipients.Count - 1
                        bcc.Append(_recipients(i))
                        bcc.Append(";")
                    Next
                    If bcc.Length > 0 Then bcc.Length -= 1 'remove trailing comma
            End Select

            Dim smtpEnableSsl As Boolean = Entities.Controllers.HostController.Instance.GetSettingsDictionary("SMTPEnableSSL") = "Y"
            If Not String.IsNullOrEmpty(ReplyToEmail) OrElse (SendMethod = SendMethods.CC AndAlso cc.Length > 0) OrElse smtpEnableSsl Then
                Return SendMail(SendFromEmail, [to].ToString, cc.ToString, bcc.ToString, ReplyToEmail, MailPriority.Normal, Subject, MailFormat, Encoding.UTF8, Message, smtpEnableSsl)
            Else
                Return Mail.SendMail(SendFromEmail, [to].ToString, bcc.ToString, Subject, Message, "", "", "", "", "", "")
            End If
        End Function

        Private Function SendMail(ByVal mailFrom As String, ByVal mailTo As String, _
            ByVal cc As String, ByVal bcc As String, ByVal replyTo As String, _
            ByVal priority As MailPriority, ByVal inSubject As String, _
            ByVal bodyFormat As MailFormat, ByVal bodyEncoding As Encoding, ByVal body As String, _
            ByVal smtpEnableSsl As Boolean) As String

            Dim rslt As String

            'Note: DNN 5.3.0 - DNN 5.5.0 SendMail overload which includes Attachments parameter as string array
            'ignore passed in ReplyTo parameter substituting it with MailFrom parameter value. Only the overload with
            'Attachments as generic list of System.Net.Mail.Attachment honors the ReplyTo parameter.

            Dim attachments As New Generic.List(Of Net.Mail.Attachment)

            Try
                rslt = Mail.SendMail(mailFrom, mailTo, cc, bcc, replyTo, priority, inSubject, bodyFormat, bodyEncoding, body, attachments, "", "", "", "", smtpEnableSsl)
            Catch ex As Exception
                If Not IsNothing(ex.InnerException) Then
                    rslt = String.Concat(ex.Message, ControlChars.CrLf, ex.InnerException.Message)
                    LogException(ex.InnerException)
                Else
                    rslt = ex.Message
                    LogException(ex)
                End If
            End Try
            Return rslt
        End Function

        Private Function IsHTMLMail(ByVal s As String) As Boolean
            Return (Not String.IsNullOrEmpty(s)) AndAlso IsHtmlRegex.IsMatch(s)
        End Function

        Public Shared Function ConvertToText(ByVal s As String) As String
            s = HtmlUtils.FormatText(s, True)
            Return HtmlUtils.StripTags(s, True)
        End Function

        Public Shared Function ConvertCrlftoHTML(ByVal s As String) As String
            If Not String.IsNullOrEmpty(s) Then s = Replace(s, vbCrLf, "<br />")
            Return s
        End Function

    End Class

End Namespace