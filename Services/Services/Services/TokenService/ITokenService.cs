using Core.IdentityEntities;

namespace Services.Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
