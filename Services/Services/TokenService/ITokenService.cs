using Core.IdentityEntities;

namespace Services.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser appUser);
    }
}
