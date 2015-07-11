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

Imports System.Data
Imports System.Text
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Feedback
#Region "FeedbackController Class"
    Public Class FeedbackController
        Implements IUpgradeable

        Public Function GetLastSubmissionDateForUserId(ByVal portalID As Integer, ByVal userId As Integer) As DateTime
            Return DataProvider.Instance().GetLastSubmissionDateForUserId(portalID, userId)
        End Function

        Public Function GetLastSubmissionDateForUserIP(ByVal portalID As Integer, ByVal remoteAddr As String) As DateTime
            Return DataProvider.Instance().GetLastSubmissionDateForUserIP(portalID, remoteAddr)
        End Function

        Public Function GetLastSubmissionDateForUserEmail(ByVal portalID As Integer, ByVal email As String) As DateTime
            Return DataProvider.Instance().GetLastSubmissionDateForUserEmail(portalID, email)
        End Function

        Public Function GetDuplicateSubmissionForUserEmail(ByVal portalID As Integer, ByVal email As String, ByVal message As String) As DateTime
            Return DataProvider.Instance().GetDuplicateSubmissionForUserEmail(portalID, email, message)
        End Function

        Public Sub CreateFeedback(ByVal o As FeedbackInfo, ByVal userId As Integer)
            With o
                o.FeedbackID = DataProvider.Instance().CreateFeedback(.PortalID, .ModuleID, .CategoryID, .SenderEmail, .Status, .Message, .Subject, .SenderName, _
                                                       .SenderStreet, .SenderCity, .SenderRegion, .SenderCountry, .SenderPostalCode, .SenderTelephone, .SenderRemoteAddr, userId, .Referrer, .UserAgent, .ContextKey)
            End With
        End Sub

        Public Function GetCategoryFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal status As FeedbackInfo.FeedbackStatusType, ByVal currentPage As Integer, ByVal pageSize As Integer) As ArrayList
            Return GetCategoryFeedback(portalID, moduleID, categoryID, status, currentPage, pageSize, "CreatedOnDate DESC")
        End Function

        Public Function GetCategoryFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal status As FeedbackInfo.FeedbackStatusType, ByVal currentPage As Integer, ByVal pageSize As Integer, ByVal orderBy As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetCategoryFeedback(portalID, moduleID, categoryID, status, currentPage, pageSize, orderBy), GetType(FeedbackInfo))
        End Function

        Public Function GetFeedback(ByVal feedbackID As Integer) As FeedbackInfo
            Return CBO.FillObject(Of FeedbackInfo)(DataProvider.Instance().GetFeedback(feedbackID))
        End Function

        Public Sub UpdateFeedback(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal subject As String, ByVal message As String, ByVal userId As Integer)
            DataProvider.Instance().UpdateFeedback(moduleID, feedbackID, subject, message, userId)
        End Sub

        Public Sub UpdateContextKey(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal contextKey As String, ByVal userId As Integer)
            DataProvider.Instance().UpdateContextKey(moduleID, feedbackID, contextKey, userId)
        End Sub

        Public Sub UpdateFeedbackStatus(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal status As FeedbackInfo.FeedbackStatusType, ByVal userId As Integer)
            DataProvider.Instance().UpdateFeedbackStatus(moduleID, feedbackID, status, userId)
        End Sub

        Public Function AddFeedbackList(ByVal o As FeedbackList) As Integer
            DataProvider.Instance().AddFeedbackList(o.PortalID, o.ListType, o.Name, o.ListValue, (CType(o.IsActive, String) <> "0"), o.Portal, o.ModuleID)
        End Function

        Public Function GetFeedbackList(ByVal singleRowOperation As Boolean, ByVal portalID As Integer, ByVal listID As Integer, ByVal listType As FeedbackList.Type, ByVal activeOnly As Boolean, ByVal moduleID As Integer, ByVal allList As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetFeedbackList(singleRowOperation, portalID, listID, listType, activeOnly, moduleID, allList), GetType(FeedbackList))
        End Function

        Public Sub EditFeedbackList(ByVal isDeleteOperation As Boolean, ByVal o As FeedbackList)
            DataProvider.Instance().EditFeedbackList(isDeleteOperation, o.ListID, o.PortalID, o.ListType, o.Name, o.ListValue, (CType(o.IsActive, String) <> "0"), o.Portal, o.ModuleID)
        End Sub

        Public Sub ChangeListSortOrder(ByVal listID As Integer, ByVal listType As FeedbackList.Type, ByVal oldSortNum As Integer, ByVal newSortNum As Integer)
            DataProvider.Instance().ChangeSortOrder(listID, listType, oldSortNum, newSortNum)
        End Sub

        Public Function GetOrphanedData() As Generic.Dictionary(Of String, Integer)
            Dim orphanedData As New Generic.Dictionary(Of String, Integer)
            Dim dr As IDataReader = Nothing
            Try
                dr = DataProvider.Instance().GetOrphanedData()
                While dr.Read
                    orphanedData.Add(Null.SetNullString(dr("ModuleId")), Null.SetNullInteger(dr("ItemCount")))
                End While
            Finally
                If dr IsNot Nothing Then
                    dr.Close()
                End If
            End Try
            Return orphanedData
        End Function

        Public Sub DeleteOrphanedData()
            DataProvider.Instance().DeleteOrphanedData()
        End Sub

        Public Sub CleanupFeedback(ByVal moduleID As Integer, ByVal cleanupPending As Boolean, ByVal cleanupPrivate As Boolean, ByVal cleanupPublished As Boolean, ByVal cleanupArchived As Boolean, ByVal cleanupSpam As Boolean, ByVal daysBefore As Integer, ByVal maxEntries As Integer)
            DataProvider.Instance().CleanupFeedback(moduleID, cleanupPending, cleanupPrivate, cleanupPublished, cleanupArchived, cleanupSpam, daysBefore, maxEntries)
        End Sub

        Public Function FeedbackListItemExists(ByVal o As FeedbackList) As Boolean
            Dim success As Boolean = False

            'Grab a list of all the feedback items for the current list type from this portal.
            Dim feedbackArryList As ArrayList = GetFeedbackList(False, o.PortalID, -99, o.ListType, False, o.ModuleID, False)
            ' ReSharper disable LoopCanBeConvertedToQuery
            For Each listItem As FeedbackList In feedbackArryList
                ' ReSharper restore LoopCanBeConvertedToQuery
                If listItem.Name.ToLower = o.Name.ToLower() Then
                    success = True
                    Exit For
                End If
            Next
            Return success
        End Function

        Public Function VerifyNoDuplicateFeedbackListItemName(ByVal o As FeedbackList) As Boolean
            Dim success As Boolean = True

            'Grab a list of all the feedback items for the current list type from this portal.
            Dim feedbackArryList As ArrayList = GetFeedbackList(False, o.PortalID, -99, o.ListType, False, o.ModuleID, False)
            ' ReSharper disable LoopCanBeConvertedToQuery
            For Each listItem As FeedbackList In feedbackArryList
                ' ReSharper restore LoopCanBeConvertedToQuery
                If listItem.Name.ToLower = o.Name.ToLower() And (o.ListID <> listItem.ListID) Then
                    success = False
                    Exit For
                End If
            Next
            Return success
        End Function

        Public Function GetFeedbackModules(ByVal portalID As Integer, ByVal desktopModuleID As Integer, ByVal selectedModules As String) As Generic.List(Of AggregatedModuleInfo)
            Dim feedbackModules As New Generic.List(Of AggregatedModuleInfo)
            Dim selectedFeedbackModules As Array = selectedModules.Split(";"c)
            Dim mc As New ModuleController
            Dim tc As New Entities.Tabs.TabController
            Dim modules As ArrayList = mc.GetModules(portalID)
            For Each m As ModuleInfo In modules
                If m.DesktopModuleID = desktopModuleID AndAlso m.ModuleDefinition.FriendlyName = "Feedback" Then
                    Dim feedbackModule As New AggregatedModuleInfo
                    With feedbackModule
                        .ModuleId = m.ModuleID
                        .TabId = m.TabID
                        .TabName = tc.GetTab(m.TabID, portalID, False).TabName
                        .ModuleTitle = m.ModuleTitle
                        .Selected = Array.IndexOf(selectedFeedbackModules, m.ModuleID.ToString) <> -1
                    End With
                    feedbackModules.Add(feedbackModule)
                End If
            Next
            Return feedbackModules
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        '''     Implements the upgrade interface for DotNetNuke
        ''' </summary>
        ''' <history>
        ''' 	[scullmann]     12/30/2005	First Implementation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function UpgradeModule(ByVal version As String) As String Implements IUpgradeable.UpgradeModule
            Dim rtnMessage As String = "Events Module Updated: " & version
            Try
                Dim objFeedbackController As New FeedbackController
                objFeedbackController.InstallFeedbackSchedule()

                'Lookup DesktopModuleID
                Dim objDesktopModule As DesktopModuleInfo
                objDesktopModule = DesktopModuleController.GetDesktopModuleByModuleName("DNN_Feedback", 0)

                If Not objDesktopModule Is Nothing Then
                    Dim objModuleDefinition As Definitions.ModuleDefinitionInfo
                    'Lookup ModuleDefID
                    objModuleDefinition = Definitions.ModuleDefinitionController.GetModuleDefinitionByFriendlyName("Feedback", objDesktopModule.DesktopModuleID)

                    If Not objModuleDefinition Is Nothing Then
                        Dim objModuleControlInfo As ModuleControlInfo
                        'Lookup ModuleControlID
                        objModuleControlInfo = ModuleControlController.GetModuleControlByControlKey("Feedback Lists", objModuleDefinition.ModuleDefID)
                        If Not objModuleControlInfo Is Nothing Then
                            ModuleControlController.DeleteModuleControl(objModuleControlInfo.ModuleControlID)
                        End If

                        objModuleControlInfo = ModuleControlController.GetModuleControlByControlKey("Feedback Moderation", objModuleDefinition.ModuleDefID)
                        If Not objModuleControlInfo Is Nothing Then
                            ModuleControlController.DeleteModuleControl(objModuleControlInfo.ModuleControlID)
                        End If
                    End If

                End If

                Return rtnMessage
            Catch ex As Exception
                LogException(ex)

                Return "Feedback Module Updated - Exception: " & version & " - Message: " & ex.Message.ToString
            End Try

        End Function

        ' Creates Feedback Schedule (if Not already installed)
        Public Sub InstallFeedbackSchedule()
            Dim objScheduleItem As Services.Scheduling.ScheduleItem
            objScheduleItem = Services.Scheduling.SchedulingProvider.Instance().GetSchedule("Components.Scheduler.FeedbackSchedule, DotNetNuke.Modules.Feedback", Nothing)
            If objScheduleItem Is Nothing Then
                objScheduleItem = New Services.Scheduling.ScheduleItem
                objScheduleItem.TypeFullName = "Components.Scheduler.FeedbackSchedule, DotNetNuke.Modules.Feedback"
                objScheduleItem.TimeLapse = 1
                objScheduleItem.TimeLapseMeasurement = "d"
                objScheduleItem.RetryTimeLapse = 6
                objScheduleItem.RetryTimeLapseMeasurement = "h"
                objScheduleItem.RetainHistoryNum = 10
                objScheduleItem.Enabled = True
                objScheduleItem.ObjectDependencies = ""
                objScheduleItem.FriendlyName = "DNN Feedback"
                Services.Scheduling.SchedulingProvider.Instance().AddSchedule(objScheduleItem)
            End If
        End Sub
    End Class
#End Region

#Region "FeedbackUpdateController Class"
    Public Class FeedbackUpdateController
#Region "Private Members"

        Private ReadOnly _myConfiguration As Configuration = Nothing
        Private ReadOnly _moduleId As Integer = 0
        Private ReadOnly _portalSettings As Entities.Portals.PortalSettings
        Private ReadOnly _localResourceFile As String = ""
        Private ReadOnly _userid As Integer = 0

#End Region

#Region "Public Methods"
        Public Sub New(ByVal moduleId As Integer, ByVal myConfiguration As Configuration, ByVal portalSettings As Entities.Portals.PortalSettings, ByVal localResourceFile As String, ByVal userid As Integer)
            _moduleId = moduleId
            _myConfiguration = myConfiguration
            _portalSettings = portalSettings
            _localResourceFile = localResourceFile
            _userid = userid
        End Sub

        Public Sub FeedbackUpdateStatus(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal changeStatus As FeedbackInfo.FeedbackStatusType, ByVal notificationId As Integer)
            Dim oFb As New FeedbackController
            Dim oFeedback As FeedbackInfo = oFb.GetFeedback(feedbackID)
            If oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam Or changeStatus = FeedbackInfo.FeedbackStatusType.StatusSpam Then
                Dim akismetApi As New Akismet(_myConfiguration.AkismetKey, NavigateURL())
                If akismetApi.VerifyKey() Then
                    Dim comment As AkismetComment = akismetApi.CreateComment(oFeedback)
                    If oFeedback.Status = FeedbackInfo.FeedbackStatusType.StatusSpam Then
                        akismetApi.SubmitHam(comment)
                    Else
                        akismetApi.SubmitSpam(comment)
                    End If
                End If
            End If

            oFb.UpdateFeedbackStatus(moduleID, feedbackID, changeStatus, _Userid)

            If Not String.IsNullOrEmpty(oFeedback.ContextKey) Or Not notificationId = Nothing Then
                Dim objNotification As New Components.Integration.Notifications
                objNotification.DeleteNotification(oFeedback.ContextKey, notificationId)
                oFb.UpdateContextKey(moduleID, oFeedback.FeedbackID, Nothing, _Userid)
            End If

            If _myConfiguration.SendWhenPublished And changeStatus = FeedbackInfo.FeedbackStatusType.StatusPublic Then
                If oFeedback.PublishedOnDate = Nothing Then
                    Dim objFeedbackEmail As New FeedbackEmail
                    objFeedbackEmail.ReplyToEmail = oFeedback.SenderEmail
                    objFeedbackEmail.SendFromEmail = GetSendFromEmail(oFeedback.SenderEmail)

                    Dim objFeedbackCategoryItem As FeedbackList = GetCategory(oFeedback.CategoryID)
                    objFeedbackEmail.SendToEmail = GetSendToEmail(objFeedbackCategoryItem.ListValue)

                    objFeedbackEmail.SendToRoles = _myConfiguration.SendToRoles
                    objFeedbackEmail.Subject = oFeedback.Subject
                    objFeedbackEmail.Message = CreateMsg(oFeedback, objFeedbackCategoryItem.Name)


                    Try
                        If _myConfiguration.SendAsync Then
                            Dim objThread As New Threading.Thread(AddressOf objFeedbackEmail.SendEmail)
                            objThread.Start()
                        Else
                            objFeedbackEmail.SendEmail()
                        End If

                    Catch ex As Exception
                    End Try
                End If
            End If

        End Sub

        Public Function GetSendToEmail(ByVal categoryValue As String) As String
            'Grab the value of the send to here. If it's blank then assume that the Administrator wants the emails
            'to be sent to the Portal Administrator.
            Dim sendToEmail As String
            If String.IsNullOrEmpty(_myConfiguration.SendTo) Then
                sendToEmail = _PortalSettings.Email
            Else
                sendToEmail = _myConfiguration.SendTo
            End If

            If _myConfiguration.CategorySelect AndAlso _myConfiguration.CategoryAsSendTo AndAlso Not categoryValue Is Nothing Then
                If IsEmail(categoryValue) Then sendToEmail = categoryValue
            End If

            Return sendToEmail
        End Function

        Public Function GetSendFromEmail(ByVal senderEmail As String) As String
            Dim sendFromEmail As String
            If Not String.IsNullOrEmpty(_myConfiguration.SendFrom) Then
                sendFromEmail = _myConfiguration.SendFrom
            ElseIf Not String.IsNullOrEmpty(senderEmail) Then
                sendFromEmail = senderEmail
            Else
                sendFromEmail = _PortalSettings.Email
            End If
            Return sendFromEmail
        End Function

        Public Function GetCategory(ByVal categoryID As String) As FeedbackList
            Dim objFeedbackItem As New FeedbackList
            If IsNumeric(categoryID) Then
                Dim objFeedbackController As New FeedbackController()
                Dim arryFeedbackItem As ArrayList = objFeedbackController.GetFeedbackList(True, _PortalSettings.PortalId, Convert.ToInt32(categoryID), FeedbackList.Type.Category, False, _ModuleId, False)
                If (arryFeedbackItem.Count > 0) Then
                    objFeedbackItem = CType(arryFeedbackItem(0), FeedbackList)
                End If
            End If
            Return objFeedbackItem
        End Function

        Public Function CreateMsg(ByVal oFeedback As FeedbackInfo, ByVal categoryText As String) As String
            Dim msg As New StringBuilder

            If Not String.IsNullOrEmpty(categoryText) Then
                msg.AppendLine(Localization.GetString("plCategory", _LocalResourceFile) & ": " & categoryText)
                msg.AppendLine()
            End If

            msg.AppendLine(oFeedback.Message)
            msg.AppendLine()

            'Append the contact information of the submitter in the message being generated.

            msg.AppendLine(Localization.GetString("plSubmittedBy", _LocalResourceFile))

            Dim spaces As String = New String(" "c, 3)
            If Not String.IsNullOrEmpty(oFeedback.SenderName) Then
                msg.Append(spaces & Localization.GetString("plName", _LocalResourceFile) & ": ")
                msg.AppendLine(oFeedback.SenderName)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderStreet) Then
                msg.Append(spaces & Localization.GetString("plStreet", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderStreet)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderCity) Then
                msg.Append(spaces & Localization.GetString("plCity", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderCity)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderRegion) Then
                msg.Append(spaces & Localization.GetString("plRegion", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderRegion)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderCountry) Then
                msg.Append(spaces & Localization.GetString("plCountry", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderCountry)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderPostalCode) Then
                msg.Append(spaces & Localization.GetString("plPostalCode", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderPostalCode)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderTelephone) Then
                msg.Append(spaces & Localization.GetString("plTelephone", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderTelephone)
            End If

            If Not String.IsNullOrEmpty(oFeedback.SenderEmail) Then
                msg.Append(spaces & Localization.GetString("plEmail", _LocalResourceFile) + ": ")
                msg.AppendLine(oFeedback.SenderEmail)
            End If

            msg.AppendLine()
            msg.AppendLine(Localization.GetString("plSubmittedFrom", _LocalResourceFile))
            msg.AppendLine(spaces & NavigateURL())

            Return msg.ToString
        End Function

#End Region

#Region "Private Methods"

        Private Function IsEmail(ByVal email As String) As Boolean
            IsEmail = True
            For Each emailAddr As String In email.Split(";"c)
                If Not RegularExpressions.Regex.Match(Trim(emailAddr), _myConfiguration.EmailRegex).Success Then
                    IsEmail = False
                End If
            Next
            Return IsEmail
        End Function

#End Region
    End Class
#End Region
End Namespace