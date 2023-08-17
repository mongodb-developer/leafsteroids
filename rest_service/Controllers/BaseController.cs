using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace RestService.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> Logger;
        protected readonly IMongoDatabase? Database;
        protected readonly IMongoClient? Client;

        public BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
            DotEnv.Load();
            var envVars = DotEnv.Read();
            var connectionString = envVars[Constants.ConnectionStringKey];
            var databaseName = envVars[Constants.DatabaseNameKey];
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }
    }
}