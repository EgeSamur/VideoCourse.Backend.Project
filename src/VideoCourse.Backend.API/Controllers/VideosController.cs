using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Requests;

namespace VideoCourse.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly IVideoService _service;
    private readonly IS3Service _s3Service;
    public VideosController(IVideoService service, IS3Service s3Service)
    {
        _service = service;
        _s3Service = s3Service;
    }
    [HttpPost("create-video")]
    public async Task<IActionResult> CreateAsync([FromBody] VideoCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
    [HttpPut("update-video")]
    public async Task<IActionResult> UpdateAsync([FromBody] VideoUpdateDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
    [HttpGet("get-videos")]
    public async Task<IActionResult> GetVideosAsync([FromQuery] PageRequest pageRequest)
    {
        var result = await _service.GetVideosAsync(pageRequest);
        return Ok(result);
    }
    [HttpGet("get-video-by-id")]
    public async Task<IActionResult> GetVideoByIdAsync([FromQuery] int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("upload-vided")]
    public async Task<IActionResult> UploadVideo([FromForm] List<IFormFile> files)
    {
        var x = new UploadVideosDto() { Files = files };
        var result = await _s3Service.UploadUserVideo(x);
        return Ok(result);
    }
}
