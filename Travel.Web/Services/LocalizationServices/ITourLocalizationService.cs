using Travel.Web.Entities;

namespace Travel.Web.Services.LocalizationServices
{
    public interface ITourLocalizationService
    {
        Task SaveLocalizationAsync(TourLocalization localization);
        Task<TourLocalization> GetLocalizationByTourAndLangAsync(string tourId, string languageCode);
        Task<string> GetLocalizedTourNameAsync(string tourId, string languageCode);
    }
}
