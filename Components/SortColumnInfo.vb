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

Namespace DotNetNuke.Modules.Feedback
    <Serializable()> _
    Public Class SortColumnInfo

        Public Const SortAscendingGlyph As String = "~/images/sortascending.gif"
        Public Const SortDescendingGlyph As String = "~/images/sortdescending.gif"
        Public Const GlyphHeight As Integer = 9
        Public Const GlyphWidth As Integer = 10

        Private _columnName As String
        Private _direction As SortDirection = SortDirection.NotSorted

        Enum SortDirection As Integer
            NotSorted = 0
            Ascending
            Descending
        End Enum

        Public Sub New()
            'Default Constructor
        End Sub

        Public Sub New(ByVal columnName As String)
            _columnName = columnName
        End Sub

        Public Sub New(ByVal columnName As String, ByVal direction As SortDirection)
            _columnName = columnName
            _direction = direction
        End Sub

        Public Sub New(ByVal columnName As String, ByVal direction As String)
            Me.New(columnName)
            Select Case direction
                Case ""
                    _direction = SortDirection.NotSorted
                Case "ASC"
                    _direction = SortDirection.Ascending
                Case "DESC"
                    _direction = SortDirection.Descending
                Case Else
                    ' ReSharper disable LocalizableElement
                    Throw New ArgumentOutOfRangeException("Direction", "Sort direction must be '', 'ASC' or 'DESC'")
                    ' ReSharper restore LocalizableElement
            End Select
        End Sub

        Public Property ColumnName() As String
            Get
                Return _ColumnName
            End Get
            Set(ByVal value As String)
                _columnName = Value
            End Set
        End Property

        Public Property Direction() As SortDirection
            Get
                Return _Direction
            End Get
            Set(ByVal value As SortDirection)
                _Direction = value
            End Set
        End Property

        Public ReadOnly Property DirectionString() As String
            Get
                Dim s As String = ""
                Select Case _Direction
                    Case SortDirection.Ascending : s = "ASC"
                    Case SortDirection.Descending : s = "DESC"
                End Select
                Return s
            End Get
        End Property

        Public ReadOnly Property OrderByExpression() As String
            Get
                Return (ColumnName & " " & DirectionString).TrimEnd(" "c)
            End Get
        End Property

        Public ReadOnly Property DirectionGlyph() As String
            Get
                Dim glyph As String = "spacer.gif"
                Select Case _Direction
                    Case SortDirection.Ascending : glyph = SortAscendingGlyph
                    Case SortDirection.Descending : glyph = SortDescendingGlyph
                End Select
                Return String.Format("<img src='{0}' height='{1}' width='{2}' border='0' />", Common.Globals.ResolveUrl(glyph), GlyphHeight, GlyphWidth)
            End Get
        End Property

        Public Sub ToggleDirection()
            Direction = CType((CType(Direction, Integer) + 1) Mod 3, SortDirection)
        End Sub

        Public Sub ToggleDirection(ByVal skipNotSorted As Boolean)
            Direction = CType(If((CType(Direction, Integer) + 1) > 2, 1, 2), SortDirection)
        End Sub
    End Class
End Namespace
