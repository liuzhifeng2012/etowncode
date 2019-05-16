<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="false" CodeBehind="Web.aspx.cs"
    Inherits="ETS2.WebApp.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getrandomtimestr", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#<%=Randomid.ClientID %>").val(data.msg);
                }
            })
        })
    </script>
</head>
<body>


    <form id="form1" method="post" action="/TicketService/httpservice.ashx">
    <textarea id="xml" name="xml" rows="20" cols="50"><%=xml1%>
      </textarea>
      <input type="text" name="organization" value="1000001038" />
      <br />
    <input type="submit" id="sub" value="发送请求" />
    </form>
    <hr />
    <form id="form2" runat="server">
    <div>
          PosID:<asp:TextBox ID="Posid" runat="server" Text="1000001038" Width="240px"></asp:TextBox><br />
        DesKey:<asp:TextBox ID="Deskey" runat="server" Text="3R4FTF1E" Width="240px"></asp:TextBox><br />
        <br />
        <asp:Button ID="Button2" runat="server" Text="生成查询格式字符串" OnClick="Button2_Click" />
        <br />
        <div id="divmsg1" runat="server">
        </div>
        <br />
        <asp:Button ID="Button3" runat="server" Text="生成验票格式字符串" OnClick="Button3_Click" />
        <br />
        <div id="divmsg2" runat="server">
        </div>
        <br />
        <asp:Button ID="Button6" runat="server" Text="生成查询服务格式字符串" OnClick="Button6_Click" />
        <br />
        <div id="div1" runat="server">
        </div>
        <br />
        <asp:Button ID="Button7" runat="server" Text="生成验证服务格式字符串" OnClick="Button7_Click" />
        <br />
        <div id="div2" runat="server">
        </div>
        <br />
        <asp:Button ID="Button4" runat="server" Text="生成冲正格式字符串" OnClick="Button4_Click" />
        <br />
        <div id="divmsg3" runat="server">
        </div>
        <br />
        <asp:Button ID="Button5" runat="server" Text="生成淘宝冲正格式字符串" OnClick="Button5_Click" />
        <br />
        <div id="divmsg4" runat="server">
        </div>
        <br />
        <asp:Button ID="Button8" runat="server" Text="补卡查询接口" OnClick="Button8_Click" />
        <br />
        <div id="div3" runat="server">
        </div>
        <br />
        <asp:Button ID="Button9" runat="server" Text="补卡确认接口" OnClick="Button9_Click" />
        <br />
        <div id="div4" runat="server">
        </div>
        <br />
        <asp:Button ID="Button10" runat="server" Text="实体卡芯片id上传接口" OnClick="Button10_Click" />
        <br />
        <div id="div5" runat="server">
        </div>
        <asp:Button ID="Button11" runat="server" Text="闸机刷卡" OnClick="Button11_Click" />
        <br />
        <div id="div6" runat="server">
        </div>
        <br />
        随机编号:<asp:TextBox ID="Randomid" runat="server" Width="240px" Text=""></asp:TextBox> 
        <asp:Button ID="Button1" runat="server" Text="MD5加密" OnClick="Button1_Click" />=>
        md5结果:<asp:TextBox ID="Result" runat="server" Width="340px"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" Text="Label" Style="font-size: 14px; color: Red;"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label" Style="font-size: 12px;">
            //以下是根据电子码和 验证数量 得到淘宝冲正所需字段的sql语句<br />
            //select top 1 order_id from taobao_send_noticeretlog where verify_codes like '%电子码%'
            order by id desc<br />
            //select top 1 serial_num from taobao_consume_retlog where verify_code='电子码' and
            consume_num=验证数量 order by id desc<br />
            //select top 1 token from taobao_send_noticelog where order_id=(select top 1 order_id
            from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc)
            order by id desc </asp:Label>
    </div>
    <div>
    </div>
    </form>--%>

<%--
    <form id="form1" method="post" action="/jsonfactory/TwoCode.ashx">
    <textarea id="oper" name="oper" rows="20" cols="50">
      </textarea><br />
    <input type="submit" id="sub" value="发送请求" />
    </form>
    <hr />
    <form id="form2" runat="server">
    <div>
        PosID:<asp:TextBox ID="Posid" runat="server" Text="999999999" Width="240px"></asp:TextBox><br />
        DesKey:<asp:TextBox ID="Deskey" runat="server" Text="4Mds1hSvWkfTmNrWMv1KTIPj" Width="240px"></asp:TextBox><br />
        <br />
        <asp:Button ID="Button2" runat="server" Text="生成查询格式字符串" OnClick="Button2_Click" />
        <br />
        <div id="divmsg1" runat="server">
        </div>
        <br />
        <asp:Button ID="Button3" runat="server" Text="生成验票格式字符串" OnClick="Button3_Click" />
        <br />
        <div id="divmsg2" runat="server">
        </div>
        <br />
        <asp:Button ID="Button6" runat="server" Text="生成查询服务格式字符串" OnClick="Button6_Click" />
        <br />
        <div id="div1" runat="server">
        </div>
        <br />
        <asp:Button ID="Button7" runat="server" Text="生成验证服务格式字符串" OnClick="Button7_Click" />
        <br />
        <div id="div2" runat="server">
        </div>
        <br />
        <asp:Button ID="Button4" runat="server" Text="生成冲正格式字符串" OnClick="Button4_Click" />
        <br />
        <div id="divmsg3" runat="server">
        </div>
        <br />
        <asp:Button ID="Button5" runat="server" Text="生成淘宝冲正格式字符串" OnClick="Button5_Click" />
        <br />
        <div id="divmsg4" runat="server">
        </div>
        <br />
        <asp:Button ID="Button8" runat="server" Text="补卡查询接口" OnClick="Button8_Click" />
        <br />
        <div id="div3" runat="server">
        </div>
        <br />
        <asp:Button ID="Button9" runat="server" Text="补卡确认接口" OnClick="Button9_Click" />
        <br />
        <div id="div4" runat="server">
        </div>
        <br />
        <asp:Button ID="Button10" runat="server" Text="实体卡芯片id上传接口" OnClick="Button10_Click" />
        <br />
        <div id="div5" runat="server">
        </div>
        <asp:Button ID="Button11" runat="server" Text="闸机刷卡" OnClick="Button11_Click" />
        <br />
        <div id="div6" runat="server">
        </div>
        <br />
        随机编号:<asp:TextBox ID="Randomid" runat="server" Width="240px" Text=""></asp:TextBox> 
        <asp:Button ID="Button1" runat="server" Text="MD5加密" OnClick="Button1_Click" />=>
        md5结果:<asp:TextBox ID="Result" runat="server" Width="340px"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" Text="Label" Style="font-size: 14px; color: Red;"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label" Style="font-size: 12px;">
            //以下是根据电子码和 验证数量 得到淘宝冲正所需字段的sql语句<br />
            //select top 1 order_id from taobao_send_noticeretlog where verify_codes like '%电子码%'
            order by id desc<br />
            //select top 1 serial_num from taobao_consume_retlog where verify_code='电子码' and
            consume_num=验证数量 order by id desc<br />
            //select top 1 token from taobao_send_noticelog where order_id=(select top 1 order_id
            from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc)
            order by id desc </asp:Label>
    </div>
    <div>
    </div>
    </form>--%>
</body>
</html>
