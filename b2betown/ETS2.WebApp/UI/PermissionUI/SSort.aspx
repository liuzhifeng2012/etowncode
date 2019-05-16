<%@ Page Title="" Language="C#" MasterPageFile="/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="SSort.aspx.cs" Inherits="ETS2.WebApp.UI.SSort" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 16; //每页显示条数
        $(function () {
         
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var edit_comid = $("#hid_edit_comid").trimVal();

            var comstate = $('#hid_comstate').trimVal();

            $('input[name="radcomstate"][value=' + comstate + ']').attr("checked", true);

            SearchList(1, "");

            //活动加载明细列表
            function SearchList(pageindex, key) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ProductHandler.ashx?oper=Comlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, comstate: comstate, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询数据出现错误！");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("没有查到预订信息。");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, key);
                            }
                        }
                    }
                })
            }
            $('input[name="radcomstate"]').bind("click", function () {
                var comstate = $('input:radio[name="radcomstate"]:checked').trimVal();
                window.open("ssort.aspx?comstate=" + comstate, target = "_self");
            });



            //分页
            function setpage(newcount, newpagesize, curpage, key) {
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

                        SearchList(page, key);

                        return false;
                    }
                });
            }


            $("#SmsEnter").click(function () {

                var smsid = $("#hid_id").trimVal();
                var edit_comid = $("#hid_edit_comid").trimVal();
                var smsaccount = $("#smsaccount").trimVal();
                var smspass = $("#smspass").trimVal();
                var smssign = $("#smssign").trimVal();
                var subid = $("#subid").trimVal();

                var smstype = $('input[name="smstype"]:checked').val();

                if (smstype == "") {
                    $.prompt("请选择短信接口！");
                    return;
                }

                
                    if (smssign == "") {
                        $.prompt("请填写签名，将使用此签名发送短信！");
                        return;
                    }

                    if (subid == "") {
                        $.prompt("扩展码请管短信通道服务商要！");
                        return;
                     }
                

                if (smsid == "0") {
                    $.prompt("参数传递错误，刷新后重新操作");
                    return;
                }

                $.post("/JsonFactory/DirectSellHandler.ashx?oper=editsmsset", { directsellsetid: smsid, comid: edit_comid, smsaccount: smsaccount, smspass: smspass, smssign: smssign, smstype: smstype,subid:subid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("短信设置设置成功");
                        return;
                    }
                })
            })

            $("#Search").click(function () {
                var key = $("#key").trimVal();
                if (key == "" || key == "商户名称") {
                    $.prompt("请输入商户名称");
                    return;
                }
                SearchList(1, key);
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                }
            });

            $("#kaihu_open").click(function () {
                var id = $("#hid_edit_comid").trimVal();

                $.post("/JsonFactory/ProductHandler.ashx?oper=UpComstate", { id: id, state: "已暂停" }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            $.prompt("账户已开通成功，下一步分配所属行业");
                            $("#kaihu").hide();
                            hangye_set(id, $("#hid_edit_company").val());
                            return;
                        }
                    }
                })
            })


            $("#smstype1").click(function () {
                $(".tongyongsms").show();
                $(".dulisms").hide();
            })

            $("#smstype2").click(function () {
                $(".tongyongsms").show();
                $(".dulisms").hide();
            })

        })

        function UpComPlatform_state(id, Platform_state) {
            var st = "";
            if (Platform_state == "已屏蔽") {
                st = "显示";
            }
            if (Platform_state == "已显示") {
                st = "屏蔽";
            }
            if (confirm("确认在商户平台列表中" + st + "此商户吗？")) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=UpComPlatform_state", { id: id, state: Platform_state }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.reload();
                            return;
                        }
                    }
                })
            } else {
                $.prompt("操作取消");
                return;
            }
        }
        function UpComstate(id, comstate) {
            var st = "";
            if (comstate == "已开通") {
                st = "暂停";
            }
            if (comstate == "已暂停") {
                st = "开通";
            }
            if (confirm("确认" + st + "商户吗？")) {
                $.post("/JsonFactory/ProductHandler.ashx?oper=UpComstate", { id: id, state: comstate }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.reload();
                            return;
                        }
                    }
                })
            } else {
                $.prompt("操作取消");
                return;
            }

        }

        function InsertChildAccount(childcomid) {
            //if (confirm("确认查看此账户吗？")) {
            $.post("/JsonFactory/AccountInfo.ashx?oper=ViewChildCompany", { childcomid: childcomid, curuserid: $("#hid_userid").val() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt(data.msg);
                    return;
                }
                if (data.type == 100) {
                    window.open("/Manage.aspx", target = "_self");
                }
            })
            //} else {
            //     $.prompt("操作取消");
            //    return;
            // }
        }
        function AdjustFee(id) {
            var fee = $("#txtfee_" + id).trimVal();
            $.post("/JsonFactory/ProductHandler.ashx?oper=AdjustFee", { id: id, fee: fee }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("调整出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("调整成功", { buttons: [{ title: "确定", value: true}], submit: function (data) {
                        window.location.reload();
                    }
                    })
                }
            })
        }
        function AdjustServiceFee(id) {
            var ServiceFee = $("#txtservicefee_" + id).trimVal();
            $.post("/JsonFactory/ProductHandler.ashx?oper=AdjustServiceFee", { id: id, ServiceFee: ServiceFee }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("调整出现错误");
                    return;
                }
                if (data.type == 100) {
                    $.prompt("调整成功", { buttons: [{ title: "确定", value: true}], submit: function (data) {
                        window.location.reload();
                    }
                    })
                }
            })
        }
        //调整是否含有所属门市
        function WhetherHasInnerChannel(companyid) {
            var hasinnerchannel = $("#selhasinnerchannel_" + companyid).val();
            $.post("/JsonFactory/ProductHandler.ashx?oper=AdjustHasInnerChannel", { companyid: companyid, hasinnerchannel: hasinnerchannel }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {
                    $.prompt("调整成功", { buttons: [{ "title": "确定", value: true}], submit: function (e, m, v, f) { if (m == true) { location.reload(); } } })
                }
            })
        }
        //关闭短信设置窗口
        function cancel() {
            $("#span_rh").text("");
            $("#smstext").text("");
            //退票
            $("#showticket").hide();
        }





        //设置短信账户
        function sms_set(id, company) {
            $("#company").html(company);
            $("#hid_edit_comid").val(id);
            $("#showticket").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory//DirectSellHandler.ashx?oper=getdirectsellset",
                data: { comid: id },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息失败");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg != null) {
                            $("#hid_id").val(data.msg.Id);
                            $("#smsaccount").val(data.msg.Smsaccount);
                            $("#smspass").val(data.msg.Smspass);
                            $("#smssign").val(data.msg.Smssign);
                            $("#subid").val(data.msg.Subid);
                            

                            if (data.msg.Smstype == 0) {
                                $(".tongyongsms").show();
                                $(".dulisms").hide();

                                $("#smstype1").attr("checked", "checked");
                                $("#smstype2").attr("checked", false);
                            } else {
                                $(".tongyongsms").show();
                                $(".dulisms").hide();
                                $("#smstype1").attr("checked", false);
                                $("#smstype2").attr("checked", "checked");
                            }
                        } else {
                            $("#smsaccount").val("");
                            $("#smspass").val("");
                            $("#smssign").val("");
                        }
                    }
                }
            })

        }


        //设置短信账户
        function kaihu_view(id, company) {
            $("#k_company").html(company);
            $("#hid_edit_company").val(company);
            $("#hid_edit_comid").val(id);
            $("#hid_kaihu").val(1);
            $("#kaihu").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/AccountInfo.ashx?oper=getcurcompany",
                data: { comid: id },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息失败");
                        return;
                    }
                    if (data.type == 100) {
                        $("#province").html(data.msg.B2bcompanyinfo.Province);
                        $("#city").html(data.msg.B2bcompanyinfo.City);
                        $("#com_type").html(data.msg.Com_type);
                        $("#Contact").html(data.msg.B2bcompanyinfo.Contact);
                        $("#tel").html(data.msg.B2bcompanyinfo.Tel);
                        $("#scenic_intro").html(data.msg.B2bcompanyinfo.Scenic_intro);
                        $("#domainname").html(data.msg.B2bcompanyinfo.Domainname);
                        $("#com_code").html(data.msg.B2bcompanyinfo.Com_code);
                        $("#weixinname").html(data.msg.B2bcompanyinfo.Weixinname); 
                  

                    }
                }
            })



        }



        //关闭短信设置窗口
        function kaihu_cancel() {
            //隐藏开户
            $("#kaihu").hide();
            $("#hid_kaihu").val(0);
        }

        //----此部分是行业部分BEGIN---//
        function changehangye() {
            var comid = $("#hid_edit_comid").val();
            var hangyeid = $("#sel_hangye").val();
            var checkedtext = $("#sel_hangye").find("option:selected").text()

            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=ChangeHangye", { comid: comid, hangye: hangyeid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整行业失败");
                } else {
                    alert("调整行业成功");
                    $("#Span_company").text("");
                    $("#div_showhangye").hide();
                    $("#a_hangye" + comid).html(checkedtext);

                    var kaihu_temp = $("#hid_kaihu").val();
                    if (kaihu_temp == 1) {
                        alert("下面进行人员权限分配");
                        window.location="/ui/permissionui/MasterList.aspx?childcomid=" + comid;
                    }
                }
            })
        }

        function hangyecancel() {
            $("#Span_company").text("");
            //退票
            $("#div_showhangye").hide();
        }
        function hangye_set(comid, comnamem) {
            $("#Span_company").html(comnamem);
            $("#hid_edit_comid").val(comid);
            $("#div_showhangye").show();
            //获得公司详情
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyDetail", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获得公司详情失败");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg.Com_type == 0) {
                        GetIndustryList(0)
                    } else {
                        GetIndustryList(data.msg.Com_type)
                    }
                    
                }
            })
        }

        function GetIndustryList(selindustryid) {
            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获取商户行业类型失败");
                    return;
                }
                if (data.type == 100) {
                    $("#sel_hangye").empty();
                    $("#sel_hangye").append('<option value="0">综合行业</option>');
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Id == selindustryid) {
                            $("#sel_hangye").append('<option value="' + data.msg[i].Id + '"  selected="selected">' + data.msg[i].Industryname + '</option>');
                        } else {
                            $("#sel_hangye").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                        }
                    }

                }
            })
        }
        //----此部分是行业部分END---//
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%--<li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>管理组管理</span></a></li>--%>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <%-- <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>--%>
                <li class="on"><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()" target=""><span>提现财务管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="rentserver_refund.aspx" onfocus="this.blur()"><span>退押金管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
                <li><a href="/weixin/WxTemplateManage.aspx" onfocus="this.blur()" target="">微信模板管理</a></li>
                <li><a href="ShouGongQuerenPay.aspx" onfocus="this.blur()" target="">手工支付确认</a></li>
                <li><a href="posloglist.aspx" onfocus="this.blur()" target="">Pos验证日志</a></li>
                <li><a href="yznoticelist.aspx" onfocus="this.blur()" target=""><span>验证通知日志</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 id="h3groups">
                    商户开通情况：
                    <label>
                        <input name="radcomstate" type="radio" value="1">
                        开通</label>
                    <label>
                        <input name="radcomstate" type="radio" value="2">
                        暂停</label>
                    <label>
                        <input name="radcomstate" type="radio" value="0">
                        全部</label>
                    <a href="/account/Register.aspx" style="float: right; margin-right: 60px; font-size: 15px;
                        color: Blue;">新增商户</a>&nbsp;&nbsp; <a href="MerchantSort.aspx" style="float: right;
                            margin-right: 60px; font-size: 15px; color: Blue;">商户排序</a>
                </h3>
                <div style="text-align: center;">
                    <label>
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="商户名称">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="  查  询  " style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="4%" height="42">
                            ID
                        </td>
                        <td width="8%">
                            商户名称
                        </td>
                        <td width="3%">
                            行业
                        </td>
                        <td width="3%">
                            人员
                        </td>
                        <td width="8%">
                            开通情况
                        </td>
                        <td width="6%">
                            平台显示
                        </td>
                        <td width="6%">
                            总会员数
                        </td>
                        <td width="6%">
                            直销收款
                        </td>
                        <td width="6%">
                            退票金额
                        </td>
                        <td width="6%">
                            提现金额
                        </td>
                        <td width="4%">
                            转商户<br />
                            支付宝
                        </td>
                        <td width="6%">
                            手续费
                        </td>
                        <td width="6%">
                            未提现金额
                        </td>
                        <td width="5%">
                            微信<br />关注数
                        </td>
                        <td width="4%">
                            有无所<br />
                            属门市
                        </td>
                        <td width="8%">
                            手续费率
                        </td>
                        <td width="6%">
                            --
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr  onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td >
                            <p align="left">
                                ${ID}</p>
                        </td>
                        <td>
                            <a href="javascript:void(0)" onclick="InsertChildAccount('${ID}')" style="color:Blue;text-decoration: underline;">${Com_name}</a>
                        </td>
                       
                         <td>
                             <a href="javascript:void(0)" onclick="hangye_set('${ID}','${Com_name}')" id="a_hangye${ID}">${Hangye}</a>
                        </td>
                         <td>
                             <a href="/ui/permissionui/MasterList.aspx?childcomid=${ID}">人员</a>
                        </td>
                          
                        <td>
                            <input type="button" onclick="UpComstate('${ID}','${Com_state}')"  value="${Com_state}"/>
                            {{if Com_state=="已暂停"}}
                            <input type="button" onclick="kaihu_view('${ID}','${Com_name}')"  value="开户"/>
                            {{/if}}
                        </td>
                        <td width="6%">
                        <input type="button" onclick="UpComPlatform_state('${ID}','${Platform_state}')"  value="${Platform_state}"/>
                        </td>
                        <td>
                        ${MembersNum}
                        </td>

                         <td>
                         ${SjZxsk}
                        </td>
                        <td>
                         ${SjZxtp}
                        </td>
                           <td>
                         ${SjTx}
                        </td>
                           <td>
                         ${SjZsjalipay}
                        </td>
                           <td>
                         ${SjSxf}
                        </td>
                           <td>
                        ${SjNotTx} 
                        </td>

                         <td>
                         ${WeiXinAttentionNum}
                        </td>
                        <td>
                         <select id="selhasinnerchannel_${ID}" onchange="WhetherHasInnerChannel('${ID}')">
                          {{if HasInnerChannel==true}}
                          <option value="true"  selected="selected">有</option>
                           <option value="false" >无</option>
                          {{else}}
                          <option value="true" >有</option>
                           <option value="false"  selected="selected" >无</option>
                          {{/if}}
                          
                         
                        </select
                        </td>
                        <td>
                        <input type="text"  value="${Fee}" id="txtfee_${ID}" size="2">
                        <input type="button" id="btn1" value="支付费" onclick="AdjustFee(${ID})" />
                         <div>
                          <input type="text"  value="${ServiceFee}" id="txtservicefee_${ID}" size="2">
                          <input type="button" id="btn2" value="服务费" onclick="AdjustServiceFee(${ID})" />
                          </div>
                        </td>
                        <td>
                           <input type="button" onclick="sms_set('${ID}','${Com_name}')"  value="设置短信"/>
                          
                        </td>
                    </tr>
                    </script>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <div id="div_showhangye" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">商户行业设置
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    商户名称：<span id="Span_company"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    行业：
                    <select id="sel_hangye">
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input id="Button1" type="button" class="formButton" onclick="changehangye()" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="hangyecancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span_ticket">
                        商户短信设置 </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    商户名称：<span id="company"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信接口商：<input  name="smstype" id="smstype1" value="0" type="radio" /> 碟信通道 <input  name="smstype"  id="smstype2"  value="1" type="radio" />至甄互联
                </td>
            </tr>
            <tr class="dulisms">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信账户：<input type="text" id="smsaccount" value="" style="width: 100px;" />
                </td>
            </tr>
            <tr class="dulisms">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信密码：<input type="text" id="smspass" value="" style="width: 100px;" />
                </td>
            </tr>
            <tr  class="tongyongsms" style=" display:none;">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信签名：<input type="text" id="smssign" value="" style="width: 100px;" />
                </td>
            </tr>
            <tr  class="tongyongsms" style=" display:none;">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    扩 展 号 ：<input type="text" id="subid" value="" style="width: 100px;" />(必须填写)
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input id="SmsEnter" name="SmsEnter" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <div id="kaihu" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 650px; height: 400px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span2">
                        商户管理(开户) </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    商户名称：<span id="k_company"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    所属省：<span id="province"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    地区市：<span id="city"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    所属行业：<span id="com_type"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    联系人姓名：<span id="Contact"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    联系人电话：<span id="tel"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    企业介绍：<span id="scenic_intro"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    官网网址：<span id="domainname"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    企业营业执照注册号：<span id="com_code"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    微信公众账号：<span id="weixinname"></span>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input id="kaihu_open" name="kaihu_open" type="button" class="formButton" value="  立即开通商户 ，下一步分配行业 " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="kaihu_cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <input type="hidden" id="hid_edit_comid" value="0" />
    <input type="hidden" id="hid_edit_company" value="" />
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_comstate" value="<%=comstate %>" />
    <input type="hidden" id="hid_kaihu" value="0" />
</asp:Content>
