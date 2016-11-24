﻿Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Language

Namespace MonteCarlo

    Public Module CodeGenerator

        <Extension> Public Function ConstAlpha(v$) As String
            Return v & "_alpha#"
        End Function

        <Extension> Public Function ConstBeta(v$) As String
            Return v & "_beta#"
        End Function

        <Extension> Public Function SPowerAlpha(v$, x$) As String
            Return v & "_" & x & "_alpha#"
        End Function

        <Extension> Public Function SPowerBeta(v$, x$) As String
            Return v & "_" & x & "_beta#"
        End Function

        ''' <summary>
        ''' Generates the S-system non-linear model VisualBasic Class.
        ''' </summary>
        ''' <param name="var$"></param>
        ''' <param name="name">
        ''' Class Name, this value should match the VisualBasic identifier rule.
        ''' </param>
        ''' <returns></returns>
        <Extension> Public Function SNonLinear(var$(), name$, Optional [namespace] As String = Nothing) As String
            Dim code As New StringBuilder

            Call code.AppendLine("Imports Microsoft.VisualBasic.Data.Bootstrapping")
            Call code.AppendLine("Imports Microsoft.VisualBasic.Data.Bootstrapping.MonteCarlo")
            Call code.AppendLine("Imports Microsoft.VisualBasic.Mathematical.Calculus")
            Call code.AppendLine("Imports Microsoft.VisualBasic.Mathematical.LinearAlgebra")
            Call code.AppendLine()

            If Not String.IsNullOrEmpty([namespace]) Then
                Call code.AppendLine($"Namespace {[namespace]}")
                Call code.AppendLine()
            End If

            Call code.AppendLine($"Public Class {name$} : Inherits MonteCarlo.Model")
            Call code.AppendLine()

            ' Generates the constants

            For Each v$ In var
                Call code.AppendLine($"Dim {(v).ConstAlpha }, {(v).ConstBeta}")
            Next
            Call code.AppendLine()

            ' Generates the S-powers

            For Each v$ In var
                Dim a As New List(Of String)
                Dim b As New List(Of String)

                For Each x$ In var
                    a += SPowerAlpha(v, x)
                    b += SPowerBeta(v, x)
                Next

                Dim line$ = $"Dim {a.JoinBy(", ")}, {b.JoinBy(", ")}"

                Call code.AppendLine(line)
            Next
            Call code.AppendLine()

            ' Generates the variables
            For Each v$ In var
                Call code.AppendLine($"Dim {v} As var")
            Next

            ' Generates the S-system non-linear model
            Call code.AppendLine()
            Call code.AppendLine("Protected Overrides Sub func(dx As Double, ByRef dy As Vector)")
            Call code.AppendLine()

            For Each v$ In var
                Dim a As New List(Of String)
                Dim b As New List(Of String)

                For Each x In var
                    a += $"({x} ^ {SPowerAlpha(v, x)})"
                    b += $"({x} ^ {SPowerBeta(v, x)})"
                Next

                Dim alpha$ = (v).ConstAlpha & " * " & a.JoinBy(" * ")
                Dim beta$ = (v).ConstBeta & " * " & b.JoinBy(" * ")

                Call code.AppendLine($"dy({v}) = {alpha} - {beta}")
            Next

            Call code.AppendLine()
            Call code.AppendLine("End Sub")
            Call code.AppendLine()
            Call code.AppendLine("        
Public Overrides Function eigenvector() As Dictionary(Of String, Eigenvector)
    Throw New NotImplementedException
End Function

Public Overrides Function params() As VariableModel()
    Throw New NotImplementedException
End Function

Public Overrides Function yinit() As VariableModel()
    Throw New NotImplementedException
End Function")
            Call code.AppendLine()
            Call code.AppendLine("End Class")

            If Not String.IsNullOrEmpty([namespace]) Then
                Call code.AppendLine("End Namespace")
            End If

            Return code.ToString
        End Function
    End Module
End Namespace