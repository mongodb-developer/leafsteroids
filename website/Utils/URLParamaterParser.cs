using Microsoft.AspNetCore.WebUtilities;

namespace website.Utils;

public static class UrlHelper
{
    public static Dictionary<string, string> GetParameters(string uriString)
    {
        var uri = new Uri(uriString);
        var parsedQuery = QueryHelpers.ParseQuery(uri.Query);
        var parameters = new Dictionary<string, string>();

        foreach (var key in parsedQuery.Keys)
        {
            parameters[key] = parsedQuery[key].FirstOrDefault() ?? string.Empty;
        }

        return parameters;
    }
}