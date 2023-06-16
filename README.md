# Leafsteroids

This repository contains the MongoDB `Leafsteroids` demo. Follow the instructions in this README to deploy a version yourself.

## The Requirements

To reproduce and publish this project, there are a few software requirements that must be met:

- A [MongoDB Atlas](https://www.mongodb.com/atlas/database) account with an M0 tier cluster or better.
- Unity 3D, version 2021.3.19f1
- The [Realm CLI](https://www.mongodb.com/docs/atlas/app-services/cli/)
- The .NET Core SDK

The project is split into three core parts which include the data and backend layer hosted on MongoDB Atlas, the game built with Unity, and the registration form built with Blazor.

## General Game Configuration

While not one of the core three parts, the game operates on a configuration file that exists in MongoDB Atlas.

Create a **game** database and a **config** collection. The collection should contain a single document with the following data:

```json
{
    "RoundDuration": {
        "$numberDouble": "60.0"
    },
    "BulletDamage": {
        "$numberDouble": "15.0"
    },
    "BulletSpeed": {
        "$numberDouble": "500.0"
    },
    "PlayerMoveSpeed": {
        "$numberDouble": "5.0"
    },
    "PlayerRotateSpeed": {
        "$numberDouble": "100.0"
    },
    "BulletLifespan": {
        "$numberDouble": "1.0"
    },
    "PelletHeatlhSmall": {
        "$numberDouble": "50.0"
    },
    "PelletHeatlhLarge": {
        "$numberDouble": "150.0"
    },
    "PelletHeatlhMedium": {
        "$numberDouble": "75.0"
    }
}
```

Failure to import the above document will result in the Unity game throwing errors.

## MongoDB Atlas App Services Configuration & Deployment

The MongoDB Atlas App Services project is included within the repository as **atlas_app_services** and it can be managed with the Realm CLI.

Setup and deployment of the Atlas App Services project can be done through the following steps:

1. Connect the Realm CLI to your Atlas Project. You can learn how to do this within the [MongoDB documentation](https://www.mongodb.com/docs/atlas/app-services/cli/).
2. Update the **atlas_app_services/data_sources/mongodb_atlas/config.json** file to include the correct Atlas cluster name for your project.
2. Execute `realm-cli push --remote "<YOUR_APP_ID>" --include-package-json`

When prompted, follow the instructions to complete the deployment of your application.

When completed, rules, functions, endpoints, and anything else related to this project will be deployed within your account. Make note of any URLs to be copied into your Unity project.

Within the Atlas App Services dashboard, obtain your API key from the **Authentication** tab.

## Unity Game Configuration

The Unity game has a strong dependency on the HTTPS Endpoints that are a part of MongoDB Atlas App Services. When you have those endpoints, add them to the **unity_game/Assets/Scripts/00_Shared/Constants.cs** file.

The file will look something like this:

```csharp
namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshot";
        public const string DataApiUrlGetMany = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshots";
        public const string DataApiUrlGetOne = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshot?id=";
        public const string GetPlayersEndpoint = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/players";
        public const string GetConfigEndpoint = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/config";

        public const string DataApiKey = "<YOUR_APP_SERVICES_API_KEY>";

        public const float RecordingSpeed = 0.1f;
    }
}
```

The Unity game will now be able to communicate with MongoDB Atlas through the HTTPS Endpoints and Functions that were previously deployed.

## Blazor Configuration & Deployment

To register new players within the game, the user must visit a website on their computer or mobile device. This website was built with Blazor and communicates to MongoDB Atlas with the C# driver. This website can be ran locally or deployed to a cloud service.

Get started by adjusting the `ConnectionString` in the **blazor_registration/gdc_2023_demo_registration/appsettings.json** file.

```csharp
"ConnectionStrings": {
    "MongoDB": "<MONGODB_ATLAS_CONNECTION_URI>"
  }
```

To run the project locally, execute the following from the command line:

```bash
dotnet run
```

The application can be accessed at http://localhost:5002

## Contributors

- [Dominic Frei](https://linktr.ee/dominicfrei)
- [Hubert Nguyen](https://)
- [Nic Raboy](https://www.nraboy.com)
- [Sig Narváez](https://www.linkedin.com/in/signarvaez/)
