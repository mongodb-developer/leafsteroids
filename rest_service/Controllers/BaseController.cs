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

            String connectionString;
            String databaseName;

            if(envVars.Count > 0) {
              connectionString = envVars[Constants.ConnectionStringKey];
              databaseName = envVars[Constants.DatabaseNameKey];
            } else {
              connectionString = System.Environment.GetEnvironmentVariable(Constants.ConnectionStringKey);
              databaseName = System.Environment.GetEnvironmentVariable(Constants.DatabaseNameKey);
            }

            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }
    }
}
