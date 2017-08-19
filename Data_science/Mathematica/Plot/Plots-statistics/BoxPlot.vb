﻿#Region "Microsoft.VisualBasic::53cae1cfbdad7b74daf62519184de1a4, ..\sciBASIC#\Data_science\Mathematica\Plot\Plots-statistics\BoxPlot.vb"

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

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Math.Quantile
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime

''' <summary>
''' ```
''' min, q1, q2, q3, max
'''       _________
'''  +----|   |   |----+
'''       ---------
''' ```
''' </summary>
Public Module BoxPlot

    <Extension> Public Function Plot(data As BoxData,
                                     Optional size$ = "3000,2700",
                                     Optional padding$ = g.DefaultPadding,
                                     Optional bg$ = "white",
                                     Optional schema$ = ColorBrewer.QualitativeSchemes.Set1_9,
                                     Optional groupLabelCSSFont$ = CSSFont.Win7LittleLarge,
                                     Optional YAxisLabelFontCSS$ = CSSFont.Win7LittleLarge,
                                     Optional tickFontCSS$ = CSSFont.Win7Normal,
                                     Optional regionStroke$ = Stroke.AxisStroke,
                                     Optional interval# = 100,
                                     Optional dotSize! = 10,
                                     Optional lineWidth% = 2,
                                     Optional showDataPoints As Boolean = True,
                                     Optional showOutliers As Boolean = True) As GraphicsData

        Dim yAxisLabelFont As Font = CSSFont.TryParse(YAxisLabelFontCSS)
        Dim groupLabelFont As Font = CSSFont.TryParse(groupLabelCSSFont)
        Dim tickLabelFont As Font = CSSFont.TryParse(tickFontCSS)
        Dim ranges As DoubleRange = data _
            .Groups _
            .Select(Function(x) x.Value) _
            .IteratesALL _
            .ToArray
        Dim colors As LoopArray(Of SolidBrush) = Designer _
            .GetColors(schema) _
            .Select(Function(color) New SolidBrush(color)) _
            .ToArray

        Dim plotInternal =
            Sub(ByRef g As IGraphics, rect As GraphicsRegion)

                Dim plotRegion = rect.PlotRegion
                Dim leftPart = yAxisLabelFont.Height + tickLabelFont.Height + 50
                Dim bottomPart = groupLabelFont.Height + 50

                With plotRegion

                    Dim topLeft = .Location.OffSet2D(leftPart, 0)
                    Dim rectSize As New Size(
                        width:= .Width - leftPart,
                        height:= .Height - bottomPart)

                    plotRegion = New Rectangle(topLeft, rectSize)
                End With

                Dim boxWidth = StackedBarPlot.BarWidth(plotRegion.Width, data.Groups.Length, interval)
                Dim bottom = plotRegion.Bottom
                Dim height = plotRegion.Height
                Dim y = Function(x#) bottom - height * (x - ranges.Min) / ranges.Length

                If Not regionStroke.StringEmpty Then
                    Call g.DrawRectangle(
                        Stroke.TryParse(regionStroke).GDIObject,
                        plotRegion)
                End If

                ' x0在盒子的左边
                Dim x0! = rect.Padding.Left + leftPart + interval
                Dim y0!

                ' 绘制盒子
                For Each group As NamedValue(Of Vector) In data.Groups
                    Dim quartile = group.Value.Quartile
                    Dim outlier = group.Value.Outlier(quartile)
                    Dim brush As SolidBrush = colors.Next
                    Dim pen As New Pen(brush.Color, lineWidth)
                    Dim x1 = x0 + boxWidth / 2  ' x1在盒子的中间

                    If Not outlier.Outlier.IsNullOrEmpty Then
                        quartile = outlier.Normal.Quartile
                    End If

                    ' max
                    y0 = y(quartile.range.Max)
                    g.DrawLine(pen, New Point(x0, y0), New Point(x0 + boxWidth, y0))

                    ' min
                    y0 = y(quartile.range.Min)
                    g.DrawLine(pen, New Point(x0, y0), New Point(x0 + boxWidth, y0))

                    ' q1
                    Dim q1Y = y(quartile.Q1)
                    g.DrawLine(pen, New Point(x0, q1Y), New Point(x0 + boxWidth, q1Y))

                    ' q2
                    Dim q2Y = y(quartile.Q2)
                    g.DrawLine(pen, New Point(x0, q2Y), New Point(x0 + boxWidth, q2Y))

                    ' q3
                    Dim q3Y = y(quartile.Q3)
                    g.DrawLine(pen, New Point(x0, q3Y), New Point(x0 + boxWidth, q3Y))

                    ' box
                    g.DrawLine(pen, New Point(x0, q3Y), New Point(x0, q1Y))
                    g.DrawLine(pen, New Point(x0 + boxWidth, q3Y), New Point(x0 + boxWidth, q1Y))

                    ' outliers + normal points
                    If showDataPoints Then
                        For Each n As Double In outlier.Normal
                            Call g.FillEllipse(brush, New PointF(x1, y(n)).CircleRectangle(dotSize))
                        Next
                    End If
                    If showOutliers Then
                        For Each n As Double In outlier.Outlier
                            Call g.FillEllipse(brush, New PointF(x1, y(n)).CircleRectangle(dotSize))
                        Next
                    End If

                    x0 += boxWidth + interval
                Next
            End Sub

        Return g.GraphicsPlots(
            size.SizeParser, padding,
            bg,
            plotInternal)
    End Function
End Module

