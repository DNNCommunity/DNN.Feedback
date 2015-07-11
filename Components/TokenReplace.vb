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
Imports DotNetNuke.Services.Localization
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Feedback

    Public Class FeedbackTokenReplace
        Inherits TokenReplace

        Private _localResourceFile As String

        Private ReadOnly _fieldLabelRegex As Regex = New Regex("{(?<fieldName>[a-zA-Z]\w*)}|(\[[^\]\[]+\])|(?<text>[^{]+)", RegexOptions.Compiled)

        Public Property LocalResourceFile() As String
            Get
                Return _localResourceFile
            End Get
            Set(ByVal value As String)
                _localResourceFile = value
            End Set
        End Property

        Public Sub New()
            MyBase.New(Services.Tokens.Scope.DefaultSettings)
            _localResourceFile = Configuration.SharedResources
        End Sub

        Public Sub New(ByVal fb As FeedbackInfo)
            Me.New()
            PropertySource("feedback") = fb
        End Sub

        Public Sub New(ByVal fb As FeedbackInfo, ByVal localResourceFile As String)
            Me.New(fb)
            _localResourceFile = localResourceFile
        End Sub

        Public Sub SetPropertySource(ByVal fb As FeedbackInfo)
            PropertySource("feedback") = fb
        End Sub

        Protected Overrides Function ReplacedTokenValue(ByVal strObjectName As String, ByVal strPropertyName As String, ByVal strFormat As String) As String
            Return MyBase.replacedTokenValue(strObjectName, strPropertyName, strFormat)
        End Function

        Public Function ReplaceFeedbackTokens(ByVal strSourceText As String) As String
            strSourceText = ReplaceFieldLabels(strSourceText)
            Return ReplaceEnvironmentTokens(strSourceText)
        End Function

        Public Function ReplaceFieldLabels(ByVal strSourceText As String) As String
            If strSourceText Is Nothing Then Return String.Empty
            Dim result As New Text.StringBuilder
            For Each currentMatch As Match In _fieldLabelRegex.Matches(strSourceText)
                Dim fieldName As String = currentMatch.Result("${fieldName}")
                If fieldName.Length > 0 Then
                    Dim localizedFieldName As String = Localization.GetString(fieldName & ".FieldLabel", LocalResourceFile)
                    If String.IsNullOrEmpty(localizedFieldName) Then
                        result.Append(fieldName)
                    Else
                        result.Append(localizedFieldName)
                    End If
                Else
                    result.Append(currentMatch.Result("${text}"))
                End If
            Next
            Return result.ToString()
        End Function
    End Class


End Namespace