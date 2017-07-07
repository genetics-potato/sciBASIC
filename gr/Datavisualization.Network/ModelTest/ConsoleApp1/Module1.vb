﻿Imports System.Drawing
Imports Microsoft.VisualBasic.Mathematical.LinearAlgebra
Imports Microsoft.VisualBasic.Imaging
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Mathematical.SyntaxAPI.MathExtension
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Module Module1

    <Extension>
    Public Function Vector2D(v As Vector) As PointF
        Return New PointF(v(0), v(1))
    End Function

    ''' <summary>
    ''' 每一个节点之间都存在斥力，只有有相互连线边的节点才会存在引力
    ''' </summary>
    Sub Main()

        Call Randomize()

        Dim V As New List(Of node)
        Dim E As New List(Of edge)

        V.Add(New node With {.ID = 1})
        V.Add(New node With {.ID = 2})
        V.Add(New node With {.ID = 3})
        V.Add(New node With {.ID = 4})
        V.Add(New node With {.ID = 5})
        V.Add(New node With {.ID = 6})
        V.Add(New node With {.ID = 7})
        V.Add(New node With {.ID = 8})

        Dim add = Sub(a%, b%)
                      E.Add(New edge With {.u = V(a - 1), .v = V(b - 1)})
                  End Sub

        add(1, 2)
        add(2, 3)
        add(2, 4)
        add(2, 5)
        add(2, 6)
        add(3, 6)
        add(4, 5)
        add(3, 5)
        add(6, 7)
        add(6, 8)

        For Each u In V
            Call cat(u.ToString)
        Next


        Call SpringG(V.ToArray, E.ToArray)


        For i = 0 To 100

            For Each u In V
                For Each u2 In V
                    If Not u Is u2 Then
                        Dim d = u2.pos - u.pos
                        u.force += d.Unit * fr(d.SumMagnitude)
                    End If
                Next
            Next

            For Each uv In E
                With uv
                    Dim d = .u.pos - .v.pos
                    .v.force += d.Unit * fa(d.Mod)
                End With
            Next

            For Each u In V
                u.pos += u.force.Unit * u.force.SumMagnitude
                u.force *= 0R
            Next
        Next



        Using g = New Size(1000, 1000).CreateGDIDevice

            For Each u In V
                Call g.DrawCircle(u.pos.Vector2D, 10, Brushes.Blue)
                Call cat(u.ToString)
            Next

            For Each uv In E
                Call g.DrawLine(Pens.Red, uv.u.pos.Vector2D, uv.v.pos.Vector2D)
            Next

            Call g.Save("x:\fsdfsdf.png", ImageFormats.Png)
        End Using

        Pause()
    End Sub

    Public Sub SpringG(V As node(), E As edge())


        For i As Integer = 0 To 1000

            For Each a In V
                For Each b In V.Where(Function(x) Not x Is a)
                    ' 节点之间存在斥力
                    Dim d = a.pos - b.pos
                    Dim distance = d.SumMagnitude + 1
                    Dim direct = d.Unit
                    a.force -= repel(direct / distance)
                    b.force += repel(direct / distance)
                Next
            Next

            For Each l In E
                Dim a = l.u
                Dim b = l.v

                Dim d = a.pos - b.pos
                Dim displa = d.SumMagnitude
                Dim direct = d.Unit

                a.force -= spring(displa * direct)
                b.force += spring(displa * direct)
            Next

            For Each u In V
                u.pos += c4 * u.force
                u.force *= 0R
            Next

            Call V.Select(Function(n) n.ToString).JoinBy("   ").__DEBUG_ECHO

        Next

        Using g = New Size(1000, 1000).CreateGDIDevice

            Dim polygon = V.Select(Function(n) n.pos.Vector2D.ToPoint).ToArray.Enlarge(0.2)
            Dim coffset = polygon.CentralOffset(g.Size).ToPoint



            For Each u In polygon
                Call g.DrawCircle(u.OffSet2D(coffset), 10, Brushes.Blue)
                Call cat(u.ToString)
            Next

            For Each uv In E
                Call g.DrawLine(Pens.Red, uv.u.pos.Vector2D.OffSet2D(coffset), uv.v.pos.Vector2D.OffSet2D(coffset))
            Next

            Call g.Save("x:\fsdfsdf.png", ImageFormats.Png)
        End Using


        Pause()
    End Sub

    Const c1# = 2
    Const c2# = 1

    Const c3# = 1
    Const c4# = 0.1

    ''' <summary>
    ''' inverse square law force
    ''' </summary>
    ''' <param name="d#"></param>
    ''' <returns></returns>
    Public Function repel(d As Vector) As Vector
        Return c3 / d ^ 2
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="d#">where d is the length of the spring</param>
    ''' <returns></returns>
    Public Function spring(d As Vector) As Vector
        Return c1 * VectorMath.Log(VectorMath.Abs(d) / c2)
    End Function

    Class edge
        Public u, v As node

        Public Overrides Function ToString() As String
            Return u.ID & ", " & v.ID
        End Function
    End Class

    Public Class node
        Public Property ID$
        Public Property force As New Vector(2)
        Public Property pos As New Vector({Rnd() * 100, Rnd() * 100})

        Public Overrides Function ToString() As String
            Return pos.Vector2D.ToString ' & " --> " & force.GetJson
        End Function
    End Class

    Const k = 0.0000000000001

    ''' <summary>
    ''' attractive force
    ''' </summary>
    ''' <param name="d#"></param>
    ''' <returns></returns>
    Public Function fa(d#) As Double
        Return d ^ 2 / k
    End Function

    ''' <summary>
    ''' repulsive force
    ''' </summary>
    ''' <param name="d#"></param>
    ''' <returns></returns>
    Public Function fr(d#) As Double
        Return -k ^ 2 / d
    End Function

End Module
