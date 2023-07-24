# Leafsteroids

This repository contains the MongoDB `Leafsteroids` demo.  
Follow the instructions in this README to run a clone of your own to get your MongoDB development jump started.

## Architecture

The demo and repository consist of the following parts:

- Game Client (Unity3D, .NET, C#)
- Game Server (Python, flask)
- Website (Blazor Server Application, .NET)

## Running your own clone

### Requirements

- Install Python and PIP
- Install the .NET SDK 7

### Prepare the database (MongoDB Atlas)

- Create a new Atlas project
- Create a new cluster (M0)
- Create a new database `Leafsteroids`:
    - Create a collection `config` and add the `config.template` document to it.
    - Create a collection `events` and add the `event.template` document to it.

You can adjust the config to change how the game behaves and add more events to have several to choose from.

### Run the Game Server

- Grab the connection string for your Atlas cluster and exchange it in the `.env` file in the `game_server` folder.

```shell
python3 -m venv venv
. venv/bin/activate
pip install -r requirements.txt
gunicorn --log-level=debug -b 0.0.0.0:8000 application:app
```

### Run the Website

- Grab the connection string for your Atlas cluster and exchange it in the `.env` file in the `game_server` folder.

```shell
dotnet run
```

### Run the Game Client

- Open the Unity project.
- In the `Assets` folder, adjust the IP in the `.env` folder to your `game_server`.
- Run the game.

## Contributors

- [Dominic Frei](https://linktr.ee/dominicfrei)
- [Hubert Nguyen](https://)
- [Nic Raboy](https://www.nraboy.com)
- [Sig Narváez](https://www.linkedin.com/in/signarvaez/)
