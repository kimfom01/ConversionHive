using System.Security.Claims;

namespace ConversionHive.Services;

public interface IJwtProcessor
{
    public Claim ExtractClaimFromJwt(string authorization, string claimType);
}
