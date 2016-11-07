using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR2.Hubs
{
    [HubName("chat")]
    public class ChatHub : Hub<IClientHandler>
    {
        static List<String> ChatLog = new List<string>();
        static List<IdName> UserName = new List<IdName>();
        static int GuestCount = 0;

        private bool GuestFlag = true;

        public void SendToAll(string msg)
        {
            ChatLog.Add(msg);
            Clients.Others.Recived(msg);
        }

        public void SetName(String name)
        {
            for (var i = 0; i < UserName.Count; ++i)
            {
                if (UserName[i].Id == Context.ConnectionId)
                {
                    UserName[i] = new IdName(Context.ConnectionId, name);
                    if (GuestFlag)
                    {
                        GuestCount--;
                        this.GuestFlag = false;
                    }

                    break;
                }
            }
            Clients.All.UpdateNameList(UserName);
        }

        public override Task OnConnected()
        {
            GuestCount++;
            UserName.Add(new IdName(Context.ConnectionId, String.Format("Guest{0}", GuestCount)));
            Clients.Caller.GetChatLog(ChatLog);
            Clients.All.UpdateNameList(UserName);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            for(var i = 0 ; i < UserName.Count; ++i)
            {
                if(UserName[i].Id == Context.ConnectionId)
                {
                    UserName.RemoveAt(i);
                }
            }

            if (GuestFlag && GuestCount > 0)
            {
                GuestCount--;
            }

            Clients.All.UpdateNameList(UserName);

            return base.OnDisconnected(stopCalled);
        }
    }

    public struct IdName
    {
        public String Id;
        public String Name;

        public IdName(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }

    public interface IClientHandler
    {
        void Recived(string msg);
        void GetChatLog(List<String> ChatLog);
        void UpdateNameList(List<IdName> UserName);
    }
}