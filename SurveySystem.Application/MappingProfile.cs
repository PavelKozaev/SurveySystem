using AutoMapper;
using SurveySystem.Contracts;
using SurveySystem.Domain.Entities;

namespace SurveySystem.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Answer, AnswerDto>()
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Question, QuestionWithAnswersDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.AnswerOptions, opt => opt.MapFrom(src => src.Answers));
        }
    }
}
