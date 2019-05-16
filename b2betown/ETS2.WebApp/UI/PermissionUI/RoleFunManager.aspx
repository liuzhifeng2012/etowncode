<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleFunManager.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.RoleFunManager" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <link rel="stylesheet" href="/scripts/jquery.ztree-3.5/css/demo.css" type="text/css" />
    <link rel="stylesheet" href="/scripts/jquery.ztree-3.5/css/zTreeStyle/zTreeStyle.css"
        type="text/css" />
    <script type="text/javascript" src="/scripts/jquery.ztree-3.5/jquery.ztree.core-3.5.js"></script>
    <script type="text/javascript" src="/scripts/jquery.ztree-3.5/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript">
		<!--
        var setting = {
            check: {
                enable: true
            },
            data: {
                simpleData: {
                    enable: true
                }
            } 

        };

        var zNodes=<%=treestr %>;
        var code;

        function setCheck() {
            var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
			py = $("#py").attr("checked") ? "p" : "",
			sy = $("#sy").attr("checked") ? "s" : "",
			pn = $("#pn").attr("checked") ? "p" : "",
			sn = $("#sn").attr("checked") ? "s" : "",
			type = { "Y": py + sy, "N": pn + sn };
            zTree.setting.check.chkboxType = type;
            showCode('setting.check.chkboxType = { "Y" : "' + type.Y + '", "N" : "' + type.N + '" };');
        }
        function showCode(str) {
            if (!code) code = $("#code");
            code.empty();
            code.append("<li>" + str + "</li>");
        }

        $(document).ready(function () {
            $.fn.zTree.init($("#treeDemo"), setting, zNodes);
            setCheck();
            $("#py").bind("change", setCheck);
            $("#sy").bind("change", setCheck);
            $("#pn").bind("change", setCheck);
            $("#sn").bind("change", setCheck);
        });
		//-->
          function onCheck(e,treeId,treeNode){
             var treeObj=$.fn.zTree.getZTreeObj("treeDemo"),
             nodes=treeObj.getCheckedNodes(true),
    
             
             selednodeid="";//选中项的节点id
             selednodepId="";//选中项的节点的父级id
             v="";             
             for(var i=0;i<nodes.length;i++){
                v+=nodes[i].name + ",";
//                alert(nodes[i].id); //获取选中节点的值
                selednodeid+=(nodes[i].id)+",";
                selednodepId+=nodes[i].pId+",";
             }
            selednodeid = selednodeid.substr(0, selednodeid.length - 1);
            selednodepId = selednodepId.substr(0, selednodepId.length - 1);
            var groupid=$("#hid_groupid").trimVal();

              $.post("/JsonFactory/PermissionHandler.ashx?oper=DistributeAction", { groupid: groupid,selednodeid:selednodeid,selednodepId:selednodepId }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作错误");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("操作成功", { buttons: [{ title: "确定", value: true}], show: "slideDown", submit: function (m, v, e, f)
                            { window.open("rolelist.aspx", target = "_self"); } 
                            });
                            return;
                    }

                });
            
          }

    </script>
    <style type="text/css">
        #mail-main ul li
        {
            list-style-type: none;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="secondary-tabs" class="navsetting ">
        <ul>
            <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
            <li class="on"><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
            <%-- <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>--%>
        </ul>
    </div>
    <h1>
        <%=groupname %>权限分配
    </h1>
    <ul id="treeDemo" class="ztree">
    </ul>
    <input type="button" id="btn" onclick="onCheck()" value="  提  交 " />
    <input type="hidden" id="hid_groupid" value="<%=groupid %>" />
</asp:Content>
