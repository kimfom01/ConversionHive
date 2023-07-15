namespace SendMail.Services;

public interface IJwtProcessor
{
    public int ExtractIdFromJwt(string authorization);
}
