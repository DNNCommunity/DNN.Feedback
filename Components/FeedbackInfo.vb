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

Imports DotNetNuke.Services.Tokens

Namespace DotNetNuke.Modules.Feedback
    Public Class FeedbackInfo
        Inherits Entities.BaseEntityInfo
        Implements Entities.Modules.IHydratable
        Implements Services.Tokens.IPropertyAccess

        Enum FeedbackStatusType As Integer
            StatusPending = 0
            StatusPrivate
            StatusPublic
            StatusArchive
            StatusDelete
            StatusSpam
        End Enum

#Region "Private Members"

        Private _feedbackID As Integer
        Private _status As FeedbackStatusType
        Private _publishedOnDate As Date
        Private _senderStreet As String
        Private _senderCity As String
        Private _senderRegion As String
        Private _senderCountry As String
        Private _senderPostalCode As String
        Private _senderEmail As String
        Private _senderTelephone As String
        Private _senderRemoteAddr As String
        Private _approvedBy As Integer
        Private _moduleID As Integer
        Private _categoryID As String
        Private _totalRecords As Integer
        Private _message As String
        Private _subject As String
        Private _portalID As Integer
        Private _senderName As String
        Private _categoryValue As String
        Private _categoryName As String
        Private _referrer As String
        Private _userAgent As String
        Private _contextKey As String
        Private _displayCreatedOnDate As DateTime

#End Region

#Region "Public Properties"

        Public Property FeedbackID() As Integer
            Get
                Return _feedbackID
            End Get
            Set(ByVal value As Integer)
                _feedbackID = Value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _portalID
            End Get
            Set(ByVal value As Integer)
                _portalID = value
            End Set
        End Property

        Public Property CategoryID() As String
            Get
                Return _categoryID
            End Get
            Set(ByVal value As String)
                _categoryID = Value
            End Set
        End Property

        Public Property CategoryValue() As String
            Get
                Return _categoryValue
            End Get
            Set(ByVal value As String)
                _categoryValue = Value
            End Set
        End Property

        Public Property CategoryName() As String
            Get
                Return _categoryName
            End Get
            Set(ByVal value As String)
                _categoryName = value
            End Set
        End Property

        Public Property Status() As FeedbackStatusType
            Get
                Return _status
            End Get
            Set(ByVal value As FeedbackStatusType)
                _status = Value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                _subject = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = Value
            End Set
        End Property

        Public Property PublishedOnDate() As Date
            Get
                Return _publishedOnDate
            End Get
            Set(ByVal value As Date)
                _publishedOnDate = Value
            End Set
        End Property

        Public Property SenderName() As String
            Get
                Return _senderName
            End Get
            Set(ByVal value As String)
                _senderName = value
            End Set
        End Property

        Public Property SenderStreet() As String
            Get
                Return _senderStreet
            End Get
            Set(ByVal value As String)
                _senderStreet = Value
            End Set
        End Property

        Public Property SenderCity() As String
            Get
                Return _senderCity
            End Get
            Set(ByVal value As String)
                _senderCity = Value
            End Set
        End Property

        Public Property SenderRegion() As String
            Get
                Return _senderRegion
            End Get
            Set(ByVal value As String)
                _senderRegion = Value
            End Set
        End Property

        Public Property SenderCountry() As String
            Get
                Return _senderCountry
            End Get
            Set(ByVal value As String)
                _senderCountry = Value
            End Set
        End Property

        Public Property SenderPostalCode() As String
            Get
                Return _senderPostalCode
            End Get
            Set(ByVal value As String)
                _senderPostalCode = Value
            End Set
        End Property

        Public Property SenderEmail() As String
            Get
                Return _senderEmail
            End Get
            Set(ByVal value As String)
                _senderEmail = Value
            End Set
        End Property

        Public Property SenderTelephone() As String
            Get
                Return _senderTelephone
            End Get
            Set(ByVal value As String)
                _senderTelephone = Value
            End Set
        End Property

        Public Property SenderRemoteAddr() As String
            Get
                Return _senderRemoteAddr
            End Get
            Set(ByVal value As String)
                _senderRemoteAddr = value
            End Set
        End Property

        Public Property ApprovedBy() As Integer
            Get
                Return _approvedBy
            End Get
            Set(ByVal value As Integer)
                _approvedBy = Value
            End Set
        End Property

        Public Property TotalRecords() As Integer
            Get
                Return _totalRecords
            End Get
            Set(ByVal value As Integer)
                _totalRecords = Value
            End Set
        End Property

        Public Property Referrer() As String
            Get
                Return _referrer
            End Get
            Set(ByVal value As String)
                _referrer = value
            End Set
        End Property

        Public Property UserAgent() As String
            Get
                Return _userAgent
            End Get
            Set(ByVal value As String)
                _userAgent = value
            End Set
        End Property

        Public Property ContextKey() As String
            Get
                Return _contextKey
            End Get
            Set(ByVal value As String)
                _contextKey = value
            End Set
        End Property

        Public Property DisplayCreatedOnDate() As DateTime
            Get
                Return Utilities.ConvertServerTimeToUserTime(CreatedOnDate)
            End Get
            Set(ByVal value As DateTime)
                _displayCreatedOnDate = value
            End Set
        End Property


#End Region

#Region "IHydratable Implementation"

        Public Sub Fill(ByVal dr As IDataReader) Implements Entities.Modules.IHydratable.Fill
            FillInternal(dr)     'Read audit fields
            With dr
                ModuleID = Null.SetNullInteger(dr("ModuleID"))
                FeedbackID = Null.SetNullInteger(dr("FeedbackID"))
                PortalID = Null.SetNullInteger(dr("PortalID"))
                Status = CType(If(Null.IsNull(dr("Status")), 0, dr("Status")), FeedbackStatusType)
                CategoryID = Null.SetNullString(dr("CategoryID"))
                CategoryValue = Null.SetNullString(dr("CategoryValue"))
                CategoryName = Null.SetNullString(dr("CategoryName"))
                Subject = Null.SetNullString(dr("Subject"))
                Message = Null.SetNullString(dr("Message"))
                SenderName = Null.SetNullString(dr("SenderName"))
                SenderStreet = Null.SetNullString(dr("SenderStreet"))
                SenderCity = Null.SetNullString(dr("SenderCity"))
                SenderRegion = Null.SetNullString(dr("SenderRegion"))
                SenderPostalCode = Null.SetNullString(dr("SenderPostalCode"))
                SenderCountry = Null.SetNullString(dr("SenderCountry"))
                SenderEmail = Null.SetNullString(dr("SenderEmail"))
                SenderTelephone = Null.SetNullString(dr("SenderTelephone"))
                SenderRemoteAddr = Null.SetNullString(dr("SenderRemoteAddr"))
                ApprovedBy = Null.SetNullInteger(dr("ApprovedBy"))
                PublishedOnDate = Null.SetNullDateTime(dr("PublishedOnDate"))
                TotalRecords = Null.SetNullInteger(dr("TotalRecords"))
                Referrer = Null.SetNullString(dr("Referrer"))
                UserAgent = Null.SetNullString(dr("UserAgent"))
                ContextKey = Null.SetNullString(dr("ContextKey"))
            End With
        End Sub

        Public Property KeyID() As Integer Implements Entities.Modules.IHydratable.KeyID
            Get
                Return FeedbackID
            End Get
            Set(ByVal value As Integer)
                FeedbackID = value
            End Set
        End Property
#End Region

#Region "IPropertyAccess Implementation"

        Public ReadOnly Property Cacheability() As Services.Tokens.CacheLevel Implements Services.Tokens.IPropertyAccess.Cacheability
            Get
                Return CacheLevel.fullyCacheable
            End Get
        End Property

        Public Function GetProperty(ByVal strPropertyName As String, ByVal strFormat As String, ByVal formatProvider As Globalization.CultureInfo, ByVal accessingUser As Entities.Users.UserInfo, ByVal accessLevel As Services.Tokens.Scope, ByRef propertyNotFound As Boolean) As String Implements Services.Tokens.IPropertyAccess.GetProperty

            Dim outputFormat As String

            If strFormat = String.Empty Then
                outputFormat = "g"
            Else
                outputFormat = strFormat
            End If

            'Content locked for NoSettings
            If accessLevel = Scope.NoSettings Then PropertyNotFound = True : Return PropertyAccess.ContentLocked

            Dim result As String = String.Empty
            Dim publicProperty As Boolean = True

            Select Case strPropertyName.ToLowerInvariant
                Case "feedbackid"
                    publicProperty = True : PropertyNotFound = False : result = FeedbackID.ToString(outputFormat, formatProvider)
                Case "category", "categoryvalue"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(CategoryValue, strFormat)
                Case "categoryname"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(CategoryName, strFormat)
                Case "categoryid"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(CategoryID, strFormat) 'note that CategoryID is System.Type.String
                Case "status"
                    publicProperty = True : PropertyNotFound = False : result = Localization.GetString(Status.ToString, Configuration.SharedResources)
                Case "subject"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(Subject, strFormat)
                Case "message"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(Message, strFormat)
                Case "sendername"
                    If String.IsNullOrEmpty(SenderName) Then
                        publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(Localization.GetString("Anonymous", Configuration.SharedResources), strFormat)
                    Else
                        publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderName, strFormat)
                    End If
                Case "senderstreet"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderStreet, strFormat)
                Case "sendercity"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderCity, strFormat)
                Case "senderregion"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderRegion, strFormat)
                Case "sendercountry"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderCountry, strFormat)
                Case "senderpostalcode"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderPostalCode, strFormat)
                Case "sendertelephone"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderTelephone, strFormat)
                Case "senderremoteaddr"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderRemoteAddr, strFormat)
                Case "senderemail"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(SenderEmail, strFormat)
                Case "createdondateutc", "datecreated"
                    publicProperty = True
                    PropertyNotFound = False
                    If CreatedOnDate <> DateTime.MinValue Then
                        result = Utilities.ConvertServerTimeToUserTime(CreatedOnDate).ToString(outputFormat, formatProvider)
                    End If
                Case "publishedondate"
                    publicProperty = True
                    propertyNotFound = False
                    If PublishedOnDate <> DateTime.MinValue Then
                        result = Utilities.ConvertServerTimeToUserTime(PublishedOnDate).ToString(outputFormat, formatProvider)
                    End If
                Case "lastmodifiedondate"
                    publicProperty = True
                    propertyNotFound = False
                    If LastModifiedOnDate <> DateTime.MinValue Then
                        result = Utilities.ConvertServerTimeToUserTime(LastModifiedOnDate).ToString(outputFormat, formatProvider)
                    End If
                Case "referrer"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(Referrer, strFormat)
                Case "useragent"
                    publicProperty = True : PropertyNotFound = False : result = PropertyAccess.FormatString(UserAgent, strFormat)
                Case Else
                    PropertyNotFound = True
            End Select

            If Not publicProperty And accessLevel <> Scope.Debug Then
                PropertyNotFound = True
                result = PropertyAccess.ContentLocked
            End If
            Return result
        End Function

#End Region

    End Class

End Namespace