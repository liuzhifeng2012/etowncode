<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MaterialList.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MaterialList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //弹出二维码大图
        function referrer_ch1(MaterialId, pixsize) {
            $("#hid_MaterialId").val(MaterialId);
            referrer_ch2(pixsize, 1);
            $("#proqrcode_rhshow").show();
        };
        //弹出二维码大图
        function referrer_ch2(pixsize, qrcodetype) {

            var MaterialId = $("#hid_MaterialId").trimVal();

            var comid = $("#hid_comid").trimVal();
            //生成素材永久二维码，保存入数据库
            $("#img1").attr("src", "/Images/defaultThumb.png");

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=editwxqrcode", { MaterialId: MaterialId, productid: 0, onlinestatus: 1, channelid: 0, qrcodeid: 0, comid: $("#hid_comid").trimVal(), qrcodename: "系统生成素材id:" + MaterialId, promoteact: 0, promotechannelcompany: 0 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("#img1").attr("src", data.qrcodeurl);
                }
            })
            //生成录音临时二维码(有效期7天),保存入缓存，缓存1天
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=createtempwxqrcode", { MaterialId: MaterialId,comid: $("#hid_comid").trimVal()}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    $("#img2").attr("src", data.qrcodeurl);
                }
            })
        };

        function closebtn() {
            $("#img1").attr("src", "/Images/defaultThumb.png")

            $("#proqrcode_rhshow").hide();
            $("#hid_MaterialId").val("0");
        }



        $(function () {

            var promotetypeid = $("#hid_promotetypeid").trimVal();

            //            $('input[name="radpromotetype"][value=' + promotetypeid + ']').attr("checked", true);


            //动态获取全部微信素材类型
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {

                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $.prompt("操作出现错误" + data.msg);
                    return;
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        $("#tdgroups").html("");

                        var groupstr = "素材类型：";
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == $("#hid_promotetypeid").trimVal()) {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '" checked="checked"  typename="' + data.msg[i].TypeName + '">' + data.msg[i].TypeName + '</label>';

                            } else {
                                groupstr += '<label><input name="radpromotetype"  type="radio" value="' + data.msg[i].Id + '" typename="' + data.msg[i].TypeName + '">' + data.msg[i].TypeName + '</label>';
                            }
                        }
                        $("#tdgroups").html(groupstr);
                    }

                }
            });



            $('input[name="radpromotetype"]').live("click", function () {
                var issuetype = $('input:radio[name="radpromotetype"]:checked').trimVal();
                window.open("Materiallist.aspx?promotetypeid=" + issuetype, target = "_self");
            });



            //验票明细列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }
                SearchList(pagee, promotetypeid);
            })

            //点击Enter键触发登录
            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#Search").click();
                }
            });

            var pagee = 1;
            var pageSize = 10; //每页显示条数
            SearchList(pagee, promotetypeid);



            function SearchList(pageindex, promotetypeid) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=pagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, applystate: 10, promotetypeid: promotetypeid, key: $("#key").val() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询微信素材列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, promotetypeid);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, promotetypeid) {
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

                        SearchList(page, promotetypeid);

                        return false;
                    }
                });
            }

        })
        function delmaterial(materialid) {
            if (confirm("确认删除此素材信息吗？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=delmaterial", { materialid: materialid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除素材信息出错" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除素材信息成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "materiallist.aspx";
                            }
                        });
                    }
                });
            } else {
                alert("你取消了删除操作");
            }
        }
        function insnewmaterial() {
            var issuetype = $('input:radio[name="radpromotetype"]:checked').trimVal();
            window.open("Materialdetail.aspx?promotetypeid=" + issuetype, target = "_self");
        }
        function sortmaterial() {
            var issuetype = $('input:radio[name="radpromotetype"]:checked').trimVal();
            window.open("Materialsort.aspx?promotetypeid=" + issuetype, target = "_self");
        }

        function forwardingset(materialid) {
            alert(materialid);
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=frowardingset", { materialid: materialid, comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("设定失败，" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    $.prompt("设定成功", {
                        buttons: [{ title: '确定', value: true}],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) {
                            if (v == true)
                                location.href = "materiallist.aspx";
                        }
                    });
                }
            });

        }

        function forwardingsetcannel(materialid) {

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=frowardingsetcannel", { materialid: materialid, comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("取消失败，" + data.msg);
                    return;
                }
                if (data.type == 100) {
                    $.prompt("取消成功", {
                        buttons: [{ title: '确定', value: true}],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) {
                            if (v == true)
                                location.href = "materiallist.aspx";
                        }
                    });
                }
            });

        }
        //----此部分是素材类型部分BEGIN---//

        function EditArticleType(isupdate) {
            if (isupdate == 1) {//修改素材类型
                var typeid = $("input:radio[name='radpromotetype']:checked").trimVal();

                if (typeid == "") {
                    $.prompt("请选择需要修改的素材类型");
                    return;
                }
                //                var typename = $("input:radio[name='radpromotetype']:checked").attr("typename");
                //                $("#txt_typename").val(typename);
                //                $("#hid_typeid").val(typeid);
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getmaterialtype", { id: typeid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("查询素材类型失败");
                        return;
                    }
                    if (data.type == 100) {
                        $("#hid_typeid").val(data.msg.Id);
                        $("#txt_typename").val(data.msg.Typename);

                        var isshow = 'true';
                        if (data.msg.Isshowpast == false) {
                            isshow = 'false';
                        }
                        $("#selisshowpast").val(isshow);
                    }
                })

            } else { //添加素材类型
                $("#txt_typename").val("");
                $("#hid_typeid").val("0");
            }
            $("#div_articaltype").show();
        }
        function DelArticleType() {
            var typeid = $("input:radio[name='radpromotetype']:checked").trimVal();

            if (typeid == "") {
                $.prompt("请选择需要删除的素材类型");
                return;
            }
            if (confirm("删除文章文章类型后，该类型下发布的文章也会删除，是否确认删除？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=delmaterialtype", { typeid: typeid, comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        alert("删除成功");
                        location.reload();
                    }
                    if (data.type == 1) {
                        alert("删除失败");
                        return;
                    }
                })
            } else {
                return;
            }
        }
        function changearticaltype() {
            var typeid = $("#hid_typeid").val();
            var typename = $("#txt_typename").val();

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=editmaterialtype", { isshowpast: $("#selisshowpast").val(), comid: $("#hid_comid").trimVal(), id: typeid, typename: typename, typeclass: "detail" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("操作错误");
                    return;
                }
                if (data.type == 100) {
                    $("#txt_typename").val("");
                    $("#hid_typeid").val("0");

                    $.prompt("操作成功", {
                        buttons: [
                                 { title: '确定', value: true }
                                ],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) {
                            if (v == true)
                                location.href = "MaterialList.aspx";
                        }
                    });

                }
            })
        }

        function articaltypecancel() {
            $("#txt_typename").val("");
            $("#hid_typeid").val("0");
            $("#div_articaltype").hide();
        }
        //----此部分是素材类型部分END---//

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="materiallist.aspx" onfocus="this.blur()"><span>文章列表</span></a></li>

                <li ><a href="AuthorFocus.aspx" onfocus="this.blur()"><span>关注作者管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table>
                    <tr>
                        <td width="80%" id="tdgroups" style="white-space: normal;">
                        </td>
                        <td>
                            <a href="javascript:void(0)" onclick="EditArticleType(1)">修改文章类型</a> <a href="javascript:void(0)"
                                onclick="EditArticleType(0)">添加文章类型</a>
                            <%if (iscandelmaterialtype == 1)
                              {
                            %>
                            <a href="javascript:void(0)" onclick="DelArticleType(0)">删除文章类型</a>
                            <%
                                }
                              else
                              {
                            %>
                            <label>
                                删除文章类型
                            </label>
                            <%
                                } %>
                        </td>
                    </tr>
                </table>
                <div style="text-align: center;">
                    <label>
                        请输入关键词
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="  查  询  " style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <h3>
                    <label>
                        文章列表</label></h3>
                <h3>
                    <label style="float: right">
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="insnewmaterial()">添加新文章</a>
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="sortmaterial()">文章排序</a></label>
                </h3>
                <table border="0" width="760" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            文章id
                        </td>
                        <td width="13%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="left">
                                标题
                            </p>
                        </td>
                        <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                            <p align="center">
                                作者
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                封面
                            </p>
                        </td>
                        <td width="12%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                关键词
                            </p>
                        </td>
                        <td width="5%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                使用状态
                            </p>
                        </td>
                        <td width="6%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                管 理
                            </p>
                        </td>
                        <td width="14%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td class="sender item">
                            <p align="center">
                                ${MaterialId}</p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="center">
                                ${Author}
                            </p>
                        </td>
                        <td  height="26" class="sender item">
                            <p align="center">
                               <img alt=""   id="headPortraitImg" src="${Imgpath}"  width="50px"  height="30px"/>  </p>
                        </td>
                      
                     
                        <td height="26" class="sender item">
                            <p align="center">
                                ${Keyword}</p>
                        </td>
                          <td width="4%" height="26" class="sender item">
                            <p align="center">
                                ${Applystate}</p>
                        </td>
                        <td  height="26" class="sender item">
                         <p align="center">
                           <a href="materialdetail.aspx?materialid=${MaterialId}"  >编辑</a>  <a   href="javascript:void(0)" onclick="delmaterial('${MaterialId}')" >删除</a>
                           </p>
                        </td>
                        <td  height="26" class="sender item">
                         <p align="center">
                           <a href="../ui/crmui/ForwardingCount.aspx?wxid=${MaterialId}"  >(${Forcount})查看转发统计</a>{{if Forset==0}} <input name="ForwardingSet" type="button"  onclick="forwardingset('${MaterialId}')" value="设定为转发活动" style="width: 100px;height: 26px;">{{else}}<input name="ForwardingsetCannel" type="button"  onclick="forwardingsetcannel('${MaterialId}')" value="取消设定" style="width: 70px;height: 26px;">{{/if}}

                            <span id="span1" style="color:blue;cursor: pointer;"   onclick="referrer_ch1('${MaterialId}',150)">二维码</span>   
                         </p>
                        </td>
                    </tr>
    </script>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    退 量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enter" name="Enter" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="div_articaltype" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">文章类型设置
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    类型名称：
                    <input type="text" id="txt_typename" value="" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    是否显示往期：
                    <select id="selisshowpast">
                        <option value="true">显 示</option>
                        <option value="false">不显示</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_typeid" value="" />
                    <input id="Button1" type="button" class="formButton" onclick="changearticaltype()"
                        value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="articaltypecancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="hid_promotetypeid" value="<%=promotetypeid %>" />
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_MaterialId" value="" />
        <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center">
                    <span style="font-size: 14px;">素材微信二维码</span>
                </td>
                <td align="center">
                    <span style="font-size: 14px;">录入语音二维码</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img1" height="150" width="150" />
                </td>
                <td align="center">
                    <img src="/Images/defaultThumb.png" id="img2" height="150" width="150" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <label>
                        *二维码尺寸请按照43像素的整数倍缩放，以保持最佳效果</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <a href="javascript:void(0)" onclick="closebtn()">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
