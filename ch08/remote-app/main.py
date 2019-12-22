from flask import Flask
import json

app = Flask(__name__)

@app.route("/")
def home():
    return "Sample remote Python App"

hobbies = [
    {"id": 1, "name": "swimming"},
    {"id": 2, "name": "diving"},
    {"id": 3, "name": "jogging"},
    {"id": 4, "name": "hiking"},
    {"id": 5, "name": "cooking"}
]

@app.route("/hobbies")
def get_hobbies():
    return json.dumps(hobbies)
