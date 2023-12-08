using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ConversionHive.Services.Implementations;

public class JwtProcessor : IJwtProcessor
{
    public Claim ExtractClaimFromJwt(string authorization, string claimType)
    {
        var jwt = authorization.Replace("Bearer ", string.Empty);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);
        
        return token.Claims.First(cl => cl.Type == claimType);
    }
}
