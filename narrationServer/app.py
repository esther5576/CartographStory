from __future__ import print_function
from flask import Flask, request, jsonify, send_from_directory
import sys
from generator import *
import signal

app = Flask(__name__, static_url_path='', static_folder='www')

gpuThread = None
model = None

@app.route('/storyFromWords', methods=['POST'])
def receiveWords():
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
        return jsonify(status='KO', error=str(e))
        
    return jsonify(status='KO', error="Unexpected error.")

@app.route('/storyFromSentences', methods=['POST'])
def receiveSentences():
    try:
        if request.is_json:
            print("\treceived : " + request.get_data(), file=sys.stderr)
            task = StoryFromSentencesTask(model, request.json['sentences'])
            gpuThread.queue.put(task)
            task.waitUntilDone()
            if task.error is not None:
                return jsonify(status='KO', error=task.error)
            return jsonify(status="OK", generatedText=task.result)
        else:
            return jsonify(status="KO", error="Invalid request.")
    except Exception as e:
        return jsonify(status='KO', error=str(e))
        
    return jsonify(status='KO', error="Unexpected error.")

@app.route('/storyFromImage', methods=['POST'])
def receiveImage():
    try:
        image = request.files.get('file')
        # see http://werkzeug.pocoo.org/docs/0.14/datastructures/#werkzeug.datastructures.FileStorage
        if image is not None:
            if model is not None:
                task = GenerateStoryTask(model, image)
                gpuThread.queue.put(task)
                task.waitUntilDone()
                if task.error is not None:
                    return jsonify(status='KO', error=task.error)
                return jsonify(status="OK", generatedText=task.result)
    except Exception as e:
        return jsonify(status='KO', error=str(e))
        
    return jsonify(status='KO', error="Unexpected error.")

@app.route('/www/<path:path>')
def serveStaticFiles(path):
    return send_from_directory('www', path)
    
@app.route('/')
def root():
    return app.send_static_file('index.html')

def stopServer(signum, frame):
    gpuThread.queue.put(StopGPUThreadTask())
    gpuThread.join()

    raise RuntimeError("Server going down")

if __name__ == '__main__':
    gpuThread = GPUThread()
    gpuThread.start()

    signal.signal(signal.SIGINT, stopServer)
    signal.signal(signal.SIGTERM, stopServer)

    loadTask = LoadModelTask()
    gpuThread.queue.put(loadTask)
    loadTask.waitUntilDone()
    model = loadTask.result
    print('Models loded !', file=sys.stderr)

    app.run(host="0.0.0.0", port=8080, debug=False)
