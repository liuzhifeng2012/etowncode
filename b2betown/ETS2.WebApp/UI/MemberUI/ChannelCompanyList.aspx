<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ChannelCompanyList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ChannelCompanyList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 16; //每页显示条数
        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            var channeltype = $("#hid_channeltype").val();
            if (channeltype != "") {
                $('input[name="radIssuetype"][value="' + channeltype + '"]').attr("checked", true);
                ("#lblchannel" + channeltype).show();
            }
            else {
                $("#lblchannel1").show();
                $("#lblchannel0").show();
            }

            ShowChannelCompanyList(channeltype, 1);

            //给渠道类型绑定点击事件
            $('input[name="radIssuetype"]').bind("click", function () {
                var channeltype = $('input:radio[name="radIssuetype"]:checked').trimVal();
                ShowChannelCompanyList(channeltype, 1);
            })
        })
        function ShowChannelCompanyList(channeltype, pageindex) {
            $.post("/JsonFactory/ChannelHandler.ashx?oper=GetChannelCompanyList", { comid: $("#hid_comid").trimVal(), channeltype: channeltype, pageindex: pageindex, pagesize: pageSize }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalCount == 0) {
                        $("#tblist").html("<tr><td colspan='4'>查询数据为空</td></tr>");
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalCount, pageSize, pageindex, channeltype);
                    }
                }
            })
        }
        //分页 
        function setpage(newcount, newpagesize, curpage, opval) {
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

                    ShowChannelCompanyList(opval, page);

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
                <li class="on"><a href="ChannelCompanyList.aspx" onfocus="this.blur()"><span>渠道公司列表</span></a></li>
                <li><a href="ChannelCompanyEdit.aspx" onfocus="this.blur()">添加渠道公司</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    渠道公司列表
                </h3>
                <table width="780" border="0">
                    <tr id="trchanneltype">
                        <td width="19%" class="tdHead">
                            渠道类型：
                        </td>
                        <td width="81%" colspan="3">
                            <label id="lblchannel1" style="display: none">
                                <input name="radIssuetype" type="radio" value="1" />
                                合作公司
                            </label>
                            <label id="lblchannel0" style="display: none">
                                <input name="radIssuetype" type="radio" value="0" />
                                内部门市
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="141">
                            <p align="left">
                                渠道单位名称</p>
                        </td>
                        <td width="141">
                            <p align="left">
                                渠道单位类型</p>
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
                               ${Companyname}</p>
                        </td>
                         <td width="141">
                            <p align="left" >
                               ${Issuetype}</p>
                        </td>
                        <td width="48">
                             <a href="ChannelCompanyEdit.aspx?channelcompanyid=${Id}">修改</a>&nbsp;&nbsp;
                            
                             <a href="#">删除(暂时未开通)</a>
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_channeltype" type="hidden" value="<%=channeltype %>" />
</asp:Content>
