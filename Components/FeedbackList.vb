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

Namespace DotNetNuke.Modules.Feedback

    Public Class FeedbackList
        Implements Entities.Modules.IHydratable

        Enum Type As Integer
            Category = 1
            Subject = 2
        End Enum

        Enum Active
            No
            Yes
        End Enum

#Region "Private Members"
        Private _listID As Integer
        Private _portalID As Integer
        Private _listType As Type
        Private _isActive As Active
        Private _name As String
        Private _listValue As String
        Private _sortOrder As Integer
        Private _portal As Boolean
        Private _moduleID As Integer
        Private _categoryCount As Integer
#End Region

#Region "Public Properties"
        Public Property ListID() As Integer
            Get
                Return _ListID
            End Get
            Set(ByVal value As Integer)
                _ListID = value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal value As Integer)
                _PortalID = value
            End Set
        End Property

        Public Property ListType() As Type
            Get
                Return _ListType
            End Get
            Set(ByVal value As Type)
                _ListType = value
                'Select Case Convert.ToInt32(value)
                '    Case 1
                '        _ListType = Type.Category
                '    Case 2
                '        _ListType = Type.Subject
                'End Select
            End Set
        End Property

        Public Property IsActive() As Active
            Get
                Return _IsActive
            End Get
            Set(ByVal value As Active)
                Select Case Convert.ToInt32(value)
                    Case 0
                        _IsActive = Active.No
                    Case Else
                        _IsActive = Active.Yes
                End Select
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Property ListValue() As String
            Get
                Return _ListValue
            End Get
            Set(ByVal value As String)
                _ListValue = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _SortOrder
            End Get
            Set(ByVal value As Integer)
                _SortOrder = value
            End Set
        End Property

        Public Property Portal() As Boolean
            Get
                Return _Portal
            End Get
            Set(ByVal value As Boolean)
                _Portal = value
            End Set
        End Property

        Public Property ModuleID() As Integer
            Get
                Return _ModuleID
            End Get
            Set(ByVal value As Integer)
                _ModuleID = value
            End Set
        End Property

        Public Property CategoryCount() As Integer
            Get
                Return _CategoryCount
            End Get
            Set(ByVal value As Integer)
                _CategoryCount = value
            End Set
        End Property
#End Region

#Region "IHydradatable Implementation"
        Public Sub Fill(ByVal dr As IDataReader) Implements Entities.Modules.IHydratable.Fill
            With dr
                ListID = Null.SetNullInteger(dr("ListID"))
                PortalID = Null.SetNullInteger(dr("PortalID"))
                ListType = CType(Null.SetNullInteger(dr("ListType")), Type)
                IsActive = CType(Null.SetNullInteger(dr("IsActive")), Active)
                Name = Null.SetNullString(dr("Name"))
                ListValue = Null.SetNullString(dr("ListValue"))
                SortOrder = Null.SetNullInteger(dr("SortOrder"))
                Portal = Null.SetNullBoolean(dr("Portal"))
                ModuleID = Null.SetNullInteger(dr("ModuleID"))
                CategoryCount = Null.SetNullInteger(dr("CategoryCount"))
            End With
        End Sub

        Public Property KeyID() As Integer Implements Entities.Modules.IHydratable.KeyID
            Get
                Return ListID
            End Get
            Set(ByVal value As Integer)
                ListID = value
            End Set
        End Property

#End Region

    End Class

End Namespace