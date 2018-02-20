using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Ninject;

namespace CacheAside
{
    class Program
    {
        static readonly string cacheKey = "prod-";
        static void Main(string[] args)
        {
            Aplicacion.Inicializa();
            ICache cache = Aplicacion.Contenedor.Get<ICache>();

            Task[] taskOnCache = new Task[]
            {
                cache.RegistraAsync(cacheKey + 1, new Producto() { Id = 1, Nombre = "Producto1" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 2, new Producto() { Id = 2, Nombre = "Producto2" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 3, new Producto() { Id = 3, Nombre = "Producto3" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 4, new Producto() { Id = 4, Nombre = "Producto4" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 5, new Producto() { Id = 5, Nombre = "Producto5" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 6, new Producto() { Id = 6, Nombre = "Producto6" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 7, new Producto() { Id = 7, Nombre = "Producto7" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 8, new Producto() { Id = 8, Nombre = "Producto8" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 9, new Producto() { Id = 9, Nombre = "Producto9" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 10, new Producto() { Id = 10, Nombre = "Producto10" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 11, new Producto() { Id = 11, Nombre = "Producto11" }, TimeSpan.FromMinutes(5)),
                cache.RegistraAsync(cacheKey + 12, new Producto() { Id = 12, Nombre = "Producto12" }, TimeSpan.FromMinutes(5))
            };
            Task.WaitAll(taskOnCache);

            for (int i = 1; i < 13; i++)
            {
                Producto p = cache.ResuelveAsync<Producto>(cacheKey + i).GetAwaiter().GetResult();
                if(p != null)
                    Console.WriteLine(p.ToString());
            }

            Console.ReadLine();
        }
    }

    class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Nombre}";
        }
    }
}
