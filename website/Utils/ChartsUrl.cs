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

    public static string CreatePlayerUrl(string chartsId, string name, string location, string eventId)
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

    public static string CreateHomeUrl(string chartsId, string eventId, string eventLocation)
    {
        return CreateBaseUrl(chartsId)
               + "&filter={%27Event._id%27:%27"
               + eventId
               + "%27,%27location%27:%27" +
               eventLocation + "%27}";
    }

    private static string CreateBaseUrl(string chartsId)
    {
        return Constants.AtlasChartEmbedDashboardUrl
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