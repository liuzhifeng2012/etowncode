using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;


namespace ETS2.Common.Business
{
    public class Domain_def
    {

        public static bool Domain_yanzheng(string domain)
        {
            string RegexText = "shop\\d{1,}.etown.cn";
            return Regex.IsMatch(domain, RegexText);
        }

       
        public static string Domain_Huoqu(string domain)
        {
            string RegexText = "\\d{1,}";
            return Regex.Match(domain, RegexText).ToString();
        }

        //验证
        //验证数字：^[0-9]*$  
        //验证n位的数字：^\d{n}$ 
        //验证至少n位数字：^\d{n,}$  验证
        //m-n位的数字：^\d{m,n}$  
        //验证零和非零开头的数字：^(0|[1-9][0-9]*)$ 
        //验证有两位小数的正实数：^[0-9]+(.[0-9]{2})?$ 
        //验证有1-3位小数的正实数：^[0-9]+(.[0-9]{1,3})?$ 
        //验证非零的正整数：^\+?[1-9][0-9]*$ 
        //验证非零的负整数：^\-[1-9][0-9]*$ 
        //验证非负整数（正整数 + 0）  ^\d+$ 
        //验证非正整数（负整数 + 0）  ^((-\d+)|(0+))$ 
        //验证长度为3的字符：^.{3}$  
        //验证由26个英文字母组成的字符串：^[A-Za-z]+$ 
        //验证由26个大写英文字母组成的字符串：^[A-Z]+$ 
        //验证由26个小写英文字母组成的字符串：^[a-z]+$  
        //验证由数字和26个英文字母组成的字符串：^[A-Za-z0-9]+$  
        //验证由数字、26个英文字母或者下划线组成的字符串：^\w+$  
        //验证用户密码:^[a-zA-Z]\w{5,17}$ 正确格式为：以字母开头，长度在6-18之间，只能包含字符、数字和下划线。
        //验证是否含有 ^%&',;=?$\" 等字符：[^%&',;=?$\x22]+ 
        //验证汉字：^[\u4e00-\u9fa5],{0,}$  
        //验证Email地址：^\w+[-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$ 
        //验证InternetURL：^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$ ；^[a-zA-z]+://(w+(-w+)*)(.(w+(-w+)*))*(?S*)?$  
        //验证电话号码：^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$：
        //验证身份证号（15位或18位数字）：^\d{15}|\d{}18$  
        public static bool RegexValidate(string regexString,string validateString) 
        {  
            Regex regex = new Regex(regexString);  
            return regex.IsMatch(validateString.Trim());             
        }

        //获取一个随机数+日期，做为用户的id 共22位
        public static string HuoQu_Temp_UserId()
        {
          
            string datenow = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            Random ra = new Random();
            string suijishu = ra.Next(10000, 99999).ToString();
            return datenow + suijishu;
        }

    }
}
