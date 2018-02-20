using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Implementaciones
{
    class RedisDb
    {
        public RedisDb()
        {
           
            try
            {
                //onn = ConnectionMultiplexer.Connect("");
                conn = ConnectionMultiplexer.Connect(Properties.Settings.Default.RedisCon);
            }
            catch { }
            finally { SubscribirEventos(); }
        }

        ConnectionMultiplexer conn;
        public ConnectionMultiplexer Connection => conn;

        void SubscribirEventos()
        {
            if (conn != null)
            {
                conn.ConfigurationChanged += (sender, args) => { };
                conn.ConfigurationChangedBroadcast += (sender, args) => { };
                conn.ConnectionFailed += (sender, args) => { };
                conn.ConnectionRestored += (sender, args) => { };
                conn.ErrorMessage += (sender, args) => { };
                conn.HashSlotMoved += (sender, args) => { };
                conn.InternalError += (sender, args) => { };
            }
        }
    }
}
