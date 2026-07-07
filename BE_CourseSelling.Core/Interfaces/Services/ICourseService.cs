using BE_CourseSelling.Core.DTOs;

namespace BE_CourseSelling.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto?> GetByIdAsync(Guid id);
    }
}
