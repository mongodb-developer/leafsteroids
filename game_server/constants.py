import os

from dotenv import load_dotenv

load_dotenv()

HTTPS_ENDPOINT_URL = os.environ["HTTPS_ENDPOINT_URL"]
ENDPOINT_GET_PLAYERS = f"{HTTPS_ENDPOINT_URL}/players"
ENDPOINT_GET_EVENTS = f"{HTTPS_ENDPOINT_URL}/events"
ENDPOINT_GET_CONFIG = f"{HTTPS_ENDPOINT_URL}/config"
ENDPOINT_POST_SNAPSHOT = f"{HTTPS_ENDPOINT_URL}/snapshot"

CONNECTION_STRING = os.environ["CONNECTION_STRING"]
DATABASE_NAME_LEAFSTEROIDS = "Leafsteroids"
COLLECTION_NAME_CONFIG = "config"
COLLECTION_NAME_EVENTS = "events"
COLLECTION_NAME_PLAYERS = "players"
COLLECTION_NAME_RECORDINGS = "recordings"
