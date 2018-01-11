using KTBDay.Models;
using RabbitMQ.Client;
using System;

namespace KTBDay.Mq
{
    public class Client : IDisposable
    {
        public IConnection Connection { get; private set; }
        public MqConnectionViewModel ConnectionConfig { get; }

        public Client(MqConnectionViewModel connectionConfig)
        {
            ConnectionConfig = connectionConfig;

            Initialize();
        }

        private void Initialize()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = ConnectionConfig.Hostname,
                UserName = ConnectionConfig.Username,
                Password = ConnectionConfig.Password
            };

            Connection = connectionFactory.CreateConnection();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
