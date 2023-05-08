using Digest.Custom.Server.Application;

namespace Digest.Custom.Server.Infrastructure;

public class HeaderService
{
    public Dictionary<string, string> ParseDigestHeaderValue(string digestHeaderValue)
    {
        var values = digestHeaderValue.Substring(Consts.Scheme.Length).Split(",");

        var valuesDict = new Dictionary<string, string>();

        foreach (var val in values)
        {
            var keyValuePair = val.Split("=", 2);

            valuesDict.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim('\"'));
        }

        return valuesDict;
    }

    public string BuildDigestHeaderValue(List<DigestValueSubItem> items, string scheme = Consts.Scheme)
    {
        scheme = string.IsNullOrEmpty(scheme) ? string.Empty : $"{scheme} ";
        return $"{scheme}{string.Join(", ", items.Select(FormatHeaderComponent))}";
    }

    private string FormatHeaderComponent(DigestValueSubItem component)
    {
        if (component.Quote)
            return $"{component.Key}=\"{component.Value}\"";

        return $"{component.Key}={component.Value}";
    }
}
