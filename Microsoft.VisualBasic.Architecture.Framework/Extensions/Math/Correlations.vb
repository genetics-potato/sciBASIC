﻿#Region "Microsoft.VisualBasic::3997c0c86d8ddaba711ceeb23ce787bc, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Extensions\Math\Correlations.vb"

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
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq.Extensions
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace Mathematical.Correlations

    <PackageNamespace("Correlations", Category:=APICategories.UtilityTools, Publisher:="amethyst.asuka@gcmodeller.org")>
    Public Module Correlations

        ''' <summary>
        ''' 假若所有的元素都是0-1之间的话，结果除以2可以得到相似度
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        <ExportAPI("SW", Info:="Sandelin-Wasserman similarity function")>
        Public Function SW(x As Double(), y As Double()) As Double
            Dim p As IEnumerable(Of Double) = From i As Integer
                                              In x.Sequence
                                              Select x(i) - y(i)
            Dim s As Double = (From n As Double In p Select n * n).Sum
            s = 2 - s
            Return s
        End Function

        <ExportAPI("KLD", Info:="Kullback-Leibler divergence")>
        Public Function KLD(x As Double(), y As Double()) As Double
            Dim index As Integer() = x.Sequence
            Dim a As Double = (From i As Integer In index Select __kldPart(x(i), y(i))).Sum
            Dim b As Double = (From i As Integer In index Select __kldPart(y(i), x(i))).Sum
            Dim value As Double = (a + b) / 2
            Return value
        End Function

        Private Function __kldPart(Xa#, Ya#) As Double
            If Xa = 0R Then
                Return 0R
            End If
            Dim value As Double = Xa * Math.Log(Xa / Ya)  ' 0 * n = 0
            Return value
        End Function

#Region "https://en.wikipedia.org/wiki/Kendall_tau_distance"

        ''' <summary>
        ''' Provides rank correlation coefficient metrics Kendall tau
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' https://github.com/felipebravom/RankCorrelation
        ''' </remarks>
        Public Function rankKendallTauBeta( x As Double(),  y As Double()) As Double
            Debug.Assert(x.Length = y.Length)
            Dim x_n As Integer = x.Length
            Dim y_n As Integer = y.Length
            Dim x_rank As Double() = New Double(x_n - 1) {}
            Dim y_rank As Double() = New Double(y_n - 1) {}
            Dim sorted As New SortedDictionary(Of Double?, HashSet(Of Integer?))()

            For i As Integer = 0 To x_n - 1
                Dim v As Double = x(i)
                If sorted.ContainsKey(v) = False Then
                    sorted(v) = New HashSet(Of Integer?)()
                End If
                sorted(v).Add(i)
            Next

            Dim c As Integer = 1
            For Each v As Double In sorted.Keys.OrderByDescending(Function(k) k)
                Dim r As Double = 0
                For Each i As Integer In sorted(v)
                    r += c
                    c += 1
                Next

                r /= sorted(v).Count

                For Each i As Integer In sorted(v)
                    x_rank(i) = r
                Next
            Next

            sorted.Clear()
            For i As Integer = 0 To y_n - 1
                Dim v As Double = y(i)
                If sorted.ContainsKey(v) = False Then
                    sorted(v) = New HashSet(Of Integer?)()
                End If
                sorted(v).Add(i)
            Next

            c = 1
            For Each v As Double In sorted.Keys.OrderByDescending(Function(k) k)
                Dim r As Double = 0
                For Each i As Integer In sorted(v)
                    r += c
                    c += 1
                Next

                r /= (sorted(v).Count)

                For Each i As Integer In sorted(v)
                    y_rank(i) = r
                Next
            Next

            Return kendallTauBeta(x_rank, y_rank)
        End Function

        ''' <summary>
        ''' Provides rank correlation coefficient metrics Kendall tau
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' https://github.com/felipebravom/RankCorrelation
        ''' </remarks>
        Public Function kendallTauBeta( x As Double(),  y As Double()) As Double
            Debug.Assert(x.Length = y.Length)

            Dim c As Integer = 0
            Dim d As Integer = 0
            Dim xTies As New Dictionary(Of Double?, HashSet(Of Integer?))()
            Dim yTies As New Dictionary(Of Double?, HashSet(Of Integer?))()

            For i As Integer = 0 To x.Length - 2
                For j As Integer = i + 1 To x.Length - 1
                    If x(i) > x(j) AndAlso y(i) > y(j) Then
                        c += 1
                    ElseIf x(i) < x(j) AndAlso y(i) < y(j) Then
                        c += 1
                    ElseIf x(i) > x(j) AndAlso y(i) < y(j) Then
                        d += 1
                    ElseIf x(i) < x(j) AndAlso y(i) > y(j) Then
                        d += 1
                    Else
                        If x(i) = x(j) Then
                            If xTies.ContainsKey(x(i)) = False Then
                                xTies(x(i)) = New HashSet(Of Integer?)()
                            End If
                            xTies(x(i)).Add(i)
                            xTies(x(i)).Add(j)
                        End If

                        If y(i) = y(j) Then
                            If yTies.ContainsKey(y(i)) = False Then
                                yTies(y(i)) = New HashSet(Of Integer?)()
                            End If
                            yTies(y(i)).Add(i)
                            yTies(y(i)).Add(j)
                        End If
                    End If
                Next
            Next

            Dim diff As Integer = c - d
            Dim denom As Double = 0

            Dim n0 As Double = (x.Length * (x.Length - 1)) / 2.0
            Dim n1 As Double = 0
            Dim n2 As Double = 0

            For Each t As Double In xTies.Keys
                Dim s As Double = xTies(t).Count
                n1 += (s * (s - 1)) / 2
            Next

            For Each t As Double In yTies.Keys
                Dim s As Double = yTies(t).Count
                n2 += (s * (s - 1)) / 2
            Next

            denom = Math.Sqrt((n0 - n1) * (n0 - n2))

            If denom = 0 Then
                denom += 0.000000001
            End If

            Dim td As Double = diff / (denom) ' 0.000..1 added on 11/02/2013 fixing NaN error

            Debug.Assert(td >= -1 AndAlso td <= 1, td)

            Return td
        End Function
#End Region

        ''' <summary>
        ''' will regularize the unusual case of complete correlation
        ''' </summary>
        Const TINY As Double = 1.0E-20

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <param name="prob"></param>
        ''' <param name="prob2"></param>
        ''' <param name="z"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' checked by Excel
        ''' </remarks>
        <ExportAPI("Pearson")>
        Public Function GetPearson(x#(), y#(), Optional ByRef prob# = 0, Optional ByRef prob2# = 0, Optional ByRef z# = 0) As Double
            Dim t#, df#
            Dim pcc As Double = GetPearson(x, y)
            Dim n As Integer = x.Length

            z = 0.5 * Math.Log((1.0 + pcc + TINY) / (1.0 - pcc + TINY))
            'fisher's z trasnformation
            df = n - 2
            t = pcc * Math.Sqrt(df / ((1.0 - pcc + TINY) * (1.0 + pcc + TINY)))
            'student's t probability
            prob = Beta.betai(0.5 * df, 0.5, df / (df + t * t))
            prob2 = Beta.erfcc(Math.Abs(z * Math.Sqrt(n - 1.0)) / 1.4142136)
            'for a large n

            Return pcc
        End Function

        <ExportAPI("Pearson")>
        Public Function GetPearson(x#(), y#()) As Double
            Dim j As Integer, n As Integer = x.Length
            Dim yt As Double, xt As Double
            Dim syy As Double = 0.0, sxy As Double = 0.0, sxx As Double = 0.0, ay As Double = 0.0, ax As Double = 0.0
            For j = 0 To n - 1
                'finds the mean
                ax += x(j)
                ay += y(j)
            Next
            ax /= n
            ay /= n
            For j = 0 To n - 1
                ' compute correlation coefficient
                xt = x(j) - ax
                yt = y(j) - ay
                sxx += xt * xt
                syy += yt * yt
                sxy += xt * yt
            Next

            Return sxy / (Math.Sqrt(sxx * syy) + TINY)
        End Function

        ''' <summary>
        ''' 相关性的计算分析函数
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <returns></returns>
        Public Delegate Function ICorrelation(X#(), Y#()) As Double

        ''' <summary>
        ''' This method should not be used in cases where the data set is truncated; that is,
        ''' when the Spearman correlation coefficient is desired for the top X records
        ''' (whether by pre-change rank or post-change rank, or both), the user should use the
        ''' Pearson correlation coefficient formula given above.
        ''' (斯皮尔曼相关性)
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' https://en.wikipedia.org/wiki/Spearman%27s_rank_correlation_coefficient
        ''' checked!
        ''' </remarks>
        '''
        <ExportAPI("Spearman",
               Info:="This method should not be used in cases where the data set is truncated; 
               that is, when the Spearman correlation coefficient is desired for the top X records 
               (whether by pre-change rank or post-change rank, or both), the user should use the 
               Pearson correlation coefficient formula given above.")>
        Public Function Spearman#(X#(), Y#())
            If X.Length <> Y.Length Then
                Dim msg As String =
                    $"[X:={X.Length}, Y:={Y.Length}] The vector length betwen the two samples is not agreed!!!"
                Throw New DataException(msg)
            ElseIf X.Length = 1 Then
                Throw New DataException(UnableMeasures)
            End If

            Dim n As Integer = X.Length  ' size n
            Dim Xx As spcc() = __getOrder(X)
            Dim Yy As spcc() = __getOrder(Y)

            Dim spcc As Double =
                1 - 6 * (From i As Integer
                         In n.Sequence
                         Select (Xx(i).rank - Yy(i).rank) ^ 2).Sum / (n ^ 3 - n)
            Return spcc
        End Function

        Const UnableMeasures$ = "Samples number just equals 1, the function unable to measure the correlation!!!"

        Private Function __getOrder(samples#()) As spcc()
            Dim dat = (From i As Integer
                       In samples.Sequence
                       Select spcc = New spcc.__spccInner With {  ' 原有的顺序
                           .i = i,
                           .val = samples(i)
                       }
                       Order By spcc.val Ascending).ToArray  ' 从小到大排序
            Dim buf = (From p As Integer  ' rank
                       In dat.Sequence
                       Select spcc = New spcc With {
                           .rank = p,
                           .data = dat(p)
                       }
                       Group spcc By spcc.data.val Into Group) _
                        .ToDictionary(Function(x) x.val,
                                      Function(x) x.Group.ToArray)

            Dim rankList As New List(Of spcc)

            For Each item As spcc() In buf.Values
                If item.Length = 1 Then
                    Call rankList.Add(item(Scan0))
                Else
                    Dim rank As Double = item.Select(Function(x) x.rank).Average
                    Dim array As spcc() = item.ToArray(
                        Function(x) New spcc With {
                            .rank = rank,
                            .data = x.data
                        })
                    Call rankList.AddRange(array)
                End If
            Next

            ' 重新按照原有的顺序返回
            Return (From x As spcc
                    In rankList
                    Select x
                    Order By x.data.i Ascending).ToArray
        End Function

        ''' <summary>
        ''' 计算所需要的临时变量类型
        ''' </summary>
        Private Structure spcc
            ''' <summary>
            ''' 排序之后得到的位置
            ''' </summary>
            Public rank As Double
            ''' <summary>
            ''' 原始数据
            ''' </summary>
            Public data As __spccInner

            Public Structure __spccInner
                ''' <summary>
                ''' 在序列之中原有的位置
                ''' </summary>
                Public i As Integer
                Public val As Double
            End Structure
        End Structure

        <Extension>
        Public Function CorrelationMatrix(data As IEnumerable(Of NamedValue(Of Double())), Optional compute As ICorrelation = Nothing) As NamedValue(Of Dictionary(Of String, Double))()
            If compute Is Nothing Then
                compute = AddressOf GetPearson
            End If

            Dim array = data.ToArray
            Dim out As New List(Of NamedValue(Of Dictionary(Of String, Double)))

            For Each a In array
                Dim ca As New Dictionary(Of String, Double)

                For Each b In array
                    ca(b.Name) = compute(a.Value, b.Value)
                Next

                out += New NamedValue(Of Dictionary(Of String, Double)) With {
                    .Name = a.Name,
                    .Value = ca
                }
            Next

            Return out
        End Function
    End Module

    Public Module Beta

        Const SWITCH As Integer = 3000, MAXIT As Integer = 1000
        Const EPS As Double = 0.0000003, FPMIN As Double = 1.0E-30

        Public Function betai(a As Double, b As Double, x As Double) As Double
            Dim bt As Double

            If x < 0.0 OrElse x > 1.0 Then
                Throw New ArgumentException($"Bad x:={x} in routine betai")
            End If
            If x = 0.0 OrElse x = 1.0 Then
                bt = 0.0
            Else
                bt = Math.Exp(gammln(a + b) - gammln(a) - gammln(b) + a * Math.Log(x) + b * Math.Log(1.0 - x))
            End If
            If x < (a + 1.0) / (a + b + 2.0) Then
                Return bt * betacf(a, b, x) / a
            Else
                Return 1.0 - bt * betacf(b, a, 1.0 - x) / b
            End If
        End Function

        Private Function gammln(xx As Double) As Double
            Dim x As Double = xx, y As Double, tmp As Double, ser As Double
            Dim j As Integer

            y = x
            tmp = x + 5.5
            tmp -= (x + 0.5) * Math.Log(tmp)
            ser = 1.00000000019001
            For j = 0 To 5
                y += 1 ' ++y
                ser += cof(j) / y
            Next

            Return -tmp + Math.Log(2.506628274631 * ser / x)
        End Function

        ReadOnly cof As Double() = {
            76.1800917294715,
            -86.5053203294168,
            24.0140982408309,
            -1.23173957245015,
            0.00120865097386618,
            -0.000005395239384953
        }

        Private Function betacf(a As Double, b As Double, x As Double) As Double
            Dim m As Integer, m2 As Integer
            Dim aa As Double,
            c As Double,
            d As Double,
            del As Double,
            h As Double,
            qab As Double,
            qam As Double,
            qap As Double

            qab = a + b
            qap = a + 1.0
            qam = a - 1.0
            c = 1.0
            d = 1.0 - qab * x / qap
            If Math.Abs(d) < FPMIN Then
                d = FPMIN
            End If
            d = 1.0 / d
            h = d

            For m = 1 To MAXIT
                m2 = 2 * m
                aa = m * (b - m) * x / ((qam + m2) * (a + m2))
                d = 1.0 + aa * d
                If Math.Abs(d) < FPMIN Then
                    d = FPMIN
                End If
                c = 1.0 + aa / c
                If Math.Abs(c) < FPMIN Then
                    c = FPMIN
                End If
                d = 1.0 / d
                h *= d * c
                aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2))
                d = 1.0 + aa * d
                If Math.Abs(d) < FPMIN Then
                    d = FPMIN
                End If
                c = 1.0 + aa / c
                If Math.Abs(c) < FPMIN Then
                    c = FPMIN
                End If
                d = 1.0 / d
                del = d * c
                h *= del
                If Math.Abs(del - 1.0) < EPS Then
                    Exit For
                End If
            Next
            If m > MAXIT Then
                Dim msg As String =
                $"a:={a} or b:={b} too big, or MAXIT too small in betacf"
                Throw New ArgumentException(msg)
            End If
            Return h
        End Function

        Public Function erfcc(x As Double) As Double
            Dim t As Double, z As Double, ans As Double

            z = Math.Abs(x)
            t = 1.0 / (1.0 + 0.5 * z)
            ans = t * Math.Exp(-z * z - 1.26551223 +
                           t * (1.00002368 +
                           t * (0.37409196 +
                           t * (0.09678418 +
                           t * (-0.18628806 +
                           t * (0.27886807 +
                           t * (-1.13520398 +
                           t * (1.48851587 +
                           t * (-0.82215223 +
                           t * 0.17087277)))))))))
            Return If(x >= 0.0, ans, 2.0 - ans)
        End Function
    End Module
End Namespace
