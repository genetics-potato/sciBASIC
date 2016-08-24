﻿#Region "Microsoft.VisualBasic::f639573b0dfec878b3707d7c649fa9c4, ..\visualbasic_App\Microsoft.VisualBasic.Architecture.Framework\Extensions\IO\SymLinker\HardLink.vb"

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

Imports System.Runtime.InteropServices

Namespace FileIO.SymLinker

    ''' <summary>
    ''' Provides access to NTFS hard links in .Net.
    ''' </summary>
    Public Module HardLink

        <DllImport("Kernel32.dll", CharSet:=CharSet.Unicode)>
        Public Function CreateHardLink(lpFileName As String, lpExistingFileName As String, lpSecurityAttributes As IntPtr) As Boolean
        End Function
    End Module
End Namespace
