# GDC 2023 Demo

<img width="2056" alt="Screenshot 2023-05-18 at 16 15 09" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/e32ade66-493e-422e-91d3-3744efb578f7">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 16" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/23b366c0-9586-46fd-8e4a-54fb825d4b4b">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 25" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/51244c98-10fd-415a-ae40-808e001945a6">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 32" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/681e2eee-ad6d-4064-b1c5-02c9606fe64c">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 38" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/6f7bf85c-7867-4935-acbe-f84ec10f1217">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 46" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/03d0f556-1b67-49e5-88e8-1293f0d1c6c0">
<img width="2056" alt="Screenshot 2023-05-18 at 16 15 54" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/b8c92675-f626-479d-916f-e68fd0655c41">
<img width="2056" alt="Screenshot 2023-05-18 at 16 16 04" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/b19613f2-f12e-4b26-9dca-67037c21e718">
<img width="2056" alt="Screenshot 2023-05-18 at 16 16 10" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/52e77fb4-ba4c-4df0-b2b4-b49d3cc46c1b">
<img width="2056" alt="Screenshot 2023-05-18 at 16 17 55" src="https://github.com/mongodb-developer/gdc-2023-demo/assets/1942012/4835da6e-c36c-4c76-98d0-2632dc8f0124">

## The Requirements

To reproduce and publish this project, there are a few software requirements that must be met:

- A [MongoDB Atlas](https://www.mongodb.com/atlas/database) account with an M0 tier cluster or better.
- The latest Unity LTS release
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
