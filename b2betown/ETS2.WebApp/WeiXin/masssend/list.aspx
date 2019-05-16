<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.masssend.list" %>

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

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getqunfalist", { comid: comid, userid: userid, pageindex: pageindex, pagesize: pagesize }, function (data) {
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/masssend/send.aspx" onfocus="this.blur()"><span>新建群发消息</span></a></li>
                <li class="on"><a href="/weixin/masssend/list.aspx" onfocus="this.blur()">已发送</a></li>
                <li><a href="/weixin/masssend/news/news_list.aspx" onfocus="this.blur()">图文信息管理</a></li>
            </ul>
        </div>--%>
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
                                发送对象
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                发送内容
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                发送结果
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
                                ${Sendobj}
                            </p>
                        </td>
                          <td width="50">
                            <p align="left">
                                ${Content}
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                ${SendResult}
                            </p>
                        </td>
                       
                        <td width="50">
                            <p align="left">
                                ${Sender}
                            </p>
                        </td>
                      <td width="40">
                            <p align="left">
                                ${SendTime}
                            </p>
                        </td>
                        
                    </tr>
    </script>
</asp:Content>
