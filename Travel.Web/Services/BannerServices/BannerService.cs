using MongoDB.Driver;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.BannerServices
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Banner> _bannerCollection;

        public BannerService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _bannerCollection = database.GetCollection<Banner>(databaseSettings.BannerCollectionName);
        }

        public Task CreateAsync(CreateBannerDto createBannerDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultBannerDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultBannerDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            throw new NotImplementedException();
        }
    }
}
