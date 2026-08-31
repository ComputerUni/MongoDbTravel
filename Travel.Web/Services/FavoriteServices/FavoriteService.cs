using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.FavoriteDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.FavoriteServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMongoCollection<Favorite> _favoriteCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMapper _mapper;

        public FavoriteService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _favoriteCollection = database.GetCollection<Favorite>(databaseSettings.FavoriteCollectionName);
            _commentCollection = database.GetCollection<Comment>(databaseSettings.CommentCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
            _mapper = mapper;
        }

        public async Task AddAsync(CreateFavoriteDto dto)
        {
            var favorite = _mapper.Map<Favorite>(dto);
            favorite.CreatedAt = DateTime.Now;
            await _favoriteCollection.InsertOneAsync(favorite);
        }

        public async Task<List<ResultFavoriteDto>> GetByUserIdAsync(string userId)
        {
            var favorites = await _favoriteCollection.Find(x => x.UserId == userId).ToListAsync();
            
            var dtos = _mapper.Map<List<ResultFavoriteDto>>(favorites);

            foreach(var dto in dtos)
            {
                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                var destination = await _destinationCollection.Find(d => d.Id == tour.DestinationId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                    dto.CoverImage = tour.CoverImage;
                    dto.Price = tour.Price;
                    dto.Duration = tour.Duration;
                    dto.Country = destination.Country;

                    var comments = await _commentCollection.Find(c => c.TourId == dto.TourId).ToListAsync();
                    dto.AverageRating = comments.Count > 0 ? Math.Round(comments.Average(c => c.Rating), 1) : 0;
                }
            }

            return dtos;
        }

        public async Task<bool> IsFavoriteAsync(string userId, string tourId)
        {
            return await _favoriteCollection.Find(t => t.Id == tourId && t.UserId == userId).AnyAsync();
        }

        public async Task RemoveAsync(string userId, string tourId)
        {
            await _favoriteCollection.DeleteOneAsync(f => f.UserId == userId && f.TourId == tourId);
        }
    }
}
