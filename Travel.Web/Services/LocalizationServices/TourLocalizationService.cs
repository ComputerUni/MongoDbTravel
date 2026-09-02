using MongoDB.Driver;
using Travel.Web.Entities;
using Travel.Web.Settings;

namespace Travel.Web.Services.LocalizationServices
{
    public class TourLocalizationService : ITourLocalizationService
    {
        private readonly IMongoCollection<TourLocalization> _tourLocalizationCollection;

        public TourLocalizationService(IDatabaseSettings databaseSettings) 
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourLocalizationCollection = database.GetCollection<TourLocalization>(databaseSettings.TourLocalizationCollectionName);
        }

        public async Task<TourLocalization> GetLocalizationByTourAndLangAsync(string tourId, string languageCode)
        {
            var localization = await _tourLocalizationCollection
                .Find(x => x.TourId == tourId && x.LanguageCode == languageCode)
                .FirstOrDefaultAsync();

            if(localization == null && languageCode != "tr")
            {
                localization = await _tourLocalizationCollection
                    .Find(x => x.TourId == tourId && x.LanguageCode == "tr")
                    .FirstOrDefaultAsync();
            }

            return localization;
        }

        public async Task<string> GetLocalizedTourNameAsync(string tourId, string languageCode)
        {
            var localization = await GetLocalizationByTourAndLangAsync(tourId, languageCode);
            return localization?.Name ?? "-";
        }

        public async Task SaveLocalizationAsync(TourLocalization localization)
        {
            var filter = Builders<TourLocalization>.Filter.Where(x => x.TourId == localization.TourId && x.LanguageCode == localization.LanguageCode);

            var update = Builders<TourLocalization>.Update
                .Set(x => x.Name, localization.Name)
                .Set(x => x.Description, localization.Description)
                .Set(x => x.ShortDescription, localization.ShortDescription)
                .Set(x => x.Route, localization.Route)
                .Set(x => x.TourType, localization.TourType)
                .Set(x => x.Transport, localization.Transport)
                .Set(x => x.Accommodation, localization.Accommodation)
                .Set(x => x.GuideLanguage, localization.GuideLanguage)
                .Set(x => x.VisaInfo, localization.VisaInfo)
                .Set(x => x.MeetingPoint, localization.MeetingPoint)
                .Set(x => x.Included, localization.Included)
                .Set(x => x.NotIncluded, localization.NotIncluded)
                .Set(x => x.Features, localization.Features)
                .Set(x => x.DayPrograms, localization.DayPrograms);

            await _tourLocalizationCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}
