from typing import Dict, List

from application import app


def test_get_events():
    response = app.test_client().get("/events")
    assert response.status_code == 200
    json = response.json
    assert json
    assert len(json) == 34
    assert json[0]["_id"] == "gdc-2023-sf"


def test_get_config():
    response = app.test_client().get("/config")
    assert response.status_code == 200
    json = response.json
    assert json
    assert json["_id"] == "640ce9f8e98101ac554aa9ef"


def test_get_players():
    response = app.test_client().get("/players")
    assert response.status_code == 200
    json = response.json
    assert json
    assert len(json) > 10
    player: List[Dict] = list(filter(lambda p: p["Nickname"] == "Dominic", json))
    assert player[0]["_id"] == "641c9bbe5b6ba7abf1ce9fac"


def test_post_recording():
    recording = {
        "SessionStatisticsPlain": {
            "BulletsFired": 42,
            "DamageDone": 42,
            "PelletsDestroyedSmall": 42,
            " PelletsDestroyedMedium": 42,
            " PelletsDestroyedLarge": 42,
            " Score": 42,
            " PowerUpBulletDamageCollected": 42,
            " PowerUpBulletSpeedCollected": 42,
            " PowerUpPlayerSpeedCollected": 42,
        },
        "location":"",
        "DateTime": "2023-05-24 10:30:00 +05:00",
        "RegisteredPlayer": {
            "_id": "640ce9f8e98101ac554aa9ef",
            "Nickname": "Dominic"
        },
        "Snapshots": [
            {
                "Position": {
                    "x": 1.0,
                    "y": 1.0,
                    "z": 1.0,
                }
            }
        ]
    }
    response = app.test_client().post("/recording", json=recording)
    assert response.status_code == 200
    assert "insertedId" in response.json
