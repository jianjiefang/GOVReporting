<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>合格证打印系统查询模块（WEB 版）</title>

        <script type="text/javascript" language="javascript">
            function closeWin() {
                  window.opener=null;
                  window.open('','_self');
                  window.close();
                }
         </script>
</head>
<body>
    <form id="thisForm" runat="server">
        <asp:ScriptManager ID="ScriptManager_Main" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
    <div style="text-align: left">
        <asp:Label ID="Label_UserName" runat="server" Font-Bold="True" Font-Size="Large"
            Text="用户名："></asp:Label>
        <asp:TextBox ID="TextBox_UserName" runat="server"></asp:TextBox>
        <asp:Label ID="Label_PWD" runat="server" Font-Bold="True" Font-Size="Large" Text="密码："></asp:Label>
        <asp:TextBox ID="TextBox_PWD" runat="server" TextMode="Password"></asp:TextBox>
        <asp:Button ID="Button_Login" runat="server" Text="登陆" OnClientClick="return chk_TextBox();" />
        <!--<asp:Label ID="Label_UserNameCHN" runat="server" Font-Bold="True" Font-Size="Large"
            Text="Label"></asp:Label>-->
        <asp:Label ID="Label_Title" runat="server" Font-Bold="True" Font-Size="Large" Text="欢迎您使用本系统！"></asp:Label>
		<!--<asp:Button ID="Button_Logout" runat="server" Text="退出" />-->
        <input type="button" value="退出" runat="server" OnClick ="closeWin()"/><br />
        <br />
        <div style="text-align: center"><strong><span style="font-size: 24pt">车辆合格证打印系统查询模块（WEB 版）<br />
            </span>
        </strong></div>
        <br />
        选择时间段：<asp:TextBox ID="TextBox_StartDate" runat="server" Text="" Width="90px"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender_TextBox_StartDate" runat="server" Enabled="True" TargetControlID="TextBox_StartDate"></ajaxToolkit:CalendarExtender> -- <asp:TextBox ID="TextBox_EndDate" runat="server" Text="" Width="90px"></asp:TextBox><ajaxToolkit:CalendarExtender ID="CalendarExtender_TextBox_EndDate" runat="server" Enabled="True" TargetControlID="TextBox_EndDate"></ajaxToolkit:CalendarExtender>
        合格证编号：<asp:TextBox ID="TextBox_HGZBH" runat="server" Text="" MaxLength="50"></asp:TextBox> 车辆识别代号：<asp:TextBox ID="TextBox_CLSBDH" runat="server" Text="" MaxLength="50"></asp:TextBox> 车辆型号：<asp:TextBox ID="TextBox_CLXH" runat="server" Text="" MaxLength="50"></asp:TextBox>
        <br />
        <asp:Button ID="Button_Query" runat="server" Text="开始查询" />
        <asp:Button ID="Button_Export" runat="server" Text="导出Excel" /><br />
        <br />
        <asp:GridView ID="GridView_List" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            DataSourceID="SqlDataSource_Query" AllowPaging="True" PageSize="50" 
            Width="300%">
            <PagerTemplate>
                      <div style="text-align: left">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Image/pe.png" 
                                CausesValidation="False" CommandArgument="First" CommandName="Page"/>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Image/pb.png" 
                                CausesValidation="False" CommandArgument="Prev" CommandName="Page"/>
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Image/pn.png" 
                                CausesValidation="False" CommandArgument="Next" CommandName="Page"/>
                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Image/pf.png" 
                                CausesValidation="False" CommandArgument="Last" CommandName="Page"/>
                                总记录数:<asp:Label ID="Label0" runat="server" Font-Bold="True" ForeColor="#0066FF" Text='<%# List_Counter %>'></asp:Label>
                        当前页:<asp:Label ID="Label1" runat="server" 
                                Text='<%# GridView_List.PageIndex+1 %>' Font-Bold="True" 
                                ForeColor="#0066FF"></asp:Label>
                        /总页数:<asp:Label 
                                ID="Label3" runat="server" Text="<%# GridView_List.PageCount %>" 
                                Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                        跳转到：<asp:textbox ID="txtNewPageIndex" runat="server" Width="20px" 
                                Text='<%# GridView_List.PageIndex + 1 %>' />
                        <asp:linkbutton ID="btnGo" runat="server" CausesValidation="False" 
                                CommandArgument="-1" CommandName="Page" Text="GO" />
                      </div>
                    </PagerTemplate>
            <Columns>
                <asp:BoundField DataField="Exhaust" HeaderText="排放系统" SortExpression="Exhaust">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="WZHGZBH" HeaderText="合格证编号" SortExpression="WZHGZBH" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FZRQ" HeaderText="发证日期" SortExpression="FZRQ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLZZQYMC" HeaderText="车辆制造企业名称" SortExpression="CLZZQYMC" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLMC" HeaderText="车辆名称" SortExpression="CLMC" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLPP" HeaderText="车辆品牌" SortExpression="CLPP" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLXH" HeaderText="车辆型号" SortExpression="CLXH" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CSYS" HeaderText="车身颜色" SortExpression="CSYS" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLSBDH" HeaderText="车辆识别代号" SortExpression="CLSBDH" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FDJH" HeaderText="发动机号" SortExpression="FDJH" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FDJXH" HeaderText="发动机型号" SortExpression="FDJXH" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="RLZL" HeaderText="燃料种类" SortExpression="RLZL" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="PFBZ" HeaderText="排放标准" SortExpression="PFBZ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="PL" HeaderText="排量" SortExpression="PL" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="GL" HeaderText="功率" SortExpression="GL" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZXXS" HeaderText="转向形式" SortExpression="ZXXS" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="QLJ" HeaderText="前轮距" SortExpression="QLJ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="HLJ" HeaderText="后轮距" SortExpression="HLJ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="LTS" HeaderText="轮胎数" SortExpression="LTS" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="LTGG" HeaderText="轮胎规格" SortExpression="LTGG" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZJ" HeaderText="轴距" SortExpression="ZJ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZH" HeaderText="轴荷" SortExpression="ZH" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZS" HeaderText="轴数" SortExpression="ZS" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="WKC" HeaderText="外廓长" SortExpression="WKC" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="WKK" HeaderText="外廓宽" SortExpression="WKK" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="WKG" HeaderText="外廓高" SortExpression="WKG" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZZL" HeaderText="总质量" SortExpression="ZZL" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZBZL" HeaderText="整备质量" SortExpression="ZBZL" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="EDZK" HeaderText="额定载客" SortExpression="EDZK" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZGCS" HeaderText="最高车速" SortExpression="ZGCS" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CLZZRQ" HeaderText="车辆制造日期" SortExpression="CLZZRQ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="BZ" HeaderText="备注" SortExpression="BZ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="QYBZ" HeaderText="企业标准" SortExpression="QYBZ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CPSCDZ" HeaderText="车辆生产单位名称" SortExpression="CPSCDZ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="QYQTXX" HeaderText="企业其它信息" SortExpression="QYQTXX" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="clscdwmc" HeaderText="车辆生产单位名称" SortExpression="clscdwmc" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="yh" HeaderText="油耗" SortExpression="yh" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CZRQ" HeaderText="操作日期" SortExpression="CZRQ" >
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <RowStyle Wrap="True" />
        </asp:GridView>
        </div>
    </form>
    <script language="javascript" type="text/javascript" >
            function chk_TextBox()
            {
                var theForm = document.forms['thisForm']
                if (theForm.TextBox_UserName.value=='' || theForm.TextBox_PWD.value=='')
                {
                    alert('请输入用户名与密码！')
                    return false;
                }
                else
                {
                    return true;
                }
            }
        </script>

    <asp:SqlDataSource ID="SqlDataSource_Query" runat="server" ConnectionString="<%$ ConnectionStrings:FJDA_DevelopmentConnectionString %>"
        SelectCommand="P_GOV_MainTable_Select_FROMWEB" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox_StartDate" Name="Start_Date" PropertyName="Text"
                Type="DateTime" />
            <asp:ControlParameter ControlID="TextBox_EndDate" Name="End_Date" PropertyName="Text"
                Type="DateTime" DefaultValue="" />
            <asp:ControlParameter ControlID="TextBox_HGZBH" Name="WZHGZBH" PropertyName="Text"
                Type="String"/>
            <asp:ControlParameter ControlID="TextBox_CLSBDH" Name="CLSBDH" PropertyName="Text"
                Type="String" DefaultValue=""/>
            <asp:ControlParameter ControlID="TextBox_CLXH" Name="CLXH" PropertyName="Text" Type="String" />
            
        </SelectParameters>
    </asp:SqlDataSource>
</body>
</html>
