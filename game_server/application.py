import logging
from datetime import datetime

from bson import ObjectId
from dotenv import load_dotenv
from flask import Flask, jsonify
from flask import request
from pymongo import MongoClient

from constants import CONNECTION_STRING, DATABASE_NAME_LEAFSTEROIDS, COLLECTION_NAME_EVENTS, COLLECTION_NAME_RECORDINGS, \
    COLLECTION_NAME_PLAYERS, COLLECTION_NAME_CONFIG

load_dotenv()

app = Flask(__name__)
gunicorn_logger = logging.getLogger('gunicorn.error')
app.logger.handlers = gunicorn_logger.handlers
app.logger.setLevel(gunicorn_logger.level)

mongo_client = MongoClient(CONNECTION_STRING)
leafsteroids_database = mongo_client.get_database(DATABASE_NAME_LEAFSTEROIDS)
config_collection = leafsteroids_database.get_collection(COLLECTION_NAME_CONFIG)
events_collection = leafsteroids_database.get_collection(COLLECTION_NAME_EVENTS)
players_collection = leafsteroids_database.get_collection(COLLECTION_NAME_PLAYERS)
recordings_collection = leafsteroids_database.get_collection(COLLECTION_NAME_RECORDINGS)


def convert_value_to_json(value):
    if isinstance(value, datetime):
        return value.isoformat()
    if isinstance(value, ObjectId):
        return str(value)
    return value


def convert_document_to_json(document):
    document = {k: convert_value_to_json(v) for k, v in document.items()}
    return document


@app.route('/', methods=['GET'])
def get_root():
    return "I'm alive!\n"


@app.route('/config')
def get_config():
    collection = leafsteroids_database[COLLECTION_NAME_CONFIG]
    documents = [convert_document_to_json(doc) for doc in collection.find()]
    return jsonify(documents)


@app.route('/events')
def get_events():
    collection = leafsteroids_database[COLLECTION_NAME_EVENTS]
    documents = [convert_document_to_json(doc) for doc in collection.find()]
    return jsonify(documents)


@app.route('/players')
def get_players():
    collection = leafsteroids_database[COLLECTION_NAME_PLAYERS]
    documents = [convert_document_to_json(doc) for doc in collection.find()]
    return jsonify(documents)


@app.route('/recording', methods=['POST'])
def post_recording():
    collection = leafsteroids_database[COLLECTION_NAME_RECORDINGS]
    try:
        result = collection.insert_one(request.get_json())
        app.logger.debug(f"Inserted new recording with id: {result.inserted_id}")
        if not result.acknowledged:
            return jsonify({"error": "Could not insert recording."}), 500
    except Exception as exception:
        return jsonify({'error': str(exception)}), 500
    return "", 201


if __name__ == '__main__':
    app.run()
