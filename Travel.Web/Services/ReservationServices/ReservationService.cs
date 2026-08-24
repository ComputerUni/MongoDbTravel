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
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;

        public ReservationService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateReservationDto createReservationDto)
        {
            var tour = await _tourCollection.Find(x => x.Id == createReservationDto.TourId).FirstOrDefaultAsync();
            var tourDate = tour.Dates.FirstOrDefault(d => d.Id == createReservationDto.TourDateId);

            int totalPerson = createReservationDto.AdultCount + createReservationDto.ChildCount;

            if (tourDate == null || tourDate.Quota < totalPerson)
            {
                throw new Exception("Yeterli kontenjan yok.");
            }

            var reservation = _mapper.Map<Reservation>(createReservationDto);
            reservation.TotalPrice = (createReservationDto.AdultCount * tour.Price) + (createReservationDto.ChildCount * tour.ChildPrice);
            reservation.Status = ReservationStatus.Bekliyor;
            reservation.CreatedAt = DateTime.UtcNow;

            await _reservationCollection.InsertOneAsync(reservation);

            var filter = Builders<Tour>.Filter.And(Builders<Tour>.Filter.Eq(t => t.Id, createReservationDto.TourId),
                Builders<Tour>.Filter.ElemMatch(t => t.Dates, d => d.Id == createReservationDto.TourDateId));

            var update = Builders<Tour>.Update.Inc("Dates.$.Quota", -totalPerson);
            await _tourCollection.UpdateOneAsync(filter, update);
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
