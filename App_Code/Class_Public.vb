'*****************************************************
'程序文件名：Class_Public.vb
'作者：Leon
'创建时间：20080409
'隶属项目名称：SWT Reporting Tool
'隶属模块名称：Public Class
'程序功能说明：公用处理类
'程序功能描述：实现在程序中公有类的调用
'版权所有：FJDA
'HISTORY 
' 
'******************************************************

Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class Class_Public
    Public Shared Function javaMsgBox(ByVal Str As String) As String
        '-------用Script的形式返回对话框信息---------
        javaMsgBox = "<Script> alert('" & chkString(Str) & "');</Script>"
    End Function

    Public Shared Function javaNavigation(ByVal Str As String) As String
        '-------用Script的形式返回对话框信息---------
        javaNavigation = "<Script> window.navigate('" & chkString(Str) & "');</Script>"
    End Function

    Public Shared Function chkString(ByVal str As String) As String
        '-------将单用“'”替换成双个双引号---------
        Dim tmp_String As String
        tmp_String = str
        tmp_String = Replace(tmp_String, "'", """")
        tmp_String = Replace(tmp_String, Chr(13) & Chr(10), "")
        chkString = tmp_String
    End Function

    Public Shared TitlelName As String = "Fujian Daimler - OA - "

    Public Shared Function CtoE(ByVal CName As String) As String
        Dim cmd As New SqlCommand()
        Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("FJDA_DevelopmentConnectionString").ToString)
        Conn.Open()
        cmd.Connection = Conn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "P_OA_ENCN_SEARCH_BY_NAME"
        cmd.Parameters.Add(New SqlParameter("@CN_Name", CName))
        cmd.Parameters.Add(New SqlParameter("@EN_Name", SqlDbType.VarChar, 1000)).Direction = ParameterDirection.Output
        cmd.ExecuteNonQuery()
        CtoE = cmd.Parameters("@EN_Name").Value
        Conn.Close()
    End Function
End Class

