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
    Public Class AggregatedModuleInfo

#Region "Private Members"
        Private _tabId As Integer
        Private _tabName As String
        Private _moduleId As Integer
        Private _moduleTitle As String
        Private _selected As Boolean
#End Region

        Public Property TabId() As Integer
            Get
                Return _tabId
            End Get
            Set(ByVal value As Integer)
                _tabId = value
            End Set
        End Property

        Public Property TabName() As String
            Get
                Return _tabName
            End Get
            Set(ByVal value As String)
                _tabName = value
            End Set
        End Property

        Public Property ModuleId() As Integer
            Get
                Return _moduleId
            End Get
            Set(ByVal value As Integer)
                _moduleId = value
            End Set
        End Property

        Public Property ModuleTitle() As String
            Get
                Return _ModuleTitle
            End Get
            Set(ByVal value As String)
                _ModuleTitle = value
            End Set
        End Property

        Public Property Selected() As Boolean
            Get
                Return _selected
            End Get
            Set(ByVal value As Boolean)
                _selected = value
            End Set
        End Property
    End Class
End Namespace
