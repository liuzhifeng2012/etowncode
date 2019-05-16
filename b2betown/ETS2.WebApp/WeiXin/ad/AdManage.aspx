<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AdManage.aspx.cs" Inherits="ETS2.WebApp.WeiXin.ad.AdManage" %>


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

        };

        function closebtn() {
            $("#img1").attr("src", "/Images/defaultThumb.png")

            $("#proqrcode_rhshow").hide();
            $("#hid_Id").val("0");
        }



        $(function () {



            //查询
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }
                SearchList(pagee);
            })

            //点击Enter键触发登录
            $("body").keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#Search").click();
                }
            });

            var pagee = 1;
            var pageSize = 10; //每页显示条数
            SearchList(pagee);



            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=adpagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, key: $("#key").val() },
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
        function delad(id) {
            if (confirm("确认删除此图片广告吗？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=delad", { id: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除图片广告出错" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除图片广告成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "AdManage.aspx";
                            }
                        });
                    }
                });
            } else {
                alert("你取消了删除操作");
            }
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

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
                        图文广告列表</label></h3>
                <h3>
                    <label style="float: right">
                        <a href="javascript:void(0)" style="color: #2D65AA;" onclick="upad('0')">添加图文广告</a>
                </h3>
                <table border="0" width="760" class="mail-list-title">
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            id
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
                                ${Id}</p>
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
                          <a href="AdUpImages.aspx?id=${Id}" >上传图片</a> <a href="javascript:void(0)" onclick="upad('${Id}')"  >编辑</a>  <a   href="javascript:void(0)" onclick="delad('${Id}')" >删除</a>
                           </p>
                        </td>
                        <td  height="26" class="sender item">
                         <p align="center">
                           (浏览量：${Lookcount}  投票量：${Votecount})

                            <span id="span1" style="color:blue;cursor: pointer;"   onclick="referrer_ch1('${Id}',150)">二维码</span>   
                         </p>
                        </td>
                    </tr>
    </script>

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
  
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_Id" value="" />
        <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr>
                <td align="center">
                    <span style="font-size: 14px;">素材微信二维码</span>
                </td>
                <td align="center">
                    <span style="font-size: 14px;">网址二维码</span>
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
