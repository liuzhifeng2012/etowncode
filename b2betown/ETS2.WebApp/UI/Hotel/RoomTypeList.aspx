<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomTypeList.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.UI.HotelUI.RoomTypeList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

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
                    url: "/JsonFactory/ProductHandler.ashx?oper=roomtypepagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {

                            $("#tblist").html("查询数据有误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
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



            //调整产品状态：上线；下线
            $("#cancel_rh").click(function () {

                $("#rhshow").hide();
            })



            $("#submit_rh").click(function () {

                var proid = $("#hid_proid").trimVal();
                if (proid == "") {
                    $.prompt("操作出现错误");
                    return;
                }

                var pro_state = $('input:radio[name="radchannelsource"]:checked').trimVal(); //  0 下线，   1 上线

                $.post("/JsonFactory/ProductHandler.ashx?oper=modifyprostate", { prostate: pro_state, proid: proid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        window.location.reload();
                }

            })


            //录入，关闭录入电子票窗口
            $("#cancel_luru").click(function () {
                $("#lurueticket").hide();
            })


            //如电子码提交
            $("#submit_luru").click(function () {

                var proid = $("#hid_luru_proid").trimVal();
                if (proid == "") {
                    $.prompt("操作出现错误");
                    return;
                }

                var eticketkey = $("#eticketkey").trimVal(); //  电子码
                var leixing = $("#leixing").trimVal(); //  电子码

                if (eticketkey == "") {
                    $.prompt("请填写电子码");
                    return;
                }
                if (leixing == "") {
                    $.prompt("传递参数错误，请刷新重新提交");
                    return;
                }
                $.post("/JsonFactory/ProductHandler.ashx?oper=lurueticket", { key: eticketkey, leixing: leixing, proid: proid, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#eticketkey").val("");
                        $("#hid_luru_proid").val("")
                        $("#lurueticket").hide();

                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#eticketkey").val("");
                        $("#hid_luru_proid").val("")
                        $("#lurueticket").hide();

                        $.prompt("操作成功" + data.msg, {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        window.location.reload();
                }

            })


        })


        //弹出产品状态修改兰
        function referrer_ch(proid) {
            $("#hid_proid").val(proid);
            $("#span_rh").text("产品状态调整:");
            $("#rhshow").show();
        };

        //弹出录入电子票兰
        function referrer_luru(proid) {
            $("#hid_luru_proid").val(proid);
            $("#lurutitle").text("录入电子票:");
            $("#submit_luru").val("确定录入电子票");
            $("#leixing").val("1");
            $("#lurueticket").show();
        };


        //弹出录入电子票兰
        function referrer_zuofei(proid) {
            $("#hid_luru_proid").val(proid);
            $("#lurutitle").text("作废电子票:");
            $("#submit_luru").val("确定，作废");
            $("#leixing").val("0");
            $("#lurueticket").show();
        };
        
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductAdd.aspx" onfocus="this.blur()" target=""><span>添加产品</span></a></li>
                <li class="on"><a href="/UI/hotel/RoomTypeList.aspx" onfocus="this.blur()" target="">
                    <span>客房管理</span></a></li>
                <li><a href="/UI/hotel/RoomTypeEdit.aspx" onfocus="this.blur()" target=""><span>添加房型
                </span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    房型列表</h3>
                <h4>
                    <a style="float: right" href="RoomTypeSort.aspx">点击对房型排序</a></h4>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="130">
                            <p align="left">
                                房型名称
                            </p>
                        </td>
                        <td width="55">
                            <p align="left">
                                床型
                            </p>
                        </td>
                        <td width="44">
                            <p align="left">
                                Wifi
                            </p>
                        </td>
                        <td width="25">
                            <p align="left">
                                上线状态
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                提交日期
                            </p>
                        </td>
                        <td width="71">
                            <p align="left">
                                &nbsp;</p>
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
                        <td>
                            <p align="left">
                                ${ID}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${RoomtypeName}
                            </p>
                        </td>
                        
                        <td>
                            <p align="left">
                                ${BedType}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Wifi}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${WhetherAvailabel}    
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${ChangeDateFormat(CretateTime)}</p>
                        </td>
                       
                        <td>
                            <p align="left">
                                <a href="RoomTypeEdit.aspx?roomtypeid=${ID}">修 改</a>  &nbsp;
                                
                                <!--<a href="javascript:void(0)" onclick="referrer_ch(${Id})">调整状态</a>-->
                            </p>
 
                        </td>
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    产品状态:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <label>
                        <input type="radio" value="1" name="radchannelsource">上 线</label>
                    <label>
                        <input type="radio" value="0" name="radchannelsource">下 线</label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_proid" value="" />
                    <input type="hidden" id="leixing" value="" />
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="lurueticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 400px; height: 300px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="lurutitle"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    录入电子码:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <textarea name="eticketkey" id="eticketkey" cols="40" rows="10"></textarea><br>
                    电子码默认每个电子码 1张票，录入时每个电子码一行
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_luru" type="button" class="formButton" value="  确定录入电子码  " />
                    <input name="cancel_luru" id="cancel_luru" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
