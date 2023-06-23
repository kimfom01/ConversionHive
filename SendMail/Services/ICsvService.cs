namespace SendMail.Services;

public interface ICsvService
{
    public IEnumerable<T>? ProcessCsv<T>(Stream fileStream);
}