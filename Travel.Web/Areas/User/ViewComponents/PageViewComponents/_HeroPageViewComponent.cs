using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Travel.Web.Areas.User.Models;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _HeroPageViewComponent : ViewComponent
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Destination> _destinationCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMapper _mapper;

        public _HeroPageViewComponent(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinationCount = (int)await _destinationCollection.EstimatedDocumentCountAsync();
            var tourCount = (int)await _tourCollection.EstimatedDocumentCountAsync();

            var reservationList = await _reservationCollection.Find(_ => true).ToListAsync();
            var totalTraveler = reservationList.Sum(x => x.AdultCount + x.ChildCount);

            var destinations = await _destinationCollection.Find(_ => true).ToListAsync();
            var destinationDtos = _mapper.Map<List<ResultDestinationDto>>(destinations);

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";
            
            if(langCode == "en")
            {
                foreach(var dest in destinationDtos)
                {
                    dest.Country = dest.CountryEn ?? dest.Country;
                }
            }

            var model = new HeroViewModel
            {
                DestinationCount = destinationCount,
                TourCount = tourCount,
                TravelerCount = totalTraveler,
                Destination = destinationDtos
            };

            return View(model);
        }
    }
}
