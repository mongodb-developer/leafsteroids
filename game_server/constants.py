import os

from dotenv import load_dotenv

load_dotenv()

CONNECTION_STRING = os.environ["CONNECTION_STRING"]
DATABASE_NAME_LEAFSTEROIDS = "Leafsteroids"
COLLECTION_NAME_CONFIG = "config"
COLLECTION_NAME_EVENTS = "events"
COLLECTION_NAME_PLAYERS = "players"
COLLECTION_NAME_PLAYERS_UNIQUE = "players_unique"
COLLECTION_NAME_RECORDINGS = "recordings"
VIEW_NAME_EVENTS4GAMECLIENT = "vwEvents4GameClient"
VIEW_NAME_PLAYERS4GAMECLIENT = "vwPlayers4GameClient"

