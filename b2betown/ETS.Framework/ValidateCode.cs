using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;
using System.IO;

namespace ETS.Framework
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class ValidateCode
    {
        /// <summary>
        /// 字符类型
        /// </summary>
        [Flags]
        public enum En_CharKind
        {
            /// <summary>
            /// 汉字（默认，不可和其他字符类型枚举合用）
            /// </summary>
            Chinese = 0,
            /// <summary>
            /// 大写字母
            /// </summary>
            Upper = 1,
            /// <summary>
            /// 小写字母
            /// </summary>
            Lower = 2,
            /// <summary>
            /// 数字
            /// </summary>
            Number = 4
        }

        /// <summary>
        /// 验证码配置信息
        /// </summary>
        public class ValidateCodeConfiger
        {
            /// <summary>
            /// 字体大小
            /// </summary>
            public int FontSize { get; set; }
            /// <summary>
            /// 字符间隔
            /// </summary>
            public int CharSpacing { get; set; }
            /// <summary>
            /// 图片长
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// 图片宽
            /// </summary>
            public int Height { get; set; }
            /// <summary>
            /// 字符个数
            /// </summary>
            public int CharNum { get; set; }

            /// <summary>
            /// 字符类型
            /// </summary>
            public En_CharKind CharKind { get; set; }

            /// <summary>
            /// 噪点（线）数
            /// </summary>
            public NoiseLine Noise { get; set; }

            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color BackgroundColor { get; set; }

            /// <summary>
            /// 边框颜色
            /// </summary>
            public Color BorderColor { get; set; }

            /// <summary>
            /// 获取一个验证码配置信息实例
            /// </summary>
            /// <returns></returns>
            public static ValidateCodeConfiger GetInstance()
            {
                ValidateCodeConfiger configer = new ValidateCodeConfiger();

                configer.CharNum = 4;
                configer.FontSize = 14;
                configer.CharSpacing = 4;
                configer.Width = (configer.CharNum * configer.FontSize) + ((configer.CharNum) * configer.CharSpacing);
                configer.Height = configer.FontSize + 6;

                //configer.CharKind = En_CharKind.Chinese;
                configer.CharKind = En_CharKind.Number;
                configer.Noise = NoiseLine.GetInstance();

                configer.BorderColor = Color.Black;
                configer.BackgroundColor = Color.White;

                return configer;
            }
        }

        /// <summary>
        /// 背景噪线
        /// </summary>
        public class NoiseLine
        {
            public NoiseLine()
            {
                BorderColor = Color.Silver;
            }

            /// <summary>
            /// 线的宽度
            /// </summary>
            public int BorderWidth { get; set; }
            /// <summary>
            /// 线的颜色
            /// </summary>
            public Color BorderColor { get; set; }
            /// <summary>
            /// 线的数量
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// 获取一个背景噪线实例
            /// </summary>
            /// <returns></returns>
            public static NoiseLine GetInstance()
            {
                return new NoiseLine()
                {
                    BorderColor = Color.LightGray,
                    BorderWidth = 1,
                    Count = 25
                };
            }
        }

        //////////////////////////////////////////////////////////////////////////////

        #region 成员
        /// <summary>
        /// 验证码配置
        /// </summary>
        public ValidateCodeConfiger Configer { get; set; }

        /// <summary>
        /// 验证码字符串
        /// </summary>
        private string _codeString;

        /// <summary>
        /// 验证码图片
        /// </summary>
        private Bitmap _codeImg;

        /// <summary>
        /// 验证码输出流
        /// </summary>
        private MemoryStream _codeStream;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public ValidateCode()
        {
            Configer = ValidateCodeConfiger.GetInstance();
            CreateCode();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="configer">验证码配置</param>
        public ValidateCode(ValidateCodeConfiger configer)
        {
            Configer = configer;
            AmendConfiger();
            CreateCode();
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 重新加载缩略图
        /// </summary>
        public void Reload()
        {
            CreateCode();
        }

        /// <summary>
        /// 获取验证码字符串
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            return _codeString;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        public Bitmap GetImg()
        {
            return _codeImg;
        }

        /// <summary>
        /// 获取验证码输出流
        /// </summary>
        public MemoryStream GetStream()
        {
            return _codeStream;
        }

        /// <summary>
        /// 获取Byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteArray()
        {
            return _codeStream.ToArray();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 修正配置信息
        /// </summary>
        private void AmendConfiger()
        {
            if (Configer.CharNum <= 0)
            {
                Configer.CharNum = 4;
            }
            if (Configer.FontSize <= 0)
            {
                Configer.FontSize = 14;
            }
            if (Configer.CharSpacing <= 0)
            {
                Configer.CharSpacing = 4;
            }
            if (Configer.Width <= 0)
            {
                Configer.Width = (Configer.CharNum * Configer.FontSize) + ((Configer.CharNum) * Configer.CharSpacing);
            }
            if (Configer.Height <= 0)
            {
                Configer.Height = Configer.FontSize + 6;
            }
            if (Configer.Noise == null)
            {
                Configer.Noise = NoiseLine.GetInstance();
            }
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        private void CreateCode()
        {
            // 获取验证码字符串
            if (Configer.CharKind == En_CharKind.Chinese)
            {
                // 生成中文字符串
                _codeString = GetCnCodeString();
            }
            else
            {
                // 生成英文、数字字符串
                _codeString = GetEnNumCodeString();
            }

            // 获取验证码图片
            _codeImg = CreateImages();
            // 获取验证码输出流
            _codeStream = GetCodeStream();
        }

        /// <summary>
        ///  获取英文、数字字符串
        /// </summary>
        /// <returns></returns>
        private string GetEnNumCodeString()
        {
            string numChars = "0123456789";
            string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int maxIndex = -1;
            string charsToBeChoiced = "";
            string charKinds = "," + Configer.CharKind.ToString().Replace(" ", "") + ",";

            if (charKinds.IndexOf("," + En_CharKind.Number.ToString() + ",") >= 0)
            {
                charsToBeChoiced += numChars;
                maxIndex += numChars.Length;
            }
            if (charKinds.IndexOf("," + En_CharKind.Lower.ToString() + ",") >= 0)
            {
                charsToBeChoiced += lowerChars;
                maxIndex += lowerChars.Length;
            }
            if (charKinds.IndexOf("," + En_CharKind.Upper.ToString() + ",") >= 0)
            {
                charsToBeChoiced += upperChars;
                maxIndex += upperChars.Length;
            }

            Random rand = new Random();
            string re = "";
            for (int i = 0; i < Configer.CharNum; i++)
            {
                re += charsToBeChoiced.Substring(rand.Next(maxIndex), 1);
            }
            return re;
        }

        /// <summary>
        /// 获取中文验证码字符串
        /// </summary>
        /// <returns></returns>
        private string GetCnCodeString()
        {
            // 获取编码
            System.Text.Encoding encoding = System.Text.Encoding.Default;

            // 产生随机中文汉字编码
            object[] bytes = CreateCnCode();


            string[] str = new string[Configer.CharNum];

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < Configer.CharNum; i++)
            {
                // 根据汉字编码的字节数组解码出中文汉字
                str[i] = encoding.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
                sb.Append(str[i].ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 产生随机中文汉字编码
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
        private object[] CreateCnCode()
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random rnd = new Random();
            object[] bytes = new object[Configer.CharNum];

            for (int i = 0; i < Configer.CharNum; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);

                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <returns></returns>
        private Bitmap CreateImages()
        {
            // 创建位图
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(Configer.Width, Configer.Height);
            // 创建画板
            Graphics graphics = Graphics.FromImage(image);
            // 清除背景色
            graphics.Clear(Configer.BackgroundColor);
            // 前景颜色
            Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            // 随机数生成器
            Random rand = new Random();

            // 画背景线
            for (int i = 0; i < Configer.Noise.Count; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                int x1 = rand.Next(image.Width);
                int x2 = rand.Next(image.Width);
                int y1 = rand.Next(image.Height);
                int y2 = rand.Next(image.Height);
                graphics.DrawLine(new Pen(Configer.Noise.BorderColor, Configer.Noise.BorderWidth), x1, y1, x2, y2);
            }

            // 画字符
            int colorMaxIndex = colors.Length - 1;
            float fontSizePt = ((float)(Configer.FontSize * 3) / 4);
            float preX = ((float)((Configer.FontSize + Configer.CharSpacing * 2) * 3) / 4);
            float preY = 3;
            for (int i = 0; i < Configer.CharNum; i++)
            {
                int cindex = rand.Next(colorMaxIndex);
                Font font = new System.Drawing.Font("宋体", fontSizePt);
                Brush brush = new System.Drawing.SolidBrush(colors[cindex]);
                graphics.DrawString(_codeString.Substring(i, 1), font, brush, (i * preX), preY);
            }

            // 画边框
            if (Configer.BorderColor != null)
            {
                graphics.DrawRectangle(new Pen(Configer.BorderColor, 0), 0, 0, image.Width - 1, image.Height - 1);
            }

            // 返回图片
            return image;
        }

        /// <summary>
        /// 获取验证码输出流
        /// </summary>
        /// <returns></returns>
        private MemoryStream GetCodeStream()
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                _codeImg.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch
            {
                stream = null;
            }
            return stream;
        }


        #endregion
    }

     
}
