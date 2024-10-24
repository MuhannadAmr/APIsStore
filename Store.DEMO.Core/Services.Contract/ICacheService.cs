using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface ICacheService
    {
        Task SetCacheKeyAsnc(string key, object value, TimeSpan expireTime);
        Task<string> GetCacheKeyAsync(string key);
    }
}
