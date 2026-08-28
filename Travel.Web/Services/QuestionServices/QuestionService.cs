using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.QuestionServices
{
    public class QuestionService : IQuestionService
    {

        private readonly IMongoCollection<Question> _questionCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public QuestionService(IDatabaseSettings databaseSettings, UserManager<AppUser> userManager, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _questionCollection = database.GetCollection<Question>(databaseSettings.QuestionCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
            _userManager = userManager;

        }

        public Task AnswerAsync(string questionId, string answer)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(CreateQuestionDto dto)
        {
            var question = _mapper.Map<Question>(dto);
            question.CreatedAt = DateTime.Now;
            await _questionCollection.InsertOneAsync(question);
        }

        public async Task<List<ResultQuestionDto>> GetByTourIdAsync(string tourId)
        {
            var questions = await _questionCollection.Find(x => x.TourId == tourId).ToListAsync();
            var dtos = _mapper.Map<List<ResultQuestionDto>>(questions);

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

        public async Task<List<ResultQuestionDto>> GetByUserIdAsync(string userId)
        {
            var questions = await _questionCollection.Find(x => x.UserId == userId).ToListAsync();
            var dtos = _mapper.Map<List<ResultQuestionDto>>(questions);

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
                }
            }
            return dtos;
        }

        public Task<List<ResultQuestionDto>> GetUnansweredAsync()
        {
            throw new NotImplementedException();
        }
    }
}
