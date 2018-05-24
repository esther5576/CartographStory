from __future__ import print_function
import sys
import threading
import Queue
from time import sleep

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
        self.running = True

    def run(self):
        while self.running:
            task = self.queue.get()
            task.run()
            task.setDone()
        
class DetectionTask(GPUTask):
    def __init__(self, words):
        GPUTask.__init__(self)
        self.result = ''
        self.words = words

    def run(self):
        self.result = generateSentence(self.words)

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