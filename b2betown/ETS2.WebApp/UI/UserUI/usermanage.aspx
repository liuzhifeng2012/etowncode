<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="usermanage.aspx.cs"
    Inherits="ETS2.WebApp.UI.UserUI.usermanage" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var comname = $("#hid_comname").trimVal();
            var userid = $("#hid_userid").trimVal();
            var staffid = $("#hid_staffid").trimVal();

            //首先确定渠道来源中"所属门市"是否选中:a.添加操作不选中；b.编辑操作并且员工所在单位不为0选中
            $("input[name='radchannelsource'][value='0']").attr("checked", false)

            //渠道来源点击事件
            $("input[name='radchannelsource']").bind("click", function () {
                var sourceid = $("input:radio[name='radchannelsource']:checked").val();
                InsChannelCompany(sourceid, 0);
                HandleGroups(0, 1);
            })

            if (staffid != "0") {//编辑员工
                $.post("/JsonFactory/UserHandle.ashx?oper=GetAccountInfo", { id: staffid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取用户信息出错", {
                            buttons: [{ title: "确定", value: true}],
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.href = "usermanage.aspx";
                                }
                            }
                        });
                    }
                    if (data.type == 100) {
                        $("#accountt").val(data.msg[0].Accounts);


                        $("#password").val(data.msg[0].PassWord);
                        $("#password2").val(data.msg[0].PassWord);

                        $("#employeename").val(data.msg[0].MasterName);

                        $("#companyname").val(data.msg[0].CompanyName);
                        $("#tel").val(data.msg[0].Tel);
                        $("#oldtel").val(data.msg[0].Tel);

                        $("#Hidden1").val(data.msg[0].Headimg);
                        $("#headPortraitImg").attr("src", data.msg[0].Headimgurl);
                        $("#txtjob").val(data.msg[0].Job);
                        $("#Workingyears").val(data.msg[0].Workingyears);
                        $("#Selfbrief").val(data.msg[0].Selfbrief);
                        $("#Workdaystime").val(data.msg[0].Workdaystime);
                        $("#Workendtime").val(data.msg[0].Workendtime);
                        //                        $("#Workaddress").val(data.msg[0].WorkAddress);
                        $("#Fixphone").val(data.msg[0].Fixphone);
                        $("#Email").val(data.msg[0].Email);
                        $("#Homepage").val(data.msg[0].Homepage);
                        $("#Weibopage").val(data.msg[0].Weibopage);
                        $("#QQ").val(data.msg[0].QQ);
                        $("#Weixin").val(data.msg[0].Weixin);


                        $("#worktimestar").val(data.msg[0].worktimestar);
                        $("#worktimeend").val(data.msg[0].worktimeend);
                        $("#workendtimestar").val(data.msg[0].workendtimestar);
                        $("#workendtimeend").val(data.msg[0].workendtimeend);
                        $("#bindingproid").val(data.msg[0].bindingproid);



                        if (data.msg[0].Viewtel == 1) {
                            $("input[name='viewtel']").attr("checked", false);
                        } else {
                            $("input[name='viewtel']").attr("checked", true);
                        }

                        //工作日
                        var workdaystr = data.msg[0].Workdays;
                        if (workdaystr != "" && workdaystr != null) {
                            $("input[name='workdays']").attr("checked", false);
                            var items = workdaystr.split(/[,，]/g);
                            $.each(items, function (index, item) {
                                $("input[name='workdays']").each(function () {
                                    if ($(this).val() == item) {
                                        $(this).attr("checked", true);
                                    }
                                });
                            });
                        }
                        var seledval = data.msg[0].GroupIds;
                        HandleGroups(seledval, 0);


                        if (data.msg[0].ChannelCompanyId != "0") {
                            //首先确定渠道来源中"所属门市"是否选中:a.添加操作不选中；b.编辑操作并且员工所在单位不为0选中
                            $("input:radio[name='radchannelsource'][value=" + data.msg[0].Channelsource + "]").attr("checked", true);
                        }
                        InsChannelCompany(data.msg[0].Channelsource, data.msg[0].ChannelCompanyId); //默认显示内部渠道


                        $("#selemployerstate").val(data.msg[0].EmployeState);
                        $("#peoplelistview").val(data.msg[0].Peoplelistview);

                    }
                })
                $("#accountt").attr("readonly", "readonly");
            } else {
                HandleGroups(0, 0);
                InsChannelCompany(0, 0); //默认显示内部渠道
                $("#accountt").removeAttr("readonly");
            }



            $("#button").click(function () {
                var account = $("#accountt").trimVal();
                var password = $("#password").trimVal();
                var password2 = $("#password2").trimVal();
                var employeename = $("#employeename").trimVal();
                var tel = $("#tel").trimVal();
                var viewtel = $("input:checkbox[name=viewtel]:checked'").trimVal();
                var oldtel = $("#oldtel").trimVal();
                var bindingproid = $("#bindingproid").trimVal();
                if (account == "") {
                    $.prompt("账号信息不可为空");
                    return;
                }
                if (password == "" || password2 == "") {
                    $.prompt("两次密码输入都不可为空");
                    return;
                }
                if (employeename == "") {
                    $.prompt("姓名不可为空");
                    return;
                }
                if (tel == "") {
                    $.prompt("电话不可为空");
                    return;
                } else {
//                    if (!$("#tel").val().match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
//                        $.prompt("手机号码格式不正确");
//                        return;
                    //                    }
                     if ($("#tel").trimVal().length!=11)
                     {
                         $.prompt("手机号码格式不正确");
                         return;
                     }

                }


                var headimg = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                if (headimg == "") {
                    headimg = $("#Hidden1").trimVal();
                }
                //                if (headimg == "" || headimg == "0") {
                //                    alert("请上传个人头像");
                //                    return;
                //                }

                var job = $("#txtjob").trimVal();
                if (job == "") {
                    alert("请输入职位描述");
                    return;
                }
                var Workingyears = $("#Workingyears").val();
                var Selfbrief = $("#Selfbrief").trimVal();
                if (Selfbrief == "") {
                    alert("请输入个人简介");
                    return;
                }
                var workdays = "";
                $("input:checkbox[name=workdays]:checked'").each(function (i) {
                    workdays += ($(this).val() + ",");
                });
                if (workdays == "") {
                    //                    $.prompt("请选择员工工作日！");
                    //                    return;
                } else {
                    workdays = workdays.substring(0, workdays.length - 1);
                }


                var worktimestar = $("#worktimestar").val();
                var worktimeend = $("#worktimeend").val();
                var workendtimestar = $("#workendtimestar").val();
                var workendtimeend = $("#workendtimeend").val();


                var Workdaystime = worktimestar + "点到" + worktimeend + "点";
                var Workendtime = workendtimestar + "点到" + workendtimeend + "点";



                var Fixphone = $("#Fixphone").trimVal();

                var Email = $("#Email").trimVal();

                var Homepage = $("#Homepage").trimVal();
                var Weibopage = $("#Weibopage").trimVal();
                var QQ = $("#QQ").trimVal();
                //                if (Email == "") {
                //                    alert("请输入QQ号");
                //                    return;
                //                }
                var Weixin = $("#Weixin").trimVal();
                //                if (Weixin == "") {
                //                    alert("请输入微信号");
                //                    return;
                //                }


                var groupids = $('input:radio[name="radgrouptype"]:checked').val();
                if (groupids == null) {
                    $.prompt("请选择用户分组！");
                    return;
                }

                var channelsource = $('input:radio[name="radchannelsource"]:checked').trimVal();

                var channelcompanyid = $("#selchannels").trimVal();
                var peoplelistview = $("#peoplelistview").trimVal();

                $.post("/JsonFactory/AccountInfo.ashx?oper=editstaff", { headimg: headimg, job: job, Workingyears: Workingyears, Selfbrief: Selfbrief, workdays: workdays, Workdaystime: Workdaystime, Workendtime: Workendtime, Fixphone: Fixphone, Email: Email, Homepage: Homepage, Weibopage: Weibopage, QQ: QQ, Weixin: Weixin, channelsource: channelsource, channelcompanyid: channelcompanyid, groupids: groupids, staffid: staffid, comid: comid, userid: userid, account: account, password: password, employeename: employeename, tel: tel, viewtel: viewtel, oldtel: oldtel, employeestate: $("#selemployerstate").val(), peoplelistview: peoplelistview, worktimestar: worktimestar, worktimeend: worktimeend, workendtimestar: workendtimestar, workendtimeend: workendtimeend,bindingproid:bindingproid }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑账号信息成功",
                                                        {
                                                            buttons: [{ title: "确定", value: true}],
                                                            submit: callbackfuc
                                                        });

                    }
                })
                function callbackfuc(e, v, m, f) {
                    if (v == true) {
                        if ('<%=istiaozhuan %>' == '1') {
                            location.href = "userlist.aspx";
                        } else {
                            return;
                        }
                    }
                }

            })

            //随渠道公司变化，权限列表变化：a.选择公司，则显示登录账户权限内的管理组列表；b.选择门市/合作单位，显示 门市经理/合作单位负责人 权限内的管理组列表
            $("#selchannels").change(function (data) {
                var channelcompanyid = $("#selchannels").val();
                var channelsource = $("input:radio[name='radchannelsource']:checked").val();

                if (channelcompanyid == 0) {
                    Handle_ChannelCompanyGroups("0,1", 0);
                } else {
                    Handle_ChannelCompanyGroups(channelsource, 0);
                }
            })
        })
        function Handle_ChannelCompanyGroups(channelsource, seledval) {

            //选择门市/合作单位，显示 门市经理/合作单位负责人 权限内的管理组列表
            $.post("/jsonfactory/permissionhandler.ashx?oper=GetGroupBychannelsource", { channelsource: channelsource,userid:$("#hid_userid").trimVal() }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    //                    $("#hid_viewgroupids").val(data.msg.Groupids);
                    //对渠道进行隐藏或者显示
                    var Isviewchannel = data.msg.Isviewchannel;
                    if (Isviewchannel == false) {
                        $("#trchannelsource").hide();
                        $("#trchannelcompany").hide();
                    }
                    else {
                        $("#trchannelsource").show();
                        $("#trchannelcompany").show();
                    }
                    var groupidsd = data.msg.Groupids;

                    //动态添加管理组
                    $.post("/jsonfactory/permissionhandler.ashx?oper=GetAllGroups", {}, function (data) {

                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("操作出现错误" + data.msg);
                            return;
                        }
                        if (data.type == 100) {

                            $("#tdgroups").empty();
                            if (data.totalCount > 0) {


                                var viewgroupids = groupidsd;
                                var items = viewgroupids.split(/[,，]/g);

                                var groupstr = "";
                                for (var i = 0; i < data.totalCount; i++) {
                                    var item = data.msg[i].Groupid.toString();
                                    if ($.inArray(item, items) != -1) {
                                        //                            groupstr += '<label><input type="checkbox" name="checkGrougId" value="' + data.msg[i].Groupid + '" onclick="this.checked=!this.checked" />' + data.msg[i].Groupname + '</label>';

                                        if (data.msg[i].Groupid == seledval) {
                                            groupstr += '<label><input  name="radgrouptype" type="radio" value="' + data.msg[i].Groupid + '"  checked="checked">' + data.msg[i].Groupname + '</label><br>';
                                        } else {
                                            groupstr += '<label><input  name="radgrouptype" type="radio" value="' + data.msg[i].Groupid + '" >' + data.msg[i].Groupname + '</label><br>';
                                        }
                                    }
                                }
                                $("#tdgroups").append(groupstr);
                            }

                        }
                    });
                }
            });

        }


        function HandleGroups(seledval, opor) {
            //如果编辑员工则根据员工所在单位(公司/门市)得到管理组；如果添加员工则根据登录账户所在单位(公司/门市)得到管理组

            var yy = $("#hid_staffid").trimVal();
            if (opor == 0)//初始页面加载操作
            {
                if (yy == 0) {
                    yy = $("#hid_userid").trimVal();
                }
            }
            if (opor == 1)//渠道单位下拉框变动操作
            {
                yy = $("#hid_userid").trimVal();
            }

            //得到用户所在单位(公司/门市/合作单位)
            $.post("/jsonfactory/CrmMemberHandler.ashx?oper=GetMemberChanelCompanyByUserid", { yy: yy }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    Handle_ChannelCompanyGroups("0,1", seledval);
                }
                if (data.type == 100) {
                    Handle_ChannelCompanyGroups(data.msg.Issuetype, seledval);
                }
            })
 
        }


        function InsChannelCompany(unittype, ChannelCompanyId) {
            //1.总公司账户：
            //   渠道全部信息显示
            //   a.渠道来源“所属门市”默认不选中，员工所在单位只有公司名称且默认选中
            //   b.选中“所属门市”后,员工所在单位：增加全部的门店列表
            //   c.选中“合作公司”后,员工所在单位：增加全部的合作公司列表
            //2.门市经理账户：
            //  渠道全部信息隐藏
            //  a.渠道来源“所属门市”默认选中；
            //  b.所在单位只有自己门市且默认选中


            var channelsource = $('input:radio[name="radchannelsource"]:checked').val();
            var ischosen = 0; //判断渠道来源中“所属门市”是否选中; 
            if (channelsource != null) {
                ischosen = 1; //“所属门市”：选中
            }

            //动态添加渠道公司
            $.post("/jsonfactory/ChannelHandler.ashx?oper=getunitlist2", { comid: $("#hid_comid").trimVal(), unittype: unittype, ischosen: ischosen, userid: $("#hid_userid").trimVal() }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    $("#selchannels").empty();
                    if (data.channelcompanyid == 0) {//公司账户，所在单位默认添加上 公司名称
                        $("#selchannels").append('<option value="0">' + $("#hid_comname").trimVal() + '</option>');
                    } else {//门市账户， 所在单位只有自己门市且默认选中
                    }
                    if (data.msg != null) {

                        if (data.msg.length > 0) {

                            var groupstr = "";


                            for (var i = 0; i < data.msg.length; i++) {

                                if (data.msg[i].Id == ChannelCompanyId) {
                                    groupstr += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Companyname + '</option>';
                                } else {
                                    groupstr += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Companyname + '</option>';

                                }
                            }
                            $("#selchannels").append(groupstr);
                        }

                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
     <%--   <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="userlist.aspx" onfocus="this.blur()" target=""><span>员工管理</span></a></li>
                <li  class="on"><a href="usermanage.aspx" onfocus="this.blur()" target=""><span>添加员工</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        账户设置</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            用户名</label>
                        <input type="text" id="accountt" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            密码</label>
                        <input type="password" id="password" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            确认密码</label>
                        <input type="password" id="password2" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        个人基本信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            姓名</label>
                        <input type="text" id="employeename" value="" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            联系手机</label>
                        <input type="text" id="tel" value="" class="mi-input" />*
                        <input checked="checked" name="viewtel" value="0" type="checkbox" />个人页面不显示手机
                        <input type="hidden" id="oldtel" value="" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            固定电话</label>
                        <input type="text" id="Fixphone" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            个人头像</label>
                        <input type="hidden" id="Hidden1" value="" />
                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                        <uc1:uploadFile ID="headPortrait" runat="server" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            建议上传300px*300px图片</label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            职位描述</label>
                        <input type="text" id="txtjob" value="" class="mi-input" maxlength="5" style="width: 80px;" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            从业经验</label>
                        <select id="Workingyears" class="mi-input">
                            <option value="1">1年</option>
                            <option value="2">2年</option>
                            <option value="3">3年</option>
                            <option value="4">4年</option>
                            <option value="5">5年</option>
                            <option value="6">6年</option>
                            <option value="7">7年</option>
                            <option value="8">8年</option>
                            <option value="9">9年</option>
                            <option value="10">10年</option>
                            <option value="11">11年</option>
                            <option value="12">12年</option>
                            <option value="13">13年</option>
                            <option value="14">14年</option>
                            <option value="15">15年</option>
                            <option value="16">16年</option>
                            <option value="17">17年</option>
                            <option value="18">18年</option>
                            <option value="19">19年</option>
                            <option value="20">20年以上</option>

                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            个人介绍(请不要超过60个汉字)
                        </label>
                        <input type="text" id="Selfbrief" value="" style="width: 500px;" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            工作日
                        </label>
                        <input name="workdays" type="checkbox" value="2" checked />周一
                        <input name="workdays" type="checkbox" value="3" checked />周二
                        <input name="workdays" type="checkbox" value="4" checked />周三
                        <input name="workdays" type="checkbox" value="5" checked />周四
                        <input name="workdays" type="checkbox" value="6" checked />周五
                        <input name="workdays" type="checkbox" value="7" checked />周六
                        <input name="workdays" type="checkbox" value="1" checked />周日
                    </div>
                    <div class="mi-form-item" >
                        <label class="mi-label">
                            平日工作时间
                        </label>
                        <span>
                        上班时间：<select id="worktimestar" class="mi-input" style=" width:60px;">
                            <option value="1">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9" selected>9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                        </select>点 --   
                        下班时间：<select id="worktimeend" class="mi-input" style=" width:60px;">
                            <option value="1">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9" >9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17" selected>17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                        </select>点
                            <input type="text" id="Workdaystime" value="上午9点到下午18点" class="mi-input" style=" display:none;" /></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            周末工作时间
                        </label>
                        <span>
                        上班时间：<select id="workendtimestar" class="mi-input" style=" width:60px;">
                            <option value="1">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9" selected>9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                        </select>点  --  
                        下班时间：<select id="workendtimeend" class="mi-input" style=" width:60px;">
                            <option value="1">0</option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9" >9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17" selected>17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                        </select>点
                            <input type="text" id="Workendtime" value="上午9点到下午18点" class="mi-input" style=" display:none;" /></span>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        工作单位信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            公司名称</label>
                        <input type="text" id="companyname" value="<%=companyname %>" readonly class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            渠道设置</label>
                        <label>
                            <input type="radio" value="0" name="radchannelsource" checked="checked">所属门店</label>
                        <label>
                            <input type="radio" value="1" name="radchannelsource">合作公司</label>
                    </div>
                    <div class="mi-form-item">
                        <select id="selchannels" class="mi-input">
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            权限设置</label>
                        <span id="tdgroups"></span>
                    </div>
                    <%--  <div class="mi-form-item">
                        <label class="mi-label">
                            办公地址</label>
                        <input type="text" id="Workaddress" value="" readonly class="mi-input" />*
                    </div>--%>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            在职状态</label>
                        <select id="selemployerstate" class="mi-input">
                            <option value="1">在职</option>
                            <option value="0">离职</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            前台列表是否显示</label>
                        <select id="peoplelistview" class="mi-input">
                            <option value="1">显示</option>
                            <option value="0">不显示</option>
                        </select>
                    </div>
                     <div class="mi-form-item">
                        <label class="mi-label">
                            教练绑定指定产品</label>
                        <input type="text" id="bindingproid" value="" class="mi-input" />
                    </div>

                    <div class="mi-form-explain">
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        二维码名片</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            电子邮箱</label>
                        <input type="text" id="Email" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item" style="display: none;">
                        <label class="mi-label">
                            主页网址</label>
                        <input type="text" id="Homepage" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            微博网址(选填)</label>
                        <input type="text" id="Weibopage" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            QQ号</label>
                        <input type="text" id="QQ" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            微信号</label>
                        <input type="text" id="Weixin" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                    <% if (weidian_url != "")
                       { %>
                    <%--  <div class="mi-form-item"  >
                        <label class="mi-label">
                            个人微店二维码</label>
                        <img src="/ui/pmui/ETicket/showtcode.aspx?pno=<%=weidian_url %>" />
                    </div>--%>
                    <%} %>
                </div>
                <table border="0" width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  " class="mi-input" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_staffid" value="<%=staffid %>" />
    <input type="hidden" id="hid_viewgroupids" value="" />
</asp:Content>
