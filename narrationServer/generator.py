

def generateSentence2(words):
    text = 'You sen me this list of words : '
    for word in words:
        text += '"' + word + '" '

    text += 'but, for now I cannot say anything about it :/'

    return text


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