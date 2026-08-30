using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.DashboardDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Question> _questionCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public DashboardService(IDatabaseSettings databaseSettings, UserManager<AppUser> userManager, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _questionCollection = database.GetCollection<Question>(databaseSettings.QuestionCollectionName);
            _commentCollection = database.GetCollection<Comment>(databaseSettings.CommentCollectionName);
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<DashboardKpiDto> GetKpiAsync()
        {
            var dto = new DashboardKpiDto();

            var tourPipeline = new[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    {"_id", "$IsActive" },
                    {"count", new BsonDocument("$sum", 1) }
                })
            };

            var tourResults = await _tourCollection.Aggregate<BsonDocument>(tourPipeline).ToListAsync();
            dto.TotalTours = tourResults.Sum(r => r["count"].AsInt32);
            dto.ActiveTours = tourResults.FirstOrDefault(r => r["_id"].AsString == "Aktif")?["count"].AsInt32 ?? 0;
            dto.PassiveTours = tourResults.FirstOrDefault(r => r["_id"].AsString == "Pasif")?["count"].AsInt32 ?? 0;

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var resPipeline = new[]
            {
                new BsonDocument("$group", new BsonDocument
                {

                        {"_id", new BsonDocument("$gte", new BsonArray {"$CreatedAt", startOfMonth}) },
                        {"count", new BsonDocument("$sum", 1) }

                })
            };

            var resResults = await _reservationCollection.Aggregate<BsonDocument>(resPipeline).ToListAsync();
            dto.TotalReservations = resResults.Sum(r => r["count"].AsInt32);
            dto.ThisMonthReservations = resResults.FirstOrDefault(r => r["_id"].AsBoolean == true)?["count"].AsInt32 ?? 0;

            dto.TotalUsers = _userManager.Users.Count();

            var questionPipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("IsAnswered", false)),
                new BsonDocument("$count", "count")
            };


            var questionResult = await _questionCollection.Aggregate<BsonDocument>(questionPipeline).ToListAsync();
            dto.PendingQuestions = questionResult.FirstOrDefault()?["count"].AsInt32 ?? 0;

            return dto;

        }

        public async Task<List<MonthlyReservationDto>> GetMonthlyReservationsAsync(string range)
        {
            if (range == "6m")
            {
                var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
                var pipeline = new[]
                {
                new BsonDocument("$match", new BsonDocument("CreatedAt", new BsonDocument("$gte", sixMonthsAgo))),
                new BsonDocument("$group", new BsonDocument{
                    {
                        "_id", new BsonDocument
                        {
                            {"year", new BsonDocument("$year", "$CreatedAt") },
                            {"month", new BsonDocument("$month", "$CreatedAt") }
                        }
                    },
                    {"count", new BsonDocument("$sum", 1) }
                }),
                new BsonDocument("$sort", new BsonDocument
                {
                    {"_id.year", 1 },
                    {"_id.month", 1 }

                })
            };

                var results = await _reservationCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                return results.Select(r => new MonthlyReservationDto
                {
                    Label = new DateTime(r["_id"]["year"].AsInt32, r["_id"]["month"].AsInt32, 1).ToString("MMMM", new System.Globalization.CultureInfo("tr-TR")),
                    Count = r["count"].AsInt32
                }).ToList();
            }
            else
            {
                int days = range == "7" ? 7 : 30;
                var startDate = DateTime.UtcNow.AddDays(-days);

                var pipeline = new[]
                {
                    new BsonDocument("$match", new BsonDocument("CreatedAt", new BsonDocument("$gte", startDate))),
                    new BsonDocument("$group", new BsonDocument
                    {
                        {
                            "_id",
                            new BsonDocument
                            {
                                { "year", new BsonDocument("$year", "$CreatedAt") },
                                { "month", new BsonDocument("$month", "$CreatedAt") },
                                { "day", new BsonDocument("$dayOfMonth", "$CreatedAt") }
                            }
                        },
                        { "count", new BsonDocument("$sum", 1) }
                    }),
                    new BsonDocument("$sort", new BsonDocument
                    {
                        { "_id.year", 1 },
                        { "_id.month", 1 },
                        { "_id.day", 1 }
                    })

                };

                var results = await _reservationCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();
                return results.Select(r => new MonthlyReservationDto
                {
                    Label = new DateTime(
                        r["_id"]["year"].AsInt32,
                        r["_id"]["month"].AsInt32,
                        r["_id"]["day"].AsInt32).ToString("dd MMM", new System.Globalization.CultureInfo("tr-TR")),
                    Count = r["count"].AsInt32
                }).ToList();

            }
        }

        public async Task<List<ResultQuestionDto>> GetPendingQuestionsAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("IsAnswered", false)),
                new BsonDocument("$sort", new BsonDocument("CreatedAt", -1)),
                new BsonDocument("$limit", 5)
            };

            var questions = await _questionCollection.Aggregate<Question>(pipeline).ToListAsync();
            return _mapper.Map<List<ResultQuestionDto>>(questions);
        }

        public async Task<List<PopularTourDto>> GetPopularToursAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    {"_id", "$TourId" },
                    {"count", new BsonDocument("$sum", 1) }
                }),
                new BsonDocument("$sort", new BsonDocument("count", -1)),
                new BsonDocument("$limit", 5)
            };

            var results = await _reservationCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            var dtos = new List<PopularTourDto>();

            foreach (var r in results)
            {
                var tourId = r["_id"].AsString;
                var tour = await _tourCollection.Find(t => t.Id == tourId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dtos.Add(new PopularTourDto
                    {
                        TourId = tourId,
                        TourName = tour.Name,
                        ReservationCount = r["count"].AsInt32,
                        CoverImage = tour.CoverImage
                    });
                }
            }
            return dtos;

        }

        public async Task<List<ResultCommentDto>> GetRecentCommentsAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$sort", new BsonDocument("CreatedAt", -1)),
                new BsonDocument("$limit", 5)
            };

            var comments = await _commentCollection.Aggregate<Comment>(pipeline).ToListAsync();
            var dtos = _mapper.Map<List<ResultCommentDto>>(comments);

            foreach (var dto in dtos)
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user != null)
                {
                    dto.UserFullName = $"{user.FirstName} {user.LastName}";
                    dto.UserInitials = $"{user.FirstName[0]}{user.LastName[0]}".ToUpper();
                    dto.UserEmail = user.Email;
                }

                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                dto.TourName = tour.Name ?? "Silinmiş Tur";
            }

            return dtos;

        }

        public async Task<List<ResultReservationDto>> GetRecentReservationAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$sort", new BsonDocument("CreatedAt", -1)),
                new BsonDocument("$limit", 5)
            };

            var reservations = await _reservationCollection.Aggregate<Reservation>(pipeline).ToListAsync();

            var dtos = _mapper.Map<List<ResultReservationDto>>(reservations);

            foreach (var dto in dtos)
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                dto.UserFullName = $"{user.FirstName} {user.LastName}";
                dto.UserEmail = user.Email;

                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                    dto.CoverImage = tour.CoverImage;
                    dto.Duration = tour.Duration;
                }
                else
                {
                    dto.TourName = "Silinmiş Tur";
                }
            }

            return dtos;
        }
    }
}
