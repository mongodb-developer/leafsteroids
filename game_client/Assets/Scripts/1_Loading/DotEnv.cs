using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

public static class DotEnv
{
    private const string EnvVarFileName = ".env";

    [DllImport("__Internal")]
    private static extern string GetEventId();
    [DllImport("__Internal")]
    private static extern string GetServiceIP();
    [DllImport("__Internal")]
    private static extern string GetServicePort();

    public static IDictionary<string, string> Read()
    {
        var response = new Dictionary<string, string>();

        var eventId = GetEventId();
        var serviceIP = GetServiceIP();
        var servicePort = GetServicePort();

        response["REST_SERVICE_IP"] = serviceIP;
        response["REST_SERVICE_PORT"] = servicePort;
        response["EVENT_ID"] = eventId;

        return response;
    }

    private static KeyValuePair<string, string>[] Parse(string[] dotEnvRows)
    {
        var validEntries = new List<KeyValuePair<string, string>>();

        foreach (var dotEnvRow in dotEnvRows)
        {
            var row = dotEnvRow.TrimStart();

            if (string.IsNullOrEmpty(row) || row.IsComment() || !row.HasEqualSign(out int index))
                continue;

            var key = row.Substring(0, index).Trim();
            var value = row.Substring(index + 1).Trim();

            if (value.IsQuoted())
            {
                value = value.StripQuotes();
            }

            validEntries.Add(new KeyValuePair<string, string>(key, value));
        }

        return validEntries.ToArray();
    }

    private static bool IsComment(this string row) => row.StartsWith("#");

    private static bool HasEqualSign(this string row, out int index)
    {
        index = row.IndexOf('=');
        return index > 0;
    }

    private static bool IsQuoted(this string value) =>
        (value.StartsWith("'") && value.EndsWith("'")) ||
        (value.StartsWith("\"") && value.EndsWith("\""));

    private static string StripQuotes(this string value) => value.Trim('\'', '"');
}
