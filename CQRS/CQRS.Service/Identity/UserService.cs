using AutoMapper;
using CQRS.Application.Common.Mappings;
using CQRS.Domain.Entities;
using CQRS.Service.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Service.Identity
{
    public class UserLoginDto
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
    public class CreateUserDto : IMapTo<Users>
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, Users>()
                .ForMember(a => a.FullName, map => map.Ignore());
        }


    }
    public interface IUserService
    {
        Task<bool> ValidateUser(UserLoginDto userLoginDto, CancellationToken cancellationToken);
        Task<AccessTokenDto> CreateAccessToken(UserLoginDto userLoginDto, CancellationToken cancellationToken);
        Task<(int userId, List<string> errors)> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<Users> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(UserManager<Users> userManager, IJwtService jwtService, IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public async Task<AccessTokenDto> CreateAccessToken(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Email);
            return _jwtService.CreateAccessToken(user);
        }

        public async Task<(int, List<string>)> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken)
        {
            Users user = new Users { Email = request.Email, UserName = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return (user.Id, new List<string>());
            }
            return (0, result.Errors.Select(a => a.Description).ToList());
        }

        public async Task<bool> ValidateUser(UserLoginDto userLoginDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Email);
            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
            }
            return false;
        }
    }
}
