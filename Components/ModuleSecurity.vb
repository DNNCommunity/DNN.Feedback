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
Imports DotNetNuke.Security
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Feedback.Security

    Public Class PermissionName
        Public Const ModerateFeedbackPosts As String = "MODERATEPOSTS"
        Public Const ManageFeedbackLists As String = "MANAGELISTS"
        Public Const Code As String = "FEEDBACK_PERMISSION"
    End Class

    Public Class ModuleSecurity
        Private ReadOnly _hasModeratePermission As Boolean
        Private ReadOnly _hasManageListPermission As Boolean

        Public Sub New(ByVal moduleId As Integer, ByVal tabId As Integer)
            Dim mc As New Entities.Modules.ModuleController
            Dim mp As Permissions.ModulePermissionCollection = mc.GetModule(moduleId, tabId).ModulePermissions
            _hasModeratePermission = Permissions.ModulePermissionController.HasModulePermission(mp, PermissionName.ModerateFeedbackPosts)
            _hasManageListPermission = Permissions.ModulePermissionController.HasModulePermission(mp, PermissionName.ManageFeedbackLists)
        End Sub

        Public Function IsAllowedToModeratePosts() As Boolean
            Return _hasModeratePermission
        End Function

        Public Function IsAllowedToManageLists() As Boolean
            Return _hasManageListPermission
        End Function

        Public Function RoleNames(ByVal user As UserInfo) As String
            Dim roles As String = ""
            If Not (User.Roles Is Nothing) Then roles = "|" & String.Join("|", User.Roles)
            roles &= "|" & glbRoleAllUsersName
            Dim administratorRoleName As String
            administratorRoleName = GetPortalSettings.AdministratorRoleName
            If PortalSecurity.IsInRole(administratorRoleName) Then roles &= "|" & administratorRoleName
            Return roles & "|"
        End Function

        Public Shared Function UserId(ByVal username As String, ByVal portalid As Integer) As Integer
            Dim ui As UserInfo = Entities.Users.UserController.GetUserByName(portalid, username)
            If ui Is Nothing Then
                Return 0
            Else
                Return (ui.UserID)
            End If
        End Function

        Public Shared Function FormatRemoteAddr(ByVal remoteAddr As String) As String
        
            If Not String.IsNullOrEmpty(remoteAddr) AndAlso Regex.Match(remoteAddr, "^\d{1,3}[:.]\d{1,3}[:.]\d{1,3}[:.]\d{1,3}$").Success Then
                Return " [ " & remoteAddr & "]"
            Else
                Return String.Empty
            End If
        End Function

        Public Shared Function GetUserIPAddress() As String
            If System.Web.HttpContext.Current Is Nothing Then
                Return String.Empty
            Else
                Return FormatRemoteAddr(System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"))
            End If
        End Function

        Public Shared Function ModeratorRoles(ByVal mp As Permissions.ModulePermissionCollection) As String

            Return Permissions.PermissionController.BuildPermissions(mp, PermissionName.ModerateFeedbackPosts)

        End Function

    End Class
End Namespace