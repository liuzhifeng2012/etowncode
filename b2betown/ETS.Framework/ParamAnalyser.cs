using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Framework
{
    public class ParamAnalyser
    {
        private string param;
        private string[] aryParam;
        private char separator;

        public ParamAnalyser(string param, char separator)
        {
            this.param = param;
            this.separator = separator;
            this.Init();
        }


        public ParamAnalyser(string param)
            : this(param, '-')
        {

        }

        private void Init()
        {
            if (String.IsNullOrEmpty(this.param))
            {
                return;
            }
            aryParam = param.Split(new char[] { separator });
        }

        public T Get<T>(int pos)
        {
            if (aryParam == null || aryParam.Length == 0 || aryParam.Length < pos)
            {
                return default(T);
            }

            try
            {
                return this.aryParam[pos].ConvertTo<T>();
                
            }
            catch
            {
                return default(T);
            }
        }

        public T Get<T>(int ops, char split, int childOps)
        {
            string param = this.Get<string>(ops);
            var subSplit = new ParamAnalyser(param, split);
            return subSplit.Get<T>(childOps);
        }
    }
}
