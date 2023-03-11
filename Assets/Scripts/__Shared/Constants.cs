namespace __Shared
{
    public static class Constants
    {
        public const string DataApiUrlInsertOne =
            "https://eu-west-1.aws.data.mongodb-api.com/app/gdc_2023-tzwoy/endpoint/snapshot";

        public const string DataApiUrlGetMany =
            "https://eu-west-1.aws.data.mongodb-api.com/app/gdc_2023-tzwoy/endpoint/snapshots";

        public const string DataApiUrlGetOne =
            "https://eu-west-1.aws.data.mongodb-api.com/app/gdc_2023-tzwoy/endpoint/snapshot?id=";

        public const string DataApiKey = "PDcJqpop7XG0TR8n6DuBi9LL2uXPj8Bk5prbiXIaqePDRZTNdgQyZmFrnnnoOtSr";

        public const string ConnectionString =
            "mongodb+srv://dbUser:dbUserPassword@cluster0.c8y2i2q.mongodb.net/?retryWrites=true&w=majority";


        public const float RecordingSpeed = 0.1f;
    }
}