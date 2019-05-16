<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ChannelList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 16; //每页显示条数
        var userid = $("#hid_userid").trimVal();
        var comid = $("#hid_comid").trimVal();
        $(function () {
            //根据渠道单位id得到渠道列表
            var channelcompanyid = $("#statistics").val();

            SearchList2(1, channelcompanyid);

        })

        //装载产品列表
        function SearchList2(pageindex, channelcompanyid) {
            $.ajax({
                type: "post",
                url: "/JsonFactory/ChannelHandler.ashx?oper=SearchChannelByChannelUnit",
                data: { comid: $("#hid_comid").val(), pageindex: pageindex, pagesize: pageSize, channelcompanyid: channelcompanyid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage2(data.totalCount, pageSize, pageindex, channelcompanyid);
                        }
                    }
                }
            })
        }
        //分页
        function setpage2(newcount, newpagesize, curpage, channelcompanyid) {
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

                    SearchList2(page, channelcompanyid);

                    return false;
                }
            });
        }
        //        //根据公司id得到渠道列表--暂时别删
        //        function SearchList(pageindex) {
        //            if (pageindex == '') {
        //                $.prompt("请选择跳到的页数");
        //                return;
        //            }
        //            $.ajax({
        //                type: "post",
        //                url: "/JsonFactory/ChannelHandler.ashx?oper=channellistbycomid",
        //                data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
        //                async: false,
        //                success: function (data) {
        //                    data = eval("(" + data + ")");

        //                    if (data.type == 1) {
        //                        $.prompt("查询渠道列表错误");
        //                        return;
        //                    }
        //                    if (data.type == 100) {
        //                        $("#tblist").empty();
        //                        $("#divPage").empty();
        //                        if (data.totalCount == 0) {
        //                            $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
        //                        } else {
        //                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
        //                            setpage(data.totalCount, pageSize, pageindex);
        //                        }


        //                    }
        //                }
        //            })
        //        }
        //       
        //        //分页
        //        function setpage(newcount, newpagesize, curpage) {
        //            $("#divPage").paginate({
        //                count: Math.ceil(newcount / newpagesize),
        //                start: curpage,
        //                display: 5,
        //                border: false,
        //                text_color: '#888',
        //                background_color: '#EEE',
        //                text_hover_color: 'black',
        //                images: false,
        //                rotate: false,
        //                mouse: 'press',
        //                onChange: function (page) {

        //                    SearchList(page);

        //                    return false;
        //                }
        //            });
        //        }

      
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li id="addoutchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=out"
                    onfocus="this.blur()">合作单位</a></li>
                <li id="addoutchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=out" onfocus="this.blur()">
                    添加合作单位</a></li>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店</a></li>
                <li id="addinnerchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=inner"
                    onfocus="this.blur()"><span>添加门店</span></a></li>
                <%}
                  else
                  {
                %>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店</a></li>
                <%
                    } %>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <div id="divaddinnerchannel">
                        <%if (channelsource == "0")
                          {
                        %>
                        <a style="text-align: left; color: Blue;" href="ChannelEdit.aspx?channeltype=inner">
                            添加所属门店人员</a><br />
                        <br />
                        <%
                            }
                          else
                          { %>
                        <a style="text-align: left; color: Blue;" href="ChannelEdit.aspx?channeltype=out">添加合作公司人员</a>
                        <%} %>
                    </div>
                </h3>
                <div style="text-align: center;">
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="141">
                            <p align="left">
                                渠道单位名称</p>
                        </td>
                        <td width="58">
                            发行人姓名
                        </td>
                        <td width="76">
                            <p align="left">
                                发行人手机</p>
                        </td>
                        <td width="119">
                            <p align="left">
                                发行人卡号
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                地 点</p>
                        </td>
                        <td width="86">
                            <p align="left">
                                渠道人群</p>
                        </td>
                        <td width="131">
                            返佣办法
                        </td>
                        <td width="52">
                            <p align="left">
                                渠道级别</p>
                        </td>
                        <td width="48">
                            录入量
                        </td>
                        <td width="48">
                            开卡量
                        </td>
                        <td width="88">
                            第一次交易量
                        </td>
                        <td width="48">
                            金额
                        </td>
                        <td width="47">
                            类型
                        </td>
                        <td width="47">
                            运行状态
                        </td>
                        <td width="48">
                            管理
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
                        <td width="26">
                            <p align="left">
                               ${Id}</p>
                        </td>
                        <td width="141">
                            <p align="left" >
                               ${CompanyName}</p>
                        </td>
                        <td width="58">
                            ${Name}
                        </td>
                        <td width="76">
                            <p align="left">
                                ${Mobile}</p>
                        </td>
                        <td width="119">
                            <p align="left">
                                ${Cardcode}</p>
                        </td>
                        <td width="50">
                            <p align="left">
                                ${Chaddress}</p>
                        </td>
                        <td width="86">
                            <p align="left">
                                ${ChObjects}</p>
                        </td>
                        <td width="131">
                            卡开${RebateOpen}元/首次交易${RebateConsume}元
                        </td>
                        <td width="52">
                            <p align="left">
                                ${RebateLevel}</p>
                        </td>
                        <td width="48">
                             ${EnterCardNum}
                        </td>
                         <td width="48">
                           <a href="MemberCardList.aspx?channelid=${Id}&isopencard=1"> ${OpenCardNum}</a>
                        </td>
                         <td width="48">
                            ${Firstdealnum}
                        </td>
                         <td width="48">
                            ${Summoney}
                        </td>
                        <td width="47">
                         {{if Issuetype==0}}
                            内部渠道
                            {{else  Issuetype==1}}
                              外部渠道
                              {{else  Issuetype==3}}
                              网站注册
                              {{else}}
                              微信注册
                          {{/if}}
                        </td>
                         <td width="47">
                            ${Runstate}
                        </td>
                        <td width="48">
                             <a href="ChannelEdit.aspx?channelid=${Id}">管理</a>
                        </td>
                    </tr>
                    
    </script>
    <input id="statistics" type="hidden" value="<%=statistics %>" />
</asp:Content>
