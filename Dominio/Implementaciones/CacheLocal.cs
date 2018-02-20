using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Implementaciones
{
    class CacheLocal : ICache
    {
        public CacheLocal()
        {
            mc = MemoryCache.Default;
        }

        MemoryCache mc;

        public Task RegistraAsync<T>(string key, T value, TimeSpan caducidad)
        {
            mc.Add(key, value, DateTime.Now.Add(caducidad));
            return Task.CompletedTask;
        }

        public Task<T> ResuelveAsync<T>(string key)
        {
            return Task.FromResult((T)mc.Get(key));
        }

        public Task BorraAsync(string key)
        {
            mc.Remove(key);
            return Task.CompletedTask;
        }
    }
}
