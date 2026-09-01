using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.WhyUsItemDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.IWhyUsServices
{
    public class WhyUsService : IWhyUsService
    {
        private readonly IMongoCollection<WhyUsItem> _whyUsCollection;
        private readonly IMapper _mapper;

        public WhyUsService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _whyUsCollection = database.GetCollection<WhyUsItem>(databaseSettings.WhyUsItemCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateWhyUsItemDto createWhyUsDto)
        {
            var whyUs = _mapper.Map<WhyUsItem>(createWhyUsDto);
            await _whyUsCollection.InsertOneAsync(whyUs);
        }

        public async Task DeleteAsync(string id)
        {
            await _whyUsCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultWhyUsItemDto>> GetAllAsync()
        {
            var items = await _whyUsCollection.AsQueryable().ToListAsync();
            return _mapper.Map<List<ResultWhyUsItemDto>>(items);
        }

        public async Task<UpdateWhyUsItemDto> GetByIdWhyUsAsync(string id)
        {
            var item = await _whyUsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UpdateWhyUsItemDto>(item);
        }

        public async Task UpdateAsync(UpdateWhyUsItemDto updateWhyUsDto)
        {
            var item = _mapper.Map<WhyUsItem>(updateWhyUsDto);
            await _whyUsCollection.FindOneAndReplaceAsync(x => x.Id == item.Id, item);
        }
    }
}
