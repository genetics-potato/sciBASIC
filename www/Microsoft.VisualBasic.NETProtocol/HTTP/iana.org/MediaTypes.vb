﻿#Region "Microsoft.VisualBasic::749a20fd8b873b7dc890f8d5719ffd9a, ..\sciBASIC#\www\Microsoft.VisualBasic.NETProtocol\HTTP\iana.org\MediaTypes.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
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


''' <summary>
''' Csv file reader for the csv file list on https://www.iana.org/assignments/media-types/media-types.xhtml
''' 
''' + [application](https://www.iana.org/assignments/media-types/application.csv)
''' + [audio](https://www.iana.org/assignments/media-types/audio.csv)
''' + [font](https://www.iana.org/assignments/media-types/font.csv)
''' + [example]()
''' + [image](https://www.iana.org/assignments/media-types/image.csv)
''' + [message](https://www.iana.org/assignments/media-types/message.csv)
''' + [model](https://www.iana.org/assignments/media-types/model.csv)
''' + [multipart](https://www.iana.org/assignments/media-types/multipart.csv)
''' + [text](https://www.iana.org/assignments/media-types/text.csv)
''' + [video](https://www.iana.org/assignments/media-types/video.csv)
''' </summary>
Public Class MediaTypes
    Public Property Name As String
    Public Property Template As String
    Public Property Reference As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

