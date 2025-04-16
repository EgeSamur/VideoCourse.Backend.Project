using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoCourse.Backend.Application.Features.Courses.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;

namespace VideoCourse.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
    {
        var result = await _courseService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CourseUpdateDto dto)
    {
        var result = await _courseService.UpdateAsync(dto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _courseService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var result = await _courseService.GetCoursesAsync(pageRequest);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _courseService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("add-sections")]
    public async Task<IActionResult> AddSections([FromBody] AddSectionsToCoursDto dto)
    {
        var result = await _courseService.AddSectionsToCourse(dto);
        return Ok(result);
    }

    [HttpPost("delete-sections")]
    public async Task<IActionResult> DeleteSections([FromBody] DeleteSectionsFromCourseDto dto)
    {
        var result = await _courseService.DeleteSectionsFromCourse(dto);
        return Ok(result);
    }

    [HttpPut("swap-sections")]
    public async Task<IActionResult> SwapSections([FromBody] SwapCourseSectionDto dto)
    {
        var result = await _courseService.SwapCourseSection(dto);
        return Ok(result);
    }

}
