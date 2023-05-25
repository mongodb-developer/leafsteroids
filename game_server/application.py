import json
import logging
import os
import sys
from typing import Optional, Any

import requests
from flask import Flask, jsonify, request

from constants import ENDPOINT_GET_EVENTS, ENDPOINT_GET_CONFIG, ENDPOINT_GET_PLAYERS, ENDPOINT_POST_SNAPSHOT

app = Flask(__name__)
app.logger.addHandler(logging.StreamHandler(sys.stdout))
logging.basicConfig(level=logging.DEBUG)
app.logger.setLevel(logging.DEBUG)


@app.route('/events', methods=['GET'])
def get_events():
    return send_request('events', ENDPOINT_GET_EVENTS)


@app.route('/config', methods=['GET'])
def get_config():
    return send_request('config', ENDPOINT_GET_CONFIG)


@app.route('/players', methods=['GET'])
def get_players():
    return send_request('players', ENDPOINT_GET_PLAYERS)


@app.route('/recording', methods=['POST'])
def post_recording():
    return send_request('recording', ENDPOINT_POST_SNAPSHOT, request.get_json())


def send_request(route: str, endpoint: str, json_object: Optional[Any] = None):
    try:
        app.logger.debug(f"send_request called:")
        app.logger.debug(f"{route}")
        app.logger.debug(f"{endpoint}")
        app.logger.debug(f"{json_object}")
        headers = {"apiKey": os.environ["API_KEY"]}

        if json_object is not None:
            result = requests.post(endpoint, headers=headers, json=json_object)
            app.logger.debug(f"POST request to {endpoint} resulted in: {result.status_code}")
            if result.status_code == 200:
                return result.json()
            else:
                return result.status_code

        else:
            result = requests.get(endpoint, headers=headers)
            json_string = result.content.decode()
            json_object = json.loads(json_string)
            if not result or not result.content:
                return jsonify({'error': f'No {route} found.'}), 404
            else:
                return jsonify(json_object)

    except Exception as exception:
        return jsonify({'error': f'Internal Server Error:\n{exception=}'}), 500


if __name__ == '__main__':
    app.run()
