<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProClass.aspx.cs" MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.ProClass" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                    $("#sel_hangye").empty();
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#industryid").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                    }

                }
            })



            //获取分类
            SearchList(1, 16);
            function SearchList(pageindex, pagesize) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=proclasslist",
                    data: { pageindex: pageindex, pagesize: pagesize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pagesize, pageindex);
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

                        SearchList(page, newpagesize);

                        return false;
                    }
                });
            }


            $("#SmsEnter").click(function () {
                var classid = $("#classid").trimVal();
                var classname = $("#classname").trimVal();
                var industryid = $("#industryid").trimVal();

                if (industryid == '') {
                    $.prompt('请选择所属行业');
                    return;
                }

                if (classname == '') {
                    $.prompt('请填写产品类目');
                    return;
                }

                $.post("/JsonFactory/ProductHandler.ashx?oper=proclassmanage", { Classname: classname, classid: classid, industryid: industryid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("编辑操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) {
                            if (v == true) {
                                location.href = "ProClass.aspx";
                         } } });
                        return;
                    }
                });

            })


        })


        //关闭短信设置窗口
        function cancel() {
            $("#classid").val("0");
            $("#classname").val("")
            $("#showticket").hide();
        }

        //设置短信账户
        function proclass_set(id,Industryid) {
            if (id != 0) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=proclassbyid",
                    data: { classid: id },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#classid").val(id);
                            $("#classname").val(data.msg.Classname);
                            if (Industryid != 0) {
                                $("input:radio[name='industryid'][value=" + data.msg.Industryid + "]").attr("checked", true);
                            }
                        }
                    }
                })
            }
            $("#showticket").show();
        }


        //删除
        function delproclass(id) {
            if (id != 0) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=proclassdel",
                    data: { classid: id },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("操作列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("删除成功", { button: [{ title: "确定", value: true}], show: "slideDown", submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.href = "ProClass.aspx";
                                } 
                            } 
                            });
                            return;

                        }
                    }
                })
            }
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()"><span>提现财务管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li class="on"><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>

            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    管理产品类目</h3>
                <br />
                <input type="button" onclick="proclass_set('0')"  value=" 添加产品类目 "/>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                ID
                                </p>
                        </td>
                        <td width="157">
                            <p align="left">
                                所属行业
                            </p>
                        </td>
                        <td width="157">
                            <p align="left">
                                类目名称
                            </p>
                        </td>
                        <td width="60">
                            <p align="center">
                                
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
                        <td width="51">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                ${Industryname}
                            </p>
                        </td>
                       <td width="157">
                            <p align="left">
                                ${Classname}
                            </p>
                        </td>
                       
                        <td width="60">
                        <p align="center">
                        <input type="button" onclick="proclass_set('${Id}','${Industryid}')"  value=" 编 辑 "/>
                        <input type="button" onclick="delproclass('${Id}')"  value=" 删 除 "/>
                        </p>
                        </td>
                      
                    </tr>
    </script>

    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">

            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span_ticket">
                        产品类目设置 </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    所属行业：<select name="industryid" id="industryid" class="ui-input">  
                        <option value="" selected="selected" >请选择所属行业</option>  
                    </select>  
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产品类目名称：<input type="text" id="classname" value="" style="width: 100px;" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                <input type="hidden" id="classid" value="0"/>
                    <input id="SmsEnter" name="SmsEnter" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
