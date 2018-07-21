import sys  

reload(sys)  
sys.setdefaultencoding('utf8')

import ml.skipthoughts
import ml.config
import numpy

with open("sentences_vernes.txt") as f:
    sentences = f.read().splitlines()

s = []

for i in xrange(0, len(sentences)-5, 5):
    tmp = sentences[i] + " " + sentences[i+1] + " "  + sentences[i+2] + " " + sentences[i+3] + " " + sentences[i+4]
    s.append(tmp)

model = ml.skipthoughts.load_model(ml.config.paths['skmodels'], ml.config.paths['sktables'])

vecs = ml.skipthoughts.encode(model, s)
#vecs2 = ml.skipthoughts.encode(model, s2)

#vecs = numpy.concatenate((vecs1, vecs2), axis=0)

mean = vecs.mean(0)

numpy.save('ml/data/storyteller/vernes_english_style_3.npy', mean)

