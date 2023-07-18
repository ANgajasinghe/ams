using System.Globalization;
using System.Text;
using ams.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace ams.Services;

public class CsvService : ICsvService
{
    public List<T> ReadCsvFileAsync<T>(string filePath)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
            Delimiter = "," // The delimiter is a comma.
        };

        var textReader = new StreamReader(filePath, Encoding.UTF8);

        using var csv = new CsvReader(textReader, configuration);
        var list = csv.GetRecords<T>().ToList();
        return list;
    }
}