using dotenv.net;

namespace website.Utils;

public static class ChartsUrl
{
    public static string CreateEventUrl(string chartsId, string eventId, string eventLocation)
    {
        return CreateBaseUrl(chartsId)
            + "&filter={%27Event._id%27:%27"
            + eventId
            + "%27,%27location%27:%27"
            + eventLocation
            + "%27}";
    }

    public static string CreatePlayerUrl(
        string chartsId,
        string name,
        string location,
        string eventId
    )
    {
        return CreateBaseUrl(chartsId)
            + "&filter={%27Player.Nickname%27:%27"
            + name
            + "%27,%27location%27:%27"
            + location
            + "%27,%27Event._id%27:%27"
            + eventId
            + "%27}";
    }

    public static string CreateSimilarUrl(string chartsId, string recordingId)
    {
        return CreateBaseUrl(chartsId)
            + "&filter={%27_id%27:{%27$oid%27:%27"
            + recordingId
            + "%27}}";
    }

    public static string CreateHomeUrl(string chartsId, string eventId, string eventLocation)
    {
        return CreateBaseUrl(chartsId)
            + "&filter={%27Event._id%27:%27"
            + eventId
            + "%27,%27location%27:%27"
            + eventLocation
            + "%27}";
    }

    private static string CreateBaseUrl(string chartsId)
    {
        DotEnv.Load();
        var envVars = DotEnv.Read();
        if (envVars.Count == 0)
        {
            envVars["ATLAS_CHART_EMBED_DASHBOARD_URL"] = System.Environment.GetEnvironmentVariable(
                "ATLAS_CHART_EMBED_DASHBOARD_URL"
            );
        }
        var atlasChartEmbedDashboardUrl = envVars["ATLAS_CHART_EMBED_DASHBOARD_URL"];

        return atlasChartEmbedDashboardUrl
            + "?id="
            + chartsId
            + "&theme=light"
            + "&autoRefresh=true"
            + "&maxDataAge=60"
            + "&showTitleAndDesc=false"
            + "&scalingWidth=100%25"
            + "&scalingHeight=fixed"
            + "&attribution=false";
    }
}

