'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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
Option Strict On
Option Explicit On

Imports DotNetNuke.Services.Social.Notifications
Imports DotNetNuke.Services.Social
Imports DotNetNuke.Modules.Feedback
Imports DotNetNuke.Web.Api
Imports System.Net.Http

Namespace Components.Services

    Public Class NotificationServiceController
        Inherits DnnApiController

#Region "Private Members"
        Private _moduleId As Integer = -1
        Private _portalId As Integer = -1
        Private _tabId As Integer = -1
        Private _feedbackId As Integer = -1
        Private _moduleConfiguration As DotNetNuke.Modules.Feedback.Configuration = Nothing
        Private Const LocalResourceFile As String = "DesktopModules/Feedback/App_LocalResources/NotificationService"

#End Region

        <DnnAuthorize()> _
        Public Function ApproveFeedback(notificationId As Integer) As HttpResponseMessage
            Return ChangeStatus(notificationId, FeedbackInfo.FeedbackStatusType.StatusPublic)
        End Function

        <DnnAuthorize()> _
        Public Function PrivateFeedback(notificationId As Integer) As HttpResponseMessage
            Return ChangeStatus(notificationId, FeedbackInfo.FeedbackStatusType.StatusPrivate)
        End Function

        <DnnAuthorize()> _
        Public Function ArchiveFeedback(notificationId As Integer) As HttpResponseMessage
            Return ChangeStatus(notificationId, FeedbackInfo.FeedbackStatusType.StatusArchive)
        End Function

        <DnnAuthorize()> _
        Public Function DeleteFeedback(notificationId As Integer) As HttpResponseMessage
            Return ChangeStatus(notificationId, FeedbackInfo.FeedbackStatusType.StatusDelete)
        End Function


#Region "Private Methods"
        Private Function ChangeStatus(ByVal notificationid As Integer, ByVal feedbackStatus As FeedbackInfo.FeedbackStatusType) As HttpResponseMessage
            Dim recipient As Messaging.MessageRecipient = Messaging.Internal.InternalMessagingController.Instance.GetMessageRecipient(notificationid, UserInfo.UserID)
            If recipient Is Nothing Then
                Return Request.CreateResponse(System.Net.HttpStatusCode.Accepted, Json(New With {.Result = "error", .Message = String.Format(Localization.GetString("NotificationRecipientError", LocalResourceFile), UserInfo.DisplayName)}))
            End If

            Dim notify As Notification = NotificationsController.Instance.GetNotification(notificationid)
            ParseApproveKey(notify.Context)

            If Not IsModerator() Then
                Return Request.CreateResponse(System.Net.HttpStatusCode.Accepted, Json(New With {.Result = "error", .Message = String.Format(Localization.GetString("NotificationSecurityError", LocalResourceFile), UserInfo.DisplayName)}))
            End If

            Dim objFeedbackUpdateController As New FeedbackUpdateController(_moduleId, _moduleConfiguration, PortalSettings, LocalResourceFile, UserInfo.UserID)
            objFeedbackUpdateController.FeedbackUpdateStatus(_moduleId, _feedbackId, feedbackStatus, notificationid)

            Return Request.CreateResponse(System.Net.HttpStatusCode.Accepted, Json(New With {.Result = "success"}))
        End Function

        Private Function IsModerator() As Boolean
            'Check security here
            Dim modSecurity As New Security.ModuleSecurity(_moduleId, _tabId)
            If Not modSecurity.IsAllowedToModeratePosts() Then
                Return False
            End If
            Return True

        End Function

        Private Sub ParseApproveKey(key As String)
            Dim keys() As String = key.Split(CChar(":"))
            ' 0 is content type string, to ensure unique key
            If keys.GetLength(0) = 5 Then
                _portalId = Integer.Parse(keys(1))
                _tabId = Integer.Parse(keys(2))
                _moduleId = Integer.Parse(keys(3))
                _feedbackId = Integer.Parse(keys(4))
                _moduleConfiguration = New DotNetNuke.Modules.Feedback.Configuration(_moduleId, _portalId)
            Else
                _tabId = Integer.Parse(keys(1))
                _moduleId = Integer.Parse(keys(2))
                _feedbackId = Integer.Parse(keys(3))
            End If
        End Sub

#End Region

    End Class

End Namespace