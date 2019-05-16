using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace ETS.Framework
{
    public class MemoryCacheHelper
    {
        private static MemoryCache memoryCache = MemoryCache.Default;
        private static readonly object objLock = new object();


        public static T Get<T>(string key, Func<T> source, CacheItemPolicy policy = null)
        {
            var obj = memoryCache.Get(key);
            if (null == obj)
            {
                lock (objLock)
                {
                    obj = memoryCache.Get(key);
                    if (obj == null)
                    {
                        T value = source();
                        if (null == value)
                        {
                            return default(T);
                        }
                        CacheItem item = new CacheItem(key, value);
                        memoryCache.Set(item, policy);
                        return value;
                    }
                }
            }
            return (T)obj;
        }

        public static CacheItemPolicy TimeExpirePolicy(int minutes)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);
            return policy;
        }

        public static CacheItemPolicy FilePolicy(params string[] files)
        {
            var itemProlicy = new CacheItemPolicy();
            itemProlicy.ChangeMonitors.Add(new HostFileChangeMonitor(files));
            return itemProlicy;
        }
        public static bool removeAll()
        {

            //MemoryCache.Default.Remove()
            return true;
        }
    }




}
