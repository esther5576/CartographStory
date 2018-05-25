from __future__ import print_function
import sys
import threading
import Queue
from time import sleep
from ml import generate

class GPUTask(object):
    def __init__(self):
        self.lock = threading.Event()

    def waitUntilDone(self):
        self.lock.wait()

    def setDone(self):
    	self.lock.set()

    def run(self):
        pass

class GPUThread(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.queue = Queue.Queue()

    def run(self):
        while True:
            task = self.queue.get()
            if (type(task) == StopGPUThreadTask):
                break
            task.run()
            task.setDone()
        print("GPU thread stopped", file=sys.stderr)

class StopGPUThreadTask(GPUTask):
    def __init__(self):
        GPUTask.__init__(self)
        
class DetectionTask(GPUTask):
    def __init__(self, words):
        GPUTask.__init__(self)
        self.result = ''
        self.words = words

    def run(self):
        self.result = generateSentence(self.words)

class LoadModelTask(GPUTask):
    def __init__(self):
        GPUTask.__init__(self)
        self.result = None
        
    def run(self):
        self.result = generate.load_all()

class GenerateStoryTask(GPUTask):
    def __init__(self, model, image):
        GPUTask.__init__(self)
        self.result = ''
        self.error = None
        self.model = model
        self.image = image

    def run(self):
        try:
            self.result = generate.story(self.model, self.image)
        except Exception:
            self.error = 'An error happend wile processing image'

class StoryFromSentencesTask(GPUTask):
    def __init__(self, model, sentences):
        GPUTask.__init__(self)
        self.result = ''
        self.error = None
        self.model = model
        self.sentences = sentences

    def run(self):
        try:
            self.result = generate.storyFromSentence(self.model, self.sentences)
        except Exception e:
            self.error = 'An error happend wile processing sentences : ' + str(e)

def generateSentence(words):
    text = ''
    if any("snow" in word for word in words):
        text += 'On little island I see some snow. I could\'t believe it but it was really there !\n'

    if any("tree" in word for word in words):
        text += 'On one of the islands I visited there was big green trees I\'ve never seen before.\n'

    if any("cloud" in word for word in words):
        text += 'Pinuts shaped clouds was dotting the shy...\n' 

    if any("pillar" in word for word in words):
        text += 'A pillar appeared suddently on a little island.\n'

    if any("rock" in word for word in words):
        text += 'I saw a big rock on one of the islands.\n'

    if text:
        text = "Today, exploring the new world, I see amazing thing\'s...\n" + text
    else:
        text = "I didn't see anything amazing today..."

    return text
