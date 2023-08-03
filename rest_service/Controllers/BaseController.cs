using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace RestService.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> Logger;
        protected readonly IMongoDatabase? Database;

        public BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
            DotEnv.Load();
            var envVars = DotEnv.Read();
            var connectionString = envVars[Constants.ConnectionStringKey];
            var databaseName = envVars[Constants.DatabaseNameKey];
            var mongoClient = new MongoClient(connectionString);
            Database = mongoClient.GetDatabase(databaseName);
        }
    }
}