using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace RestService.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly MongoClient _mongoClient;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
            DotEnv.Load();
            var envVars = DotEnv.Read();
            var connectionString = envVars[Constants.ConnectionString];
            _mongoClient = new MongoClient(connectionString);
        }
    }
}