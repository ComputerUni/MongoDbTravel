using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.Areas.Admin.Models;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Settings;

namespace Travel.Web.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IDestinationService _destinationService;
        private readonly ILookupService _lookupService;

        public TourService(ICategoryService categoryService, IDestinationService destinationService, ILookupService lookupService, IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _categoryService = categoryService;
            _destinationService = destinationService;
            _lookupService = lookupService;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateTourDto createTourDto)
        {
            var tour = _mapper.Map<Tour>(createTourDto);
            if(createTourDto.CoverImage != null)
            {
                tour.CoverImage = await SaveFileAsync(createTourDto.CoverImage);
            }

            if(createTourDto.Gallery != null && createTourDto.Gallery.Any())
            {
                tour.Gallery = new List<string>();
                foreach(var file in createTourDto.Gallery)
                {
                    tour.Gallery.Add(await SaveFileAsync(file));
                }
            }

            await _tourCollection.InsertOneAsync(tour);
        }

        public async Task DeleteAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<ResultTourDto>> GetAllAsync()
        {
            var tours = await _tourCollection.AsQueryable().ToListAsync();
            var categories = await _categoryService.GetAllAsync();

            var dtos = _mapper.Map<List<ResultTourDto>>(tours);

            foreach(var dto in dtos)
            {
                dto.CategoryName = categories.FirstOrDefault(c => c.Id == dto.CategoryId)?.CategoryName ?? "-";
            }

            return dtos;
        }

        public async Task<ResultTourDto> GetByIdAsync(string id)
        {
            var tour = await _tourCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultTourDto>(tour);
        }

        public async Task UpdateAsync(UpdateTourDto updateTourDto)
        {
            var tour = _mapper.Map<Tour>(updateTourDto);

            tour.CoverImage = updateTourDto.CoverImage != null ? await SaveFileAsync(updateTourDto.CoverImage) : updateTourDto.ExistingCoverImage;

            if(updateTourDto.Gallery != null && updateTourDto.Gallery.Any())
            {
                tour.Gallery = new List<string>();
                foreach(var file in updateTourDto.Gallery)
                {
                    tour.Gallery.Add(await SaveFileAsync(file));
                }
            }
            else
            {
                tour.Gallery = updateTourDto.ExistingGallery;
            }

            await _tourCollection.FindOneAndReplaceAsync(x => x.Id == tour.Id, tour);
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var folderPath = Path.Combine("wwwroot", "uploads", "tours");
            Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/tours/" + fileName;
        }
    }
}
