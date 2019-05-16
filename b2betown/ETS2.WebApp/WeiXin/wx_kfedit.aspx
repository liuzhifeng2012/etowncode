<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_kfedit.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.wx_kfedit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    $(function () { 
       var kfid=<%=kfid %>;
       if(kfid>0){
       $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getwxkf",{kfid:kfid,comid:$("#hid_comid").trimVal()},function(data){
       data=eval("("+data+")");
       if(data.type==1){}
       if(data.type==100){
           $("#kfaccount").val(data.msg.Kf_account);
           $("#kfnick").val(data.msg.Kf_nick);
           $("#hid_msid").val(data.msg.Ms_id);
           $("#hid_msname").val(data.msg.Ms_name);
           $("#hid_ygid").val(data.msg.Yg_id);
           $("#hid_ygname").val(data.msg.Yg_name);
           $("#sel_isrun").val(data.msg.Isrun);
           $("#sel_iszongkf").val(data.msg.Iszongkf);

           if(data.msg.Isbinded==1)
           {
             $("#hid_isverify").val("1");
              $("#lblmsg").html("门市:"+data.msg.Ms_name+";员工:"+data.msg.Yg_name);
           }else{
            
            $("#lblmsg").html("");
            $("#lblmsg").html("当前客服号还没有绑定员工");
           }
       }
       });
       }

       $("#button").click(function(){
          if($("#hid_isverify").trimVal()=="0")
          {
            alert("当前客服号还没有绑定员工");
            return ;
          }

          $.post("/JsonFactory/WeiXinHandler.ashx?oper=bindwxdkf",{kfid:<%=kfid %>,ygid:$("#hid_ygid").trimVal(),ygname:$("#hid_ygname").trimVal(),msid:$("#hid_msid").trimVal(),msname:$("#hid_msname").trimVal(),isrun:$("#sel_isrun").trimVal(),comid:$("#hid_comid").trimVal(),createuserid:$("#hid_userid").trimVal(),iszongkf:$("#sel_iszongkf").trimVal(),isbinded:1},function(data){
           data=eval("("+data+")");
           if(data.type==1){
             alert(data.msg);
             return;
           }
           if(data.type==100){
              $.prompt("编辑成功", { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v == true) {window.open("/weixin/wx_kflist.aspx",target="_self") } } });
                            return;
           }
          })
       })
    })
    function phonesel()
    {
        var phone=$("#txtphone").trimVal();
        if(phone=="")
        {
          alert("请输入手机号");
          return ;
        }
        if(!$("#txtphone").checkMobile())
        {
          alert("手机号格式错误");
          return;
        }

        //根据手机号获得员工信息
        $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getygdetail",{phone:phone,comid:$("#hid_comid").trimVal()},function(data){
               data=eval("("+data+")");
               if(data.type==1){
                  $("#lblmsg").html("");
                  $("#lblmsg").html(data.msg);
                  $("#hid_isverify").val("0");
               }
               if(data.type==100){
                     $("#hid_msid").val(data.ms_id);
                     $("#hid_msname").val(data.ms_name);
                     $("#hid_ygid").val(data.yg_id);
                     $("#hid_ygname").val(data.yg_name);
                     $("#hid_isverify").val("1");

                    $("#lblmsg").html("");
                    $("#lblmsg").html("门市:"+data.ms_name+";员工:"+data.yg_name);
               }
       });
    }
    
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="WxAllSendMsgPage.aspx" onfocus="this.blur()"><span>微信留言</span></a></li>
                <li class="on"><a href="wx_kflist.aspx" onfocus="this.blur()"><span>微信多客服列表</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        微信多客服编辑</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            客服工号</label>
                        <input type="text" id="kfaccount" value="" class="mi-input" readonly="readonly" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            客服昵称</label>
                        <input type="text" id="kfnick" value="" class="mi-input" readonly="readonly" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            手机号(通过手机号查询需要绑定的员工)</label>
                        <input type="text" id="txtphone" value="" class="mi-input" /><input type="button"
                            id="btn1" value="  查询  " onclick="phonesel()" class="mi-input" autocomplete="off"
                            style="width: 50px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label" id="lblmsg" style="color: red;">
                        </label>
                        <input type="hidden" id="hid_msid" value="" />
                        <input type="hidden" id="hid_msname" value="" />
                        <input type="hidden" id="hid_ygid" value="" />
                        <input type="hidden" id="hid_ygname" value="" />
                        <input type="hidden" id="hid_isverify" value="0" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            状态</label>
                        <select id="sel_isrun" class="mi-input">
                            <option value="1">运行</option>
                            <option value="0">暂停</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            是否是总客服</label>
                        <select id="sel_iszongkf" class="mi-input">
                            <option value="1">是</option>
                            <option value="0">否</option>
                        </select>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table border="0" width="780" class="grid">
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" id="button" value="  提交  " class="mi-input" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
