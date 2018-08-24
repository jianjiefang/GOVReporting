Imports EN_DE.STRING
Imports System.DirectoryServices
Imports System.DirectoryServices.Protocols
Imports Class_Public
Imports System.Data
Imports System.Data.SqlClient

Partial Class CHK_Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim keyWord As String = ConfigurationManager.AppSettings("Key_Value")
            Dim KY As String = Request.Cookies("UserSettings")("Exdate")
            Dim UN As String = Request.Cookies("UserSettings")("UserName")
            Dim PD As String = Request.Cookies("UserSettings")("Password")
            Dim URL As String = Request.Cookies("UserSettings")("URL")
            Session("URL") = URL

            Dim Language As String = Request.Cookies("UserSettings")("Language")
            Select Case Language
                Case True
                    Session("Language") = "E"
                Case False
                    Session("Language") = "C"
            End Select

            Dim edDLL As New StringENDE
            Dim KY_String As String = edDLL.Decrypt_Key(KY, keyWord)
            Dim KY_Date As String = Mid(KY_String, 1, 4) & "-" & Mid(KY_String, 5, 2) & "-" & Mid(KY_String, 7, 2) & " " & Mid(KY_String, 9, 2) & ":" & Mid(KY_String, 11, 2) & ":" & Mid(KY_String, 13, 2)
            Dim KY_Dynamic As String = KY_String
            If IsDate(KY_Date) Then
                If DateDiff(DateInterval.Minute, CDate(KY_Date), Now) > 20 Then
                    '-----*****如果超出15分钟，将参数加密后回主页*****-----
                    If Session("Language") = "E" Then
                        Response.Write(javaMsgBox(CtoE("请先登陆！")))
                    Else
                        Response.Write(javaMsgBox("请先登陆！"))
                    End If
                    Response.Write(javaNavigation(ConfigurationManager.AppSettings("Website")))
                    Exit Sub
                End If
            Else
                '-----*****如果不是日期型，将参数加密后主页*****-----
                Response.Write(javaMsgBox("登陆超时，请重新登陆！"))
                Response.Write(javaNavigation(ConfigurationManager.AppSettings("Website")))
                Exit Sub
            End If
            Dim UN_String As String = edDLL.Decrypt_Key(UN, Right(KY_Dynamic, 8))
            Dim PD_String As String = edDLL.Decrypt_Key(PD, Right(KY_Dynamic, 8))
            If Not IsUserValid(UN_String, PD_String) Then
                Response.Write(javaMsgBox("用户名称与口令不一致，请重新输入！"))
                Response.Write(javaNavigation("http://" & URL & "Login.aspx"))
                Exit Sub
            Else
                Dim cmd As New SqlCommand()
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("FJDA_DevelopmentConnectionString").ToString)
                Conn.Open()
                cmd.Connection = Conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "P_MaterialDeliverySystem_Authority_Search_Specify"
                cmd.Parameters.Add(New SqlParameter("@Type", "Guard"))
                cmd.Parameters.Add(New SqlParameter("@Out_String", SqlDbType.VarChar, 6000)).Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()
                Session("Authority_Guard") = cmd.Parameters("@Out_String").Value

                cmd.Parameters.Clear()

                cmd.Parameters.Add(New SqlParameter("@Type", "Admin"))
                cmd.Parameters.Add(New SqlParameter("@Out_String", SqlDbType.VarChar, 6000)).Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()
                Session("Authority_Admin") = cmd.Parameters("@Out_String").Value

                cmd.Dispose()
                Conn.Close()

                Response.Write(javaNavigation("DEFAULT.ASPX"))
            End If
        Catch ex As Exception
            Dim URL As String = ConfigurationManager.AppSettings("HomeSite")
            Response.Write(javaMsgBox("Time out."))
            Response.Write(javaNavigation(URL))
        End Try

    End Sub


    Function IsUserValid(ByVal UserName As String, ByVal PassWord As String) As Boolean
        '****************************************************
        '过程或函数名称:IsUserValid
        '功能描述：经过域认证
        '输入参数：ByVal UserName As String, ByVal PassWord As String
        '返回参数：IsUserValid As Boolean
        '****************************************************
        Dim deUser As DirectoryEntry
        Try
            deUser = New DirectoryEntry("LDAP://" & ConfigurationManager.AppSettings("Domain"), UserName, PassWord, AuthenticationTypes.Secure)
            Dim Sch As New DirectorySearcher
            Sch.SearchRoot = deUser
            Sch.Filter = "(sAMAccountName=" & UserName & ")"
            Dim res As SearchResult = Sch.FindOne

            Session("User_ID") = res.GetDirectoryEntry().Guid.ToString
            Session("UserName_C") = res.GetDirectoryEntry().Properties("DisplayName").Value
            Dim ADpath() As String = res.GetDirectoryEntry.Path.Split(",")
            Dim aCounter As Integer
            For aCounter = 0 To ADpath.Length - 1
                If InStr(ADpath(aCounter), "OU=") <> 0 Then
                    Session("Department") = Mid(ADpath(aCounter), 4, Len(ADpath(aCounter)) - 3)
                    Exit For
                End If
            Next
            Session("User_Name") = UserName
            Session("User_PWD") = PassWord
            Dim native As Object = deUser.NativeObject
            deUser.Close()
            Return True
        Catch ex As Exception
            Return False
        Finally

        End Try
    End Function
End Class
