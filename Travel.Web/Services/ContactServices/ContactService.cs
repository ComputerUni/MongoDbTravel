using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.ContactDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Contact> _contactCollection;
        private readonly IMapper _mapper;

        public ContactService(IDatabaseSettings databaseSettings ,IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _contactCollection = database.GetCollection<Contact>(databaseSettings.ContactCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateContactDto createContactDto)
        {
            var contact = _mapper.Map<Contact>(createContactDto);
            await _contactCollection.InsertOneAsync(contact);
        }

        public async Task DeleteAsync(string id)
        {
            await _contactCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultContactDto>> GetAllAsync()
        {
            var items = await _contactCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultContactDto>>(items);
        }

        public async Task<ResultContactDto> GetByIdAsync(string id)
        {
            var item = await _contactCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultContactDto>(item);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<Contact>.Update.Set(x => x.IsRead, true);
            await _contactCollection.UpdateOneAsync(x => x.Id == id, update);
        }
    }
}
