<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Ad_image_up.aspx.cs" Inherits="ETS2.WebApp.UI.ShangJiaUI.Ad_image_up" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>

    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script> 

    <script type="text/javascript">
        $(function () {


            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            var adid = $("#hid_adid").trimVal();
            //            $("#linkurl").attr("disabled", "disabled");

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getwxadimages", { adid: adid, id: id }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //$.prompt("获取图片列表出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg != null) {
                        $("#hid_id").val(data.msg.Id);

                        $("#hid_Musicid").val(data.msg.Musicid);

                        $("input:radio[name='Adtype'][value=" + data.msg.Adtype + "]").attr("checked", true);

                    }
                }
            })


            $("#button").click(function () {


                var Imageid = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
                var Link = "";
                if (Imageid == "") {
                    Imageid = $("#headPortraitImg").trimVal();
                }



                $.post("/JsonFactory/WeiXinHandler.ashx?oper=Editwxadimage", { id: $("#hid_id").val(), adid: adid, Link: Link, Imageid: Imageid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("添加出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("添加成功");
                        location.href = "Ad_image_manage.aspx?adid=" + adid;
                        return;
                    }
                })
            })


        })





        //分页
        function setpage1(newcount, newpagesize, curpage) {
            $("#divPage1").paginate({
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
                    SearchFontList(page);
                    return false;
                }
            });
        }


       
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div>
                </div>
                <h3>
                  </h3>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10; display:">
                <h2 class="p-title-area"><%=Title %> - 上传图片</h2>

                   <div class="mi-form-item"  id="fonttab">
                       
                        <table>
                        <tr id="img">
                        <td class="tdHead">
                            图片：
                        </td>
                        <td>
                            <div class="C_head">
                                <dl>
                                    <dt>
                                        <input type="hidden" id="Hidden1" value="" />
                                        <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png"  style=" background-color: #0099ff"/></dt>
                                    <dd>上传图片或在图库中选择图片
                                       </dd>
                                </dl>
                                <div class="cl">
                                </div>
                            </div>
                            <div class="C_head_no">
                                <div class="C_head_1">
                                    <ul>


                                        <li style="height: 20px; margin-left: 10px;float:left;"">
                                            <div class="C_verify">
                                                <label>
                                                   </label>
                                                <span>
                                                    <uc1:uploadFile ID="headPortrait" runat="server" />
                                                </span>
                                            </div>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                        </table>
                   </div>
                   
                   <div class="mi-form-explain"></div>
                    <div class="mi-form-explain"></div>
               </div>

                <table width="780px" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确 认 提 交  " class="mi-input" >
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>


    </div>


    <input type="hidden" id="hid_id" value="<%=id %>" />
    <input type="hidden" id="hid_adid" value="<%=adid %>" />
    <input type="hidden" id="hid_Imageid" value="<%=headPortraitImgSrc %>" />
</asp:Content>
