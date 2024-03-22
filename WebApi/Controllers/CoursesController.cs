using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CoursesRepository _courseRepository;
        private readonly DataContext _dataContext;

        public CoursesController(CoursesRepository courseRepository, DataContext dataContext)
        {
            _courseRepository = courseRepository;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _dataContext.Courses.ToListAsync();

            if(result != null)
            {
                return Ok(result);
            }
            if (result == null)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await _courseRepository.GetOne(id);


            if (result.StatusCode == Infrastructure.Models.StatusCodes.OK)
            {
                return Ok(result);
            }
            if (result.StatusCode == Infrastructure.Models.StatusCodes.NOT_FOUND)
            {
                return NoContent();
            }
            if (result.StatusCode == Infrastructure.Models.StatusCodes.ERROR)
            {
                return BadRequest();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne (CourseModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _courseRepository.CreateOne(model);

                if (result.StatusCode == Infrastructure.Models.StatusCodes.OK)
                {
                    return Created("", result);
                }
                if (result.StatusCode == Infrastructure.Models.StatusCodes.NOT_FOUND)
                {
                    return NoContent();
                }
                if (result.StatusCode == Infrastructure.Models.StatusCodes.ERROR)
                {
                    return BadRequest();
                }
                return BadRequest();
            }

            return BadRequest();
        }
    }
}
