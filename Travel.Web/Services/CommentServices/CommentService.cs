using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Settings;

namespace Travel.Web.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CommentService(IDatabaseSettings databaseSettings, UserManager<AppUser> userManager, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _commentCollection = database.GetCollection<Comment>(databaseSettings.CommentCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task CreateAsync(CreateCommentDto dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            comment.CreatedAt = DateTime.Now;
            await _commentCollection.InsertOneAsync(comment);
        }

        public async Task DeleteAsync(string userId, string commentId)
        {
            await _commentCollection.DeleteOneAsync(x => x.Id == commentId && x.UserId == userId);
        }

        public async Task<List<ResultCommentDto>> GetAllAsync()
        {
            var comments = await _commentCollection.AsQueryable().ToListAsync();
            var dtos = _mapper.Map<List<ResultCommentDto>>(comments);

            foreach (var dto in dtos)
            {
                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                }

                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user != null)
                {
                    dto.UserFullName = user.FirstName + " " + user.LastName;
                    dto.UserInitials = user.FirstName[0].ToString() + user.LastName[0].ToString();
                    dto.UserEmail = user.Email;
                }

            }

            return dtos;
        }

        public async Task<List<ResultCommentDto>> GetByTourIdAsync(string tourId)
        {
            var comments = await _commentCollection.Find(x => x.TourId == tourId && x.Status == CommentStatus.Yayında).ToListAsync();
            var dtos = _mapper.Map<List<ResultCommentDto>>(comments);

            var tour = await _tourCollection.Find(t => t.Id == tourId).FirstOrDefaultAsync();

            foreach (var dto in dtos)
            {
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                }

                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user != null)
                {
                    dto.UserFullName = user.FirstName + " " + user.LastName;
                    dto.UserInitials = user.FirstName[0].ToString() + user.LastName[0].ToString();
                }
            }

            return dtos;
        }

        public async Task<List<ResultCommentDto>> GetByUserIdAsync(string userId)
        {
            var comments = await _commentCollection.Find(x => x.UserId == userId).ToListAsync();

            var dtos = _mapper.Map<List<ResultCommentDto>>(comments);

            foreach (var dto in dtos)
            {
                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                var destination = await _destinationCollection.Find(d => d.Id == tour.DestinationId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                    dto.Country = destination.Country;
                    dto.DestinationName = destination.Name;
                }

                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user != null)
                {
                    dto.UserFullName = user.FirstName + " " + user.LastName;
                    dto.UserInitials = user.FirstName[0].ToString() + user.LastName[0].ToString();
                }
            }

            return dtos;
        }

        public async Task UpdateAsync(string commentId, string content, int rating)
        {
            var update = Builders<Comment>.Update.Set(x => x.Content, content).Set(x => x.Rating, rating);
            await _commentCollection.UpdateOneAsync(x => x.Id == commentId, update);
        }

        public async Task UpdateRatingAsync(string commentId, int rating)
        {
            var update = Builders<Comment>.Update.Set(x => x.Rating, rating);
            await _commentCollection.UpdateOneAsync(x => x.Id == commentId, update);
        }

        public async Task ApproveAsync(string commentId)
        {
            var approved = Builders<Comment>.Update.Set(x => x.Status, CommentStatus.Yayında);
            await _commentCollection.UpdateOneAsync(x => x.Id == commentId, approved);
        }

        public async Task MarkAsSpamAsync(string commentId)
        {
            var spam = Builders<Comment>.Update.Set(x => x.Status, CommentStatus.Spam);
            await _commentCollection.UpdateOneAsync(x => x.Id == commentId, spam);
        }

        public async Task<CommentKpiDto> GetCommentKpiAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$facet", new BsonDocument
                {
                    {
                        "totalComment", new BsonArray
                        {
                            new BsonDocument("$count", "count")
                        }
                    },

                    {
                        "statusCounts", new BsonArray
                        {
                            new BsonDocument("$group", new BsonDocument
                            {
                                { "_id", "$Status" },
                                {"count", new BsonDocument("$sum", 1) }
                            })
                        }
                    },
                    {
                        "averageRating", new BsonArray
                        {
                            new BsonDocument("$group", new BsonDocument
                            {
                                {"_id", BsonNull.Value },
                              { "avg", new BsonDocument("$avg", "$Rating") }
                            })
                        }
                    }
                })
            };

            var results = await _commentCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            var kpiDto = new CommentKpiDto();

            if(results != null)
            {
                var totalArr = results["totalComment"].AsBsonArray;
                if(totalArr.Count > 0)
                {
                    kpiDto.TotalCommentCount = totalArr[0]["count"].AsInt32;
                }

                var statusArr = results["statusCounts"].AsBsonArray;
                foreach(var doc in statusArr)
                {
                    var statusId = doc["_id"];
                    int statusCode = -1;

                    if(statusId != null && !statusId.IsBsonNull)
                    {
                        statusCode = statusId.ToInt32();
                    }

                    var count = doc["count"].AsInt32;

                    if(statusCode == 1)
                    {
                        kpiDto.PublishedCommentCount = count;
                    }
                    else if(statusCode == 0)
                    {
                        kpiDto.PendingCommentCount = count;
                    }
                }

                var ratingArr = results["averageRating"].AsBsonArray;
                if (ratingArr.Count > 0 && !ratingArr[0]["avg"].IsBsonNull)
                {
                    kpiDto.AverageRating = Math.Round(ratingArr[0]["avg"].ToDouble(), 1);
                }
            }

            return kpiDto;
        }


    }
}
