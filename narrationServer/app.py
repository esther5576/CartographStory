from __future__ import print_function
from flask import Flask, request, jsonify
import sys
from generator import generateSentence, GPUThread, DetectionTask

app = Flask(__name__)

gpuThread = None

@app.route('/', methods=['POST'])
def receiveJSON():
    try:
        if request.is_json:
            print("\treceived : " + request.get_data(), file=sys.stderr)
            task = DetectionTask(request.json['words'])
            gpuThread.queue.put(task)
            task.waitUntilDone()
            return jsonify(status="OK", generatedText=task.result)
        else:
            return jsonify(status="KO", error="Invalid request.")
    except Exception as e:
        return jsonify(status='KO', error="test : " + str(e))
        
    return jsonify(status='KO', error="Unexpected error.")


if __name__ == '__main__':
    gpuThread = GPUThread()
    gpuThread.start()
    app.run(host="0.0.0.0", port=8080, debug=False)
