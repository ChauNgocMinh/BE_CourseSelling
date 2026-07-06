using AutoMapper;
using BE_CourseSelling.Core.Entities;
using BE_CourseSelling.Core.DTOs;

namespace BE_CourseSelling.Service.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<CourseDTO, CourseResponseDto>();
        }
    }
}
