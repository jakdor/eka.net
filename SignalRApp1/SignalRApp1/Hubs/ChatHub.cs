using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRApp1.Hubs
{
    public static class UserRepository
    {
        public static ConcurrentDictionary<string, IPrincipal> UserIds { get; } =
            new ConcurrentDictionary<string, IPrincipal>();

    }
    [HubName("chat")]
    public class ChatHub : Hub<IClientHandler>
    {
        public void SendMessage(string msg)
        {
            Clients.All.ShowMessage(msg);
        }

        public override Task OnReconnected()
        {
            UserRepository.UserIds.AddOrUpdate(Context.ConnectionId, Context.User, (key, oldUser) => Context.User);
            return base.OnReconnected();
        }

        public override Task OnConnected()
        {
            UserRepository.UserIds.TryAdd(Context.ConnectionId, Context.User);
            Clients.All.UpdateNumberOfClients(UserRepository.UserIds.Count);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            IPrincipal principal;
            UserRepository.UserIds.TryRemove(Context.ConnectionId, out principal);
            Clients.All.UpdateNumberOfClients(UserRepository.UserIds.Count);
            return base.OnDisconnected(stopCalled);
        }
    }

    public interface IClientHandler
    {
        void UpdateNumberOfClients(int msg);
        void ShowMessage(string msg);
    }
}