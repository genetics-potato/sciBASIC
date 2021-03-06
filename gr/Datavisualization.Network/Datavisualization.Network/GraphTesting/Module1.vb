﻿#Region "Microsoft.VisualBasic::fe9246e05688762407f3268a61a25f82, ..\sciBASIC#\gr\Datavisualization.Network\Datavisualization.Network\GraphTesting\Module1.vb"

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

Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Linq

Module Module1

    Sub Main()

        Dim graph = GraphAPI.CytoscapeExportAsGraph(
        "D:\GCModeller\src\runtime\sciBASIC#\gr\Datavisualization.Network\net_test\xcb-main-Edges.csv",
        "D:\GCModeller\src\runtime\sciBASIC#\gr\Datavisualization.Network\net_test\xcb-main-Nodes.csv")

        Dim treeModel As New GraphTree(graph)

        Call graph.ToString.__DEBUG_ECHO
        Call treeModel.ToString.__DEBUG_ECHO

        Pause()

    End Sub

    Function ExampleNetwork() As NetworkGraph
        Dim graph As New NetworkGraph
        Dim nodes = 20&.Sequence.Select(Function(x) New Graph.Node With {.ID = "#" & x}).ToArray

        nodes.DoEach(Function(node) graph.AddNode(node))

        Dim add = Sub(i%, j%)
                      graph.AddEdge(New Edge With {.Source = nodes(i), .Target = nodes(j), .ID = $"{i} --> {j}"})
                  End Sub

        ' sub 1
        add(0, 1)
        add(0, 2)
        add(0, 3)
        add(0, 4)
        add(6, 1)
        add(7, 1)
        add(5, 6)

        ' sub 2
        add(8, 9)
        add(8, 10)
        add(8, 11)
        add(12, 11)
        add(12, 13)
        add(13, 8)

        ' sub 3
        add(14, 15)
        add(15, 16)
        add(15, 17)
        add(15, 18)
        add(16, 19)

        Return graph
    End Function
End Module
