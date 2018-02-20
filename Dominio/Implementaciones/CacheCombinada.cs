using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Dominio.Implementaciones
{
    class CacheCombinado : ICache
    {
        public CacheCombinado(CacheRedis redis, CacheLocal local)
        {
            try
            {
                if (!redis.EstaConectado)
                    throw new ArgumentNullException("Redis no fue inicializado");
                this.redis = redis;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print($"Asignando cache sql debido a: {ex.Message}");
                sql = Aplicacion.Contenedor.Get<CacheSql>();
            }
            this.local = local;
        }

        ICache redis;
        ICache local;
        ICache sql;

        public async Task BorraAsync(string key)
        {
            if (local != null)
                await local.BorraAsync(key);
            if (redis != null)
                await redis.BorraAsync(key);
            if (sql != null)
                await sql.BorraAsync(key);
        }

        public async Task RegistraAsync<T>(string key, T value, TimeSpan caducidad)
        {
            if (local != null)
                await local.RegistraAsync(key, value, caducidad);
            if (redis != null)
                await redis.RegistraAsync(key, value, caducidad);
            if (sql != null)
                await sql.RegistraAsync(key, value, caducidad);
        }

        public async Task<T> ResuelveAsync<T>(string key)
        {
            T value = default(T);
            if (local != null)
                value = await local.ResuelveAsync<T>(key);
            if (value == null && redis != null)
            {
                value = await redis.ResuelveAsync<T>(key);
                if (local != null && value != null)
                    await local.RegistraAsync(key, value, Properties.Settings.Default.TiempoEnCacheLocal);
            }
            if (value == null && sql != null)
            {
                value = await sql.ResuelveAsync<T>(key);
                if (local != null && value != null)
                    await local.RegistraAsync(key, value, Properties.Settings.Default.TiempoEnCacheLocal);
            }
            return value;
        }
    }
}
