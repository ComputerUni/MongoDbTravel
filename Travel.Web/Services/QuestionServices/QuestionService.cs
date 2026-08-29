using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.DTOs.ReservationDtos;
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

        public async Task AnswerAsync(string questionId, string answer)
        {
            var updatedDefinition = Builders<Question>.Update.Set(q => q.Answer, answer).Set(q => q.IsAnswered, true);
            await _questionCollection.UpdateOneAsync(x => x.Id == questionId, updatedDefinition);
        }

        public async Task CreateAsync(CreateQuestionDto dto)
        {
            var question = _mapper.Map<Question>(dto);
            question.CreatedAt = DateTime.Now;
            await _questionCollection.InsertOneAsync(question);
        }


        public async Task<ResultQuestionDto> GetByIdAsync(string id)
        {
            var question = await _questionCollection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            var dto = _mapper.Map<ResultQuestionDto>(question);

            var tour = await _tourCollection.AsQueryable().FirstOrDefaultAsync(t => t.Id == dto.TourId);
            var user = await _userManager.FindByIdAsync(dto.UserId);


            dto.UserFullName = user.FirstName + " " + user.LastName;
            dto.UserInitials = $"{user.FirstName[0]}{user.LastName[0]}".ToUpper();
            dto.Email = user.Email;
            dto.TourName = tour.Name;


            return dto;

        }




        public async Task<List<ResultQuestionDto>> GetAllAsync()
        {
            var questions = await _questionCollection.AsQueryable().ToListAsync();
            var dtos = _mapper.Map<List<ResultQuestionDto>>(questions);

            var tours = await _tourCollection.AsQueryable().ToListAsync();

            foreach (var dto in dtos)
            {
                var tour = tours.FirstOrDefault(t => t.Id == dto.TourId);
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                }

                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user != null)
                {
                    dto.UserFullName = user.FirstName + " " + user.LastName;
                    dto.UserInitials = $"{user.FirstName[0]}{user.LastName[0]}".ToUpper();
                }
            }

            return dtos;
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

        public async Task<List<ResultQuestionDto>> GetUnansweredAsync()
        {
            var questions = await _questionCollection.Find(q => !q.IsAnswered).ToListAsync();
            var dtos = _mapper.Map<List<ResultQuestionDto>>(questions);

            var tours = await _tourCollection.AsQueryable().ToListAsync();

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
                    dto.UserInitials = $"{user.FirstName[0]}{user.LastName[0]}".ToUpper();
                }

            }
            return dtos;
        }
    }
}
