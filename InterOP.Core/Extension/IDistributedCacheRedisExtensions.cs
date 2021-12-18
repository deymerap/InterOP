using InterOP.Core.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterOP.Core.Extension
{
    public static class IDistributedCacheRedisExtensions
    {
        public static async Task<IList<EntProveedores>> GetSearchResultsAsync(this IDistributedCache pvCache, string pvSearchId)
        {
            return await pvCache.GetAsync<IList<EntProveedores>>(pvSearchId);
        }
        public static async Task AddSearchResultsAsync(this IDistributedCache pvCache, string searchId, IEnumerable<EntProveedores> pvEntProveedores)
        {
            var vObOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
            await pvCache.SetAsync(searchId, pvEntProveedores, vObOptions);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache pvCache, string pvKey) where T : class
        {
            var vObJSon = await pvCache.GetStringAsync(pvKey);
            if (vObJSon == null)
                return null;

            return JsonConvert.DeserializeObject<T>(vObJSon);
        }
        public static async Task SetAsync<T>(this IDistributedCache pvCache, string pvKey, T pvValue, DistributedCacheEntryOptions pvOptions) where T : class
        {
            var vJSon = JsonConvert.SerializeObject(pvValue);
            await pvCache.SetStringAsync(pvKey, vJSon, pvOptions);
        }
    }
}
