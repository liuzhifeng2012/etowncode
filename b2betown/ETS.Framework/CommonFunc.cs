using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using ETS.Framework;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace ETS.Framework
{
    public class CommonFunc : System.Web.UI.Page
    {
        //清除数字前的0
        public static  string Clearzero(String a)
        {
            int len = a.Length;                          //取长度
            String b = "";
            int i = 0;
            while (a.Substring(i, 1) == "0" && i < len) i++;  //找到第一个不是0的位置

            if (i < len)
            {
                b = a.Substring(i);                 //拷贝
            }
            return b;
        }
        //判断数字
        public bool IsNum(string Str)
        {
            bool bl = false;
            string Rx = @"^[1-9]\d*$";
            if (Regex.IsMatch(Str, Rx))
            {
                bl = true;
            }
            else
            {
                bl = false;
            }
            return bl;
        } 

        #region 提取字符串中的数字
        public static int GetNumber(string strTempContent)
        {
            try
            {
                strTempContent = System.Text.RegularExpressions.Regex.Replace(strTempContent, @"[^\d]*", "");
                return strTempContent.ConvertTo<int>(0);
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        #region  保留2位小数点,以后用后一种方法GetRound
        public static string OperTwoDecimal(string r)
        {
            if (r.IndexOf(".") != -1)
            {
                if (r.Substring(r.IndexOf(".") + 1).Length >= 2)
                {
                    //if (r.Substring(r.IndexOf(".") + 1).Length==2)
                    //{
                    //    string rr = r.Substring(r.IndexOf(".") + 1, 2);
                    //    if (rr == "00")
                    //    {
                    //        string ret = r.Substring(0, r.IndexOf("."))+".00";
                    //        return ret;
                    //    }
                    //    else
                    //    {
                    //        string ret = r.Substring(0, r.IndexOf(".") + 3);
                    //        return ret;
                    //    }
                    //} 
                    //else
                    //{
                    string ret = r.Substring(0, r.IndexOf(".") + 3);
                    return ret;
                    //}
                }
                else
                {
                    string ret = r + "0";
                    return ret;
                }
            }
            else
            {
                return r + ".00";
            }
        }
        #endregion
        #region 数据四舍五入，保留小数点位数
        /// <summary>  
        /// 四舍五入  
        /// </summary>  
        /// <param name="dblnum">源数据</param>  
        /// <param name="numberprecision">小数位</param>  
        /// <returns></returns>  
        public static string GetRound(object objnum, int numberprecision)
        {
            double returnnum = 0;
            if (objnum != null)
            {
                try
                {
                    double dblnum = double.Parse(objnum.ToString());
                    int tmpNum = dblnum > 0 ? 5 : -5;
                    double dblreturn = Math.Truncate(dblnum * Math.Pow(10, numberprecision + 1)) + tmpNum;
                    dblreturn = Math.Truncate(dblreturn / 10) / Math.Pow(10, numberprecision);
                    returnnum = dblreturn;
                }
                catch { }
            }
            //如果小数点位数不足，则用0补齐
            string returnnum_str = returnnum.ToString();

            if (returnnum_str.IndexOf(".") != -1)
            {
                int n = returnnum_str.Substring(returnnum_str.IndexOf(".") + 1).Length;//小数点后位数
                if (n < numberprecision)
                {
                    string nstr = "";
                    for (int i = n; i < numberprecision; i++)
                    {
                        nstr += "0";
                    }
                    if (nstr != "")
                    {
                        returnnum_str = returnnum_str + nstr;
                    }
                }
            }
            else
            {
                string nstr = "";
                for (int i = 0; i < numberprecision; i++)
                {
                    nstr += "0";
                }
                if (nstr != "")
                {
                    returnnum_str = returnnum_str + "." + nstr;
                }
            }
            return returnnum_str;
        }
        #endregion


        /// <summary>

        /// unix时间转换为adatetime

        /// </summary>

        /// <param name="timeStamp"></param>

        /// <returns></returns>

        public static DateTime UnixTimeToTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }



        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        public static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }
        #region 产生随机数
        #region 产生N位随机数字
        /// <summary>
        /// 产生N位随机数字
        /// </summary>
        /// <returns></returns>
        public static string CreateNum(int n)
        {
            string result = "";
            Random r = new Random();
            StringBuilder sb;

            sb = new StringBuilder();
            for (int j = 0; j < n; j++)
            {
                sb.Append(r.Next(10));
            }
            result = sb.ToString();
            return result;

        }
        #endregion
        #region 产生N位随机数字
        /// <summary>
        /// 产生N位随机数字
        /// </summary>
        /// <returns></returns>
        public static string RandomCode(int N)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < N; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 数字和字母随机数
        /// <summary> 
        /// 数字和字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandCode(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
'0','1','2','3','4','5','6','7','8','9', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 字母随机数
        /// <summary> 
        /// 字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandLetter(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
'_', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 日期随机函数
        /// <summary> 
        /// 日期随机函数 
        /// </summary> 
        /// <param name="ra">长度</param> 
        /// <returns></returns> 
        public static string DateRndName(Random ra)
        {
            DateTime d = DateTime.Now;
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            s += y + m + dd + h + mm + ss;
            s += ra.Next(100, 999).ToString();
            return s;
        }
        #endregion
        #region 生成全局唯一标识符GUID
        /// <summary> 
        /// 生成全局唯一标识符GUID 
        /// </summary> 
        /// <returns></returns> 
        public static string GetGuid()
        {
            System.Guid g = System.Guid.NewGuid();
            return g.ToString();
        }
        #endregion
        #endregion
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }


        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
            }

            return sArray;



        }
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(SortedDictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        public static string CombinePath(params string[] path)
        {
            string fullPath = Path.Combine(path);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return fullPath;
        }

        public static string CombineUrl(params string[] urls)
        {
            if (urls == null || urls.Length == 0)
            {
                return null;
            }
            return urls.Aggregate((f, n) =>
            {
                if (n[0] != '/')
                {
                    return f.TrimEnd('/') + '/' + n;
                }
                return f.TrimEnd('/') + n;
            });
        }

        //public static string ClearHtml(string inputString)
        //{
        //    if (String.IsNullOrEmpty(inputString))
        //    {
        //        return null;
        //    }
        //    return Regex.Replace(inputString, @"<(.[^>]*)>", "");
        //}

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void RendJsBlock(string jsContent, HttpResponse res)
        {
            res.Write("<script type=\"text/javascript\">");
            res.Write(jsContent);
            res.Write("</script>");
        }

        //#region 获取真实IP by zhangsucai

        ///// <summary>
        ///// 获取真实IP
        ///// </summary>
        ///// <returns>返回IP字符串</returns>
        //public static string GetRealIP()
        //{
        //    string ip = "";
        //    string huancun;
        //    try
        //    {
        //        HttpRequest request = HttpContext.Current.Request;
        //        if (request.ServerVariables["HTTP_VIA"] != null)
        //        {
        //            //ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
        //            huancun = request.ServerVariables["x-forwarded-for"];
        //            if (huancun != null)
        //            {
        //                ip = huancun.ToString().Split(',')[0].Trim();
        //            }
        //        }
        //        else
        //        {
        //            ip = request.UserHostAddress;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //throw e;
        //        ip = "0.0.0.0";
        //    }

        //    return ip;

        //}
        //#endregion

        /// <summary>  
        /// 获取远程访问用户的Ip地址  
        /// </summary>  
        /// <returns>返回Ip地址</returns>  
        public static string GetRealIP()
        {
            string loginip = "";
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                //Request.ServerVariables[""]--获取服务变量集合   
                if (request.ServerVariables["REMOTE_ADDR"] != null) //判断发出请求的远程主机的ip地址是否为空  
                {
                    //获取发出请求的远程主机的Ip地址  
                    loginip = request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                //判断登记用户是否使用设置代理  
                else if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        //获取代理的服务器Ip地址  
                        loginip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        //获取客户端IP  
                        loginip = request.UserHostAddress;
                    }
                }
                else
                {
                    //获取客户端IP  
                    loginip = request.UserHostAddress;
                }
            }
            catch (Exception e)
            {
                loginip = "0.0.0.0";
            }
            return loginip;
        }

        #region 恶意代码过滤
        /// <summary>
        /// 防止恶意代码攻击
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterScript(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            return html;
        }
        #endregion

        #region 生成随机数
        public static string CreateRandom(int length)
        {
            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < length; i++)
            {
                randValue = rand.Next();

                code += randValue.ToString();
            }

            return code;
        }
        #endregion

        /// <summary>
        /// 汉字转拼音类
        /// </summary>

        private static int[] pyValue = new int[]
    {
    -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
    -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
    -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
    -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
    -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
    -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
    -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
    -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
    -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
    -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
    -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
    -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
    -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
    -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
    -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
    -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
    -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
    -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
    -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
    -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
    -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
    -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
    -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
    -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
    -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
    -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
    -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
    -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
    -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
    -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
    -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
    -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
    -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
    };

        private static string[] pyName = new string[]
    {
    "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
    "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
    "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
    "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
    "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
    "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
    "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
    "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
    "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
    "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
    "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
    "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
    "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
    "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
    "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
    "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
    "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
    "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
    "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
    "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
    "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
    "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
    "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
    "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
    "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
    "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
    "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
    "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
    "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
    "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
    "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
    "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
    "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
    };

        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string Convert(string hzString)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }
        /// <summary>
        /// 替换html标记
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string NoHTML(string html)
        {
            html = html.Replace("<p>", "").Replace("</p>", "\n");
            html = html.Replace("&nbsp;", " ");
            string StrNohtml = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
            return StrNohtml;
        }



        /// <summary>
        /// 替换HTML标记 功能增强代码
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string ExtendNoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        /// <summary>
        /// 只是要求aspx页面中含有所需打印的代码，其他标签代码不存在
        /// </summary>
        private void ClearHtml()
        {
            Response.Buffer = false;
            Response.Flush();
            Response.End();
            Response.Close();
        }

        /// <summary>
        ///计算两点GPS坐标的距离
        /// </summary>
        /// <param name="n1">第一点的纬度坐标</param>
        /// <param name="e1">第一点的经度坐标</param>
        /// <param name="n2">第二点的纬度坐标</param>
        /// <param name="e2">第二点的经度坐标</param>
        /// <returns></returns>
        public static double Distance(double n1, double e1, double n2, double e2)
        {
            double jl_jd = 102834.74258026089786013677476285;
            double jl_wd = 111712.69150641055729984301412873;
            double b = Math.Abs((e1 - e2) * jl_jd);
            double a = Math.Abs((n1 - n2) * jl_wd);
            return Math.Sqrt((a * a + b * b));

        }

        //验证电话号码的主要代码如下：

        public static bool IsTelephone(string str_telephone)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");

        }

        //验证手机号码的主要代码如下：

        public static bool IsMobile(string str_handset)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[1,2,3,4,5,6,7,8,9,0]+\d{9}");

        }

        //验证身份证号的主要代码如下：

        public static bool IsIDcard(string str_idcard)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");

        }

        //验证输入为数字的主要代码如下：

        public static bool IsNumber(string str_number)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");

        }

        //验证邮编的主要代码如下：

        public static bool IsPostalcode(string str_postalcode)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");

        }

        /// <summary> 
        /// 清除文本中Html的标签 
        /// </summary> 
        /// <param name="Content"></param> 
        /// <returns></returns> 
        public static string ClearHtml(string Content)
        {
            Content = Zxj_ReplaceHtml("&#[^>]*;", "", Content);
            Content = Zxj_ReplaceHtml("</?marquee[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?object[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?param[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?embed[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?table[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml(" ", "", Content);
            Content = Zxj_ReplaceHtml("</?tr[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?p[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?a[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?img[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?tbody[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?li[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?span[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?div[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?th[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?td[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?script[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("(javascript|jscript|vbscript|vbs):", "", Content);
            Content = Zxj_ReplaceHtml("on(mouse|exit|error|click|key)", "", Content);
            Content = Zxj_ReplaceHtml("<\\?xml[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("<\\/?[a-z]+:[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?font[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?b[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?u[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?i[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?strong[^>]*>", "", Content);
            Content = Zxj_ReplaceHtml("</?strong[^>]*>", "", Content);

            Content = Zxj_ReplaceHtml(" ", "", Content);
            Regex r = new Regex(@"\s+");
            Content = r.Replace(Content, "");

            Content.Trim();
            string clearHtml = Content;
            return clearHtml;
        }

        /// <summary> 
        /// 清除文本中的Html标签 
        /// </summary> 
        /// <param name="patrn">要替换的标签正则表达式</param> 
        /// <param name="strRep">替换为的内容</param> 
        /// <param name="content">要替换的内容</param> 
        /// <returns></returns> 
        private static string Zxj_ReplaceHtml(string patrn, string strRep, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
            Regex rgEx = new Regex(patrn, RegexOptions.IgnoreCase);
            string strTxt = rgEx.Replace(content, strRep);
            return strTxt;
        }



    }
}
