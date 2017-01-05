﻿#Region "Microsoft.VisualBasic::bc4366840a8e57261deed40bd615906b, ..\sciBASIC#\Data_science\Mathematical\Math.Statistics\src\TabularFunctions\ISampleWithUncertainty.vb"

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

Imports Microsoft.VisualBasic.Language

'
' * To change this license header, choose License Headers in Project Properties.
' * To change this template file, choose Tools | Templates
' * and open the template in the editor.
' 
Namespace TabularFunctions

	''' 
	''' <summary>
	''' @author Will_and_Sara
	''' </summary>
	Public Interface ISampleWithUncertainty
		Inherits IWriteToXML

		Function GetYFromX( x As Double,  probability As Double) As Double
		Function GetYValues( probability As Double) As List(Of Double)
		Function GetYDistributions() As List(Of Distributions.ContinuousDistribution)
		Function CurveSample( probability As Double) As ISampleDeterministically
	End Interface

End Namespace