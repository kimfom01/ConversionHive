namespace ConversionHive.Services;

public interface IDecoder
{
    string DecodeValue(string encodedValue, string key);
}