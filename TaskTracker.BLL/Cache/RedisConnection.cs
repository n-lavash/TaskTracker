using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.BLL.Cache
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        static RedisConnection()
        {
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(()
                => ConnectionMultiplexer.Connect("redis:6379"));
        }

        public static ConnectionMultiplexer Connection => _connectionMultiplexer.Value;
    }
}
