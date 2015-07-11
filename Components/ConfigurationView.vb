Imports DotNetNuke.Common.Utilities.DataCache
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Feedback

#Region "Configuration for View"
    Public Class ConfigurationView

#Region "Private Members"
        Private ReadOnly _allsettings As Hashtable
        Private ReadOnly _allTabsettings As Hashtable
        Private _moduleID As Integer = -1
        Private _tabModuleID As Integer = -1

        Private _enablePager As Boolean = False
        Private _defaultPageSize As Integer = 10
        Private _viewCategory As String()
        Private _selectedModules As String()

#End Region

#Region " Constructors "
        Public Sub New()
        End Sub

        Public Sub New(ByVal moduleId As Integer, ByVal tabModuleID As Integer)
            _moduleID = moduleId
            _tabModuleID = TabModuleID
            Dim mc As New ModuleController
            _allsettings = mc.GetModuleSettings(_moduleID)
            _allTabsettings = mc.GetTabModuleSettings(_tabModuleID)

            ReadValue(_allsettings, "Feedback_EnablePager", EnablePager)
            ReadValue(_allsettings, "Feedback_DefaultPageSize", DefaultPageSize)
            ReadValue(_allTabsettings, "Feedback_ViewCategory", ViewCategory)
            ReadValue(_allTabsettings, "Feedback_SelectedModules", SelectedModules)
            If SelectedModules Is Nothing Then
            ElseIf SelectedModules(0) = "-1" Or SelectedModules(0) = "" Then
                SelectedModules() = Nothing
            End If
        End Sub
#End Region

#Region " Public Methods "

        Public Sub SaveSettings(ByVal moduleId As Integer, ByVal tabModuleID As Integer)

            Dim objModules As New ModuleController
            With objModules
                .UpdateTabModuleSetting(tabModuleID, "Feedback_SelectedModules", ArrayToString(SelectedModules))
                .UpdateModuleSetting(moduleId, "Feedback_EnablePager", EnablePager.ToString)
                .UpdateModuleSetting(moduleId, "Feedback_DefaultPageSize", DefaultPageSize.ToString)
                .UpdateTabModuleSetting(tabModuleID, "Feedback_ViewCategory", ArrayToString(ViewCategory))
            End With
            Dim cacheKey As String = "ViewCommentsSettings" & moduleId.ToString
            SetCache(cacheKey, Me)

        End Sub

        Public Function GetViewCommentsConfig(ByVal moduleID As Integer, ByVal tabModuleID As Integer) As ConfigurationView
            _moduleID = moduleID
            _tabModuleID = TabModuleID
            If _moduleID > 0 Then
                Dim cacheKey As String = "ViewCommentsSettings" & moduleID.ToString
                Dim bs As ConfigurationView
                bs = CType(GetCache(cacheKey), ConfigurationView)
                If bs Is Nothing Then
                    bs = New ConfigurationView(_moduleID, _tabModuleID)
                    SetCache(cacheKey, bs)
                End If
                Return bs
            End If
            Return Nothing
        End Function

        Public Function StringToArray(ByVal inString As String) As String()
            Return inString.Split(CChar(";"))
        End Function

        Public Function ArrayToString(ByVal inArray As String()) As String
            Dim strComma As String = ""
            Dim outString As String = ""
            If Not inArray Is Nothing Then
                For Each strItem As String In inArray
                    outString = outString & strComma & strItem
                    strComma = ";"
                Next
            End If
            Return outString
        End Function

#End Region

#Region "Public Properties "

        Public Property EnablePager() As Boolean
            Get
                Return _EnablePager
            End Get
            Set(ByVal value As Boolean)
                _enablePager = value
            End Set
        End Property

        Public Property DefaultPageSize() As Integer
            Get
                Return _defaultPageSize
            End Get
            Set(ByVal value As Integer)
                _defaultPageSize = Value
            End Set
        End Property

        Public Property ViewCategory() As String()
            Get
                Return _viewCategory
            End Get
            Set(ByVal value As String())
                _viewCategory = Value
            End Set
        End Property

        Public Property SelectedModules() As String()
            Get
                Return _selectedModules
            End Get
            Set(ByVal value As String())
                _selectedModules = Value
            End Set
        End Property
#End Region


#Region " Private Methods "
        Private Shared Sub ReadValue(ByRef valueTable As Hashtable, ByVal valueName As String, ByRef variable As String())
            If Not valueTable.Item(valueName) Is Nothing Then
                Try
                    Dim cfg As New ConfigurationView
                    variable = cfg.StringToArray(CType(valueTable.Item(valueName), String))
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Shared Sub ReadValue(ByRef valueTable As Hashtable, ByVal valueName As String, ByRef variable As Integer)
            If Not valueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(valueTable.Item(ValueName), Integer)
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Shared Sub ReadValue(ByRef valueTable As Hashtable, ByVal valueName As String, ByRef variable As Boolean)
            If Not valueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(valueTable.Item(ValueName), Boolean)
                Catch ex As Exception
                End Try
            End If
        End Sub

#End Region


    End Class
#End Region

End Namespace