Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users


Namespace DotNetNuke.Modules.Feedback
    Public Class Utilities

        Public Shared Function ConvertServerTimeToUserTime(ByVal value As DateTime) As DateTime
            Dim result As DateTime

            Try
                'Get the portalTimeZone as a fallback
                Dim portalTimeZone As TimeZoneInfo = PortalController.Instance.GetCurrentPortalSettings().TimeZone

                'Get the userTime based on user profile preference if user is authenticated
                If System.Web.HttpContext.Current.Request.IsAuthenticated Then
                    Dim objUser As UserInfo = UserController.Instance.GetCurrentUserInfo()
                    Dim userTimeZone As TimeZoneInfo = objUser.Profile.PreferredTimeZone
                    result = TimeZoneInfo.ConvertTimeFromUtc(value, userTimeZone)
                Else
                    result = TimeZoneInfo.ConvertTimeFromUtc(value, portalTimeZone)
                End If
            Catch
                result = value
            End Try

            Return result
        End Function


    End Class
End Namespace
