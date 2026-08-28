using AutoMapper;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class QuestionMappings : Profile
    {
        public QuestionMappings()
        {
            CreateMap<CreateQuestionDto, Question>();
            CreateMap<Question, ResultQuestionDto>();
        }
    }
}
