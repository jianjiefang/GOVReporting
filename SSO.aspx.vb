Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Data.SqlClient
Partial Class SSO
    Inherits System.Web.UI.Page

    Private Sub SSO_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim Token As String = Request.QueryString("token")
        Dim RedirectTo As String = Request.QueryString("RedirectTo")
        If Not String.IsNullOrEmpty(Token) Then
            Dim UserLoginName As String = ""
            Dim errorMessage As String = ""
            Dim success As Boolean = ValidateToken(Token, UserLoginName, errorMessage)
            If (success And Not String.IsNullOrEmpty(RedirectTo)) Then
                Session("UserLoginName") = UserLoginName
                Dim cmd As New SqlCommand()
                Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("FJDA_DevelopmentConnectionString").ToString)
                Conn.Open()
                cmd.Connection = Conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "P_SSO_Load"
                cmd.Parameters.Add(New SqlParameter("@UserLoginName", Session("UserLoginName")))
                Dim Result As SqlDataReader = cmd.ExecuteReader()
                If Result.Read Then
                    Session("User_ID") = Trim(Result("ID"))
                    Session("User_Name") = UserLoginName
                    Session("UserManagment_Signal") = True
                Else
                    Session("UserManagment_Signal") = False
                End If
                Result.Close()
                Conn.Close()
                Response.Redirect(RedirectTo)
            End If

        End If
    End Sub

    Function ValidateToken(ByVal Token As String, ByRef UserLoginName As String, ByRef errorMessage As String) As Boolean
        UserLoginName = ""
        errorMessage = ""
        Dim url As String = String.Format("http://eip.fujianbenz.com/_layouts/15/g2/ws/sso.aspx?token={0}", Token)
        Dim Result As String = HttpGet(url)
        If (String.Format("{0}", Result).ToLower().Contains("error")) Then
            Return False
        End If
        UserLoginName = Result
        Return True
    End Function

    Function HttpGet(ByVal uri As String) As String
        Dim respBody As StringBuilder = New StringBuilder()
        Dim RQ As HttpWebRequest = HttpWebRequest.Create(uri)
        RQ.Method = "GET"
        RQ.ContentType = "application/x-www-form-urlencoded;charset=utf-8"

        Dim RP As HttpWebResponse = RQ.GetResponse()

        Dim Buffer As Byte() = New Byte(8192) {}
        Dim streamA As Stream
        streamA = RP.GetResponseStream()
        Dim count As Integer = 0
        Do
            count = streamA.Read(Buffer, 0, Buffer.Length)
            If count <> 0 Then
                respBody.Append(Encoding.UTF8.GetString(Buffer, 0, count))
            End If
        Loop While count > 0

        Dim responseText As String = respBody.ToString()
        Return responseText
    End Function
End Class
