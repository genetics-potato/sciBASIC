Imports System.Collections.Generic
Imports System.Text

Namespace YAML.Syntax
    Public Class YamlDocument
        Public Root As DataItem

        Public Directives As New List(Of Directive)()
        Public AnchoredItems As New Dictionary(Of String, DataItem)()
    End Class
End Namespace
