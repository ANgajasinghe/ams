namespace ams.Interfaces;

public interface ICsvService
{
    List<T> ReadCsvFileAsync<T>(string file);
}