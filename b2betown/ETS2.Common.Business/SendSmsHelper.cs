using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using ETS2.Common.Business.Wssmslink;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using System.Security.Cryptography;

namespace ETS2.Common.Business
{
    public class SendSmsHelper
    {
        private SqlHelper sqlHelper;
        public SendSmsHelper(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 限时发送HTTP请求
        /// <summary> 
        ///根据url获取网站html内容 
        /// </summary> 
        /// <param name="url">网址</param> 
        /// <returns>获取网站html内容</returns> 
        private static string GetHtmlContentByUrl(string url, int Timeout, out string msg)
        {

            var htmlContent = string.Empty;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Timeout = Timeout;
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var stream = httpWebResponse.GetResponseStream();
                if (stream != null)
                {
                    var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    htmlContent = streamReader.ReadToEnd();
                    streamReader.Close();
                    streamReader.Dispose();
                    stream.Close();
                    stream.Dispose();
                }
                httpWebResponse.Close();
                msg = "";
                return htmlContent;
            }
            catch (Exception ex)
            {
                msg = "失败" + ex.Message;
                return "";
            }
        }
        #endregion


        #region 万龙八易签名
        public static string wanlongqianming(string text, string key){
               Encoding encode = Encoding.GetEncoding("utf-8");
               byte[] byteData = encode.GetBytes(text);
               byte[] byteKey = encode.GetBytes(key);
               HMACSHA1 hmac = new HMACSHA1(byteKey);
               CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
               cs.Write(byteData, 0, byteData.Length);
               cs.Close();
               string ret = Convert.ToBase64String(hmac.Hash);
               return ret;
        }
        #endregion

        #region HTTP请求
        public static string GetHttpPost(string url, string sEncoding,string token="")
        {

            try
            {

                string authorization = "MWS" + " " + " " + ":" + token;//编辑签名
                WebClient WC = new WebClient();
                WC.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                if (token != "")
                {
                    WC.Headers.Add("Authorization", authorization);//增加签名到header头里
                }
                int p = url.IndexOf("?");
                string sData = url.Substring(p + 1);
                url = url.Substring(0, p);
                byte[] postData = Encoding.GetEncoding(sEncoding).GetBytes(sData);
                byte[] responseData = WC.UploadData(url, "POST", postData);
                string ct = Encoding.GetEncoding(sEncoding).GetString(responseData);
                return ct;

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        #endregion


        #region 碟信接口发送短信http
        /// <summary> 
        ///发送碟信接口
        /// </summary> 
        /// <param name="mobile">手机</param> 
        /// <param name="Content">发送内容,限时3000</param> 
        /// <returns>获取返回值</returns> 
        public static int SendSms(string mobile, string Content, int comid, out string msg)
        {
            if (mobile == null || mobile == "")
            {
                msg = "短信传递参数错误";
                return -9;
            }
            else {
                mobile=mobile.Replace(" ", "");
            }


            string url = "http://sms.airlead.net:28080/dxin10/SendMessage_utf8";//碟信
            string CorpID = "lixhai";
            string Pwd = "lixhai";
            string Cell = "";
            string subid = ""; //使用默认签名 易城商户
            string smstext = "";
            Content = Guolvheici(Content);//过滤黑词，只要有字符传递过来并且未执行错误，只是把黑词中间加 .


            Content = Content.Replace("【", "[");//针对与短信签名冲突，替换
            Content = Content.Replace("】", "]");//

            //智甄账户发送，读取签名
            url = "http://115.28.112.245:8082/SendMT/SendMessage";
            CorpID = "maike";
            Pwd = "lixh1210";
            smstext = "【绿野小站】";
            subid = "28";
            


            B2b_company_saleset com = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
            if (com != null)
            {
                if (com.Smstype == 0)
                {//使用碟信账户

                    smstext = com.Smssign;
                    subid = com.Subid;
                    Content = smstext + Content;//通用账户加签名
                }
                else
                {   //智甄账户发送，读取签名
                    url = "http://115.28.112.245:8082/SendMT/SendMessage";
                    CorpID = "maike";
                    Pwd = "lixh1210";
                    smstext = com.Smssign;
                    subid = com.Subid;
                    Content = smstext + Content;//通用账户加签名
                }
            }
            else {
                Content = smstext + Content;//通用账户加签名
            }

            //如果签名为空或异常，统一使用默认商户
            if (smstext == "" || subid=="0") {
                subid = "04";
                smstext = "【易城商户】";
                Content = smstext + Content;//通用账户加签名
            }


            //if (CorpID == "easetown" || CorpID == "wlcdd")
            //{
            //    url = "http://115.28.112.245:8082/SendMT/SendMessage";
            //    CorpID = "maike";
            //    Pwd = "lixh1210";
            //    string smstext = "【易城商户】";
            //    subid = "04";
            //    Content = smstext + Content;//通用账户加签名
            //}

            //测试手机，都发送 到 我的手机
            if (mobile == "18518562160" || mobile == "18000000001" || mobile == "18000000002" || mobile == "18000000003" || mobile == "18000000004" || mobile == "18000000005" || mobile == "18000000006" || mobile == "18000000007" || mobile == "18000000008" || mobile == "18000000009")
            {
                mobile = "13488761102";
            }

            try
            {

                string postString = "UserName=" + CorpID + "&UserPass=" + Pwd + "&Mobile=" + mobile + "&Content=" + Content + "&subid=" + subid;

                byte[] postData = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(postString);

                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] responseData = webClient.UploadData(url, "POST", postData);
                string srcString = System.Text.Encoding.GetEncoding("UTF-8").GetString(responseData);


                //byte[] postData = System.Text.Encoding.GetEncoding("GBK").GetBytes(postString);
                //WebClient webClient = new WebClient();
                //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //byte[] responseData = webClient.UploadData(url, "POST", postData);
                //string srcString = System.Text.Encoding.GetEncoding("GBK").GetString(responseData);

                srcString = srcString.Replace("\r", "");
                srcString = srcString.Replace("\n", "");

                if (srcString != "" || srcString != null)
                {
                    if (srcString.IndexOf(",") > -1)
                    {
                        var strarr = srcString.Split(',');

                        if (strarr[0] == "00" || strarr[0] == "03")
                        {
                            msg = "";
                            return 1;
                        }
                        msg = GetBackSmsErr(strarr[0]);
                    }
                    else
                    {
                        if (srcString == "00" || srcString == "03")
                        {
                            msg = "";
                            return 1;
                        }

                        msg = GetBackSmsErr(srcString);
                    }
                    return -1;
                }
                else
                {

                    msg = "短信发送错误,未接到返回";
                    return -1;
                }
            }
            catch (Exception ex)
            {
                msg = "短信发送错误" + ex.Message;
                return -9;
            }
        }
        #endregion




        #region 发送短信
        public static int GetMember_sms(string phone, string name, string card, string password, decimal money, string key, int comid)
        {
            string msg = "";
            string content = "";
            using (var helper = new SqlHelper())
            {
                B2b_crm surplus = null;
                if (card != "")
                {
                    surplus = new B2bCrmData().GetB2bCrmByCardcode(decimal.Parse(card));
                }
                var pro = new SendSmsHelper(helper).member_sms(phone, name, card, password, money, key);
                if (pro != null)
                {
                    if (pro.Remark.ToString() != "" || pro.Remark.ToString() != null)
                    {
                        content = pro.Remark.ToString();
                        content = content.Replace("$name$", name);
                        content = content.Replace("$phone$", phone);
                        content = content.Replace("$card$", card);
                        content = content.Replace("$pass$", password);
                        content = content.Replace("$money$", System.Math.Abs(money).ToString());
                        if (surplus != null)
                        {
                            content = content.Replace("$Ysurplus$", surplus.Imprest.ToString());
                            content = content.Replace("$Xsurplus$", surplus.Integral.ToString());
                        }
                    }

                }

            }

            try
            {
                var backContent = SendSms(phone, content, comid, out msg);
                return backContent;
            }
            catch (Exception ex)
            {
                msg = "短信发送错误" + ex.Message;
                return -9;
            }
        }


        public static int GetMember_sms(string phone, string name, string card, string password, decimal money, string key, int comid, int sendnum)
        {
            string msg = "";
            string content = "";
            using (var helper = new SqlHelper())
            {
                B2b_crm surplus = null;
                if (card != "")
                {
                    surplus = new B2bCrmData().GetB2bCrmByCardcode(decimal.Parse(card));
                }
                var pro = new SendSmsHelper(helper).member_sms(phone, name, card, password, money, key);
                if (pro != null)
                {
                    if (pro.Remark.ToString() != "" || pro.Remark.ToString() != null)
                    {
                        content = pro.Remark.ToString();
                        if (sendnum > 1)
                        {
                            content = "R" + (sendnum - 1).ToString() + pro.Remark.ToString();
                        }
                        content = content.Replace("$name$", name);
                        content = content.Replace("$phone$", phone);
                        content = content.Replace("$card$", card);
                        content = content.Replace("$pass$", password);
                        content = content.Replace("$money$", System.Math.Abs(money).ToString());
                        if (surplus != null)
                        {
                            content = content.Replace("$Ysurplus$", surplus.Imprest.ToString());
                            content = content.Replace("$Xsurplus$", surplus.Integral.ToString());
                        }
                    }

                }

            }

            try
            {
                var backContent = SendSms(phone, content, comid, out msg);
                return backContent;
            }
            catch (Exception ex)
            {
                msg = "短信发送错误" + ex.Message;
                return -9;
            }
        }


        internal Member_sms member_sms(string phone, string name, string card, string password, decimal money, string key)
        {
            string sqltxt = "";
            sqltxt = @"select id,sms_key,title,Remark,openstate,subdate,ip from member_sms where sms_key=@key and openstate=1 order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@key", key);

            using (var reader = cmd.ExecuteReader())
            {
                Member_sms m = null;

                while (reader.Read())
                {
                    m = new Member_sms
                        {
                            Id = reader.GetValue<int>("Id"),
                            Sms_key = reader.GetValue<string>("sms_key"),
                            Title = reader.GetValue<string>("Title"),
                            Remark = reader.GetValue<string>("Remark"),
                            Openstate = reader.GetValue<bool>("Openstate"),
                            Subdate = reader.GetValue<DateTime>("Subdate"),
                            Ip = reader.GetValue<string>("Ip")
                        };
                }
                return m;
            }
        }
        #endregion



        #region 短信黑词过滤
        /// <summary> 
        ///短信黑词过滤
        /// </summary> 
        /// <param name="msg">短信内容</param> 
        /// <returns>获取返回值</returns> 
        public static string Guolvheici(string msg)
        {
            string heici = "优惠,微信,平日,0平,温泉,（抵）押免担保,（套牌）车,《12个春秋》,《风波记》,《目睹中国改革开放28年之怪现状》,001工程,12580,12590,125元获取网站,13001217727,13381086033,13608171307,13895473436,13911838888,16大,16大1717wg.88448.com,17da,17大,1等奖,21世纪中国基金会,3p,3级,4．25事件,4P,610洗脑班,6-4tianwang,68170802,6996162,7.31,88448,89-64cdjp,9.635,9评,areunder,AV女,AV片,A级,A极,A片,A片软件,bigbowl,bignews,bitch,blowjob,boxun,B样,CDMA,chinaliberal,chinamz,chinesenewsnet,clit,creaders,dafa,dajiyuan,dfd,dfdz,dildo,dpp,erotic,falu,falun,falundafa,falungong,fa轮,flg,freechina,freedom,freenet,FUCK,gay,GCD,Hardcore,hongzhi,hrichina,huanet,hypermart.net,H动漫,H漫画,IDP教育集团,incest,IP17908,JAPAN,jiangdongriji,J巴,k-100氧吧,KISS,K粉,K仔,l2590803027,Lesbian,lihongzhi,LIMIT_WORD,makelove,making,minghui,minghuinews,nacb,naive,nipple,nmis,nude,oralsex,orgysex,paper64,peacehall,penis,PK黑社会,playboy,PORN,Private,pussy,renminbao,renmingbao,rfa,safeweb,semen,SEX,shit,simple,SIM卡抽奖,Slut,Soccer01.com,softcore,striptease,svdc,taip,tibetalk,TMD,triangle,triangleboy,txwq.net,UltraSurf,unixbox,ustibet,vagina,voa,voachinese,voachinesewangce,voyeur,wangce,wstaiji,xinsheng,XX,xxx,X服务,X你,X夜激情,yuming,zesctv,zhengjian,zhengjianwang,zhenshanren,zhongmin,zhuanfalun,阿扁,阿甘,阿娇,阿姨,爱抚,爱国者同盟,爱经,爱奴,爱液,安定片,安魂网,安装发票,按揭,按摸,暗杀,奥运会,澳雯丽,八佰伴珠宝,八九,八老,巴赫,扒光,罢工,罢课,白带,白虎,白浆,白军,白立朴,白梦,白皮书,白银晚盘,扳开,办理文凭,办理证件,办证,伴游,绑交,包你射,包皮,包厢,包赢,包准,保本出售,保本价,保钓,保就业,保利泉馨苑,报禁,鲍戈,鲍彤,鲍鱼,暴动,暴力,暴露,暴乱,暴政,爆浆,北大三角地论坛,北韩,北京奥舒,北京帮,北京车豪,北京当局,北京政权,北京之春,北京装修网,北美讲坛,北美讲坛s,北美自由论坛,贝领,被操,被盗,被干,被搞,被奸,本道,屄,逼样,毙,避孕套,便宜贷款,便宜房,变性,婊子,冰毒,病业说,波霸,勃起,博彩,博购软件,博讯,不穿衣,不挂牌,不手术,不授权,布局十七大,部隊,部牵头,部认定,部指定,彩丽·推广达人,彩民,菜市口交,蔡崇国,残忍,藏独,藏旗,藏青会,操屄,操逼,操比,操蛋,操你,操你妈,操你娘,操死,操他,操我,曹长青,曹刚川,曹颖,厕所,廁所,插她,插你,插他,插我,插穴,柴玲,长片,长舌象,常劲,超常科学,超低地产,超低地价,超低价,超低楼盘,超级性虐,超妍,超值地价出售,超值价,朝鲜,潮騷,炒股软件,车仑,车牌,车税,车天下汽车网团,车主名单,彻底根治,陈炳基,陈冠希,陈军,陈蒙,陈破空,陈水扁,陈水匾,陈希同,陈小同,陈宣良,陈一谘,陈总统,成本房,成本楼盘,成绩不理想怎么办,成人表演,成人电影,成人录像,成人论坛,成人漫画,成人片,成人书库,成人图,成人图片,成人网,成人网站,成人文学,成人小说,成人笑话,成人影视,成人用品,诚聘,诚信通手机商城,程凯,程真,迟浩田,持不同政见,赤党,赤匪,赤化,赤裸,赤身,冲动,冲凉,抽插,抽奖,出国移民,出口退税,出卖,出售答案,出售假币,出售假钞,出售枪支,出售手枪,出水,出血,出血价,初潮,初交,初熟,初夜,处男,处女,处女膜,处女终结者,处身,處女,传单,传说的胡曾联手是一种假象,传销,传中共中央关于17大的人事安排意见,床上,床戏,吹气娃娃,吹萧,吹箫,春潮,春宫,春情,春水,春夏之交,春夏自由论坛,春宵,春药,纯交,蠢猪,雌雄,粗口歌,促销,促销术语脑白金,催情,催情药,摧残,达赖,达赖喇嘛,答案卫星接收机,打倒,打倒共产党,打家劫舍,打炮,打折,打折机票,大b,大逼,大比,大便,大波,大彩,大参考,大出血,大蒂,大法,大法大福,大法弟子,大法洪传,大法轮,大放血,大花逼,大鸡巴,大纪,大纪元,大纪元时报,大纪元新闻网,大纪元杂志,大纪园,大家论坛,大减价,大奖,大陆,大陆当局,大明湖畔,大奶,大乳,大善大忍,大史,大史记,大史纪,大甩卖,大腿,大胸,大阴,大圆满,大中国论坛,大中华论坛,大众真人真事,代办签证,代办移民,代办证件,代缴发票,代缴税,代开安装发票,代开餐饮发票,代开地税发票,代开发票,代开服务发票,代开广告发票,代开国税发票,代开建筑发票,代开普通发票,代开商品发票,代开维修发票,代开运输发票,代开增值税,代开租赁发票,代卖发票,代售发票,贷款,贷款零利率,戴相龙,黛比,弹劾,弹药,党禁,党内分裂,荡妇,倒卖,倒台,登辉,登天梯,邓派,邓小平,邓笑贫,邓颖超日记,低价出售,低价房,低价楼盘,低息贷款,迪吧,迪里夏提,迪厅,底价,抵制日货,地皮出售,地皮低价出售,地下教会,地下刊物,地下钱庄,弟疼,弟痛,弟子,帝国之梦,第四代,颠覆中国政权,颠覆中华人民共和国政,电视流氓,玷污,貂蝉,钓鱼岛,调情,调试家用卫星,调戏,丁关根,丁元,丁子霖,顶级,顶极,顶片,鼎辉考研,东北独立,东方红时空,东方时空,东航金融,东南西北论,东南西北论谈,东社,东突,东突厥斯坦,东突厥斯坦伊斯兰,东突厥斯坦伊斯兰运动,东土耳其斯坦,东西南北论坛,东洲,董建华,动乱,胴体,毒气,独裁,独裁政治,独岛,独夫,独家代理,独立,独立台湾会,独立中文笔会,堵截,赌博,赌具,赌码,赌盘,赌球网站,杜智富,短.信群发,短片,短信广告,短信群发,短信群发器,短信商务广告,对冲,对日强硬,多党,多党制,多维,堕落,屙民,俄国,俄罗斯,恶搞晚会,恶警,二B,二逼,二手房,发飙,发廊,发愣,发抡,发抡功,发仑,发伦,发伦功,发囵,发沦,发纶,发轮,发论,发论公,发论功,发票,发票抵扣,发情,发骚,发展行,发正念,法*功,法.轮.功,法lun功,法L功,法功,法国国立工艺学院,法洪,法会,法愣,法理,法抡,法抡功,法仑,法伦,法囵,法沦,法纶,法轮,法轮大法,法轮弟子,法轮佛法,法轮功,法輪,法论,法十轮十功,法网恢恢,法谪,法谪功,法正,法正乾坤,法正人间,珐.輪功,反党,反动,反封锁,反封锁技术,反腐败论坛,反革命,反攻,反攻大陆,反共,反国家,反华,反雷达测速,反民主,反人类,反日,反社会,反政府,返税,饭岛,贩毒品,方励之,方舟子,房产,房屋出售,仿真,仿真假钞,放贷款,放荡,放光明电视制作中心,放炮,放屁,放下生死,放账,放纵,飞天舞蹈学校,飞扬论坛,菲律宾,肥穴,斐得勒,废除劳教签名,废统,费良勇,分家在,分裂,分裂国家,分裂人民,分裂中国,分裂中华人民共和国,分裂祖国,分娩,粉嫩小洞,粉饰太平,粉臀,粉仔,风波,风骚,风雨神州,风雨神州论坛,封从德,封杀,風騷,冯东海,冯素英,凤凰化妆品,佛法,佛展千手法,夫妻交换,服侍,服務,福彩,福田欧曼、潍柴动力,腐败,阝月,付申奇,妇人,复仇,复转军人,傅申奇,傅志寰,干柴烈火,干她,干妳,干妳老母,干妳妈,干妳娘,干你,干你妈,干你妈b,干你妈逼,干你娘,干死,干死你,干他,感情陪护,感受澳洲文化,幹女,赣江学院暴动,肛交,肛门,港澳博球网,港片,港台,羔羊,高潮,高官,高利贷,高利息贷款,高文谦,高息贷款,高薪养廉,高瞻,高自联,睾丸,告全国股民同胞书,告全体网民书,告中国人民解放军广大官兵书,戈扬,哥疼,哥痛,鸽派,割肉价,歌功颂德,蛤蟆,个人崇拜,工力,工自联,公安,公关,公关小姐,公务员,功德圆满,功法,功友,功友弟子,共.产.党,共.产党,共产,共产.党,共产党,共产极权,共产王朝,共产无赖,共产专制,共铲党,共党,共黨,共匪,共狗,共军,共奴,共青团背景,勾魂,勾引,狗b,狗逼,狗操,狗卵子,狗娘,狗屁,狗日,狗日的,狗屎,购房送钻戒,古方迷香,古怪歌,关卓中,官场日志,官倒,官商,官商勾结,官员成为政治新星,管理哲学双证班,贯通两极法,广闻,龟公,龟头,闺房,鬼村,滚珠棒,郭伯雄,郭罗基,郭平,郭岩华,国家安全,国家参与,国家承认大专学历,国家挂牌,国家机密,国家机器,国家牵头,国家认定,国家软弱,国家授权,国家指定,国军,国色天香网,国研新闻邮件,国贼,过瘾,哈批,海尔集团,海洛因,海外护法,寒假作业全托班,韩东方,韩联潮,韩日,汉风,汉奸,汗毛,航空订票,航空机票,航空母舰,航空票务,好荡,好多水,好片,好色,好爽,号码获得,号码生成器,喝精,合房网,合约,何德普,和平请愿书,和平修炼,和平演变道路已被中共堵死,河南金耘,河殇,核污染,黑车,黑房,黑鬼,黑幕,黑幕重重腐败到底,黑枪,黑社会,黑手党,黑庄,嘿咻,恒基中心,衡阳万通房产,弘法,弘志,红潮谎言录,红灯区,红魂网站,红色恐怖,红兽,红杏,红志,宏法,宏志,洪传,洪发交流,洪吟,洪哲胜,洪志,紅杏,后庭,胡紧掏,胡锦涛,胡锦滔,胡锦淘,胡景涛,胡平,胡书记,胡温政府,胡耀邦,胡总书记,互助游,户型,护法,花花公子,华建敏,华通时事论坛,华夏文摘,华夏文摘快递,华语世界论坛,华远猎头,华岳时事论坛,话紫禁城,换届隐忧,换妻,皇后,黄慈萍,黄祸,黄金时代网络电话,黄菊,黄丽满,黄片,黄色表演,黄色电影,黄色录像,黄色录音,黄色漫画,黄色书籍,黄色书刊,黄色图片,黄色网站,黄色文学,黄色小电影,黄色小说,黄色笑话,黄色音像,黄色影视,黄翔,簧片,回良玉,回民,回民暴,回民暴动,悔过书,汇款中心,惠晨,慧峰旗舰,慧网,昏药,活体器官,火车服务,火枪,饥渴,鸡八,鸡巴,鸡吧,鸡鸡,鸡毛信文汇,姬胜德,积克馆,基督,激情,激情大片,激情电影,激情小电影,级片,极品,极品价,急着用钱怎么办,極品,集会,集体上访,集体做爱,纪元,妓女,妓院,夹棍,佳静安定片,佳信,家用天线,家用卫星,贾庆林,贾廷安,贾育台,假币,假钞,假发,假发票,假护照,假教育,假身份证,假照,假证,假证件,假证照,奸,奸污,奸淫,尖挺,监管局,监听,监听宝,监听器,监听王,监狱,简鸿章,檢查,見證,建国党,贱B,贱逼,贱比,贱货,贱人,江core,江XX,江ze民,江八点,江独裁,江二世,江蛤蟆,江核心,江湖淫娘,江昏君,江老贼,江理论,江流氓,江路线,江罗,江罗犯罪集团,江罗集团,江绵恒,江青,江神经,江戏子,江则民,江择民,江泽慧,江泽民,江澤民,江贼,江贼民,江折民,江浙民,江猪,江猪媳,江主席,姜春云,将则民,僵贼,僵贼民,疆独,讲法,讲事实,降价,酱猪媳,交班,交媾,交欢,交换夫妻,交配,交友网,胶交,叫床,叫春,叫鸡,叫雞,教徒,教徒人权,教养院,接客,揭批书,结帐,姐疼,姐痛,解体,金榜题名,金瓶,金瓶梅,金尧如,金证通,锦涛,緊縛,禁果,禁看,禁片,禁区,禁书,经期,经文,惊暴双乳,惊艳,精液,精子,警察殴打,婧女,靓女,靓欣家家装,静坐,九.评,九码,九评,九-评,九评共产党,九一八,菊花蕾,巨棒,巨根,巨奖,巨片,巨乳,—聚友快车道国际名车会所,绝技,绝品,绝食,绝食抗暴,絕食,军妓,军警,军委主席,军转安置,军转干部,开包,开苞,开处,开发票,开放杂志,开盘,开天目,开悟,揩油,凯子,砍死,看中国,康涛杰,抗日,抗议,考后付款,考前发放,靠你妈,可卡因,刻章,空姐,恐怖,恐怖袭击,口含,口技,口交,口淫,跨世纪的良心犯,快感狂欲,狂操,邝锦文,窺探,昆明民航,拉丹,拉登,拉屎,辣舞,蓝黛酒吧,蓝丝带,烂逼,烂比,烂货,滥交,廊坊印务,浪荡,浪穴,劳动教养所,劳改,劳教,劳务签证,劳务中介,老二,老江,老龄产业,老龄集团,老毛,老人政治,勒索,黎安友,李长春,李大师,李登辉,李红痔,李宏志,李宏治,李洪宽,李洪志,李继耐,李兰菊,李岚清,李老师,李录,李禄,李鹏,李瑞环,李三共志,李少民,李淑娴,李旺阳,李文斌,李向东,李小朋,李小鹏,李月月鸟,李志绥,李总理,李总统,励志教育,连胜德,莲花艺术团,联邦政府,联合起诉最高人民法院,联通短信,联通贵宾卡,联通商务通,联总,联总这声传单,联总之声,联总之声传单,廉政大论坛,练功,炼功,梁光烈,梁擎墩,两岸关系,两岸三地论坛,两个中国,两会,两会报道,两会新闻,两性,两性狂情,廖锡龙,劣等人种博彩,林保华,林长盛,林樵清,林慎立,林昭纪念奖,灵动卡,灵修,凌锋,凌辱,领地,刘宾深,刘宾雁,刘刚,刘国凯,刘华清,刘俊国,刘凯中,刘千石,刘青,刘山青,刘士贤,刘文胜,刘晓波,刘晓竹,刘旋,刘永川,流经,流精,流氓,流氓民运,流氓无产者,流氓政府,流脓,流亡,留条后路,柳树中学,六●四,六彩,六合彩,六码,六四,六四事件,龙虎豹,楼上楼,漏点,漏光,露点,露二点,露两点,露露,露毛,露体,陆委会,吕京花,吕秀莲,绿色环保手机,氯氨酮,乱叫,乱伦,乱倫,乱论,亂倫,抡功,伦功,伦特微,轮大,轮功,轮奸,罗干,罗礼诗,裸干,裸聊,裸露,裸体,落红,妈的,妈批,麻黄素,麻醉,麻醉钢枪,麻醉枪,麻醉药,麻醉乙醚,马大维,马会,马良骏,马赛克,马三家,马时敏,码会,吗啡,吗啡碱,吗啡片,买春,买卖枪支,麦角酸,卖逼,卖比,卖春,卖二手车,卖国,卖砒,卖身,卖淫,瞒报,猫肉,毛厕洞,毛都露出,毛派,毛片,毛一鲜,毛泽东,毛泽东123,毛泽东2,毛贼东,冒汁,貌美,美发屋,美凤,美鳳,美国参考,美国之音,美眉,美媚,美女露点裸照,美女视频,美片,美容专家,美少妇,美腿,美腿写真,美臀,美穴,妹疼,妹痛,猛料,猛男,猛男强奸,猛片,蒙独,蒙古独,蒙古独立,蒙汗药粉,梦网洪志,咪咪,迷昏药,迷奸,迷奸药,迷思,迷药,密拍,密桃,密穴,蜜蜂棒,蜜桃,蜜穴,绵恒,免费贷款,免费坐诊,面授,面具PARTY,面授,灭绝,民国,民进党,民联,民生行,民意,民意论坛,民运,民阵,民猪,民主,民主墙,民族矛盾,民族问题,名山县,明Hui,明慧,明慧网,明慧周刊,命根,摸奶,末世论,莫伟强,墨池轩中考美亚利桑,木犀地,木子论坛,内部地产,内部地价,内部机票,内部价,内斗退党,内裤,内窺,奶头,奶头真红,奶子,男妓,男女服务,男女公关,南大自由论坛,南华早报,闹事,嫩穴,尼姑,倪育贤,你爸,你妈,你妈逼,你妈的,你娘,你娘的,你说我说论坛,拟募集总规模3亿元,娘们,牛B,牛逼,牛比,农行农业银行,奴隶,奴隷,奴役,女郎,女性用品,女阴,女优,虐待,锘挎瘺娉戒笢1234,锘挎瘺娉戒笢12345,排泄,牌九,潘国平,潘金莲,盘古乐队,炮图,泡沫经济,陪聊,陪洗,陪浴,赔率,喷精,喷你,喷涌,蓬门,屁眼,騙局,嫖娼,嫖客,瓶梅,迫娼,迫害,破处,破瓜,破鞋,璞瑅,浦发,祁建,齐墨,气球男孩,汽车购置税,器官移植,钱达,钱国梁,钱其琛,钱庄,潜在用,枪决女犯,枪决现场,枪手,枪支,枪支弹药,強暴,強姦,强X,强暴,强奸,强奸犯,强效失意药,抢粮,抢粮记,抢油,敲诈,乔石,窃取,窃听,窃听器,窃听器材,窃听王,亲共行动,亲临,亲美,亲日,钦本立,秦晋,禽兽,禽獸,青楼,青天白日旗,轻舟快讯,清仓价,清纯,清纯少妇,情妇,情色,情色大片,情色电影,情欲,请愿,庆红,球彩,璩美,去你妈的,去他娘的,全城最低,全国两会,全国退党,全国最低,全市最低,全新假钞,犬交,裙下,群发,群奸,群交,群伦,群体灭绝,热比娅,热辣,热站政论网,人民报,人民报讯,人民大众时事参考,人民内情真相,人民真实,人民真实报道,人民之声论坛,人权,人兽,人妖,人与兽,人渣,仁和会计,仁吉旺姆,忍,认筹,日阿,日啊,日本鬼子,日逼,日韩,日奸,日立挖掘机,日内瓦金融,日你,日你妈,日您,日批,日片,日死你,日他,日元贷款,熔基,揉捏,蹂躏,肉棒,肉洞,肉棍,肉体,肉穴,乳房,乳交,乳头,乳臀,瑞士金融大学,润茂,萨达姆,赛马,三点,三点水,三个代表,三级,三级片,三码,三陪,三陪先生,三陪小姐,三式,三水法轮,三唑仑,桑拿,骚,骚逼,骚货,骚乱,骚女,骚穴,臊样,騷騷,色狼,色魔,色女,色情,色情服务,色情网站,色情小电影,色诱,色欲,杀人,傻B,傻逼,煞笔,善恶有报,商铺,商务领航,上床,上访,上海帮,上海孤儿院,上海酷选,上她,上口阳光,上午收盘,少儿不宜,少妇,少婦,少女高潮,邵家健,射精,射颜,射液,身份证生成器,呻吟,深闺,深喉,神墨,神棍,神墨,神通加持法,神州电影制片厂,沈彤,审判,升天,升值潜力,生理,生死,生者和死者,生殖,省委大门集合,省政府大门集合,圣徒,盛华仁,盛雪,失密,失身,失贞,师父,师傅法身,施暴,十八大,十八摸,十六大,石戈,时代论坛,时事论坛,实录,世界经济导报,市场最低价,示威,事实独立,视频裸聊,试车付款,试卷、,试题答案,试衣,是中国社会的爱滋病,嗜秽,收容所,收縮,手机复制,手机跟踪定位器,手机广告,手机号码改号器,手机卡复制器,手机群发,手技,手枪,手槍,手淫,首付,受贿,兽奸,兽交,售弹药,售地皮,售卖武器,售枪支,舒淇,舒琪,熟妇,熟女,沭河湾,数据中国,刷卡消费,甩卖,甩卖价,双峰,双规,双十节,双腿间的禁地,谁是新中国,氵去,氵去车仑工力,水扁,税力,税票,丝袜,丝袜写真,司马晋,司马璐,司徒华,私彩,私处,私處,斯诺,四川独,四川独立,四级答案,四陪,四小码,宋xx,宋平,宋书元,宋祖英,搜身,苏绍智,苏网培训学校,苏晓康,酥乳,素人,速汇,宿命,宿命论,他妈,他妈的,他母亲,他娘的,台独,台港,台盟,台湾18DY电影,台湾独,台湾独立,台湾共合国,台湾狗,台湾建国运动组织,台湾青年独立联盟,台湾政论区,台湾自由联盟,太子党,泰姬,贪官,贪污犯,探测狗,汤光中,唐柏桥,唐家璇,唐捷,唐荆陵,陶驷驹,讨伐中宣部,套牌,套牌车,套子,特等奖,特级片,特价房,特价机票,特码,特首,特殊服务,特务民运,特写,特寫,特异功能,腾文生,滕文生,提高学历,提现,体彩,體檢,剃毛,替考托福,天安门,天安门录影带,天安门母亲,天安门事件,天安门屠杀,天安门一代,天安門,天材教育,天府广场集会,天国乐团,天怒,天葬,天柱,舔,舔奶,舔穴,挑逗,跳蛋,跳楼价,通信维权,同床,同房,同居,同性,童屹,酮体,统独,统独论坛,统战,捅你,捅我,偷渡,偷欢,偷窥,偷窺,偷拍,偷窃,偷情,投毒杀人,透码,透视镜,透视眼镜,突厥斯坦,突破封锁,涂运普,屠杀,兔女郎,推翻,推翻社会主义制度,推油,退出共产党,退党,退税,拖光,脫星,脱光,脱衣,娃娃兵,外交论坛,外交与方略,外教,外蒙,外围赌球,外围码,外围庄家,玩弄,晚间游戏,晚年周恩来,万润南,万维读者论坛,万晓东,汪岷,亡党,王八蛋,王宝森,王炳章,王策,王超华,王辅臣,王刚,王涵万,王沪宁,王军涛,王力雄,王瑞林,王润生,王若望,王希哲,王小丫,王秀丽,王冶坪,网络代理,网络色情网址大全,网上博彩,网上兼职一个月能赚,网上赚钱,网特,网赚队伍,微型无线隐型耳机,维权抗暴,伟哥,猥亵,卫生部,卫生巾,卫星安装调试,卫星电视安装,未成年,尉健行,慰安,慰安妇,魏京生,魏新生,温家宝,温元凯,温州百得,文革,文化大革命,窝囊的中国,窝囊中国,我XX你,我操,我操你,我草,我的后讨伐中宣部时代,我靠,我日,我是回民,龌龃,污辱,无担保贷款,无抵押贷款,无风险贷款,无界浏览器,无码,无碼,无痛苦,无网界,无网界浏览,无息贷款,无遮,吴百益,吴邦国,吴方城,吴官正,吴弘达,吴宏达,吴仁华,吴学灿,吴学璨,吴仪,吾尔开希,五不,五套功法,伍凡,武士,侮辱,舞娘,勿忘国耻,西安职,西藏,西藏独,西藏独立,西藏论坛,西藏人权,西藏天葬,西片,吸毒,吸鸟,希望之声国际广播电台,洗黑钱,洗脑,洗钱,下部,下法轮,下流,下身,下台,下体,下午收盘,下阴,先天健康法,咸湿,现金奖,限制级,献身,香港明报,香港总部,香艳,向导学校,项怀诚,项小吉,消魂,消业论,销魂,销售枪支,小便,小参考,小弟弟,小电影,小鸡鸡,小来子,小灵通,小灵通短信,小妹,小日本,小升初,小穴,肖强,邪恶,寫真,泄密,谢长廷,谢选骏,谢中之,心藏大恶,辛灏年,新党,新观察论坛,新华举报,新华内情,新华通论坛,新疆独,新疆独立,新起点教育,新生网,新唐人,新唐人电视台,新唐人晚会,新闻封锁,新乡惠诚公司,新语丝,新悦大酒店,信用危机,星亚网络影视公司,行房,邢铮,性爱,性爱图,性服务,性福,性高潮,性关系,性技,性交,性交大赛,性交易,性交姿势,性娇,性免费电影,性奴,性虐待,性派对,性器,性侵害,性趣,性生活,性事,性无能,性用品,性游戏,性欲,性战,性知识,胸罩,熊炎,熊焱,修练,修炼,修炼之歌,秀蕊,徐邦秦,徐才厚,徐匡迪,徐水良,许家屯,旭辉朗香郡,畜生,玄`机,玄机,薛伟,穴穴,学潮,学联,学生妹,学习班,学运,学自联,學員,雪山狮子,血泪,血洗,血腥,鸦片,鸦片液,鸦片渣,亚热,亚洲自由之声,阉割,严家其,严家祺,阎明复,颜射,艳情,艳舞,艳照,艳照门,燕南评论,艷遇,央视内部晚会,阳光大厦,阳光家电城,阳光洗化,阳具,阳萎,阳痿,杨怀安,杨建利,杨巍,杨月清,杨周,样板房,妖麗,姚月谦,摇奖,摇头丸,摇头玩,野鸡,野炮,夜场,夜话紫禁城,夜激情,夜魅一族,夜生活,夜总会,夜總會,一党,一党专政,一党专制,一等奖,一对一,一凤,一夜,一夜情,一中一台,伊斯兰运动,姨妈,移动短信,以身护法,以血护法,义解,亦凡,异见人士,异物,异议人士,抑制剂,易丹轩,易志熹,阴部,阴唇,阴道,阴道被捅,阴蒂,阴户,阴茎,阴精,阴胫,阴毛,阴门,阴囊,阴水,陰唇,陰道,淫,淫荡,淫妇,淫秽,淫姐姐,淫乱,淫猫,淫毛,淫靡,淫女,淫妻,淫兽,淫水,淫图,淫穴,淫汁,银联卡,银行卡,银行联合管理局,尹庆民,引诱,隐私,隐私图片,隐形喷剂,印尼伊斯兰祈祷团,应招妓女,应召,应召女,应召女郎,英国杜伦大学,英酷国际英语,英语枪手,英语四六级答案,應召,鹰派,优惠房,尤物,由喜贵,游戏管理员,游行,右派,幼B,幼齿,幼瓜,幼奸,幼交,幼女,诱惑,诱奸,誘惑,于大海,于浩成,余英时,娱乐足球,舆论,舆论反制,与美女过夜,宇明网,宇宙真理,宇宙最高法理,玉弓,玉女,玉蒲,玉蒲团,玉体,玉液,预约卡,欲报从速,欲望,慾火,鸳鸯,元/平方,圆满,圆明网,援交,远程偷拍,远志明,月经,月經,月薪,月薪万元,月赚千圆,月赚三千,月赚万元,岳武,越轨,云雨,杂交,杂种,砸馆,在十月,昝爱宗,早泄,早泻,造爱,造反,则民,择民,择泯,泽民,泽云轩,贼民,曾培炎,曾庆红,增值税退税,张伯笠,张钢,张宏堡,张开双腿,张林,张万年,张伟国,张昭富,张志清,招宝网兆亿擔保,招妓,招聘,找政府评理,召妓,兆赫,兆亿担保,赵海青,赵南,赵品潞,赵晓微,赵紫阳,折后再送,折扣价,折磨,哲民,浙江求是,针对台湾,针孔摄像,侦探设备,真、善、忍,真荡,真人,真善美,真善忍,真相,真象,真修,镇压,争鸣论坛,正点,正法,正法时期,正见网,正派民运,正邪大决战,正义党论坛,证照,郑义,郑源,政变,政府参与,政府挂牌,政府牵头,政府认定,政府软弱,政府授权,政府无能,政府指定,政权,政协,政治潮流,政治处,政治反对派,政治犯,政治风波,政治局,支联会,支那,蜘蛛,直销航空意外险,职业托福枪手,職責,指点江山论坛,制服,治愈率百分百,致胡书记的公开信,中大奖,中俄边界,中俄边界新约,中俄密约,中功,中共,中共暴政,中共当局,中共独裁,中共独枭,中共恶霸,中共监狱,中共警察,中共领导人黑幕,中共迫害,中共太子,中共统治,中共伪政权,中共小丑,中共心中最大的恐惧,中共政权,中共政坛腐败内幕,中共政治流氓,中共政治新星,中共中央,中共中央大换血,中共中央黑幕,中共专制,中共走狗,中国报禁,中国泛蓝联盟,中国复兴论坛,中国改革年代政治斗争,中国高层权力斗争,中国高层人事变动解读,中国孤儿院,中国好声音,中国和平,中国六四真相,中国论坛,中国民主党联合总部,中国社会的艾滋病,中国社会进步党,中国社会论坛,中国威胁论,中国问题论坛,中国移动通信,中国银联,中国在统一问题上的投降主义,中国真实内容,中国政坛“明日之星”,中国政坛“清华帮”盛极而衰,中国政坛新星,中国政坛新星中的四大天王,中国政治新星,中国之春,中国猪,中國當局,中华大地,中华大地思考,中华大众,中华讲清,中华联邦政府,中华民国,中华人民实话实说,中华人民正邪,中华时事,中华养生益智功,中华真实报道,中级职称开班中,中奖,中奖信息,中坤广场,中民保险网,中南海的权利游戏,中南海斗争,中南海高层权利斗争,中南海惊现东宫小朝廷,中南海秘闻,中南海内斗,中南海内幕,中南海浓云密布,中南海权力斗争,中南海权利斗争,中全会,中央军委,中央派系斗争,中一九骏,中域,中域公考,钟山风雨论坛,众益德,周恩来忏悔,周恩来后悔,周恩来自责,周锋锁,周刊纪事,周天法,周旋,朱嘉明,朱琳,朱毛,朱容基,朱溶剂,朱镕基,猪操,猪聋畸,猪毛,猪毛1,专业代考,专业美容,专业美容专家,专政机器,专制,转法轮,转化,准确率为90.81,资产抵押,子弹,子宫,自焚,自民党,自摸,自拍,自杀,自杀手册,自杀指南,自卫队,自慰,自已的故事,自由,自由联邦,自由民主论坛,自由网,自由写作奖,自由亚洲,自制手枪,宗教压迫,走光,走私,足球博彩,足球娱乐,阻止奥运,阻止中华人民共和国统,祖传秘方,最低的价格,最好的产品,最佳投资,最优惠的价格,作爱,作秀,坐爱,坐台小姐,做爱,做爱经典,做爱全过程,做爱挑逗,做愛,房地产,10010,房源,精装,链家,开房,重金,抵押,拿货,聪明树早教,移民,情感陪护,特肖,内部直招,卫星电视,无须担保,详询10086,神州行,赵洪祝,抵制,电狗,胡温,包二奶,枪模,仿真枪,管制刀具,迷魂药,炸药,假文凭,买枪,拥护台独,吕祖善,习近平,反动派,你他妈的,放贷,无须抵押,克隆,复制手机卡,上网文凭,君赢,六码中一特,婚外情,利民投资,内幕,茉莉花,一次通过,谷开来,5798333,旺铺,借款,尼尔伍德,薄熙来,薄瓜瓜,王立军,低息,高租,货到付款,财富热线,金海岸,绿卡,年息,手雷,扣款,放款,担保,反对,貸款,生不如死,报复,移民,贷款,开盘,平米,平米,户型,别墅,产权,庭院,独栋,公寓,现房,豪宅,两居,三居,两室,三室,经典户,精品户,跃层,CBD,/平,/平米,外.教,外教,首付,首.付,万起,洋房,产权,0年产,留学,李克强,孟母来文,文香雅苑,乌海大学,涵养人生,白云山庄,68502001,建材,中国移动,中国联通,中国电信,警官,警官,警察,派出所,公安局,冻结,冻结,4000-218660,淘宝客服,留学,㎡,吃屎,丧命,武汉光博会,来自北美老师的先进教育,800017164,现金收购各类,脱光底价,奔驰中国再营销团队,出行或投融资的需求,淘.宝客服,淘宝客服,淘.宝.客.服,汇通汽修,万盛投资,半仓操作,OPO王鹏眼镜,伊贝诗大,湖南卫视,我是歌手,www.hngs8.com,hngs8,万科,欧泊,68311022,变变变,万科欧泊,心随你变,最强音,歌手,歌手,歌*手,佳丽,淘宝提示,4006772708,400-8218-888,4008218-888,4008218888,400-8218888,金玉恒通,6.22202E+18,6.22848E+18,6.227E+18,6.22848E+18,9.55998E+18,0411-39630739,41139630739,6.22848E+18,6.22848E+18,6.22848E+18,6.22204E+17,6.227E+18,0731—6209909,7316209909,023-61654593,2361654593,6.22848E+18,0371-65291901,37165291901,6.22843E+18,6.22848E+18,6.22202E+18,6.22204E+17,6.22202E+18,7316136740,0731-6136740,6228481100497229713,6.22202E+18,0991--2126136,0991-2126136,9912126136,125906429,400-6264-119,4006264119,400-6264119,0411-39639795,0557-3230831,0316-6647365,6.22202E+18,6.22202E+18,6.227E+18,www.hkytjt.cn,我要上春晚,上春晚,春晚,春晚,春-晚,春.晚,代购,工信部,自动关闭交易,讨厌、讨厌啦,全5星好评截图,【俊朗服饰】,拍下的宝贝尚未付款,1个月租金+送10万积分,【嗨学网】,诠渡一年一度双11狂欢节,赠品数量有限,返亲10元优惠券,百度翔计划助力,100元的优惠券,现赠送您免费,千星移动,赎身,免单活动,路演活动,湛江金富,再等一年,你相识已有50天,辉煌科技,酷狗繁星网,狂欢倒计时,深圳华高,www.znpz.com.cn,五粮液,汉氏外卖,【联想手机官方商城】,【中国国家地理天猫旗舰店】,双11就不要去四处搜索,松竹化妆品双11狂欢,华商投资基金参与的,装修买木门到天猫,新品上柜，感恩有礼,汉臣氏携小太阳,【提前双11】,11风暴来袭,宁夏红奔,选橱柜衣柜先比较后消费,黄金285元,锦绣新天地,订餐热线,自如事业部】,吉利GX7,首租首日免租金,双11斩获全年抄底价,双语教材,热映推荐,【第九城市】,好装好卸大量要车,大众网日照童博会,五粮液集团建厂60周年,几十年年份老酒万金难求,39800元两樽,薇信,不想死找医生,搜房,双11来十里河灯饰城,推车5折优惠,展推车立减800元,【链家在线】,并买一送一,本店已有新款上柜,人保京,抢先加入购物车,娇兰大连麦凯乐冬季会员日,尊享正价9折优惠,北京股商推出,凯盛经略,中融,中融-吉盛源,【精品生活馆】,来电有更多礼品赠送,VIP会员折扣七折,FOT项目,联通商城,1.5万让利,联通网上商城,av,自助餐98元,投资,教育,报价,联通,黄色,枪,创富金融,顾家家居,恭喜您中奖了，所中奖项为：,14年成人学士学位报名已经开始,火爆销售中,火热预定中";
            var iString = "";
            int j = 0;
            try
            {
                if (msg != "")
                {
                    string[] backContent = heici.Split(',');
                    foreach (string i in backContent)
                    {
                        j = msg.IndexOf(i);
                        if (j >= 0)
                        {
                            if ((i.Trim().Length) > 0)
                            {
                                for (int m = 0; m <= i.Trim().Length - 1; ++m)
                                {
                                    iString = iString + i[m] + ".";
                                }
                            }
                            else
                            {

                                iString = i;
                            }
                            msg = msg.Replace(i, iString);
                            iString = "";
                        }
                    }
                    return msg;
                }
                else
                {
                    return msg;
                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion



        //#region 碟信接口发送短信webserver
        ///// <summary> 
        /////发送碟信接口
        ///// </summary> 
        ///// <param name="mobile">手机</param> 
        ///// <param name="Content">发送内容</param> 
        ///// <returns>获取返回值</returns> 
        //public static int WebServerSendSms(string mobile, string Content, out string msg)
        //{
        //    string smsurl = "http://115.28.14.21/";
        //    string CorpID = "wlc";
        //    string Pwd = "lixh1210";
        //    string Cell = "";
        //    string SendTime = "";
        //    var backContent = string.Empty;

        //    LinkWSSoapClient WSS = new LinkWSSoapClient();

        //    try
        //    {
        //        int R = WSS.Send(CorpID, Pwd, mobile, Content, Cell, SendTime);

        //        if (R == 0)
        //        {
        //            msg = ("发送成功！");
        //        }
        //        else if (R == -1)
        //        {
        //            msg = ("帐号未注册！");
        //        }
        //        else if (R == -2)
        //        {
        //            msg = ("其他错误！");

        //        }
        //        else if (R == -3)
        //        {
        //            msg = ("密码错误！");
        //        }
        //        else if (R == -4)
        //        {
        //            msg = ("手机号码格式不对！");
        //        }
        //        else if (R == -5)
        //        {
        //            msg = ("余额不足！");
        //        }
        //        else if (R == -6)
        //        {
        //            msg = ("定时发送时间不是有效时间格式！");
        //        }
        //        else {
        //            msg = "未知错误";
        //        }

        //        return R;
        //    }
        //    catch (System.Net.WebException WebExcp)
        //    {
        //        msg = ("网络错误，无法连接到服务器！");
        //        return 9;
        //    }

        //}
        //#endregion


        #region 发送短信公用
        public static int Member_smsBal(Smsmodel model)
        {
            string msg = "";
            string content = "";
            using (var helper = new SqlHelper())
            {
                B2b_crm surplus = null;
                if (model.Card != "" && model.Card != null)
                {
                    surplus = new B2bCrmData().GetB2bCrmByCardcode(decimal.Parse(model.Card));
                }
                var pro = new SendSmsHelper(helper).member_sms(model.Phone, model.Name, model.Card, model.Password, model.Money, model.Key);
                if (pro != null)
                {
                    if (pro.Remark.ToString() != "" || pro.Remark.ToString() != null)
                    {
                        content = pro.Remark.ToString();
                        content = content.Replace("$comName$", model.ComName);
                        content = content.Replace("$phone$", model.Phone);
                        content = content.Replace("$name$", model.Name);
                        content = content.Replace("$card$", model.Card);
                        content = content.Replace("$password$", model.Password);
                        content = content.Replace("$money$", System.Math.Abs(model.Money).ToString());
                        content = content.Replace("$num$", model.Num.ToString());
                        content = content.Replace("$old$", model.Old);
                        content = content.Replace("$starttime$", model.Starttime.ToString("yyyy-MM-dd"));
                        content = content.Replace("$endtime$", model.Endtime.ToString("yyyy-MM-dd"));
                        content = content.Replace("$code$", model.Code);
                        content = content.Replace("$customtext$", model.Customtext);
                        content = content.Replace("$key$", model.Key);
                        content = content.Replace("$title$", model.Title);
                        content = content.Replace("$num1$", model.Num1.ToString());
                        if (surplus != null)
                        {
                            content = content.Replace("$imprest$", System.Math.Abs(model.Imprest).ToString());
                            content = content.Replace("$integral$", System.Math.Abs(model.Integral).ToString());
                        }
                    }

                }

            }

            try
            {
                if (model.Key == "预订酒店短信")//不用支付，直接发送预订短信
                {
                    var backContent = SendSms(model.Phone, content, model.Comid, out msg);
                    return backContent;
                }
                else if (model.Key == "微信酒店预订服务商通知短信")//服务商通知短信
                {
                    var backContent = SendSms(model.RecerceSMSPhone, content, model.Comid, out msg);
                    return backContent;
                }
                else
                {
                    var backContent = SendSms(model.Phone, content, model.Comid, out msg);
                    return backContent;
                }
            }
            catch (Exception ex)
            {
                msg = "短信发送错误" + ex.Message;
                return -9;
            }
        }
        #endregion

        public string GetSmsContent(string smskey)
        {
            string sql = "select Remark from member_sms where sms_key='" + smskey + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return reader.GetValue<string>("remark");
                }
                return "";
            }


        }


        public static string GetBackSmsErr(string backkey)
        {
            if (backkey == "02")
            {
                return "IP限制";
            }
            if (backkey == "04")
            {
                return "用户名错误";
            }
            if (backkey == "05")
            {
                return "密码错误";
            }
            if (backkey == "06")
            {
                return "编码错误";
            }
            if (backkey == "07")
            {
                return "发送时间有误";
            }
            if (backkey == "08")
            {
                return "参数错误";
            }
            if (backkey == "09")
            {
                return "手机号码有误";
            }
            if (backkey == "10")
            {
                return "扩展号码有误";
            }
            if (backkey == "11")
            {
                return "余额不足";
            }
            if (backkey == "-1")
            {
                return "服务器内部异常";
            }
            if (backkey == "REJECT")
            {
                return "非法消息内容";
            }


            return "未知异常";

        }
    }
}
