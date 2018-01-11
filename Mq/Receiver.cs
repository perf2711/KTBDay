using KTBDay.Hubs;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace KTBDay.Mq
{
    public class Receiver : IDisposable
    {
        private IHubContext<MessengerHub> _hub;
        private Client _client;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private string _queueName;

        public Receiver(Client client, IHubContext<MessengerHub> hub)
        {
            _client = client;
            _hub = hub;

            Initialize();
        }

        private void Initialize()
        {
            _channel = _client.Connection.CreateModel();

            _channel.ExchangeDeclare(_client.ConnectionConfig.Exchange, "fanout");
            _queueName = _channel.QueueDeclare("").QueueName;
            _channel.QueueBind(_queueName, _client.ConnectionConfig.Exchange, "");

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                await _hub.Clients.All.InvokeAsync("Message", message);
            };

            _channel.BasicConsume(_queueName, true, _consumer);
        }

        public void Dispose()
        {
            _channel?.BasicCancel(_consumer.ConsumerTag);
            _channel?.Dispose();
        }
    }
}
