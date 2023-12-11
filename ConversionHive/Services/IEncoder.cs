namespace ConversionHive.Services;

public interface IEncoder
{
    string EncodeValue(string value, string key);
}