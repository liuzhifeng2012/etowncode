<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member_interest_manage.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.CrmUI.member_interest_manage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr
        {
            line-height: 15px;
        }
        
        #settings h3
        {
            padding: 10px 0 5px 0;
            font-size: 20px;
            clear: both;
            color: #2D65AA;
            font-weight: bold;
        }
        #settings h4
        {
            padding: 10px 0 5px 10px;
            font-size: 15px;
            clear: both;
            color: #2D65AA;
            font-weight: bold;
        }
        .button
        {
            width: 140px;
            height: 40px;
            line-height: 8px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            text-shadow: 1px 1px 1px #333;
            border-radius: 5px;
            margin: 0 20px 20px 0;
            position: relative;
            overflow: hidden;
        }
        
        .button.blue
        {
            border: 1px solid #1e7db9;
            box-shadow: 0 1px 2px #8fcaee inset,0 -1px 0 #497897 inset,0 -2px 3px #8fcaee inset;
            background: -webkit-linear-gradient(top,#42a4e0,#2e88c0);
            background: -moz-linear-gradient(top,#42a4e0,#2e88c0);
            background: linear-gradient(top,#42a4e0,#2e88c0);
            margin-top: 20px;
            margin-left: 5px;
        }
        .blue:hover
        {
            background: -webkit-linear-gradient(top,#70bfef,#4097ce);
            background: -moz-linear-gradient(top,#70bfef,#4097ce);
            background: linear-gradient(top,#70bfef,#4097ce);
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var crmid = $("#hid_crmid").val();

            //获得公司详情
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyDetail", {}, function (data) {
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

            $("#sel_hangye").change(function () {
                var selindustryid = $("#sel_hangye").val();
                GetTagTypeListByIndustryId(selindustryid); //根据行业获得标签类型列表
            })

            $("#Button2").click(function () {
                var checkedstr = "";
                $("input:checkbox[name='myBox']:checked").each(function () {
                    checkedstr += $(this).val() + ",";
                })
                if (checkedstr == "") {
                    alert("请选择会员兴趣");
                    return;
                }

                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=EditCrmInterest", { crmid: crmid, checkedstr: checkedstr }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", {
                            buttons: [
                                 { title: '确定', value: true }
                                ],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "member_manage.aspx?id=" + $("#hid_crmid").trimVal();
                                    if ($("#hid_crmpageindex").trimVal() != 0) {
                                        //location.href = "BusinessCustomersList.aspx?pageindex=" + $("#hid_crmpageindex").trimVal();
                                    } else {
                                       // location.href = "/member_manage.aspx?id=" + $("#hid_crmid").trimVal();
                                    }
                            }
                        });
                    }
                });
            })


        })

        function GetIndustryList(selindustryid) {
            //商户行业类型
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetIndustryList", { industryid: selindustryid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获取商户行业类型失败");
                    return;
                }
                if (data.type == 100) {
                    $("#sel_hangye").empty();

                    for (var i = 0; i < data.msg.length; i++) {
                        if (i == 0) {
                            GetTagTypeListByIndustryId(data.msg[i].Id); //根据行业获得标签类型列表
                        }
                        $("#sel_hangye").append('<option value="' + data.msg[i].Id + '">' + data.msg[i].Industryname + '</option>');
                    }

                }
            })
        }
        function GetTagTypeListByIndustryId(industryid) {
            //            //获得会员兴趣标签列表
            //            if (crmid > 0) {
            //                $.post("", {}, function (data) {
            //                    data = eval("(" + data + ")");
            //                    if (data.type == 1) {

            //                    }
            //                    if (data.type == 100) {
            //                        GetTagList(industryid,seltags);
            //                    }
            //                })
            //            } else {
            GetTagTypeList(industryid, "");
            //            }

        }
        function GetTagTypeList(industryid, seltags) {
            $("#tbody1").html("");
            //兴趣标签类型列表
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetTagTypeListByIndustryid", { industryid: industryid, comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("获取兴趣标签类型列表失败");
                    return;
                }
                var body_temp = "";
                var arr = $("#hid_crminterest").val().split(',');
                if (data.type == 100) {
                    if (data.msg.length > 0) {
                        for (var i = 0; i < data.msg.length; i++) {
                            body_temp = "<tr>" +
                                "<td colspan=\"2\">" +
                                    "<h4><a href='javascript:void(0)' \">" + data.msg[i].Typename + "</a></h4>" +
                                "</td>" +
                            "</tr>" +
                            "<tr  id=\"tr_type_" + data.msg[i].Id + "\"  style=\"padding-left: 30px;\">";

                            if (data.msg[i].B2binteresttag.length > 0) {
                                var ret = "";
                                for (var j = 0; j < data.msg[i].B2binteresttag.length; j++) {
                                    var ss = data.msg[i].B2binteresttag[j].Id;
                                    var _exist = $.inArray(ss.toString(), arr);
                                    if (_exist >= 0) {
                                        ret += "<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg[i].B2binteresttag[j].Id + "\"  checked=\"checked\" /> " + data.msg[i].B2binteresttag[j].TagName + " </label>";
                                    } else {
                                        ret += "<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg[i].B2binteresttag[j].Id + "\" /> " + data.msg[i].B2binteresttag[j].TagName + " </label>";
                                    }
                                }

                                body_temp += "<td id=\"td_type_" + data.msg[i].Id + "\"  style=\"padding-left: 30px;\">" + ret + "</td>" +
                           "<td><input type=\"text\" id=\"td_type_txt_" + data.msg[i].Id + "\" value=\"\" /><input id=\"Button1\" type=\"button\" value=\"添 加\" class=\"button blue\" style=\"width: 50px;height: 20px;\" onclick=\"addtag('" + data.msg[i].Id + "')\" /></td>";
                            }
                            else {
                                body_temp += "<td id=\"td_type_" + data.msg[i].Id + "\"  style=\"padding-left: 30px;\"></td>" +
                            "<td><input type=\"text\" id=\"td_type_txt_" + data.msg[i].Id + "\" value=\"\" /><input id=\"Button1\" type=\"button\" value=\"添 加\" class=\"button blue\" style=\"width: 50px;height: 20px;\" onclick=\"addtag('" + data.msg[i].Id + "')\" /></td>";
                            }

                            body_temp += "</tr>";
                            $("#tbody1").append(body_temp);
                        }
                    } else {
                        alert("还未添加兴趣标签类型");
                        return;
                    }
                }
            })
        }
        function GetTagListByType(typeid, seltags) {


            //兴趣标签列表 :系统添加的和特定公司自己添加的(如果公司id=0,则查询系统添加的和所有公司添加的)

            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetTagListByTypeid", { typeid: typeid, comid: $("#hid_comid").trimVal(), issystemadd: "0,1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {

                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        var ret = "";
                        if ($("#hid_crminterest").val() != '') {
                            var arr = $("#hid_crminterest").val().split(',');

                            for (var i = 0; i < data.msg.length; i++) {
                                var ss = data.msg[i].Id;
                                var _exist = $.inArray(ss.toString(), arr);
                                if (_exist >= 0) {
                                    ret += "<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg[i].Id + "\"  checked=\"checked\"/>" + data.msg[i].TagName + "</label>";
                                } else {
                                    ret += "<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg[i].Id + "\" />" + data.msg[i].TagName + "</label>";
                                }
                            }
                        } else {
                            for (var i = 0; i < data.msg.length; i++) {

                                ret += "<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg[i].Id + "\" />" + data.msg[i].TagName + "</label>";
                            }
                        }
                        $("#tr_type_" + typeid).html("");
                        $("#tr_type_" + typeid).append("<td id=\"td_type_" + typeid + "\"  style=\"padding-left: 30px;\">" + ret + "</td>" +
                           "<td><input type=\"text\" id=\"td_type_txt_" + typeid + "\" value=\"\" /><input id=\"Button1\" type=\"button\" value=\"添 加\" class=\"button blue\" style=\"width: 50px;height: 20px;\" onclick=\"addtag('" + typeid + "')\" /></td>");
                    }
                    else {
                        $("#tr_type_" + typeid).html("");
                        $("#tr_type_" + typeid).append("<td id=\"td_type_" + typeid + "\"  style=\"padding-left: 30px;\"></td>" +
                            "<td><input type=\"text\" id=\"td_type_txt_" + typeid + "\" value=\"\" /><input id=\"Button1\" type=\"button\" value=\"添 加\" class=\"button blue\" style=\"width: 50px;height: 20px;\" onclick=\"addtag('" + typeid + "')\" /></td>");
                    }
                }
            })

        }
        function addtag(typeid) {
            var tag = $("#td_type_txt_" + typeid).val();
            if (tag == "") {
                alert("标签名称不可为空");
                return;
            } else {
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=EditTag", { id: 0, name: tag, tagtypeid: typeid, issystemadd: 0, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加失败");
                        return;
                    }
                    if (data.type == 100) {
                        //                        $.prompt("成功");
                        $("#td_type_" + typeid).append("<label><input type=\"checkbox\" name=\"myBox\" value=\"" + data.msg + "\" />" + tag + "</label>");
                        return;
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
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li><a href="/ui/crmui/member_manage.aspx?id=<%=crmid %>" onfocus="this.blur()">
                    会员基本信息</a></li>
                <li class="on"><a href="/ui/crmui/member_interest_manage.aspx?crmid=<%=crmid %>" onfocus="this.blur()">
                    会员兴趣设置</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <table style="width: 100%;">
                    <tr>
                        <td height="24" colspan="2">
                            <label style="padding: 10px 0 5px 0; font-size: 20px; clear: both; color: #2D65AA;
                                font-weight: bold;">
                                商户行业类型</label>
                            <select id="sel_hangye" style="margin: 0px  10px 10px 10px; width: 120px; height: 25px;">
                                <option value="1">旅行社</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                兴趣标签类型
                            </h3>
                        </td>
                    </tr>
                    <tbody id="tbody1">
                        <%--  <tr>
                            <td colspan="2">
                                <h4>
                                    旅游目的地兴趣</h4>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 30px;">
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox0" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox1" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox2" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox3" value="1" />日本
                                </label>
                                <input id="Button1" type="button" value="添 加" class="button blue" style="width: 90px;
                                    height: 30px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h4>
                                    出游方式兴趣</h4>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 30px;">
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox4" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox5" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox6" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox7" value="1" />日本
                                </label>
                                <input id="Button3" type="button" value="添 加" class="button blue" style="width: 90px;
                                    height: 30px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h4>
                                    自定义兴趣
                                </h4>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 30px;">
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox8" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox9" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox10" value="1" />日本
                                </label>
                                <label>
                                    <input type="checkbox" name="myBox" id="Checkbox11" value="1" />日本
                                </label>
                                <input id="Button4" type="button" value="添 加" class="button blue" style="width: 90px;
                                    height: 30px;" />
                            </td>
                        </tr>
                        --%>
                    </tbody>
                    <%--     <tr>
                        <td colspan="2" style="padding-top: 20px; padding-left: 23px;">
                            <textarea id="txtselfdefined" cols="150" rows="3"></textarea>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" style="padding-top: 20px;">
                            <input type="hidden" id="hid_crmid" value="<%=crmid %>" />
                            <input type="hidden" id="hid_crminterest" value="<%=crminterest %>" />
                            <input type="hidden" id="hid_crmpageindex" value="<%=crmpageindex %>" />
                            <%if (crmid > 0)
                              { 
                            %>
                            <input id="Button2" type="button" value="提交完成" class="button blue" />
                            <%} %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
