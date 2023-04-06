# GDC 2023 Demo

## The Requirements

To reproduce and publish this project, there are a few software requirements that must be met:

- A [MongoDB Atlas](https://) account with an M0 tier cluster or better.
- The latest Unity LTS release
- The [Realm CLI](https://)

The project is split into three core parts which include the data and backend layer hosted on MongoDB Atlas, the game built with Unity, and the registration form built with Blazor.

## MongoDB Atlas App Services Configuration

The MongoDB Atlas App Services project is included within the repository as **atlas_app_services** and it can be managed with the Realm CLI.

Setup and deployment of the Atlas App Services project can be done through the following steps:

1. Connect the Realm CLI to your Atlas Project. You can learn how to do this within the [MongoDB documentation](https://www.mongodb.com/docs/atlas/app-services/cli/).
2. Execute `realm-cli push --remote "<YOUR_APP_ID>"`

When prompted, follow the instructions to complete the deployment of your application.

When completed, rules, functions, endpoints, and anything else related to this project will be deployed within your account. Make note of any URLs to be copied into your Unity project.

## Unity Game Configuration

The Unity game has a strong dependency on the HTTPS Endpoints that are a part of MongoDB Atlas App Services. When you have those endpoints, add them to the **unity_game/Assets/Scripts/00_Shared/Constants.cs** file.

The file will look something like this:

```csharp
namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshot";
        public const string DataApiUrlGetMany = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshots";
        public const string DataApiUrlGetOne = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshot?id=";
        public const string GetPlayersEndpoint = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/players";
        public const string GetConfigEndpoint = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/config";

        public const string DataApiKey = "<YOUR_API_KEY>";

        public const float RecordingSpeed = 0.1f;
    }
}
```

The Unity game will now be able to communicate with MongoDB Atlas through the HTTPS Endpoints and Functions that were previously deployed.

## Demo Registration

The Demo Registration page uses the MongoDB Drivers for which you need to adjust the `ConnectionString` in the `appsettings.json` file.

```csharp
"ConnectionStrings": {
    "MongoDB": "<YOUR_CONNECTION_STRING>"
  }
```

## Contributors

- [Dominic Frei](https://linktr.ee/dominicfrei)
- [Hubert Nguyen](https://)
- [Nic Raboy](https://www.nraboy.com)
