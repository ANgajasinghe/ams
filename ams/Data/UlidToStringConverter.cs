using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ams;

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 26);

    public UlidToStringConverter()
        : base(convertToProviderExpression: x => x.ToString(),
            convertFromProviderExpression: x => Ulid.Parse(x))
    {
    }
}