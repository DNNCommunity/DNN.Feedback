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
Imports System.Data
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities

Namespace DotNetNuke.Modules.Feedback

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The SqlDataProvider Class is an SQL Server implementation of the DataProvider Abstract
	''' class that provides the DataLayer for the Feedback Module.
	''' </summary>
    ''' -----------------------------------------------------------------------------
	Public Class SqlDataProvider

		Inherits DataProvider

#Region "Private Members"

		Private Const ProviderType As String = "data"

        Private ReadOnly _providerConfiguration As Providers.ProviderConfiguration = Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private ReadOnly _connectionString As String
        Private ReadOnly _providerPath As String
        Private ReadOnly _objectQualifier As String
        Private ReadOnly _databaseOwner As String

#End Region

#Region "Constructors"

		Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Providers.Provider)

            ' Read the attributes for this provider
            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If
        End Sub

#End Region

#Region "Properties"

		Public ReadOnly Property ConnectionString() As String
			Get
				Return _connectionString
			End Get
		End Property

		Public ReadOnly Property ProviderPath() As String
			Get
				Return _providerPath
			End Get
		End Property

		Public ReadOnly Property ObjectQualifier() As String
			Get
				Return _objectQualifier
			End Get
		End Property

		Public ReadOnly Property DatabaseOwner() As String
			Get
				Return _databaseOwner
			End Get
		End Property

#End Region

#Region "Private Methods"

        Private Function GetNull(ByVal field As Object) As Object
            Return Null.GetNull(field, DBNull.Value)
        End Function

#End Region

#Region "Public Methods - DataProvider Overrides"

        Public Overrides Function GetLastSubmissionDateForUserId(ByVal portalID As Integer, ByVal userId As Integer) As DateTime
            Return Null.SetNullDateTime(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetLastSubmissionDateForUserId", portalID, userId))
        End Function

        Public Overrides Function GetLastSubmissionDateForUserIP(ByVal portalID As Integer, ByVal remoteAddr As String) As DateTime
            Return Null.SetNullDateTime(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetLastSubmissionDateForUserIP", portalID, remoteAddr))
        End Function

        Public Overrides Function GetLastSubmissionDateForUserEmail(ByVal portalID As Integer, ByVal email As String) As DateTime
            Return Null.SetNullDateTime(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetLastSubmissionDateForUserEmail", portalID, email))
        End Function

        Public Overrides Function GetDuplicateSubmissionForUserEmail(ByVal portalID As Integer, ByVal email As String, ByVal message As String) As DateTime
            Return Null.SetNullDateTime(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetDuplicateSubmissionForUserEmail", portalID, email, message))
        End Function

        Public Overrides Function CreateFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal senderEmail As String, _
                                            ByVal status As FeedbackInfo.FeedbackStatusType, ByVal message As String, ByVal subject As String, _
                                            ByVal senderName As String, ByVal senderStreet As String, ByVal senderCity As String, ByVal senderRegion As String, _
                                            ByVal senderCountry As String, ByVal senderPostalCode As String, ByVal senderTelephone As String, ByVal senderRemoteAddr As String, _
                                            ByVal userId As Integer, ByVal referrer As String, ByVal userAgent As String, ByVal contextKey As String) As Integer
            Return Null.SetNullInteger(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_CreateFeedback", _
                  portalID, GetNull(moduleID), GetNull(categoryID), senderEmail, status, message, subject, senderName, senderStreet, senderCity, senderRegion, _
                  senderCountry, senderPostalCode, senderTelephone, senderRemoteAddr, userId, referrer, userAgent, contextKey))
        End Function

        Public Overrides Function GetFeedback(ByVal feedbackID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetFeedback", feedbackID), IDataReader)
        End Function

        Public Overrides Function GetCategoryFeedback(ByVal portalID As Integer, ByVal moduleID As Integer, ByVal categoryID As String, ByVal status As FeedbackInfo.FeedbackStatusType, ByVal currentPage As Integer, ByVal pageSize As Integer, ByVal orderBy As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetFeedbackByCategory", portalID, GetNull(moduleID), categoryID, status, currentPage, pageSize, orderBy), IDataReader)
        End Function

        Public Overrides Sub UpdateFeedback(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal subject As String, ByVal message As String, ByVal userId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_UpdateFeedback", moduleID, FeedbackID, subject, message, userId)
        End Sub

        Public Overrides Sub UpdateContextKey(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal contextKey As String, ByVal userId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_UpdateContextKey", moduleID, FeedbackID, contextKey, userId)
        End Sub

        Public Overrides Sub UpdateFeedbackStatus(ByVal moduleID As Integer, ByVal feedbackID As Integer, ByVal status As FeedbackInfo.FeedbackStatusType, ByVal userId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_UpdateStatus", moduleID, FeedbackID, status, userId)
        End Sub

        Public Overrides Function AddFeedbackList(ByVal portalID As Integer, ByVal listType As Integer, ByVal name As String, ByVal listValue As String, ByVal isActive As Boolean, ByVal portal As Boolean, ByVal moduleID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_AddList", portalID, listType, name, listValue, IsActive, portal, moduleID), Integer)
        End Function

        Public Overrides Function GetFeedbackList(ByVal singleRowOperation As Boolean, ByVal portalID As Integer, ByVal listID As Integer, ByVal listType As Integer, ByVal activeOnly As Boolean, ByVal moduleID As Integer, ByVal allList As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetList", singleRowOperation, portalID, listID, listType, activeOnly, moduleID, allList), IDataReader)
        End Function

        Public Overrides Sub EditFeedbackList(ByVal isDeleteOperation As Boolean, ByVal listID As Integer, ByVal portalID As Integer, ByVal listType As Integer, ByVal name As String, ByVal listValue As String, ByVal isActive As Boolean, ByVal portal As Boolean, ByVal moduleID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_EditList", IsDeleteOperation, listID, portalID, listType, name, listValue, IsActive, portal, moduleID)
        End Sub

        Public Overrides Sub ChangeSortOrder(ByVal listID As Integer, ByVal listType As Integer, ByVal oldSortNum As Integer, ByVal newSortNum As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_ChangeListSortOrder", ListID, ListType, oldSortNum, newSortNum)
        End Sub

        Public Overrides Function GetOrphanedData() As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_GetOrphanedData"), IDataReader)
        End Function

        Public Overrides Sub DeleteOrphanedData()
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_DeleteOrphanedData")
        End Sub

        Public Overrides Sub CleanupFeedback(ByVal moduleID As Integer, ByVal cleanupPending As Boolean, ByVal cleanupPrivate As Boolean, ByVal cleanupPublished As Boolean, ByVal cleanupArchived As Boolean, ByVal cleanupSpam As Boolean, ByVal daysBefore As Integer, ByVal maxEntries As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Feedback_CleanupFeedback", moduleID, cleanupPending, cleanupPrivate, cleanupPublished, cleanupArchived, cleanupSpam, daysBefore, maxEntries)
        End Sub

#End Region

    End Class

End Namespace