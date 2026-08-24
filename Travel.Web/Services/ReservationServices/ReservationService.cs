using AutoMapper;
using MongoDB.Driver;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMapper _mapper;

        public ReservationService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateReservationDto createReservationDto)
        {
            var reservation = _mapper.Map<Reservation>(createReservationDto);
            

        }

        public Task<List<ResultReservationDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultReservationDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultReservationDto>> GetByTourIdAsync(string tourId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultReservationDto>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(string id, ReservationStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
