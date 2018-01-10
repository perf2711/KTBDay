using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace KTBDay.Hubs
{
    public class MessengerHub : Hub
    {
        public Task Message(string message)
        {
            return Clients.All.InvokeAsync("Message", message);
        }
    }
}
