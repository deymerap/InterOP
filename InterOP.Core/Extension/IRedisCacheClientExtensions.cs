using InterOP.Core.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Extension
{
    public static class IRedisCacheClientExtensions
    {
        //public static async Task<IList<EntProveedores>> GetSearchResultsAsync(this IRedisDatabase pvCache, string pvSearchId)
        //{
        //    return await pvCache.getAsync<IList<EntProveedores>>(pvSearchId);
        //}

        //public static async Task<T> getAsync<T>(this IRedisDatabase pvCache, string pvKey) 
        //{
        //    var vObJSon = await pvCache.GetAsync<T>(pvKey);
        //    if (vObJSon == null)
        //        return vObJSon;

        //    return vObJSon;// JsonConvert.DeserializeObject<T>(vObJSon);
        //}

        //public static async Task<IList<EntProveedores>> GetSearchResultsAsync(this IRedisDatabase pvCache, string pvSearchId)
        //{
        //    return await pvCache.Database.GetAsync(pvSearchId);
        //}

        ////public static async Task<T> getAsync<T>(this IRedisDatabase pvCache, string pvKey) where T : class
        ////{
        ////    var vObJSon = await pvCache.GetAsync<T>(pvKey);
        ////    if (vObJSon == null)
        ////        return null;

        ////    return vObJSon;
        ////}

        //public static async Task<RedisValue> GetAsync(this IRedisDatabase pvCache, RedisKey key, CommandFlags flags = CommandFlags.None)
        //{
        //    var vObJSon = (string)await pvCache.Database.StringGetAsync(key);

        //    if (vObJSon == null)
        //        return null;

        //    return vObJSon;
        //}


        //public static async Task SetAsync<T>(this IRedisCacheClient pvCache, string pvKey, T pvValue, DistributedCacheEntryOptions pvOptions) where T : class
        //{
        //    var vJSon = JsonConvert.SerializeObject(pvValue);
        //    await pvCache.SetStringAsync(pvKey, vJSon, pvOptions);
        //}

        //public void Remove(this IRedisCacheClient pvCache, string key)
        //{
        //    if (key == null)
        //    {
        //        throw new ArgumentNullException(nameof(key));
        //    }

        //    Connect();

        //    pvCache.(_instance + key);
        //    // TODO: Error handling
        //}
    }
}
