using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.LookupDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.LookupServices
{
    public class LookupService : ILookupService
    {
        private readonly IMongoCollection<LookupItem> _lookupItemCollection;
        private readonly IMapper _mapper;

        public LookupService(IDatabaseSettings databaseSettings ,IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _lookupItemCollection = database.GetCollection<LookupItem>(databaseSettings.LookupItemCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateLookupDto createLookupDto)
        {
            var lookup = _mapper.Map<LookupItem>(createLookupDto);
            await _lookupItemCollection.InsertOneAsync(lookup);
        }

        public async Task DeleteAsync(string id)
        {
            await _lookupItemCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultLookupDto>> GetAllAsync()
        {
            var lookups = await _lookupItemCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultLookupDto>>(lookups);
        }

        public async Task<ResultLookupDto> GetByIdAsync(string id)
        {
            var lookup = await _lookupItemCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultLookupDto>(lookup);
        }

        public async Task UpdateAsync(UpdateLookupDto updateLookupDto)
        {
            var lookup = _mapper.Map<LookupItem>(updateLookupDto);
            await _lookupItemCollection.FindOneAndReplaceAsync(x => x.Id == lookup.Id, lookup);
        }

        async Task<List<ResultLookupDto>> ILookupService.GetByTypeAsync(string type)
        {
            var lookups = await _lookupItemCollection.Find(x => x.Type == type).ToListAsync();
            return _mapper.Map<List<ResultLookupDto>>(lookups);
        }
    }
}
