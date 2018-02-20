using Dominio.Implementaciones;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public static class Aplicacion
    {
        internal static Func<CacheSqlContexto> CreaContexto = () => new CacheSqlContexto();
        private static IKernel contenedor;

        public static IKernel Contenedor
        {
            get
            {
                if (contenedor == null)
                    contenedor = new StandardKernel();
                return contenedor;
            }
        }

        public static void Inicializa()
        {
            InitIoC();
            //init db
            using (var ctx = CreaContexto())
            {
                ctx.Database.CreateIfNotExists();
            }
        }

        static void InitIoC()
        {
            Contenedor.Bind<RedisDb>().ToSelf().InSingletonScope();
            Contenedor.Bind<CacheLocal>().ToSelf().InSingletonScope();
            Contenedor.Bind<CacheSql>().ToSelf().InSingletonScope();
            Contenedor.Bind<ICache>().To(ResuelveTipoCache());
        }

        static Type ResuelveTipoCache()
        {
            switch (Properties.Settings.Default.TipoCache)
            {
                case "redis":
                    return typeof(CacheRedis);
                case "memoria":
                    return typeof(CacheLocal);
                case "combinado":
                    return typeof(CacheCombinado);
                default:
                    return typeof(CacheSql);
            }
        }
    }
}
