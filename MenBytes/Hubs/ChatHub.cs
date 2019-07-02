using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MenBytes.Models.Context;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace MenBytes.Hubs
{
    public class ChatHub:Hub
    {

        public void JoinRoom(string room)
        {
            Groups.Add(Context.ConnectionId,room);
        }
        public override Task OnConnected()
        {

            return base.OnConnected();
        }

        //public void NewMessage(string UserName, string Message)
        //{
        //    string userId = Context.User.Identity.GetUserId();

        //    string name = new ApplicationDbContext().Users.Where(x => x.Id == userId).SingleOrDefault().user_Name;
        //    Clients.Group("Chat/Admin").sentMessage(name, Message, DateTime.Now);
        //}


        public void NewMessage(string UserName, string Message)
        {
            string name= String.Empty;
            string userId = Context.User.Identity.GetUserId();
            if (userId != null)
            {
                name = new ApplicationDbContext().Users.Where(x => x.Id == userId).SingleOrDefault().user_Name;
            }
            
            Clients.Others.sentMessage(name, Message, DateTime.Now);
        }
    }
}