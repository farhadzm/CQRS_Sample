using CQRS.Domain.Entities;

namespace CQRS.Service.Jwt
{
    public interface IJwtService
    {
        AccessTokenDto CreateAccessToken(Users user);
    }
}
