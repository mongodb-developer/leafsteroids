namespace website.Utils;

public static class Constants
{
    public const string DefaultEventId = "mdb-global-event";

    // TO-DO: Chart URL and ID's should be obtained from either .env or REST service
    public const string AtlasChartEmbedDashboardUrl =
        "https://charts.mongodb.com/charts-global_shared-jebkk/embed/dashboards";

    // STAGING
    //public const string AtlasChartIdEvent = "64c95022-391d-4240-8d73-b033c8e34195"; 
    //public const string AtlasChartIdPlayer = "64c95045-413c-4e46-8c40-f033169d4011"; 
    //public const string AtlasChartIdHome = "64c95059-aef7-4a35-86e8-ea27edd5b50b";

    // PRODUCTION
    public const string AtlasChartIdEvent = "6453fb66-c2fc-4212-8dbd-4ab8365f1ac0";
    public const string AtlasChartIdPlayer = "64540ea3-f8b8-4211-8c3b-cff2506301a4";
    public const string AtlasChartIdHome = "645abbc7-2258-4908-8eb4-94bbe0c6d324";

    public const string RestServiceEndpointEvents = "events";
    public const string RestServiceEndpointPlayers = "players";
    public const string RestServiceEndpointPlayersAutoComplete = "players/autocomplete";
    public const string RestServiceEndpointPlayersSearch = "players/search";

    public const string QueryParameterEventId = "EventId";
    public const string QueryParameterName = "Name";
}