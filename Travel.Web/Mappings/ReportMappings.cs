using AutoMapper;
using Travel.Web.DTOs.ReportDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class ReportMappings : Profile
    {
        public ReportMappings()
        {
            CreateMap<Reservation, TourParticipantReportDto>();
        }
    }
}
