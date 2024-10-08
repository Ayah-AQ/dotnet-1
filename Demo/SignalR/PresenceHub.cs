﻿using Demo.Extintions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Demo.SignalR
{
        [Authorize]
    public class PresenceHub(PresenceTracker presenceTracker) : Hub
    {
        public async override Task OnConnectedAsync()
        {
            if (Context.User == null) throw new HubException("Can't get the user ");
            
                    
             var isOnline= await presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
          if(isOnline) await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetUsername());

            var currentUser = await presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUser);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User == null) throw new HubException("Can't get current user claim");
            var isOffline = await presenceTracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

            if(isOffline) await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());

            //var currentUser = await presenceTracker.GetOnlineUsers();
            //await Clients.All.SendAsync("GetOfflineUsers", currentUser);


            await base.OnDisconnectedAsync(exception);
        }





    }
}
