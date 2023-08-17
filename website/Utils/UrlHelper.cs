using System.Text;
using System.Web;
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

    public static string BuildUrlWithQuery(string url, IDictionary<string, string> queryParams)
    {
        var builder = new StringBuilder(url);

        if (queryParams.Count > 0)
        {
            builder.Append("?");
            foreach (var kvp in queryParams)
            {
                string key = HttpUtility.UrlEncode(kvp.Key);
                string value = HttpUtility.UrlEncode(kvp.Value);
                builder.AppendFormat("{0}={1}&", key, value);
            }
            builder.Length--; // Remove the last '&' character
        }

        return builder.ToString();
    }
}