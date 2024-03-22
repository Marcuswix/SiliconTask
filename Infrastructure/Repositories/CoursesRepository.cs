using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class CoursesRepository
    {
        private readonly DataContext _dataContext;

        public CoursesRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<RepositoriesResult> GetAll()
        {
            try
            {
                var result = _dataContext.Courses.ToList();

                if(result != null)
                {
                    return ResponseFactory.Ok(result);
                }
                if(result == null)
                {
                    return ResponseFactory.NotFound();
                }
                return ResponseFactory.Error();

            }
            catch (Exception ex) 
            { Debug.WriteLine("GetAllCourses" + ex.Message);
                return ResponseFactory.Error();
            }
        }

        public async Task<RepositoriesResult> GetOne(int id)
        {
            try
            {
                var result = _dataContext.Courses.FirstOrDefault(x => x.Id == id);

                if (result != null)
                {
                    return ResponseFactory.Ok(result);
                }
                if (result == null)
                {
                    return ResponseFactory.NotFound();
                }
                return ResponseFactory.Error();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetOneCourses" + ex.Message);
                return ResponseFactory.Error();
            }
        }

        public async Task<RepositoriesResult> CreateOne(CourseModel model)
        {
            try
            {
                var entity = new CourseEntity
                {
                    Title = model.Title,
                    Author = model.Author,
                    Description = model.Description,
                    DiscountPrice = model.DiscountPrice,
                    Hours = model.Hours,
                    IsBestseller = model.IsBestseller,
                    LikesInNumbers = model.LikesInNumbers,
                    LikesInProcent = model.LikesInProcent,
                    Price = model.Price,
                    WhatYouLearn = model.WhatYouLearn,
                };

                var result = await _dataContext.Courses.AddAsync(entity);
                await _dataContext.SaveChangesAsync();

                var course = new CourseModel
                {
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description,
                    DiscountPrice = entity.DiscountPrice,
                    Hours = entity.Hours,
                    IsBestseller = entity.IsBestseller,
                    LikesInNumbers = entity.LikesInNumbers,
                    LikesInProcent = entity.LikesInProcent,
                    Price = entity.Price,
                    WhatYouLearn = entity.WhatYouLearn,
                };

                if (result != null)
                {
                    return ResponseFactory.Ok(result);
                }
                if (result == null)
                {
                    return ResponseFactory.NotFound();
                }
                return ResponseFactory.Error();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateOneCourses" + ex.Message);
                return ResponseFactory.Error();
            }
        }
    }
}
