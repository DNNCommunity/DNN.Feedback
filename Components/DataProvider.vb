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
Imports DotNetNuke.Modules.Feedback.FeedbackInfo

Namespace DotNetNuke.Modules.Feedback

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The DataProvider Class Is an abstract class that provides the DataLayer
	''' for the Html Module.
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/23/2004	Moved Html to a separate Project
	''' </history>
	''' -----------------------------------------------------------------------------
	Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

		' singleton reference to the instantiated object 
        Private Shared _objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            _objProvider = CType(Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Feedback", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return _objProvider
        End Function

#End Region

#Region "Abstract methods"

        Public MustOverride Function GetLastSubmissionDateForUserId(ByVal portalID As Integer, ByVal userId As Integer) As DateTime
        Public MustOverride Function GetLastSubmissionDateForUserIP(ByVal portalID As Integer, ByVal remoteAddr As String) As DateTime
        Public MustOverride Function GetLastSubmissionDateForUserEmail(ByVal portalID As Integer, ByVal email As String) As DateTime
        Public MustOverride Function GetDuplicateSubmissionForUserEmail(ByVal portalID As Integer, ByVal email As String, ByVal message As String) As DateTime

        Public MustOverride Function CreateFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal senderEmail As String, ByVal status As FeedbackStatusType, ByVal message As String, _
                                               ByVal subject As String, ByVal senderName As String, ByVal senderStreet As String, ByVal senderCity As String, ByVal senderRegion As String, _
                                               ByVal senderCountry As String, ByVal senderPostalCode As String, ByVal senderTelephone As String, ByVal senderRemoteAddr As String, _
                                               ByVal userId As Integer, ByVal referrer As String, ByVal userAgent As String, ByVal contextKey As String) As Integer

        Public MustOverride Function GetCategoryFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal status As FeedbackStatusType, ByVal currentPage As Integer, ByVal pageSize As Integer, ByVal orderBy As String) As IDataReader

        Public MustOverride Function GetFeedback(ByVal feedbackID As Integer) As IDataReader
        Public MustOverride Sub UpdateFeedback(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal subject As String, ByVal message As String, ByVal userId As Integer)
        Public MustOverride Sub UpdateContextKey(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal conextkey As String, ByVal userId As Integer)
        Public MustOverride Sub UpdateFeedbackStatus(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal status As FeedbackStatusType, ByVal userId As Integer)

        Public MustOverride Function AddFeedbackList(ByVal portalID As Integer, ByVal listType As Integer, ByVal name As String, ByVal listValue As String, ByVal isActive As Boolean, ByVal portal As Boolean, ByVal moduleID As Integer) As Integer
        Public MustOverride Function GetFeedbackList(ByVal singleRowOperation As Boolean, ByVal portalID As Integer, ByVal listID As Integer, ByVal listType As Integer, ByVal activeOnly As Boolean, ByVal moduleID As Integer, ByVal allList As Boolean) As IDataReader
        Public MustOverride Sub EditFeedbackList(ByVal isDeleteOperation As Boolean, ByVal listID As Integer, ByVal portalID As Integer, ByVal listType As Integer, ByVal name As String, ByVal listValue As String, ByVal isActive As Boolean, ByVal portal As Boolean, ByVal moduleID As Integer)
        Public MustOverride Sub ChangeSortOrder(ByVal listID As Integer, ByVal listType As Integer, ByVal oldSortNum As Integer, ByVal newSortNum As Integer)
        Public MustOverride Function GetOrphanedData() As IDataReader
        Public MustOverride Sub DeleteOrphanedData()
        Public MustOverride Sub CleanupFeedback(ByVal moduleID As Integer, ByVal cleanupPending As Boolean, ByVal cleanupPrivate As Boolean, ByVal cleanupPublished As Boolean, ByVal cleanupArchived As Boolean, ByVal cleanupSpam As Boolean, ByVal daysBefore As Integer, ByVal maxEntries As Integer)

#End Region

    End Class

End Namespace