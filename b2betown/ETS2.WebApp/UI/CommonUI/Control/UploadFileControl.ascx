<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadFileControl.ascx.cs"
    Inherits="ETS2.WebApp.UI.CommonUI.Control.UploadFileControl" %>
<link href="/Scripts/jquery.uploadify-v2.1.4/uploadify.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
<script src="/Scripts/jquery.uploadify-v2.1.4/swfobject.js" type="text/javascript"></script>
<script src="/Scripts/slvUploader.js" type="text/javascript"></script>
<script type="text/javascript">
            var decToHex = function(str) {
                var res=[];
                for(var i=0;i < str.length;i++)
                    res[i]=("00"+str.charCodeAt(i).toString(16)).slice(-4);
                return "\\u"+res.join("\\u");
            }
            var hexToDec = function(str) {
                str=str.replace(/\\/g,"%");
                return unescape(str);
            }

          $(document).ready(function () {

              var fileType = '<%=fileType %>';
              var fileDesc = '<%=fileDesc %>';
              var fileSize = '<%=fileSize %>';
              var displayImgId = '<%=displayImgId %>';
              var displayImgNameId = '<%=displayImgNameId %>';

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
                  'onSelect': function (event, queueID, fileObj) {

                      //                      alert("唯一标识:" + queueId + "\r\n" +
                      //                           "文件名：" + fileObj.name + "\r\n" +
                      //                           "文件大小：" + fileObj.size + "\r\n" +
                      //                           "创建时间：" + fileObj.creationDate + "\r\n" +
                      //                           "最后修改时间：" + fileObj.modificationDate + "\r\n" +
                      //                          "文件类型：" + fileObj.type
                      //                      );
                      //                      if (fileType == "*") {
                      //                          return ture;
                      //                      }
                      //                      else if (fileType.indexOf(fileObj.type) < 0) {
                      //                          alert('类型' + fileObj.type + '不符合上传类型' + fileDes);
                      //                          return false;
                      //                      }
                      //                      else if (fileObj.size > fileSize) {
                      //                          alert('文件大小不能超过' + fileSize / 1000 + 'k');
                      //                          return false;
                      //                      }
                  },
                  'onCheck': function (event, checkScript, fileQueue, folder, single) {
                      //var v = 1;
                  },
                  'onComplete': function (event, queueID, fileObj, response, data) {
                      //var arrUrl = window.location.href.split('/');
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

                                  $("#<%=hidFileObject.ClientID %>").val(response);
                                  //$("#fileQueue").html(fileObj.name + " 完成！");
                                  $("#<%=hidFileUploadId.ClientID %>").val(obj[0].Id);
                                  //$("#<%=hidFilePath.ClientID %>").val(imgRootUrl + "/UploadFile/" + arrResult[1]);
                                  $("#<%=hidFilePath.ClientID %>").val(fileRootUrl+ obj[0].FilePath);
                                  $("#<%=hidFileName.ClientID %>").val(obj[0].FileName);
                                  if (displayImgId != '' && $("#" + displayImgId) != undefined) {//显示图片
                                      $("#" + displayImgId).attr("src", $("#<%=hidFilePath.ClientID %>").val());
                                  }
                                  if (displayImgNameId != '' && $("#" + displayImgNameId) != undefined) {//显示图片名
                                      $("#" + displayImgNameId).html($("#<%=hidFileName.ClientID %>").val());
                                  }
                                   
                                  //$("#image").css("display", "block").attr("src", $("#hidFilePath").val());

//                                  try {
//                                      UploadSuccessHandler("<%=uploadify.ClientID %>"); //上传成功回调函数
//                                  }
//                                  catch (e) {
//                                  }


                                  ///////////////新上传的文件对象赋值给隐藏控件//////////////////

//                                  var vString = "{\"Id\":" + "\"" + obj[0].Id + "\",";
//                                  vString += "\"OrigenalName\":" + "\"" + obj[0].FileName + "\",";
//                                  vString += "\"RelativePath\":" + "\"" + obj[0].FilePath + "\"}";

//                                  var vData = $("#" + '<%= hideResult.ClientID%>').val();
//                                  var objData = eval(vData);



//                                  vData = vData.replace(']', ',') + vString + "]";
//                                  objData = eval(vData);

//                                  $("#" + '<%= hideResult.ClientID%>').val(vData); //新上传的文件对象赋值给隐藏控件

                                  if(!$('.displayCheckEdit').is(":hidden")){
                                 
                                  //var path = '"{"Id":35822,"OrigenalName":"286960464.jpg","RelativePath":"HotelProduct/2013/05/07/1256787802.jpg","ExtentionName":".jpg","Type":0,"ObjId":0,"ObjType":23,"Creator":"16329","CreationIp":"127.0.0.1"}"';
                                  var path = "{\"Id\":" + "\"" + obj[0].Id + "\",";
                                  path += "\"OrigenalName\":" + "\"" + obj[0].FileName + "\",";
                                  path += "\"RelativePath\":" + "\"" + obj[0].FilePath + "\",";
                                  path += "\"ObjId\":" + "\"" + obj[0].ObjId + "\",";
                                  path += "\"ObjType\":" + "\"" + obj[0].ObjType + "\",";
                                  path += "\"Type\":" + "\"" + obj[0].Type + "\",";
                                  path += "\"Creator\":" + "\"" + obj[0].Creator + "\",";
                                  path += "\"CreationIp\":" + "\"" + obj[0].CreationIp + "\"}";

                                  <%= this.ClientID %>_Uploaded(path);//往编辑框里，添加新上传的文件信息
                                  
                                  }
                                  //////////////////////////////////////////////////////////////

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
                  //'multi': true, //是否允许同时上传多文件，默认false
                  //'simUploadLimit': 5, //多文件上传时，同时上传文件数目限制
                  'sizeLimit': fileSize, //限制文件大小
                  'fileDesc': fileDesc, //出现在上传对话框中的文件类型描述
                  'fileExt': fileType //控制可上传文件的扩展名，启用本项时需同时声明fileDesc

              });


              //根据上传文件自增Id，删除文件
              function DeleteFileById(vFileUploadId) {
                  //var vHidFileUploadId = $("#hidFileUploadId").val(); //上传Id
                  //var vHidFilePath = $("#hidFilePath").val(); //附件相对路径

                  if (vFileUploadId == 0 || vFileUploadId == "") {
                      alert("objId不能为0");
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

              //              //页面定义一个此名称的按钮，hidFileUploadId赋值即可
              //              $("#btnDeleteById").click(function () {
              //                  DeleteFileById($("#<%=hidFileUploadId.ClientID %>").val());
              //              });

              //              //页面定义一个此名称的按钮，hidObjId和hidObjType赋值即可
              //              $("#btnDelete").click(function () {
              //                  //DeleteFile($("#hidObjId").val(), $("#hidObjType").val());
              //                  DeleteFile(objFile.ObjId, objFile.ObjType);
              //              });

              $("#<%=DeleteBtnId %>").click(function () {
                  DeleteFileById($("#<%=FileUploadId_ClientId %>").val());
              });
          });

</script>
<div class='edit-edit-hook slvUpload-hook'>
    <%--         <div id="fileList">
            <p>ssssdd1.jpg<input id="Checkbox1" type="checkbox" data='' /></p>
            <p>ssssdd2.jpg<input id="Checkbox2" type="checkbox" data='' /></p>
            <p>ssssdd3.jpg<input id="Checkbox3" type="checkbox" data='' /></p>

            
         </div>--%>
    <div class="uploadPannel edit-hook displayCheckEdit" id="dvUploadPannel_<%= this.ClientID %>"
        style='<%=DisplayCheckEdit?"display:block": "display:none"%>'>
    </div>
    <div class="upload">
        <input type="hidden" value="" class='<%= this.ClientID %>_hideResult uploadResult'
            runat="server" id="hideResult" />
        <input type="file" name="uploadify" id="uploadify" runat="server" />
        <span id="fileQueue" runat="server"></span>
        <%--<a href="javascript:$('#uploadify').uploadifyUpload()">上传</a>| 
         <a href="javascript:$('#uploadify').uploadifyClearQueue(); window.opener.unlockui("+""+")"> 取消上传</a>--%>
        <span id='spanMessage'></span>
        <!--自增Id-->
        <input id="hidFileUploadId" type="hidden" runat="server" />
        <!--返回的相对路径-->
        <input id="hidFilePath" type="hidden" runat="server" />
        <!--文件原名-->
        <input id="hidFileName" type="hidden" runat="server" />
        <!--文件原名-->
        <input id="hidFileObject" type="hidden" runat="server" />
    </div>
</div>
<%--<div>
      <img id="image" src="" alt="" />
     </div>--%>
<%--<script type="text/javascript">
        var <%= this.ClientID %>_slvUploader = new slvUploader('dvUploadPannel_<%= this.ClientID %>','<%=this.hideResult.ClientID %>','',2);
        function <%= this.ClientID %>_Uploaded(path,name){
            <%= this.ClientID %>_slvUploader.onUploaded(path,name);
        }
</script>--%>
