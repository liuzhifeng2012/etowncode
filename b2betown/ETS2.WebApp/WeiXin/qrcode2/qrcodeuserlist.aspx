<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qrcodeuserlist.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.WeiXin.qrcode2.qrcodeuserlist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 16; //每页显示条数

        $(function () {

            var comid = $("#hid_comid").trimVal();
            var subscribesourceid = $("#hid_subscribesourceid").trimVal();


            //查询微信关注用户列表
            SearchList(1, subscribesourceid);

            //装载产品列表
            function SearchList(pageindex, subscribesourceid) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=GetWxSubscribeList",
                    data: { comid: comid, subscribesourceid: subscribesourceid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {

                            $("#divtotalCount").html("共" + data.totalCount + "条记录");
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td colspan='8'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, subscribesourceid);
                            }
                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, subscribesourceid) {
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

                        SearchList(page, subscribesourceid);

                        return false;
                    }
                });
            }

            $("input:radio[name='radsourcetype']").live("click", function () {
                var val = $("input:radio[name='radsourcetype']:checked").trimVal();

                window.open("qrcodeuserlist.aspx?wxsourcetype=" + val, target = "_self");
            })

        })
        
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
               
                <li><a href="qrcodelist.aspx?isqudao=1" onfocus="this.blur()">渠道二维码列表</a></li>
                <li><a href="qrcodelist.aspx?isqudao=0" onfocus="this.blur()">活动二维码列表</a></li>
                <li><a href="qrcodemanager.aspx" onfocus="this.blur()">二维码添加</a></li>
                 <li class="on"><a href="qrcodeuserlist.aspx" onfocus="this.blur()"><span>扫码实时记录</span></a></li>
            </ul>
        </div> 
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    扫码记录</h3>
                <%-- <h3 id="tdgroups0">
                    微信用户来源类型：<label style="font-family: Tahoma; outline: none;">
                        <input name="radsourcetype" type="radio" value="0" />全 部
                        <input name="radsourcetype" type="radio" value="1" />活动推广
                        <input name="radsourcetype" type="radio" value="2" />门市推广
                        <input name="radsourcetype" type="radio" value="3" />搜索公众号
                    </label>
                </h3>--%>
                <p>
                    &nbsp;</p>
                <table width="780" border="0">
                    <tr>
                        <td style="width: 50px;">
                            <p align="left">
                                id</p>
                        </td>
                        <td style="width: 80px;">
                            <p align="left">
                                扫码时间
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                渠道
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                渠道人
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                活动
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                线路
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                用户微信名</p>
                        </td>
                        <td style="width: 50px;">
                            <p align="left">
                                性别</p>
                        </td>
                        <td style="width: 50px;">
                            <p align="left">
                                城市</p>
                        </td>
                        <td>
                            <p align="left">
                                绑定手机
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divtotalCount">
                </div>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                  <tr>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                              ${ChangeDateFormat(SubscribeTime)}
                                 
                            </p>
                        </td>
                        <td>
                            <p align="left">
                              ${ChannelCompanyName}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                              ${ChannelName}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                               ${ActivityName}
                            </p>
                        </td>
                           <td>
                            <p align="left">
                               ${ProductName}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                                ${Nickname}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               ${Sex}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                 ${City}
                            </p>
                        </td>            
                        <td>
                            <p align="left">
                                ${BindPhone}
                            </p>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_subscribesourceid" value="<%=subscribesourceid %>" />
</asp:Content>
