using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface ICache
    {
        Task RegistraAsync<T>(string key, T value, TimeSpan caducidad);
        Task<T> ResuelveAsync<T>(string key);
        Task BorraAsync(string key);
    }
}
