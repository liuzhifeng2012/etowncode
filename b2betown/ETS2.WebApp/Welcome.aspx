<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/ui/etown.master" CodeBehind="Welcome.aspx.cs" Inherits="ETS2.WebApp.Welcome" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        $(function () {
            //            if ($("#hid_comid").trimVal() == 101) {
            $("#setting-home").show();

            //获得登录账户信息
            $.post("/JsonFactory/UserHandle.ashx?oper=GetAccountInfo", { id: $("#hid_userid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    //                        $.prompt("获得登录账户信息出错");
                    window.open("/logout.aspx", target = "_self");
                    return;
                }
                if (data.type == 100) {
                    $("#Span19").html(data.msg[0].Accounts);
                    $("#span20").html(data.msg[0].MasterName);
                    $("#span21").html(data.msg[0].CompanyName);
                    $("#span22").html(data.msg[0].CompanyName + "(" + data.msg[0].ChannelCompanyName + ")");
                }
            })
            //            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="body">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%-- <li class="on"><a href="javascript:void(0)" onFocus="this.blur()" target="right-main">
                     </a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone" style="display: none">
            <div class="inner">
               <div id="yui_3_1_0_1_140601287152821" class="main homepage_wrapper yui3-widget yui3-app yui3-app_view">
        <div class="hello">
            <span class="hello_left">
            	            		 尊敬的<span id="Span19">weilvxing</span>，您好！
                            </span>	
            
                          
	             <p>
	            	
	            </p>
        </div>
		
                <div class="unread_mail">
            	    <span class="ico_mail_not"></span>
                    <a href="manage.aspx">
            	    点击查看我的统计数据&nbsp;
            	    </a>
				</div>


        <table class="features" cellpadding="0" cellspacing="0">
            <tbody><tr>
                <th colspan="2">设置流程</th>
            </tr>
			<tr>
                <td>
                    <p class="features_p1">
                        <span class="netdisk icons"></span>
                        <a href="javascript:;" class="ui-exec exec-mail-netfs item" onfocus="this.blur()">
                        	第一步 设置商家信息 
                        </a>
                        <br>
                        可上传和保存超大文件，支持断点续传和下载
                    </p>
                </td>
                
            </tr>
            <tr>
                <td>
                    <p class="features_p1" style="border-bottom: 0px">
                        <span class="pop_account icons"></span>
                        <a href="javascript:;" class="ui-exec exec-mail-othermail" onfocus="this.blur()">
                        	代收邮件
                        </a>
                        <br>
                        代收您的其他邮箱收到的邮件
                    </p>
                </td>
               
            </tr>
        </tbody></table>
    <div class="yui3-app_view-content" id="yui_3_1_0_1_140601287152823"></div></div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <%--    <asp:Label ID="lable1" runat="server"></asp:Label>--%>
</asp:Content>
