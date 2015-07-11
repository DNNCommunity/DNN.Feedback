'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2012
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

Imports DotNetNuke.Services.Social.Notifications
Imports DotNetNuke.Security.Roles


Namespace Components.Integration
    Public Class Notifications
        Public Const ContentTypeName As String = "DNN_Feedback"
        Public Const NotificationModerationRequiredTypeName As String = "FeedbackModeration"

        ''' <summary>
        ''' Informs the core journal that the user that a feedback entry needs moderation
        ''' </summary>
        ''' <param name="sendToRoles"></param>
        ''' <remarks></remarks>
        Friend Function SendFeedbackModerationNotification(ByVal sendToRoles() As String, ByVal senderUserID As Integer, ByVal subject As String, ByVal body As String, _
                                                           ByVal portalID As Integer, ByVal tabID As Integer, ByVal moduleID As Integer, ByVal feedbackID As Integer) As String
            Dim notificationType As NotificationType = NotificationsController.Instance.GetNotificationType(NotificationModerationRequiredTypeName)
            If notificationType Is Nothing Then
                notificationType = CreateFeedbackNotificationTypes(portalID)
            End If

            Dim notificationKey As String = String.Format("{0}:{1}:{2}:{3}:{4}", ContentTypeName + NotificationModerationRequiredTypeName, portalID, tabID, moduleID, feedbackID)
            Dim objNotification As New Notification
            Dim objUserController As New UserController()
            Dim senderUser As UserInfo = objUserController.GetUser(portalID, senderUserID)

            objNotification.NotificationTypeID = notificationType.NotificationTypeId
            objNotification.Subject = subject
            objNotification.Body = body
            objNotification.IncludeDismissAction = True
            objNotification.SenderUserID = senderUserID
            objNotification.Context = notificationKey
            objNotification.From = senderUser.DisplayName

            Dim colRoles As New Generic.List(Of RoleInfo)
            Dim objRoleController As New RoleController
            Dim colUsers As New Generic.List(Of UserInfo)
            For Each roleName As String In sendToRoles
                If Left(roleName, 1) = "[" And Right(roleName, 1) = "]" Then
                    Dim objUser As UserInfo = objUserController.GetUser(portalID, CInt(Mid(roleName, 2, Len(roleName) - 2)))
                    colUsers.Add(objUser)
                Else
                    Dim objRole As RoleInfo = objRoleController.GetRoleByName(portalID, roleName)
                    colRoles.Add(objRole)
                End If
            Next

            NotificationsController.Instance.SendNotification(objNotification, portalID, colRoles, colUsers)
            Return notificationKey
        End Function

        Private Function CreateFeedbackNotificationTypes(ByVal portalID As Integer) As NotificationType
            Dim deskModuleId As Integer = Entities.Modules.DesktopModuleController.GetDesktopModuleByModuleName("DNN_Feedback", portalID).DesktopModuleID
            Dim objNotificationType As NotificationType = New NotificationType
            objNotificationType.Name = NotificationModerationRequiredTypeName
            objNotificationType.Description = "Feedback Moderation Required"
            objNotificationType.DesktopModuleId = deskModuleId
            NotificationsController.Instance.CreateNotificationType(objNotificationType)

            Dim actions As Generic.List(Of NotificationTypeAction) = New Generic.List(Of NotificationTypeAction)
            Dim objAction As New NotificationTypeAction
            objAction.NameResourceKey = "ApproveFeedback"
            objAction.DescriptionResourceKey = "ApproveFeedback_Desc"
            objAction.ConfirmResourceKey = "ApproveFeedback_Confirm"
            objAction.APICall = "DesktopModules/Feedback/API/NotificationService.ashx/ApproveFeedback"
            objAction.Order = 1
            actions.Add(objAction)

            objAction = New NotificationTypeAction
            objAction.NameResourceKey = "PrivateFeedback"
            objAction.DescriptionResourceKey = "PrivateFeedback_Desc"
            objAction.ConfirmResourceKey = "PrivateFeedback_Confirm"
            objAction.APICall = "DesktopModules/Feedback/API/NotificationService.ashx/PrivateFeedback"
            objAction.Order = 2
            actions.Add(objAction)

            objAction = New NotificationTypeAction
            objAction.NameResourceKey = "ArchiveFeedback"
            objAction.DescriptionResourceKey = "ArchiveFeedback_Desc"
            objAction.ConfirmResourceKey = "ArchiveFeedback_Confirm"
            objAction.APICall = "DesktopModules/Feedback/API/NotificationService.ashx/ArchiveFeedback"
            objAction.Order = 3
            actions.Add(objAction)

            objAction = New NotificationTypeAction
            objAction.NameResourceKey = "DeleteFeedback"
            objAction.DescriptionResourceKey = "DeleteFeedback_Desc"
            objAction.ConfirmResourceKey = "DeleteFeedback_Confirm"
            objAction.APICall = "DesktopModules/Feedback/API/NotificationService.ashx/DeleteFeedback"
            objAction.Order = 4
            actions.Add(objAction)

            NotificationsController.Instance.SetNotificationTypeActions(actions, objNotificationType.NotificationTypeId)

            Return objNotificationType

        End Function

        Public Sub DeleteNotification(ByVal contextKey As String, ByVal notificationId As Integer)
            If NotificationId = Nothing Then
                Dim notificationType As NotificationType = NotificationsController.Instance.GetNotificationType(NotificationModerationRequiredTypeName)
                Dim oNotifications As System.Collections.Generic.IList(Of Notification) = NotificationsController.Instance().GetNotificationByContext(notificationType.NotificationTypeId, contextKey)
                For Each oNotification As Notification In oNotifications
                    NotificationsController.Instance().DeleteNotification(oNotification.NotificationID)
                Next
            Else
                NotificationsController.Instance().DeleteNotification(NotificationId)
            End If

        End Sub
    End Class
End Namespace
