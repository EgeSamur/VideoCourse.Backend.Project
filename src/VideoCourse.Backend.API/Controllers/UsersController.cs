using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Requests;

namespace VideoCourse.Backend.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service)
    {
        _service = service;
    }
    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto dto)
    {
        var result = await _service.LoginAsync(dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsersAsync([FromQuery] PageRequest pageRequest)
    {
        var result = await _service.GetUsers(pageRequest);
        return Ok(result);
    }

    [HttpPost("get-user-by-id")]
    public async Task<IActionResult> GetUserByIdAsync([FromQuery] int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }
}
