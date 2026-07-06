using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE_CourseSelling.Core.Entities;
using BE_CourseSelling.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using BE_CourseSelling.Infrastructure.Data;

namespace BE_CourseSelling.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _db;

        public CourseRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(CourseDTO entity)
        {
            await _db.Courses.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(CourseDTO entity)
        {
            _db.Courses.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseDTO>> GetAllAsync()
        {
            return await _db.Courses.AsNoTracking().ToListAsync();
        }

        public async Task<CourseDTO?> GetByIdAsync(Guid id)
        {
            return await _db.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(CourseDTO entity)
        {
            _db.Courses.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
