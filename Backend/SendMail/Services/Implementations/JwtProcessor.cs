using System.IdentityModel.Tokens.Jwt;

namespace SendMail.Services.Implementations;

public class JwtProcessor : IJwtProcessor
{
    public int ExtractIdFromJwt(string authorization)
    {
        var jwt = authorization.Replace("Bearer ", string.Empty);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.ReadJwtToken(jwt);

        var id = int.Parse(token.Claims.First(cl => cl.Type == "Id").Value);

        return id;
    }
}
