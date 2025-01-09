using Core.IdentityEntities;

namespace Application.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser appUser);
    }
}
