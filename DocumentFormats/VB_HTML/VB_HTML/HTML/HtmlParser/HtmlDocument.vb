﻿#Region "Microsoft.VisualBasic::575dc5ebb6b52ecc117160ceeb2fb6ff, ..\VB_HTML\VB_HTML\HTML\HtmlParser\HtmlDocument.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text

Namespace HTML

    Public Class HtmlDocument

        Public Const HTML_PAGE_CONTENT_TITLE As String = "<title>.+?</title>"

        Public Property Tags As InnerPlantText()

        ''' <summary>
        ''' 假设所加载的html文档是完好的格式的，即没有不匹配的标签的
        ''' </summary>
        ''' <param name="url"></param>
        ''' <returns></returns>
        Public Function LoadDocument(url As String) As HtmlDocument
            Dim pageContent As String = url.GET.Replace(vbCr, "").Replace(vbLf, "") '是使用<br />标签来分行的
            Dim List As List(Of InnerPlantText) = New List(Of InnerPlantText)

            pageContent = Regex.Replace(pageContent, "<!--.+?-->", "")

            Do While pageContent.Length > 0
                Dim element As InnerPlantText = DocParserAPI.TextParse(pageContent)
                If element Is Nothing Then
                    Exit Do
                Else
                    If Not element.IsEmpty Then
                        Call List.Add(element)
                    End If
                End If
            Loop

            Return Me.InvokeSet(NameOf(Tags), List.ToArray)
        End Function

        Public Shared Function Load(url As String) As HtmlDocument
            Return New HtmlDocument().LoadDocument(url)
        End Function
    End Class
End Namespace
