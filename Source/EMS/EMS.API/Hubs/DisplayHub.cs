using EMS.API.Hubs.IHubs;
using Microsoft.AspNetCore.SignalR;
namespace EMS.API.Hubs
{
    public class DisplayHub : Hub<IDisplayHub>
    {
        public async Task SendAsync(string user, string message)
        {
            await Clients.All.SendMessage(user, message);
        }


        public async Task UpdateUserStatusAsync(int userId, string statusName, string note, DateTime lastUpdated)
        {
            await Clients.All.UpdateUserStatus(userId, statusName, note, lastUpdated);
        }


        public async Task AddUser( int userId, string userName, DateTime lastUpdated)
        {
           await Clients.All.AddUser(userId, userName, lastUpdated);
        }

        public async Task EditUser(int userId,   string userName, string statusName, string note, DateTime lastUpdated)
        {
            await Clients.All.EditUser(userId,  userName, statusName,  note, lastUpdated);
        }

        public async Task DeleteUser(int userId)
        {
            await Clients.All.DeleteUser(userId);
        }

        public override Task OnConnectedAsync()
        {
            //LogDebug($"({Context.ConnectionId}) connected to notification hub.");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //LogDebug($"({Context.ConnectionId}) disconnected to notification hub.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
