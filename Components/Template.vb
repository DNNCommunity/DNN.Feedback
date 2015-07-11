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

Imports System.Web.UI.WebControls
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Feedback
    <Serializable()>
    Public Class Template

#Region "Enums"

        Public Enum TemplateTypes As Integer
            Header = 0
            Item
            Separator
            AltItem
            Footer
        End Enum

#End Region

#Region "Private Members"

        Private ReadOnly _localResourceFile As String = ApplicationPath + "/DesktopModules/Feedback/App_LocalResources/CommentSettings.ascx.resx"

        Private ReadOnly _settings As Hashtable = Nothing
        Private ReadOnly _tabModuleId As Integer = -1

        Private _templateName As String = "Default"
        Private _headerTemplate As String
        Private _itemTemplate As String
        Private _separatorTemplate As String
        Private _altItemTemplate As String
        Private _footerTemplate As String
#End Region

#Region "Public Properties"

        Public ReadOnly Property TemplateCacheKey() As String
            Get
                Return GetTemplateCacheKey(_tabModuleId)
            End Get
        End Property

        Public Property TemplateName() As String
            Get
                Return _templateName
            End Get
            Set(ByVal value As String)
                _templateName = value
            End Set
        End Property

        Public Property HeaderTemplate() As String
            Get
                Return _headerTemplate
            End Get
            Set(ByVal value As String)
                _headerTemplate = value
            End Set
        End Property

        Public Property ItemTemplate() As String
            Get
                Return _itemTemplate
            End Get
            Set(ByVal value As String)
                _itemTemplate = value
            End Set
        End Property

        Public Property SeparatorTemplate() As String
            Get
                Return _separatorTemplate
            End Get
            Set(ByVal value As String)
                _separatorTemplate = value
            End Set
        End Property

        Public Property AltItemTemplate() As String
            Get
                Return _altItemTemplate
            End Get
            Set(ByVal value As String)
                _altItemTemplate = value
            End Set
        End Property

        Public Property FooterTemplate() As String
            Get
                Return _footerTemplate
            End Get
            Set(ByVal value As String)
                _footerTemplate = value
            End Set
        End Property

        Public ReadOnly Property TokenReplaceNeeded() As Boolean
            Get
                Return (Not String.IsNullOrEmpty(HeaderTemplate)) Or (Not String.IsNullOrEmpty(FooterTemplate)) Or (Not String.IsNullOrEmpty(SeparatorTemplate))
            End Get
        End Property
#End Region

        Private Function GetTemplateItemKey(ByVal templateType As TemplateTypes) As String
            Return "Feedback_" & templateType.ToString & "Template"
        End Function

        Public Shared Function GetTemplateCacheKey(ByVal tabModuleId As Integer) As String
            Return "Feedback_Template_" + tabModuleId.ToString
        End Function

        Public Overloads Function GetTemplate(ByVal templateType As TemplateTypes, ByVal loadDefault As Boolean) As String
            Dim key As String = GetTemplateItemKey(templateType)
            If loadDefault OrElse (_settings(key) Is Nothing OrElse CType(_settings(key), String).Length = 0) Then
                Return Regex.Replace(Localization.GetString(key & ".Text", _localResourceFile), "^(\[L\])?(.*)", "$2")
            Else
                Return CType(_settings(key), String)
            End If
        End Function

        Public Overloads Function GetTemplate(ByVal templateType As TemplateTypes) As String
            Return GetTemplate(templateType, False)
        End Function

        Public Sub New(ByVal tabModuleId As Integer, ByVal settings As Hashtable)
            _settings = settings
            _tabModuleId = tabModuleId

            HeaderTemplate = GetTemplate(TemplateTypes.Header)
            ItemTemplate = GetTemplate(TemplateTypes.Item)
            SeparatorTemplate = GetTemplate(TemplateTypes.Separator)
            AltItemTemplate = GetTemplate(TemplateTypes.AltItem)
            FooterTemplate = GetTemplate(TemplateTypes.Footer)
        End Sub

        Public Shared Function LoadTemplate(ByVal moduleId As Integer, ByVal tabModuleId As Integer, ByVal settings As Hashtable) As Template
            Dim cacheKey As String = GetTemplateCacheKey(tabModuleId)
            Dim objTemplate As Template = DataCache.GetCache(Of Template)(cacheKey)
            If objTemplate Is Nothing Then
                objTemplate = New Template(tabModuleId, settings)
                DataCache.SetCache(cacheKey, objTemplate)
            End If
            Return objTemplate
        End Function

        Public Sub UpdateTemplate()
            Dim moduleController As New Entities.Modules.ModuleController
            moduleController.UpdateTabModuleSetting(_tabModuleId, GetTemplateItemKey(TemplateTypes.Item), ItemTemplate)
            moduleController.UpdateTabModuleSetting(_tabModuleId, GetTemplateItemKey(TemplateTypes.AltItem), AltItemTemplate)
            moduleController.UpdateTabModuleSetting(_tabModuleId, GetTemplateItemKey(TemplateTypes.Separator), SeparatorTemplate)
            moduleController.UpdateTabModuleSetting(_tabModuleId, GetTemplateItemKey(TemplateTypes.Header), HeaderTemplate)
            moduleController.UpdateTabModuleSetting(_tabModuleId, GetTemplateItemKey(TemplateTypes.Footer), FooterTemplate)

            DataCache.RemoveCache(TemplateCacheKey)

        End Sub

        Public Function CreateTemplate(ByVal type As ListItemType) As System.Web.UI.ITemplate

            Dim lit As String = Nothing

            Select Case type
                Case ListItemType.Header
                    lit = HeaderTemplate
                Case ListItemType.Item
                    lit = ItemTemplate
                Case ListItemType.Separator
                    lit = SeparatorTemplate
                Case ListItemType.AlternatingItem
                    If String.IsNullOrEmpty(AltItemTemplate) Then
                        lit = ItemTemplate
                    Else
                        lit = AltItemTemplate
                    End If
                Case ListItemType.Footer
                    lit = FooterTemplate
            End Select

            If String.IsNullOrEmpty(lit) Then
                Return Nothing
            Else
                Return New DataboundListTemplate(type, lit)
            End If
        End Function

    End Class

    Public Class DataboundListTemplate
        Implements System.Web.UI.ITemplate

        Private ReadOnly _templateType As ListItemType
        Private ReadOnly _literal As String

        Sub New(ByVal type As ListItemType, ByVal literal As String)
            _templateType = type
            _literal = literal
        End Sub

        Public Shared Function CreateTemplate(ByVal type As ListItemType, ByVal literal As String) As System.Web.UI.ITemplate
            If String.IsNullOrEmpty(literal) Then
                Return Nothing
            Else
                Return New DataboundListTemplate(type, literal)
            End If
        End Function

        Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

            Dim ph As New PlaceHolder
            Dim lit As System.Web.UI.LiteralControl = Nothing

            If Not String.IsNullOrEmpty(_literal) Then
                Select Case (_templateType)
                    Case ListItemType.Header, ListItemType.Footer, ListItemType.Separator
                        lit = New System.Web.UI.LiteralControl(_literal)
                    Case ListItemType.Item
                        lit = New System.Web.UI.LiteralControl(_literal)
                    Case ListItemType.AlternatingItem
                        lit = New System.Web.UI.LiteralControl(_literal)
                End Select

                If lit IsNot Nothing Then
                    ph.Controls.Add(lit)
                    container.Controls.Add(ph)
                End If
            End If
        End Sub
    End Class
End Namespace
