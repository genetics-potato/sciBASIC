﻿#Region "Microsoft.VisualBasic::13ac48f74261f4bf71476fddfb4520d0, ..\visualbasic_App\Data_science\Mathematical\Plots\Heatmap.vb"

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

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.DocumentStream
Imports Microsoft.VisualBasic.Linq

Public Module Heatmap

    <Extension>
    Public Iterator Function Pearson(data As IEnumerable(Of DataSet)) As IEnumerable(Of NamedValue(Of Dictionary(Of String, Double)))
        Dim dataset As DataSet() = data.ToArray
        Dim keys As String() = dataset(Scan0).Properties.Keys.ToArray

        For Each x As DataSet In dataset
            Dim out As New Dictionary(Of String, Double)
            Dim array As Double() = keys.ToArray(Function(o) x(o))

            For Each y As DataSet In dataset
                out(y.Identifier) = Correlations.GetPearson(
                    array,
                    keys.ToArray(Function(o) y(o)))
            Next

            Yield New NamedValue(Of Dictionary(Of String, Double)) With {
                .Name = x.Identifier,
                .x = out
            }
        Next
    End Function
End Module

