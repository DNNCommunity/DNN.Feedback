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
Imports System.Net
Imports System.Web
Imports System.IO
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Feedback
    Public Class Akismet
        Inherits PortalModuleBase

        Const VerifyUrl As String = "http://rest.akismet.com/1.1/verify-key"
        Const CommentCheckUrl As String = "http://{0}.rest.akismet.com/1.1/comment-check"
        Const SubmitSpamUrl As String = "http://{0}.rest.akismet.com/1.1/submit-spam"
        Const SubmitHamUrl As String = "http://{0}.rest.akismet.com/1.1/submit-ham"

        ReadOnly _apiKey As String = Nothing
        ReadOnly _akismetUserAgent As String = Nothing
        ReadOnly _blog As String = Nothing

#Region "Public Properties"
        Public CharSet As String = "UTF-8"
#End Region
#Region "Public Methods"
        ''' <summary>Creates an Akismet API object.</summary>
        ''' <param name="apiKey">Your API key.</param>
        ''' <param name="blog">URL to your blog</param>
        ''' <remarks>Accepts required fields 'apiKey', 'blog', 'userAgent'.</remarks>
        Public Sub New(ByVal apiKey As String, ByVal blog As String)
            _apiKey = apiKey
            _blog = blog
            Dim objDesktopModule As DesktopModuleInfo
            objDesktopModule = DesktopModuleController.GetDesktopModuleByModuleName("DNN_Feedback", 0)
            _akismetUserAgent = String.Format("DNN Feedback/{0} | Akismet/1.11", objDesktopModule.Version)
        End Sub

        ''' <summary>Verifies your key.</summary>
        ''' <returns>'True' if key is valid.</returns>
        Public Function VerifyKey() As Boolean
            Dim value As Boolean
            Dim chkResponse As String = HttpPost(VerifyUrl, [String].Format("key={0}&blog={1}", _apiKey, HttpUtility.UrlEncode(_blog)), CharSet)
            value = (chkResponse = "valid")
            Return value
        End Function

        ''' <summary>Checks AkismetComment object against Akismet database.</summary>
        ''' <param name="comment">AkismetComment object to check.</param>
        ''' <returns>'True' if spam, 'False' if not spam.</returns>
        Public Function CommentCheck(ByVal comment As AkismetComment) As Boolean
            Dim value As Boolean
            value = Convert.ToBoolean(HttpPost([String].Format(CommentCheckUrl, _apiKey), CreateData(comment), CharSet))
            Return value
        End Function

        ''' <summary>Submits AkismetComment object into Akismet database.</summary>
        ''' <param name="comment">AkismetComment object to submit.</param>
        Public Sub SubmitSpam(ByVal comment As AkismetComment)
            HttpPost([String].Format(SubmitSpamUrl, _apiKey), CreateData(comment), CharSet)
        End Sub

        ''' <summary>Retracts false positive from Akismet database.</summary>
        ''' <param name="comment">AkismetComment object to retract.</param>
        Public Sub SubmitHam(ByVal comment As AkismetComment)
            HttpPost([String].Format(SubmitHamUrl, _apiKey), CreateData(comment), CharSet)
        End Sub

        Public Function CreateComment(ByVal oFeedback As FeedbackInfo) As AkismetComment
            Dim comment As New AkismetComment
            With comment
                .Blog = NavigateURL()
                .UserIp = oFeedback.SenderRemoteAddr
                .UserAgent = oFeedback.UserAgent
                .Referrer = oFeedback.Referrer
                .Permalink = NavigateURL()
                .CommentType = "feedback"
                .CommentAuthor = oFeedback.SenderName
                .CommentAuthorEmail = oFeedback.SenderEmail
                .CommentAuthorUrl = ""
                .CommentContent = oFeedback.Message
            End With
            Return comment
        End Function

#End Region

#Region "Private Methods"
        ''' <summary>Sends HttpPost</summary>
        ''' <param name="url">URL to Post data to.</param>
        ''' <param name="data">Data to post</param>
        ''' <param name="inCharSet">Character set of your blog. example: UTF-8</param>
        ''' <returns>A System.String containing the Http Response.</returns>
        Private Function HttpPost(ByVal url As String, ByVal data As String, ByVal inCharSet As String) As String
            Dim value As String 

            ' Initialize Connection
            Dim objRequest As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
            objRequest.Method = "POST"
            objRequest.ContentType = "application/x-www-form-urlencoded; charset=" + inCharSet
            objRequest.UserAgent = _akismetUserAgent
            objRequest.ContentLength = data.Length

            ' Write Data
            Dim writer As New StreamWriter(objRequest.GetRequestStream())
            writer.Write(data)
            writer.Close()

            ' Read Response
            Dim objResponse As HttpWebResponse = DirectCast(objRequest.GetResponse(), HttpWebResponse)
            Dim reader As New StreamReader(objResponse.GetResponseStream())
            value = reader.ReadToEnd()
            reader.Close()

            Return value
        End Function

        ''' <summary>Takes an AkismetComment object and returns an (escaped) string of data to POST.</summary>
        ''' <param name="comment">AkismetComment object to translate.</param>
        ''' <returns>A System.String containing the data to POST to Akismet API.</returns>
        Private Function CreateData(ByVal comment As AkismetComment) As String
            If String.IsNullOrEmpty(comment.UserIp) Then comment.UserIp = "127.0.0.1"
            Dim value As String = String.Format("blog={0}&user_ip={1}&user_agent={2}&referrer={3}&permalink={4}&comment_type={5}" + "&comment_author={6}&comment_author_email={7}&comment_author_url={8}&comment_content={9}", _
                                  HttpUtility.UrlEncode(comment.Blog), HttpUtility.UrlEncode(comment.UserIp), HttpUtility.UrlEncode(comment.UserAgent), HttpUtility.UrlEncode(comment.Referrer), _
                                  HttpUtility.UrlEncode(comment.Permalink), HttpUtility.UrlEncode(comment.CommentType), HttpUtility.UrlEncode(comment.CommentAuthor), _
                                  HttpUtility.UrlEncode(comment.CommentAuthorEmail), HttpUtility.UrlEncode(comment.CommentAuthorUrl), HttpUtility.UrlEncode(comment.CommentContent))
            Return value
        End Function

#End Region

    End Class

#Region "Public Class AkismetComment"
    Public Class AkismetComment
        Public Blog As String = Nothing
        Public UserIp As String = Nothing
        Public UserAgent As String = Nothing
        Public Referrer As String = Nothing
        Public Permalink As String = Nothing
        Public CommentType As String = Nothing
        Public CommentAuthor As String = Nothing
        Public CommentAuthorEmail As String = Nothing
        Public CommentAuthorUrl As String = Nothing
        Public CommentContent As String = Nothing
    End Class
#End Region

End Namespace
