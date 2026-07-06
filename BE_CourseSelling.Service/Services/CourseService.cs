using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BE_CourseSelling.Core.DTOs;
using BE_CourseSelling.Core.Entities;
using BE_CourseSelling.Core.Interfaces.Repositories;
using BE_CourseSelling.Core.Interfaces.Services;

namespace BE_CourseSelling.Service.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseResponseDto>>(entities);
        }

        public async Task<CourseResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<CourseResponseDto>(entity);
        }
    }
}
