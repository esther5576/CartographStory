from __future__ import print_function
from flask import Flask, request, jsonify
import sys
import generator

app = Flask(__name__)


@app.route('/', methods=['POST'])
def receiveJSON():
    try:
        if request.is_json:
            print("\treceived : " + request.get_data(), file=sys.stderr)
            sentence = generator.generateSentence(request.json['words'])
            return jsonify(status="OK", generatedText=sentence)
        else:
            return jsonify(status="KO", error="Error parsing submitted JSON.")
    except Exception as e:
        return jsonify(status='KO', error=str(e))
    return jsonify(status='KO', error="Unexpected error.")


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=8080, debug=False)
