﻿#Region "Microsoft.VisualBasic::b44805f59963a13aedfe27a617f274e6, ..\sciBASIC#\gr\Microsoft.VisualBasic.Imaging\d3js\scale\IScale.vb"

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

Imports Microsoft.VisualBasic.ComponentModel.Ranges

Namespace d3js.scale

    Public MustInherit Class IScale(Of T As IScale(Of T))

        Default Public MustOverride ReadOnly Property Value(x#) As Double
        Default Public MustOverride ReadOnly Property Value(term$) As Double

        Protected _range As DoubleRange = {0, 1}

        Public MustOverride Function domain(values As IEnumerable(Of Double)) As T
        Public MustOverride Function domain(values As IEnumerable(Of String)) As T
        Public MustOverride Function domain(values As IEnumerable(Of Integer)) As T

        ''' <summary>
        ''' If range is specified, sets the range of the ordinal scale to the specified array of values. 
        ''' The first element in the domain will be mapped to the first element in range, the second 
        ''' domain value to the second range value, and so on. If there are fewer elements in the range 
        ''' than in the domain, the scale will reuse values from the start of the range. If range is 
        ''' not specified, this method returns the current range.
        ''' </summary>
        ''' <param name="values"></param>
        ''' <returns></returns>
        Public Function range(Optional values As IEnumerable(Of Double) = Nothing) As T
            _range = values.ToArray
            Return Me
        End Function

    End Class
End Namespace
