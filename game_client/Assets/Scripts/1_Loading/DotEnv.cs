using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class DotEnv
{
    private const string EnvVarFileName = ".env";

    public static IDictionary<string, string> Read()
    {
        string filePath = Path.Combine(Application.dataPath, EnvVarFileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file '{EnvVarFileName}' does not exist.");
        }

        string[] rawEnvRows = File.ReadAllLines(filePath, Encoding.UTF8);
        KeyValuePair<string, string>[] envRows = rawEnvRows.Length == 0 ? new KeyValuePair<string, string>[0] : Parse(rawEnvRows);

        var response = new Dictionary<string, string>();
        foreach (var envRow in envRows)
        {
            response[envRow.Key] = envRow.Value;
        }

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
