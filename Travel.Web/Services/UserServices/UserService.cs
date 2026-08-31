using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMongoCollection<AppUser> _userCollection;

        public UserService(UserManager<AppUser> userManager, IDatabaseSettings databaseSettings)
        {
            _userManager = userManager;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<AppUser>(databaseSettings.UserCollectionName);
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task SetActiveAsync(string id)
        {
            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, Guid.Parse(id));
            var update = Builders<AppUser>.Update.Set(u => u.LockoutEnd, null).Set(u => u.LockoutEnabled, false);

            await _userCollection.UpdateOneAsync(filter, update);

        }


        public async Task SetPassiveAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
    }
}
