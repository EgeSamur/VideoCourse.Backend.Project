using Amazon.S3;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Application.Common.DTOs;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Security.Hashing;
using VideoCourse.Backend.Shared.Security.JWT;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
using VideoCourse.Backend.Shared.Utils.Results.Concrete;

namespace VideoCourse.Backend.Application.Features.Users;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly AwsConfiguration _awsConfiguration;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenHelper tokenHelper, IOptions<AwsConfiguration> awsConfiguration)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.UserRepository;
        _mapper = mapper;
        _tokenHelper = tokenHelper;
        _awsConfiguration = awsConfiguration.Value;
    }
    public async Task<IDataResult<LoggedDto>> CreateAsync(CreateUserDto dto)
    {
        try
        {
            byte[] passwordHash, passwordSalt;
            // Helper'ı kullanarak şifreyi hashle
            HashingHelper.CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);
            // Yeni kullanıcı nesnesi oluştur
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Job = dto.Job,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            string[] roles = Array.Empty<string>();
            string[] permissions = Array.Empty<string>();
            AccessToken token = _tokenHelper.CreateToken(user.Id, roles, permissions);
            var response = new LoggedDto()
            {
                UserId = user.Id,
                AccessToken = token,
            };
            return new SuccessDataResult<LoggedDto>(response, MessageHelper.Created("User"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }

    }
    public async Task<IDataResult<LoggedDto>> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetAsync(predicate: i => i.Email == dto.Email,
            enableTracking: true);
        if (user == null)
            throw new NotFoundException(MessageHelper.NotFound("User"));
        // Şifreyi hash ile doğrula
        if (!HashingHelper.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new AuthorizationException(MessageHelper.Invalid("password"));
        string[] roles = Array.Empty<string>();
        string[] permissions = Array.Empty<string>();
        if (user.IsAdmin)
        {
            roles = new[] { "admin" };  // Admin rolünü içeren dizi
        }
        var token = _tokenHelper.CreateToken(user!.Id, roles, permissions!);
        var response = new LoggedDto()
        {
            UserId = user.Id,
            AccessToken = token,
        };
        return new SuccessDataResult<LoggedDto>(response, MessageHelper.Created("Token"));
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetAsync(predicate: i => i.Id == id);
            if (user == null)
                throw new NotFoundException(MessageHelper.NotFound("User"));
            await _userRepository.HardDeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessResult(MessageHelper.Deleted("User"));
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    public async Task<IDataResult<UserDto>> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.UserRepository.GetWithProjectionAsync(
            predicate: i => i.Id == id,
            selector: x => new UserDto()
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FullName,
                IsAdmin = x.IsAdmin,
                Job = x.Job,
                PhoneNumber = x.PhoneNumber,
                ProfileImageUrl = x.ProfileImageUrl
            });
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("User"));

        return new SuccessDataResult<UserDto>(data, MessageHelper.FetchedById("Job"));
    }

    public async Task<IDataResult<PaginatedResponse<UserDto>>> GetUsers(PageRequest pageRequest)
    {
        var users = await _unitOfWork.UserRepository.GetListWithProjectionAsync(
            selector: x => new UserDto()
            {
                Id = x.Id,
                Email = x.Email,
                FullName = x.FullName,
                IsAdmin = x.IsAdmin,
                Job = x.Job,
                PhoneNumber = x.PhoneNumber,
                ProfileImageUrl = x.ProfileImageUrl
            }
            ,size: pageRequest.Size,
            index: pageRequest.Index,
            isAll: pageRequest.IsAll);
        var result = _mapper.Map<PaginatedResponse<UserDto>>(users);
        return new SuccessDataResult<PaginatedResponse<UserDto>>(result, MessageHelper.Listed("Jobs"));
    }
}

