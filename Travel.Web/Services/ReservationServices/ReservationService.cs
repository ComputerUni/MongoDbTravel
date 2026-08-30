using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Settings;

namespace Travel.Web.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ReservationService(IDatabaseSettings databaseSettings, UserManager<AppUser> userManager, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> CreateAsync(CreateReservationDto createReservationDto)
        {
            var tour = await _tourCollection.Find(x => x.Id == createReservationDto.TourId).FirstOrDefaultAsync();
            var tourDate = tour.Dates.FirstOrDefault(d => d.Id == createReservationDto.TourDateId);

            Console.WriteLine($"Gelen TourDateId: {createReservationDto.TourDateId}");
            Console.WriteLine($"Tour Dates: {string.Join(", ", tour.Dates.Select(d => d.Id))}");

            int totalPerson = createReservationDto.AdultCount + createReservationDto.ChildCount;

            if (tourDate == null || tourDate.Quota < totalPerson)
            {
                throw new Exception("Yeterli kontenjan yok.");
            }

            var reservation = _mapper.Map<Reservation>(createReservationDto);
            reservation.TotalPrice = (createReservationDto.AdultCount * tour.Price) + (createReservationDto.ChildCount * tour.ChildPrice);
            reservation.Status = ReservationStatus.Bekliyor;
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.TourDate = tourDate.StartDate;

            await _reservationCollection.InsertOneAsync(reservation);

            var filter = Builders<Tour>.Filter.And(Builders<Tour>.Filter.Eq(t => t.Id, createReservationDto.TourId),
                Builders<Tour>.Filter.ElemMatch(t => t.Dates, d => d.Id == createReservationDto.TourDateId));

            var update = Builders<Tour>.Update.Inc("Dates.$.Quota", -totalPerson);

            var updateResult = await _tourCollection.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount == 0)
            {
                throw new Exception("Rezervasyon oluşturuldu ancak kontenjan düşürülemedi, ID eşleşmesini kontrol edin.");
            }

            return reservation.Id;
        }

        public async Task<List<ResultReservationDto>> GetAllAsync()
        {
            var reservations = await _reservationCollection.AsQueryable().OrderByDescending(r => r.CreatedAt).ToListAsync();
            var tours = await _tourCollection.AsQueryable().ToListAsync();
            var destinations = await _destinationCollection.AsQueryable().ToListAsync();

            var dtos = _mapper.Map<List<ResultReservationDto>>(reservations);


            foreach (var dto in dtos)
            {
                var tour = tours.FirstOrDefault(t => t.Id == dto.TourId);
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                    dto.CoverImage = tour.CoverImage;
                    dto.Duration = tour.Duration;
                    dto.UserFullName = user.FirstName + " " + user.LastName;
                    dto.UserEmail = user.Email;

                    var destination = destinations.FirstOrDefault(d => d.Id == tour.DestinationId);
                    dto.DestinationName = destination?.Name;

                }
                else
                {
                    dto.TourName = "Silinmiş Tur";
                    dto.DestinationName = "-";
                }
            }

            return dtos;
        }

        public async Task<ResultReservationDto> GetByIdAsync(string id)
        {
            var reservation = await _reservationCollection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
            var dto = _mapper.Map<ResultReservationDto>(reservation);

            var tour = await _tourCollection.AsQueryable().FirstOrDefaultAsync(t => t.Id == dto.TourId);
            var user = await _userManager.FindByIdAsync(dto.UserId);
            var destinations = await _destinationCollection.AsQueryable().ToListAsync();

            dto.UserFullName = user.FirstName + " " + user.LastName;
            dto.UserEmail = user.Email;

            if (tour != null)
            {
                dto.TourName = tour.Name;
                dto.CoverImage = tour.CoverImage;
                dto.Duration = tour.Duration;

                var destination = destinations.FirstOrDefault(d => d.Id == tour.DestinationId);
                dto.DestinationName = destination.Name;
            }

            else
            {
                dto.TourName = "Silinmiş Tur";
                dto.DestinationName = "-";
            }

            return dto;

        }

        public async Task<List<ResultReservationDto>> GetByTourIdAsync(string tourId, string tourDateId = null, string status = null)
        {
            var filter = Builders<Reservation>.Filter.Eq(r => r.TourId, tourId);

            if (!string.IsNullOrEmpty(tourDateId))
            {
                filter &= Builders<Reservation>.Filter.Eq(r => r.TourDateId, tourDateId);
            }


            if (!string.IsNullOrEmpty(status) && Enum.TryParse<ReservationStatus>(status, out var statusEnum))
            {
                filter &= Builders<Reservation>.Filter.Eq(r => r.Status, statusEnum);
            }


            var reservations = await _reservationCollection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();


            Console.WriteLine($"tourId: {tourId}");
            Console.WriteLine($"tourDateId: {tourDateId}");
            Console.WriteLine($"status: {status}");
            Console.WriteLine($"rezervasyon sayısı: {reservations.Count}");


            var tour = await _tourCollection.Find(t => t.Id == tourId).FirstOrDefaultAsync();
            var dtos = _mapper.Map<List<ResultReservationDto>>(reservations);

            foreach (var dto in dtos)
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                dto.UserFullName = $"{user.FirstName} {user.LastName}";
                dto.UserEmail = user.Email;
                dto.UserPhone = user.PhoneNumber;

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

        public async Task<List<ResultReservationDto>> GetByUserIdAsync(string userId)
        {
            var reservations = await _reservationCollection.Find(r => r.UserId == userId).SortByDescending(r => r.CreatedAt).ToListAsync();
            var dtos = _mapper.Map<List<ResultReservationDto>>(reservations);

            foreach (var dto in dtos)
            {
                var tour = await _tourCollection.Find(t => t.Id == dto.TourId).FirstOrDefaultAsync();
                if (tour != null)
                {
                    dto.TourName = tour.Name;
                    dto.CoverImage = tour.CoverImage;
                    dto.Duration = tour.Duration;

                    bool isOverallActive = tour.IsActive == "Aktif";

                    bool isDateActive = true;
                    if (tour.Dates != null && tour.Dates.Any())
                    {
                        var matchingDate = tour.Dates.FirstOrDefault(d => d.StartDate.Date == dto.TourDate.Date);
                        if (matchingDate != null)
                        {
                            isDateActive = matchingDate.IsActive;
                        }
                    }

                    dto.TourIsActive = (isOverallActive && isDateActive) ? "Aktif" : "Pasif";
                }
            }

            return dtos;
        }

        public async Task UpdateStatusAsync(string id, ReservationStatus status)
        {
            var reservation = await _reservationCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
            var filter = Builders<Reservation>.Filter.Eq(r => r.Id, id);
            var update = Builders<Reservation>.Update.Set(x => x.Status, status);

            await _reservationCollection.UpdateOneAsync(filter, update);


            if (status == ReservationStatus.İptal)
            {
                int totalPerson = reservation.AdultCount + reservation.ChildCount;

                var tourFilter = Builders<Tour>.Filter.And(
                    Builders<Tour>.Filter.Eq(t => t.Id, reservation.TourId),
                    Builders<Tour>.Filter.ElemMatch(t => t.Dates, d => d.Id == reservation.TourDateId)
                );

                var tourUpdate = Builders<Tour>.Update.Inc("Dates.$.Quota", totalPerson);
                await _tourCollection.UpdateOneAsync(tourFilter, tourUpdate);
            }


        }
    }
}
