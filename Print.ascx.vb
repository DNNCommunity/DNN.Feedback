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

Imports System.Web

Namespace DotNetNuke.Modules.Feedback
    Partial Public Class Print
        Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"

        Private _myConfiguration As Configuration = Nothing
        Private _feedbackID As Integer = -1
        Private _returnURL As String

#End Region

#Region "Public and Private Properties"
        Public ReadOnly Property MyConfiguration() As Configuration
            Get
                If _myConfiguration Is Nothing Then
                    _myConfiguration = New Configuration(ModuleId, Settings)
                End If
                Return _myConfiguration
            End Get
        End Property

        Private ReadOnly Property ReturnURL() As String
            Get
                If String.IsNullOrEmpty(_returnURL) Then
                    Return NavigateURL()
                Else
                    Return _returnURL
                End If
            End Get
        End Property

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            If Not DotNetNuke.Security.Permissions.ModulePermissionController.CanViewModule(ModuleConfiguration) Then Response.Redirect(AccessDeniedURL(), True)

            If Request.QueryString("feedbackid") IsNot Nothing Then
                _feedbackID = Integer.Parse(Request.QueryString("feedbackid"))
            End If

            Dim objFeedbackController As New FeedbackController
            Dim objFeedbackInfo As FeedbackInfo = objFeedbackController.GetFeedback(_feedbackID)
            If objFeedbackInfo Is Nothing OrElse (objFeedbackInfo.ModuleID <> ModuleId OrElse objFeedbackInfo.PortalID <> PortalId) Then
                Response.Redirect(AccessDeniedURL(), True) ' probably attempt to view feedback directly by manipulating URL
            Else
                Dim tr As New FeedbackTokenReplace(objFeedbackInfo, Configuration.SharedResources)
                Dim content As String = tr.ReplaceFeedbackTokens(MyConfiguration.PrintTemplate)
                litContents.Text = content
            End If

            If Request.QueryString("returnurl") Is Nothing Then
                With btnClose
                    .Text = Localization.GetString("btnClose", LocalResourceFile)
                    .OnClientClick = "javascript:window.close();return false;"
                End With
            Else
                _returnURL = HttpUtility.UrlDecode(Request.QueryString("returnurl"))
                If _returnURL.Contains("://") Then _returnURL = String.Empty
                btnClose.Text = Localization.GetString("btnReturn", LocalResourceFile)
            End If
        End Sub

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Try
                Response.Redirect(ReturnURL, True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
    End Class
End Namespace