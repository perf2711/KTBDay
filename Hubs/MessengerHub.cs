using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using KTBDay.Mq;

namespace KTBDay.Hubs
{
    public class MessengerHub : Hub
    {
        private Sender _sender;

        public MessengerHub(Sender sender)
        {
            _sender = sender;
        }

        public Task Message(string message)
        {
            _sender.SendMessage(message);
            return Clients.All.InvokeAsync("Message", message);
        }
    }
}
