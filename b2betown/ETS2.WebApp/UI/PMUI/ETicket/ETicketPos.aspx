<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"
    CodeBehind="ETicketPos.aspx.cs" Inherits="ETS2.WebApp.UI.UserUI.bangdingpos" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
<script type="text/javascript">
    var pageSize = 10; //每页显示条数

    $(function () {
        var userid = $("#hid_userid").trimVal();
        var comid = $("#hid_comid").trimVal();
        SearchList(1);

        //装载产品列表
        function SearchList(pageindex) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/AccountInfo.ashx?oper=poslist",
                data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询产品列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("查询数据为空");
                        } else {
                            $("#PosItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex);
                        }


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

                    SearchList(page);

                    return false;
                }
            });
        }
    })
    </script>
  
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
            <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()" target=""><span>
                    电脑验码</span></a></li>
             <%--   <li class="on"><a href="/ui/pmui/eticket/ETicketPos.aspx" onfocus="this.blur()" target="">
                                    Pos验证</a></li>--%>
                <li ><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()"
                    target=""><span>验码明细</span></a></li>
             <%--   <li><a href="dayjiesuan.aspx" onfocus="this.blur()" target=""><span>每日结算</span></a></li>--%>

                
            </ul>
        </div>
       
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <a href="ETicketPos.aspx" onfocus="this.blur()" target="">Pos终端绑定</a></h3>
                <div>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="51" height="31" >
                            ID                        </td>
                        <td width="154" >
                            POS编号                        </td>
                        <td width="111" >
                            绑定时间                        </td>
                        <td width="87" >
                            操作员                        </td>
                        <td width="248" >
                            备注                        </td>
                        <td width="103" >
                                                    </td>
                    </tr>
                   <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <br />
            </div>
        </div>

    </div>
  

    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="PosItemEdit">   
                    <tr>
                        <td width="51">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="154">
                            <p align="left">
                              ${Posid}  
                            </p>
                        </td>
                        <td width="111">
                            <p align="left">
                               ${ChangeDateFormat(BindingTime)} </p>
                        </td>
                        <td width="87">
                            <p align="left">
                               ${Admin} </p>
                        </td>
                        <td width="248">
                            <p align="left">
                               ${Remark} </p>
                        </td>
                        <td width="103">
                            <p align="left">
                              </p>
                        </td>

                    </tr>
    </script>
</asp:Content>
