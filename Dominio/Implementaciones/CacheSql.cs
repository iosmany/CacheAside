using Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Implementaciones
{
    class CacheSql : ICache
    {
        public async Task BorraAsync(string key)
        {
            using (var ctx = Aplicacion.CreaContexto())
            {
                var value = await ctx.DatosPersistidos.FindAsync(key);
                if (value != null)
                {
                    ctx.Entry(value).State = System.Data.Entity.EntityState.Deleted;
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public async Task RegistraAsync<T>(string key, T value, TimeSpan caducidad)
        {
            using (var ctx = Aplicacion.CreaContexto())
            {
                var dato = await ctx.DatosPersistidos.FindAsync(key);
                if (dato == null)
                    ctx.DatosPersistidos.Add(new DatoPersistido()
                    {
                        Key = key,
                        Value = JsonConvert.SerializeObject(value),
                        Caducidad = DateTime.Now.Add(caducidad)
                    });
                else
                {
                    dato.Value = JsonConvert.SerializeObject(value);
                    dato.Caducidad = DateTime.Now.Add(caducidad);
                }
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<T> ResuelveAsync<T>(string key)
        {
            using (var ctx = Aplicacion.CreaContexto())
            {
                var dato = await ctx.DatosPersistidos.FindAsync(key);
                if (dato != null)
                    return JsonConvert.DeserializeObject<T>(dato.Value);
                return default(T);
            }
        }
    }
}
