<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiUploadFileControl.ascx.cs"
    Inherits="ETS2.WebApp.UI.CommonUI.Control.MultiUploadFileControl" %>
<link href="/Scripts/jquery.uploadify-v2.1.4/uploadify.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
<script src="/Scripts/jquery.uploadify-v2.1.4/swfobject.js" type="text/javascript"></script>
<script src="/Scripts/slvUploader.js" type="text/javascript"></script>
<script type="text/javascript">
    var decToHex = function (str) {
        var res = [];
        for (var i = 0; i < str.length; i++)
            res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4);
        return "\\u" + res.join("\\u");
    }
    var hexToDec = function (str) {
        str = str.replace(/\\/g, "%");
        return unescape(str);
    }
    $(document).ready(function () {
        $("#<%=hidFileObject.ClientID %>").val("");
        $("#<%=hidFileUploadId.ClientID %>").val("");
        $("#<%=hidFilePath.ClientID %>").val("");
        $("#<%=hidFileName.ClientID %>").val("");
        $("#divPreview").html("");

        var IsMultiUpload = '<%=IsMultiUpload %>';
        if (IsMultiUpload == 'True'||IsMultiUpload=='true') {
            IsMultiUpload = true;
        } else {
            IsMultiUpload = false;
        }

        var fileType = '<%=fileType %>';
        var fileDesc = '<%=fileDesc %>';
        var fileSize = '<%=fileSize %>';


        var objFile = {};
        objFile.OptionFlag = "UploadFile";
        objFile.ObjId = '<%=uploadFileInfo.ObjId %>';
        objFile.ObjType = '<%=uploadFileInfo.ObjType %>';
        objFile.FileType = fileType;
        objFile.FileSize = fileSize;
        objFile.CreatorId = '<%=CreatorId %>';
        $("#<%=uploadify.ClientID %>").uploadify({
            //                  'buttonText': 'Upload File',
            'buttonImg': '/Images/UploadFile/selectFile.png', //按钮图片路径
            'width': '55', //按钮图片的长度。默认 110
            'height': '30', //按钮图片的高度。默认 30
            'debug': true, //debug模式开/关，打开后会显示debug时的信息
            'uploader': '/Scripts/jquery.uploadify-v2.1.4/uploadify.swf', //进度条，Uploadify里面含有
            'fileDataName': 'Filedata', //文件在上传服务器脚本阵列的名称，默认值Filedata
            'script': '/UI/CommonUI/Control/UploadHandler.ashx', //一般处理程序
            //'scriptData': { 'ObjId': '<%=uploadFileInfo.ObjId %>', 'ObjType': '<%=uploadFileInfo.ObjType %>' },
            'scriptData': objFile,
            'method': 'get', //默认值'post'
            'cancelImg': '/Images/UploadFile/cancel.png', //取消图片路径
            'folder': 'UploadFile', //上传文件夹名
            'queueID': '<%=fileQueue.ClientID %>',
            'auto': true, //是否自动上传
            'onSelect': function (e, queueId, fileObj) {
                //                        alert("唯一标识:" + queueId + "\r\n" +
                //                              "文件名：" + fileObj.name + "\r\n" +
                //                              "文件大小：" + fileObj.size + "\r\n" +
                //                              "创建时间：" + fileObj.creationDate + "\r\n" +
                //                              "最后修改时间：" + fileObj.modificationDate + "\r\n" +
                //                              "文件类型：" + fileObj.type
                //                        );

            },
            'onCheck': function (event, checkScript, fileQueue, folder, single) {
                //var v = 1;
            },
            'onComplete': function (event, queueID, fileObj, response, data) {

                var fileRootUrl = '<%=fileUrl %>';
                switch (response) {
                    case "typeError":
                        $(".uploadifyQueueItem").hide();
                        alert("不符合上传类型:" + fileDesc);
                        break;
                    case "sizeError":
                        $(".uploadifyQueueItem").hide();
                        alert("文件大小不能超过:" + fileSize / 1000 + 'k');
                        break;
                    default:
                        if (response.indexOf("Exception:") >= 0) {
                            $(".uploadifyQueueItem").hide();

                        }
                        else {//上传成功 
                            var obj = eval(hexToDec(response));

                            if (IsMultiUpload==true) {
                                //                                var aData = $("#<%=hidFileObject.ClientID %>").val();
                                //                                $("#<%=hidFileObject.ClientID %>").val(aData + "," + response);

                                //                                  $("#fileQueue").html(fileObj.name + " 完成！");
                                var bData = $("#<%=hidFileUploadId.ClientID %>").val();
                                if (bData == "") {
                                    bData += obj[0].Id;
                                } else {
                                    bData += "," + obj[0].Id;
                                }
                                $("#<%=hidFileUploadId.ClientID %>").val(bData);

                                var cData = $("#<%=hidFilePath.ClientID %>").val();
                                if (cData == "") {
                                    cData += fileRootUrl + obj[0].FilePath;
                                } else {
                                    cData += "," + fileRootUrl + obj[0].FilePath;
                                }
                                $("#<%=hidFilePath.ClientID %>").val(cData);

                                var dData = $("#<%=hidFileName.ClientID %>").val();
                                if (dData == "") {
                                    dData += obj[0].FileName;
                                } else {
                                    dData += "," + obj[0].FileName;
                                }
                                $("#<%=hidFileName.ClientID %>").val(dData);

                                $("#divPreview").append('<div style="position: relative; float: left;" id="div_upimg_' + obj[0].Id + '" name="div_upimg" editid="' + obj[0].Id + '"  onmouseover="UploadImgMouseOver(this)" onmousedown="UploadImgMouseDown(this)" onmouseout="UploadImgMouseOut(this)">  <img src="' + fileRootUrl + obj[0].FilePath + '" width="80px" height="60px" />  <div class="preview-imgli bn">   <span class="pic_del">删除</span></div>  </div>');

                            } else {
                                //                                $("#<%=hidFileObject.ClientID %>").val(response);
                                //                                  $("#fileQueue").html(fileObj.name + " 完成！");
                                $("#<%=hidFileUploadId.ClientID %>").val(obj[0].Id);
                                $("#<%=hidFilePath.ClientID %>").val(fileRootUrl + obj[0].FilePath);
                                $("#<%=hidFileName.ClientID %>").val(obj[0].FileName);

                                $("#divPreview").html('<div style="position: relative; float: left;" id="div_upimg_' + obj[0].Id + '" name="div_upimg" editid="' + obj[0].Id + '"> <img src="' + fileRootUrl + obj[0].FilePath + '" width="80px" height="60px" />  <div class="preview-imgli bn">   <span class="pic_del">删除</span></div>  </div>');
                            }

                            ///////////////新上传的文件对象赋值给隐藏控件//////////////////
                            //                            var vString = "{\"Id\":" + "\"" + obj[0].Id + "\",";
                            //                            vString += "\"OrigenalName\":" + "\"" + obj[0].FileName + "\",";
                            //                            vString += "\"RelativePath\":" + "\"" + obj[0].FilePath + "\"}";

                            //                            var vData = $("#" + '<%= hideResult.ClientID%>').val();

                            //                            vData = vData.replace(']', ',') + vString + "]";

                            //                            $("#" + '<%= hideResult.ClientID%>').val(vData); //新上传的文件对象赋值给隐藏控件
                            ////////////////////////////////////////////////////////////////
                        }
                        break;
                }


            },
            'onAllComplete': function (event, data) {
                //var v2 = 2;
                //$("#fileQueue").hide();
            },
            'onError': function (event, queueID, fileObj, d) {
                //alert('上传错误');
                if (d.status == 404) {
                    $(".uploadifyQueueItem").hide();
                    alert('找不到文件');

                    return false;
                }
                else if (d.type === "HTTP") {
                    $(".uploadifyQueueItem").hide();
                    alert('error ' + d.type + ": " + d.status);

                    return false;
                }
                else if (d.type === "File Size") {
                    $(".uploadifyQueueItem").hide();
                    alert('上传的文件合计大小不能超过' + fileSize / 1000 + 'k');

                    return false;
                }
                else {
                    $(".uploadifyQueueItem").hide();
                    alert('error ' + d.type + ": " + d.info);

                    return false;
                }
            },
            'multi': IsMultiUpload, //是否允许同时上传多文件，默认false
            //             'simUploadLimit': 5, //多文件上传时，同时上传文件数目限制
            'sizeLimit': fileSize, //限制文件大小
            'fileDesc': fileDesc, //出现在上传对话框中的文件类型描述
            'fileExt': fileType //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
        });

        //如果查询对象id>0 并且 viewImgFlag!="" ，查询的图片种类
        var ObjId = '<%=uploadFileInfo.ObjId %>';
        var viewImgFlag = '<%=viewImgFlag %>';
        if (ObjId != '0' && viewImgFlag != '') {
            GetImgs(ObjId, viewImgFlag);
        }
    });

    function GetImgs(ObjId, viewImgFlag) {
        $.ajax({
            type: "GET",
            url: "/UI/CommonUI/Control/UploadHandler.ashx",
            data: "OptionFlag=" + viewImgFlag + "&objId=" + ObjId,
            cache: false,
            beforeSend: function () {

            },
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.type == 100) {
                  if(data.msg==''||data.msg==null){}
                    if (data.msg.length > 0) {
                        var fileRootUrl = '<%=fileUrl %>';

                        for (var i = 0; i < data.msg.length; i++) {
                            var bData = $("#<%=hidFileUploadId.ClientID %>").val();
                            if (bData == "") {
                                bData += data.msg[i].Id;
                            } else {
                                bData += "," + data.msg[i].Id;
                            }
                            $("#<%=hidFileUploadId.ClientID %>").val(bData);

                            var cData = $("#<%=hidFilePath.ClientID %>").val();
                            if (cData == "") {
                                cData += fileRootUrl + data.msg[i].Relativepath;
                            } else {
                                cData += "," + fileRootUrl + data.msg[i].Relativepath;
                            }
                            $("#<%=hidFilePath.ClientID %>").val(cData);

                            var dData = $("#<%=hidFileName.ClientID %>").val();
                            if (dData == "") {
                                dData += data.msg[i].OrigenalName;
                            } else {
                                dData += "," + data.msg[i].OrigenalName;
                            }
                            $("#<%=hidFileName.ClientID %>").val(dData);

                            $("#divPreview").append('<div style="position: relative; float: left;" id="div_upimg_' + data.msg[i].Id + '" name="div_upimg" editid="' + data.msg[i].Id + '"  onmouseover="UploadImgMouseOver(this)" onmousedown="UploadImgMouseDown(this)" onmouseout="UploadImgMouseOut(this)">  <img src="' + fileRootUrl + data.msg[i].Relativepath + '" width="80px" height="60px" />  <div class="preview-imgli bn">   <span class="pic_del">删除</span></div>  </div>');
                        }
                    }
                   
                }
                if (data.type == 1) { }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(XMLHttpRequest.responseText);
            }
        });
    }


    function UploadImgMouseOver(obj) {
        $(obj).find(".preview-imgli").removeClass("bn");
    }
    function UploadImgMouseOut(obj) {
        $(obj).find(".preview-imgli").addClass("bn");
    }
    function UploadImgMouseDown(obj) {
        DeleteFileById($(obj).attr("editid"));
    }

    //根据上传文件自增Id，删除文件
    function DeleteFileById(vFileUploadId) {
        //var vHidFileUploadId = $("#hidFileUploadId").val(); //上传Id
        //var vHidFilePath = $("#hidFilePath").val(); //附件相对路径

        if (vFileUploadId == 0 || vFileUploadId == "") {
            alert("删除图片id不能为0");
            return false;
        }

        $.ajax({
            type: "GET",
            url: "/UI/CommonUI/Control/UploadHandler.ashx",
            //data: "OptionFlag=DeleteFile&hidFileUploadId=" + vFileUploadId + "&hidFilePath=" + vHidFilePath,
            data: "OptionFlag=DeleteFile&fileUploadId=" + vFileUploadId,
            cache: false,
            beforeSend: function () {
                //$("#div1").css("color", "#999");
                //$("#div1").text("请稍后...");
            },
            success: function (msg) {
                if (msg == "success") {
                    var newid = $("#<%=hidFileUploadId.ClientID %>").val().replace(vFileUploadId + ",", "").replace(vFileUploadId, "");
                    $("#<%=hidFileUploadId.ClientID %>").val(newid);

                    var newpath = $("#<%=hidFilePath.ClientID %>").val().replace($("#upimgid_" + vFileUploadId).attr("src") + ",", "").replace($("#upimgid_" + vFileUploadId).attr("src"), "");
                    $("#<%=hidFilePath.ClientID %>").val(newpath);

                    var newname = $("#<%=hidFileName.ClientID %>").val().replace($("#hid_upimgname_" + vFileUploadId).val() + ",", "").replace($("#hid_upimgname_" + vFileUploadId).val(), "");
                    $("#<%=hidFileName.ClientID %>").val(newname);

                    $("#divPreview").find("#div_upimg_" + vFileUploadId).remove();

                }
                else if (msg.indexOf("Exception:") >= 0) {
                    alert(msg);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(XMLHttpRequest.responseText);
            }
        });


    }

    //根据产品Id和上传文件类型，删除文件
    function DeleteFile(objId, objType) {
        if (objId == 0) {
            alert("objId不能为0");
            return false;
        }

        $.ajax({
            type: "GET",
            url: "/UI/CommonUI/Control/UploadHandler.ashx",
            data: "OptionFlag=DeleteFileBy&objId=" + objId + "&objType=" + objType,
            //data: "OptionFlag=DeleteFile&fileUploadId=" + vFileUploadId,
            cache: false,
            beforeSend: function () {

            },
            success: function (msg) {
                if (msg == "success") {
                    $("#<%=hidFileUploadId.ClientID %>").val("");
                    $("#<%=hidFilePath.ClientID %>").val("");
                    $("#<%=hidFileName.ClientID %>").val("");
                    if (displayImgId != '' && $("#" + displayImgId) != undefined) {//去除图片显示
                        $("#" + displayImgId).attr("src", "");
                    }
                    if (displayImgNameId != '' && $("#" + displayImgNameId) != undefined) {//去除显示图片名
                        $("#" + displayImgNameId).html("");
                    }
                }
                else if (msg.indexOf("Exception:") >= 0) {
                    alert(msg);
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert(XMLHttpRequest.responseText);
            }
        });


    }

</script>
<style type="text/css">
    .multiuploadimg .preview-imgli
    {
        position: absolute;
        left: 0px;
        top: 0px;
        z-index: 999;
        filter: alpha(Opacity=80);
        -moz-opacity: 0.8;
        opacity: 0.8;
        background-color: #ffffff;
        width: 80px;
        height: 40px;
        cursor: pointer;
        text-align: center;
        padding-top: 20px;
    }
    .multiuploadimg .bn
    {
        display: none;
    }
    .multiuploadimg .pic_del
    {
        font-size: 12px;
    }
</style>
<div class='edit-edit-hook slvUpload-hook'>
    <table>
        <tr>
            <td>
                <div id="divPreview" class="multiuploadimg">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="upload">
                    <input type="hidden" value="" class='<%= this.ClientID %>_hideResult uploadResult'
                        runat="server" id="hideResult" />
                    <input type="file" name="uploadify" id="uploadify" runat="server" />
                    <span id="fileQueue" runat="server"></span><span id='spanMessage'></span>
                    <!--自增Id-->
                    <input id="hidFileUploadId" type="hidden" runat="server" />
                    <!--返回的相对路径-->
                    <input id="hidFilePath" type="hidden" runat="server" />
                    <!--文件原名-->
                    <input id="hidFileName" type="hidden" runat="server" />
                    <!--返回的json-->
                    <input id="hidFileObject" type="hidden" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</div>
