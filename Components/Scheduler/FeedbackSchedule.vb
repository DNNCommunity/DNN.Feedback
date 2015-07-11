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
Imports System.Collections
Imports DotNetNuke.Entities.Modules

Namespace Components.Scheduler
    Public Class FeedbackSchedule
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

#Region "Constructors"
        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            ScheduleHistoryItem = objScheduleHistoryItem
        End Sub
#End Region

#Region "Methods"
        Public Overrides Sub DoWork()
            Try
                'notification that the event is progressing
                Progressing()        'OPTIONAL

                Dim returnStrCleanup As String
                returnStrCleanup = CleanupFeedback()
                If returnStrCleanup <> "" Then
                    ScheduleHistoryItem.AddLogNote("<br />" & returnStrCleanup & "<br />")
                End If

                ScheduleHistoryItem.Succeeded = True     'REQUIRED

            Catch exc As Exception      'REQUIRED
                ScheduleHistoryItem.Succeeded = False    'REQUIRED
                ScheduleHistoryItem.AddLogNote("Feedback schedule failed." + exc.ToString + Status)    'OPTIONAL
                'notification that we have errored
                Errored(exc)         'REQUIRED
                'log the exception
                LogException(exc)       'OPTIONAL
            End Try
        End Sub

        Private Function CleanupFeedback() As String
            Const returnStr As String = "Feedback Cleanup completed."

            Status = "Performing Feedback Cleanup"

            Dim objDesktopModule As Entities.Modules.DesktopModuleInfo
            objDesktopModule = DesktopModuleController.GetDesktopModuleByModuleName("DNN_Feedback", 0)

            If objDesktopModule Is Nothing Then
                Return "Module Definition 'DNN_Feedback' not found. Cleanup cannot be performed."
            End If

            Status = "Performing Feedback Cleanup:" + objDesktopModule.FriendlyName

            Dim objPortals As New Entities.Portals.PortalController
            Dim objModules As New Entities.Modules.ModuleController

            Dim lstportals As ArrayList = objPortals.GetPortals()
            For Each objPortal As Entities.Portals.PortalInfo In lstportals
                Status = "Performing Feedback Cleanup:" + objDesktopModule.FriendlyName + " PortalID:" + objPortal.PortalID.ToString

                Dim lstModules As ArrayList = objModules.GetModulesByDefinition(objPortal.PortalID, objDesktopModule.FriendlyName)
                For Each objModule As Entities.Modules.ModuleInfo In lstModules
                    ' This check for objModule = Nothing because of bug in DNN 5.0.0 in GetModulesByDefinition
                    If objModule Is Nothing Then
                        Continue For
                    End If
                    Status = "Performing Feedback Cleanup:" + objDesktopModule.FriendlyName + " PortalID:" + objPortal.PortalID.ToString + " ModuleID:" + objModule.ModuleID.ToString

                    Dim moduleConfiguration As New DotNetNuke.Modules.Feedback.Configuration(objModule.ModuleID, objPortal.PortalID)
                    Dim objFeedbackController As New DotNetNuke.Modules.Feedback.FeedbackController
                    With moduleConfiguration
                        objFeedbackController.CleanupFeedback(objModule.ModuleID, .CleanupPending, .CleanupPrivate, .CleanupPublished, .CleanupArchived, .CleanupSpam, .CleanupDaysBefore, .CleanupMaxEntries)
                    End With
                Next
            Next
            Status = "Cleanup complete"
            Return returnStr
        End Function

#End Region

    End Class

End Namespace
