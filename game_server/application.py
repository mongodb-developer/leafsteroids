import json
import logging
import os
from typing import Optional, Any

import requests
from dotenv import load_dotenv
from flask import Flask, jsonify, request

from constants import ENDPOINT_GET_EVENTS, ENDPOINT_GET_CONFIG, ENDPOINT_GET_PLAYERS, ENDPOINT_POST_SNAPSHOT

load_dotenv()

app = Flask(__name__)
gunicorn_logger = logging.getLogger('gunicorn.error')
app.logger.handlers = gunicorn_logger.handlers
app.logger.setLevel(gunicorn_logger.level)


@app.route('/', methods=['GET'])
def get_root():
    return "I'm alive!\n"


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
        app.logger.debug(f"send_request called:\n"
                         f"{route=}\n"
                         f"{endpoint=}\n"
                         f"{json_object=}")

        headers = {"apiKey": os.environ["API_KEY"]}

        if json_object is None:
            result = requests.get(endpoint, headers=headers)
            app.logger.debug(f"GET request to {endpoint} resulted in: {result.status_code}")
            json_string = result.content.decode()
            json_object = json.loads(json_string)
            if not result or not result.content:
                app.logger.info(f"Returning status code 404")
                return jsonify({'error': f'No {route=} found (internal status code: {result.status_code}).'}), 404
            else:
                app.logger.info(f"Returning {json_object=}")
                return jsonify(json_object)

        else:
            result = requests.post(endpoint, headers=headers, json=json_object)
            app.logger.debug(f"POST request to {endpoint} resulted in: {result.status_code}")
            if result.status_code == 200:
                app.logger.info(f"Returning: {json_object=}")
                return result.json()
            else:
                app.logger.info(f"Returning status code {result.status_code}")
                return result.status_code

    except Exception as exception:
        app.logger.error(f"{exception=}")
        return jsonify({'error': f'Internal Server Error:\n{exception=}'}), 500


if __name__ == '__main__':
    app.run()
