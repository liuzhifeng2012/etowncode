<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="bangdingpos.aspx.cs"
    Inherits="ETS2.WebApp.UI.UserUI.bangdingpos" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            $("#Search").click(function () {
                SearchList(1);
           })

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AccountInfo.ashx?oper=allpos",
                    data: { pageindex: pageindex, pagesize: pageSize,key:$("#key").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $("#tblist").empty();
                            $("#divPage").empty();
//                            $.prompt("查询产品列表错误");
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
                <li><a href="/ui/PermissionUI/notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li class="on"><a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">
                    <span>Pos绑定</span></a></li>
            </ul>
        </div> 
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <a href="/ui/userui/bangdingpos.aspx" onfocus="this.blur()" target="">Pos终端绑定</a></h3>
                <div style="text-align: center; display:;">
                    <label>
                        关键词查询 
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;"  placeholder="posid、pos绑定公司名"/>
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </div>
                <div style="text-align: right;">
                    <a href="/ui/userui/bangdingpos_add.aspx" target="" title="">绑定新POS</a></div>
                <table width="780" border="0">
                    <tr>
                        <td width="51" height="31">
                            ID
                        </td>
                        <td width="80">
                            POS编号
                        </td>
                        <td width="87">
                            公司
                        </td>
                        <td width="87">
                            指定项目
                        </td>
                        <td width="81">
                            绑定时间
                        </td>
                        <td width="218">
                            备注
                        </td>
                        <td width="163">
                            管理
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
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                              ${Posid}  
                            </p>
                        </td>
                        <td >
                            <p align="left">
                               ${Poscompany} </p>
                        </td>
                        <td >
                            <p align="left">
                               {{if Projectname==""}}
                               未绑定，通用项目
                               {{else}}
                               ${Projectname}
                               {{/if}}
                                </p>
                        </td>
                        <td>
                            <p align="left">
                               ${ChangeDateFormat(BindingTime)} </p>
                        </td>
                        <td>
                            <p align="left">
                               ${Remark} </p>
                        </td>
                        <td>
                            <p align="left">
                               <a href="bangdingpos_add.aspx?pos_id=${Id}">修改</a>&nbsp;&nbsp; <a align="left" onclick="showmd5key('${Posid}','${Poscompany}','${md5key}')">点击获取秘钥
                                 </a></p>
                                
                        </td>
                    </tr>
    </script>
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 10%; position: absolute; top: 20%;">
        <table width="800px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td>
                    <span style="font-size: 14px;">公司</span>
                </td>
                <td>
                    <span style="font-size: 14px;" id="span_company"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-size: 14px;">PosID</span>
                </td>
                <td>
                    <input type="text" id="span_posid" value="" readonly="readonly" size="50" />
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-size: 14px;">Pos秘钥</span>
                </td>
                <td>
                    <input type="text" id="span_md5key" value="" readonly="readonly" size="50" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="closebtn" type="button" class="formButton" value="  关 闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function showmd5key(posid, company, md5key) {
            $("#proqrcode_rhshow").show();
            $("#span_company").text(company);
            $("#span_posid").val(posid);
            $("#span_md5key").val(md5key);
        }
        $(function () {
            $("#closebtn").click(function () {
                $("#proqrcode_rhshow").hide();
            })
        })
    </script>
</asp:Content>
