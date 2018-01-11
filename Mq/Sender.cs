using KTBDay.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTBDay.Mq
{
    public class Sender : IDisposable
    {
        private Client _client;
        private IModel _channel;

        public Sender(Client client)
        {
            _client = client;

            Initialize();
        }

        private void Initialize()
        {
            _channel = _client.Connection.CreateModel();
            _channel.ExchangeDeclare(_client.ConnectionConfig.Exchange, "fanout");
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(_client.ConnectionConfig.Exchange, "", null, body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
