using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    [HubName("puns")]
    public class PunsHub : Hub<IPunsClientHandler>
    {
        public void SendPath(string path)
        {
            Clients.Others.DrawPath(path);
        }
    }

    public interface IPunsClientHandler
    {
        void DrawPath(string path);
    }
}