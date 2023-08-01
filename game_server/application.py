import logging
import json
import jsonschema
from datetime import datetime

from bson import ObjectId
from flask import Flask, jsonify
from flask import request
from pymongo import MongoClient

from constants import *


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
    collection = leafsteroids_database[VIEW_NAME_EVENTS4GAMECLIENT]
    documents = [convert_document_to_json(doc) for doc in collection.find()]
    return jsonify(documents)


@app.route('/players')
def get_players():
    collection = leafsteroids_database[VIEW_NAME_PLAYERS4GAMECLIENT]
    documents = [convert_document_to_json(doc) for doc in collection.find()]
    return jsonify(documents)


@app.route('/recording', methods=['POST'])
def post_recording():

    collection = leafsteroids_database[COLLECTION_NAME_RECORDINGS]
    recording = request.get_json()

    try:

        # Verify Event
        event_id = recording['Event']['_id']
        event = leafsteroids_database[COLLECTION_NAME_EVENTS]\
            .find_one({'_id': event_id})
        if not event:
            err = {"error": "Could not retrieve event: " + event_id}
            app.logger.debug(json.dumps(err))
            return jsonify(err), 500

        eventExtRef = {
            "_id": event["_id"]
        }

        location = event["location"]  # location for recording

        # Verify Player
        nickname = recording['Player']['Nickname']

        player_unique = leafsteroids_database[COLLECTION_NAME_PLAYERS]\
            .find_one({'Nickname': nickname})
        if not player_unique:
            err = {"error": "Could not retrieve player_unique: " + nickname}
            app.logger.debug(json.dumps(err))
            return jsonify(err), 500

        player = leafsteroids_database[COLLECTION_NAME_PLAYERS]\
            .find_one({'location': player_unique["location"], 'Nickname': nickname})
        if not player:
            err = {"error": "Could not retrieve player: " + nickname}
            app.logger.debug(json.dumps(err))
            return jsonify(err), 500

        playerExtRef = {
            "_id": player["_id"],
            "Nickname": player["Nickname"]
        }

        # Create Recording to persist
        recordingToPersist = {
            "location": location,
            "DateTime": datetime.now(),
            "Player": playerExtRef,     # Extended Reference Pattern
            "Event": eventExtRef,       # Extended Reference Pattern
            "SessionStatisticsPlain": recording["SessionStatisticsPlain"],
            "Snapshots": recording["Snapshots"],
        }

        # Validate against $jsonSchema
        validateRecordingSchema(recordingToPersist)

        # Persist
        result = collection.insert_one(recordingToPersist)
        app.logger.debug(f"Inserted new recording with id: {result.inserted_id}")

        if not result.acknowledged:
            return jsonify({"error": "Could not insert recording."}), 500

    except Exception as exception:
        app.logger.debug(json.dumps(exception))
        return jsonify({'error': str(exception)}), 500
    return "", 201


def validateRecordingSchema(recording):

    # TO-DO: Move json schema to a file
    recording_schema = {
        "$jsonSchema": {
            "bsonType": "object",
            "required": ["location", "DateTime", "Player", "Event", "SessionStatisticsPlain", "Snapshots"],
            "additionalProperties": False,
            "properties": {
                "location": {
                  "bsonType": "string"
                },
                "DateTime": {
                    "bsonType": "date"
                },
                "Player": {
                    "type": "object",
                    "properties": {
                        # TO-DO
                        # "_id": {
                        #   "type": "object",
                        #   "properties": {
                        #     "$oid": {
                        #       "type": "string",
                        #       "pattern": "^[0-9a-fA-F]{24}$"
                        #     }
                        #   },
                        #   "required": ["$oid"],
                        # },
                        "Nickname": {
                          "type": "string"
                        }
                    },
                    "required": ["_id", "Nickname"],    # TO-DO: add _id
                    "additionalProperties": True        # TO-DO: Change to false whe setting _id
                },
                "Event": {
                    "type": "object",
                    "properties": {
                        "_id": {
                          "type": "string",
                        }
                    },
                    "required": ["_id"],
                    "additionalProperties": False
                },
                "SessionStatisticsPlain": {
                    "type": "object",
                    "properties": {
                        "BulletsFired": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "DamageDone": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PelletsDestroyedSmall": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PelletsDestroyedMedium": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PelletsDestroyedLarge": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "Score": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PowerUpBulletDamageCollected": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PowerUpBulletSpeedCollected": {
                            "type": "integer",
                            "minimum": 0
                        },
                        "PowerUpPlayerSpeedCollected": {
                            "type": "integer",
                            "minimum": 0
                        }
                    },
                    "required": [
                        "BulletsFired",
                        "DamageDone",
                        "PelletsDestroyedSmall",
                        "PelletsDestroyedMedium",
                        "PelletsDestroyedLarge",
                        "Score",
                        "PowerUpBulletDamageCollected",
                        "PowerUpBulletSpeedCollected",
                        "PowerUpPlayerSpeedCollected"
                    ],
                    "additionalProperties": False
                },
                "Snapshots": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "PlayerPosition": {
                                "type": "object",
                                "properties": {
                                    "X": {
                                        "type": "number"
                                    },
                                    "Y": {
                                        "type": "number"
                                    },
                                    "Z": {
                                        "type": "number"
                                    }
                                },
                                "required": ["X", "Y", "Z"],
                                "additionalProperties": False
                            }
                        },
                        "required": ["PlayerPosition"],
                        "additionalProperties": False
                    },
                    "required": ["items"],
                    "additionalProperties": False
                }
            }
        }
    }

    try:

        # TO-DO: Adding this property should fail validation, but it doesn't
        recording["TEST"] = "BOO!"
        jsonschema.validate(recording, recording_schema)
        del recording["TEST"]
        return

    except jsonschema.exceptions.ValidationError as e:
        raise Exception({"message": "Invalid payload schema.", "error": str(e)})
    except Exception as e:
        raise Exception({"message": "An error occurred.", "error": str(e)})


if __name__ == '__main__':
    app.run()
