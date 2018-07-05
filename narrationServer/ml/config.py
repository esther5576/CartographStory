"""
Configuration for the generate module
"""

#-----------------------------------------------------------------------------#
# Flags for running on CPU
#-----------------------------------------------------------------------------#
FLAG_CPU_MODE = False

#-----------------------------------------------------------------------------#
# Paths to models and biases
#-----------------------------------------------------------------------------#
paths = dict()

# Skip-thoughts
paths['skmodels'] = 'ml/data/models/'
paths['sktables'] = 'ml/data/models/'

# Decoder
#paths['decmodel'] = 'ml/data/storyteller/toy.npz'
paths['decmodel'] = 'ml/data/storyteller/romance.npz'

#paths['dictionary'] = 'ml/data/models/dict_french.pkl'
paths['dictionary'] = 'ml/data/storyteller/romance_dictionary.pkl'


# Image-sentence embedding
paths['vsemodel'] = 'ml/data/storyteller/coco_embedding.npz'

# VGG-19 convnet
paths['vgg'] = 'ml/data/vgg19.pkl'
#paths['pycaffe'] = '/u/yukun/Projects/caffe-run/python'
paths['vgg_proto_caffe'] = 'ml/data/VGG_ILSVRC_19_layers_deploy.prototxt'
paths['vgg_model_caffe'] = 'ml/data/VGG_ILSVRC_19_layers.caffemodel'


# COCO training captions
paths['captions'] = 'ml/data/storyteller/coco_train_caps.txt'

# Biases
paths['negbias'] = 'ml/data/storyteller/caption_style.npy'
#paths['posbias'] = 'ml/data/storyteller/vernes_style.npy'
#paths['posbias'] = 'ml/data/storyteller/swift_style.npy'
#paths['posbias'] = 'ml/data/storyteller/style.npy'
paths['posbias'] = 'ml/data/storyteller/vernes_english_style.npy'
#paths['posbias'] = 'ml/data/storyteller/romance_style.npy'
