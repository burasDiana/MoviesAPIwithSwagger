using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace TestWebAPI.Security
{
    public class MemoryCacher
    {
        public static object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public static bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }

        public static bool KeyExists(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Contains(key) ? true : false;
        }

        public static void Set(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(key, value, absExpiration);
        }
    }
}