using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoCourse.Backend.Application.Features.CourseSections.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;

namespace VideoCourse.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseSectionsController : ControllerBase
{
    private readonly ICourseSectionService _courseSectionService;

    public CourseSectionsController(ICourseSectionService courseSectionService)
    {
        _courseSectionService = courseSectionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseSectionCreateDto dto)
    {
        var result = await _courseSectionService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CourseSectionUpdateDto dto)
    {
        var result = await _courseSectionService.UpdateAsync(dto);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _courseSectionService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var result = await _courseSectionService.GetVideoSectionsAsync(pageRequest);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _courseSectionService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("add-videos")]
    public async Task<IActionResult> AddVideos([FromBody] AddVideosCourseSectionDto dto)
    {
        var result = await _courseSectionService.AddVideosToCourseSection(dto);
        return Ok(result);
    }

    [HttpDelete("delete-videos")]
    public async Task<IActionResult> RemoveVideos([FromBody] DeleteVideosFromCourseSectionDto dto)
    {
        var result = await _courseSectionService.DeleteVideoFromCourseSectionAsync(dto);
        return Ok(result);
    }
}
