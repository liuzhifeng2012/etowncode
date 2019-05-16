<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sendlog.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.invitecode.sendlog" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            SearchList(1, 10);
        })
        function SearchList(pageindex, pagesize) {
            var comid = $("#hid_comid").val();
            var userid = $("#hid_userid").val();

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getinvitecodesendlog", { comid: comid, userid: userid, pageindex: pageindex, pagesize: pagesize }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalCount == 0) {

                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalcount, pagesize, pageindex);
                    }
                }
            })
        }
        //分页
        function setpage(newcount, newpagesize, curpage) {
            $("#divPage").paginate({
                count: Math.ceil(newcount / newpagesize),
                start: curpage,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li><a href="/weixin/invitecode/send.aspx" onfocus="this.blur()">发送邀请码</a></li>
                <li class="on"><a href="/weixin/invitecode/sendlog.aspx" onfocus="this.blur()">邀请码发送记录</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <%--<td width="51">
                            <p align="left">
                                编号</p>
                        </td>--%>
                        <td width="50">
                            <p align="left">
                                电话
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                发送内容
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                验证码
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                发送人
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                发送时间
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                发送状态
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                发送类型
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                备注
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <!--<td width="51">
                             <p>${Id}</p>
                        </td>-->
                         <td width="50">
                            <p align="left">
                                ${Phone}
                            </p>
                        </td>
                          <td width="50">
                            <p align="left">
                                ${Smscontent}
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                ${Invitecode}
                            </p>
                        </td>
                       
                        <td width="50">
                            <p align="left">
                                ${Senduserid}
                            </p>
                        </td>
                      <td width="40">
                            <p align="left">
                                ${Sendtime}
                            </p>
                        </td>
                         <td width="40">
                            <p align="left">
                                ${Issendsuc}
                            </p>
                        </td>
                         <td width="40">
                            <p align="left">
                                ${Isqunfa}
                            </p>
                        </td>
                         <td width="40">
                            <p align="left">
                                ${Remark}
                            </p>
                        </td>
                        
                    </tr>
    </script>
</asp:Content>
