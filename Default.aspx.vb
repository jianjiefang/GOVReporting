'*****************************************************
'程序文件名：Default.aspx.vb
'作者：Leon Huang
'创建时间：20081208
'隶属项目名称：Government PC System
'隶属模块名称：Certificate Module
'程序功能说明：车辆合格证打印系统网上查询
'程序功能描述：实现车辆合格证数据通过网站查询的方式来解决。通过生成日期、合格证编号、车辆识别代号、车辆型号等做为查询条件来完成查询。
'版权所有：FJDA
'HISTORY 
' 
'20081208 针对 《变更申请单》（批次编号：2008120501)所做的功能增加
'20081215 针对 《变更申请单》（批次编号：2008120501)所做的功能增加 增加域用户登陆
'******************************************************
Imports Class_Public
Imports System.Data
Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.DirectoryServices.Protocols
Imports EN_DE.STRING


Partial Class _Default
    Inherits System.Web.UI.Page

    Dim Flag_M, Flag_O As Boolean
    Public Shared List_Counter As Integer   '---全程变量：记录总数

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '****************************************************
        '过程或函数名称:Page_Load
        '功能描述:窗体加载事件。加载控件初始状态，加载是否有报表查询功能。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************

        If Session("User_ID") = "" Then
            'Label_UserName.Visible = True
            'TextBox_UserName.Visible = True
            'Label_PWD.Visible = True
            'TextBox_PWD.Visible = True
            'Button_Login.Visible = True
            'Label_UserNameCHN.Visible = False
            'Label_Title.Visible = False
            'TextBox_UserName.Focus()
            'Button_Query.Enabled = False
            'Button_Export.Enabled = False
            'Button_Logout.Visible = False
            '    Dim URL As String = ConfigurationManager.AppSettings("HomeSite")

            '    Response.Write(javaMsgBox("Time out."))
            '    Response.Write(javaNavigation(URL))
            'Else
            Label_UserName.Visible = False
            TextBox_UserName.Visible = False
            Label_PWD.Visible = False
            TextBox_PWD.Visible = False
            Button_Login.Visible = False
            Label_UserNameCHN.Visible = True
            Label_Title.Visible = True
            Label_UserNameCHN.Text = Session("UserName_C") & ",您好！"
            Button_Logout.Visible = True

            Dim cmd As New SqlCommand()
            Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("FJDA_DevelopmentConnectionString").ToString)
            Conn.Open()
            cmd.Connection = Conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "P_GOV_Authority_Select_Designation"
            cmd.Parameters.Add(New SqlParameter("@UserID", Session("User_ID")))
            cmd.Parameters.Add(New SqlParameter("@Flag_M", SqlDbType.Bit)).Direction = ParameterDirection.Output
            cmd.Parameters.Add(New SqlParameter("@Flag_O", SqlDbType.Bit)).Direction = ParameterDirection.Output
            cmd.ExecuteNonQuery()
            Flag_M = cmd.Parameters("@Flag_M").Value
            Flag_O = cmd.Parameters("@Flag_O").Value

            Conn.Close()
            If Flag_M = True Then
                Button_Query.Enabled = True
                Button_Export.Enabled = True
            Else
                Button_Query.Enabled = False
                Button_Export.Enabled = False
            End If

            '-----*****加密用户信息*****-----
            'Dim edDLL As New StringENDE
            'Dim getDate As String = Format(Now, "yyyyMMdd") & Format(Now, "HHmmss")
            'Dim myCookie As HttpCookie = New HttpCookie("UserSettings")
            'Dim keyWord As String = ConfigurationManager.AppSettings("Key_Value")
            'Dim KY_String As String = edDLL.Encrypt_Key(getDate, keyWord)
            'myCookie("UserName") = edDLL.Encrypt_Key(Session("User_Name"), Right(getDate, 8))
            'myCookie("Password") = edDLL.Encrypt_Key(Session("User_PWD"), Right(getDate, 8))
            'myCookie("Exdate") = KY_String
            'myCookie("URL") = Request.Url.Host & ":" & Request.Url.Port & Request.ApplicationPath

            'myCookie("Language") = Session("Language")

            'myCookie.Expires = Now.AddDays(1)
            'myCookie.Domain = ".fjbenz.com"
            'Response.Cookies.Add(myCookie)

            'FormsAuthentication.SetAuthCookie(edDLL.Encrypt_Key(Session("User_Name"), Right(getDate, 8)), False)
        End If
    End Sub

    Protected Sub Button_Login_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Login.Click
        '****************************************************
        '过程或函数名称:Button_Login_Click
        '功能描述:Button_Login单击事件。判断登陆及UserID加载。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        'Dim cmd As New SqlCommand()
        'Dim Conn As New SqlConnection(ConfigurationManager.ConnectionStrings("FJDA_DevelopmentConnectionString").ToString)
        'Conn.Open()
        'cmd.Connection = Conn
        'cmd.CommandType = CommandType.StoredProcedure
        'cmd.CommandText = "P_CheckLogin"

        '' Create a SqlParameter for each parameter in the stored procedure.
        'Dim customerIDParam_1 As New SqlParameter("@I_Username", TextBox_UserName.Text)
        'Dim customerIDParam_2 As New SqlParameter("@I_Password", TextBox_PWD.Text)
        'cmd.Parameters.Add(customerIDParam_1)
        'cmd.Parameters.Add(customerIDParam_2)
        'Dim Result As SqlDataReader = cmd.ExecuteReader()
        'If Result.Read() Then
        '    '=====成功验证用户与密码后获取相关的信息Begin=====
        '    Session("User_ID") = Result("ID")
        '    Session("UserName_C") = Result("User_name")
        '    Conn.Close()
        '    Response.Redirect("Default.aspx")
        'Else
        '    Response.Write(javaMsgBox("用户名与密码不符，请重新输入！"))
        'End If
        Dim DE As DirectoryEntry

        If Not IsUserValid(TextBox_UserName.Text.Trim, TextBox_PWD.Text.Trim) Then
            Response.Write(javaMsgBox("用户名与密码不符，请重新输入！"))
            TextBox_PWD.Text = ""
            TextBox_PWD.Focus()
            Exit Sub
        Else
            Try
                DE = GetUser(TextBox_UserName.Text.Trim, TextBox_PWD.Text.Trim)
                Session("User_ID") = DE.Guid.ToString
                Session("UserName_C") = DE.Properties("DisplayName").Value
                DE.Close()
                Response.Redirect("Default.aspx")
            Catch ex As Exception
                Response.Write(javaMsgBox("系统异常！原因如下" & Chr(13) & Chr(10) & ex.Message))
            Finally
                DE.Close()
            End Try
        End If
    End Sub

    Public Function javaMsgBox(ByVal Str As String) As String
        '****************************************************
        '过程或函数名称:javaMsgBox
        '功能描述:自定义函数，确认框加载,用Script的形式返回对话框信息
        '输入参数：ByVal Str As String
        '返回参数：javaMsgBox As String
        '****************************************************
        javaMsgBox = "<Script> alert('" & chkString(Str) & "');</Script>"
    End Function

    Public Shared Function chkString(ByVal str As String) As String
        '****************************************************
        '过程或函数名称:chkString
        '功能描述:自定义函数，将'号转换成双引号。
        '输入参数：ByVal str As String
        '返回参数：chkString As String
        '****************************************************
        chkString = Replace(str, "'", """")
    End Function

    Protected Sub SqlDataSource_Query_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource_Query.Selected
        '****************************************************
        '过程或函数名称:SqlDataSource_Query_Selected
        '功能描述:SqlDataSource_Query选择后事件。取查询后的记录数。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        List_Counter = e.AffectedRows
    End Sub

    Protected Sub SqlDataSource_Query_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource_Query.Selecting
        '****************************************************
        '过程或函数名称:SqlDataSource_Query_Selecting
        '功能描述:SqlDataSource_Query选择时事件。验证文本框中数据的正确性。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        If TextBox_HGZBH.Text.Trim = "" Then
            e.Command.Parameters("@WZHGZBH").Value = ""
        End If
        If TextBox_CLSBDH.Text.Trim = "" Then
            e.Command.Parameters("@CLSBDH").Value = ""
        End If
        If TextBox_CLXH.Text.Trim = "" Then
            e.Command.Parameters("@CLXH").Value = ""
        End If
    End Sub

    Protected Sub GridView_List_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView_List.PageIndexChanging
        '****************************************************
        '过程或函数名称:GridView_List_PageIndexChanging
        '功能描述:GridView_List页数改变时事件。主要实现指定跳转功能。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        Dim txtNewPageIndex As New TextBox
        Dim newpageindex As Integer = 0
        If e.NewPageIndex = -2 Then
            Dim pagerRow As GridViewRow = GridView_List.BottomPagerRow
            If Not IsDBNull(pagerRow) Then
                VarType(vbInteger)
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex")
            End If
            If Not IsDBNull(txtNewPageIndex) Then
                If IsNumeric(txtNewPageIndex.Text) Then

                    newpageindex = CInt(txtNewPageIndex.Text) - 1
                Else
                    MsgBox("请输入数字！")
                    Exit Sub
                End If

            End If
        Else
            newpageindex = e.NewPageIndex
        End If
        If newpageindex < 0 Then
            newpageindex = 0
        End If
        If newpageindex >= GridView_List.PageCount Then
            newpageindex = GridView_List.PageCount - 1
        End If
        GridView_List.PageIndex = newpageindex
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal Control As Control)
        '=======生成Excel时所必需的===========
    End Sub


    Protected Sub Button_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Export.Click
        '****************************************************
        '过程或函数名称:Button_Export_Click
        '功能描述:Button_Export单击事件。实现导成EXCEL功能。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        GridView_List.AllowPaging = False
        GridView_List.AutoGenerateEditButton = False
        GridView_List.DataBind()
        HttpContext.Current.Response.Charset = "UTF8"
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312")
        HttpContext.Current.Response.ContentType = "application/ms-excel"
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Export_Report.XLS")
        GridView_List.Page.EnableViewState = False
        Dim tw As New IO.StringWriter
        Dim hw As New HtmlTextWriter(tw)
        GridView_List.RenderControl(hw)
        HttpContext.Current.Response.Write(Trim(tw.ToString()))
        HttpContext.Current.Response.End()
    End Sub

    Protected Sub Button_Query_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Query.Click
        '****************************************************
        '过程或函数名称:Button_Query_Click
        '功能描述:Button_Query单击事件。实现查询条件的判断。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        If IsDate(TextBox_StartDate.Text) And IsDate(TextBox_EndDate.Text) Then
            If CDate(TextBox_StartDate.Text) > CDate(TextBox_EndDate.Text) Then
                Response.Write(javaMsgBox("开始日期必须小于结束日期！"))
                TextBox_EndDate.Text = ""
            End If
            If DateDiff(DateInterval.Day, CDate(TextBox_StartDate.Text), CDate(TextBox_EndDate.Text)) > 30 Then
                Response.Write(javaMsgBox("开始日期与结束日期的跨度要小于31天！"))
                TextBox_EndDate.Text = ""
            End If
        Else
            Response.Write(javaMsgBox("不是合法的日期格式！"))
            TextBox_StartDate.Text = ""
            TextBox_EndDate.Text = ""
        End If
    End Sub

    Protected Sub Button_Logout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Logout.Click
        '****************************************************
        '过程或函数名称:Button_Logout_Click
        '功能描述:Button_Logout单击事件。实现登出功能。
        '输入参数：ByVal sender As System.Object, ByVal e As System.EventArgs
        '返回参数：无
        '****************************************************
        If Not Session("URL") = "" Then
            '-----*****链接地址加密*****-----
            Response.Redirect("Http://" & Session("URL") & "/CHK_Login.aspx")
        Else
            Session.Clear()
            Dim URL As String = ConfigurationManager.AppSettings("HomeSite")
            Response.Write(javaMsgBox("Time out."))
            Response.Write(javaNavigation(URL))
        End If
    End Sub

    Public Shared Function IsUserValid(ByVal UserName As String, ByVal PassWord As String) As Boolean
        '****************************************************
        '过程或函数名称:IsUserValid
        '功能描述：经过域认证
        '输入参数：ByVal UserName As String, ByVal PassWord As String
        '返回参数：IsUserValid As Boolean
        '****************************************************
        Dim deUser As DirectoryEntry
        deUser = New DirectoryEntry("LDAP://" & ConfigurationManager.AppSettings("Domain"), UserName, PassWord, AuthenticationTypes.Secure)
        Try
            Dim native As Object = deUser.NativeObject
            Return True
        Catch ex As Exception
            Return False
        Finally
            deUser.Close()
        End Try
    End Function

    Public Shared Function GetDirectoryObject(ByVal UserName As String, ByVal PassWord As String) As DirectoryEntry
        Dim oDe As DirectoryEntry
        oDe = New DirectoryEntry("LDAP://" & ConfigurationManager.AppSettings("Domain"), UserName, PassWord, AuthenticationTypes.Secure)
        Return oDe
    End Function

    Public Shared Function GetUser(ByVal UserName As String, ByVal PassWord As String) As DirectoryEntry
        Dim de As DirectoryEntry = GetDirectoryObject(UserName, PassWord)
        Dim deSearch As New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + UserName + "))"
        deSearch.SearchScope = System.DirectoryServices.Protocols.SearchScope.Subtree
        Dim results As SearchResult = deSearch.FindOne
        If (results IsNot DBNull.Value) Then
            de = New DirectoryEntry(results.Path, UserName, PassWord, AuthenticationTypes.Secure)
            Return de
        Else
            Return Nothing
        End If
    End Function

    Protected Sub GridView_List_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView_List.RowCreated
        Dim iCounter As Integer
        For iCounter = 0 To e.Row.Cells.Count - 1
            e.Row.Cells(iCounter).Attributes.Add("style", "vnd.ms-excel.numberformat:@")
        Next
    End Sub

    Protected Sub TextBox_UserName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_UserName.TextChanged

    End Sub
End Class
