<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TwoCodeList.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ETicket.TwoCodeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" type="text/css" href="http://www.etown.cn/web-test/weixin/css4/css4.css" />
    <title>易城卡-电子凭证列表信息</title>
    <script type="text/javaScript">
        //进入详情页
        function getCardDetail(userId, passId) {
            window.location.href = "http://test.etown.cn/ui/pmui/eticket/twocodedetail.aspx?pno=1";
        }
        //按账号管理按钮，进入账号管理界面
        function accManage() {
            //            window.location.href = 'http://www.etown.cn/web-test/weixin/manage.html';
            window.location.href = 'http://test.etown.cn/ui/pmui/eticket/twocodelist.aspx';
        }
    </script>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/Styles/base2.css" />
    <link rel="stylesheet" type="text/css" href="/Styles/common.css" />
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <link href="/Scripts/Impromptu.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <%-- <script type="text/javascript">

        var pageSize = 10; //每页显示条数
        $(function () {
           
            var uid = $("#hid_uid").trimVal();

            SearchList(1);

            //特定用户列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=pagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, eclass: eclass, proid: proid, jsid: jsid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询验票明细错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                
                            }


                        }
                    }
                })


            }

      
        })
    </script>--%>
</head>
<body>
    <div id="header">
        <!-- <span class="left btn_back">返回</span> -->
        <span id="setAccountInfo" class="left btn_set" onclick="accManage();"></span>我的易城卡包<!-- <span class="right btn_more">更多</span> -->
    </div>
    <tbody id="tblist">
    </tbody>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
       <section onclick="getCardDetail('51d565e145cea08c341dac4a','51d565e145cea08c341dac4d')"
         class="section1">
			<header>
				<span class="status_list_03">状态：已使用</span>
					<img  src='http://www.etown.cn/web-test/weixin/img/51c9cc8445ce6a9318020791,100_35.png' class="card_logo" alt="">成人门票
			</header>
		    <article>
			   <p><f effect='4c000000,0,-1'  size='21' rowSpace='5' maxRows='2' face='DIN-Regular'>成人门票一张								
			   </p>
		    </article>
		    <time><f effect='66ffffff,0,1' size='12' face='DIN-Regular'>2013.07.06								
		    </time>
		</section>
    </script>
    <input type="hidden" id="hid_uid" value="<%=uid %>" />
</body>
</html>
