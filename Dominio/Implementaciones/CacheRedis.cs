using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dominio.Implementaciones
{
    class CacheRedis : ICache
    {
        public CacheRedis(RedisDb redis)
        {
            if(redis.Connection != null)
                db = redis.Connection.GetDatabase();
        }

        IDatabase db;
        const string redisKey = "rds-";

        internal bool EstaConectado => db != null;

        public async Task BorraAsync(string key)
        {
            await db.KeyDeleteAsync(redisKey + key);
        }

        public async Task RegistraAsync<T>(string key, T value, TimeSpan caducidad)
        {
            await db.StringSetAsync(redisKey + key, JsonConvert.SerializeObject(value), caducidad);
        }

        public async Task<T> ResuelveAsync<T>(string key)
        {
            var json = await db.StringGetAsync(redisKey + key);
            if (json.IsNull)
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
