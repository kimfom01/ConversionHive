using System.Globalization;
using CsvHelper;

namespace SendMail.Services;

public class CsvService : ICsvService
{
    public IEnumerable<T>? ProcessCsv<T>(Stream fileStream)
    {
        var reader = new StreamReader(fileStream);

        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records =  csv.GetRecords<T>();

        return records;
    }
}