<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B2bCrm_DengjifenManage.aspx.cs"   MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.CrmUI.B2bCrm_DengjifenManage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting">
            <ul>
                   <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">
                    会员列表</a></li>
                       <li class="on"><a href="/ui/crmui/B2bCrm_LevelManage.aspx" onfocus="this.blur()">
                    会员级别设置</a></li>
                       <li><a href="/ui/crmui/B2bCrm_jifenManage.aspx" onfocus="this.blur()">
                    会员积分设置</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        摇奖奖项</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            一等奖</label>
                        <label>
                            名称<input name="Award_title1" id="Award_title1" type="text" size="12" class="mi-input"
                                style="width: 100px;" />
                        </label>
                        <label>
                            数量
                            <input name="Award_num1" id="Award_num1" type="text" size="6" class="mi-input" style="width: 40px;" />
                        </label>
                        <select name="Award_type1" id="Award_type1" class="mi-input" style="width: 100px;">
                            <option value="0">奖品类型</option>
                            <option value="1">实物奖</option>
                            <option value="2">会员积分/积分</option>
                            <!--<option value="3">优惠券</option>-->
                        </select>
                        <label>
                            价值<input name="Award_Get_Num1" id="Award_Get_Num1" size="6" type="text" class="mi-input"
                                style="width: 50px;" />(会员积分数，优惠券ID)
                        </label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            二等奖</label>
                        <label>
                            名称<input name="Award_title2" id="Award_title2" type="text" size="12" class="mi-input"
                                style="width: 100px;" />
                        </label>
                        <label>
                            数量
                            <input name="Award_num2" id="Award_num2" type="text" size="6" class="mi-input" style="width: 40px;" />
                        </label>
                        <select name="Award_type2" id="Award_type2" class="mi-input" style="width: 100px;">
                            <option value="0">奖品类型</option>
                            <option value="1">实物奖</option>
                            <option value="2">会员积分/积分</option>
                            <!--<option value="3">优惠券</option>-->
                        </select>
                        <label>
                            价值<input name="Award_Get_Num2" id="Award_Get_Num2" size="6" type="text" class="mi-input"
                                style="width: 50px;" />(会员积分数，优惠券ID)
                        </label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            三等奖</label>
                        <label>
                            名称<input name="Award_title3" id="Award_title3" type="text" size="12" class="mi-input"
                                style="width: 100px;" />
                        </label>
                        <label>
                            数量
                            <input name="Award_num3" id="Award_num3" type="text" size="6" class="mi-input" style="width: 40px;" />
                        </label>
                        <select name="Award_type3" id="Award_type3" class="mi-input" style="width: 100px;">
                            <option value="0">奖品类型</option>
                            <option value="1">实物奖</option>
                            <option value="2">会员积分/积分</option>
                            <!--<option value="3">优惠券</option>-->
                        </select>
                        <label>
                            价值<input name="Award_Get_Num3" id="Award_Get_Num3" size="6" type="text" class="mi-input"
                                style="width: 50px;" />(会员积分数，优惠券ID)
                        </label>
                    </div>
                    <div class="mi-form-item" style="display: none;">
                        <label class="mi-label">
                            四等奖</label>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            运行状态</label>
                        <label>
                            <input name="Runstate" type="radio" value="1" checked />
                            运行中</label>
                        <label>
                            <input type="radio" name="Runstate" value="0">
                            停止运行</label>
                        (新添加项目，只有设置完成，运行后才能上线)
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="699" height="44" align="center">
                            <input type="button" name="GoActAddNext" id="GoActAddNext" value="  确  认  " class="mi-input" />
                        </td>
                        <td width="59">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>