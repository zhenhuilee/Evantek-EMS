namespace EMS.API.Hubs.IHubs
{
    public interface IDisplayHub
    {
        Task SendMessage(string user, string message);
        Task UpdateUserStatus(int userId, string statusName ,string note, DateTime lastUpdated);

        Task AddUser(int userId, string userName, DateTime lastUpdated);

        Task EditUser(int userId,  string username, string statusName, string note, DateTime lastUpdated);

        Task DeleteUser(int userId);
        
    }
}