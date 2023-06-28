# Leafsteroids

This repository contains the MongoDB `Leafsteroids` demo.  
Follow the instructions in this README to deploy a version yourself.

## Architecture

The demo consists of the following parts:

- Unity game
- Website for registering players and checking statistics (Blazor Server Application)
- game server (Python flask app)
- MongoDB Atlas (configuration files are part of the repo)

## Deployment

To deploy your own clone of this demo, go through the following steps, starting with a fork of this repository.

### Prepare the database (MongoDB Atlas)

- Create a new Atlas project
- Create a new cluster (M0)
- Create a new app in the project
- App Services -> Manage -> Deployment:
    - Install App Services as a GitHub app into your fork
    - Authorize the GitHub app
    - Choose the path: atlas_app_services/Leafsteroids
    - Enable `Automatic Deployment`
- In your repo: `atlas_app_services/Leafsteroids/realm_config.json` update your `AppID`.
- Wait for the deployment to finish.
- Create a new database `Leafsteroids`:
    - Create a collection `config` and add the `config.template` document to it.
    - Create a collection `events` and add the `event.template` document to it.

### Package the website

- cd blazor_registration
- cd gdc_2023_demo_registration
- dotnet publish -o output
- cd output
- zip -r ../deploy.zip .

### Deploy the website (AWS Beanstalk)

- Create a new Beanstalk app
- Create a new environment for the app:
    - Platform: `.NET Core on Linux`
    - Platform branch: `.NET Core running on 64bit Amazon Linux 2`
    - Platform version: `2.5.4 (Recommended)`
    - Upload a local file by choosing above created `deploy.zip`
    - The service access is up to your choice.
    - Make sure, on the last page, to add an environment variable `CONNECTION_STRING` using the MongoDB Atlas connection
      string.

### Deploy the game server (AWS EC2)

- Create a new AWS EC2 instance for the game server, choose `Ubuntu 22.04 LTS` as the OS and a keypair. The rest can be
  kept default.
- Select the security group for your new instance and add an inbound rule: `Custom TCP`, port `8000`, from `anywhere`.
- Create a `.env` file based on the `.env.template` file in the `game_server` folder.
- Update the `HTTPS_ENDPOINT_URL` and `API_KEY` as described in the `.env` file.
- Using the `game_server_setup.md` copy the files to the server and start the game server.

### Game Setup

- Download the game and create a new file `local_config.json` based on `local_config.json.template`.
- Place it in the same directory as the game (Windows) or the appropriate folders `Contents` (Mac OS) or `Data` (Linux).
- Have fun! :)

## Contributors

- [Dominic Frei](https://linktr.ee/dominicfrei)
- [Hubert Nguyen](https://)
- [Nic Raboy](https://www.nraboy.com)
- [Sig Narváez](https://www.linkedin.com/in/signarvaez/)
