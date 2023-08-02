from jsonschema import validate

schema = {
    "type": "object",
    "properties": {
        "price": {"type": "number"},
        "name": {"type": "string"},
    },
}


# def test_schema():
#     validate(instance={"name": "Eggs", "price": 34.99}, schema=schema)
#     validate(
#         instance={"name": "Eggs", "price": "Invalid"},
#         schema=schema,
#     )


def test_json_schema():
    recording_schema = {
        "$jsonSchema": {
            "bsonType": "object",
            "required": [
                "location",
                "DateTime",
                "Player",
                "Event",
                "SessionStatisticsPlain",
                "Snapshots",
            ],
            "additionalProperties": False,
            "properties": {
                "location": {"bsonType": "string"},
                "DateTime": {"bsonType": "date"},
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
                        "Nickname": {"type": "string"}
                    },
                    "required": ["_id", "Nickname"],  # TO-DO: add _id
                    "additionalProperties": True,  # TO-DO: Change to false whe setting _id
                },
                "Event": {
                    "type": "object",
                    "properties": {
                        "_id": {
                            "type": "string",
                        }
                    },
                    "required": ["_id"],
                    "additionalProperties": False,
                },
                "SessionStatisticsPlain": {
                    "type": "object",
                    "properties": {
                        "BulletsFired": {"type": "integer", "minimum": 0},
                        "DamageDone": {"type": "integer", "minimum": 0},
                        "PelletsDestroyedSmall": {"type": "integer", "minimum": 0},
                        "PelletsDestroyedMedium": {"type": "integer", "minimum": 0},
                        "PelletsDestroyedLarge": {"type": "integer", "minimum": 0},
                        "Score": {"type": "integer", "minimum": 0},
                        "PowerUpBulletDamageCollected": {
                            "type": "integer",
                            "minimum": 0,
                        },
                        "PowerUpBulletSpeedCollected": {
                            "type": "integer",
                            "minimum": 0,
                        },
                        "PowerUpPlayerSpeedCollected": {
                            "type": "integer",
                            "minimum": 0,
                        },
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
                        "PowerUpPlayerSpeedCollected",
                    ],
                    "additionalProperties": False,
                },
                "Snapshots": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "PlayerPosition": {
                                "type": "object",
                                "properties": {
                                    "X": {"type": "number"},
                                    "Y": {"type": "number"},
                                    "Z": {"type": "number"},
                                },
                                "required": ["X", "Y", "Z"],
                                "additionalProperties": False,
                            }
                        },
                        "required": ["PlayerPosition"],
                        "additionalProperties": False,
                    },
                    "required": ["items"],
                    "additionalProperties": False,
                },
            },
        }
    }

    recording = {}

    try:
        validate(recording, recording_schema)

    except Exception as exception:
        print(exception)
