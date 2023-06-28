import os

from dotenv import load_dotenv

load_dotenv()

APP_ID = os.environ["APP_ID"]
APP_SERVICES_URL = f"https://data.mongodb-api.com/app/{APP_ID}/endpoint"
ENDPOINT_GET_PLAYERS = f"{APP_SERVICES_URL}/players"
ENDPOINT_GET_EVENTS = f"{APP_SERVICES_URL}/events"
ENDPOINT_GET_CONFIG = f"{APP_SERVICES_URL}/config"
ENDPOINT_POST_SNAPSHOT = f"{APP_SERVICES_URL}/snapshot"
