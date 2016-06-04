'! 
'@file AbstractRenderer.cs
'@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
'		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
'@date August 08, 2013
'@brief Abstract Renderer Interface
'@version 1.0
'
'@section LICENSE
'
'The MIT License (MIT)
'
'Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>
'
'Permission is hereby granted, free of charge, to any person obtaining a copy
'of this software and associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
'copies of the Software, and to permit persons to whom the Software is
'furnished to do so, subject to the following conditions:
'
'The above copyright notice and this permission notice shall be included in
'all copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
'OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
'THE SOFTWARE.
'
'@section DESCRIPTION
'
'An Interface for the Abstract Renderer Class.
'
'

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public MustInherit Class AbstractRenderer
	Implements IRenderer
	Protected forceDirected As IForceDirected
	Public Sub New(iForceDirected As IForceDirected)
		forceDirected = iForceDirected
	End Sub


	Public Sub Draw(iTimeStep As Single) Implements IRenderer.Draw
		forceDirected.Calculate(iTimeStep)
		Clear()
		forceDirected.EachEdge(Sub(edge As Edge, spring As Spring) drawEdge(edge, spring.point1.position, spring.point2.position))
		forceDirected.EachNode(Sub(node As Node, point As Point) drawNode(node, point.position))
	End Sub
	Public MustOverride Sub Clear()
	Protected MustOverride Sub drawEdge(iEdge As Edge, iPosition1 As AbstractVector, iPosition2 As AbstractVector)
	Protected MustOverride Sub drawNode(iNode As Node, iPosition As AbstractVector)

End Class
