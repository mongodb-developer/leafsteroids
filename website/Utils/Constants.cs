namespace website.Utils;

public static class Constants
{
    public const string DefaultEventId = "mdb-global-event";

    public const string RestServiceEndpointEvents = "events";
    public const string RestServiceEndpointPlayers = "players";
    public const string RestServiceEndpointPlayersAutoComplete = "players/autocomplete";
    public const string RestServiceEndpointPlayersSearch = "players/search";
    public const string RestServiceEndpointPlayersEvents = "players/events";
    public const string RestServiceEndpointRecordings = "recordings";
    public const string RestServiceEndpointRecordingsSimilar = "recordings/similar";
    public const string RestServiceEndpointRecordingsSimilarBySpeed = "recordings/similarBySpeed";
    public const string RestServiceEndpointRecordingsSimilarByAccel = "recordings/similarByAcceleration";
    public const string RestServiceEndpointRecordingsSimilarByStats = "recordings/similarByStats";
    public const string RestServiceEndpointRecordingsSimilarByScoreProgress = "recordings/similarByScoreProgress";
    public const string RestServiceEndpointRecordingsSimilarByGameStyle = "recordings/similarByGameStyle";
    public const string RestServiceEndpointRecordingsSimilarByAverageScorePerBullet = "recordings/similarByAverageScorePerBullet";
    public const string RestServiceEndpointRecordingsSimilarByAverageDamagePerBullet = "recordings/similarByAverageDamagePerBullet";
    public const string RestServiceEndpointRecordingsSimilarByAveragePelletsPerBullet = "recordings/similarByAveragePelletsPerBullet";
    public const string QueryParameterEventId = "EventId";
    public const string QueryParameterName = "Name";
    public const string QueryParameterRecordingId = "recordingId";
}