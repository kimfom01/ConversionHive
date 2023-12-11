using System.Security.Claims;

namespace ConversionHive.Services;

public interface IJwtProcessor
{
    int GetIdFromJwt(string authorization);
}
